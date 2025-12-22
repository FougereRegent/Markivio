using System;


namespace Markivio.Application.Dto;

public record TagInformation(
    Guid Id,
    string Name,
    string Color
);


public record TagCreateArticle(
    Guid Id,
    string Name
);