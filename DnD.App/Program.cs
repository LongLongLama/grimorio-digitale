using DnD.Domain.Entities;
using DnD.Domain.Enums;
using DnD.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Spectre.Console;

namespace DnD.App
{
    internal class Program
    {

        private static readonly Random random = new Random();
        static void Main(string[] args)
        {
            using var context = new AppDbContext();
            MostraMenuPrincipale(context);
        }

        public static void MostraMenuPrincipale(AppDbContext context)
        {
            

            while (true)
            {
                Console.Clear();
                AnsiConsole.MarkupLine("[bold yellow]Benvenuto nel tuo Compendio di Dungeons & Dragons![/]");
                AnsiConsole.WriteLine();
                var scelta = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Cosa vuoi fare?")
                        .PageSize(10)
                        .AddChoices(new[] {
                            "Visualizza tutti i Personaggi",
                            "Consulta l'Enciclopedia (Razze, Classi, ecc.)",
                            "Accedi come Giocatore",
                            "Modalità Master",
                            "Esci dall'applicazione"
                        }));

                switch (scelta)
                {
                    case "Visualizza tutti i Personaggi":
                        VisualizzaTuttiPersonaggi(context);
                        break;
                    case "Consulta l'Enciclopedia (Razze, Classi, ecc.)":
                        MostraMenuEnciclopedia(context);
                        break;
                    case "Accedi come Giocatore":
                        MenuLoginGiocatore(context);
                        break;
                    case "Modalità Master": 
                        MostraMenuMaster(context); 
                        break;
                    case "Esci dall'applicazione":
                        AnsiConsole.MarkupLine("[bold red]Arrivederci, avventuriero![/]");
                        Environment.Exit(0);
                        return;
                }
            }
        }

        public static void MenuLoginGiocatore(AppDbContext context)
        {
            AnsiConsole.MarkupLine("[bold]Selezione del Giocatore[/]");
            var players = context.Players.ToList();

            if (!players.Any())
            {
                AnsiConsole.MarkupLine("[red]Nessun giocatore trovato nel database. Aggiungine uno prima.[/]");
                Console.ReadKey();
                return;
            }

            var chosenPlayer = AnsiConsole.Prompt(
                new SelectionPrompt<Player>()
                    .Title("Chi sta giocando?")
                    .PageSize(10)
                    .UseConverter(p => $"{p.FirstName} {p.LastName}")
                    .AddChoices(players));

            AnsiConsole.MarkupLine($"[green]Accesso effettuato come {chosenPlayer.FirstName}.[/]");
            MostraMenuGiocatore(context, chosenPlayer);
        }

