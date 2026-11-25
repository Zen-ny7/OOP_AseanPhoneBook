
using System;
using System.Collections.Generic;
using System.Linq;

// CLASS 1: STUDENT
class Student
{
    public string StudentNumber;
    public string Surname;
    public string FirstName;
    public string Occupation;
    public string Gender;
    public int CountryCode;
    public int AreaCode;
    public string PhoneNumber;

    public Student(string number, string surname, string firstName,
        string occupation, string gender, int countryCode,
        int areaCode, string phoneNumber)
    {
        StudentNumber = number;
        Surname = surname;
        FirstName = firstName;
        Occupation = occupation;
        Gender = gender;
        CountryCode = countryCode;
        AreaCode = areaCode;
        PhoneNumber = phoneNumber;
    }

    public string GetFullPhoneNumber()
    {
        return CountryCode + "-" + AreaCode + "-" + PhoneNumber;
    }
}

// CLASS 2: ASEAN PHONEBOOK
class AseanPhonebook
{
    public List<Student> Students = new List<Student>();

    public void AddStudent(Student student)
    {
        Students.Add(student);
    }

    public Student? FindStudent(string number)
    {
        return Students.Find(s => s.StudentNumber == number);
    }

    public void SearchByCountry(List<int> codes)
    {
        var results = Students
            .Where(s => codes.Contains(s.CountryCode))
            .OrderBy(s => s.Surname)
            .ToList();

        if (results.Count == 0)
        {
            Console.WriteLine("No students found.");
            return;
        }

        foreach (Student s in results)
        {
            Console.WriteLine(
                $"{s.Surname}, {s.FirstName} - {s.StudentNumber}, " +
                $"{s.Occupation}, {s.GetFullPhoneNumber()}");
        }
    }
}

// CLASS 3: PROGRAM
class Program
{
    static AseanPhonebook phonebook = new AseanPhonebook();

    static bool ValidCountryCode(int code)
    {
        return code == 60 || code == 62 || code == 63 ||
               code == 65 || code == 66;
    }

