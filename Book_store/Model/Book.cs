namespace Book_store.Model
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AuthorName { get; set; }
        public string BookRegion { get; set; }
        public string BookGenre { get; set; }
        public int Price { get; set; }
    }
}
