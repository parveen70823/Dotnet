// See https://aka.ms/new-console-template for more information
using Microsoft.IdentityModel.Tokens;
using SamuraiApp.Data;
using SamuraiApp.Domain;

class Program
{
    private static SamuraiContext _context = new SamuraiContext();

    private static void Main(string[] args)
    {
        _context.Database.EnsureCreated(); //This will cause EF Core to read the provider and connection string defined in
        //the context class, and then go look to see if the database exists
        //GetSamurais("Before Add:");
        //AddSamurai();
        //GetSamurais("After Add:");
        AddSamurais("Julie", "Sampson");
        GetSamurais("");
        Console.Write("Press any key...");
        Console.ReadKey();
    }
    private static void AddSamurais(params string[] names)
    {
        //var samurai = new Samurai { Name = "new Entry" };
        //_context.Samurais.Add(samurai);
        foreach(string name in names)
        {
            _context.Samurais.Add(new Samurai { Name = name });
        }
        _context.SaveChanges();
    }
    private static void GetSamurais(string text)
    {
        var samurais = _context.Samurais.ToList();
        Console.WriteLine($"{text}: Samurai count is {samurais.Count}");
        foreach (var samurai in samurais)
        {
            Console.WriteLine(samurai.Name);
        }
    }
}
