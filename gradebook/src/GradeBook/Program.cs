// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World432!");
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using GradeBook;
using Microsoft.VisualBasic;


// if(args.Length>0){
//     Console.WriteLine($"hello, {args[0]} !");
// }else{
//     Console.WriteLine($"hello");
// }

//using classes
// var book = new InMemoryBook("Grade book");
var book = new DiskBook("New GradeBook");


// book.GradeAdded += OnGradAdded;
// book.GradeAdded += OnGradAdded;
// book.GradeAdded -= OnGradAdded;
// book.GradeAdded += OnGradAdded;

// book.AddGrade(45.5);
// book.AddGrade(80.6);
// book.AddGrade(90.1);
EnterGrades(book);//extracting the method -> way to refactor or rearrange our code.

var stat = book.GetStatistics();

System.Console.WriteLine($"Book name is {book.Name}");
System.Console.WriteLine($"average of the grades is {stat.Average:N1}");
System.Console.WriteLine($"maximum value of grades is {stat.High}");
System.Console.WriteLine($"minimum value of grades is {stat.Low}");
System.Console.WriteLine($"Letter grade is  {stat.Letter}");

static void OnGradAdded(object sender, EventArgs e)
{
    Console.WriteLine("A Grade is added succesfully");
}

static void EnterGrades(Book book)
{
    while (true)
    {
        Console.WriteLine("Enter a grade or press q to exit");
        var input = Console.ReadLine();
        if (input == "q")
        {
            break;
        }
        try
        {
            var grade = double.Parse(input);
            book.AddGrade(grade);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (FormatException ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            // Console.WriteLine("**");
        }

    }
}