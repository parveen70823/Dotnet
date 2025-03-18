using Microsoft.EntityFrameworkCore;

namespace DbOperationsWithEFCoreApp.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            //Dbcontext class provide options which helps in connect our db to the application
            //using base class we can send the values from child constructor to parent constructor.
        {
        }

        //for seeding data in master table we can use 3 methods 1.using sql file 
        //2. writing insertion code and calling it
        //3. using dbcontext class for insertion usecase-> we don't have headache to call explicitly
        // by defalt it is called itself and our table will populated itself. we are using 3rd one.below
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>().HasData(
                new Currency() { Id = 1, Title = "INR", Description = "Indian INR" },
                new Currency() { Id = 2, Title = "Dollar", Description = "Dollar" },
                new Currency() { Id = 3, Title = "Euro", Description = "Euro" },
                new Currency() { Id = 4, Title = "Dinar", Description = "Dinar" }
                );

            modelBuilder.Entity<Language>().HasData(
                new Language() { Id = 1, Title = "Hindi", Description = "Hindi" },
                new Language() { Id = 2, Title = "Tamil", Description = "Tamil" },
                new Language() { Id = 3, Title = "Punjabi", Description = "Punjabi" },
                new Language() { Id = 4, Title = "Urdu", Description = "Urdu" }
                );
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<BookPrice> BookPrices { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Author> Authors { get; set; }

    }
}
