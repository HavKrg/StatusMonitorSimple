namespace Application.Dtos;

public class PaginatedDataResponse<T>
{
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public T? Data { get; set; }
}
