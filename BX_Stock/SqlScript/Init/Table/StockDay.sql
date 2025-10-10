USE [Stock]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- [StockDay]
CREATE TABLE [dbo].[StockDay](
	[StockNo] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[TradeVolume] [bigint] NOT NULL,
	[TradeValue] [bigint] NOT NULL,
	[OpeningPrice] [decimal](9, 2) NOT NULL,
	[HighestPrice] [decimal](9, 2) NOT NULL,
	[LowestPrice] [decimal](9, 2) NOT NULL,
	[ClosingPrice] [decimal](9, 2) NOT NULL,
	[Change] [decimal](9, 2) NOT NULL,
	[Transaction] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_DayStock] PRIMARY KEY CLUSTERED 
(
	[StockNo] ASC,
	[Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO