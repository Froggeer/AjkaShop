using System.Collections.Generic;

namespace Ajka.BL.Models.ProductImport
{
    public class ImportAdlerDto
    {
        public string CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string ProductName { get; set; }

        public string ProductLabel { get; set; }

        public string Description { get; set; }

        public string SizeName { get; set; }

        public string ColorName { get; set; }

        public string SizeId { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string ProductImagePath { get; set; }

        public string Grammage { get; set; }

        public IList<string> SexList { get; set; } = new List<string>();

        public IList<ImportAdlerImageDto> ImagePaths { get; set; }  = new List<ImportAdlerImageDto>();

        public IList<ImportAdlerSizeDto> Sizes { get; set; } = new List<ImportAdlerSizeDto>();
    }
}
