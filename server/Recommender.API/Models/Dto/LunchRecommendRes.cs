namespace RecommendHub.API.Models.Dto;

public class LunchRecommendRes
{
    public string StoreName { get; set; }
    public string MainMenu { get; set; }
    public List<string> SubMenus { get; set; }
    public List<string> Tags { get; set; }
}