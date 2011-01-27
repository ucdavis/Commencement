SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Alan Lai
-- Create date: 1/27/2011	
-- Description:	Takes a list of values from a table to
--				return a set csv
-- =============================================
CREATE FUNCTION udf_GetSpecialNeedsCSV
(
	@id int	-- Registration Id
)
RETURNS varchar(max)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @results varchar(max)

	select @results = coalesce(@results + ', ', '') + sn.Name
	from RegistrationSpecialNeeds rpn 
		inner join SpecialNeeds sn on rpn.SpecialNeedId = sn.id
	where rpn.RegistrationId = @id

	-- Return the result of the function
	return @results

END
GO

