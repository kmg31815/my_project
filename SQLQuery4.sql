select * from BOOK_LEND_RECORD;
select * from BOOK_CLASS;
select * from BOOK_DATA;

select 
	book.Seq ,
	book.BookClass , 
	book.BookId ,
	book.BookName ,
	book.Cnt
from
	(
	select 
		ROW_NUMBER() OVER(partition by bc.BOOK_CLASS_NAME order by COUNT(blr.BOOK_ID) desc) AS Seq ,
		bc.BOOK_CLASS_NAME BookClass ,
		blr.BOOK_ID BookId , 
		bd.BOOK_NAME BookName ,
		COUNT(blr.BOOK_ID) Cnt
	from
		BOOK_LEND_RECORD blr join BOOK_DATA bd on blr.BOOK_ID = bd.BOOK_ID join BOOK_CLASS bc on bd.BOOK_CLASS_ID = bc.BOOK_CLASS_ID
	group by 
		bc.BOOK_CLASS_NAME , blr.BOOK_ID , bd.BOOK_NAME
	) book
where 
	book.Seq <=3
order by book.Seq;
