USE [Stock]
GO

IF  EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'UDT_MovingAverage' AND ss.name = N'dbo')
DROP TYPE [dbo].[UDT_MovingAverage]
GO

USE [Stock]
GO

CREATE TYPE [dbo].[UDT_MovingAverage] AS TABLE(
    [StockNo]		INT,
	[Date]			DATETIME,
	[MA5]			DECIMAL(9,2),
	[MA10]			DECIMAL(9,2),
	[MA20]			DECIMAL(9,2),
	[MA30]			DECIMAL(9,2),
	[MA60]			DECIMAL(9,2),
	[MA180]			DECIMAL(9,2),
	[MA365]			DECIMAL(9,2),
	[CreateDate]	DATETIME
)
GO

GRANT EXECUTE ON TYPE::[dbo].[UDT_MovingAverage] TO [public] AS [dbo]
GO