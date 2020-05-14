namespace MMSL.Domain.EntityHelpers {
    public class PagerIdParams : PagerParams {
        public long Id { get; }

        public PagerIdParams(long id, int pageNumber, int limit, string searchTerm) 
            : base(pageNumber, limit, searchTerm) {
            Id = id;
        }
    }
}
