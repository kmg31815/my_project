select * from BOOK_LEND_RECORD;
select * from BOOK_CLASS;
select * from BOOK_DATA;
select * from BOOK_CODE;
select * from MEMBER_M;

select
	bd.BOOK_ID �ѥ�ID,
	bd.BOOK_BOUGHT_DATE �ʮѤ��,
	blr.LEND_DATE �ɾ\���,

from
	BOOK_LEND_RECORD blr 
	join BOOK_DATA bd on blr.BOOK_ID = bd.BOOK_ID 
	join BOOK_CLASS bc on bd.BOOK_CLASS_ID = bc.BOOK_CLASS_ID 
	join BOOK_CODE bc2 on bc2.CODE_ID = bd.BOOK_STATUS
