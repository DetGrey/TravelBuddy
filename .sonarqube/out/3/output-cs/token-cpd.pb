é
ÄC:\Users\natha\OneDrive\Skrivebord\Kea-it-arkitektur\5. semester\TravelBuddy\src\Modules\TravelBuddy.Messaging\Models\Message.cs
	namespace 	
TravelBuddy
 
. 
	Messaging 
.  
Models  &
;& '
public 
partial 
class 
Message 
{ 
public		 

int		 
	MessageId		 
{		 
get		 
;		 
set		  #
;		# $
}		% &
public 

int 
? 
SenderId 
{ 
get 
; 
set  #
;# $
}% &
public 

string 
Content 
{ 
get 
;  
set! $
;$ %
}& '
=( )
null* .
!. /
;/ 0
public 

DateTime 
? 
SentAt 
{ 
get !
;! "
set# &
;& '
}( )
public 

int 
ConversationId 
{ 
get  #
;# $
set% (
;( )
}* +
public 

virtual 
Conversation 
Conversation  ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
=; <
null= A
!A B
;B C
public 

virtual 
User 
? 
Sender 
{  !
get" %
;% &
set' *
;* +
}, -
} Ÿ

êC:\Users\natha\OneDrive\Skrivebord\Kea-it-arkitektur\5. semester\TravelBuddy\src\Modules\TravelBuddy.Messaging\Models\ConversationParticipant.cs
	namespace 	
TravelBuddy
 
. 
	Messaging 
.  
Models  &
;& '
public 
partial 
class #
ConversationParticipant ,
{ 
public		 

int		 
ConversationId		 
{		 
get		  #
;		# $
set		% (
;		( )
}		* +
public 

int 
UserId 
{ 
get 
; 
set  
;  !
}" #
public 

DateTime 
? 
JoinedAt 
{ 
get  #
;# $
set% (
;( )
}* +
public 

virtual 
Conversation 
Conversation  ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
=; <
null= A
!A B
;B C
public 

virtual 
User 
User 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
null- 1
!1 2
;2 3
} Â
çC:\Users\natha\OneDrive\Skrivebord\Kea-it-arkitektur\5. semester\TravelBuddy\src\Modules\TravelBuddy.Messaging\Models\ConversationOverview.cs
	namespace 	
TravelBuddy
 
. 
	Messaging 
.  
Models  &
;& '
public 
class  
ConversationOverview !
{ 
public 

int 
ConversationId 
{ 
get  #
;# $
set% (
;( )
}* +
public 

int 
? 
TripDestinationId !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 

bool 
IsGroup 
{ 
get 
; 
set "
;" #
}$ %
public 

DateTime 
? 
	CreatedAt 
{  
get! $
;$ %
set& )
;) *
}+ ,
public		 

bool		 

IsArchived		 
{		 
get		  
;		  !
set		" %
;		% &
}		' (
public

 

int

 
ParticipantCount

 
{

  !
get

" %
;

% &
set

' *
;

* +
}

, -
public 

string 
? 
LastMessagePreview %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 

DateTime 
? 
LastMessageAt "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 

string 
? 
ConversationName #
{$ %
get& )
;) *
set+ .
;. /
}0 1
} †
äC:\Users\natha\OneDrive\Skrivebord\Kea-it-arkitektur\5. semester\TravelBuddy\src\Modules\TravelBuddy.Messaging\Models\ConversationAudit.cs
	namespace 	
TravelBuddy
 
. 
	Messaging 
.  
Models  &
;& '
public 
partial 
class 
ConversationAudit &
{ 
public		 

int		 
AuditId		 
{		 
get		 
;		 
set		 !
;		! "
}		# $
public 

int 
ConversationId 
{ 
get  #
;# $
set% (
;( )
}* +
public 

int 
? 
AffectedUserId 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 

string 
Action 
{ 
get 
; 
set  #
;# $
}% &
=' (
null) -
!- .
;. /
public 

int 
? 
	ChangedBy 
{ 
get 
;  
set! $
;$ %
}& '
public 

DateTime 
? 
	Timestamp 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 

virtual 
User 
? 
AffectedUser %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
public 

virtual 
Conversation 
Conversation  ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
=; <
null= A
!A B
;B C
public 

virtual 
User 
? 
ChangedByNavigation ,
{- .
get/ 2
;2 3
set4 7
;7 8
}9 :
} «
ÖC:\Users\natha\OneDrive\Skrivebord\Kea-it-arkitektur\5. semester\TravelBuddy\src\Modules\TravelBuddy.Messaging\Models\Conversation.cs
	namespace 	
TravelBuddy
 
. 
	Messaging 
.  
Models  &
;& '
public 
partial 
class 
Conversation !
{ 
public		 

int		 
ConversationId		 
{		 
get		  #
;		# $
set		% (
;		( )
}		* +
public 

int 
? 
TripDestinationId !
{" #
get$ '
;' (
set) ,
;, -
}. /
public 

bool 
IsGroup 
{ 
get 
; 
set "
;" #
}$ %
public 

DateTime 
? 
	CreatedAt 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 

bool 

IsArchived 
{ 
get  
;  !
set" %
;% &
}' (
public 

virtual 
ICollection 
< 
ConversationAudit 0
>0 1
ConversationAudits2 D
{E F
getG J
;J K
setL O
;O P
}Q R
=S T
newU X
ListY ]
<] ^
ConversationAudit^ o
>o p
(p q
)q r
;r s
public 

virtual 
ICollection 
< #
ConversationParticipant 6
>6 7$
ConversationParticipants8 P
{Q R
getS V
;V W
setX [
;[ \
}] ^
=_ `
newa d
Liste i
<i j$
ConversationParticipant	j Å
>
Å Ç
(
Ç É
)
É Ñ
;
Ñ Ö
public 

virtual 
ICollection 
< 
Message &
>& '
Messages( 0
{1 2
get3 6
;6 7
set8 ;
;; <
}= >
=? @
newA D
ListE I
<I J
MessageJ Q
>Q R
(R S
)S T
;T U
public 

virtual 
TripDestination "
?" #
TripDestination$ 3
{4 5
get6 9
;9 :
set; >
;> ?
}@ A
} ”
ÇC:\Users\natha\OneDrive\Skrivebord\Kea-it-arkitektur\5. semester\TravelBuddy\src\Modules\TravelBuddy.Messaging\MessagingService.cs
	namespace 	
TravelBuddy
 
. 
	Messaging 
{ 
public 

	interface 
IMessagingService &
{ 
Task		 
<		 
IEnumerable		 
<		 #
ConversationOverviewDto		 0
>		0 1
>		1 2(
GetConversationsForUserAsync		3 O
(		O P
int		P S
userId		T Z
)		Z [
;		[ \
Task

 
<

 
(

 
bool

 
Success

 
,

 
string

 "
?

" #
ErrorMessage

$ 0
)

0 1
>

1 2#
CreateConversationAsync

3 J
(

J K!
CreateConversationDto

K `!
createConversationDto

a v
)

v w
;

