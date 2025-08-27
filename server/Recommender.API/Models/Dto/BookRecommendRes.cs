namespace RecommendHub.API.Models.Dto;

public class BookRecommendRes
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public List<string> Tags { get; set; }
}