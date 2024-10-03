namespace MasterPiece.DTO
{
    public class cartItemResponseDTO
    {
        public int CartItemId { get; set; }

        public int? CartId { get; set; }

        public productDTO? Product { get; set; }

        public int? Quantity { get; set; }
    }

    public class productDTO
    {
        public int ProductId { get; set; }

        public string? Name { get; set; }

        public string? Image { get; set; }

        public decimal? Price { get; set; }

        public decimal? PriceWithDiscount { get; set; }

    }
}