w x
Task 
< !
ConversationDetailDto "
?" #
># $&
GetConversationDetailAsync% ?
(? @
int@ C
userIdD J
,J K
intL O
conversationIdP ^
)^ _
;_ `
Task 
< 
IReadOnlyList 
< 

MessageDto %
>% &
>& '+
GetMessagesForConversationAsync( G
(G H
intH K
userIdL R
,R S
intT W
conversationIdX f
)f g
;g h
Task 
< 

MessageDto 
? 
> 
SendMessageAsync *
(* +
int+ .
userId/ 5
,5 6
int7 :
conversationId; I
,I J
stringK Q
contentR Y
)Y Z
;Z [
Task 
< 
IEnumerable 
<  
ConversationAuditDto -
>- .
>. /&
GetConversationAuditsAsync0 J
(J K
)K L
;L M
} 
public 

class 
MessagingService !
:" #
IMessagingService$ 5
{ 
private 
readonly '
IMessagingRepositoryFactory 4'
_messagingRepositoryFactory5 P
;P Q
public 
MessagingService 
(  '
IMessagingRepositoryFactory  ;&
messagingRepositoryFactory< V
)V W
{ 	'
_messagingRepositoryFactory '
=( )&
messagingRepositoryFactory* D
;D E
} 	
private  
IMessagingRepository $
GetRepo% ,
(, -
)- .
=>/ 1'
_messagingRepositoryFactory2 M
.M N"
GetMessagingRepositoryN d
(d e
)e f
;f g
public 
async 
Task 
< 
IEnumerable %
<% &#
ConversationOverviewDto& =
>= >
>> ?(
GetConversationsForUserAsync@ \
(\ ]
int] `
userIda g
)g h
{   	
var!! 
messagingRepository!! #
=!!$ %
GetRepo!!& -
(!!- .
)!!. /
;!!/ 0
var"" 
conversations"" 
="" 
await""  %
messagingRepository""& 9
.""9 :(
GetConversationsForUserAsync"": V
(""V W
userId""W ]
)""] ^
;""^ _
return$$ 
conversations$$  
.%% 
Select%% 
(%% 
c%% 
=>%% 
new%%  #
ConversationOverviewDto%%! 8
(%%8 9
ConversationId&& "
:&&" #
c&&$ %
.&&% &
ConversationId&&& 4
,&&4 5
TripDestinationId'' %
:''% &
c''' (
.''( )
TripDestinationId'') :
,'': ;
IsGroup(( 
:(( 
c(( 
.(( 
IsGroup(( &
,((& '
	CreatedAt)) 
:)) 
c))  
.))  !
	CreatedAt))! *
,))* +

IsArchived** 
:** 
c**  !
.**! "

IsArchived**" ,
,**, -
ParticipantCount++ $
:++$ %
c++& '
.++' (
ParticipantCount++( 8
,++8 9
LastMessagePreview,, &
:,,& '
c,,( )
.,,) *
LastMessagePreview,,* <
,,,< =
LastMessageAt-- !
:--! "
c--# $
.--$ %
LastMessageAt--% 2
,--2 3
ConversationName.. $
:..$ %
c..& '
...' (
ConversationName..( 8
)// 
)// 
.00 
ToList00 
(00 
)00 
;00 
}11 	
public22 
async22 
Task22 
<22 
(22 
bool22 
Success22  '
,22' (
string22) /
?22/ 0
ErrorMessage221 =
)22= >
>22> ?#
CreateConversationAsync22@ W
(22W X!
CreateConversationDto22X m"
createConversationDto	22n É
)
22É Ñ
{33 	
var44 
messagingRepository44 #
=44$ %
GetRepo44& -
(44- .
)44. /
;44/ 0
return55 
await55 
messagingRepository55 ,
.55, -#
CreateConversationAsync55- D
(55D E!
createConversationDto55E Z
)55Z [
;55[ \
}66 	
public77 
async77 
Task77 
<77 !
ConversationDetailDto77 /
?77/ 0
>770 1&
GetConversationDetailAsync772 L
(77L M
int77M P
userId77Q W
,77W X
int77Y \
conversationId77] k
)77k l
{88 	
var99 
messagingRepository99 #
=99$ %
GetRepo99& -
(99- .
)99. /
;99/ 0
var:: 
conversation:: 
=:: 
await:: $
messagingRepository::% 8
.::8 9+
GetConversationParticipantAsync::9 X
(::X Y
conversationId::Y g
)::g h
;::h i
if<< 
(<< 
conversation<< 
==<< 
null<<  $
)<<$ %
return== 
null== 
;== 
if?? 
(?? 
!?? 
conversation?? 
.?? $
ConversationParticipants?? 6
.??6 7
Any??7 :
(??: ;
cp??; =
=>??> @
cp??A C
.??C D
UserId??D J
==??K M
userId??N T
)??T U
)??U V
return@@ 
null@@ 
;@@ 
varBB 
participantsBB 
=BB 
conversationBB +
.BB+ ,$
ConversationParticipantsBB, D
.CC 
SelectCC 
(CC 
pCC 
=>CC 
newCC  &
ConversationParticipantDtoCC! ;
(CC; <
UserIdDD 
:DD 
pDD 
.DD 
UserIdDD $
,DD$ %
NameEE 
:EE 
pEE 
.EE 
UserEE  
.EE  !
NameEE! %
,EE% &
EmailFF 
:FF 
pFF 
.FF 
UserFF !
.FF! "
EmailFF" '
)GG 
)GG 
.HH 
ToListHH 
(HH 
)HH 
;HH 
returnJJ 
newJJ !
ConversationDetailDtoJJ ,
(JJ, -
IdKK 
:KK 
conversationKK  
.KK  !
ConversationIdKK! /
,KK/ 0
IsGroupLL 
:LL 
conversationLL %
.LL% &
IsGroupLL& -
,LL- .

IsArchivedMM 
:MM 
conversationMM (
.MM( )

IsArchivedMM) 3
,MM3 4
	CreatedAtNN 
:NN 
conversationNN '
.NN' (
	CreatedAtNN( 1
,NN1 2
ParticipantCountOO  
:OO  !
participantsOO" .
.OO. /
CountOO/ 4
,OO4 5
ParticipantPP 
:PP 
participantsPP )
)QQ 
;QQ 
}RR 	
publicTT 
asyncTT 
TaskTT 
<TT 
IReadOnlyListTT '
<TT' (

MessageDtoTT( 2
>TT2 3
>TT3 4+
GetMessagesForConversationAsyncTT5 T
(TTT U
intTTU X
userIdTTY _
,TT_ `
intTTa d
conversationIdTTe s
)TTs t
{UU 	
varVV 
messagingRepositoryVV #
=VV$ %
GetRepoVV& -
(VV- .
)VV. /
;VV/ 0
varXX 
conversationXX 
=XX 
awaitXX $
messagingRepositoryXX% 8
.XX8 9+
GetConversationParticipantAsyncXX9 X
(XXX Y
conversationIdXXY g
)XXg h
;XXh i
ifYY 
(YY 
conversationYY 
==YY 
nullYY  $
)YY$ %
returnZZ 
ArrayZZ 
.ZZ 
EmptyZZ "
<ZZ" #

MessageDtoZZ# -
>ZZ- .
(ZZ. /
)ZZ/ 0
;ZZ0 1
var\\ 
isParticipant\\ 
=\\ 
conversation\\  ,
.\\, -$
ConversationParticipants\\- E
.]] 
Any]] 
(]] 
cp]] 
=>]] 
cp]] 
.]] 
UserId]] $
==]]% '
userId]]( .
)]]. /
;]]/ 0
if__ 
(__ 
!__ 
isParticipant__ 
)__ 
{`` 
returnaa 
Arrayaa 
.aa 
Emptyaa "
<aa" #

MessageDtoaa# -
>aa- .
(aa. /
)aa/ 0
;aa0 1
}bb 
varee 
messagesee 
=ee 
awaitee  
messagingRepositoryee! 4
.ee4 5+
GetMessagesForConversationAsyncee5 T
(eeT U
conversationIdeeU c
)eec d
;eed e
returnhh 
messageshh 
.ii 
Selectii 
(ii 
mii 
=>ii 
newii  

MessageDtoii! +
(ii+ ,
Idjj 
:jj 
mjj 
.jj 
	MessageIdjj #
,jj# $
ConversationIdkk "
:kk" #
mkk$ %
.kk% &
ConversationIdkk& 4
,kk4 5
SenderIdll 
:ll 
mll 
.ll  
SenderIdll  (
,ll( )

SenderNamemm 
:mm 
mmm  !
.mm! "
Sendermm" (
?mm( )
.mm) *
Namemm* .
,mm. /
Contentnn 
:nn 
mnn 
.nn 
Contentnn &
,nn& '
SentAtoo 
:oo 
moo 
.oo 
SentAtoo $
)pp 
)pp 
.qq 
ToListqq 
(qq 
)qq 
;qq 
}ss 	
publicuu 
asyncuu 
Taskuu 
<uu 

MessageDtouu $
?uu$ %
>uu% &
SendMessageAsyncuu' 7
(uu7 8
intuu8 ;
userIduu< B
,uuB C
intuuD G
conversationIduuH V
,uuV W
stringuuX ^
contentuu_ f
)uuf g
{vv 	
varww 
messagingRepositoryww #
=ww$ %
GetRepoww& -
(ww- .
)ww. /
;ww/ 0
varyy 
conversationyy 
=yy 
awaityy $
messagingRepositoryyy% 8
.yy8 9+
GetConversationParticipantAsyncyy9 X
(yyX Y
conversationIdyyY g
)yyg h
;yyh i
ifzz 
(zz 
conversationzz 
==zz 
nullzz  $
)zz$ %
{{{ 
return|| 
null|| 
;|| 
}}} 
var
ÄÄ 
isParticipant
ÄÄ 
=
ÄÄ 
conversation
ÄÄ  ,
.
ÄÄ, -&
ConversationParticipants
ÄÄ- E
.
ÅÅ 
Any
ÅÅ 
(
ÅÅ 
cp
ÅÅ 
=>
ÅÅ 
cp
ÅÅ 
.
ÅÅ 
UserId
ÅÅ $
==
ÅÅ% '
userId
ÅÅ( .
)
ÅÅ. /
;
ÅÅ/ 0
if
ÉÉ 
(
ÉÉ 
!
ÉÉ 
isParticipant
ÉÉ 
)
ÉÉ 
{
ÑÑ 
return
ÖÖ 
null
ÖÖ 
;
ÖÖ 
}
ÜÜ 
var
ââ 
message
ââ 
=
ââ 
new
ââ 
Message
ââ %
{
ää 
ConversationId
ãã 
=
ãã  
conversationId
ãã! /
,
ãã/ 0
SenderId
åå 
=
åå  
userId
åå! '
,
åå' (
Content
çç 
=
çç  
content
çç! (
,
çç( )
SentAt
éé 
=
éé  
DateTime
éé! )
.
éé) *
UtcNow
éé* 0
}
èè 
;
èè 
var
íí 
saved
íí 
=
íí 
await
íí !
messagingRepository
íí 1
.
íí1 2
AddMessageAsync
íí2 A
(
ííA B
message
ííB I
)
ííI J
;
ííJ K
var
ïï 

senderUser
ïï 
=
ïï 
conversation
ïï )
.
ïï) *&
ConversationParticipants
ïï* B
.
ññ 
FirstOrDefault
ññ 
(
ññ  
cp
ññ  "
=>
ññ# %
cp
ññ& (
.
ññ( )
UserId
ññ) /
==
ññ0 2
userId
ññ3 9
)
ññ9 :
?
ññ: ;
.
óó 
User
óó 
;
óó 
var
ôô 

senderName
ôô 
=
ôô 

senderUser
ôô '
?
ôô' (
.
ôô( )
Name
ôô) -
;
ôô- .
return
úú 
new
úú 

MessageDto
úú !
(
úú! "
Id
ùù 
:
ùù 
saved
ùù $
.
ùù$ %
	MessageId
ùù% .
,
ùù. /
ConversationId
ûû 
:
ûû 
saved
ûû  %
.
ûû% &
ConversationId
ûû& 4
,
ûû4 5
SenderId
üü 
:
üü 
saved
üü $
.
üü$ %
SenderId
üü% -
,
üü- .

SenderName
†† 
:
†† 

senderName
†† )
,
††) *
Content
°° 
:
°° 
saved
°° $
.
°°$ %
Content
°°% ,
,
°°, -
SentAt
¢¢ 
:
¢¢ 
saved
¢¢ $
.
¢¢$ %
SentAt
¢¢% +
)
££ 
;
££ 
}
§§ 	
public
ßß 
async
ßß 
Task
ßß 
<
ßß 
IEnumerable
ßß %
<
ßß% &"
ConversationAuditDto
ßß& :
>
ßß: ;
>
ßß; <(
GetConversationAuditsAsync
ßß= W
(
ßßW X
)
ßßX Y
{
®® 	
var
©© !
messagingRepository
©© #
=
©©$ %
GetRepo
©©& -
(
©©- .
)
©©. /
;
©©/ 0
var
™™ 
audits
™™ 
=
™™ 
await
™™ !
messagingRepository
™™ 2
.
™™2 3(
GetConversationAuditsAsync
™™3 M
(
™™M N
)
™™N O
;
™™O P
return
´´ 
audits
´´ 
.
´´ 
Select
´´  
(
´´  !
ca
´´! #
=>
´´$ &
new
´´' *"
ConversationAuditDto
´´+ ?
(
´´? @
ca
¨¨ 
.
¨¨ 
AuditId
¨¨ 
,
¨¨ 
ca
≠≠ 
.
≠≠ 
ConversationId
≠≠ !
,
≠≠! "
ca
ÆÆ 
.
ÆÆ 
AffectedUserId
ÆÆ !
,
ÆÆ! "
ca
ØØ 
.
ØØ 
Action
ØØ 
,
ØØ 
ca
∞∞ 
.
∞∞ 
	ChangedBy
∞∞ 
,
∞∞ 
ca
±± 
.
±± 
	Timestamp
±± 
)
≤≤ 
)
≤≤ 
.
≤≤ 
ToList
≤≤ 
(
≤≤ 
)
≤≤ 
;
≤≤ 
}
≥≥ 	
}
¥¥ 
}µµ ªı
üC:\Users\natha\OneDrive\Skrivebord\Kea-it-arkitektur\5. semester\TravelBuddy\src\Modules\TravelBuddy.Messaging\Infrastructure\Neo4j\Neo4jMessagingRepository.cs
	namespace		 	
TravelBuddy		
 
.		 
	Messaging		 
{

 
public 

sealed 
class $
Neo4jMessagingRepository 0
:1 2 
IMessagingRepository3 G
{ 
private 
readonly 
IDriver  
_driver! (
;( )
public $
Neo4jMessagingRepository '
(' (
IDriver( /
driver0 6
)6 7
{ 	
_driver 
= 
driver 
?? 
throw  %
new& )!
ArgumentNullException* ?
(? @
nameof@ F
(F G
driverG M
)M N
)N O
;O P
} 	
private"" 
static"" 
int"" 
ReadInt"" "
(""" #
INode""# (
node"") -
,""- .
string""/ 5
key""6 9
,""9 :
int""; >
defaultValue""? K
=""L M
$num""N O
)""O P
{## 	
if$$ 
($$ 
!$$ 
node$$ 
.$$ 

Properties$$  
.$$  !
TryGetValue$$! ,
($$, -
key$$- 0
,$$0 1
out$$2 5
var$$6 9
value$$: ?
)$$? @
||$$A C
value$$D I
is$$J L
null$$M Q
)$$Q R
return%% 
defaultValue%% #
;%%# $
return'' 
value'' 
switch'' 
{(( 
int)) 
i)) 
=>)) 
i)) 
,)) 
long** 
l** 
=>** 
(** 
int** 
)** 
l**  
,**  !
string++ 
s++ 
when++ 
int++ !
.++! "
TryParse++" *
(++* +
s+++ ,
,++, -
out++. 1
var++2 5
i++6 7
)++7 8
=>++9 ;
i++< =
,++= >
_,, 
=>,, 
defaultValue,, !
}-- 
;-- 
}.. 	
private00 
static00 
int00 
?00 
ReadNullableInt00 +
(00+ ,
INode00, 1
node002 6
,006 7
string008 >
key00? B
)00B C
{11 	
if22 
(22 
!22 
node22 
.22 

Properties22  
.22  !
TryGetValue22! ,
(22, -
key22- 0
,220 1
out222 5
var226 9
value22: ?
)22? @
||22A C
value22D I
is22J L
null22M Q
)22Q R
return33 
null33 
;33 
return55 
value55 
switch55 
{66 
int77 
i77 
=>77 
i77 
,77 
long88 
l88 
=>88 
(88 
int88 
)88 
l88  
,88  !
string99 
s99 
when99 
int99 !
.99! "
TryParse99" *
(99* +
s99+ ,
,99, -
out99. 1
var992 5
i996 7
)997 8
=>999 ;
i99< =
,99= >
_:: 
=>:: 
(:: 
int:: 
?:: 
):: 
null:: 
};; 
;;; 
}<< 	
private>> 
static>> 
bool>> 
ReadBool>> $
(>>$ %
INode>>% *
node>>+ /
,>>/ 0
string>>1 7
key>>8 ;
,>>; <
bool>>= A
defaultValue>>B N
=>>O P
false>>Q V
)>>V W
{?? 	
if@@ 
(@@ 
!@@ 
node@@ 
.@@ 

Properties@@  
.@@  !
TryGetValue@@! ,
(@@, -
key@@- 0
,@@0 1
out@@2 5
var@@6 9
value@@: ?
)@@? @
||@@A C
value@@D I
is@@J L
null@@M Q
)@@Q R
returnAA 
defaultValueAA #
;AA# $
returnCC 
valueCC 
switchCC 
{DD 
boolEE 
bEE 
=>EE 
bEE 
,EE 
stringFF 
sFF 
whenFF 
boolFF "
.FF" #
TryParseFF# +
(FF+ ,
sFF, -
,FF- .
outFF/ 2
varFF3 6
bFF7 8
)FF8 9
=>FF: <
bFF= >
,FF> ?
_GG 
=>GG 
defaultValueGG !
}HH 
;HH 
}II 	
privateKK 
staticKK 
DateTimeKK 
?KK   
ReadNullableDateTimeKK! 5
(KK5 6
INodeKK6 ;
nodeKK< @
,KK@ A
stringKKB H
keyKKI L
)KKL M
{LL 	
ifMM 
(MM 
!MM 
nodeMM 
.MM 

PropertiesMM  
.MM  !
TryGetValueMM! ,
(MM, -
keyMM- 0
,MM0 1
outMM2 5
varMM6 9
valueMM: ?
)MM? @
||MMA C
valueMMD I
isMMJ L
nullMMM Q
)MMQ R
returnNN 
nullNN 
;NN 
returnQQ 
valueQQ 
switchQQ 
{RR 
DateTimeSS 
dtSS 
=>SS 
dtSS !
,SS! "
Neo4jTT 
.TT 
DriverTT 
.TT 
LocalDateTimeTT *
ldtTT+ .
=>TT/ 1
ldtTT2 5
.TT5 6

ToDateTimeTT6 @
(TT@ A
)TTA B
,TTB C
Neo4jUU 
.UU 
DriverUU 
.UU 
ZonedDateTimeUU *
zdtUU+ .
=>UU/ 1
zdtUU2 5
.UU5 6
UtcDateTimeUU6 A
,UUA B
stringVV 
sVV 
whenVV 
DateTimeVV &
.VV& '
TryParseVV' /
(VV/ 0
sVV0 1
,VV1 2
outVV3 6
varVV7 :
dtVV; =
)VV= >
=>VV? A
dtVVB D
,VVD E
_WW 
=>WW 
(WW 
DateTimeWW 
?WW 
)WW  
nullWW  $
}XX 
;XX 
}YY 	
private[[ 
static[[ 
string[[ 
?[[ 

ReadString[[ )
([[) *
INode[[* /
node[[0 4
,[[4 5
string[[6 <
key[[= @
)[[@ A
{\\ 	
if]] 
(]] 
!]] 
node]] 
.]] 

Properties]]  
.]]  !
TryGetValue]]! ,
(]], -
key]]- 0
,]]0 1
out]]2 5
var]]6 9
value]]: ?
)]]? @
||]]A C
value]]D I
is]]J L
null]]M Q
)]]Q R
return^^ 
null^^ 
;^^ 
return`` 
value`` 
.`` 
ToString`` !
(``! "
)``" #
;``# $
}aa 	
publichh 
asynchh 
Taskhh 
<hh 
IEnumerablehh %
<hh% & 
ConversationOverviewhh& :
>hh: ;
>hh; <(
GetConversationsForUserAsynchh= Y
(hhY Z
inthhZ ]
userIdhh^ d
)hhd e
{ii 	
constjj 
stringjj 
cypherjj 
=jj  !
$strjs" 
;ss 
varuu 
	overviewsuu 
=uu 
newuu 
Listuu  $
<uu$ % 
ConversationOverviewuu% 9
>uu9 :
(uu: ;
)uu; <
;uu< =
awaitww 
usingww 
varww 
sessionww #
=ww$ %
_driverww& -
.ww- .
AsyncSessionww. :
(ww: ;
oww; <
=>ww= ?
oww@ A
.wwA B!
WithDefaultAccessModewwB W
(wwW X

AccessModewwX b
.wwb c
Readwwc g
)wwg h
)wwh i
;wwi j
varxx 
cursorxx 
=xx 
awaitxx 
sessionxx  '
.xx' (
RunAsyncxx( 0
(xx0 1
cypherxx1 7
,xx7 8
newxx9 <
{xx= >
userIdxx? E
}xxF G
)xxG H
;xxH I
varyy 
recordsyy 
=yy 
awaityy 
cursoryy  &
.yy& '
ToListAsyncyy' 2
(yy2 3
)yy3 4
;yy4 5
foreach{{ 
({{ 
var{{ 
record{{ 
in{{  "
records{{# *
){{* +
{|| 
var}} 
cNode}} 
=}}% &
record}}' -
[}}- .
$str}}. 1
]}}1 2
.}}2 3
As}}3 5
<}}5 6
INode}}6 ;
>}}; <
(}}< =
)}}= >
;}}> ?
var~~ 
participantNodes~~ $
=~~% &
record~~' -
[~~- .
$str~~. <
]~~< =
.~~= >
As~~> @
<~~@ A
List~~A E
<~~E F
INode~~F K
>~~K L
>~~L M
(~~M N
)~~N O
;~~O P
var 
lastMsgNode 
=% &
record' -
[- .
$str. ;
]; <
.< =
As= ?
<? @
INode@ E
?E F
>F G
(G H
)H I
;I J
var
ÅÅ 
conversationId
ÅÅ "
=
ÅÅ& '
ReadInt
ÅÅ( /
(
ÅÅ/ 0
cNode
ÅÅ0 5
,
ÅÅ5 6
$str
ÅÅ7 G
)
ÅÅG H
;
ÅÅH I
var
ÇÇ 
tripDestinationId
ÇÇ %
=
ÇÇ& '
ReadNullableInt
ÇÇ( 7
(
ÇÇ7 8
cNode
ÇÇ8 =
,
ÇÇ= >
$str
ÇÇ? R
)
ÇÇR S
;
ÇÇS T
var
ÉÉ 
isGroup
ÉÉ 
=
ÉÉ& '
ReadBool
ÉÉ( 0
(
ÉÉ0 1
cNode
ÉÉ1 6
,
ÉÉ6 7
$str
ÉÉ8 A
)
ÉÉA B
;
ÉÉB C
var
ÑÑ 

isArchived
ÑÑ 
=
ÑÑ& '
ReadBool
ÑÑ( 0
(
ÑÑ0 1
cNode
ÑÑ1 6
,
ÑÑ6 7
$str
ÑÑ8 D
)
ÑÑD E
;
ÑÑE F
var
ÖÖ 
	createdAt
ÖÖ 
=
ÖÖ& '"
ReadNullableDateTime
ÖÖ( <
(
ÖÖ< =
cNode
ÖÖ= B
,
ÖÖB C
$str
ÖÖD O
)
ÖÖO P
;
ÖÖP Q
var
áá 
participantCount
áá $
=
áá% &
participantNodes
áá' 7
?
áá7 8
.
áá8 9
Count
áá9 >
??
áá? A
$num
ááB C
;
ááC D
string
åå 
conversationName
åå '
;
åå' (
if
çç 
(
çç 
isGroup
çç 
)
çç 
{
éé 
conversationName
èè $
=
èè% &
$str
èè' ;
;
èè; <
}
êê 
else
ëë 
{
íí 
var
ìì 
otherUserNode
ìì %
=
ìì& '
participantNodes
ìì( 8
?
ìì8 9
.
îî 
FirstOrDefault
îî '
(
îî' (
p
îî( )
=>
îî* ,
ReadInt
îî- 4
(
îî4 5
p
îî5 6
,
îî6 7
$str
îî8 @
)
îî@ A
!=
îîB D
userId
îîE K
)
îîK L
;
îîL M
var
ññ 
	otherName
ññ !
=
ññ" #
otherUserNode
ññ$ 1
!=
ññ2 4
null
ññ5 9
?
óó 

ReadString
óó $
(
óó$ %
otherUserNode
óó% 2
,
óó2 3
$str
óó4 :
)
óó: ;
:
òò 
null
òò 
;
òò 
conversationName
öö $
=
öö% &
	otherName
öö' 0
??
öö1 3
$str
öö4 D
;
ööD E
}
õõ 
string
ùù 
?
ùù  
lastMessagePreview
ùù ,
=
ùù- .
null
ùù/ 3
;
ùù3 4
DateTime
ûû 
?
ûû 
lastMessageAt
ûû '
=
ûû- .
null
ûû/ 3
;
ûû3 4
if
†† 
(
†† 
lastMsgNode
†† 
!=
††  "
null
††# '
)
††' (
{
°°  
lastMessagePreview
¢¢ &
=
¢¢' (

ReadString
¢¢) 3
(
¢¢3 4
lastMsgNode
¢¢4 ?
,
¢¢? @
$str
¢¢A J
)
¢¢J K
;
¢¢K L
lastMessageAt
££ !
=
££' ("
ReadNullableDateTime
££) =
(
££= >
lastMsgNode
££> I
,
££I J
$str
££K S
)
££S T
;
££T U
}
§§ 
	overviews
¶¶ 
.
¶¶ 
Add
¶¶ 
(
¶¶ 
new
¶¶ !"
ConversationOverview
¶¶" 6
{
ßß 
ConversationId
®® "
=
®®' (
conversationId
®®) 7
,
®®7 8
TripDestinationId
©© %
=
©©' (
tripDestinationId
©©) :
,
©©: ;
IsGroup
™™ 
=
™™' (
isGroup
™™) 0
,
™™0 1
	CreatedAt
´´ 
=
´´' (
	createdAt
´´) 2
,
´´2 3

IsArchived
¨¨ 
=
¨¨' (

isArchived
¨¨) 3
,
¨¨3 4
ParticipantCount
≠≠ $
=
≠≠' (
participantCount
≠≠) 9
,
≠≠9 : 
LastMessagePreview
ÆÆ &
=
ÆÆ' ( 
lastMessagePreview
ÆÆ) ;
,
ÆÆ; <
LastMessageAt
ØØ !
=
ØØ' (
lastMessageAt
ØØ) 6
,
ØØ6 7
ConversationName
∞∞ $
=
∞∞' (
conversationName
∞∞) 9
}
±± 
)
±± 
;
±± 
}
≤≤ 
return
¥¥ 
	overviews
¥¥ 
;
¥¥ 
}
µµ 	
public
∑∑ 
async
∑∑ 
Task
∑∑ 
<
∑∑ 
(
∑∑ 
bool
∑∑ 
Success
∑∑  '
,
∑∑' (
string
∑∑) /
?
∑∑/ 0
ErrorMessage
∑∑1 =
)
∑∑= >
>
∑∑> ?%
CreateConversationAsync
∑∑@ W
(
∑∑W X#
CreateConversationDto
∑∑X m$
createConversationDto∑∑n É
)∑∑É Ñ
{
∏∏ 	
return
ππ 
(
ππ 
false
ππ 
,
ππ 
null
ππ 
)
ππ  
;
ππ  !
}
∫∫ 	
public
ºº 
async
ºº 
Task
ºº 
<
ºº 
Conversation
ºº &
?
ºº& '
>
ºº' (-
GetConversationParticipantAsync
ºº) H
(
ººH I
int
ººI L
conversationId
ººM [
)
ºº[ \
{
ΩΩ 	
const
ææ 
string
ææ 
cypher
ææ 
=
ææ  !
$str
æ√" 
;
√√ 
await
≈≈ 
using
≈≈ 
var
≈≈ 
session
≈≈ #
=
≈≈$ %
_driver
≈≈& -
.
≈≈- .
AsyncSession
≈≈. :
(
≈≈: ;
o
≈≈; <
=>
≈≈= ?
o
≈≈@ A
.
≈≈A B#
WithDefaultAccessMode
≈≈B W
(
≈≈W X

AccessMode
≈≈X b
.
≈≈b c
Read
≈≈c g
)
≈≈g h
)
≈≈h i
;
≈≈i j
var
∆∆ 
cursor
∆∆ 
=
∆∆ 
await
∆∆ 
session
∆∆  '
.
∆∆' (
RunAsync
∆∆( 0
(
∆∆0 1
cypher
∆∆1 7
,
∆∆7 8
new
∆∆9 <
{
∆∆= >
conversationId
∆∆? M
}
∆∆N O
)
∆∆O P
;
∆∆P Q
var
«« 
records
«« 
=
«« 
await
«« 
cursor
««  &
.
««& '
ToListAsync
««' 2
(
««2 3
)
««3 4
;
««4 5
var
»» 
record
»» 
=
»» 
records
»» !
.
»»! "
SingleOrDefault
»»" 1
(
»»1 2
)
»»2 3
;
»»3 4
if
   
(
   
record
   
is
   
null
   
)
   
return
ÀÀ 
null
ÀÀ 
;
ÀÀ 
var
ÕÕ 
cNode
ÕÕ 
=
ÕÕ! "
record
ÕÕ# )
[
ÕÕ) *
$str
ÕÕ* -
]
ÕÕ- .
.
ÕÕ. /
As
ÕÕ/ 1
<
ÕÕ1 2
INode
ÕÕ2 7
>
ÕÕ7 8
(
ÕÕ8 9
)
ÕÕ9 :
;
ÕÕ: ;
var
ŒŒ 
participantNodes
ŒŒ  
=
ŒŒ! "
record
ŒŒ# )
[
ŒŒ) *
$str
ŒŒ* 8
]
ŒŒ8 9
.
ŒŒ9 :
As
ŒŒ: <
<
ŒŒ< =
List
ŒŒ= A
<
ŒŒA B
INode
ŒŒB G
>
ŒŒG H
>
ŒŒH I
(
ŒŒI J
)
ŒŒJ K
;
ŒŒK L
var
–– 
convId
–– 
=
––" #
ReadInt
––$ +
(
––+ ,
cNode
––, 1
,
––1 2
$str
––3 C
)
––C D
;
––D E
var
—— 
tripDestinationId
—— !
=
——" #
ReadNullableInt
——$ 3
(
——3 4
cNode
——4 9
,
——9 :
$str
——; N
)
——N O
;
——O P
var
““ 
isGroup
““ 
=
““" #
ReadBool
““$ ,
(
““, -
cNode
““- 2
,
““2 3
$str
““4 =
)
““= >
;
““> ?
var
”” 

isArchived
”” 
=
””" #
ReadBool
””$ ,
(
””, -
cNode
””- 2
,
””2 3
$str
””4 @
)
””@ A
;
””A B
var
‘‘ 
	createdAt
‘‘ 
=
‘‘" #"
ReadNullableDateTime
‘‘$ 8
(
‘‘8 9
cNode
‘‘9 >
,
‘‘> ?
$str
‘‘@ K
)
‘‘K L
;
‘‘L M
var
÷÷ 
participants
÷÷ 
=
÷÷ 
new
÷÷ "
List
÷÷# '
<
÷÷' (%
ConversationParticipant
÷÷( ?
>
÷÷? @
(
÷÷@ A
)
÷÷A B
;
÷÷B C
foreach
ÿÿ 
(
ÿÿ 
var
ÿÿ 
uNode
ÿÿ 
in
ÿÿ !
participantNodes
ÿÿ" 2
)
ÿÿ2 3
{
ŸŸ 
if
⁄⁄ 
(
⁄⁄ 
uNode
⁄⁄ 
is
⁄⁄ 
null
⁄⁄ !
)
⁄⁄! "
continue
⁄⁄# +
;
⁄⁄+ ,
var
‹‹ 
participantUserId
‹‹ %
=
‹‹& '
ReadInt
‹‹( /
(
‹‹/ 0
uNode
‹‹0 5
,
‹‹5 6
$str
‹‹7 ?
)
‹‹? @
;
‹‹@ A
var
›› 
name
›› 
=
›› 

ReadString
›› &
(
››& '
uNode
››' ,
,
››, -
$str
››. 4
)
››4 5
??
››7 9
string
››: @
.
››@ A
Empty
››A F
;
››F G
var
ﬁﬁ 
email
ﬁﬁ 
=
ﬁﬁ 

ReadString
ﬁﬁ &
(
ﬁﬁ& '
uNode
ﬁﬁ' ,
,
ﬁﬁ, -
$str
ﬁﬁ. 5
)
ﬁﬁ5 6
??
ﬁﬁ7 9
string
ﬁﬁ: @
.
ﬁﬁ@ A
Empty
ﬁﬁA F
;
ﬁﬁF G
var
‡‡ 
user
‡‡ 
=
‡‡ 
new
‡‡ 
User
‡‡ #
{
·· 
UserId
‚‚ 
=
‚‚ 
participantUserId
‚‚ .
,
‚‚. /
Name
„„ 
=
„„ 
name
„„ !
,
„„! "
Email
‰‰ 
=
‰‰ 
email
‰‰ "
}
ÂÂ 
;
ÂÂ 
participants
ÁÁ 
.
ÁÁ 
Add
ÁÁ  
(
ÁÁ  !
new
ÁÁ! $%
ConversationParticipant
ÁÁ% <
{
ËË 
ConversationId
ÈÈ "
=
ÈÈ# $
convId
ÈÈ% +
,
ÈÈ+ ,
UserId
ÍÍ 
=
ÍÍ# $
participantUserId
ÍÍ% 6
,
ÍÍ6 7
JoinedAt
ÎÎ 
=
ÎÎ# $
null
ÎÎ% )
,
ÎÎ) *
User
ÏÏ 
=
ÏÏ# $
user
ÏÏ% )
}
ÌÌ 
)
ÌÌ 
;
ÌÌ 
}
ÓÓ 
var
 
conversation
 
=
 
new
 "
Conversation
# /
{
ÒÒ 
ConversationId
ÚÚ 
=
ÚÚ) *
convId
ÚÚ+ 1
,
ÚÚ1 2
TripDestinationId
ÛÛ !
=
ÛÛ) *
tripDestinationId
ÛÛ+ <
,
ÛÛ< =
IsGroup
ÙÙ 
=
ÙÙ) *
isGroup
ÙÙ+ 2
,
ÙÙ2 3

IsArchived
ıı 
=
ıı) *

isArchived
ıı+ 5
,
ıı5 6
	CreatedAt
ˆˆ 
=
ˆˆ) *
	createdAt
ˆˆ+ 4
,
ˆˆ4 5&
ConversationParticipants
˜˜ (
=
˜˜) *
participants
˜˜+ 7
,
˜˜7 8
Messages
¯¯ 
=
¯¯) *
new
¯¯+ .
List
¯¯/ 3
<
¯¯3 4
Message
¯¯4 ;
>
¯¯; <
(
¯¯< =
)
¯¯= >
}
˘˘ 
;
˘˘ 
return
˚˚ 
conversation
˚˚ 
;
˚˚  
}
¸¸ 	
public
˛˛ 
async
˛˛ 
Task
˛˛ 
<
˛˛ 
IReadOnlyList
˛˛ '
<
˛˛' (
Message
˛˛( /
>
˛˛/ 0
>
˛˛0 1-
GetMessagesForConversationAsync
˛˛2 Q
(
˛˛Q R
int
˛˛R U
conversationId
˛˛V d
)
˛˛d e
{
ˇˇ 	
const
ÄÄ 
string
ÄÄ 
cypher
ÄÄ 
=
ÄÄ  !
$str
ÄÖ" 
;
ÖÖ 
var
áá 
messages
áá 
=
áá 
new
áá 
List
áá #
<
áá# $
Message
áá$ +
>
áá+ ,
(
áá, -
)
áá- .
;
áá. /
await
ââ 
using
ââ 
var
ââ 
session
ââ #
=
ââ$ %
_driver
ââ& -
.
ââ- .
AsyncSession
ââ. :
(
ââ: ;
o
ââ; <
=>
ââ= ?
o
ââ@ A
.
ââA B#
WithDefaultAccessMode
ââB W
(
ââW X

AccessMode
ââX b
.
ââb c
Read
ââc g
)
ââg h
)
ââh i
;
ââi j
var
ää 
cursor
ää 
=
ää 
await
ää 
session
ää  '
.
ää' (
RunAsync
ää( 0
(
ää0 1
cypher
ää1 7
,
ää7 8
new
ää9 <
{
ää= >
conversationId
ää? M
}
ääN O
)
ääO P
;
ääP Q
var
ãã 
records
ãã 
=
ãã 
await
ãã 
cursor
ãã  &
.
ãã& '
ToListAsync
ãã' 2
(
ãã2 3
)
ãã3 4
;
ãã4 5
foreach
çç 
(
çç 
var
çç 
record
çç 
in
çç  "
records
çç# *
)
çç* +
{
éé 
var
èè 
mNode
èè 
=
èè 
record
èè "
[
èè" #
$str
èè# &
]
èè& '
.
èè' (
As
èè( *
<
èè* +
INode
èè+ 0
>
èè0 1
(
èè1 2
)
èè2 3
;
èè3 4
var
êê 
uNode
êê 
=
êê 
record
êê "
[
êê" #
$str
êê# &
]
êê& '
.
êê' (
As
êê( *
<
êê* +
INode
êê+ 0
?
êê0 1
>
êê1 2
(
êê2 3
)
êê3 4
;
êê4 5
var
íí 
	messageId
íí 
=
íí 
ReadInt
íí  '
(
íí' (
mNode
íí( -
,
íí- .
$str
íí/ :
)
íí: ;
;
íí; <
var
ññ 
convId
ññ 
=
ññ 
conversationId
ññ +
;
ññ+ ,
var
ôô 
senderId
ôô 
=
ôô# $
ReadNullableInt
ôô% 4
(
ôô4 5
mNode
ôô5 :
,
ôô: ;
$str
ôô< F
)
ôôF G
;
ôôG H
int
öö 
?
öö 
senderIdFinal
öö "
=
öö# $
senderId
öö% -
;
öö- .
User
õõ 
?
õõ 

senderUser
õõ  
=
õõ# $
null
õõ% )
;
õõ) *
if
ùù 
(
ùù 
uNode
ùù 
!=
ùù 
null
ùù !
)
ùù! "
{
ûû 
var
üü 
uid
üü 
=
üü 
ReadInt
üü  '
(
üü' (
uNode
üü( -
,
üü- .
$str
üü/ 7
)
üü7 8
;
üü8 9
var
†† 
name
†† 
=
†† 

ReadString
††  *
(
††* +
uNode
††+ 0
,
††0 1
$str
††2 8
)
††8 9
??
††; =
string
††> D
.
††D E
Empty
††E J
;
††J K
var
°° 
email
°° 
=
°° 

ReadString
°°  *
(
°°* +
uNode
°°+ 0
,
°°0 1
$str
°°2 9
)
°°9 :
??
°°; =
string
°°> D
.
°°D E
Empty
°°E J
;
°°J K

senderUser
££ 
=
££  
new
££! $
User
££% )
{
§§ 
UserId
•• 
=
••  
uid
••! $
,
••$ %
Name
¶¶ 
=
¶¶  
name
¶¶! %
,
¶¶% &
Email
ßß 
=
ßß  
email
ßß! &
}
®® 
;
®® 
if
™™ 
(
™™ 
senderIdFinal
™™ %
is
™™& (
null
™™) -
)
™™- .
{
´´ 
senderIdFinal
¨¨ %
=
¨¨& '
uid
¨¨( +
;
¨¨+ ,
}
≠≠ 
}
ÆÆ 
var
∞∞ 
content
∞∞ 
=
∞∞ 

ReadString
∞∞ (
(
∞∞( )
mNode
∞∞) .
,
∞∞. /
$str
∞∞0 9
)
∞∞9 :
??
∞∞; =
string
∞∞> D
.
∞∞D E
Empty
∞∞E J
;
∞∞J K
var
±± 
sentAt
±± 
=
±± "
ReadNullableDateTime
±± 2
(
±±2 3
mNode
±±3 8
,
±±8 9
$str
±±: B
)
±±B C
;
±±C D
var
≥≥ 
msg
≥≥ 
=
≥≥ 
new
≥≥ 
Message
≥≥ %
{
¥¥ 
	MessageId
µµ 
=
µµ# $
	messageId
µµ% .
,
µµ. /
ConversationId
∂∂ "
=
∂∂# $
convId
∂∂% +
,
∂∂+ ,
SenderId
∑∑ 
=
∑∑# $
senderIdFinal
∑∑% 2
,
∑∑2 3
Content
∏∏ 
=
∏∏# $
content
∏∏% ,
,
∏∏, -
SentAt
ππ 
=
ππ# $
sentAt
ππ% +
,
ππ+ ,
Sender
∫∫ 
=
∫∫# $

senderUser
∫∫% /
}
ªª 
;
ªª 
messages
ΩΩ 
.
ΩΩ 
Add
ΩΩ 
(
ΩΩ 
msg
ΩΩ  
)
ΩΩ  !
;
ΩΩ! "
}
ææ 
return
¡¡ 
messages
¡¡ 
;
¡¡ 
}
¬¬ 	
public
ƒƒ 
async
ƒƒ 
Task
ƒƒ 
<
ƒƒ 
Message
ƒƒ !
>
ƒƒ! "
AddMessageAsync
ƒƒ# 2
(
ƒƒ2 3
Message
ƒƒ3 :
message
ƒƒ; B
)
ƒƒB C
{
≈≈ 	
if
∆∆ 
(
∆∆ 
message
∆∆ 
is
∆∆ 
null
∆∆ 
)
∆∆  
throw
«« 
new
«« #
ArgumentNullException
«« /
(
««/ 0
nameof
««0 6
(
««6 7
message
««7 >
)
««> ?
)
««? @
;
««@ A
if
…… 
(
…… 
message
…… 
.
…… 
SenderId
……  
is
……! #
null
……$ (
)
……( )
throw
   
new
   '
InvalidOperationException
   3
(
  3 4
$str
  4 r
)
  r s
;
  s t
await
ÃÃ 
using
ÃÃ 
var
ÃÃ 
session
ÃÃ #
=
ÃÃ$ %
_driver
ÃÃ& -
.
ÃÃ- .
AsyncSession
ÃÃ. :
(
ÃÃ: ;
o
ÃÃ; <
=>
ÃÃ= ?
o
ÃÃ@ A
.
ÃÃA B#
WithDefaultAccessMode
ÃÃB W
(
ÃÃW X

AccessMode
ÃÃX b
.
ÃÃb c
Write
ÃÃc h
)
ÃÃh i
)
ÃÃi j
;
ÃÃj k
const
œœ 
string
œœ 
maxIdCypher
œœ $
=
œœ% &
$str
œ“' 
;
““ 
var
‘‘ 
	maxCursor
‘‘ 
=
‘‘ 
await
‘‘ "
session
‘‘# *
.
‘‘* +
RunAsync
‘‘+ 3
(
‘‘3 4
maxIdCypher
‘‘4 ?
)
‘‘? @
;
‘‘@ A
var
’’ 

maxRecords
’’ 
=
’’ 
await
’’ "
	maxCursor
’’# ,
.
’’, -
ToListAsync
’’- 8
(
’’8 9
)
’’9 :
;
’’: ;
var
÷÷ 
	maxRecord
÷÷ 
=
÷÷ 

maxRecords
÷÷ '
.
÷÷' (
Single
÷÷( .
(
÷÷. /
)
÷÷/ 0
;
÷÷0 1
var
◊◊ 
nextId
◊◊ 
=
◊◊ 
	maxRecord
◊◊ &
[
◊◊& '
$str
◊◊' .
]
◊◊. /
.
◊◊/ 0
As
◊◊0 2
<
◊◊2 3
long
◊◊3 7
>
◊◊7 8
(
◊◊8 9
)
◊◊9 :
+
◊◊; <
$num
◊◊= >
;
◊◊> ?
var
ŸŸ 
	messageId
ŸŸ 
=
ŸŸ 
(
ŸŸ 
int
ŸŸ  
)
ŸŸ  !
nextId
ŸŸ! '
;
ŸŸ' (
var
⁄⁄ 
sentAt
⁄⁄ 
=
⁄⁄ 
message
⁄⁄ #
.
⁄⁄# $
SentAt
⁄⁄$ *
??
⁄⁄+ -
DateTime
⁄⁄. 6
.
⁄⁄6 7
UtcNow
⁄⁄7 =
;
⁄⁄= >
const
‹‹ 
string
‹‹ 
createCypher
‹‹ %
=
‹‹& '
$str
‹Ë( 
;
ËË 
var
ÍÍ 

parameters
ÍÍ 
=
ÍÍ 
new
ÍÍ  
{
ÎÎ 
conversationId
ÏÏ 
=
ÏÏ  
message
ÏÏ! (
.
ÏÏ( )
ConversationId
ÏÏ) 7
,
ÏÏ7 8
senderId
ÌÌ 
=
ÌÌ  
message
ÌÌ! (
.
ÌÌ( )
SenderId
ÌÌ) 1
.
ÌÌ1 2
Value
ÌÌ2 7
,
ÌÌ7 8
	messageId
ÓÓ 
,
ÓÓ 
content
ÔÔ 
=
ÔÔ  
message
ÔÔ! (
.
ÔÔ( )
Content
ÔÔ) 0
,
ÔÔ0 1
sentAt
 
}
ÒÒ 
;
ÒÒ 
await
ÛÛ 
session
ÛÛ 
.
ÛÛ 
RunAsync
ÛÛ "
(
ÛÛ" #
createCypher
ÛÛ# /
,
ÛÛ/ 0

parameters
ÛÛ1 ;
)
ÛÛ; <
;
ÛÛ< =
message
ıı 
.
ıı 
	MessageId
ıı 
=
ıı 
	messageId
ıı  )
;
ıı) *
message
ˆˆ 
.
ˆˆ 
SentAt
ˆˆ 
=
ˆˆ 
sentAt
ˆˆ  &
;
ˆˆ& '
return
¯¯ 
message
¯¯ 
;
¯¯ 
}
˘˘ 	
public
¸¸ 
async
¸¸ 
Task
¸¸ 
<
¸¸ 
IEnumerable
¸¸ %
<
¸¸% &
ConversationAudit
¸¸& 7
>
¸¸7 8
>
¸¸8 9(
GetConversationAuditsAsync
¸¸: T
(
¸¸T U
)
¸¸U V
{
˝˝ 	
return
ˇˇ 
await
ˇˇ 
Task
ˇˇ 
.
ˇˇ 

FromResult
ˇˇ (
(
ˇˇ( )

Enumerable
ˇˇ) 3
.
ˇˇ3 4
Empty
ˇˇ4 9
<
ˇˇ9 :
ConversationAudit
ˇˇ: K
>
ˇˇK L
(
ˇˇL M
)
ˇˇM N
)
ˇˇN O
;
ˇˇO P
}
ÄÄ 	
}
ÅÅ 
}ÇÇ Ω9
üC:\Users\natha\OneDrive\Skrivebord\Kea-it-arkitektur\5. semester\TravelBuddy\src\Modules\TravelBuddy.Messaging\Infrastructure\MySql\MySqlMessagingRepository.cs
	namespace 	
TravelBuddy
 
. 
	Messaging 
;  
public 
class $
MySqlMessagingRepository %
:& ' 
IMessagingRepository( <
{ 
private		 
readonly		 
MessagingDbContext		 '
_context		( 0
;		0 1
public 
$
MySqlMessagingRepository #
(# $
MessagingDbContext$ 6
context7 >
)> ?
{ 
_context 
= 
context 
; 
} 
public 

async 
Task 
< 
IEnumerable !
<! " 
ConversationOverview" 6
>6 7
>7 8(
GetConversationsForUserAsync9 U
(U V
intV Y
userIdZ `
)` a
{ 
return 
await 
_context 
. !
ConversationOverviews 3
. 
FromSqlInterpolated  
(  !
$"! #
$str# ?
{? @
userId@ F
}F G
$strG H
"H I
)I J
. 
ToListAsync 
( 
) 
; 
} 
public 

async 
Task 
< 
( 
bool 
Success #
,# $
string% +
?+ ,
ErrorMessage- 9
)9 :
>: ;#
CreateConversationAsync< S
(S T!
CreateConversationDtoT i!
createConversationDtoj 
)	 Ä
{ 
try 
{ 	
if 
( 
! !
createConversationDto &
.& '
IsGroup' .
&&/ 1!
createConversationDto2 G
.G H
OtherUserIdH S
!=T V
nullW [
)[ \
{] ^
await 
_context 
. 
Database '
.' ('
ExecuteSqlInterpolatedAsync( C
(C D
$@"D G
$strG 
{ !
createConversationDto .
.. /
TripDestinationId/ @
}@ A
$strA 
{ !
createConversationDto .
.. /
OwnerId/ 6
}6 7
$str7 
{ !
createConversationDto .
.. /
OtherUserId/ :
}: ;
$str ; 
"   
)   
;   
return!! 
(!! 
true!! 
,!! 
null!! "
)!!" #
;!!# $
}"" 
else"" 
if"" 
("" !
createConversationDto"" ,
."", -
IsGroup""- 4
&&""5 7!
createConversationDto""8 M
.""M N
TripDestinationId""N _
!=""` b
null""c g
)""g h
{## 
await$$ 
_context$$ 
.$$ 
Database$$ '
.$$' ('
ExecuteSqlInterpolatedAsync$$( C
($$C D
$@"$$D G
$str$&G 
{&& !
createConversationDto&& .
.&&. /
TripDestinationId&&/ @
}&&@ A
$str&'A 
{'' !
createConversationDto'' .
.''. /
OwnerId''/ 6
}''6 7
$str'(7 
"(( 
)(( 
;(( 
return)) 
()) 
true)) 
,)) 
null)) "
)))" #
;))# $
}** 
return,, 
(,, 
true,, 
,,, 
null,, 
),, 
;,,  
}-- 	
catch.. 
(.. 
	Exception.. 
ex.. 
).. 
{// 	
return00 
(00 
false00 
,00 
$str00 %
+00& '
ex00( *
.00* +
Message00+ 2
)002 3
;003 4
}11 	
}22 
public44 

async44 
Task44 
<44 
Conversation44 "
?44" #
>44# $+
GetConversationParticipantAsync44% D
(44D E
int44E H
conversationId44I W
)44W X
{55 
return66 
await66 
_context66 
.66 
Conversations66 +
.77 
Include77 
(77 
c77 
=>77 
c77 
.77 $
ConversationParticipants77 4
)774 5
.88 
ThenInclude88 
(88 
cp88 
=>88  "
cp88# %
.88% &
User88& *
)88* +
.99 
FirstOrDefaultAsync99  
(99  !
c99! "
=>99# %
c99& '
.99' (
ConversationId99( 6
==997 9
conversationId99: H
)99H I
;99I J
}:: 
public<< 

async<< 
Task<< 
<<< 
IReadOnlyList<< #
<<<# $
Message<<$ +
><<+ ,
><<, -+
GetMessagesForConversationAsync<<. M
(<<M N
int<<N Q
conversationId<<R `
)<<` a
{== 
return>> 
await>> 
_context>> 
.>> 
Messages>> &
.?? 
Include?? 
(?? 
m?? 
=>?? 
m?? 
.?? 
Sender?? "
)??" #
.@@ 
Where@@ 
(@@ 
m@@ 
=>@@ 
m@@ 
.@@ 
ConversationId@@ (
==@@) +
conversationId@@, :
)@@: ;
.AA 
OrderByAA 
(AA 
mAA 
=>AA 
mAA 
.AA 
SentAtAA "
)AA" #
.BB 
AsNoTrackingBB 
(BB 
)BB 
.CC 
ToListAsyncCC 
(CC 
)CC 
;CC 
}DD 
publicFF 

asyncFF 
TaskFF 
<FF 
MessageFF 
>FF 
AddMessageAsyncFF .
(FF. /
MessageFF/ 6
messageFF7 >
)FF> ?
{GG 
awaitHH 
_contextHH 
.HH 
MessagesHH 
.HH  
AddAsyncHH  (
(HH( )
messageHH) 0
)HH0 1
;HH1 2
awaitII 
_contextII 
.II 
SaveChangesAsyncII '
(II' (
)II( )
;II) *
returnJJ 
messageJJ 
;JJ 
}KK 
publicNN 

asyncNN 
TaskNN 
<NN 
IEnumerableNN !
<NN! "
ConversationAuditNN" 3
>NN3 4
>NN4 5&
GetConversationAuditsAsyncNN6 P
(NNP Q
)NNQ R
{OO 
returnPP 
awaitPP 
_contextPP 
.PP 
ConversationAuditsPP 0
.QQ 
AsNoTrackingQQ 
(QQ 
)QQ 
.RR 
ToListAsyncRR 
(RR 
)RR 
;RR 
}SS 
}TT ≥ï
ôC:\Users\natha\OneDrive\Skrivebord\Kea-it-arkitektur\5. semester\TravelBuddy\src\Modules\TravelBuddy.Messaging\Infrastructure\MySql\MessagingDbContext.cs
	namespace 	
TravelBuddy
 
. 
	Messaging 
.  
Infrastructure  .
;. /
public

 
partial

 
class

 
MessagingDbContext

 '
:

( )
	DbContext

* 3
{ 
public 

MessagingDbContext 
( 
DbContextOptions .
<. /
MessagingDbContext/ A
>A B
optionsC J
)J K
: 	
base
 
( 
options 
) 
{ 
} 
public 

virtual 
DbSet 
< 
Conversation %
>% &
Conversations' 4
{5 6
get7 :
;: ;
set< ?
;? @
}A B
public 

virtual 
DbSet 
< 
ConversationAudit *
>* +
ConversationAudits, >
{? @
getA D
;D E
setF I
;I J
}K L
public 

virtual 
DbSet 
< #
ConversationParticipant 0
>0 1$
ConversationParticipants2 J
{K L
getM P
;P Q
setR U
;U V
}W X
public 

virtual 
DbSet 
< 
Message  
>  !
Messages" *
{+ ,
get- 0
;0 1
set2 5
;5 6
}7 8
public 

virtual 
DbSet 
< 
TripDestination (
>( )
TripDestinations* :
{; <
get= @
;@ A
setB E
;E F
}G H
public 

virtual 
DbSet 
< 
User 
> 
Users $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 

DbSet 
<  
ConversationOverview %
>% &!
ConversationOverviews' <
{= >
get? B
;B C
setD G
;G H
}I J
	protected!! 
override!! 
void!! 
OnModelCreating!! +
(!!+ ,
ModelBuilder!!, 8
modelBuilder!!9 E
)!!E F
{"" 
modelBuilder## 
.$$ 
UseCollation$$ 
($$ 
$str$$ .
)$$. /
.%% 

HasCharSet%% 
(%% 
$str%% !
)%%! "
;%%" #
modelBuilder'' 
.'' 
Entity'' 
<'' 
Conversation'' (
>''( )
('') *
entity''* 0
=>''1 3
{(( 	
entity)) 
.)) 
HasKey)) 
()) 
e)) 
=>)) 
e))  
.))  !
ConversationId))! /
)))/ 0
.))0 1
HasName))1 8
())8 9
$str))9 B
)))B C
;))C D
entity++ 
.++ 
ToTable++ 
(++ 
$str++ )
)++) *
;++* +
entity-- 
.-- 
HasIndex-- 
(-- 
e-- 
=>--  
e--! "
.--" #
TripDestinationId--# 4
,--4 5
$str--6 L
)--L M
;--M N
entity// 
.// 
Property// 
(// 
e// 
=>//  
e//! "
.//" #
ConversationId//# 1
)//1 2
.//2 3
HasColumnName//3 @
(//@ A
$str//A R
)//R S
;//S T
entity00 
.00 
Property00 
(00 
e00 
=>00  
e00! "
.00" #
	CreatedAt00# ,
)00, -
.11 
HasDefaultValueSql11 #
(11# $
$str11$ 7
)117 8
.22 
HasColumnType22 
(22 
$str22 )
)22) *
.33 
HasColumnName33 
(33 
$str33 +
)33+ ,
;33, -
entity44 
.44 
Property44 
(44 
e44 
=>44  
e44! "
.44" #

IsArchived44# -
)44- .
.44. /
HasColumnName44/ <
(44< =
$str44= J
)44J K
;44K L
entity55 
.55 
Property55 
(55 
e55 
=>55  
e55! "
.55" #
IsGroup55# *
)55* +
.55+ ,
HasColumnName55, 9
(559 :
$str55: D
)55D E
;55E F
entity66 
.66 
Property66 
(66 
e66 
=>66  
e66! "
.66" #
TripDestinationId66# 4
)664 5
.665 6
HasColumnName666 C
(66C D
$str66D Y
)66Y Z
;66Z [
entity88 
.88 
HasOne88 
(88 
d88 
=>88 
d88  
.88  !
TripDestination88! 0
)880 1
.881 2
WithMany882 :
(88: ;
)88; <
.99 
HasForeignKey99 
(99 
d99  
=>99! #
d99$ %
.99% &
TripDestinationId99& 7
)997 8
.:: 
OnDelete:: 
(:: 
DeleteBehavior:: (
.::( )
SetNull::) 0
)::0 1
.;; 
HasConstraintName;; "
(;;" #
$str;;# 9
);;9 :
;;;: ;
}<< 	
)<<	 

;<<
 
modelBuilder>> 
.>> 
Entity>> 
<>> 
ConversationAudit>> -
>>>- .
(>>. /
entity>>/ 5
=>>>6 8
{?? 	
entity@@ 
.@@ 
HasKey@@ 
(@@ 
e@@ 
=>@@ 
e@@  
.@@  !
AuditId@@! (
)@@( )
.@@) *
HasName@@* 1
(@@1 2
$str@@2 ;
)@@; <
;@@< =
entityBB 
.BB 
ToTableBB 
(BB 
$strBB /
)BB/ 0
;BB0 1
entityDD 
.DD 
HasIndexDD 
(DD 
eDD 
=>DD  
eDD! "
.DD" #
AffectedUserIdDD# 1
,DD1 2
$strDD3 L
)DDL M
;DDM N
entityFF 
.FF 
HasIndexFF 
(FF 
eFF 
=>FF  
eFF! "
.FF" #
ConversationIdFF# 1
,FF1 2
$strFF3 I
)FFI J
;FFJ K
entityHH 
.HH 
HasIndexHH 
(HH 
eHH 
=>HH  
eHH! "
.HH" #
	ChangedByHH# ,
,HH, -
$strHH. F
)HHF G
;HHG H
entityJJ 
.JJ 
PropertyJJ 
(JJ 
eJJ 
=>JJ  
eJJ! "
.JJ" #
AuditIdJJ# *
)JJ* +
.JJ+ ,
HasColumnNameJJ, 9
(JJ9 :
$strJJ: D
)JJD E
;JJE F
entityKK 
.KK 
PropertyKK 
(KK 
eKK 
=>KK  
eKK! "
.KK" #
ActionKK# )
)KK) *
.LL 
HasColumnTypeLL 
(LL 
$strLL L
)LLL M
.MM 
HasColumnNameMM 
(MM 
$strMM '
)MM' (
;MM( )
entityNN 
.NN 
PropertyNN 
(NN 
eNN 
=>NN  
eNN! "
.NN" #
AffectedUserIdNN# 1
)NN1 2
.NN2 3
HasColumnNameNN3 @
(NN@ A
$strNNA S
)NNS T
;NNT U
entityOO 
.OO 
PropertyOO 
(OO 
eOO 
=>OO  
eOO! "
.OO" #
ConversationIdOO# 1
)OO1 2
.OO2 3
HasColumnNameOO3 @
(OO@ A
$strOOA R
)OOR S
;OOS T
entityPP 
.PP 
PropertyPP 
(PP 
ePP 
=>PP  
ePP! "
.PP" #
	TimestampPP# ,
)PP, -
.QQ 
HasDefaultValueSqlQQ #
(QQ# $
$strQQ$ 7
)QQ7 8
.RR 
HasColumnTypeRR 
(RR 
$strRR )
)RR) *
.SS 
HasColumnNameSS 
(SS 
$strSS *
)SS* +
;SS+ ,
entityTT 
.TT 
PropertyTT 
(TT 
eTT 
=>TT  
eTT! "
.TT" #
	ChangedByTT# ,
)TT, -
.TT- .
HasColumnNameTT. ;
(TT; <
$strTT< H
)TTH I
;TTI J
entityVV 
.VV 
HasOneVV 
(VV 
dVV 
=>VV 
dVV  
.VV  !
AffectedUserVV! -
)VV- .
.VV. /
WithManyVV/ 7
(VV7 8
)VV8 9
.WW 
HasForeignKeyWW 
(WW 
dWW  
=>WW! #
dWW$ %
.WW% &
AffectedUserIdWW& 4
)WW4 5
.XX 
OnDeleteXX 
(XX 
DeleteBehaviorXX (
.XX( )
SetNullXX) 0
)XX0 1
.YY 
HasConstraintNameYY "
(YY" #
$strYY# <
)YY< =
;YY= >
entity\\ 
.\\ 
HasOne\\ 
(\\ 
d\\ 
=>\\ 
d\\  
.\\  !
ChangedByNavigation\\! 4
)\\4 5
.\\5 6
WithMany\\6 >
(\\> ?
)\\? @
.]] 
HasForeignKey]] 
(]] 
d]]  
=>]]! #
d]]$ %
.]]% &
	ChangedBy]]& /
)]]/ 0
.^^ 
OnDelete^^ 
(^^ 
DeleteBehavior^^ (
.^^( )
SetNull^^) 0
)^^0 1
.__ 
HasConstraintName__ "
(__" #
$str__# ;
)__; <
;__< =
}`` 	
)``	 

;``
 
modelBuilderbb 
.bb 
Entitybb 
<bb #
ConversationParticipantbb 3
>bb3 4
(bb4 5
entitybb5 ;
=>bb< >
{cc 	
entitydd 
.dd 
HasKeydd 
(dd 
edd 
=>dd 
newdd "
{dd# $
edd% &
.dd& '
ConversationIddd' 5
,dd5 6
edd7 8
.dd8 9
UserIddd9 ?
}dd@ A
)ddA B
.ee 
HasNameee 
(ee 
$stree "
)ee" #
.ff 
HasAnnotationff 
(ff 
$strff 8
,ff8 9
newff: =
[ff= >
]ff> ?
{ff@ A
$numffB C
,ffC D
$numffE F
}ffG H
)ffH I
;ffI J
entityhh 
.hh 
ToTablehh 
(hh 
$strhh 5
)hh5 6
;hh6 7
entityjj 
.jj 
HasIndexjj 
(jj 
ejj 
=>jj  
ejj! "
.jj" #
UserIdjj# )
,jj) *
$strjj+ 7
)jj7 8
;jj8 9
entityll 
.ll 
Propertyll 
(ll 
ell 
=>ll  
ell! "
.ll" #
ConversationIdll# 1
)ll1 2
.ll2 3
HasColumnNamell3 @
(ll@ A
$strllA R
)llR S
;llS T
entitymm 
.mm 
Propertymm 
(mm 
emm 
=>mm  
emm! "
.mm" #
UserIdmm# )
)mm) *
.mm* +
HasColumnNamemm+ 8
(mm8 9
$strmm9 B
)mmB C
;mmC D
entitynn 
.nn 
Propertynn 
(nn 
enn 
=>nn  
enn! "
.nn" #
JoinedAtnn# +
)nn+ ,
.oo 
HasDefaultValueSqloo #
(oo# $
$stroo$ 7
)oo7 8
.pp 
HasColumnTypepp 
(pp 
$strpp )
)pp) *
.qq 
HasColumnNameqq 
(qq 
$strqq *
)qq* +
;qq+ ,
entityss 
.ss 
HasOness 
(ss 
dss 
=>ss 
dss  
.ss  !
Conversationss! -
)ss- .
.ss. /
WithManyss/ 7
(ss7 8
pss8 9
=>ss: <
pss= >
.ss> ?$
ConversationParticipantsss? W
)ssW X
.tt 
HasForeignKeytt 
(tt 
dtt  
=>tt! #
dtt$ %
.tt% &
ConversationIdtt& 4
)tt4 5
.uu 
HasConstraintNameuu "
(uu" #
$struu# 7
)uu7 8
;uu8 9
entityww 
.ww 
HasOneww 
(ww 
dww 
=>ww 
dww  
.ww  !
Userww! %
)ww% &
.ww& '
WithManyww' /
(ww/ 0
)ww0 1
.xx 
HasForeignKeyxx 
(xx 
dxx  
=>xx! #
dxx$ %
.xx% &
UserIdxx& ,
)xx, -
.yy 
HasConstraintNameyy "
(yy" #
$stryy# /
)yy/ 0
;yy0 1
}zz 	
)zz	 

;zz
 
modelBuilder|| 
.|| 
Entity|| 
<|| 
Destination|| '
>||' (
(||( )
entity||) /
=>||0 2
{}} 	
entity~~ 
.~~ 
HasKey~~ 
(~~ 
e~~ 
=>~~ 
e~~  
.~~  !
DestinationId~~! .
)~~. /
.~~/ 0
HasName~~0 7
(~~7 8
$str~~8 A
)~~A B
;~~B C
entity
ÄÄ 
.
ÄÄ 
ToTable
ÄÄ 
(
ÄÄ 
$str
ÄÄ (
)
ÄÄ( )
;
ÄÄ) *
entity
ÇÇ 
.
ÇÇ 
HasIndex
ÇÇ 
(
ÇÇ 
e
ÇÇ 
=>
ÇÇ  
new
ÇÇ! $
{
ÇÇ% &
e
ÇÇ' (
.
ÇÇ( )
Country
ÇÇ) 0
,
ÇÇ0 1
e
ÇÇ2 3
.
ÇÇ3 4
State
ÇÇ4 9
}
ÇÇ: ;
,
ÇÇ; <
$str
ÇÇ= T
)
ÇÇT U
;
ÇÇU V
entity
ÑÑ 
.
ÑÑ 
Property
ÑÑ 
(
ÑÑ 
e
ÑÑ 
=>
ÑÑ  
e
ÑÑ! "
.
ÑÑ" #
DestinationId
ÑÑ# 0
)
ÑÑ0 1
.
ÑÑ1 2
HasColumnName
ÑÑ2 ?
(
ÑÑ? @
$str
ÑÑ@ P
)
ÑÑP Q
;
ÑÑQ R
entity
ÖÖ 
.
ÖÖ 
Property
ÖÖ 
(
ÖÖ 
e
ÖÖ 
=>
ÖÖ  
e
ÖÖ! "
.
ÖÖ" #
Country
ÖÖ# *
)
ÖÖ* +
.
ÜÜ 
HasMaxLength
ÜÜ 
(
ÜÜ 
$num
ÜÜ !
)
ÜÜ! "
.
áá 
HasColumnName
áá 
(
áá 
$str
áá (
)
áá( )
;
áá) *
entity
àà 
.
àà 
Property
àà 
(
àà 
e
àà 
=>
àà  
e
àà! "
.
àà" #
Latitude
àà# +
)
àà+ ,
.
ââ 
HasPrecision
ââ 
(
ââ 
$num
ââ  
,
ââ  !
$num
ââ" #
)
ââ# $
.
ää 
HasColumnName
ää 
(
ää 
$str
ää )
)
ää) *
;
ää* +
entity
ãã 
.
ãã 
Property
ãã 
(
ãã 
e
ãã 
=>
ãã  
e
ãã! "
.
ãã" #
	Longitude
ãã# ,
)
ãã, -
.
åå 
HasPrecision
åå 
(
åå 
$num
åå  
,
åå  !
$num
åå" #
)
åå# $
.
çç 
HasColumnName
çç 
(
çç 
$str
çç *
)
çç* +
;
çç+ ,
entity
éé 
.
éé 
Property
éé 
(
éé 
e
éé 
=>
éé  
e
éé! "
.
éé" #
Name
éé# '
)
éé' (
.
èè 
HasMaxLength
èè 
(
èè 
$num
èè !
)
èè! "
.
êê 
HasColumnName
êê 
(
êê 
$str
êê %
)
êê% &
;
êê& '
entity
ëë 
.
ëë 
Property
ëë 
(
ëë 
e
ëë 
=>
ëë  
e
ëë! "
.
ëë" #
State
ëë# (
)
ëë( )
.
íí 
HasMaxLength
íí 
(
íí 
$num
íí !
)
íí! "
.
ìì 
HasColumnName
ìì 
(
ìì 
$str
ìì &
)
ìì& '
;
ìì' (
}
îî 	
)
îî	 

;
îî
 
modelBuilder
ññ 
.
ññ 
Entity
ññ 
<
ññ 
Message
ññ #
>
ññ# $
(
ññ$ %
entity
ññ% +
=>
ññ, .
{
óó 	
entity
òò 
.
òò 
HasKey
òò 
(
òò 
e
òò 
=>
òò 
e
òò  
.
òò  !
	MessageId
òò! *
)
òò* +
.
òò+ ,
HasName
òò, 3
(
òò3 4
$str
òò4 =
)
òò= >
;
òò> ?
entity
öö 
.
öö 
ToTable
öö 
(
öö 
$str
öö $
)
öö$ %
;
öö% &
entity
úú 
.
úú 
HasIndex
úú 
(
úú 
e
úú 
=>
úú  
e
úú! "
.
úú" #
ConversationId
úú# 1
,
úú1 2
$str
úú3 L
)
úúL M
;
úúM N
entity
ûû 
.
ûû 
HasIndex
ûû 
(
ûû 
e
ûû 
=>
ûû  
e
ûû! "
.
ûû" #
SenderId
ûû# +
,
ûû+ ,
$str
ûû- @
)
ûû@ A
;
ûûA B
entity
†† 
.
†† 
Property
†† 
(
†† 
e
†† 
=>
††  
e
††! "
.
††" #
	MessageId
††# ,
)
††, -
.
††- .
HasColumnName
††. ;
(
††; <
$str
††< H
)
††H I
;
††I J
entity
°° 
.
°° 
Property
°° 
(
°° 
e
°° 
=>
°°  
e
°°! "
.
°°" #
Content
°°# *
)
°°* +
.
¢¢ 
HasMaxLength
¢¢ 
(
¢¢ 
$num
¢¢ "
)
¢¢" #
.
££ 
HasColumnName
££ 
(
££ 
$str
££ (
)
££( )
;
££) *
entity
§§ 
.
§§ 
Property
§§ 
(
§§ 
e
§§ 
=>
§§  
e
§§! "
.
§§" #
ConversationId
§§# 1
)
§§1 2
.
§§2 3
HasColumnName
§§3 @
(
§§@ A
$str
§§A R
)
§§R S
;
§§S T
entity
•• 
.
•• 
Property
•• 
(
•• 
e
•• 
=>
••  
e
••! "
.
••" #
SenderId
••# +
)
••+ ,
.
••, -
HasColumnName
••- :
(
••: ;
$str
••; F
)
••F G
;
••G H
entity
¶¶ 
.
¶¶ 
Property
¶¶ 
(
¶¶ 
e
¶¶ 
=>
¶¶  
e
¶¶! "
.
¶¶" #
SentAt
¶¶# )
)
¶¶) *
.
ßß  
HasDefaultValueSql
ßß #
(
ßß# $
$str
ßß$ 7
)
ßß7 8
.
®® 
HasColumnType
®® 
(
®® 
$str
®® )
)
®®) *
.
©© 
HasColumnName
©© 
(
©© 
$str
©© (
)
©©( )
;
©©) *
entity
´´ 
.
´´ 
HasOne
´´ 
(
´´ 
d
´´ 
=>
´´ 
d
´´  
.
´´  !
Conversation
´´! -
)
´´- .
.
´´. /
WithMany
´´/ 7
(
´´7 8
p
´´8 9
=>
´´: <
p
´´= >
.
´´> ?
Messages
´´? G
)
´´G H
.
¨¨ 
HasForeignKey
¨¨ 
(
¨¨ 
d
¨¨  
=>
¨¨! #
d
¨¨$ %
.
¨¨% &
ConversationId
¨¨& 4
)
¨¨4 5
.
≠≠ 
HasConstraintName
≠≠ "
(
≠≠" #
$str
≠≠# <
)
≠≠< =
;
≠≠= >
entity
ØØ 
.
ØØ 
HasOne
ØØ 
(
ØØ 
d
ØØ 
=>
ØØ 
d
ØØ  
.
ØØ  !
Sender
ØØ! '
)
ØØ' (
.
ØØ( )
WithMany
ØØ) 1
(
ØØ1 2
)
ØØ2 3
.
∞∞ 
HasForeignKey
∞∞ 
(
∞∞ 
d
∞∞  
=>
∞∞! #
d
∞∞$ %
.
∞∞% &
SenderId
∞∞& .
)
∞∞. /
.
±± 
OnDelete
±± 
(
±± 
DeleteBehavior
±± (
.
±±( )
Cascade
±±) 0
)
±±0 1
.
≤≤ 
HasConstraintName
≤≤ "
(
≤≤" #
$str
≤≤# 6
)
≤≤6 7
;
≤≤7 8
}
≥≥ 	
)
≥≥	 

;
≥≥
 
modelBuilder
µµ 
.
µµ 
Entity
µµ 
<
µµ 
Trip
µµ  
>
µµ  !
(
µµ! "
entity
µµ" (
=>
µµ) +
{
∂∂ 	
entity
∑∑ 
.
∑∑ 
HasKey
∑∑ 
(
∑∑ 
e
∑∑ 
=>
∑∑ 
e
∑∑  
.
∑∑  !
TripId
∑∑! '
)
∑∑' (
.
∑∑( )
HasName
∑∑) 0
(
∑∑0 1
$str
∑∑1 :
)
∑∑: ;
;
∑∑; <
entity
ππ 
.
ππ 
ToTable
ππ 
(
ππ 
$str
ππ !
)
ππ! "
;
ππ" #
entity
ªª 
.
ªª 
HasIndex
ªª 
(
ªª 
e
ªª 
=>
ªª  
e
ªª! "
.
ªª" #
OwnerId
ªª# *
,
ªª* +
$str
ªª, ;
)
ªª; <
;
ªª< =
entity
ΩΩ 
.
ΩΩ 
Property
ΩΩ 
(
ΩΩ 
e
ΩΩ 
=>
ΩΩ  
e
ΩΩ! "
.
ΩΩ" #
TripId
ΩΩ# )
)
ΩΩ) *
.
ΩΩ* +
HasColumnName
ΩΩ+ 8
(
ΩΩ8 9
$str
ΩΩ9 B
)
ΩΩB C
;
ΩΩC D
entity
ææ 
.
ææ 
Property
ææ 
(
ææ 
e
ææ 
=>
ææ  
e
ææ! "
.
ææ" #
Description
ææ# .
)
ææ. /
.
øø 
HasMaxLength
øø 
(
øø 
$num
øø !
)
øø! "
.
¿¿ 
HasColumnName
¿¿ 
(
¿¿ 
$str
¿¿ ,
)
¿¿, -
;
¿¿- .
entity
¡¡ 
.
¡¡ 
Property
¡¡ 
(
¡¡ 
e
¡¡ 
=>
¡¡  
e
¡¡! "
.
¡¡" #
EndDate
¡¡# *
)
¡¡* +
.
¡¡+ ,
HasColumnName
¡¡, 9
(
¡¡9 :
$str
¡¡: D
)
¡¡D E
;
¡¡E F
entity
¬¬ 
.
¬¬ 
Property
¬¬ 
(
¬¬ 
e
¬¬ 
=>
¬¬  
e
¬¬! "
.
¬¬" #

IsArchived
¬¬# -
)
¬¬- .
.
¬¬. /
HasColumnName
¬¬/ <
(
¬¬< =
$str
¬¬= J
)
¬¬J K
;
¬¬K L
entity
√√ 
.
√√ 
Property
√√ 
(
√√ 
e
√√ 
=>
√√  
e
√√! "
.
√√" #

MaxBuddies
√√# -
)
√√- .
.
√√. /
HasColumnName
√√/ <
(
√√< =
$str
√√= J
)
√√J K
;
√√K L
entity
ƒƒ 
.
ƒƒ 
Property
ƒƒ 
(
ƒƒ 
e
ƒƒ 
=>
ƒƒ  
e
ƒƒ! "
.
ƒƒ" #
OwnerId
ƒƒ# *
)
ƒƒ* +
.
ƒƒ+ ,
HasColumnName
ƒƒ, 9
(
ƒƒ9 :
$str
ƒƒ: D
)
ƒƒD E
;
ƒƒE F
entity
≈≈ 
.
≈≈ 
Property
≈≈ 
(
≈≈ 
e
≈≈ 
=>
≈≈  
e
≈≈! "
.
≈≈" #
	StartDate
≈≈# ,
)
≈≈, -
.
≈≈- .
HasColumnName
≈≈. ;
(
≈≈; <
$str
≈≈< H
)
≈≈H I
;
≈≈I J
entity
∆∆ 
.
∆∆ 
Property
∆∆ 
(
∆∆ 
e
∆∆ 
=>
∆∆  
e
∆∆! "
.
∆∆" #
TripName
∆∆# +
)
∆∆+ ,
.
«« 
HasMaxLength
«« 
(
«« 
$num
«« !
)
««! "
.
»» 
HasColumnName
»» 
(
»» 
$str
»» *
)
»»* +
;
»»+ ,
}
   	
)
  	 

;
  
 
modelBuilder
ÃÃ 
.
ÃÃ 
Entity
ÃÃ 
<
ÃÃ 
TripDestination
ÃÃ +
>
ÃÃ+ ,
(
ÃÃ, -
entity
ÃÃ- 3
=>
ÃÃ4 6
{
ÕÕ 	
entity
ŒŒ 
.
ŒŒ 
HasKey
ŒŒ 
(
ŒŒ 
e
ŒŒ 
=>
ŒŒ 
e
ŒŒ  
.
ŒŒ  !
TripDestinationId
ŒŒ! 2
)
ŒŒ2 3
.
ŒŒ3 4
HasName
ŒŒ4 ;
(
ŒŒ; <
$str
ŒŒ< E
)
ŒŒE F
;
ŒŒF G
entity
–– 
.
–– 
ToTable
–– 
(
–– 
$str
–– -
)
––- .
;
––. /
entity
““ 
.
““ 
HasIndex
““ 
(
““ 
e
““ 
=>
““  
new
““! $
{
““% &
e
““' (
.
““( )
	StartDate
““) 2
,
““2 3
e
““4 5
.
““5 6
EndDate
““6 =
}
““> ?
,
““? @
$str
““A O
)
““O P
;
““P Q
entity
‘‘ 
.
‘‘ 
HasIndex
‘‘ 
(
‘‘ 
e
‘‘ 
=>
‘‘  
e
‘‘! "
.
‘‘" #
DestinationId
‘‘# 0
,
‘‘0 1
$str
‘‘2 F
)
‘‘F G
;
‘‘G H
entity
÷÷ 
.
÷÷ 
HasIndex
÷÷ 
(
÷÷ 
e
÷÷ 
=>
÷÷  
e
÷÷! "
.
÷÷" #
TripId
÷÷# )
,
÷÷) *
$str
÷÷+ 8
)
÷÷8 9
;
÷÷9 :
entity
ÿÿ 
.
ÿÿ 
Property
ÿÿ 
(
ÿÿ 
e
ÿÿ 
=>
ÿÿ  
e
ÿÿ! "
.
ÿÿ" #
TripDestinationId
ÿÿ# 4
)
ÿÿ4 5
.
ÿÿ5 6
HasColumnName
ÿÿ6 C
(
ÿÿC D
$str
ÿÿD Y
)
ÿÿY Z
;
ÿÿZ [
entity
ŸŸ 
.
ŸŸ 
Property
ŸŸ 
(
ŸŸ 
e
ŸŸ 
=>
ŸŸ  
e
ŸŸ! "
.
ŸŸ" #
Description
ŸŸ# .
)
ŸŸ. /
.
⁄⁄ 
HasMaxLength
⁄⁄ 
(
⁄⁄ 
$num
⁄⁄ !
)
⁄⁄! "
.
€€ 
HasColumnName
€€ 
(
€€ 
$str
€€ ,
)
€€, -
;
€€- .
entity
‹‹ 
.
‹‹ 
Property
‹‹ 
(
‹‹ 
e
‹‹ 
=>
‹‹  
e
‹‹! "
.
‹‹" #
DestinationId
‹‹# 0
)
‹‹0 1
.
‹‹1 2
HasColumnName
‹‹2 ?
(
‹‹? @
$str
‹‹@ P
)
‹‹P Q
;
‹‹Q R
entity
›› 
.
›› 
Property
›› 
(
›› 
e
›› 
=>
››  
e
››! "
.
››" #
EndDate
››# *
)
››* +
.
››+ ,
HasColumnName
››, 9
(
››9 :
$str
››: D
)
››D E
;
››E F
entity
ﬁﬁ 
.
ﬁﬁ 
Property
ﬁﬁ 
(
ﬁﬁ 
e
ﬁﬁ 
=>
ﬁﬁ  
e
ﬁﬁ! "
.
ﬁﬁ" #

IsArchived
ﬁﬁ# -
)
ﬁﬁ- .
.
ﬂﬂ  
HasDefaultValueSql
ﬂﬂ #
(
ﬂﬂ# $
$str
ﬂﬂ$ )
)
ﬂﬂ) *
.
‡‡ 
HasColumnName
‡‡ 
(
‡‡ 
$str
‡‡ ,
)
‡‡, -
;
‡‡- .
entity
·· 
.
·· 
Property
·· 
(
·· 
e
·· 
=>
··  
e
··! "
.
··" #
SequenceNumber
··# 1
)
··1 2
.
··2 3
HasColumnName
··3 @
(
··@ A
$str
··A R
)
··R S
;
··S T
entity
‚‚ 
.
‚‚ 
Property
‚‚ 
(
‚‚ 
e
‚‚ 
=>
‚‚  
e
‚‚! "
.
‚‚" #
	StartDate
‚‚# ,
)
‚‚, -
.
‚‚- .
HasColumnName
‚‚. ;
(
‚‚; <
$str
‚‚< H
)
‚‚H I
;
‚‚I J
entity
„„ 
.
„„ 
Property
„„ 
(
„„ 
e
„„ 
=>
„„  
e
„„! "
.
„„" #
TripId
„„# )
)
„„) *
.
„„* +
HasColumnName
„„+ 8
(
„„8 9
$str
„„9 B
)
„„B C
;
„„C D
}
‰‰ 	
)
‰‰	 

;
‰‰
 
modelBuilder
ÊÊ 
.
ÊÊ 
Entity
ÊÊ 
<
ÊÊ 
User
ÊÊ  
>
ÊÊ  !
(
ÊÊ! "
entity
ÊÊ" (
=>
ÊÊ) +
{
ÁÁ 	
entity
ËË 
.
ËË 
HasKey
ËË 
(
ËË 
e
ËË 
=>
ËË 
e
ËË  
.
ËË  !
UserId
ËË! '
)
ËË' (
.
ËË( )
HasName
ËË) 0
(
ËË0 1
$str
ËË1 :
)
ËË: ;
;
ËË; <
entity
ÍÍ 
.
ÍÍ 
ToTable
ÍÍ 
(
ÍÍ 
$str
ÍÍ !
)
ÍÍ! "
;
ÍÍ" #
entity
ÏÏ 
.
ÏÏ 
HasIndex
ÏÏ 
(
ÏÏ 
e
ÏÏ 
=>
ÏÏ  
e
ÏÏ! "
.
ÏÏ" #
Email
ÏÏ# (
,
ÏÏ( )
$str
ÏÏ* 1
)
ÏÏ1 2
.
ÏÏ2 3
IsUnique
ÏÏ3 ;
(
ÏÏ; <
)
ÏÏ< =
;
ÏÏ= >
entity
ÓÓ 
.
ÓÓ 
Property
ÓÓ 
(
ÓÓ 
e
ÓÓ 
=>
ÓÓ  
e
ÓÓ! "
.
ÓÓ" #
UserId
ÓÓ# )
)
ÓÓ) *
.
ÓÓ* +
HasColumnName
ÓÓ+ 8
(
ÓÓ8 9
$str
ÓÓ9 B
)
ÓÓB C
;
ÓÓC D
entity
ÔÔ 
.
ÔÔ 
Property
ÔÔ 
(
ÔÔ 
e
ÔÔ 
=>
ÔÔ  
e
ÔÔ! "
.
ÔÔ" #
	Birthdate
ÔÔ# ,
)
ÔÔ, -
.
ÔÔ- .
HasColumnName
ÔÔ. ;
(
ÔÔ; <
$str
ÔÔ< G
)
ÔÔG H
;
ÔÔH I
entity
 
.
 
Property
 
(
 
e
 
=>
  
e
! "
.
" #
Email
# (
)
( )
.
ÒÒ 
HasMaxLength
ÒÒ 
(
ÒÒ 
$num
ÒÒ !
)
ÒÒ! "
.
ÚÚ 
HasColumnName
ÚÚ 
(
ÚÚ 
$str
ÚÚ &
)
ÚÚ& '
;
ÚÚ' (
entity
ÛÛ 
.
ÛÛ 
Property
ÛÛ 
(
ÛÛ 
e
ÛÛ 
=>
ÛÛ  
e
ÛÛ! "
.
ÛÛ" #
	IsDeleted
ÛÛ# ,
)
ÛÛ, -
.
ÛÛ- .
HasColumnName
ÛÛ. ;
(
ÛÛ; <
$str
ÛÛ< H
)
ÛÛH I
;
ÛÛI J
entity
ÙÙ 
.
ÙÙ 
Property
ÙÙ 
(
ÙÙ 
e
ÙÙ 
=>
ÙÙ  
e
ÙÙ! "
.
ÙÙ" #
Name
ÙÙ# '
)
ÙÙ' (
.
ıı 
HasMaxLength
ıı 
(
ıı 
$num
ıı !
)
ıı! "
.
ˆˆ 
HasColumnName
ˆˆ 
(
ˆˆ 
$str
ˆˆ %
)
ˆˆ% &
;
ˆˆ& '
entity
˜˜ 
.
˜˜ 
Property
˜˜ 
(
˜˜ 
e
˜˜ 
=>
˜˜  
e
˜˜! "
.
˜˜" #
PasswordHash
˜˜# /
)
˜˜/ 0
.
¯¯ 
HasMaxLength
¯¯ 
(
¯¯ 
$num
¯¯ !
)
¯¯! "
.
˘˘ 
HasColumnName
˘˘ 
(
˘˘ 
$str
˘˘ .
)
˘˘. /
;
˘˘/ 0
entity
˙˙ 
.
˙˙ 
Property
˙˙ 
(
˙˙ 
e
˙˙ 
=>
˙˙  
e
˙˙! "
.
˙˙" #
Role
˙˙# '
)
˙˙' (
.
˚˚  
HasDefaultValueSql
˚˚ #
(
˚˚# $
$str
˚˚$ ,
)
˚˚, -
.
¸¸ 
HasColumnType
¸¸ 
(
¸¸ 
$str
¸¸ 5
)
¸¸5 6
.
˝˝ 
HasColumnName
˝˝ 
(
˝˝ 
$str
˝˝ %
)
˝˝% &
;
˝˝& '
}
˛˛ 	
)
˛˛	 

;
˛˛
 
modelBuilder
ÄÄ 
.
ÄÄ 
Entity
ÄÄ 
<
ÄÄ 

BuddyAudit
ÄÄ &
>
ÄÄ& '
(
ÄÄ' (
)
ÄÄ( )
.
ÄÄ) *
HasNoKey
ÄÄ* 2
(
ÄÄ2 3
)
ÄÄ3 4
;
ÄÄ4 5
modelBuilder
ÅÅ 
.
ÅÅ 
Entity
ÅÅ 
<
ÅÅ 
	TripAudit
ÅÅ %
>
ÅÅ% &
(
ÅÅ& '
)
ÅÅ' (
.
ÅÅ( )
HasNoKey
ÅÅ) 1
(
ÅÅ1 2
)
ÅÅ2 3
;
ÅÅ3 4
modelBuilder
ÇÇ 
.
ÇÇ 
Entity
ÇÇ 
<
ÇÇ 
	UserAudit
ÇÇ %
>
ÇÇ% &
(
ÇÇ& '
)
ÇÇ' (
.
ÇÇ( )
HasNoKey
ÇÇ) 1
(
ÇÇ1 2
)
ÇÇ2 3
.
ÇÇ3 4
Ignore
ÇÇ4 :
(
ÇÇ: ;
e
ÇÇ; <
=>
ÇÇ= ?
e
ÇÇ@ A
.
ÇÇA B!
ChangedByNavigation
ÇÇB U
)
ÇÇU V
;
ÇÇV W
modelBuilder
ÑÑ 
.
ÑÑ 
Entity
ÑÑ 
<
ÑÑ 
Buddy
ÑÑ !
>
ÑÑ! "
(
ÑÑ" #
)
ÑÑ# $
.
ÑÑ$ %
Ignore
ÑÑ% +
(
ÑÑ+ ,
b
ÑÑ, -
=>
ÑÑ. 0
b
ÑÑ1 2
.
ÑÑ2 3
BuddyAudits
ÑÑ3 >
)
ÑÑ> ?
;
ÑÑ? @
modelBuilder
ÖÖ 
.
ÖÖ 
Entity
ÖÖ 
<
ÖÖ 
Trip
ÖÖ  
>
ÖÖ  !
(
ÖÖ! "
)
ÖÖ" #
.
ÖÖ# $
Ignore
ÖÖ$ *
(
ÖÖ* +
t
ÖÖ+ ,
=>
ÖÖ- /
t
ÖÖ0 1
.
ÖÖ1 2

TripAudits
ÖÖ2 <
)
ÖÖ< =
;
ÖÖ= >
modelBuilder
ÜÜ 
.
ÜÜ 
Entity
ÜÜ 
<
ÜÜ 
User
ÜÜ  
>
ÜÜ  !
(
ÜÜ! "
)
ÜÜ" #
.
ÜÜ# $
Ignore
ÜÜ$ *
(
ÜÜ* +
t
ÜÜ+ ,
=>
ÜÜ- /
t
ÜÜ0 1
.
ÜÜ1 2

UserAudits
ÜÜ2 <
)
ÜÜ< =
;
ÜÜ= >
modelBuilder
ää 
.
ää 
Entity
ää 
<
ää "
ConversationOverview
ää 0
>
ää0 1
(
ää1 2
)
ää2 3
.
ää3 4
HasNoKey
ää4 <
(
ää< =
)
ää= >
;
ää> ?$
OnModelCreatingPartial
åå 
(
åå 
modelBuilder
åå +
)
åå+ ,
;
åå, -
}
çç 
partial
èè 
void
èè $
OnModelCreatingPartial
èè '
(
èè' (
ModelBuilder
èè( 4
modelBuilder
èè5 A
)
èèA B
;
èèB C
}êê ≈¸
£C:\Users\natha\OneDrive\Skrivebord\Kea-it-arkitektur\5. semester\TravelBuddy\src\Modules\TravelBuddy.Messaging\Infrastructure\MongoDb\MongoDbMessagingRepository.cs
	namespace 	
TravelBuddy
 
. 
	Messaging 
{ 
[ #
BsonIgnoreExtraElements 
] 
internal 
class 
UserDocument 
{ 
[ 	
BsonId	 
] 
public 
int 
UserId 
{ 
get 
;  
set! $
;$ %
}& '
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}' (
=) *
null+ /
!/ 0
;0 1
public 
string 
Email 
{ 
get !
;! "
set# &
;& '
}( )
=* +
null, 0
!0 1
;1 2
public 
string 
PasswordHash "
{# $
get% (
;( )
set* -
;- .
}/ 0
=1 2
null3 7
!7 8
;8 9
public 
DateTime 
? 
	Birthdate "
{# $
get% (
;( )
set* -
;- .
}/ 0
public 
bool 
	IsDeleted 
{ 
get  #
;# $
set% (
;( )
}* +
public 
string 
Role 
{ 
get  
;  !
set" %
;% &
}' (
=) *
null+ /
!/ 0
;0 1
}   
["" #
BsonIgnoreExtraElements"" 
]"" 
internal## 
class## +
ConversationParticipantEmbedded## 2
{$$ 
public%% 
int%% 
UserId%% 
{%% 
get%% 
;%%  
set%%! $
;%%$ %
}%%& '
public&& 
DateTime&& 
?&& 
JoinedAt&& !
{&&" #
get&&$ '
;&&' (
set&&) ,
;&&, -
}&&. /
}'' 
[)) #
BsonIgnoreExtraElements)) 
])) 
internal** 
class**  
ConversationDocument** '
{++ 
[-- 	
BsonId--	 
]-- 
public.. 
int.. 
ConversationId.. !
{.." #
get..$ '
;..' (
set..) ,
;.., -
}... /
public00 
int00 
?00 
TripDestinationId00 %
{00& '
get00( +
;00+ ,
set00- 0
;000 1
}002 3
public11 
bool11 
IsGroup11 
{11 
get11 !
;11! "
set11# &
;11& '
}11( )
public22 
DateTime22 
?22 
	CreatedAt22 "
{22# $
get22% (
;22( )
set22* -
;22- .
}22/ 0
public33 
bool33 

IsArchived33 
{33  
get33! $
;33$ %
set33& )
;33) *
}33+ ,
public44 
List44 
<44 +
ConversationParticipantEmbedded44 3
>443 4
Participants445 A
{44B C
get44D G
;44G H
set44I L
;44L M
}44N O
=44P Q
new44R U
(44U V
)44V W
;44W X
}55 
[77 #
BsonIgnoreExtraElements77 
]77 
internal88 
class88 
MessageDocument88 "
{99 
public:: 
int:: 
	MessageId:: 
{:: 
get:: "
;::" #
set::$ '
;::' (
}::) *
public;; 
int;; 
ConversationId;; !
{;;" #
get;;$ '
;;;' (
set;;) ,
;;;, -
};;. /
public<< 
int<< 
SenderId<< 
{<< 
get<< !
;<<! "
set<<# &
;<<& '
}<<( )
public== 
string== 
Content== 
{== 
get==  #
;==# $
set==% (
;==( )
}==* +
===, -
null==. 2
!==2 3
;==3 4
public>> 
DateTime>> 
?>> 
SentAt>> 
{>>  !
get>>" %
;>>% &
set>>' *
;>>* +
}>>, -
}?? 
[AA #
BsonIgnoreExtraElementsAA 
]AA 
internalBB 
classBB '
TripDestinationNameDocumentBB .
{CC 
publicDD 
intDD 
TripDestinationIdDD $
{DD% &
getDD' *
;DD* +
setDD, /
;DD/ 0
}DD1 2
publicEE 
intEE 
DestinationIdEE  
{EE! "
getEE# &
;EE& '
setEE( +
;EE+ ,
}EE- .
}FF 
[HH #
BsonIgnoreExtraElementsHH 
]HH 
internalII 
classII #
DestinationNameDocumentII *
{JJ 
publicKK 
intKK 
DestinationIdKK  
{KK! "
getKK# &
;KK& '
setKK( +
;KK+ ,
}KK- .
publicLL 
stringLL 
NameLL 
{LL 
getLL  
;LL  !
setLL" %
;LL% &
}LL' (
=LL) *
nullLL+ /
!LL/ 0
;LL0 1
}MM 
publicSS 

classSS &
MongoDbMessagingRepositorySS +
:SS, - 
IMessagingRepositorySS. B
{TT 
privateUU 
readonlyUU 
IMongoCollectionUU )
<UU) * 
ConversationDocumentUU* >
>UU> ?#
_conversationCollectionUU@ W
;UUW X
privateVV 
readonlyVV 
IMongoCollectionVV )
<VV) *
MessageDocumentVV* 9
>VV9 :
_messageCollectionVV; M
;VVM N
privateWW 
readonlyWW 
IMongoCollectionWW )
<WW) *
UserDocumentWW* 6
>WW6 7
_userCollectionWW8 G
;WWG H
privateXX 
readonlyXX 
IMongoCollectionXX )
<XX) *'
TripDestinationNameDocumentXX* E
>XXE F
_tripCollectionXXG V
;XXV W
privateYY 
readonlyYY 
IMongoCollectionYY )
<YY) *#
DestinationNameDocumentYY* A
>YYA B"
_destinationCollectionYYC Y
;YYY Z
public[[ &
MongoDbMessagingRepository[[ )
([[) *
IMongoClient[[* 6
client[[7 =
)[[= >
{\\ 	
var]] 
database]] 
=]] 
client]] !
.]]! "
GetDatabase]]" -
(]]- .
$str]]. B
)]]B C
;]]C D#
_conversationCollection^^ #
=^^$ %
database^^& .
.^^. /
GetCollection^^/ <
<^^< = 
ConversationDocument^^= Q
>^^Q R
(^^R S
$str^^S b
)^^b c
;^^c d
_messageCollection__ 
=__$ %
database__& .
.__. /
GetCollection__/ <
<__< =
MessageDocument__= L
>__L M
(__M N
$str__N X
)__X Y
;__Y Z
_userCollection`` 
=``$ %
database``& .
.``. /
GetCollection``/ <
<``< =
UserDocument``= I
>``I J
(``J K
$str``K R
)``R S
;``S T
_tripCollectionaa 
=aa$ %
databaseaa& .
.aa. /
GetCollectionaa/ <
<aa< ='
TripDestinationNameDocumentaa= X
>aaX Y
(aaY Z
$straaZ a
)aaa b
;aab c"
_destinationCollectionbb "
=bb$ %
databasebb& .
.bb. /
GetCollectionbb/ <
<bb< =#
DestinationNameDocumentbb= T
>bbT U
(bbU V
$strbbV d
)bbd e
;bbe f
}cc 	
privateii 
staticii  
ConversationOverviewii +#
MapConversationOverviewii, C
(iiC D 
ConversationDocumentjj  
docjj! $
,jj$ %
MessageDocumentkk 
?kk 
lastMessageDockk +
,kk+ ,
intll 
currentUserIdll 
,ll #
DestinationNameDocumentmm #
?mm# $
destinationmm% 0
=mm1 2
nullmm3 7
,mm7 8
IEnumerablenn 
<nn 
UserDocumentnn $
>nn$ %
?nn% &
usersnn' ,
=nn- .
nullnn/ 3
)nn3 4
{oo 	
stringpp 
conversationNamepp #
;pp# $
ifrr 
(rr 
docrr 
.rr 
IsGrouprr 
)rr 
{ss 
conversationNamett  
=tt! "
destinationtt# .
!=tt/ 1
nulltt2 6
?uu 
$"uu 
$struu "
{uu" #
destinationuu# .
.uu. /
Nameuu/ 3
}uu3 4
"uu4 5
:vv 
$strvv *
;vv* +
}ww 
elsexx 
{yy 
varzz 
otherUserIdzz 
=zz  !
doczz" %
.zz% &
Participantszz& 2
.{{ 
FirstOrDefault{{ #
({{# $
p{{$ %
=>{{& (
p{{) *
.{{* +
UserId{{+ 1
!={{2 4
currentUserId{{5 B
){{B C
?{{C D
.{{D E
UserId{{E K
;{{K L
conversationName}}  
=}}! "
users}}# (
?}}( )
.~~ 
FirstOrDefault~~ #
(~~# $
u~~$ %
=>~~& (
u~~) *
.~~* +
UserId~~+ 1
==~~2 4
otherUserId~~5 @
)~~@ A
?~~A B
.~~B C
Name~~C G
?? 
$str '
;' (
}
ÄÄ 
return
ÇÇ 
new
ÇÇ "
ConversationOverview
ÇÇ +
{
ÉÉ 
ConversationId
ÑÑ 
=
ÑÑ# $
doc
ÑÑ% (
.
ÑÑ( )
ConversationId
ÑÑ) 7
,
ÑÑ7 8
TripDestinationId
ÖÖ !
=
ÖÖ# $
doc
ÖÖ% (
.
ÖÖ( )
TripDestinationId
ÖÖ) :
,
ÖÖ: ;
IsGroup
ÜÜ 
=
ÜÜ# $
doc
ÜÜ% (
.
ÜÜ( )
IsGroup
ÜÜ) 0
,
ÜÜ0 1
	CreatedAt
áá 
=
áá# $
doc
áá% (
.
áá( )
	CreatedAt
áá) 2
,
áá2 3

IsArchived
àà 
=
àà# $
doc
àà% (
.
àà( )

IsArchived
àà) 3
,
àà3 4
ParticipantCount
ââ  
=
ââ# $
doc
ââ% (
.
ââ( )
Participants
ââ) 5
.
ââ5 6
Count
ââ6 ;
,
ââ; < 
LastMessagePreview
ää "
=
ää# $
lastMessageDoc
ää% 3
?
ää3 4
.
ää4 5
Content
ää5 <
,
ää< =
LastMessageAt
ãã 
=
ãã# $
lastMessageDoc
ãã% 3
?
ãã3 4
.
ãã4 5
SentAt
ãã5 ;
,
ãã; <
ConversationName
åå  
=
åå# $
conversationName
åå% 5
}
çç 
;
çç 
}
éé 	
private
êê 
async
êê 
Task
êê 
<
êê 
Conversation
êê '
?
êê' (
>
êê( )+
MapConversationWithUsersAsync
êê* G
(
êêG H"
ConversationDocument
êêH \
?
êê\ ]
doc
êê^ a
)
êêa b
{
ëë 	
if
íí 
(
íí 
doc
íí 
==
íí 
null
íí 
)
íí 
return
ìì 
null
ìì 
;
ìì 
var
ïï 
userIds
ïï 
=
ïï 
doc
ïï 
.
ïï 
Participants
ïï *
.
ïï* +
Select
ïï+ 1
(
ïï1 2
p
ïï2 3
=>
ïï4 6
p
ïï7 8
.
ïï8 9
UserId
ïï9 ?
)
ïï? @
.
ïï@ A
Distinct
ïïA I
(
ïïI J
)
ïïJ K
.
ïïK L
ToList
ïïL R
(
ïïR S
)
ïïS T
;
ïïT U
var
óó 
userDocs
óó 
=
óó 
await
óó  
_userCollection
óó! 0
.
òò 
Find
òò 
(
òò 
Builders
òò 
<
òò 
UserDocument
òò +
>
òò+ ,
.
òò, -
Filter
òò- 3
.
òò3 4
In
òò4 6
(
òò6 7
u
òò7 8
=>
òò9 ;
u
òò< =
.
òò= >
UserId
òò> D
,
òòD E
userIds
òòF M
)
òòM N
)
òòN O
.
ôô 
ToListAsync
ôô 
(
ôô 
)
ôô 
;
ôô 
var
õõ 
userMap
õõ 
=
õõ 
userDocs
õõ "
.
õõ" #
ToDictionary
õõ# /
(
õõ/ 0
u
õõ0 1
=>
õõ2 4
u
õõ5 6
.
õõ6 7
UserId
õõ7 =
,
õõ= >
u
õõ? @
=>
õõA C
u
õõD E
)
õõE F
;
õõF G
var
ùù 
conversation
ùù 
=
ùù 
new
ùù "
Conversation
ùù# /
{
ûû 
ConversationId
üü 
=
üü) *
doc
üü+ .
.
üü. /
ConversationId
üü/ =
,
üü= >
TripDestinationId
†† !
=
††) *
doc
††+ .
.
††. /
TripDestinationId
††/ @
,
††@ A
IsGroup
°° 
=
°°) *
doc
°°+ .
.
°°. /
IsGroup
°°/ 6
,
°°6 7
	CreatedAt
¢¢ 
=
¢¢) *
doc
¢¢+ .
.
¢¢. /
	CreatedAt
¢¢/ 8
,
¢¢8 9

IsArchived
££ 
=
££) *
doc
££+ .
.
££. /

IsArchived
££/ 9
,
££9 :&
ConversationParticipants
§§ (
=
§§) *
new
§§+ .
List
§§/ 3
<
§§3 4%
ConversationParticipant
§§4 K
>
§§K L
(
§§L M
)
§§M N
,
§§N O
Messages
•• 
=
••) *
new
••+ .
List
••/ 3
<
••3 4
Message
••4 ;
>
••; <
(
••< =
)
••= >
}
¶¶ 
;
¶¶ 
foreach
®® 
(
®® 
var
®® 
p
®® 
in
®® 
doc
®® !
.
®®! "
Participants
®®" .
)
®®. /
{
©© 
userMap
™™ 
.
™™ 
TryGetValue
™™ #
(
™™# $
p
™™$ %
.
™™% &
UserId
™™& ,
,
™™, -
out
™™. 1
var
™™2 5
uDoc
™™6 :
)
™™: ;
;
™™; <
conversation
¨¨ 
.
¨¨ &
ConversationParticipants
¨¨ 5
.
¨¨5 6
Add
¨¨6 9
(
¨¨9 :
new
¨¨: =%
ConversationParticipant
¨¨> U
{
≠≠ 
ConversationId
ÆÆ "
=
ÆÆ# $
conversation
ÆÆ% 1
.
ÆÆ1 2
ConversationId
ÆÆ2 @
,
ÆÆ@ A
UserId
ØØ 
=
ØØ# $
p
ØØ% &
.
ØØ& '
UserId
ØØ' -
,
ØØ- .
JoinedAt
∞∞ 
=
∞∞# $
p
∞∞% &
.
∞∞& '
JoinedAt
∞∞' /
,
∞∞/ 0
Conversation
±±  
=
±±# $
conversation
±±% 1
,
±±1 2
User
≤≤ 
=
≤≤ 
new
≤≤ 
User
≤≤ #
{
≥≥ 
UserId
¥¥ 
=
¥¥  
p
¥¥! "
.
¥¥" #
UserId
¥¥# )
,
¥¥) *
Name
µµ 
=
µµ  
uDoc
µµ! %
?
µµ% &
.
µµ& '
Name
µµ' +
??
µµ- /
string
µµ0 6
.
µµ6 7
Empty
µµ7 <
,
µµ< =
Email
∂∂ 
=
∂∂  
uDoc
∂∂! %
?
∂∂% &
.
∂∂& '
Email
∂∂' ,
??
∂∂- /
string
∂∂0 6
.
∂∂6 7
Empty
∂∂7 <
}
∑∑ 
}
∏∏ 
)
∏∏ 
;
∏∏ 
}
ππ 
return
ªª 
conversation
ªª 
;
ªª  
}
ºº 	
private
ææ 
async
ææ 
Task
ææ 
<
ææ 
int
ææ 
>
ææ #
GetNextMessageIdAsync
ææ  5
(
ææ5 6
)
ææ6 7
{
øø 	
var
¿¿ 
last
¿¿ 
=
¿¿ 
await
¿¿  
_messageCollection
¿¿ /
.
¡¡ 
Find
¡¡ 
(
¡¡ 
FilterDefinition
¡¡ &
<
¡¡& '
MessageDocument
¡¡' 6
>
¡¡6 7
.
¡¡7 8
Empty
¡¡8 =
)
¡¡= >
.
¬¬ 
SortByDescending
¬¬ !
(
¬¬! "
m
¬¬" #
=>
¬¬$ &
m
¬¬' (
.
¬¬( )
	MessageId
¬¬) 2
)
¬¬2 3
.
√√ 
Limit
√√ 
(
√√ 
$num
√√ 
)
√√ 
.
ƒƒ !
FirstOrDefaultAsync
ƒƒ $
(
ƒƒ$ %
)
ƒƒ% &
;
ƒƒ& '
return
∆∆ 
(
∆∆ 
last
∆∆ 
?
∆∆ 
.
∆∆ 
	MessageId
∆∆ #
??
∆∆$ &
$num
∆∆' (
)
∆∆( )
+
∆∆* +
$num
∆∆, -
;
∆∆- .
}
«« 	
public
ÕÕ 
async
ÕÕ 
Task
ÕÕ 
<
ÕÕ 
IEnumerable
ÕÕ %
<
ÕÕ% &"
ConversationOverview
ÕÕ& :
>
ÕÕ: ;
>
ÕÕ; <*
GetConversationsForUserAsync
ÕÕ= Y
(
ÕÕY Z
int
ÕÕZ ]
userId
ÕÕ^ d
)
ÕÕd e
{
ŒŒ 	
var
–– 
filter
–– 
=
–– 
Builders
–– !
<
––! ""
ConversationDocument
––" 6
>
––6 7
.
––7 8
Filter
––8 >
.
—— 
	ElemMatch
—— 
(
—— 
c
—— 
=>
—— 
c
——  !
.
——! "
Participants
——" .
,
——. /
p
——0 1
=>
——2 4
p
——5 6
.
——6 7
UserId
——7 =
==
——> @
userId
——A G
)
——G H
;
——H I
var
”” 
docs
”” 
=
”” 
await
”” %
_conversationCollection
”” 4
.
””4 5
Find
””5 9
(
””9 :
filter
””: @
)
””@ A
.
””A B
ToListAsync
””B M
(
””M N
)
””N O
;
””O P
var
‘‘ 
conversationIds
‘‘ 
=
‘‘  !
docs
‘‘" &
.
‘‘& '
Select
‘‘' -
(
‘‘- .
d
‘‘. /
=>
‘‘0 2
d
‘‘3 4
.
‘‘4 5
ConversationId
‘‘5 C
)
‘‘C D
.
‘‘D E
ToList
‘‘E K
(
‘‘K L
)
‘‘L M
;
‘‘M N
var
◊◊ 
lastMessages
◊◊ 
=
◊◊ 
await
◊◊ $ 
_messageCollection
◊◊% 7
.
ÿÿ 
	Aggregate
ÿÿ 
(
ÿÿ 
)
ÿÿ 
.
ŸŸ 
Match
ŸŸ 
(
ŸŸ 
m
ŸŸ 
=>
ŸŸ 
conversationIds
ŸŸ +
.
ŸŸ+ ,
Contains
ŸŸ, 4
(
ŸŸ4 5
m
ŸŸ5 6
.
ŸŸ6 7
ConversationId
ŸŸ7 E
)
ŸŸE F
)
ŸŸF G
.
⁄⁄ 
SortByDescending
⁄⁄ !
(
⁄⁄! "
m
⁄⁄" #
=>
⁄⁄$ &
m
⁄⁄' (
.
⁄⁄( )
SentAt
⁄⁄) /
)
⁄⁄/ 0
.
€€ 
Group
€€ 
(
€€ 
m
€€ 
=>
€€ 
m
€€ 
.
€€ 
ConversationId
€€ ,
,
€€, -
g
‹‹ 
=>
‹‹ 
g
‹‹ 
.
‹‹ 
First
‹‹  
(
‹‹  !
)
‹‹! "
)
‹‹" #
.
›› 
ToListAsync
›› 
(
›› 
)
›› 
;
›› 
var
ﬂﬂ !
lastMessageByConvId
ﬂﬂ #
=
ﬂﬂ$ %
lastMessages
ﬂﬂ& 2
.
ﬂﬂ2 3
ToDictionary
ﬂﬂ3 ?
(
ﬂﬂ? @
m
ﬂﬂ@ A
=>
ﬂﬂB D
m
ﬂﬂE F
.
ﬂﬂF G
ConversationId
ﬂﬂG U
)
ﬂﬂU V
;
ﬂﬂV W
var
‚‚ 
tripDestIds
‚‚ 
=
‚‚ 
docs
‚‚ "
.
„„ 
Where
„„ 
(
„„ 
d
„„ 
=>
„„ 
d
„„ 
.
„„ 
TripDestinationId
„„ /
.
„„/ 0
HasValue
„„0 8
)
„„8 9
.
‰‰ 
Select
‰‰ 
(
‰‰ 
d
‰‰ 
=>
‰‰ 
d
‰‰ 
.
‰‰ 
TripDestinationId
‰‰ 0
)
‰‰0 1
.
ÂÂ 
ToList
ÂÂ 
(
ÂÂ 
)
ÂÂ 
;
ÂÂ 
var
ÁÁ 
tripDestDocs
ÁÁ 
=
ÁÁ 
await
ÁÁ $
_tripCollection
ÁÁ% 4
.
ËË 
Find
ËË 
(
ËË 
td
ËË 
=>
ËË 
tripDestIds
ËË '
.
ËË' (
Contains
ËË( 0
(
ËË0 1
td
ËË1 3
.
ËË3 4
TripDestinationId
ËË4 E
)
ËËE F
)
ËËF G
.
ÈÈ 
ToListAsync
ÈÈ 
(
ÈÈ 
)
ÈÈ 
;
ÈÈ 
var
ÎÎ 
destinationIds
ÎÎ 
=
ÎÎ  
tripDestDocs
ÎÎ! -
.
ÎÎ- .
Select
ÎÎ. 4
(
ÎÎ4 5
td
ÎÎ5 7
=>
ÎÎ8 :
td
ÎÎ; =
.
ÎÎ= >
DestinationId
ÎÎ> K
)
ÎÎK L
.
ÎÎL M
ToList
ÎÎM S
(
ÎÎS T
)
ÎÎT U
;
ÎÎU V
var
ÌÌ 
destinations
ÌÌ 
=
ÌÌ 
await
ÌÌ $$
_destinationCollection
ÌÌ% ;
.
ÓÓ 
Find
ÓÓ 
(
ÓÓ 
d
ÓÓ 
=>
ÓÓ 
destinationIds
ÓÓ )
.
ÓÓ) *
Contains
ÓÓ* 2
(
ÓÓ2 3
d
ÓÓ3 4
.
ÓÓ4 5
DestinationId
ÓÓ5 B
)
ÓÓB C
)
ÓÓC D
.
ÔÔ 
ToListAsync
ÔÔ 
(
ÔÔ 
)
ÔÔ 
;
ÔÔ 
var
ÒÒ 
destById
ÒÒ 
=
ÒÒ 
destinations
ÒÒ +
.
ÒÒ+ ,
ToDictionary
ÒÒ, 8
(
ÒÒ8 9
d
ÒÒ9 :
=>
ÒÒ; =
d
ÒÒ> ?
.
ÒÒ? @
DestinationId
ÒÒ@ M
)
ÒÒM N
;
ÒÒN O
var
ÚÚ 
tripDestById
ÚÚ 
=
ÚÚ 
tripDestDocs
ÚÚ +
.
ÚÚ+ ,
ToDictionary
ÚÚ, 8
(
ÚÚ8 9
td
ÚÚ9 ;
=>
ÚÚ< >
td
ÚÚ? A
.
ÚÚA B
TripDestinationId
ÚÚB S
)
ÚÚS T
;
ÚÚT U
var
ÙÙ 
users
ÙÙ 
=
ÙÙ 
await
ÙÙ 
_userCollection
ÙÙ -
.
ÙÙ- .
Find
ÙÙ. 2
(
ÙÙ2 3
_
ÙÙ3 4
=>
ÙÙ5 7
true
ÙÙ8 <
)
ÙÙ< =
.
ÙÙ= >
ToListAsync
ÙÙ> I
(
ÙÙI J
)
ÙÙJ K
;
ÙÙK L
return
˜˜ 
docs
˜˜ 
.
˜˜ 
Select
˜˜ 
(
˜˜ 
doc
˜˜ "
=>
˜˜# %
{
¯¯ !
lastMessageByConvId
˘˘ #
.
˘˘# $
TryGetValue
˘˘$ /
(
˘˘/ 0
doc
˘˘0 3
.
˘˘3 4
ConversationId
˘˘4 B
,
˘˘B C
out
˘˘D G
var
˘˘H K
lastMsg
˘˘L S
)
˘˘S T
;
˘˘T U%
DestinationNameDocument
˚˚ '
?
˚˚' (
destination
˚˚) 4
=
˚˚5 6
null
˚˚7 ;
;
˚˚; <
if
¸¸ 
(
¸¸ 
doc
¸¸ 
.
¸¸ 
TripDestinationId
¸¸ )
.
¸¸) *
HasValue
¸¸* 2
&&
¸¸3 5
tripDestById
˝˝  
.
˝˝  !
TryGetValue
˝˝! ,
(
˝˝, -
doc
˝˝- 0
.
˝˝0 1
TripDestinationId
˝˝1 B
.
˝˝B C
Value
˝˝C H
,
˝˝H I
out
˝˝J M
var
˝˝N Q
tripDest
˝˝R Z
)
˝˝Z [
&&
˝˝\ ^
destById
˛˛ 
.
˛˛ 
TryGetValue
˛˛ (
(
˛˛( )
tripDest
˛˛) 1
.
˛˛1 2
DestinationId
˛˛2 ?
,
˛˛? @
out
˛˛A D
var
˛˛E H
dest
˛˛I M
)
˛˛M N
)
˛˛N O
{
ˇˇ 
destination
ÄÄ 
=
ÄÄ  !
dest
ÄÄ" &
;
ÄÄ& '
}
ÅÅ 
return
ÉÉ %
MapConversationOverview
ÉÉ .
(
ÉÉ. /
doc
ÉÉ/ 2
,
ÉÉ2 3
lastMsg
ÉÉ4 ;
,
ÉÉ; <
userId
ÉÉ= C
,
ÉÉC D
destination
ÉÉE P
,
ÉÉP Q
users
ÉÉR W
)
ÉÉW X
;
ÉÉX Y
}
ÑÑ 
)
ÑÑ 
.
ÑÑ 
ToList
ÑÑ 
(
ÑÑ 
)
ÑÑ 
;
ÑÑ 
}
ÖÖ 	
public
áá 
async
áá 
Task
áá 
<
áá 
(
áá 
bool
áá 
Success
áá  '
,
áá' (
string
áá) /
?
áá/ 0
ErrorMessage
áá1 =
)
áá= >
>
áá> ?%
CreateConversationAsync
áá@ W
(
ááW X#
CreateConversationDto
ááX m$
createConversationDtoáán É
)ááÉ Ñ
{
àà 	
return
ââ 
(
ââ 
false
ââ 
,
ââ 
null
ââ 
)
ââ  
;
ââ  !
}
ää 	
public
åå 
async
åå 
Task
åå 
<
åå 
Conversation
åå &
?
åå& '
>
åå' (-
GetConversationParticipantAsync
åå) H
(
ååH I
int
ååI L
conversationId
ååM [
)
åå[ \
{
çç 	
var
éé 
doc
éé 
=
éé 
await
éé %
_conversationCollection
éé 3
.
èè 
Find
èè 
(
èè 
c
èè 
=>
èè 
c
èè 
.
èè 
ConversationId
èè +
==
èè, .
conversationId
èè/ =
)
èè= >
.
êê !
FirstOrDefaultAsync
êê $
(
êê$ %
)
êê% &
;
êê& '
return
íí 
await
íí +
MapConversationWithUsersAsync
íí 6
(
íí6 7
doc
íí7 :
)
íí: ;
;
íí; <
}
ìì 	
public
ïï 
async
ïï 
Task
ïï 
<
ïï 
IReadOnlyList
ïï '
<
ïï' (
Message
ïï( /
>
ïï/ 0
>
ïï0 1-
GetMessagesForConversationAsync
ïï2 Q
(
ïïQ R
int
ïïR U
conversationId
ïïV d
)
ïïd e
{
ññ 	
var
óó 
msgDocs
óó 
=
óó 
await
óó  
_messageCollection
óó  2
.
òò 
Find
òò 
(
òò 
m
òò 
=>
òò 
m
òò 
.
òò 
ConversationId
òò +
==
òò, .
conversationId
òò/ =
)
òò= >
.
ôô 
SortBy
ôô 
(
ôô 
m
ôô 
=>
ôô 
m
ôô 
.
ôô 
SentAt
ôô %
)
ôô% &
.
öö 
ToListAsync
öö 
(
öö 
)
öö 
;
öö 
var
úú 
conversation
úú 
=
úú 
await
úú $-
GetConversationParticipantAsync
úú% D
(
úúD E
conversationId
úúE S
)
úúS T
;
úúT U
var
ûû 
messages
ûû 
=
ûû 
new
ûû 
List
ûû #
<
ûû# $
Message
ûû$ +
>
ûû+ ,
(
ûû, -
)
ûû- .
;
ûû. /
foreach
üü 
(
üü 
var
üü 
m
üü 
in
üü 
msgDocs
üü %
)
üü% &
{
†† 
var
°° 
sender
°° 
=
°° 
conversation
°° )
?
°°) *
.
°°* +&
ConversationParticipants
°°+ C
.
¢¢ 
FirstOrDefault
¢¢ #
(
¢¢# $
cp
¢¢$ &
=>
¢¢' )
cp
¢¢* ,
.
¢¢, -
UserId
¢¢- 3
==
¢¢4 6
m
¢¢7 8
.
¢¢8 9
SenderId
¢¢9 A
)
¢¢A B
?
¢¢B C
.
¢¢C D
User
¢¢D H
;
¢¢H I
messages
§§ 
.
§§ 
Add
§§ 
(
§§ 
new
§§  
Message
§§! (
{
•• 
	MessageId
¶¶ 
=
¶¶# $
m
¶¶% &
.
¶¶& '
	MessageId
¶¶' 0
,
¶¶0 1
ConversationId
ßß "
=
ßß# $
m
ßß% &
.
ßß& '
ConversationId
ßß' 5
,
ßß5 6
SenderId
®® 
=
®®# $
m
®®% &
.
®®& '
SenderId
®®' /
,
®®/ 0
Content
©© 
=
©©# $
m
©©% &
.
©©& '
Content
©©' .
,
©©. /
SentAt
™™ 
=
™™# $
m
™™% &
.
™™& '
SentAt
™™' -
,
™™- .
Conversation
´´  
=
´´# $
conversation
´´% 1
??
´´2 4
new
´´5 8
Conversation
´´9 E
{
´´F G
ConversationId
´´H V
=
´´W X
conversationId
´´Y g
}
´´h i
,
´´i j
Sender
¨¨ 
=
¨¨# $
sender
¨¨% +
}
≠≠ 
)
≠≠ 
;
≠≠ 
}
ÆÆ 
return
∞∞ 
messages
∞∞ 
;
∞∞ 
}
±± 	
public
≥≥ 
async
≥≥ 
Task
≥≥ 
<
≥≥ 
Message
≥≥ !
>
≥≥! "
AddMessageAsync
≥≥# 2
(
≥≥2 3
Message
≥≥3 :
message
≥≥; B
)
≥≥B C
{
¥¥ 	
if
µµ 
(
µµ 
message
µµ 
.
µµ 
	MessageId
µµ !
==
µµ" $
$num
µµ% &
)
µµ& '
{
∂∂ 
message
∑∑ 
.
∑∑ 
	MessageId
∑∑ !
=
∑∑" #
await
∑∑$ )#
GetNextMessageIdAsync
∑∑* ?
(
∑∑? @
)
∑∑@ A
;
∑∑A B
}
∏∏ 
message
∫∫ 
.
∫∫ 
SentAt
∫∫ 
??=
∫∫ 
DateTime
∫∫ '
.
∫∫' (
UtcNow
∫∫( .
;
∫∫. /
var
ºº 
doc
ºº 
=
ºº 
new
ºº 
MessageDocument
ºº )
{
ΩΩ 
	MessageId
ææ 
=
ææ  
message
ææ! (
.
ææ( )
	MessageId
ææ) 2
,
ææ2 3
ConversationId
øø 
=
øø  
message
øø! (
.
øø( )
ConversationId
øø) 7
,
øø7 8
SenderId
¿¿ 
=
¿¿  
message
¿¿! (
.
¿¿( )
SenderId
¿¿) 1
??
¿¿2 4
$num
¿¿5 6
,
¿¿6 7
Content
¡¡ 
=
¡¡  
message
¡¡! (
.
¡¡( )
Content
¡¡) 0
,
¡¡0 1
SentAt
¬¬ 
=
¬¬  
message
¬¬! (
.
¬¬( )
SentAt
¬¬) /
}
√√ 
;
√√ 
await
≈≈  
_messageCollection
≈≈ $
.
≈≈$ %
InsertOneAsync
≈≈% 3
(
≈≈3 4
doc
≈≈4 7
)
≈≈7 8
;
≈≈8 9
return
∆∆ 
message
∆∆ 
;
∆∆ 
}
«« 	
public
   
async
   
Task
   
<
   
IEnumerable
   %
<
  % &
ConversationAudit
  & 7
>
  7 8
>
  8 9(
GetConversationAuditsAsync
  : T
(
  T U
)
  U V
{
ÀÀ 	
return
ÕÕ 
await
ÕÕ 
Task
ÕÕ 
.
ÕÕ 

FromResult
ÕÕ (
(
ÕÕ( )

Enumerable
ÕÕ) 3
.
ÕÕ3 4
Empty
ÕÕ4 9
<
ÕÕ9 :
ConversationAudit
ÕÕ: K
>
ÕÕK L
(
ÕÕL M
)
ÕÕM N
)
ÕÕN O
;
ÕÕO P
}
ŒŒ 	
}
œœ 
}–– „
çC:\Users\natha\OneDrive\Skrivebord\Kea-it-arkitektur\5. semester\TravelBuddy\src\Modules\TravelBuddy.Messaging\IMessagingRepositoryFactory.cs
	namespace 	
TravelBuddy
 
. 
	Messaging 
{ 
public 

	interface '
IMessagingRepositoryFactory 0
{  
IMessagingRepository "
GetMessagingRepository 3
(3 4
)4 5
;5 6
} 
} ®
ÜC:\Users\natha\OneDrive\Skrivebord\Kea-it-arkitektur\5. semester\TravelBuddy\src\Modules\TravelBuddy.Messaging\IMessagingRepository.cs
	namespace 	
TravelBuddy
 
. 
	Messaging 
;  
public 
	interface  
IMessagingRepository %
{ 
Task		 
<		 	
IEnumerable			 
<		  
ConversationOverview		 )
>		) *
>		* +(
GetConversationsForUserAsync		, H
(		H I
int		I L
userId		M S
)		S T
;		T U
Task

 
<

 	
(

	 

bool


 
Success

 
,

 
string

 
?

 
ErrorMessage

  ,
)

, -
>

- .#
CreateConversationAsync

/ F
(

F G!
CreateConversationDto

G \!
createConversationDto

] r
)

r s
;

s t
Task 
< 	
Conversation	 
? 
> +
GetConversationParticipantAsync 7
(7 8
int8 ;
conversationId< J
)J K
;K L
Task 
< 	
IReadOnlyList	 
< 
Message 
> 
>  +
GetMessagesForConversationAsync! @
(@ A
intA D
conversationIdE S
)S T
;T U
Task 
< 	
Message	 
> 
AddMessageAsync !
(! "
Message" )
message* 1
)1 2
;2 3
Task 
< 	
IEnumerable	 
< 
ConversationAudit &
>& '
>' (&
GetConversationAuditsAsync) C
(C D
)D E
;E F
} ÿ
åC:\Users\natha\OneDrive\Skrivebord\Kea-it-arkitektur\5. semester\TravelBuddy\src\Modules\TravelBuddy.Messaging\DTOs\CreateConversationDto.cs
	namespace 	
TravelBuddy
 
. 
	Messaging 
;  
public 
record !
CreateConversationDto #
($ %
int 
OwnerId 
, 
int 
? 
TripDestinationId	 
, 
bool 
IsGroup	 
, 
int 
? 
OtherUserId	 
) 
; ‰
áC:\Users\natha\OneDrive\Skrivebord\Kea-it-arkitektur\5. semester\TravelBuddy\src\Modules\TravelBuddy.Messaging\DTOs\ConversationDtos.cs
	namespace 	
TravelBuddy
 
. 
	Messaging 
;  
public 
record "
ConversationSummaryDto $
(% &
int 
Id 

,
 
bool 
IsGroup	 
, 
bool 

IsArchived	 
, 
int 
ParticipantCount 
, 
DateTime 
? 
	CreatedAt 
)		 
;		 
public 
record #
ConversationOverviewDto %
(& '
int 
ConversationId 
, 
int 
? 
TripDestinationId	 
, 
bool 
IsGroup	 
, 
DateTime 
? 
	CreatedAt 
, 
bool 

IsArchived	 
, 
int 
ParticipantCount 
, 
string 

?
 
LastMessagePreview 
, 
DateTime 
? 
LastMessageAt 
, 
string 

?
 
ConversationName 
) 
; 
public 
record &
ConversationParticipantDto (
() *
int 
UserId 
, 
string 

Name 
, 
string 

Email 
) 
; 
public 
record !
ConversationDetailDto #
($ %
int 
Id 

,
 
bool 
IsGroup	 
, 
bool 

IsArchived	 
, 
DateTime   
?   
	CreatedAt   
,   
int!! 
ParticipantCount!! 
,!! 
IEnumerable"" 
<"" &
ConversationParticipantDto"" *
>""* +
Participant"", 7
)## 
;## 
public%% 
record%% 

MessageDto%% 
(%% 
int&& 
Id&& 

,&&
 
int'' 
ConversationId'' 
,'' 
int(( 
?(( 
SenderId((	 
,(( 
string)) 

?))
 

SenderName)) 
,)) 
string** 

Content** 
,** 
DateTime++ 
?++ 
SentAt++ 
),, 
;,, 
public.. 
record.. !
SendMessageRequestDto.. #
(..# $
string// 

Content// 
)00 
;00 ¸
ãC:\Users\natha\OneDrive\Skrivebord\Kea-it-arkitektur\5. semester\TravelBuddy\src\Modules\TravelBuddy.Messaging\DTOs\ConversationAuditDto.cs
	namespace 	
TravelBuddy
 
. 
	Messaging 
.  
Models  &
;& '
public 
record  
ConversationAuditDto "
(" #
int 
AuditId 
, 
int 
ConversationId 
, 
int 
? 
AffectedUserId	 
, 
string 

Action 
, 
int 
? 
	ChangedBy	 
, 
DateTime		 
?		 
	Timestamp		 
)

 
;

 