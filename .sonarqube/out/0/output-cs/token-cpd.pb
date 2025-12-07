ó
‡C:\Users\natha\OneDrive\Skrivebord\Kea-it-arkitektur\5. semester\TravelBuddy\src\Shared\TravelBuddy.SharedKernel\SharedKernelService.cs
	namespace 	
TravelBuddy
 
. 
SharedKernel "
{ 
public 

	interface  
ISharedKernelService )
{ 
Task 
< 
IEnumerable 
< 
SystemEventLog '
>' (
>( )#
GetSystemEventLogsAsync* A
(A B
)B C
;C D
}		 
public 

class 
SharedKernelService $
:% & 
ISharedKernelService' ;
{ 
private 
readonly *
ISharedKernelRepositoryFactory 7*
_sharedKernelRepositoryFactory8 V
;V W
public 
SharedKernelService "
(" #*
ISharedKernelRepositoryFactory# A)
sharedKernelRepositoryFactoryB _
)_ `
{ 	*
_sharedKernelRepositoryFactory *
=+ ,)
sharedKernelRepositoryFactory- J
;J K
} 	
private #
ISharedKernelRepository '
GetRepo( /
(/ 0
)0 1
=>2 4*
_sharedKernelRepositoryFactory5 S
.S T%
GetSharedKernelRepositoryT m
(m n
)n o
;o p
public 
async 
Task 
< 
IEnumerable %
<% &
SystemEventLog& 4
>4 5
>5 6#
GetSystemEventLogsAsync7 N
(N O
)O P
{ 	
return 
await 
GetRepo  
(  !
)! "
." ##
GetSystemEventLogsAsync# :
(: ;
); <
;< =
} 	
} 
} ô
’C:\Users\natha\OneDrive\Skrivebord\Kea-it-arkitektur\5. semester\TravelBuddy\src\Shared\TravelBuddy.SharedKernel\ISharedKernelRepositoryFactory.cs
	namespace 	
TravelBuddy
 
. 
SharedKernel "
{ 
public 

	interface *
ISharedKernelRepositoryFactory 3
{ #
ISharedKernelRepository %
GetSharedKernelRepository  9
(9 :
): ;
;; <
} 
} ý	
‰C:\Users\natha\OneDrive\Skrivebord\Kea-it-arkitektur\5. semester\TravelBuddy\src\Shared\TravelBuddy.SharedKernel\Models\SystemEventLog.cs
	namespace 	
TravelBuddy
 
. 
SharedKernel "
." #
Models# )
;) *
public 
partial 
class 
SystemEventLog #
{ 
public 

int 
EventId 
{ 
get 
; 
set !
;! "
}# $
public

 

string

 
	EventType

 
{

 
get

 !
;

! "
set

# &
;

& '
}

( )
=

* +
null

, 0
!

0 1
;

1 2
public 

int 
? 

AffectedId 
{ 
get  
;  !
set" %
;% &
}' (
public 

DateTime 
? 
TriggeredAt  
{! "
get# &
;& '
set( +
;+ ,
}- .
public 

string 
? 
Details 
{ 
get  
;  !
set" %
;% &
}' (
} ±
‹C:\Users\natha\OneDrive\Skrivebord\Kea-it-arkitektur\5. semester\TravelBuddy\src\Shared\TravelBuddy.SharedKernel\ISharedKernelRepository.cs
	namespace 	
TravelBuddy
 
. 
SharedKernel "
;" #
public 
	interface #
ISharedKernelRepository (
{ 
Task 
< 	
IEnumerable	 
< 
SystemEventLog #
># $
>$ %#
GetSystemEventLogsAsync& =
(= >
)> ?
;? @
} ì
žC:\Users\natha\OneDrive\Skrivebord\Kea-it-arkitektur\5. semester\TravelBuddy\src\Shared\TravelBuddy.SharedKernel\Infrastructure\MySql\SharedKernelDbContext.cs
	namespace 	
TravelBuddy
 
. 
SharedKernel "
." #
Infrastructure# 1
;1 2
public 
partial 
class !
SharedKernelDbContext *
:+ ,
	DbContext- 6
{		 
public

 
!
SharedKernelDbContext

  
(

  !
DbContextOptions

! 1
<

1 2!
SharedKernelDbContext

2 G
>

G H
options

I P
)

P Q
: 	
base
 
( 
options 
) 
{ 
} 
public 

virtual 
DbSet 
< 
SystemEventLog '
>' (
SystemEventLogs) 8
{9 :
get; >
;> ?
set@ C
;C D
}E F
	protected 
override 
void 
OnModelCreating +
(+ ,
ModelBuilder, 8
modelBuilder9 E
)E F
{ 
modelBuilder 
. 
UseCollation 
( 
$str .
). /
. 

HasCharSet 
( 
$str !
)! "
;" #
modelBuilder 
. 
Entity 
< 
SystemEventLog *
>* +
(+ ,
entity, 2
=>3 5
{ 	
entity 
. 
HasKey 
( 
e 
=> 
e  
.  !
EventId! (
)( )
.) *
HasName* 1
(1 2
$str2 ;
); <
;< =
entity 
. 
ToTable 
( 
$str -
)- .
;. /
entity 
. 
Property 
( 
e 
=>  
e! "
." #
EventId# *
)* +
.+ ,
HasColumnName, 9
(9 :
$str: D
)D E
;E F
entity 
. 
Property 
( 
e 
=>  
e! "
." #

AffectedId# -
)- .
.. /
HasColumnName/ <
(< =
$str= J
)J K
;K L
entity 
. 
Property 
( 
e 
=>  
e! "
." #
Details# *
)* +
.   
HasMaxLength   
(   
$num   !
)  ! "
.!! 
HasColumnName!! 
(!! 
$str!! (
)!!( )
;!!) *
entity"" 
."" 
Property"" 
("" 
e"" 
=>""  
e""! "
.""" #
	EventType""# ,
)"", -
.## 
HasMaxLength## 
(## 
$num## !
)##! "
.$$ 
HasColumnName$$ 
($$ 
$str$$ +
)$$+ ,
;$$, -
entity%% 
.%% 
Property%% 
(%% 
e%% 
=>%%  
e%%! "
.%%" #
TriggeredAt%%# .
)%%. /
.&& 
HasDefaultValueSql&& #
(&&# $
$str&&$ 7
)&&7 8
.'' 
HasColumnType'' 
('' 
$str'' )
)'') *
.(( 
HasColumnName(( 
((( 
$str(( -
)((- .
;((. /
})) 	
)))	 

;))
 "
OnModelCreatingPartial++ 
(++ 
modelBuilder++ +
)+++ ,
;++, -
},, 
partial.. 
void.. "
OnModelCreatingPartial.. '
(..' (
ModelBuilder..( 4
modelBuilder..5 A
)..A B
;..B C
}// ¦

¤C:\Users\natha\OneDrive\Skrivebord\Kea-it-arkitektur\5. semester\TravelBuddy\src\Shared\TravelBuddy.SharedKernel\Infrastructure\MySql\MySqlSharedKernelRepository.cs
	namespace 	
TravelBuddy
 
. 
SharedKernel "
;" #
public 
class '
MySqlSharedKernelRepository (
:) *#
ISharedKernelRepository+ B
{		 
private

 
readonly

 !
SharedKernelDbContext

 *
_context

+ 3
;

3 4
public 
'
MySqlSharedKernelRepository &
(& '!
SharedKernelDbContext' <
context= D
)D E
{ 
_context 
= 
context 
; 
} 
public 

async 
Task 
< 
IEnumerable !
<! "
SystemEventLog" 0
>0 1
>1 2#
GetSystemEventLogsAsync3 J
(J K
)K L
{ 
return 
await 
_context 
. 
SystemEventLogs -
. 
AsNoTracking 
( 
) 
. 
ToListAsync 
( 
) 
; 
} 
} 