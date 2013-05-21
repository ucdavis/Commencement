CREATE FUNCTION [dbo].[udf_CalculateEndTerm]
(
	@initial int
)
RETURNS INT
AS
BEGIN
	
	declare @initialstr varchar(6) = convert(varchar(6), @initial), @year int, @end varchar(6)
	
	set @year = convert(int,substring(@initialstr, 0, 5)) + 1
	set @end = convert(varchar(4), @year) + '01'

	RETURN @end

END
