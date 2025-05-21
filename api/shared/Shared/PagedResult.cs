namespace Shared;

public class PagedResult<T>
{
    public T[] Items { get; }
    public int CurrentPage { get; }
    public int ItemsPerPage { get; }
    public double TotalPages { get;}
    public int TotalItemsCount { get;  }

    public PagedResult(T[] items, int currentPage, int itemsPerPage, double totalPages, int totalItemsCount)
    {
        Items = items;
        CurrentPage = currentPage;
        ItemsPerPage = itemsPerPage;
        TotalPages = totalPages;
        TotalItemsCount = totalItemsCount;
    }
}