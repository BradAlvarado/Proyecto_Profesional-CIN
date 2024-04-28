namespace Sistema_CIN.Models
{
    public class PagedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalItems { get; private set; }
        public int PageSize { get; private set; }

        public PagedList(List<T> items, int pageIndex, int pageSize, int totalItems, int totalPages)
        {
            PageIndex = pageIndex;
            TotalPages = totalPages;
            TotalItems = totalItems;
            PageSize = pageSize;

            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public static PagedList<T> Create(List<T> source, int pageIndex, int pageSize)
        {
            var totalItems = source.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, pageIndex, pageSize, totalItems, totalPages);
        }
    }
}
