USE [Stock]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_Query_Stock]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_Query_Stock]
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
2025/10/09 Leo Created.

=================================================================
Step

==================================================================
Result

==================================================================
Example

EXEC Stock.dbo.SP_Query_Stock
    @StockNo = '1001',
	@IsListed = 1,
	@IsEnabled = 1;


*/


CREATE PROCEDURE [dbo].[SP_Query_Stock]
	@StockNo INT,
	@IsListed BIT NULL,
	@IsEnabled BIT NULL
AS
BEGIN

	SELECT * 
	FROM Stock WITH(NOLOCK)
	WHERE (@StockNo = -1 OR StockNo = @StockNo)
	AND (@IsListed IS NULL OR IsListed = @IsListed)
	AND (@IsEnabled IS NULL OR IsEnabled = @IsEnabled)

END

GO


