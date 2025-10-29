Assolutamente sì! Invece di usare un path assoluto specifico del tuo computer (C:\Users\Davide…), puoi usare un path relativo “universale” che funziona su qualsiasi macchina su cui giri la tua app ASP.NET Core.

1️⃣ Salva i file nella cartella wwwroot del progetto

ASP.NET Core serve automaticamente i file statici dentro wwwroot.

Esempio:

Progetto/
│
├─ wwwroot/
│  └─ songs/
│      └─ Monkeys-Spinning-Monkeys.mp3

2️⃣ Usa un path relativo nel database

Colonna FilePath → salva il percorso relativo:

/songs/Monkeys-Spinning-Monkeys.mp3


Così la tua Web API o il client web può generare l’URL completo in modo dinamico, senza dipendere dal percorso locale del tuo PC.

3️⃣ Endpoint ASP.NET Core per servire i file

Nel tuo Startup.cs o Program.cs devi abilitare i file statici (se non già fatto):

app.UseStaticFiles(); // permette l'accesso ai file in wwwroot


Poi nel database basta avere /songs/Monkeys-Spinning-Monkeys.mp3 e nel front-end puoi fare:

<audio controls>
    <source src="https://tuo-dominio.com/songs/Monkeys-Spinning-Monkeys.mp3" type="audio/mpeg">
</audio>

4️⃣ Query SQL per aggiornare tutti i record con path relativo
UPDATE Songs
SET FilePath = '/songs/Monkeys-Spinning-Monkeys.mp3';


✅ Funzionerà su qualsiasi macchina, perché la Web API userà wwwroot come root dei file statici.

Se vuoi, posso prepararti uno schema completo con la tabella SQL + query + struttura della cartella wwwroot + esempio di endpoint ASP.NET Core già pronto per servire tutte le canzoni senza dipendere dai percorsi locali. Vuoi che lo faccia?

