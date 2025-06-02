namespace Core.Specifications
{
    public class ProductSpecParams
    {
        private const int MaxPageSize = 50;

        // Make these nullable so you can detect if they were omitted
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }

        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? Sort { get; set; }
        public string? Search { get; set; }

        public int GetPageIndex() => PageIndex ?? 1;

        public int GetPageSize() => (PageSize.HasValue && PageSize.Value <= MaxPageSize)
            ? PageSize.Value
            : 10;
    }
}
