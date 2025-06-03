namespace ArtMart.Models.ViewModels
{
    public class OrderPlacementViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Amount { get; set; }
        public string SellerName { get; set; }

        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }
    }

}
