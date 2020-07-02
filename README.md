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
  
 <p align="center">
  <img src="https://user-images.githubusercontent.com/18721181/85980973-a7abf280-b9e3-11ea-9a48-b1b631b00744.png">
</p>
  
  Na slici možemo vidjeti prikaz navedenih komponenata aplikacije, gdje je na 5 komponenata dodan još StackExchange.Redis kao Redis klijent za C# i Dapper kao ORM koji služi mapiranju između baze i C#-a.
  
## Testiranje 

Kako bi se simulirao realni scenarij korištenja API-a, cache-a i baze, potrebno je bilo izraditi load test koji bi sadržavao prikaz potencijalnog rada ovih komponenti. Load test bi prikazao perfomanse izvođenja ovog sustava, a za samo modeliranje i izvedbu testa koristit ćemo JMeter. 

Jedan od scenarija koji be testirao ponašanje komponenata te mjerio perfomanse istih glasi ovako: 
 - pokretanje 1000 thread-ova (users) da rade 1h
 - svaki thread radi sljedeće:
   1. pokrene sesiju
   2. okida random (5-20) broj pingova svakih 30 sekundi
   3. prekida sesiju (osim u 30% slučajeva kada nema zahtjeva prekida)
   
Navedeni scenarij izrađen je u JMeter-u, kao što i vidimo na slici ispod. U Test Plan stavili smo novu Thread grupu ("Sesija") u kojo smo specificirali 1000 thread-ova te ukupno trajanje od 1h. Na samom početku ove grupe specificirali smo u HTTP Header Manager-u Content-Type koji će prihvaćati json (application/json) te u HTTP Request Defaults-u specificirano je ime servera, port number i protokol. Sada dolazimo do dijela gdje određujemo što će svaki thread raditi: "Once Only Controller" upravlja POST request-om te dogoditi će se samo jednom, što znači pokrenuti ćemo sesiju (postavljamo objekt u cache te ima status "PLAY" te ID sesije će biti generiran s UUID funkcijom). Nadalje dolazimo do "Loop Controller-a" koji se odvija N puta (N je zapravo neki random broj od 5 do 20 kojeg generiramo u "Random Number of Pings"). S "Random Controller-om" odabiremo jedan od PUT request-ova koji će biti poslani (šalje se status "RESUME", "SEEK" ili "PAUSE"). Unutar ovog kontrolera smješten je i timer koji će pauzirati odvijanje loop-a na 30 sekundi. Još jedan element u ovom kontroleru je "HTTP Request Defaults" koji preuzima ime servera, port i protokol od elementa konfiguriranog na samom početku ove grupe. U njega je samo dodatno je smještena putanja (PATH) za dohvaćanje resursa (endpoint). Zadnji kontroler je "Throughput Controller" koji se odvije jednom za svaki thread te postavlja na 70% thread-ova prekid sesije (PUT Request s statusom "FINISHED"). Tu je isto element "HTTP Request Defaults" u koji smještamo putanju do resursa. 

<p align="center">
  <img width="250" height="300" src="https://user-images.githubusercontent.com/18721181/86340619-79cde480-bc55-11ea-8dea-cab0fce2bcac.PNG">
</p>

Na kraju Thread grupe imamo dva reporta: "View Results Report" i "Aggregate Report". Prema njima možemo doći do nekih zapažanja i zaključaka. Prema "View Result Report" se može vidjeti jesu li zahtjevi uspješno izvršeni, ali to isto, samo preglednije možemo vidjeti na ""Aggregate Report-u", koji je prikazan na slici ispod. Vidimo da je izvršeno 1000 POST zahtjeva, što znači da su svi thread-ovi uspješno pokrenuli svoju sesiju. Potom vidimo PUT zahtjeve (Resume, Pause, Seek) koji su se random slali svakih 30 sekundi. Prema PUT zahtjevu za završetak sesije vidimo da je 700 sesija prekinuto (status FINISHED) što je upravo ono što smo zadali (70%). Ukupni broj zahtjeva je 14277 koji su se odvili u 10 minuta i 53 sekunde što je zadovoljavajuće. U stupcu "Error %" vidimo da je postotak grešaka za sve metode 0.00 %. 

<p align="center">
  <img src="https://user-images.githubusercontent.com/18721181/86340835-cd403280-bc55-11ea-8974-3e91037a60b8.PNG">
</p>

Pozadinski dio ovog testa je isto uspješan. Background Worker je uspješno periodički (svakih 60 sekundi) obavljao perzistenciju - provjeravao istekle sesije te sesije sa statusom FINISHED, iste spremao u bazu te brisao ih iz cache-a.

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
  * Postman - Web API testiranje
  * JMeter - load testiranje
  
