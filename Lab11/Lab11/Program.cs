using System.Text;
using Firebase.Database;
using Firebase.Database.Query;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;

namespace Lab11;

class Program
{
    private const string FIREBASE_DB_URL =
        "https://gameprogramminglanguageclass-default-rtdb.asia-southeast1.firebasedatabase.app/";
    
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(
                    "C:\\Users\\khanh\\Downloads\\gameprogramminglanguageclass-firebase-adminsdk-fbsvc-be73d384a9.json")
            });
        }

        var firebase = new FirebaseClient(FIREBASE_DB_URL);

        List<Player> players = await LoadPlayers();
        
        // Task 1:
        var richPlayers = (from p in players
            where p.Gold > 1000 && p.Coins > 100
            orderby p.Gold descending
            select new
            {
                p.Name, p.Gold, p.Coins
            }).ToList();

        foreach (var player in richPlayers)
        {
            Console.WriteLine($"Name: { player.Name}, Gold: { player.Gold},Coins: { player.Coins}");
        }

        await firebase.Child("quiz_bai1_richPLayers").PutAsync(richPlayers);

        Console.WriteLine("========================");
        
        
        // Task 2:
        var totalVip = players.Count(p => p.VipLevel > 0);
        Console.WriteLine("Number of VIP players: " + totalVip);

        var vipByRegion = from p in players
            where p.VipLevel > 0
            group p by p.Region
            into g
            select new
            {
                Region = g.Key, 
                Count = g.Count()
            };
        foreach (var player in vipByRegion)
        {
            Console.WriteLine($"{player.Region}: {player.Count} VIP players");
        }


        DateTime now = new DateTime(2025, 06, 30, 0, 0, 0);

        var recentVipPlayers = players
            .Where(p => p.VipLevel > 0 && (now - p.LastLogin).TotalDays <= 2)
            .Select(p => new
            {
                p.Name, p.VipLevel, p.LastLogin
            })
            .ToList();

        foreach (var player in recentVipPlayers)
        {
            Console.WriteLine("Name: " + player.Name + " VIP level: " + player.VipLevel + " Last login: " + player.LastLogin);
        }
        
        await firebase.Child("quiz_bai2_recentVipPlayers").PutAsync(recentVipPlayers);

        Console.WriteLine();
    }

    static async Task<List<Player>> LoadPlayers()
    {
        string url = "https://raw.githubusercontent.com/NTH-VTC/OnlineDemoC-/refs/heads/main/simple_players.json";
        HttpClient httpClient = new HttpClient();

        var json = await httpClient.GetStringAsync(url);
        return JsonConvert.DeserializeObject<List<Player>>(json);
    }
}