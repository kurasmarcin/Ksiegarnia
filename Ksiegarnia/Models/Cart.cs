namespace Ksiegarnia.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public List<Book> Books { get; set; } // Lista książek w koszyku
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
