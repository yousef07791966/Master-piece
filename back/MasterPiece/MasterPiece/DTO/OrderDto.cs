namespace MasterPiece.DTO
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string TransactionId { get; set; }
        public int Status { get; set; }
        public decimal Amount { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
