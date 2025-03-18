namespace DbOperationsWithEFCoreApp.Data
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int NoOfPages { get; set; }
        public bool IsActice { get; set; }
        public DateTime CreatedOn { get; set; }
        public int LanguageId { get; set; }//this work as the fk of this table and below we make ref.
        public int? AuthorId { get; set; } //here ? makes it nullable

        public Author? Author { get; set; }
        public Language? Language { get; set; }//this property will be use to reference the Language table
    }
}
