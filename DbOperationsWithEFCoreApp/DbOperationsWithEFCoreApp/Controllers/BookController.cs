using DbOperationsWithEFCoreApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbOperationsWithEFCoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController(AppDbContext appDbcontext) : ControllerBase
        //here we use Primary constructor new feature of dotnet8 and in currency contrller we use depedency injection by legacy contructor.
    {
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
            //var books = await appDbcontext.Books.Where(x=> x.Id < 4).ToListAsync();
            //appDbcontext.Books.RemoveRange(books);//in RevomoveRange() method it execute delete query one by one in backend.
            //await appDbcontext.SaveChangesAsync();
            //return Ok();

        }
    }
}
