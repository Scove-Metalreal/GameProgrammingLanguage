using Firebase.Database;
using Firebase.Database.Query;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System.Net;
using System.Reactive;
using System.Text;

namespace Lab0
{
    internal class Program
    {
        private static string FIREBASE_DB_URL = "https://gameprogramminglanguageclass-default-rtdb.asia-southeast1.firebasedatabase.app/";
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Firesharp installed successfully");

            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("C:\\Users\\khanh\\Downloads\\gameprogramminglanguageclass.json")
            });

            Console.WriteLine("Connected successfully");

            await AddTestData();
            await ReadTestData();
            await UpdateTestData();
            await ReadTestData();
            await DeleteTestData();
            await ReadTestData();
        }

        public static async Task AddTestData()
        {
            var firebase = new FirebaseClient(FIREBASE_DB_URL);

            var testData = new
            {
                Message = "Hello Firebase!",
                Timestamp = DateTime.UtcNow.ToString("yyy-MM-dd HH:mm:ss")
            };

            await firebase.Child("test").PutAsync(testData);

            Console.WriteLine("Put on firebase successfully!");
        }

        public static async Task ReadTestData()
        {
            var firebase = new FirebaseClient(FIREBASE_DB_URL);

            var testData = await firebase.Child("test").OnceSingleAsync<dynamic>();

            Console.WriteLine($"Message: {testData.Message}");
            Console.WriteLine($"Timestamp: {testData.Timestamp}");

        }

        public static async Task UpdateTestData()
        {
            var firebase = new FirebaseClient(FIREBASE_DB_URL);

            var updatedTestData = new
            {
                Message = "Updated message",
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
            };

            await firebase.Child("test").PatchAsync(updatedTestData);
            Console.WriteLine("Update data successfully");
        }

        public static async Task DeleteTestData()
        {
            var firebase = new FirebaseClient(FIREBASE_DB_URL);

            await firebase.Child("test").DeleteAsync();
            Console.WriteLine("Deleted data successfully");
        }


    }
}
