using System.Text;
using Firebase.Database;
using Firebase.Database.Query;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace StudentFirebaseManagement;

class Program
{

    private const string FIREBASE_DB_URL =
        "https://gameprogramminglanguageclass-default-rtdb.asia-southeast1.firebasedatabase.app/";
    
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        // Initialize Firebase
        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("C:\\Users\\khanh\\Downloads\\gameprogramminglanguageclass.json")
            });
        }

        Console.WriteLine("Firebase Admin SDK connected successfully.");
        Console.WriteLine("Press Enter to start the student management screen...");
        Console.ReadLine();

        bool isRunning = true;

        while (isRunning)
        {
            Console.Clear();
            Console.WriteLine("=========== STUDENT MANAGEMENT ==========");
            Console.WriteLine("1. Add a student");
            Console.WriteLine("2. Add multiple students");
            Console.WriteLine("3. View a student by ID");
            Console.WriteLine("4. Update a student by ID");
            Console.WriteLine("5. Delete a student by ID");
            Console.WriteLine("0. Exit");
            Console.WriteLine("=========================================");
            Console.Write("Your choice: ");

            string input = Console.ReadLine()?.Trim().ToLower();

            switch (input)
            {
                case "1":
                    await AddStudent();
                    break;
                case "2":
                    await AddMultipleStudents();
                    break;
                case "3":
                    await GetStudentById();
                    break;
                case "4":
                    await UpdateStudentById();
                    break;
                case "5":
                    await DeleteStudentById();
                    break;
                case "0":
                case "exit":
                    isRunning = false;
                    Console.WriteLine("\nGoodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            if (isRunning)
            {
                Console.WriteLine("\nPress Enter to return to the menu or type 'exit' to quit...");
                string back = Console.ReadLine()?.Trim().ToLower();
                if (back == "exit" || back == "0") isRunning = false;
            }
        }
    }


    private static async Task AddStudent()
    {
        var firebase = new FirebaseClient(FIREBASE_DB_URL);

        Console.Write("Enter student ID: ");
        int studentID = Int16.Parse(Console.ReadLine());

        Console.Write("Enter student name: ");
        string name = Console.ReadLine();

        Console.Write("Enter student email: ");
        string email = Console.ReadLine();

        Console.Write("Enter student class: ");
        string classAttending = Console.ReadLine();

        Student newStudent = new Student(studentID, name, email, classAttending);

        await firebase.Child("Students").Child(newStudent.StudentID.ToString()).PutAsync(newStudent);

        Console.WriteLine($"Added student with ID: {newStudent.StudentID} successfully");
    }
    
    private static async Task AddMultipleStudents()
    {
        Console.Write("How many students do you want to add? ");
        if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
        {
            Console.WriteLine("Invalid number.");
            return;
        }

        for (int i = 1; i <= count; i++)
        {
            Console.WriteLine($"\n--- Enter details for student {i} ---");
            await AddStudent();
        }

        Console.WriteLine($"\nSuccessfully added {count} students.");
    }


    private static async Task GetStudentById()
    {
        var firebase = new FirebaseClient(FIREBASE_DB_URL);
        
        Console.Write("Enter the student ID to look up: ");
        string studentId = Console.ReadLine();

        try
        {
            var student = await firebase
                .Child("Students")
                .Child(studentId)
                .OnceSingleAsync<Student>();

            if (student == null || string.IsNullOrEmpty(student.Name))
            {
                Console.WriteLine($"No student found with ID: {studentId}");
                return;
            }

            Console.WriteLine("Student Info:");
            Console.WriteLine($"ID: {student.StudentID}");
            Console.WriteLine($"Name: {student.Name}");
            Console.WriteLine($"Email: {student.Email}");
            Console.WriteLine($"CLass: {student.ClassAttending}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error Reading student: {e.Message}");
        }
    }

    private static async Task UpdateStudentById()
    {
        var firebase = new FirebaseClient(FIREBASE_DB_URL);

        Console.WriteLine("Enter the student ID to update: ");
        string studentId = Console.ReadLine();

        try
        {
            var existingStudent = await firebase
                .Child("Students")
                .Child(studentId)
                .OnceSingleAsync<Student>();

            if (existingStudent == null || string.IsNullOrEmpty(existingStudent.Name))
            {
                Console.WriteLine($"No student found with ID: {studentId}");
                return;
            }

            Console.WriteLine($"Current Name: {existingStudent.Name}");
            Console.WriteLine($"Current Email: {existingStudent.Email}");
            Console.WriteLine($"Current Class: {existingStudent.ClassAttending}");

            Console.WriteLine("\nEnter new values (leave blank to keep current):");

            Console.Write("New Name: ");
            string newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName))
                existingStudent.Name = newName;

            Console.Write("New Email: ");
            string newEmail = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newEmail))
                existingStudent.Email = newEmail;

            Console.Write("New Class: ");
            string newClass = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newClass))
                existingStudent.ClassAttending = newClass;

            await firebase
                .Child("Students")
                .Child(studentId)
                .PutAsync(existingStudent);

            Console.WriteLine($"Student with ID {studentId} updated successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error updating student: {e.Message}");
        }
    }
    
    private static async Task DeleteStudentById()
    {
        var firebase = new FirebaseClient(FIREBASE_DB_URL);

        Console.WriteLine("Enter the student ID to delete: ");
        string studentId = Console.ReadLine();

        try
        {
            var student = await firebase
                .Child("Students")
                .Child(studentId)
                .OnceSingleAsync<Student>();

            if (student == null || string.IsNullOrEmpty(student.Name))
            {
                Console.WriteLine($"No student found with ID: {studentId}");
                return;
            }

            Console.WriteLine("Are you sure you want to delete this student? (y/n)");
            Console.WriteLine($"ID: {student.StudentID}, Name: {student.Name}, Email: {student.Email}, Class: {student.ClassAttending}");
            string confirmation = Console.ReadLine()?.Trim().ToLower();

            if (confirmation == "y" || confirmation == "yes")
            {
                await firebase
                    .Child("Students")
                    .Child(studentId)
                    .DeleteAsync();

                Console.WriteLine($"Student with ID {studentId} deleted successfully.");
            }
            else
            {
                Console.WriteLine("Deletion canceled.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting student: {ex.Message}");
        }
    }
}