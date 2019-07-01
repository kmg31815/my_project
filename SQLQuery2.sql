select * from BOOK_LEND_RECORD;
select * from BOOK_DATA;

select top(5) blr.BOOK_ID BookId, bd.BOOK_NAME BookName, COUNT(*) QTY
from BOOK_LEND_RECORD blr join BOOK_DATA bd on blr.BOOK_ID = bd.BOOK_ID
group by blr.BOOK_ID , bd.BOOK_NAME
order by QTY desc;

