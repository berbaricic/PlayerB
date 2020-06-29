# SessionControl - servis za kontrolu sesija
Projekt u sklopu internship-a u kojem je cilj bio učenje određenih koncepata i tehnologija u programiranju.

## Ideja projekta

Ideja ovog projekta je izrada servisa koji bi imao kontrolu nad sesijom. Sesiju možemo definirati kao jedno gledanje videa nekog sadržaja. 
Sesija je entitet koji ima svoj identifikator te stanja koja se mijenjaju kroz PING. Stanja sesije mogu biti sljedeća: PLAY, PAUSE, SEEK, ENDED, FINISHED koja dobivamo iz događaja
na playeru. Sesija započinje prilikom play-a playera te traje do gašenja playera. Ukoliko PING ne dolazi 60 sekundi, servis treba zatvoriti sesiju.

## Rješenje projekta

Rješenje projekta dijelimo na 5 komponenata: klijentska aplikacija, web servis, cache, baza i backgroundworker.

  * Klijentska aplikacija: u aplikaciji se nalazi videoplayer koji sadrži događaje (play, pause, seek, ended). Korištenjem aplikacije korisnik aktivira događaje 
  koji šalju zahtjeve (requests) na Web API kontroler te potom se određene akcije odvijaju.
  * Web API: u API kontroleru se nalaze tri metode za obradu zahtjeva (GET, POST, PUT). Aktiviranjem događaja "play", s POST metodom kreira se novi objekt 
  u cache-u te spremaju se sljedeći podaci (atributi): Id, Status, UserAdress, IdVideo, RequestTime. Svako iduće aktiviranje nekog od događaja, s PUT metodom izmjenjuje se
  "Status" postojećeg objekta.
  * Cache: u cache spremamo zapise dobivene na temelju aktiviranih događaja. Zapisi se pohranju u strukturu "Key-Value pairs" gdje se key generira te je jedinstven, a u value spremamo podatke o sesiji.
  Također, u drugu strukturu "SortedSet" spremamo ključeve (member) i vrijeme zadnjeg zahtjeva na taj ključ(score). SortedSet nam omogućava provjeru isteklih sesija koje onda možemo 
  dohvatiti iz "Key-Value pairs" te spremiti u bazu.
  * Baza: u bazu spremamo periodički zapise iz cache-a (sesije koje su istekle). 
  * BackgroundWorker: omogućava periodičku obradu podataka te prebacivanje isteklih sesija u bazu.
  
  ![arhitektura](https://user-images.githubusercontent.com/18721181/85980973-a7abf280-b9e3-11ea-9a48-b1b631b00744.png)
  
  Na slici možemo vidjeti prikaz navedenih komponenata aplikacije, gdje je na 5 komponenata dodan još StackExchange.Redis kao Redis klijent za C# i Dapper kao ORM koji služi mapiranju između baze i C#-a.

## Tehnologije
Za izradu projekta korištene su sljedeće tehnologije:
  * Microsoft Visual Studio 2019 Community
      - ASP.NET Core MVC
      - StackExchange.Redis: Redis klijent za C#
      - Dapper: micro ORM (mapiranje između baze i C#-a)
  * Redis Cache
  * Microsoft SQL Server 2019
  * Microsoft SQL Server Managment Studio 2019
  * Docker - pokretanje MsSQL Server i Redis Cache container image-a
  
  
  


