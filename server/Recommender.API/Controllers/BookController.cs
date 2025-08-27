using Microsoft.AspNetCore.Mvc;
using RecommendHub.API.Models.Dto;

namespace RecommendHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private static readonly List<BookRecommendRes> Books = new List<BookRecommendRes>
    {
        new BookRecommendRes {
            Title = "자기혁명",
            Author = "김철수",
            Description = "자기계발과 성장에 관한 책.",
            Tags = new List<string> { "자기계발", "성장", "동기부여" }
        },
        new BookRecommendRes {
            Title = "클린 코드",
            Author = "로버트 C. 마틴",
            Description = "소프트웨어 개발자를 위한 코드 작성 원칙.",
            Tags = new List<string> { "프로그래밍", "코딩", "개발" }
        },
        new BookRecommendRes {
            Title = "해리포터와 마법사의 돌",
            Author = "J.K. 롤링",
            Description = "마법과 모험이 가득한 판타지 소설.",
            Tags = new List<string> { "소설", "판타지", "청소년" }
        },
        new BookRecommendRes {
            Title = "나는 네트워크 엔지니어다",
            Author = "이영호",
            Description = "네트워크의 기본과 실무를 다룬 책.",
            Tags = new List<string> { "네트워크", "IT", "실무" }
        }
    };

    [HttpGet("recommend")]
    public ActionResult<BookRecommendRes> Recommend()
    {
        System.Console.WriteLine("Book 추천 api 실행");
        var random = new Random();
        var book = Books[random.Next(Books.Count)];
        return Ok(book);
    }

    [HttpGet("recommend-by-genre")]
    public ActionResult<BookRecommendRes> RecommendByGenre([FromQuery] string genre)
    {
        System.Console.WriteLine("파라메터 장르: " + genre);
        if (string.IsNullOrWhiteSpace(genre))
            return BadRequest("genre 파라미터를 입력하세요.");

        var genreBooks = Books.FindAll(b => b.Tags.Exists(tag => tag.Equals(genre, StringComparison.OrdinalIgnoreCase)));
        if (genreBooks.Count == 0)
            return NotFound($"'{genre}' 장르의 책이 없습니다.");

        var random = new Random();
        var book = genreBooks[random.Next(genreBooks.Count)];
        return Ok(book);
    }
}