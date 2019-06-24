select * from dbo.BOOK_LEND_RECORD;
select * from dbo.BOOK_DATA;

select top(5) blr.BOOK_ID , bd.BOOK_NAME , COUNT(*) QTY
from dbo.BOOK_LEND_RECORD blr join dbo.BOOK_DATA bd on blr.BOOK_ID = bd.BOOK_ID
group by blr.BOOK_ID , bd.BOOK_NAME
order by QTY desc;

