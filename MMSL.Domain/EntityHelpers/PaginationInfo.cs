namespace MMSL.Domain.EntityHelpers {
    public class PaginationInfo {
        public int TotalItems { get; set; } = 0;

        public int PageSize { get; set; } = 20;

        public int PageNumber { get; set; } = 1;

        public double PagesCount { get; set; } = 0.0;
    }
}
