USE [Stock]
GO

IF  EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'UDT_StockDay' AND ss.name = N'dbo')
DROP TYPE [dbo].[UDT_StockDay]
GO

USE [Stock]
GO

CREATE TYPE [dbo].[UDT_StockDay] AS TABLE(
    [StockNo]		INT,
	[Date]			DATETIME,
	[TradeVolume]    BIGINT,
	[TradeValue]    BIGINT,
	[OpeningPrice]  DECIMAL(9,2),
	[HighestPrice]  DECIMAL(9,2),
	[LowestPrice]   DECIMAL(9,2),
	[ClosingPrice]  DECIMAL(9,2),
	[Change]		DECIMAL(9,2),
	[Transaction]	INT,
	[CreateDate]	DATETIME
)
GO

GRANT EXECUTE ON TYPE::[dbo].[UDT_StockDay] TO [public] AS [dbo]
GO