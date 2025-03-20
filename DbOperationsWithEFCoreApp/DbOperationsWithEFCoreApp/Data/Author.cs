namespace DbOperationsWithEFCoreApp.Data
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int AuthorId { get; set; }

        public virtual Address? Address { get; set; }
    }
}
