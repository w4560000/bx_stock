
-- 批次處理價格異常資料
-- 異常資料的開盤價、最高價、最低價、收盤價 替換成 前一天的收盤價
begin tran

-- 建立暫存table 當作要處理的List<int>
-- 每處理完一筆在砍掉自己 
Create Table #temp1 (StockNo int)
insert into #temp1 (StockNo)
select StockNo from StockDay
where HighestPrice = 0
group by StockNo
order by StockNo

-- 宣告迴圈起始
declare @i int
set @i = 0
declare @ProcessCount int
select @ProcessCount = Count(*) from StockDay where OpeningPrice = 0

while(@i < @ProcessCount)
begin
	
	-- 宣告這次要處理的個股代號
	DECLARE @stockNo int
	set @stockNo = (select top 1 StockNO from #temp1 order by StockNo)

	-- 撈出該個股異常資料第一筆的代號與時間
	DECLARE @dt DateTime
	SELECT @dt = Date
	FROM StockDay
	WHERE StockNO = @stockNo and HighestPrice = 0
	order by Date desc

	-- 撈出該個股異常資料上一筆的收盤價
	DECLARE @close decimal(9,2)
	SELECT Top 1 @close = ClosingPrice FROM StockDay 
	WHERE Date < @dt AND StockNo = @stockNo
	ORDER BY Date desc

	-- 更新該筆錯誤資料
	update StockDay
	set OpeningPrice = @close,
		HighestPrice = @close,
		LowestPrice = @close,
		ClosingPrice = @close
	where StockNO = @stockNo and Date = @dt 

	-- 若該個股已經處理完 則從temptable裡刪除
	if (select Count(*) from StockDay where OpeningPrice = 0 and StockNO = @stockNo) = 0
	   delete #temp1 where StockNo = @stockNo

	set @i = @i+1
end

--rollback

--commit

--drop table #temp1

