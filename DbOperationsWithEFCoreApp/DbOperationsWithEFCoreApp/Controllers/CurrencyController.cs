using DbOperationsWithEFCoreApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DbOperationsWithEFCoreApp.ApiResposeDto;

namespace DbOperationsWithEFCoreApp.Controllers
{
    //[Route("api/[Controller]")]
    [Route("api/currencies")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public CurrencyController(AppDbContext appDbContext)
        //in this constr. we are using dependency
        //injection of dbcontext object and this context is already registerd in program.cs
        //2nd way of this is using new keyword for the class
        {
            _appDbContext = appDbContext;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllCurrencies() {
            //var result = _appDbContext.Currencies.ToList(); //tolist() is actually under the linq
            //simplest way to do linq query in api 
            //var result = (from currencies in _appDbContext.Currencies
            //             select currencies).ToList();//For complex queries this method of writing of linq
            // queries we will be using



            /*usually in db calls we will use async await becase in async function thread will do another
            task untill it will fetch the data which will improve performance and handle many request.*/
            //var result = await _appDbContext.Currencies
            //    .Select(x => new 
            //    {
            //        CurrId = x.Id,
            //        CurrName = x.Title
            //    }).ToListAsync();
            //ToListAsync()-> always return collection ([{},{}...])
            var result = await (from currencies in _appDbContext.Currencies
                                select new
                                {
                                    currId = currencies.Id,
                                    currTitle = currencies.Title
                                }).AsNoTracking().ToListAsync();
            //use this AsNoTracking() method in scenario when we only read bulk data because this function disable the tracking of the state of the entity in efcore which save resources.

            return Ok(result);//generlly we will use diff class for result for that we will use auto
                              //mapper which map field from the table class.
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCurrencyByIdAsync([FromRoute] int id)
        //in async prog generally we add suffix Async for better understanding
        {
            var result = await _appDbContext.Currencies.FindAsync(id);
            return Ok(result);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetCurrencyByNameAsync([FromRoute] string name, [FromQuery] string? description)
        {   /*In firstasnc() we get data if record present in db. if  data not found then we get exception in api response
             but in firstORDefault() case we don't get error but get null or no data if there is not any data found.
            in this context singleasync() and signgleOrDeafult() do same.

            only diff btwn first() and single() methods that single() method only give gata if the
            record is unique or only 1 entry on the basis of condition and in all other cases
            it throw error.
            
             for increasing performance remove where condition and put it inside the first(----)
            as soon it get its first entrt it will return. It willl not iterate all table. */

            //var result =await _appDbContext.Currencies.Where(x => x.Title == name).FirstAsync();
            //var result =await _appDbContext.Currencies.Where(x => x.Title == name).SingleOrDefaultAsync();

            /*  using multiple condition to get single record  */
            var result = await _appDbContext.Currencies
                .FirstOrDefaultAsync(x =>
                x.Title == name //*** below concept -> if descr. is null then only check title condition and if not null then check 2nd condition
                && (string.IsNullOrEmpty(description) || x.Description == description)
                );

            /* using multiple condition to get  multiple records */
            //var result = await _appDbContext.Currencies
            //    .Where(x =>
            //    x.Title == name
            //    && (string.IsNullOrEmpty(description) || x.Description == description)
            //    ).ToListAsync();
            //we can use where() after toList() function but it will decrease performance
            return Ok(result);
        }


        /*Get Records based on Ids [1,2,3....n] from body parameter in efcore*/
        [HttpPost("all")]
        public async Task<IActionResult> GetCurrenciesForIdsAsync([FromBody] List<int> ids)
        {
            var result = await _appDbContext.Currencies
                .Where(x=> ids.Contains(x.Id))
                .Select(x=> new CurrencyDto//In this LINQ query we are specifying which columns we want
                                           // to send and mapped to a dto class & can also use new anonoumous obj.
                                           //in sql server profiler tool we can this specific mapped sql query.
                {
                    CurrencyId = x.Id,
                    CurrencyName = x.Title
                }).ToListAsync();
            return Ok(result);
        }
    }
}
