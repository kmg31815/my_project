select * from dbo.BOOK_LEND_RECORD;
select * from dbo.MEMBER_M;
select * from dbo.BOOK_CLASS;
select * from dbo.BOOK_CODE;
select * from dbo.BOOK_DATA;

select 
	blr.KEEPER_ID KeeperId,
	mm.USER_CNAME CName,
	mm.USER_ENAME EName,
	year(blr.LEND_DATE) BorrowYear,
	COUNT(*) BorrowCnt
from MEMBER_M mm join BOOK_LEND_RECORD blr on blr.KEEPER_ID = mm.USER_ID
group by blr.KEEPER_ID , mm.USER_CNAME, mm.USER_ENAME, year(blr.LEND_DATE)
order by KeeperId , BorrowYear;

