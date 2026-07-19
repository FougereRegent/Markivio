namespace Markivio.Application.Dto;

public readonly record struct ArticleFilters(
    string? Title,
    List<string>? TagNames
);

