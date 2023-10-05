using System;

public class School
{
    
    public string SchoolName { get; set; }
    public int SchoolId { get; set; }

    public School(string name, int id)
    {
        SchoolName = name;
        SchoolId = id;
    }

    
    public class Student
    {
        
        public string StudentName { get; set; }
        public int StudentId { get; set; }

        
        public Student(string name, int id)
        {
            StudentName = name;
            StudentId = id;
        }

        public void DisplayStudentInfo()
        {
            Console.WriteLine($"Student Name: {StudentName}, Student ID: {StudentId}");
        }
    }
}

class Program
{
    public static void Main(string[] args)
    {
        
        School school = new School("ABC High School", 1);
        
        School.Student student = new School.Student("John Doe", 101);

        Console.WriteLine($"School Name: {school.SchoolName}, School ID: {school.SchoolId}");
        student.DisplayStudentInfo();
    }
}
