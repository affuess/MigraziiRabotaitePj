namespace MigraziiRabotaitePj.DTO
{
    public class ProductQuery
    {
        //Filtering
        public string? Brand { get; set; }
        public string? State { get; set; }
        public decimal PriceFrom { get; set; }
        public decimal PriceTo { get; set; }

        //Sorting
        public string SortBy { get; set; } = "Brand";
        public string SortDir { get; set; } = "asc";

        //Pagination
        public int Page { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value < 1 ? 10 :
                (value > 50 ? 50 : value);
        }
        public DateTime? CreatedAt { get; set; }
    }
}