        public static void InserimentoPersonaggio(AppDbContext context, Player p)
        {

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + ("benvenuto".Length / 2)) + "}", "BENVENUTO!"));
            Console.WriteLine("");
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + ("| Creazione guidata di un nuovo personaggio |".Length / 2)) + "}", "| Creazione guidata di un nuovo personaggio |"));
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            Console.WriteLine("");
            var nome = AnsiConsole.Prompt(
            new TextPrompt<string>("Qual'è il nome del tuo personaggio?"));
            var racesFromDb = context.CharacterRaces.ToList();
            List<string> razze = [];
            foreach (var item in racesFromDb)
            {
                razze.Add(item.Name);
            }
            var razza = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Seleziona una [green]razza[/]!")
                            .PageSize(10)
                            .MoreChoicesText("[grey](Scorri su e giù per vedere le razze)[/]")
                            .AddChoices(razze));
            AnsiConsole.WriteLine($"Hai selezionato [green]{razza}[/]");
            var razzaSel = racesFromDb.Where(r => r.Name.Equals(razza)).FirstOrDefault();
            var classesFromDb = context.CharacterClasses.ToList();
            List<string> classi = [];
            foreach (var item in classesFromDb)
            {
                classi.Add(item.Name);
            }
            var classe = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Seleziona una [purple]classe[/]!")
                            .PageSize(10)
                            .MoreChoicesText("[grey](Scorri su e giù per vedere le classi)[/]")
                            .AddChoices(classi));
            
            var classeSel = classesFromDb.Where(r => r.Name.Equals(classe)).FirstOrDefault();
            var spellFromDb = context.Spell.Include(s => s.Classes).ToList();
            List<string> spell = [];
            foreach (var item in spellFromDb.Where(s=>s.Level<2).Where(s=>s.Classes.Contains(classeSel)))
            {
                
                spell.Add(item.Name);
            }
            List<Spell> incSel = null; 
            if (classeSel.ClassSpells != null) {
                List<string> incantesimi = [];
                while (true) { 
                incantesimi = AnsiConsole.Prompt(
                        new MultiSelectionPrompt<string>()
                            .Title("Seleziona massimo 3 [purple]incantesimi[/]")
                            .PageSize(10)
                            .MoreChoicesText("[grey](Scorri su e giù per vedere gli incantesimi)[/]")
                            .InstructionsText("[grey](Premi [blue]<space>[/] per selezionare un incantesimo, [green]<enter>[/] per confermare la lista)[/]")
                            .AddChoices(spell));
            
                    if (incantesimi.Count > 3) { Console.WriteLine("Massimo 3 incantesimi sono ammessi! Rifai la selezione."); }
                    else break;
                }
                incSel = spellFromDb.Where(r => incantesimi.Contains(r.Name)).ToList();
            }

         
            AnsiConsole.MarkupLine("[bold yellow]Adesso tiriamo i dadi per le statistiche! Ecco come funziona:[/]");
            AnsiConsole.MarkupLine("[grey]Tira 4 dadi a 6 facce (4d6)[/]");
            AnsiConsole.MarkupLine("[grey]Ignora (scarta) il risultato più basso.[/]");
            AnsiConsole.MarkupLine("[grey]Somma i tre risultati rimanenti.[/]");
            AnsiConsole.MarkupLine("[grey]Il risultato è il tuo punteggio per una caratteristica.[/]");
            AnsiConsole.MarkupLine("[grey]Scegli a che caratteristica vuoi assegnarlo. Ripeti questo processo 6 volte per ottenere sei punteggi da assegnare.[/]");
            AnsiConsole.WriteLine();

            var punteggio = 0;

            var rolledScores = new List<int>();
            for (int i = 0; i < 6; i++)
            {
                AnsiConsole.WriteLine("");
                AnsiConsole.WriteLine("");
                AnsiConsole.WriteLine("Andiamo con il prossimo!");
                int score = RollAndDisplayStat();
                rolledScores.Add(score);
            }

            rolledScores = [.. rolledScores.OrderByDescending(s => s)];

            AnsiConsole.MarkupLine("[bold green]Ecco i tuoi punteggi![/]");
            AnsiConsole.MarkupLine(string.Join(", ", rolledScores.Select(s => "[yellow]"+s+"[/]")));
            AnsiConsole.MarkupLine("Ora assegnali alle tue caratteristiche.");
            AnsiConsole.WriteLine();

            var stats = new Dictionary<string, int>();
            var availableStats = new List<string> { "Forza", "Destrezza", "Costituzione", "Intelligenza", "Saggezza", "Carisma" };

            foreach (var score in rolledScores)
            {
                var chosenStat = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"A quale caratteristica vuoi assegnare il punteggio [yellow bold]{score}[/]?")
                        .AddChoices(availableStats));

                stats.Add(chosenStat, score);
                availableStats.Remove(chosenStat); 
            }

            CalcoloPersonaggio(context, nome, razzaSel, classeSel, stats, incSel, p);

        }
        public static void CalcoloPersonaggio(AppDbContext context1, string nome, CharacterRace razzaSel, CharacterClass classeSel, Dictionary<string, int> statsBase, IEnumerable<Spell> incantesimiSel, Player p)
        {
            Console.WriteLine("\n--- CALCOLO DELLA SCHEDA DEL PERSONAGGIO ---");

           
            var bonusRazziali = context1.RacialStatBonus
                                        .Where(b => b.Race.Id == razzaSel.Id)
                                        .ToList();

            var statsFinali = new Dictionary<string, int>();
            statsFinali["Forza"] = statsBase["Forza"] + (bonusRazziali.FirstOrDefault(b => b.Type == StatType.Strength)?.BonusValue ?? 0);
            statsFinali["Destrezza"] = statsBase["Destrezza"] + (bonusRazziali.FirstOrDefault(b => b.Type == StatType.Dexterity)?.BonusValue ?? 0);
            statsFinali["Costituzione"] = statsBase["Costituzione"] + (bonusRazziali.FirstOrDefault(b => b.Type == StatType.Constitution)?.BonusValue ?? 0);
            statsFinali["Intelligenza"] = statsBase["Intelligenza"] + (bonusRazziali.FirstOrDefault(b => b.Type == StatType.Intelligence)?.BonusValue ?? 0);
            statsFinali["Saggezza"] = statsBase["Saggezza"] + (bonusRazziali.FirstOrDefault(b => b.Type == StatType.Wisdom)?.BonusValue ?? 0);
            statsFinali["Carisma"] = statsBase["Carisma"] + (bonusRazziali.FirstOrDefault(b => b.Type == StatType.Charisma)?.BonusValue ?? 0);

            var modificatori = new Dictionary<string, int>();
            foreach (var stat in statsFinali)
            {
                
                modificatori[stat.Key] = (int)Math.Floor((stat.Value - 10) / 2.0);
            }

            int modificatoreCostituzione = modificatori["Costituzione"];
            int maxHitPoints = classeSel.HitDice + modificatoreCostituzione;


            var nuovoPersonaggio = new Character
            {
                Player = p,
                Name = nome,
                Level = 1,
                MaxHitPoints = (short)maxHitPoints,
                CurrentHitPoints = (short)maxHitPoints,
                Race = razzaSel,
                Class = classeSel,
                Strength = statsFinali["Forza"],
                Dexterity = statsFinali["Destrezza"],
                Constitution = statsFinali["Costituzione"],
                Intelligence = statsFinali["Intelligenza"],
                Wisdom = statsFinali["Saggezza"],
                Charisma = statsFinali["Carisma"],

                KnownSpells = [],
                Inventory = []
            };

            if (incantesimiSel != null)
            {
                foreach (var spell in incantesimiSel)
                {
                    nuovoPersonaggio.KnownSpells.Add(spell);
                }
            }


            AnsiConsole.MarkupLine("[bold green]========================================[/]");
            AnsiConsole.MarkupLine("[bold green]    SCHEDA PERSONAGGIO CREATA[/]");
            AnsiConsole.MarkupLine("[bold green]========================================[/]");
            AnsiConsole.MarkupLine($"[bold]Nome:[/][yellow]{nuovoPersonaggio.Name}[/] ");
            AnsiConsole.MarkupLine($"[bold]Razza/Classe: [/][green]{razzaSel.Name}[/] [purple]{classeSel.Name}[/] di Livello {nuovoPersonaggio.Level}");
            AnsiConsole.MarkupLine($"[bold red]Punti Ferita: [/][red]{nuovoPersonaggio.MaxHitPoints}[/]");
            AnsiConsole.MarkupLine($"[bold blue]Classe Armatura:[/] [blue]{CalcolaClasseArmatura(nuovoPersonaggio)}[/]");
            Console.WriteLine("--- Caratteristiche ---");

            
            foreach (var stat in statsFinali)
            {
                string modificatoreStr = modificatori[stat.Key] >= 0 ? $"+{modificatori[stat.Key]}" : modificatori[stat.Key].ToString();
                AnsiConsole.MarkupLine($"[bold]{stat.Key,-15}[/]{stat.Value,2} ({modificatoreStr})");
            }

            AnsiConsole.MarkupLine("\n[bold]--- Equipaggiamento ---[/]");
            AnsiConsole.MarkupLine($"[bold]Arma Primaria:[/] \t{nuovoPersonaggio.PrimaryWeapon?.Name ?? "Disarmato"} ([grey]{CalcolaDannoArma(nuovoPersonaggio)}[/])");
            AnsiConsole.MarkupLine($"[bold]Arma Secondaria:[/] {nuovoPersonaggio.SecondaryWeapon?.Name ?? "Mano Libera"}");
            AnsiConsole.MarkupLine($"[bold]Armatura:[/]\t{nuovoPersonaggio.EquippedArmor?.Name ?? "Nessuna"}");


            if (nuovoPersonaggio.KnownSpells.Any())
            {
                AnsiConsole.MarkupLine("[bold aqua]--- Incantesimi Conosciuti ---[/]");
                foreach (var cs in nuovoPersonaggio.KnownSpells)
                {
                    AnsiConsole.MarkupLine($"[aqua]- {cs.Name}[/]");
                }
            }
            AnsiConsole.MarkupLine("[green]========================================[/]");

           


            if (AnsiConsole.Confirm("[bold]Vuoi salvare questo personaggio nel database?[/]"))
            {
                context1.Characters.Add(nuovoPersonaggio);
                context1.SaveChanges();
                AnsiConsole.MarkupLine("[bold green]Personaggio salvato con successo![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[yellow]Creazione personaggio annullata.[/]");
            }
        }
        private static int RollAndDisplayStat()
        {
            int totalScore = 0;

            AnsiConsole.Status()
                .Spinner(Spinner.Known.SquareCorners)
                .SpinnerStyle(Style.Parse("yellow"))
                .Start("Il dado sta rotolando...", ctx =>
                {
                    var rolls = new List<int>();
                    AnsiConsole.MarkupLine($"Premi qualsiasi tasto per fermare il primo dado");
                    Console.ReadKey();
                    for (int j = 0; j < 4; j++)
                    {
                        rolls.Add(random.Next(1, 7));
                        AnsiConsole.MarkupLine($"Risultato {j+1} dado: {rolls[j]}. Premi qualsiasi tasto per fermare il prossimo ");
                        Console.ReadKey();
                    }

                    Thread.Sleep(750);
                    var orderedRolls = rolls.OrderBy(r => r).ToList();
                    int droppedDie = orderedRolls.First(); 

                    totalScore = orderedRolls.Skip(1).Sum();

                    AnsiConsole.MarkupLine($"Tiri: {string.Join(", ", rolls.Select(r => $"[blue]{r}[/]"))} -> ");
                    AnsiConsole.MarkupLine($"Scartiamo il [red]{droppedDie}[/] -> ");
                    AnsiConsole.MarkupLine($"Totale: [green bold]{totalScore}[/]");
                });



            return totalScore;
        }
        public static void RimuoviPersonaggio(AppDbContext context, Player giocatore)
        {
            var myCharacters = context.Characters
                                      .Include(c => c.Player)
                                      .Where(c => c.Player.Id == giocatore.Id)
                                      .ToList();
            if (myCharacters.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]Non hai personaggi da rimuovere.[/]");
                Console.ReadKey();
                return;
            }

            var characterToDelete = AnsiConsole.Prompt(
                new SelectionPrompt<Character>()
                    .Title("[bold red]Quale personaggio vuoi rimuovere PERMANENTEMENTE?[/]")
                    .UseConverter(c => c.Name)
                    .AddChoices(myCharacters));

            
            if (AnsiConsole.Confirm($"[bold red]Sei assolutamente sicuro di voler eliminare {characterToDelete.Name}? Questa azione è irreversibile.[/]"))
            {
                context.Characters.Remove(characterToDelete);
                context.SaveChanges();
                AnsiConsole.MarkupLine($"[green]Il personaggio '{characterToDelete.Name}' è stato eliminato.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[yellow]Rimozione annullata.[/]");
            }
            Console.ReadKey();
        }

        public static void AggiornaPersonaggio(AppDbContext context, Player giocatore)
        {
            var myCharacters = context.Characters
                                      .Include(c => c.Player)
                                      .Where(c => c.Player.Id == giocatore.Id)
                                      .ToList();
            if (myCharacters.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]Non hai personaggi da modificare.[/]");
                Console.ReadKey();
                return;
            }

            var characterToModify = AnsiConsole.Prompt(
                new SelectionPrompt<Character>()
                    .Title("Quale personaggio vuoi modificare?")
                    .UseConverter(c => c.Name)
                    .AddChoices(myCharacters)
                    );


            var newName = AnsiConsole.Ask<string>($"Inserisci il nuovo nome per {characterToModify.Name}:", characterToModify.Name);
            characterToModify.Name = newName;

            context.SaveChanges();
            AnsiConsole.MarkupLine($"[green]Nome del personaggio aggiornato in '{newName}'.[/]");
            Console.ReadKey();
        }

        public static void MostraMenuGiocatore(AppDbContext context, Player giocatore)
        {
            while (true)
            {
                Console.WriteLine();
                AnsiConsole.MarkupLine($"[bold]Menu di {giocatore.FirstName}[/]");
                var scelta = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Cosa vuoi fare?")
                        .AddChoices(new[] {
                            "Visualizza i miei Personaggi",
                            "Crea un nuovo Personaggio",
                            "Modifica un Personaggio",
                            "Rimuovi un Personaggio",
                            "Esci (torna al menu principale)"
                        }));

                switch (scelta)
                {
                    case "Visualizza i miei Personaggi":
                        VisualizzaPersonaggiGiocatore(context, giocatore);
                        break;
                    case "Crea un nuovo Personaggio":
                        InserimentoPersonaggio(context, giocatore);
                        break;
                    case "Modifica un Personaggio":
                        MenuModificaPersonaggio(context, giocatore);
                        break;
                    case "Rimuovi un Personaggio":
                        RimuoviPersonaggio(context, giocatore);
                        break;
                    case "Esci (torna al menu principale)":
                        AnsiConsole.MarkupLine("[yellow]Disconnessione...[/]");
                        return;
                }
            }
        }

        public static void VisualizzaPersonaggiGiocatore(AppDbContext context, Player giocatore)
        {
            AnsiConsole.MarkupLine($"[bold]Elenco dei personaggi di {giocatore.FirstName}:[/]");

            var myCharacters = context.Characters
                                      .Include(c => c.Player)
                                      .Where(c => c.Player.Id == giocatore.Id)
                                      .Include(c => c.Race)
                                      .Include(c => c.Class)
                                      .ToList();

            if (!myCharacters.Any())
            {
                AnsiConsole.MarkupLine("[red]Non hai ancora creato nessun personaggio.[/]");
                Console.ReadKey();
                return;
            }

            var table = new Table().Border(TableBorder.Rounded);
            table.AddColumn("Nome");
            table.AddColumn("Livello");
            table.AddColumn("Razza");
            table.AddColumn("Classe");

            foreach (var character in myCharacters)
            {
                table.AddRow(character.Name, character.Level.ToString(), character.Race.Name, character.Class.Name);
            }

            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine("[grey]Premi un tasto per tornare al menu...[/]");
            Console.ReadKey();
        }

        public static void MostraMenuEnciclopedia(AppDbContext context)
        {
            while (true)
            {
                Console.WriteLine();
                AnsiConsole.MarkupLine("[bold]Enciclopedia di D&D[/]");
                var scelta = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Quale sezione vuoi consultare?")
                        .AddChoices(new[] {
                            "Razze", "Classi", "Incantesimi", "Torna al menu principale"
                        }));

                switch (scelta)
                {
                    case "Razze":
                        MostraDettagliRazze(context);
                        break;
                    case "Classi":
                        MostraDettagliClassi(context);
                        break;
                    case "Incantesimi":
                        MostraDettagliIncantesimi(context);
                        break;
                    case "Torna al menu principale":
                        return;
                }
            }
        }

        public static void MostraDettagliRazze(AppDbContext context)
        {
            var allRaces = context.CharacterRaces.Include(r => r.Traits).ToList();
            var chosenRace = AnsiConsole.Prompt(
                new SelectionPrompt<CharacterRace>()
                    .Title("Seleziona una razza per vederne i dettagli")
                    .UseConverter(r => r.Name)
                    .AddChoices(allRaces));

            var panel = new Panel(
                $"[grey]{chosenRace.Description}[/]" +
                $"[bold]Velocità Base:[/] {chosenRace.BaseSpeed} piedi" +
                $"[bold]Tratti Razziali:[/]" +
                string.Join("\n", chosenRace.Traits.Select(t => $" - {t.Name}"))
            )
            .Header($"[bold green]{chosenRace.Name}[/]")
            .Border(BoxBorder.Rounded);

            AnsiConsole.Write(panel);
            Console.ReadKey();
        }

        public static void MostraDettagliClassi(AppDbContext context)
        {
            var allClasses = context.CharacterClasses.ToList();
            var chosenClass = AnsiConsole.Prompt(
                new SelectionPrompt<CharacterClass>()
                    .Title("Seleziona una classe per vederne i dettagli")
                    .UseConverter(c => c.Name)
                    .AddChoices(allClasses));

            var panel = new Panel(
                $"[grey]{chosenClass.Descritpion}[/]\n\n" +
                $"[bold]Dado Vita:[/] d{chosenClass.HitDice}"
            )
            .Header($"[bold purple]{chosenClass.Name}[/]")
            .Border(BoxBorder.Rounded);

            AnsiConsole.Write(panel);
            Console.ReadKey();
        }

        public static void MostraDettagliIncantesimi(AppDbContext context)
        {
            var allSpells = context.Spell.OrderBy(s => s.Level).ThenBy(s => s.Name).ToList();
            var chosenSpell = AnsiConsole.Prompt(
                new SelectionPrompt<Spell>()
                    .Title("Seleziona un incantesimo per vederne i dettagli")
                    .UseConverter(s => $"[grey](Liv. {s.Level})[/] {s.Name}")
                    .AddChoices(allSpells));

            var panel = new Panel(
                $"[grey]{chosenSpell.Description}[/]\n\n" +
                $"[bold]Livello:[/] {chosenSpell.Level}\n" +
                $"[bold]Tempo di lancio:[/] {chosenSpell.CastingTime}\n" +
                $"[bold]Gittata:[/] {chosenSpell.Range} piedi\n" +
                $"[bold]Durata:[/] {chosenSpell.Duration} secondi"
            )
            .Header($"[bold aqua]{chosenSpell.Name}[/]")
            .Border(BoxBorder.Rounded);

            AnsiConsole.Write(panel);
            Console.ReadKey();
        }

        public static void VisualizzaTuttiPersonaggi(AppDbContext context)
        {
            AnsiConsole.MarkupLine("\n[bold]Elenco di tutti gli avventurieri conosciuti:[/]");
            var allCharacters = context.Characters
                                       .Include(c => c.Race)
                                       .Include(c => c.Class)
                                       .Include(c => c.Player)
                                       .ToList();
            if (!allCharacters.Any())
            {
                AnsiConsole.MarkupLine("[red]Nessun personaggio trovato nel database.[/]");
                Console.ReadKey();
                return;
            }
            var table = new Table().Border(TableBorder.Rounded);
            table.AddColumn("Nome"); table.AddColumn("Livello"); table.AddColumn("Razza"); table.AddColumn("Classe"); table.AddColumn("Giocatore");
            foreach (var character in allCharacters)
            {
                table.AddRow(character.Name, character.Level.ToString(), character.Race.Name, character.Class.Name, $"{character.Player.FirstName}");
            }
            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine("\n[grey]Premi un tasto per tornare al menu...[/]");
            Console.ReadKey();
        }

        public static void MenuModificaPersonaggio(AppDbContext context, Player giocatore)
        {
            var characterToModify = SelezionaPersonaggioGiocatore(context, giocatore, "Quale personaggio vuoi modificare?");
            if (characterToModify == null) return;

            while (true)
            {
                var scelta = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"Cosa vuoi modificare per [yellow]{characterToModify.Name}[/]?")
                        .AddChoices(new[] { "Cambia Nome", "Gestisci Inventario", "Torna indietro", "Gestisci Equipaggiamento" }));

                switch (scelta)
                {
                    case "Cambia Nome":
                        var newName = AnsiConsole.Ask($"Inserisci il nuovo nome:", characterToModify.Name);
                        characterToModify.Name = newName;
                        context.SaveChanges();
                        AnsiConsole.MarkupLine($"[green]Nome aggiornato in '{newName}'.[/]");
                        break;
                    case "Gestisci Inventario":
                        GestisciInventario(context, characterToModify);
                        break;
                    case "Gestisci Equipaggiamento":
                        MenuGestisciEquipaggiamento(context, giocatore.Id);
                        break;
                    case "Torna indietro":
                        return;
                }
            }
        }

        public static void GestisciInventario(AppDbContext context, Character personaggio)
        {
            
            while (true)
            {
                
                var personaggioConInventario = context.Characters.Include(c => c.Inventory).FirstOrDefault(c => c.Id == personaggio.Id);

                if (personaggioConInventario == null)
                {
                    AnsiConsole.MarkupLine("[red]Errore: impossibile trovare il personaggio.[/]");
                    Console.ReadKey();
                    return;
                }

                AnsiConsole.MarkupLine($"\nInventario di [yellow]{personaggio.Name}[/]:");

                if (personaggioConInventario.Inventory == null || !personaggioConInventario.Inventory.Any())
                {
                    AnsiConsole.MarkupLine("[grey]Zaino vuoto.[/]");
                }
                else
                {
                    var itemGroups = personaggioConInventario.Inventory.GroupBy(item => item.Name);
                    foreach (var group in itemGroups)
                    {
                        AnsiConsole.MarkupLine($"- {group.Key} [grey](x{group.Count()})[/]");
                    }
                }
                var scelta = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Cosa vuoi fare?")
                        .AddChoices("Aggiungi Oggetto", "Rimuovi Oggetto", "Indietro"));

                switch (scelta)
                {
                    case "Aggiungi Oggetto":
                        var tuttiGliOggetti = context.Items.ToList();
                        var oggettoDaAggiungere = AnsiConsole.Prompt(
                            new SelectionPrompt<Item>()
                                .Title("Scegli un oggetto da aggiungere:")
                                .UseConverter(i => i.Name)
                                .AddChoices(tuttiGliOggetti));

                        personaggioConInventario.Inventory.Add(oggettoDaAggiungere);
                        context.SaveChanges();

                        AnsiConsole.MarkupLine($"[green]'{oggettoDaAggiungere.Name}' aggiunto all'inventario.[/]");
                        Console.ReadKey();
                        break;

                    case "Rimuovi Oggetto":
                        if (personaggioConInventario.Inventory == null || !personaggioConInventario.Inventory.Any())
                        {
                            AnsiConsole.MarkupLine("[red]Non ci sono oggetti da rimuovere.[/]");
                            Console.ReadKey();
                            continue;
                        }
                        var oggettiUnici = personaggioConInventario.Inventory.GroupBy(item => item).Select(g => g.Key).ToList();
                        var oggettoDaRimuovereScelta = AnsiConsole.Prompt(
                            new SelectionPrompt<Item>()
                                .Title("Scegli un tipo di oggetto da rimuovere:")
                                .UseConverter(i => i.Name)
                                .AddChoices(oggettiUnici));

                        var itemInstanceToRemove = personaggioConInventario.Inventory.FirstOrDefault(i => i.Id == oggettoDaRimuovereScelta.Id);

                        if (itemInstanceToRemove != null)
                        {
                            personaggioConInventario.Inventory.Remove(itemInstanceToRemove);
                            context.SaveChanges();
                            AnsiConsole.MarkupLine($"[red]Una copia di '{oggettoDaRimuovereScelta.Name}' è stata rimossa dall'inventario.[/]");
                        }
                        Console.ReadKey();
                        break;

                    case "Indietro":
                        return;
                }
            }
        }

        public static void MostraMenuMaster(AppDbContext context)
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[bold magenta]-- MODALITÀ MASTER ATTIVATA --[/]");
            while (true)
            {
                var scelta = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Scegli un'azione da Master:")
                        .AddChoices(["Fai salire di livello TUTTI i personaggi", "Esci dalla Modalità Master"]));

                switch (scelta)
                {
                    case "Fai salire di livello TUTTI i personaggi":
                        LevelUpDiGruppo(context);
                        break;
                    case "Esci dalla Modalità Master":
                        AnsiConsole.MarkupLine("[magenta]-- MODALITÀ MASTER DISATTIVATA --[/]");
                        return;
                }
            }
        }
        public static void LevelUpDiGruppo(AppDbContext context)
        {
            var allCharacters = context.Characters.Include(c => c.Class).ToList();
            if (!allCharacters.Any())
            {
                AnsiConsole.MarkupLine("[red]Non ci sono personaggi da far salire di livello.[/]");
                return;
            }

            var livelloAttuale = allCharacters.First().Level;
            if (AnsiConsole.Confirm($"[bold yellow]Sei sicuro di voler far avanzare tutti i {allCharacters.Count} personaggi dal livello {livelloAttuale} al livello {livelloAttuale + 1}?[/]"))
            {
                foreach (var character in allCharacters)
                {
                    character.Level++;

                    int hitDieRoll = random.Next(1, character.Class.HitDice + 1);
                    int constitutionModifier = (int)((character.Constitution - 10) / 2.0);
                    int hpIncrease = Math.Max(1, hitDieRoll + constitutionModifier); 

                    character.MaxHitPoints += (short)hpIncrease;
                    character.CurrentHitPoints = character.MaxHitPoints;

                    AnsiConsole.MarkupLine($"[yellow]{character.Name}[/] sale al livello {character.Level}! Guadagna [green]{hpIncrease}[/] Punti Ferita (Totali: {character.MaxHitPoints}).");
                }

                context.SaveChanges();
                AnsiConsole.MarkupLine("[bold green]Avanzamento di livello completato per tutti i personaggi![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[yellow]Avanzamento di livello annullato.[/]");
            }
            Console.ReadKey();
        }

        public static Character SelezionaPersonaggioGiocatore(AppDbContext context, Player giocatore, string promptTitle)
        {
            var myCharacters = context.Characters.Where(c => c.Player.Id == giocatore.Id).ToList();
            if (!myCharacters.Any())
            {
                AnsiConsole.MarkupLine("[red]Non hai personaggi a cui applicare questa azione.[/]");
                Console.ReadKey();
                return null;
            }

            var characterChoice = AnsiConsole.Prompt(
                new SelectionPrompt<Character>()
                    .Title(promptTitle)
                    .UseConverter(c => c.Name)
                    .AddChoices(myCharacters)
                    );

            return characterChoice;
        }

        public static void MenuGestisciEquipaggiamento(AppDbContext context, int characterId)
        {
            while (true)
            {
                
                var character = context.Characters
                                       .Include(c => c.Inventory)   
                                       .Include(c => c.EquippedArmor)
                                       .Include(c => c.PrimaryWeapon)
                                       .Include(c => c.SecondaryWeapon)
                                       .FirstOrDefault(c => c.Id == characterId);

                if (character == null)
                {
                    AnsiConsole.MarkupLine("[red]Errore: Personaggio non trovato.[/]");
                    return;
                }

               
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine($"[bold]Gestione Equipaggiamento di [yellow]{character.Name}[/][/]");
                AnsiConsole.MarkupLine($"[cyan]1. Armatura:[/] \t\t{character.EquippedArmor?.Name ?? "[grey]Nessuna[/]"}");
                AnsiConsole.MarkupLine($"[cyan]2. Arma Primaria:[/] \t{character.PrimaryWeapon?.Name ?? "[grey]Nessuna[/]"}");
                AnsiConsole.MarkupLine($"[cyan]3. Arma Secondaria:[/] \t{character.SecondaryWeapon?.Name ?? "[grey]Nessuna[/]"}");

               
                var scelta = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("\nSeleziona uno slot da modificare")
                        .AddChoices(new[] { "Armatura", "Arma Primaria", "Arma Secondaria", "[red]Indietro[/]" }));

               
                character.Inventory ??= [];

                switch (scelta)
                {
                    case "Armatura":
                        var armorsInInventory = character.Inventory.Where(item => item.Type == ItemType.Armor).ToList();
                        armorsInInventory.Add(new Item { Id = 0, Name = "[red]Rimuovi Armatura Attuale[/]" });

                        var armorChoice = AnsiConsole.Prompt(
                            new SelectionPrompt<Item>()
                                .Title("Scegli un'armatura dall'inventario")
                                .UseConverter(i => i.Name)
                                .AddChoices(armorsInInventory));

                        character.EquippedArmorId = armorChoice.Id == 0 ? null : armorChoice.Id;
                        break;

                    case "Arma Primaria":
                        var weaponsInInventory = character.Inventory.Where(item => item.Type == ItemType.Weapon).ToList();
                        weaponsInInventory.Add(new Item { Id = 0, Name = "[red]Rimuovi Arma Attuale[/]" });

                        var weaponChoice = AnsiConsole.Prompt(
                            new SelectionPrompt<Item>()
                                .Title("Scegli un'arma dall'inventario")
                                .UseConverter(i => i.Name)
                                .AddChoices(weaponsInInventory));

                        character.PrimaryWeaponId = weaponChoice.Id == 0 ? null : weaponChoice.Id;
                        break;

                    case "Arma Secondaria":
                        var secondaryWeapons = character.Inventory.Where(item => item.Type == ItemType.Weapon).ToList();
                        var shields = character.Inventory.Where(item => item.Type == ItemType.Armor && item.ArmorClass == 2).ToList(); 
                        var secondaryOptions = secondaryWeapons.Concat(shields).ToList();
                        secondaryOptions.Add(new Item { Id = 0, Name = "[red]Rimuovi Oggetto Attuale[/]" });

                        var secondaryChoice = AnsiConsole.Prompt(
                            new SelectionPrompt<Item>()
                                .Title("Scegli un'arma o uno scudo dall'inventario")
                                .UseConverter(i => i.Name)
                                .AddChoices(secondaryOptions));

                        character.SecondaryWeaponId = secondaryChoice.Id == 0 ? null : secondaryChoice.Id;
                        break;

                    case "[red]Indietro[/]":
                        return; 
                }
                context.SaveChanges();
                AnsiConsole.MarkupLine("[green]Equipaggiamento aggiornato![/]");
                Console.ReadKey();
            }
        }

        public static int CalcolaClasseArmatura(Character character)
        {
            int dexterityModifier = (int)((character.Dexterity - 10) / 2.0);

            
            int classeArmatura = 10 + dexterityModifier;

            if (character.EquippedArmor != null && character.EquippedArmor.Type == ItemType.Armor)
            {
                
                int armorBaseAC = character.EquippedArmor.ArmorClass ?? 0;
 
                if (armorBaseAC >= 16)
                {
                    classeArmatura = armorBaseAC;
                }
                else 
                {
                    classeArmatura = armorBaseAC + dexterityModifier;
                }
            }

            if (character.SecondaryWeapon != null && character.SecondaryWeapon.ArmorClass.HasValue)
            {
                if (character.SecondaryWeapon.Type == ItemType.Armor)
                {
                    classeArmatura += character.SecondaryWeapon.ArmorClass.Value;
                }
            }
            return classeArmatura;
        }

        public static string CalcolaDannoArma(Character character)
        {
            if (character.PrimaryWeapon == null || string.IsNullOrEmpty(character.PrimaryWeapon.Damage))
            {
                return "1 + " + (int)((character.Strength - 10) / 2.0) + " Contundente (Pugno)";
            }

            int strengthModifier = (int)((character.Strength - 10) / 2.0);
            string modifierString = strengthModifier >= 0 ? $"+{strengthModifier}" : strengthModifier.ToString();
            return $"{character.PrimaryWeapon.Damage} {modifierString} {character.PrimaryWeapon.DamageType}";
        }


    }
}
