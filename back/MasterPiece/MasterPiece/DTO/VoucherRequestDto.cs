namespace MasterPiece.DTO
{
    public class VoucherRequestDto
    {
        public string? Code { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
