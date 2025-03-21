// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System.Security.Cryptography;
using LinqSample.Classes;

//var data = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

///* Query Syntax in LINQ*/
//var QuerySyntax = from obj in data
//                  where obj > 2
//                  select obj;
//foreach(var d in QuerySyntax)
//{
//    Console.WriteLine(d);
//}
//Console.WriteLine("---------------");

///*Query syntax of LINQ*/
//var MethodSyntax = data.Where(obj => obj > 3);
//foreach (var i in MethodSyntax)
//{
//    Console.WriteLine(i);
//}
//Console.WriteLine("---------------");


///*Mixed = query + method()*/
//var MixedMethod = (from obj in data
//                  select obj).Max();
//Console.WriteLine(MixedMethod);




/*IEnumerable=> basically use with the in memory collection and Ienumerable is base class
 * of the collections (list, dictnory...) and these collection inherit the interface ienumerable 
 
 IQuerable => also an interface basically used with the data sources like (enity framwork, sql server,
    ado.net obj...) and iQueryable is the child class of the ienumerable interface
 */


//List<Employee> employees = new List<Employee>()
//{
//    new Employee(){Id=1, Name ="John", Email="test@gmail.com"},
//    new Employee(){Id=2, Name ="smith",Email = "Test2@gmail.com"},
//    new Employee(){Id=3, Name ="new",Email = "Test3@gmail.com"},
//    new Employee(){Id=4, Name ="known",Email = "Test4@gmail.com"},
//    new Employee(){Id=5, Name ="idea",Email = "Test5@gmail.com"}
//};

//IEnumerable<Employee> Query = from emp in employees
//                              where emp.Id == 2
//                              select emp;

//IQueryable<Employee> query2 = employees.AsQueryable().Where(obj => obj.Id == 1);

//foreach(var item in query2)
//{
//    Console.WriteLine("Id is:  "+ item.Id+ " and name is :  "+item.Name);
//}

//var BasicPropQuery = (from emp in employees
//                      select new Employee//for shaping the o/p we can use dto, Same object and any anonoumous class in both query and method syntax
//                      {
//                          Id= emp.Id,
//                          Email = emp.Email
//                      })
//                      .ToList();//Tolist() execute this query immediately

/////Operation in LINQ

//var BasicPropMethod = employees.Select(x=> x.Id.ToString()).ToList();
//var que = employees.Select((emp, index) => new//can also query the obj as pair with index
//{
//    Index = index,
//    Fullname = emp.Name
//}).ToList();

//foreach (var item in BasicPropQuery)
//{
//    Console.WriteLine("Id is: " + item.Id + " and Name is :  " + item.Name+" Email= "+item.Email);
//}


///SelectMany is SelectMany is used to flatten collections within collections (nested collections) and 
///return a single sequence of elements. It is useful when working with nested lists, arrays, or child collections.
//class Employee
//{
//    public string Name { get; set; }
//    public List<string> Skills { get; set; }
//}

//var employees = new List<Employee>()
//{
//    new Employee { Name = "John", Skills = new List<string> { "C#", "SQL" } },
//    new Employee { Name = "Alice", Skills = new List<string> { "Java", "Python" } }
//};

//// Using SelectMany to extract all skills into a single list
//var allSkills = employees.SelectMany(e => e.Skills);

//foreach (var skill in allSkills)
//{
//    Console.WriteLine(skill);
//}
/* Output:
C#
SQL
Java
Python
*/


///Filtereing => 1. Where 2. OfType(in method)/ is(in Querysyntax)

var dataSource = new List<Object>() { "new", "object", "thing", 1, 2, 3, 4 };
var ofTypeExp = dataSource.OfType<string>().ToList();
var isEXp = (from x in dataSource
            where x is int
            select x).ToList();
Console.WriteLine("s");


///sortin in linq=> 1. OrderBy 2. OrderByDescending 3.ThenBY  4. ThenByDescending 5. Reverse
///The Reverse() method in LINQ is used to reverse the order of elements in a sequence. It works with both 
///lists and queries but executes in memory, meaning it does not optimize performance for databases (e.g., Entity Framework).

//var numbers = new List<int> { 1, 2, 3, 4, 5 };
//var reversedNumbers = numbers.Reverse(); // This does NOT return a new list


///Quantifiers=> are used on the data source to check if some or all elements of the data sources satisfy a condition or not.
///1. All() => this check all the elements satisfy the following condition or not return=> Boolean value true/false.
///2.Any() => atleast one value of the datasource satisfy a specific condition. REturn=> True/false
///3.Contains() => used to check whether  sequence contains a specific element Return=>True/False
///For objects Contains only check refernce . to work with value we have to do some extra things(Comparer concept)


