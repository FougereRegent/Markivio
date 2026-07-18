namespace Markivio.Application.Dto;


public class ArticleInformation
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string? Description { get; set; } = null;
    public bool IsFramable { get; set; } = false;
    public UserInformation User { get; set; }
    public List<TagInformation> Tags { get; set; } = new List<TagInformation>();
    public string Content { get; set; } = string.Empty;
	public bool IsFavorite { get; set; }
}
