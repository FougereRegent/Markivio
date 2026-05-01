namespace Markivio.Domain.Repositories;


public record PaginatedValues<T>
(
    List<T> Values,
    int PageSize,
    int PageNumber,
    int TotalPage
 ) where T : class;

public record CursorValues<T>(
    List<T> Values,
    Guid NextCursor,
    Guid LastCursor
    ) where T : class;
