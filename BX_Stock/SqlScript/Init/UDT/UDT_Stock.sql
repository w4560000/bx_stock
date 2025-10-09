USE [Stock]
GO

IF  EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'UDT_Stock' AND ss.name = N'dbo')
DROP TYPE [dbo].[UDT_Stock]
GO

USE [Stock]
GO

CREATE TYPE [dbo].[UDT_Stock] AS TABLE(
    [StockNo]      INT,
	[StockName]    NCHAR(10),
	[IsListed]     BIT,
	[CreateDate]   DATETIME,
	[IsEnabled]    BIT,
	[UnabledDate]  DATETIME
)
GO

GRANT EXECUTE ON TYPE::[dbo].[UDT_Stock] TO [public] AS [dbo]
GO