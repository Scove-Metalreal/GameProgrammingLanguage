using System.Text;
using Firebase.Database;
using Firebase.Database.Query;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace SEMI_FINAL;

class Program
{
    private const string FIREBASE_DB_URL = "https://semi-final-c6eb9-default-rtdb.asia-southeast1.firebasedatabase.app/";
    
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        // Initialize Firebase
        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("D:\\Scove Folder\\GameProgrammingLanguage\\SEMI_FINAL\\SEMI_FINAL\\semi-final.json")
            });
        }

        Console.WriteLine("Firebase Admin SDK connected successfully.");
        Console.WriteLine("Press Enter to start the student management screen...");
        Console.ReadLine();

        bool isRunning = true;

        while (isRunning)
        {
            Console.Clear();
            Console.WriteLine("=========== PLAYER MANAGER ==========");
            Console.WriteLine("1. Add a player");
            Console.WriteLine("2. Add multiple players");
            Console.WriteLine("3. Display list of all players");
            Console.WriteLine("4. Update a player info");
            Console.WriteLine("5. Delete a player");
            Console.WriteLine("6. Get top gold");
            Console.WriteLine("7. Get top score");
            Console.WriteLine("0. Exit");
            Console.WriteLine("=========================================");
            Console.Write("Enter your choice: ");

            string input = Console.ReadLine()?.Trim().ToLower();

            switch (input)
            {
                case "1":
                    await AddPlayer();
                    await DisplayListOfPlayers();
                    break;
                case "2":
                    await AddMultiplePlayers();
                    await DisplayListOfPlayers();
                    break;
                case "3":
                    await DisplayListOfPlayers();
                    break;
                case "4":
                    await UpdatePlayerInfo();
                    await DisplayListOfPlayers();
                    break;
                case "5":
                    await DeletePlayer();
                    await DisplayListOfPlayers();
                    break;
                case "6":
                    await GetTopGold();
                    break;
                case "7":
                    await GetTopScore();
                    break;
                case "0":
                case "exit":
                    isRunning = false;
                    Console.WriteLine("\nGoodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid selection. Please try again.");
                    break;
            }
            if (isRunning)
            {
                Console.WriteLine("\nEnter any key to continue...");
                Console.ReadKey();
            }
        }
    }

    private static async Task AddPlayer()
    {
        var firebase = new FirebaseClient(FIREBASE_DB_URL);
        
        
        string playerID = null;
        while (true)
        {
            Console.Write("Enter Player ID: ");
            playerID = Console.ReadLine();

            try
            {
                var currentPlayer = await firebase
                    .Child("Players")
                    .Child(playerID)
                    .OnceSingleAsync<Player>();

                if (currentPlayer == null || string.IsNullOrEmpty(currentPlayer.Name))
                {
                    break;
                }

                Console.WriteLine("Player Id was already been used. please enter another Id");
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Error fetching from database!" + e.Message);
            }
        }
        

        Console.Write("Enter Player name: ");
        string name = Console.ReadLine();

        Console.Write("Enter gold quantity: ");
        int gold = int.Parse(Console.ReadLine());

        Console.Write("Enter Player's score: ");
        int score = int.Parse(Console.ReadLine());

        Player newPlayer = new Player(playerID, name, gold, score);

        await firebase.Child("Players").Child(newPlayer.PlayerID).PutAsync(newPlayer);

        Console.WriteLine($"Added new Player with ID: {newPlayer.PlayerID} successfully");
    }
    
    private static async Task AddMultiplePlayers()
    {
        Console.Write("How many PLayers do you want to add? ");
        if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
        {
            Console.WriteLine("Invalid number.");
            return;
        }

        for (int i = 1; i <= count; i++)
        {
            Console.WriteLine($"\n--- Enter details for Player {i} ---");
            await AddPlayer();
        }

        Console.WriteLine($"\nSuccessfully added {count} PLayers.");
    }

    private static async Task DisplayListOfPlayers()
    {
        var  firebase = new FirebaseClient(FIREBASE_DB_URL);
        
        var myPlayers = await firebase.Child("Players").OnceAsync<Player>();

        var allPlayers = myPlayers.Select(s => s.Object).ToList();

        Console.WriteLine("------  List of all players  ------");
        foreach (var VARIABLE in allPlayers) 
        {
            Console.WriteLine("Player ID: " + VARIABLE.PlayerID);
            Console.WriteLine("Player name: " + VARIABLE.Name);
            Console.WriteLine("Gold: " + VARIABLE.Gold);
            Console.WriteLine("Score: " + VARIABLE.Score);
            Console.WriteLine();
        }
        Console.WriteLine("------  ------");
    }

    private static async Task UpdatePlayerInfo()
    {
        var firebase = new FirebaseClient(FIREBASE_DB_URL);

        Console.WriteLine("Enter Player ID: ");
        string playerID = Console.ReadLine();

        try
        {
            var currentPlayer = await firebase
                .Child("Players")
                .Child(playerID)
                .OnceSingleAsync<Player>();

            if (currentPlayer == null || string.IsNullOrEmpty(currentPlayer.Name))
            {
                Console.WriteLine("Player not exists");
                return;
            }

            Console.WriteLine($"--- Current player information ---");
            Console.WriteLine("Player ID: " + currentPlayer.PlayerID);
            Console.WriteLine("Player name: " + currentPlayer.Name);
            Console.WriteLine("Gold: " + currentPlayer.Gold);
            Console.WriteLine("Score: " + currentPlayer.Score);

            bool isRunning = true;
            while (isRunning)
            {
                Console.WriteLine("1. Update Player's gold");
                Console.WriteLine("2. Update Player's score");
                Console.WriteLine("0. Exit");
                Console.WriteLine("Enter your selection: ");

                string selection = Console.ReadLine()?.Trim().ToLower();

                switch (selection)
                {
                    case "1":
                        Console.WriteLine("Enter new Player's gold value: ");
                        string newGoldValue = Console.ReadLine();
                        if (!string.IsNullOrEmpty(newGoldValue))
                            currentPlayer.Gold = int.Parse(newGoldValue);
                        break;
                    case "2":
                        Console.WriteLine("Enter new Player's score value: ");
                        string newScoreValue = Console.ReadLine();
                        if (!string.IsNullOrEmpty(newScoreValue))
                            currentPlayer.Score = int.Parse(newScoreValue);
                        break;
                    case "0":
                    case "exit":
                        isRunning = false;
                        Console.WriteLine("Exiting!");
                        break;
                    default:
                        Console.WriteLine("Invalid choose. Please enter again.");
                        break;
                }

                await firebase.Child("Players").Child(currentPlayer.PlayerID).PutAsync(currentPlayer);

                Console.WriteLine("Update player successfully!");
                
                if (isRunning)
                {
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error fetching from database!" + e.Message);
        }
    }

    private static async Task DeletePlayer()
    {
        var firebase = new FirebaseClient(FIREBASE_DB_URL);

        Console.WriteLine("Enter Player ID: ");
        string playerId =  Console.ReadLine();

        try
        {
            var currentPlayer = await firebase
                .Child("Players")
                .Child(playerId)
                .OnceSingleAsync<Player>();
            
            if (currentPlayer == null || string.IsNullOrEmpty(currentPlayer.Name))
            {
                Console.WriteLine("Player not exists");
                return;
            }
            
            Console.WriteLine(currentPlayer);
            Console.Write("Are you sure want to delete this player? (y/n)");
            string confirmation = Console.ReadLine()?.Trim().ToLower();

            if (confirmation == "y" || confirmation == "yes")
            {
                await firebase
                    .Child("Players")
                    .Child(playerId)
                    .DeleteAsync();

                Console.WriteLine($"PLayer with ID {playerId} was deleted successfully.");
            }
            else
            {
                Console.WriteLine("Cancel deletion.");
            }
        } 
        catch (Exception e)
        {
            Console.WriteLine("Error fetching from database!" + e.Message);
        }
    }
    
    private static async Task GetTopGold()
    {
        var firebase = new FirebaseClient(FIREBASE_DB_URL);
        var rawPlayers = await firebase.Child("Players").OnceAsync<Player>();

        var top5 = rawPlayers.OrderByDescending(p => p.Object.Gold)
            .Take(5)
            .Select((x, pos) => new
            {
                Player = x.Object,
                Rank = pos + 1
            })
            .ToList();
        
        string stamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

        foreach (var VARIABLE in top5)
        {
            await firebase.Child("TopGold")
                .Child(VARIABLE.Player.PlayerID)
                .PutAsync(new
                {
                    gold = VARIABLE.Player.Gold,
                    score = VARIABLE.Player.Score,
                    name = VARIABLE.Player.Name,
                    index = VARIABLE.Rank,
                    timestamp = stamp,
                });
        }
        
        Console.WriteLine("------  TOP GOLD  ------");
        foreach (var VARIABLE in top5) 
        {
            Console.WriteLine("Player ID: " + VARIABLE.Player.PlayerID);
            Console.WriteLine("Player name: " + VARIABLE.Player.Name);
            Console.WriteLine("Gold: " + VARIABLE.Player.Gold);
            Console.WriteLine("Score: " + VARIABLE.Player.Score);
            Console.WriteLine("Index: " + VARIABLE.Rank);
            Console.WriteLine();
        }
        Console.WriteLine("------  ------");
    }
    
    private static async Task GetTopScore()
    {
        var firebase = new FirebaseClient(FIREBASE_DB_URL);
        var rawPlayers = await firebase.Child("Players").OnceAsync<Player>();

        var top5 = rawPlayers.OrderByDescending(p => p.Object.Score)
            .Take(5)
            .Select((x, pos) => new
            {
                Player = x.Object,
                Rank = pos + 1
            })
            .ToList();
        
        string stamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

        foreach (var VARIABLE in top5)
        {
            await firebase.Child("TopScore")
                .Child(VARIABLE.Player.PlayerID)
                .PutAsync(new
                {
                    gold = VARIABLE.Player.Gold,
                    score = VARIABLE.Player.Score,
                    name = VARIABLE.Player.Name,
                    index = VARIABLE.Rank,
                    timestamp = stamp,
                });
        }
        
        Console.WriteLine("------  TOP SCORE  ------");
        foreach (var VARIABLE in top5) 
        {
            Console.WriteLine("Player ID: " + VARIABLE.Player.PlayerID);
            Console.WriteLine("Player name: " + VARIABLE.Player.Name);
            Console.WriteLine("Gold: " + VARIABLE.Player.Gold);
            Console.WriteLine("Score: " + VARIABLE.Player.Score);
            Console.WriteLine("Index: " + VARIABLE.Rank);
            Console.WriteLine();
        }
        Console.WriteLine("------  ------");
    }
}