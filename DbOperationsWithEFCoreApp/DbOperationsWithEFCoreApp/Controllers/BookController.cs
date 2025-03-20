using DbOperationsWithEFCoreApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DbOperationsWithEFCoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController(AppDbContext appDbcontext) : ControllerBase
        //here we use Primary constructor new feature of dotnet8 and in currency contrller we use depedency injection by legacy contructor.
    {
        [HttpGet("")]
        public async Task<IActionResult> GetAllBooksAsync()
        {
            /*getting relaed tables data using navigational property in efcore*/

            //var result = await appDbcontext.Books.Select(x => new
            //{
            //    Id = x.Id,
            //    Title = x.Title,
            //    Author = x.Author != null ? x.Author.Name : "NA",
            //    Language = x.Language
            //}).ToListAsync();

            /* Eager loading */
            //var result = await appDbcontext.Books
            //    .Include(x => x.Author)
            //    //.Include(x => x.Language)
            //    //.ThenInclude(a => a.Address)//can chain like this to include more corelated table and send multiple data table in one
            //    .ToListAsync();
            //return Ok(result);

            /*Explicit Loading*/
            //var books = await appDbcontext.Books.ToListAsync();
            //foreach(var book in books)
            //{
            //    /*Reference in Explicit Loading is used for one to one mapping*/
            //    await appDbcontext.Entry(book).Reference(x => x.Language).LoadAsync();
            //    await appDbcontext.Entry(book).Reference(x => x.Author).LoadAsync();
            //}
            //return Ok(books);

            /*Lazy Loading=> in lazy loading we have to install nuget package proxies and add its method while configuring
              Db and chain the method named UseLazyLoadingProxies() and make all the classes virtual but keep in mind
            in lazy loading all related table will make a seprate db call which may affect performance*/
            //var book = await appDbcontext.Books.FirstAsync();
            //var author = book.Author;
            //return Ok(book);


            /*WE can also use sql in efcore and also add where LInQ method in it*/
            //var title = "multilple table insertion bulk update title 33 title";
            //var colName = "Id";
            //var colValue = "9";
            //var parameter = new SqlParameter("colValue", colValue);
            //var books = await appDbcontext.Books
            //    .FromSql($"select * from Books where {colName} = {colValue}")
            //    .ToListAsync();//here security  of FromSql() works and not let client to steal the data
            //var book1 = await appDbcontext.Books
            //    .FromSqlRaw($"select * from books where {colName} = @colValue", parameter)
            //    .ToListAsync();
            //var book =await appDbcontext.Books.FromSql($"select * from books").Where(x=>x.Title == title).ToListAsync();
            //return Ok(book);


            /*here we can also implement normal sql queries unlike upper we do pereform on specifically on our entities*/
            //var result = await appDbcontext.Database.SqlQuery<int>($"select id from books").ToListAsync();//Using SqlQuery we cannot update in DB
            var result = await appDbcontext.Database.ExecuteSqlAsync($"update books set NoofPages = 2 where id = 9");// here we can do updates also.
            return Ok(result);

        }

       
        [HttpGet("languages")]
        public async Task<IActionResult> GetAllLanguagesAsync()
        {
            //var result = await appDbcontext
            //    .Languages
            //    //.Include(x => x.Books)
            //    .ToListAsync();
            //return Ok(result);


            /*usning collection in many to one or many realtionship in explicit loading*/
            var languages = await appDbcontext.Languages.ToListAsync();
            foreach(var language in languages)
            {
                await appDbcontext.Entry(language).Collection(x => x.Books)
                    //.Query()// HERE  we can put query() method to add any methods like where clause etc,
                    //.Where()
                    .LoadAsync();
            }
            return Ok(languages);
        }

            [HttpPost("")]
        public async Task<IActionResult> AddNewBookAsync([FromBody] Book model)
        {
            /*since in Book.cs We refereced author to Book so. here in author object data will stored in author table due to its ref. */

            //var author = new Author()
            //{
            //    Name = "test",
            //    Email = "test@test.com"
            //};
            //model.Author = author;
            
            appDbcontext.Books.Add(model);//EfCore Track the change being made with the entity(Book)here any db interaction not occur. only change on our entity
            await appDbcontext.SaveChangesAsync();//after that db interaction occur here. actually map our obj to table.
            return Ok(model);
        }
        

        [HttpPost("bulk")]
        public async Task<IActionResult> AddBooksAsync([FromBody] List<Book> model)
        {
            appDbcontext.Books.AddRange(model);//in previous version of efcore in bulk insertion. there are multiple sql queries was running= no on records
                                               //but in latest efcore only one hit to the db in bulk insertion. increase performance. can see in sqlserver profiler.
            await appDbcontext.SaveChangesAsync();
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook([FromRoute] int id, [FromBody] Book model)
        {
            var book = await appDbcontext.Books.FirstOrDefaultAsync(x => x.Id == id);
            /* in this code of updation we are calling 2 times Db 1upper and 2nd in savechanges()*/
            if(book == null)
            {
                return NotFound();
            }
            book.Title = model.Title;
            book.Description = model.Description;
            book.NoOfPages = model.NoOfPages;

            await appDbcontext.SaveChangesAsync();
            return Ok(model);
        }

        [HttpPut("")]
        public async Task<IActionResult> UpdateBookInSingleQuery([FromBody] Book model)
        {
            //appDbcontext.Update(model);//in this of updataion we update table in single query but there is one problem that
            //we have to pass all the paramter. in case if we don't give any value to a parameter
            //it by default make that null in table even it have some value previously in table.

            /* ultimately there is ENTRY State parameter in efcore which make changes to our Db(modified etc). we can modify manually also.*/
            appDbcontext.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await appDbcontext.SaveChangesAsync();
            return Ok(model);

        }

        [HttpPut("bulk")]
        public async Task<IActionResult> UpdateBooksInBulk()
        {
            await appDbcontext.Books
                .Where(x=>x.NoOfPages ==100)
                .ExecuteUpdateAsync(x => x
            .SetProperty(p=>p.Description, " 33 updation")
            .SetProperty(p=>p.Title, p=>p.Title +  " 33 title")
            );

            /* *** here we don't used savechanges() method because this method works on the basis of Changetracker
             of efcore and this only work when we make changes in entity and want to sync up to DB. then we use savechanges()
            but in ExecuteUpdate() method we directly run the command not make any changes in the entithy*/
            return Ok();
        }

        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeletebookAsync([FromRoute] int bookId)
        {
            //var book = await appDbcontext.Books.FindAsync(bookId);
            //if(book == null)
            //{
            //    return NotFound();
            //}
            //appDbcontext.Books.Remove(book);
            //await appDbcontext.SaveChangesAsync();
            //return Ok();

            var book = new Book
            {
                Id = bookId
            };
            appDbcontext.Entry(book).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            await appDbcontext.SaveChangesAsync();//here only ONE hit to the db because here we are not getting data first and then deleteting
            return Ok();
        }
        
        [HttpDelete("bulk")]
        public async Task<IActionResult> DeletebookInBulkAsync()
        {
            //var books = await appDbcontext.Books.Where(x => x.Id < 4).ToListAsync();
            //appDbcontext.Books.RemoveRange(books);//in RevomoveRange() method it execute delete query one by one in backend.
            //await appDbcontext.SaveChangesAsync();
            //return Ok();

            //foreach (var item in ids)
            //{
            //    var book = new Book { Id = item };
            //    appDbcontext.Entry(book).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            //    await appDbcontext.SaveChangesAsync();
            //}

            var resul = await appDbcontext.Books.Where(x=>x.Id<7).ExecuteDeleteAsync();
            //in Backend this execute a single delete query for bulk delete which increase performance and even use this method in sigle row deletion
            return Ok();
        }
    }
}
