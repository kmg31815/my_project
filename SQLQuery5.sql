select * from BOOK_LEND_RECORD;
select * from BOOK_CLASS;
select * from BOOK_DATA;

	select
		bc.BOOK_CLASS_ID ClassId ,
		bc.BOOK_CLASS_NAME ClassName ,
		count(blr.BOOK_ID) CNT2016
	from
		BOOK_LEND_RECORD blr join BOOK_DATA bd on blr.BOOK_ID = bd.BOOK_ID join BOOK_CLASS bc on bd.BOOK_CLASS_ID = bc.BOOK_CLASS_ID
	where
		year(blr.LEND_DATE) = 2016
	group by 
		bc.BOOK_CLASS_ID ,
		blr.BOOK_ID ,
		bc.BOOK_CLASS_NAME
	;