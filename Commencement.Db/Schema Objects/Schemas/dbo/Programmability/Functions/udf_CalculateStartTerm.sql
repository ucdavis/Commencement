CREATE FUNCTION [dbo].[udf_CalculateStartTerm]
(
	@initial int
)
RETURNS INT
AS
BEGIN
	
	declare @initialstr varchar(6) = convert(varchar(6), @initial), @start varchar(6)

	set @start = substring(@initialstr, 0, 6) + '3'

	return convert(int, @start)

END
