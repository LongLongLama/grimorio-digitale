# Grimorio Digitale 📖✨

**Grimorio Digitale** è un tool manager per Dungeons & Dragons in C#/.NET che automatizza la creazione e la gestione dei personaggi. Semplifica il calcolo della scheda, l'inventario e il level up. La sua architettura flessibile offre sia un'interfaccia console (Spectre.Console) che una Web API (ASP.NET), per la massima versatilità.

---

### 📜 Indice

*   [Informazioni sul Progetto](#-informazioni-sul-progetto)
*   [Funzionalità Principali](#-funzionalità-principali)
*   [Stack Tecnologico](#-stack-tecnologico)
*   [Architettura del Progetto](#-architettura-del-progetto)
*   [Come Iniziare](#-come-iniziare)
    *   [Prerequisiti](#prerequisiti)
    *   [Installazione](#installazione)
*   [Utilizzo](#-utilizzo)
*   [Sviluppi Futuri](#-sviluppi-futuri)
*   [Contatti](#-contatti)

---

### ℹ️ Informazioni sul Progetto

Questo progetto nasce dalla passione per Dungeons & Dragons e dalla volontà di creare uno strumento utile sia per i giocatori che per i Dungeon Master. L'obiettivo è centralizzare tutte le informazioni di un personaggio, automatizzando i calcoli più ripetitivi e offrendo un rapido accesso a un compendio di regole, magie e oggetti.

La caratteristica distintiva del progetto è la sua **architettura a layer disaccoppiati**, che permette di utilizzare la stessa logica di business sia da una semplice e veloce applicazione console, sia esponendola tramite una moderna API web pronta per essere consumata da un frontend.

---

### ✨ Funzionalità Principali

*   **Creazione Guidata del Personaggio**: Un percorso step-by-step per creare nuovi personaggi in modo facile e intuitivo.
*   **Scheda Personaggio Automatica**: Calcolo e aggiornamento automatico di tutte le statistiche, bonus, tiri salvezza e competenze.
*   **Compendio Integrato**: Un database per consultare rapidamente informazioni su classi, razze, magie e regole.
*   **Gestione dell'Inventario**: Tracciamento dettagliato di armi, armature, oggetti e valuta.
*   **Avanzamento di Livello**: Funzione per il DM per gestire il level up dei PG, con ricalcolo automatico delle statistiche.
*   **Doppia Interfaccia**: Accesso ai dati sia tramite un'app console che tramite una Web API RESTful.

---

### 🛠️ Stack Tecnologico

*   **Linguaggio**: C#
*   **Framework**: .NET / ASP.NET Core
*   **Accesso ai Dati**: Entity Framework Core
*   **Database**: SQL Server
*   **Layer Console**: Spectre.Console per un'interfaccia testuale ricca e interattiva.
*   **IDE**: Visual Studio

---

### 🏛️ Architettura del Progetto

Il progetto è strutturato in layer separati per garantire manutenibilità e scalabilità (Separation of Concerns).
---

### 🚀 Come Iniziare

Segui queste istruzioni per avere una copia del progetto funzionante sulla tua macchina.

#### Prerequisiti

Assicurati di avere installato:
*   [.NET SDK](https://dotnet.microsoft.com/download) (versione 9.0)
*   Un IDE come [Visual Studio](https://visualstudio.microsoft.com/) o [JetBrains Rider](https://www.jetbrains.com/rider/).

#### Installazione

1.  Clona la repository:
    ```bash
    git clone https://github.com/LongLongLama/grimorio-digitale.git
    ```
2.  Apri il file `GrimorioDigitale.sln` con il tuo IDE.
3.  Esegui il build della solution per ripristinare i pacchetti NuGet.
4.  (Se necessario) Applica le migrazioni di Entity Framework per creare il database:
    ```bash
    dotnet ef database update --project NOME_PROGETTO_INFRASTRUCTURE
    ```

---

### 💡 Utilizzo

A seconda della versione che vuoi lanciare:

1.  **Per la versione Console**:
    *   In Visual Studio, fai clic destro sul progetto `DnD.App` e seleziona "Imposta come progetto di avvio".
    *   Premi F5 per avviare l'applicazione.

2.  **Per la versione Web API**:
    *   Imposta il progetto `DnD.Api` come progetto di avvio.
    *   Premi F5. L'API sarà in ascolto su `https://localhost:[5000]`.
    *   Puoi esplorare gli endpoint disponibili tramite l'interfaccia di Swagger all'indirizzo `https://localhost:[5000]/swagger`.

---

### 🔮 Sviluppi Futuri

*   [ ] Creazione di un'interfaccia frontend in **Angular** per consumare la Web API.
*   [ ] Aggiunta di un sistema di autenticazione per gestire utenti e campagne separate.

---

### 📫 Contatti

Alessandro La Martina – ([URL_LINKEDIN](https://www.linkedin.com/in/alessandro-la-martina-1a84302b0/))

Link al Progetto: [https://github.com/LongLongLama/grimorio-digitale](https://github.com/LongLongLama/grimorio-digitale)
