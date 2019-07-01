select * from BOOK_LEND_RECORD;

select 
	case (MONTH(LEND_DATE)-1)/3
		when 0 then '2019/01~2019/03'
		when 1 then '2019/01~2019/03'
		when 2 then '2019/01~2019/03'
		when 3 then '2019/01~2019/03' end Quarter ,
	count(BOOK_ID) Cnt
from BOOK_LEND_RECORD
where year(LEND_DATE) = 2019
group by (MONTH(LEND_DATE)-1)/3
order by (MONTH(LEND_DATE)-1)/3;

