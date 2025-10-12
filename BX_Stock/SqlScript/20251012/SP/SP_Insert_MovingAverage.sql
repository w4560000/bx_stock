USE [Stock]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_Insert_MovingAverage]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_Insert_MovingAverage]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
==================================================================
Description


==================================================================
History
2025/10/12 Leo Created.

=================================================================
Step

==================================================================
Result

==================================================================
Example

DECLARE @P1 UDT_MovingAverage;

INSERT INTO @P1 (StockNo, Date, MA5, MA10, MA20, MA30, MA60, MA180, MA365, CreateDate)
VALUES 
    (1001, '2025-09-08', 100, 100, 100, 100, 100, 100, 100, 100, GETDATE())

EXEC Stock.dbo.SP_Insert_MovingAverage
    @UDT_MovingAverage = @P1;


*/


CREATE PROCEDURE [dbo].[SP_Insert_MovingAverage]
@UDT_MovingAverage UDT_MovingAverage READONLY
AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO MovingAverage
	SELECT StockNo, Date, MA5, MA10, MA20, MA30, MA60, MA180, MA365, CreateDate
	FROM @UDT_MovingAverage

END

GO

GRANT EXECUTE ON [SP_Insert_MovingAverage] TO [PUBLIC] AS [dbo] ;
GO