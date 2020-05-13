namespace MMSL.Domain.EntityHelpers {
    public class PagerParams {
        public int Offset { get; }
        public int Limit { get; }
        public string SearchTerm { get; }

        public PagerParams(int pageNumber, int limit) {
            Limit = (limit <= 0 ? 20 : limit);
            Offset = (pageNumber <= 0 ? 0 : pageNumber - 1) * Limit;
            SearchTerm = string.Empty;
        }

        public PagerParams(int pageNumber, int limit, string searchTerm)
            : this(pageNumber, limit) {
            SearchTerm = string.IsNullOrEmpty(searchTerm) ? string.Empty : searchTerm;
        }
    }
}
