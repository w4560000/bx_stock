USE [Stock]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_InSertUpt_Stock]') AND type IN (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_InSertUpt_Stock]
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

DECLARE @P1 UDT_Stock;

INSERT INTO @P1 (StockNo, StockName, IsListed, CreateDate, IsEnabled, UnabledDate)
VALUES 
    (1002, NULL, NULL, NULL, NULL, NULL)

EXEC Stock.dbo.SP_InSertUpt_Stock
    @UDT_Stock = @P1;


*/


CREATE PROCEDURE [dbo].[SP_InSertUpt_Stock]
@UDT_Stock UDT_Stock READONLY
AS
BEGIN

	SET NOCOUNT ON;

	MERGE Stock a
	USING @UDT_Stock b
	ON a.StockNo = b.StockNo
	WHEN MATCHED THEN
		UPDATE SET 
					a.StockName = ISNULL(b.StockName, a.StockName),
					a.IsListed = ISNULL(b.IsListed, a.IsListed),
					a.CreateDate = ISNULL(b.CreateDate, a.CreateDate),
					a.IsEnabled = ISNULL(b.IsEnabled, a.IsEnabled),
					a.UnabledDate = ISNULL(b.UnabledDate, a.UnabledDate)
	WHEN NOT MATCHED THEN
		INSERT VALUES (b.StockNo, b.StockName, b.IsListed, b.CreateDate, b.IsEnabled, b.UnabledDate);

END

GO

GRANT EXECUTE ON [SP_InSertUpt_Stock] TO [PUBLIC] AS [dbo] ;
GO