# SessionControl - servis za kontrolu sesija
Projekt u sklopu internship-a u kojem je cilj bio učenje određenih koncepata i tehnologija u programiranju.

## Ideja projekta

Ideja ovog projekta je izrada servisa koji bi imao kontrolu nad sesijom. Sesiju možemo definirati kao jedno gledanje videa nekog sadržaja. 
Sesija je entitet koji ima svoj identifikator te stanja koja se mijenjaju kroz PING. Stanja sesije mogu biti sljedeća: PLAY, PAUSE, UNSTARTED, VIDEO_CUED,ENDED i CLOSED koja dobivamo iz događaja
na playeru. Sesija započinje prilikom play-a playera te traje do gašenja playera. Ukoliko PING ne dolazi 60 sekundi, servis treba zatvoriti sesiju. Potrebno je održavati sesiju kroz PING svakih 30 sekundi te slati status playera kako imali zapis što se izvodi. 

## Rješenje projekta

Rješenje projekta dijelimo na 5 komponenata: klijentska aplikacija, web servis, cache, baza i backgroundworker.

  * Klijentska aplikacija (VideoPlayer): Angular aplikacija u kojoj je omogućeno pokretanje YouTube videa. Početna stranica sadrži pretragu videa te popis YouTube videa, točnije prikaz umanjenih slika gdje svaka predstavlja jedan YouTube video. Odabirom jedne od tih link-slika, otvara se novi page gdje se može pokrenuti video. Prilikom pokretanja videa, događaju se određeni događaji. S događajem "UNSTARTED" pokrećemo player te se kreira novi HTTP POST zahjev koji predstavlja pokretanje nove sesije. Na svaku izmjenu događaja, s POST metodom dodajemo nove zapise u cache-u te također svakih 30 sekundi od pokretanja playera se šalje zahtjev sa statusom playera kako bi se sesija redovno odražavala. Ako se napusti page (stranica) ili zatvori pretraživač, status sesije se postavlja na CLOSED. Provjeru istekle ili završene sesije obaviti će BackgroundWorker.
  Angular aplikacija sastavljena je od tri modula: standardni "app.module" te novokreirani "video.module" i "search.module".
    - "video.module" import-a "YouTubePlayerModule" koji nam omogućuje YT player kao i praćanje događaja na njemu te "HttpClientModule" koji nam služi za slanje HTTP zahtjeva na Web API. Također, sam "video.module" import-amo u "app.module" (root module) koji pokreće aplikaciju te bez toga import-a "video.module" ne bi imao svrhu,a i root modul ne bi imao funkcionalnost pokretana YT playera i slanje HTTP zahtjeva. 
    - "search.module" se također import-a u "app.module" te daje mogućnost pretrage YouTube videa te popis pretraženih videa. Da bi se ovo ostvarilo bilo je potrebno koristiti [YouTube v3 search API](https://developers.google.com/youtube/v3/docs/search/list). Da bi se API mogao koristiti, bilo je potrebno generirati API_KEY te omogućiti YouTube Data API v3. Nakon pretrage i učitavanja videa, odabiremo jedan video za izvedbu te otvara se novi page na kojem možemo pokrenuti video. Prikaz YouTube videa te sve karakteristike vezanu uz YouTube video, dobili smo uz pomoć [YouTube Player API for iframe Embeds](https://developers.google.com/youtube/iframe_api_reference#top_of_page). 
    
    Također, dio prikaza rezultata i state management (sačuvani rezultati pretrage) su riješeni uz upotrebu Facade pattern-a i Observables-a. State (rezultati pretrage) je sačuvan u posebnoj vrsti Observable-a koji se zove BehaviorSubject. S BehaviorSubject dobiveno je prikazivanje (emitiranje) zadnjih rezultata na view-u, kao i čuvanje zadnje pretrage ako se obriše input iz searchbox-a. View je sada postao subscriber na varijablu videoDetail koja emitira rezultate, ali i sakriva state management od samog view-a. Nakon unašanja inputa u searchbox, videoDetail poprima tu vrijednost te se ta vrijednost sada emitira. 
  * Web API: u API kontroleru se nalazi jedna metoda za obradu zahtjeva (POST). Aktiviranjem događaja "play", s POST metodom kreira se novi objekt 
  u cache-u te spremaju se sljedeći podaci (atributi): Id, Status, UserAdress, IdVideo, RequestTime. Svako iduće aktiviranje nekog od događaja, također s POST metodom kreiramo novi zapis koji sadrži trenutnu sesiju s 
  * Cache: u cache spremamo zapise dobivene na temelju aktiviranih događaja. Zapisi se pohranju u strukturu "Key-Value pairs" gdje se key generira te je jedinstven, a u value spremamo podatke o sesiji.
  Također, u drugu strukturu "SortedSet" spremamo ključeve (member) i vrijeme zadnjeg zahtjeva na taj ključ(score). SortedSet nam omogućava provjeru isteklih sesija koje onda možemo 
  dohvatiti iz "Key-Value pairs" te spremiti u bazu.
  * Baza: u bazu spremamo periodički zapise iz cache-a (sesije koje su istekle). Baza sadrži dvije tablice: Session i Videoplayer. Tablica Session sadrži atribute Id, SessionId, Status, UserAdress, IdVideo i RequestTime, dok tablica Videoplayer sadrži atribute Id i IdVideo. Pri pokretanju kontejnera, na Ms SQL Serveru, kreira se baza SessionDatabase s navedenim tablicama te obavlja se mehanizam perzistencije podataka, i to prebacivanje podataka s vanjskog diska na unutarnji folder kontejnera. Tako smo dobili trajne podatke, i ako se kontejner restarta.
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

## Pokretanje komponenata

Kako bi pokrenuli svih 5 komponenata, dovoljno je preuzeti docker-compose.yaml datoteku koja se nalazi u glavnom direktoriju ovog repozitorija. Nakon preuzimanja, navedenu datoteku možemo spremiti u određeni folder na našem računalu. Uz pomoć omiljenog terminala navodimo se do tog foldera te ulazimo u njega. Uz preduvjet da je Docker već instaliran na računalu, s naredbom "docker-compose up --build" možemo preuzeti slike i pokrenuti kontejnere. 
* Angular aplikacija dostupna na: http://localhost:4200
* Web Api dostupan na: http://localhost:5000
* Ostale komponente možemo razmatrati preko njihovih CLI-a (redis-cli, sqlcmd) ili preko logova na Docker Desktopu.

Na slici ispod vidimo da je pokrenuto svih 5 komponenata te možemo vidjeti nazive kontejnera, nazive njihovih image-a te koji su portovi pridjeljeni u kontejnerima.

 <p align="center">
  <img src="https://user-images.githubusercontent.com/18721181/89890312-6a867300-dbd3-11ea-8121-6f7d7e06dc30.PNG">
</p>

## Tehnologije
Za izradu projekta korištene su sljedeće tehnologije:
  * Microsoft Visual Studio 2019 Community
      - ASP.NET Core MVC
      - StackExchange.Redis: Redis klijent za C#
      - Dapper: micro ORM (mapiranje između baze i C#-a)
      - Angular 8     
  * Redis Cache
  * Microsoft SQL Server 2019
  * Microsoft SQL Server Managment Studio 2018
  * Docker 
  * Postman - Web API testiranje
  * JMeter - load testiranje
  
## Dodatni zadaci: SignalR, XUnit 

### SignalR 

SignalR je open-source library koji pojednostavljuje dodavanje real-time funkcionalnosti aplikacijama uz pomoć HTTP-a (protokol za komunikaciju između klijenta i servera). Real-time znači da server strana push-a podatke istog trena na sve klijente koji su povezani. SignalR koristi "hubs" - high level pipeline koji omogućuje klijentu i serveru međusobno pozivanje metoda.

Za primjenu SignalR, riješen je sljedeći zadatak: SignalR servis push-a periodično broj zapisa iz tablice Session na korisničku aplikaciju te se taj broj prikazuje u headeru.

Rješavanje zadatka je započeto s preuzimanjem SignalR library-a. Budući da je SignalR server library već uključen u ASP.NET Core, samo je preuzet SignalR client library za Angular. 
Nakon toga kreirana je klasa SessionHub koja se izvodi iz klase SignalR Hub. SessionHub klasa je prazna jer zasad ostvarujemo samo jednosmjernu komunikaciju (server prema klijentu), stoga nijedna metoda nije tu kreirana.
Potom, u ConfigureService metodi dodan je SignalR u IService kolekciju te u metodi Configure usmjeravamo SignalR zahtjeve na SessionHub uz pomoć putanje /signalr.
Kako bi upravljali s HTTP zahtjevima, kreiramo novi kontroler koji ima putanju api/signalr. U kontroleru kreiramo instancu interface-a IHubContext (dependency injection) te s objektom instance može pristupiti i pozvati hub-ove metode.
Kreirana je metoda Get u kojoj je pozvana također instanca klase TimerManager gdje je sređen timer. Svi klijenti se mogu subscribe-ati na ime "ShowNumber" kako bi dobivali podatak o broju zapisa u bazi.
Na Angular aplikaciji, nakon preuzimanje SignalR library-a, kreiran je servis u kojem startamo konekciju na naš SignalR, dodajemo listenera na metodu "ShowNumber" i šaljemo HTTP zahtjev kako bi dobili odgovor s servera. Podatak 
o broju zapisa u bazi prikazujemo u Navigation komponenti.

### XUnit

Xunit je besplatni open-source alat za testiranje dijelova u .NET Core-u. Unit testovi su provedeni na testnoj klasi koja je kreirana samo za odrađivanje ovog zadatka. Ime klase je Weather te sadrži metodu za testiranje. Metoda "MethodTest" provjerava parametre metode da li su null te provjerava da li lokacija sadrži brojeve. Nadalje, metoda poziva 3rd party servis (Weather API), čeka odgovor te ga evaluira. Rad s servisom treba biti riješen korištenjem Moy frameworka, to znači da ćemo imitirati ponašanje objekata HttpWebRequest i HttpWebRequest te time nećemo stvarati prave objekte (nećemo pozivati servis, čekati odgovor, evaluirati pravi odgovor). Ovo izolira kôd koji testiramo, osiguravajući da radi sam za sebe i da niti jedan drugi kôd neće srušiti testove.

Prvo su provedena tri testa koja su vezana uz provjeru argumenata, a zatim su provedena tri testa koji obuhvaćaju rad s mockanim objektima. U tim testovima prvo su kreirani mockani objekti te zatim je testirano ponašanje dijela metode s servisom. 


