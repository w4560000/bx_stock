
-- �妸�B�z���沧�`���
-- ���`��ƪ��}�L���B�̰����B�̧C���B���L�� ������ �e�@�Ѫ����L��
begin tran

-- �إ߼Ȧstable ��@�n�B�z��List<int>
-- �C�B�z���@���b�屼�ۤv 
Create Table #temp1 (StockNo int)
insert into #temp1 (StockNo)
select StockNo from StockDay
where HighestPrice = 0
group by StockNo
order by StockNo

-- �ŧi�j��_�l
declare @i int
set @i = 0
declare @ProcessCount int
select @ProcessCount = Count(*) from StockDay where OpeningPrice = 0

while(@i < @ProcessCount)
begin
	
	-- �ŧi�o���n�B�z���ӪѥN��
	DECLARE @stockNo int
	set @stockNo = (select top 1 StockNO from #temp1 order by StockNo)

	-- ���X�ӭӪѲ��`��ƲĤ@�����N���P�ɶ�
	DECLARE @dt DateTime
	SELECT @dt = Date
	FROM StockDay
	WHERE StockNO = @stockNo and HighestPrice = 0
	order by Date desc

	-- ���X�ӭӪѲ��`��ƤW�@�������L��
	DECLARE @close decimal(9,2)
	SELECT Top 1 @close = ClosingPrice FROM StockDay 
	WHERE Date < @dt AND StockNo = @stockNo
	ORDER BY Date desc

	-- ��s�ӵ����~���
	update StockDay
	set OpeningPrice = @close,
		HighestPrice = @close,
		LowestPrice = @close,
		ClosingPrice = @close
	where StockNO = @stockNo and Date = @dt 

	-- �Y�ӭӪѤw�g�B�z�� �h�qtemptable�̧R��
	if (select Count(*) from StockDay where OpeningPrice = 0 and StockNO = @stockNo) = 0
	   delete #temp1 where StockNo = @stockNo

	set @i = @i+1
end

--rollback

--commit

--drop table #temp1

