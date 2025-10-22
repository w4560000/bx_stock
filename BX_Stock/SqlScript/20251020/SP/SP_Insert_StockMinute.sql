USE [Stock]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_Insert_StockMinute]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_Insert_StockMinute]
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
2025/10/20 Leo Created.

=================================================================
Step

==================================================================
Result

==================================================================
Example

DECLARE @P1 UDT_StockMinute;

INSERT INTO @P1 (StockNo, Date, TradeVolume, TradeValue, OpeningPrice, HighestPrice, LowestPrice,
				 ClosingPrice, Change, Transaction, CreateDate)
VALUES 
    (1001, '2025-09-08 09:01:00', 1000, 10, 20, 20, 20, 20, 0, 10, GETDATE())

EXEC Stock.dbo.SP_Insert_StockMinute
    @UDT_StockMinute = @P1;


*/


CREATE PROCEDURE [dbo].[SP_Insert_StockMinute]
@UDT_StockMinute UDT_StockMinute READONLY
AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO StockMinute
	SELECT StockNo, Date, TradeVolume, TradeValue, OpeningPrice, HighestPrice, LowestPrice, ClosingPrice, [Change], [Transaction], CreateDate
	FROM @UDT_StockMinute

END

GO

GRANT EXECUTE ON [SP_Insert_StockMinute] TO [PUBLIC] AS [dbo] ;
GO