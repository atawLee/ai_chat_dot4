using Microsoft.AspNetCore.Mvc;
using RecommendHub.API.Models.Dto;

namespace RecommendHub.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Food", "Restaurant", "Lunch", "Menu")]  // Swagger 태그로 카테고리 명시

    public class LunchController : ControllerBase
    {
        private static readonly List<LunchRecommendRes> Menus = new List<LunchRecommendRes>
        {
            new LunchRecommendRes {
                StoreName = "한식당 김밥천국",
                MainMenu = "김치찌개",
                SubMenus = new List<string> { "공기밥", "계란말이" },
                Tags = new List<string> { "한식", "찌개", "매운맛" }
            },
            new LunchRecommendRes {
                StoreName = "맛있는 고기집",
                MainMenu = "불고기",
                SubMenus = new List<string> { "쌈채소", "된장찌개" },
                Tags = new List<string> { "한식", "고기", "추천" }
            },
            new LunchRecommendRes {
                StoreName = "비빔밥 명가",
                MainMenu = "비빔밥",
                SubMenus = new List<string> { "미소된장국", "계란후라이" },
                Tags = new List<string> { "한식", "밥", "야채" }
            },
            new LunchRecommendRes {
                StoreName = "돈까스 하우스",
                MainMenu = "돈까스",
                SubMenus = new List<string> { "샐러드", "우동" },
                Tags = new List<string> { "일식", "튀김", "고기" }
            },
            new LunchRecommendRes {
                StoreName = "치킨나라",
                MainMenu = "치킨",
                SubMenus = new List<string> { "감자튀김", "콜라" },
                Tags = new List<string> { "양식", "치킨", "간편식" }
            },
            new LunchRecommendRes {
                StoreName = "피자헛",
                MainMenu = "피자",
                SubMenus = new List<string> { "치즈스틱", "샐러드" },
                Tags = new List<string> { "양식", "피자", "파티" }
            }
        };

        [HttpGet("recommend")]
        public ActionResult<LunchRecommendRes> Recommend()
        {
            Console.WriteLine("Launch 추천 api 실행");
            var random = new Random();
            var menu = Menus[random.Next(Menus.Count)];
            return Ok(menu);
        }
    }
}
