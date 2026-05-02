namespace TaskManager.Application.DTOs
{
    public class Pagination<T>
    {
        public List<T> Items { get; set; } = null!;
        public int TotalItems { get; set; }
        public int TolalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public Pagination(List<T> items, int totalItems, int tolalPages, int currentPage, int pageSize)
        {
            Items = items;
            TotalItems = totalItems;
            TolalPages = tolalPages;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }
    }
}
