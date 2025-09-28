namespace Markivio.Domain.Repositories;


public record PaginatedValues<T>
(
    List<T> Values,
    uint ElementByPage,
    uint PageNumber,
    uint TotalPage
 ) where T : class;

public record CursorValues<T>(
    List<T> Values,
    Guid NextCursor,
    Guid LastCursor
    ) where T : class;
