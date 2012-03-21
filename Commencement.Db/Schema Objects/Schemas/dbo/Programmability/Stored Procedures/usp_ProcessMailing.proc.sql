CREATE PROCEDURE [dbo].[usp_ProcessMailing]
	@immediate bit
AS
	
	declare @subject varchar(100), @body varchar(max), @emails varchar(max), @queueId int
	declare @automatedEmail varchar(50) = 'automatedemail@caes.ucdavis.edu'
	declare @queue cursor

	set @queue = cursor for
		select emailqueue.id, [subject], [body]
			,case 
				when registrations.email is null then students.email
				else students.email + ';' + registrations.email
				end as emails
		from emailqueue
			inner join Students on students.Id = EmailQueue.Student_Id
			inner join Registrations on registrations.id = EmailQueue.RegistrationId
		where Pending = 1
		  and [immediate] = @immediate
	
	open @queue

	fetch next from @queue into @queueId, @subject, @body, @emails

	while (@@FETCH_STATUS = 0)
	begin
	
		exec msdb.dbo.sp_send_dbmail
			@recipients = @emails,
			@blind_copy_recipients = @automatedEmail,
			@subject = @subject,
			@body = @body,
			@body_format = 'HTML';
		
		update emailqueue
		set errorcode = @@ERROR, SentDateTime = GETDATE(), Pending = 0
		where id = @queueId

		fetch next from @queue into @queueId, @subject, @body, @emails
	end

	close @queue
	deallocate @queue

RETURN 0