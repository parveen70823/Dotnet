// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using SamuraiApp.UI;

class Program
{
    private static SamuraiContext _context = new SamuraiContext();
    private static SamuraiContext _contextNT = new SamuraiContextNoTracking();

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
    private void AddVariouType()
    {
        var samurai = new Samurai { Name = "Julie" };
        _context.Samurais.Add(samurai);
        var battle = new Battle { Name = "Battle of Okehazama" };
        _context.Battles.Add(battle);
        _context.SaveChanges();
    }
    private static void AddSamurais(params string[] names)
    {
        //var samurai = new Samurai { Name = "new Entry" };
        //_context.Samurais.Add(samurai);
        foreach(string name in names)
        {
            _contextNT.Samurais.Add(new Samurai { Name = name });
            //_context.Samurais.Add(new Samurai { Name = name });
        }
        _context.SaveChanges();
    }
    private static void GetSamurais(string text)
    {
        var samurais = _contextNT.Samurais.ToList();//using as notrackin context
        //var samurais = _context.Samurais.ToList();
        Console.WriteLine($"{text}: Samurai count is {samurais.Count}");
        foreach (var samurai in samurais)
        {
            Console.WriteLine(samurai.Name);
        }
    }
    private static void QueryFilters()
    {
        //var name = "Sampson";
        //var samurais = _context.Samurais.Where(s => s.Name == name).ToList();
        var filters = "J%";
        var samurais = _contextNT.Samurais
            .Where(s=>EF.Functions.Like(s.Name, filters)).ToList();
    }
    private static void QueryAggregates()
    {
        var name = "Sampson";
        var samurai = _context.Samurais.FirstOrDefault(s => s.Name == name);
        var samurai2 = _context.Samurais.Find(2);
        //var samurais = _context.Samurais.OrderBy(s => s.Id).Skip(1).Take(1).ToList(); //skik and take mehtod used in pagination in web
    }
    private static void ReteriveAndUpdateSamurai()
    {
        var samurai = _context.Samurais.FirstOrDefault();
        samurai.Name += "San";
        _context.SaveChanges();
    }
    private static void QueryAndUpdateBattle_Disconnected()
    {
        List<Battle> disconnectedBattles;
        using (var context1 = new SamuraiContext())
        {
            disconnectedBattles = _context.Battles.ToList();
        } //context1 is disposed
        disconnectedBattles.ForEach(b =>
        {
            b.StartDate = new DateTime(1570, 1, 1);
            b.EndDate = new DateTime(1570, 12, 31);
        });
        using(var Context2 = new SamuraiContext())
        {
            Context2.Battles.UpdateRange(disconnectedBattles);
            Context2.SaveChanges();
        }
    }
}
