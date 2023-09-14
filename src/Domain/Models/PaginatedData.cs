namespace Domain.Models;

public class PaginatedData<T> {
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public T? Data { get; set; }

    public PaginatedData(int currentPage, int totalPages, int pageSize)
    {
        CurrentPage = currentPage;
        TotalPages = totalPages;
        PageSize =  pageSize;
    }
}