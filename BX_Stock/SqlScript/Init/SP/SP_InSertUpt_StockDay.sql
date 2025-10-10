USE [Stock]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_Insert_StockDay]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_Insert_StockDay]
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
2025/10/10 Leo Created.

=================================================================
Step

==================================================================
Result

==================================================================
Example

DECLARE @P1 UDT_StockDay;

INSERT INTO @P1 (StockNo, Date, TradeVolume, TradeValue, OpeningPrice, HighestPrice, LowestPrice,
				 ClosingPrice, Change, Transaction, CreateDate)
VALUES 
    (1001, '2025-09-08', 1000, 10, 20, 20, 20, 20, 0, 10, GETDATE())

EXEC Stock.dbo.SP_Insert_StockDay
    @UDT_StockDay = @P1;


*/


CREATE PROCEDURE [dbo].[SP_Insert_StockDay]
@UDT_StockDay UDT_StockDay READONLY
AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO StockDay
	SELECT StockNo, Date, TradeVolume, TradeValue, OpeningPrice, HighestPrice, LowestPrice, ClosingPrice, [Change], [Transaction], CreateDate
	FROM @UDT_StockDay

END

GO

GRANT EXECUTE ON [SP_Insert_StockDay] TO [PUBLIC] AS [dbo] ;
GO