    static int ReadNumber(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? "";

            if (int.TryParse(input, out int number))
                return number;

            Console.WriteLine("Invalid number. Try again.");
        }
    }

    static void Main()
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n===== ASEAN PHONEBOOK =====");
            Console.WriteLine("[1] Store to ASEAN phonebook");
            Console.WriteLine("[2] Edit entry");
            Console.WriteLine("[3] Search by country");
            Console.WriteLine("[4] Exit");
            Console.Write("Enter choice: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    StoreStudent();
                    break;

                case "2":
                    EditStudent();
                    break;

                case "3":
                    SearchStudents();
                    break;

                case "4":
                    running = false;
                    Console.WriteLine("Goodbye!");
                    break;

                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }
        }
    }

    // ADD STUDENT
    static void StoreStudent()
    {
        string again = "Y";

        do
        {
            Console.Write("\nEnter student number: ");
            string number = Console.ReadLine() ?? "";

            if (phonebook.FindStudent(number) != null)
            {
                Console.WriteLine("Student number already exists.");
                continue;
            }

            Console.Write("Enter surname: ");
            string surname = Console.ReadLine() ?? "";

            Console.Write("Enter first name: ");
            string firstName = Console.ReadLine() ?? "";

            Console.Write("Enter occupation: ");
            string occupation = Console.ReadLine() ?? "";

            Console.Write("Enter gender (M/F): ");
            string gender = (Console.ReadLine() ?? "").ToUpper();

            while (gender != "M" && gender != "F")
            {
                Console.Write("Enter M or F only: ");
                gender = (Console.ReadLine() ?? "").ToUpper();
            }

            int countryCode = ReadNumber("Enter country code: ");

            while (!ValidCountryCode(countryCode))
            {
                Console.WriteLine("Invalid country code.");
                countryCode = ReadNumber("Try again: ");
            }

            int areaCode = ReadNumber("Enter area code: ");

            Console.Write("Enter phone number: ");
            string phone = Console.ReadLine() ?? "";

            Student student = new Student(
                number, surname, firstName, occupation,
                gender, countryCode, areaCode, phone
            );

            phonebook.AddStudent(student);

            Console.WriteLine("Student successfully added!");

            Console.Write("Add another entry? (Y/N): ");
            again = (Console.ReadLine() ?? "").ToUpper();

        } while (again == "Y");
    }

    // EDIT STUDENT
    static void EditStudent()
    {
        Console.Write("\nEnter student number: ");
        string number = Console.ReadLine() ?? "";

        Student? s = phonebook.FindStudent(number);

        if (s == null)
        {
            Console.WriteLine("Student not found!");
            return;
        }

        bool editing = true;

        while (editing)
        {
            Console.WriteLine("\nExisting information:");
            Console.WriteLine($"{s.FirstName} {s.Surname}");
            Console.WriteLine($"Phone: {s.GetFullPhoneNumber()}");

            Console.WriteLine("\n===== EDIT MENU =====");
            Console.WriteLine("[1] Student number");
            Console.WriteLine("[2] Surname");
            Console.WriteLine("[3] Gender");
            Console.WriteLine("[4] Occupation");
            Console.WriteLine("[5] Country code");
            Console.WriteLine("[6] Area code");
            Console.WriteLine("[7] Phone number");
            Console.WriteLine("[8] Back to main menu");
            Console.Write("Enter choice: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    Console.Write("New student number: ");
                    string newNumber = Console.ReadLine() ?? "";

                    Student? existing = phonebook.FindStudent(newNumber);

                    if (existing == null || existing == s)
                    {
                        s.StudentNumber = newNumber;
                        Console.WriteLine("Student number updated.");
                    }
                    else
                    {
                        Console.WriteLine("Student number already exists.");
                    }
                    break;

                case "2":
                    Console.Write("New surname: ");
                    s.Surname = Console.ReadLine() ?? "";
                    break;

                case "3":
                    Console.Write("New gender (M/F): ");
                    string gender = (Console.ReadLine() ?? "").ToUpper();

                    if (gender == "M" || gender == "F")
                        s.Gender = gender;
                    else
                        Console.WriteLine("Invalid gender.");
                    break;

                case "4":
                    Console.Write("New occupation: ");
                    s.Occupation = Console.ReadLine() ?? "";
                    break;

                case "5":
                    int code = ReadNumber("New country code: ");

                    if (ValidCountryCode(code))
                        s.CountryCode = code;
                    else
                        Console.WriteLine("Invalid country code.");
                    break;

                case "6":
                    s.AreaCode = ReadNumber("New area code: ");
                    break;

                case "7":
                    Console.Write("New phone number: ");
                    s.PhoneNumber = Console.ReadLine() ?? "";
                    break;

                case "8":
                    editing = false;
                    break;

                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }
        }
    }

    // SEARCH STUDENTS
    static void SearchStudents()
    {
        List<int> codes = new List<int>();

        while (true)
        {
            Console.WriteLine("\n===== SEARCH BY COUNTRY =====");
            Console.WriteLine("[1] Philippines");
            Console.WriteLine("[2] Thailand");
            Console.WriteLine("[3] Singapore");
            Console.WriteLine("[4] Indonesia");
            Console.WriteLine("[5] Malaysia");
            Console.WriteLine("[6] ALL");
            Console.WriteLine("[0] No More");
            Console.Write("Enter choice: ");

            string choice = Console.ReadLine() ?? "";

            if (choice == "0")
                break;

            if (choice == "6")
            {
                codes = new List<int> { 60, 62, 63, 65, 66 };
                break;
            }

            int code = 0;

            switch (choice)
            {
                case "1":
                    code = 63;
                    break;

                case "2":
                    code = 66;
                    break;

                case "3":
                    code = 65;
                    break;

                case "4":
                    code = 62;
                    break;

                case "5":
                    code = 60;
                    break;

                default:
                    Console.WriteLine("Invalid choice!");
                    continue;
            }

            if (!codes.Contains(code))
                codes.Add(code);
        }

        Console.WriteLine("\n===== SEARCH RESULTS =====");
        phonebook.SearchByCountry(codes);
    }
}
