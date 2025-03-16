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
    }
}
