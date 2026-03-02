namespace MigraziiRabotaitePj.Models
{
    public class Order
    {
        public Guid Id { set; get; }
        public string? CustomerId { set; get; }
        public string Status { set; get; }
        public DateTime? CreatedAt { set; get; }
        public decimal Total { set; get; }
        public List<OrderItem> Items { set; get; }
    }
}
