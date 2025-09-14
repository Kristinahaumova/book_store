namespace Book_store.Model
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int Quantity { get; set; }
        public int Price => Book.Price;

        /*public OrderItem(int bookId, int quantity, Book book)
        {
            BookId = bookId;
            Quantity = quantity;
            Book = book;
        }*/
    }
}
