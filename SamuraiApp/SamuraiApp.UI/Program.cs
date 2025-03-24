// See https://aka.ms/new-console-template for more information
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
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
                                           //AddSamurais("Julie", "Sampson");
                                           //GetSamurais("");
                                           //Console.Write("Press any key...");
                                           //InsertNewSamuraiWithAQuote();
                                           //InsertNewSamuraiWithManyQuotes();
                                           //AddQuoteToExistingSamuraiWhileTracked();
                                           //AddQuoteToExistingSamuraiNotTracked(2);
                                           //EagerLoadSamuraiWithQuotes();
                                           //ProjectSomeProperties();

        ProjectSamuraiWithQuotes();
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
        foreach (string name in names)
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
            .Where(s => EF.Functions.Like(s.Name, filters)).ToList();
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
        using (var Context2 = new SamuraiContext())
        {
            Context2.Battles.UpdateRange(disconnectedBattles);
            Context2.SaveChanges();
        }
    }

    private static void InsertNewSamuraiWithAQuote()
    {
        var samurai = new Samurai
        {
            Name = "kambei Shimada",
            Quotes = new List<Quote>
            {
                new Quote{Text = "I have come to save you"}
            }
        };
        _context.Samurais.Add(samurai);
        _context.SaveChanges();

    }
    private static void InsertNewSamuraiWithManyQuotes()
    {
        var samurai = new Samurai
        {
            Name = " Kyuozo",
            Quotes = new List<Quote>
            {
                new Quote{Text = "Watch out for my sharp sword!"},
                new Quote{Text = " I told you to watch out for the sharp sword! oh well!"}
            }
        };
        _context.Samurais.Add(samurai);
        _context.SaveChanges();
    }
    private static void AddQuoteToExistingSamuraiWhileTracked()
    {
        var samurai = _context.Samurais.FirstOrDefault();
        samurai.Quotes.Add(new Quote
        {
            Text = "I Bet you are happy that i have save yoy"
        });
        _context.SaveChanges();
    }
    private static void AddQuoteToExistingSamuraiNotTracked(int SamuraiId)
    {
        var samurai = _context.Samurais.Find(SamuraiId);
        samurai.Quotes.Add(new Quote
        {
            Text = "Now that i have saved you, will you feed me dinner!"
        });
        using (var newContext = new SamuraiContext())
        {
            newContext.Samurais.Update(samurai);
            newContext.SaveChanges();
        }
    }

    private static void EagerLoadSamuraiWithQuotes()
    {
        var samuraiWithQuotes = _context.Samurais.Include(s => s.Quotes).ToList();
        var splitQuery = _context.Samurais.AsSplitQuery().Include(static s => s.Quotes).ToList();
        var filterInclude = _context.Samurais
            .Include(s => s.Quotes.Where(q => q.Text.Contains("Thanks!"))).ToList();
        var filterPrimaryEntityWithInclude = _context.Samurais
            .Where(s => s.Name.Contains("Sampson"))
            .Include(s => s.Quotes).FirstOrDefault();
    }
    private static void ProjectSomeProperties()
    {
        //var someProperties = _context.Samurais.Select(s => new { s.id, s.Name }).ToList();
        var idAndName = _context.Samurais.Select(s => new IdAndName(s.id, s.Name)).ToList();
    }
    public struct IdAndName
    {
        public IdAndName(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
    private static void ProjectSamuraiWithQuotes()
    {
        var somePropertiesWithQuotes = _context.Samurais
            .Select(s => new
            {
                s.id,
                s.Name,
                QuotesCount = s.Quotes.Count
            }).ToList();
    }

}
