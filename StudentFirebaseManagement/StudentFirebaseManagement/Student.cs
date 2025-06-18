namespace StudentFirebaseManagement;

public class Student
{
    public int StudentID{ get; set;  }
    public string Name { get; set; }
    public string Email { get; set; }
    public string ClassAttending { get; set; }

    public Student(int id, string name, string email, string classAttending)
    {
        this.StudentID = id;
        this.Name = name;
        this.Email = email;
        this.ClassAttending = classAttending;
    }

    public override string ToString()
    {
        return $"Student ID: {StudentID} - Name: {Name}\nEmail: {Email}\nClass: {ClassAttending}";
    }
}