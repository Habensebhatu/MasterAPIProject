using Microsoft.AspNetCore.Http;

namespace business_logic_layer.ViewModel
{
    public class productModel
    {
        public Guid ProductId { get; set; }
        public string Title { get; set; }
        public decimal PiecePrice { get; set; }
        public decimal? Kilo { get; set; }
        public int? InstokeOfPiece { get; set; }

        public decimal? CratePrice { get; set; }
        public int? CrateQuantity { get; set; }
        public int? InstokeOfCrate { get; set; }

        public string Description { get; set; }
        public bool IsPopular { get; set; } = false;
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<IFormFile> NewImages { get; set; }
        public List<ExistingImageUrlModel> ExistingImageUrls { get; set; }
        public List<int> NewImageIndices { get; set; }

    }

    public class ExistingImageUrlModel
    {
        public int index { get; set; }
        public string file { get; set; }
    }
}

