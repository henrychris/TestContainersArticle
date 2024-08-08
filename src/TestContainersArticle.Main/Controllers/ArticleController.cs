using HenryUtils.Api.Controllers;
using HenryUtils.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TestContainersArticle.Main.Features.CreateArticle;
using TestContainersArticle.Main.Features.GetArticle;

namespace TestContainersArticle.Main.Controllers
{
    public class ArticleController(IMediator mediator) : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateArticle(CreateArticleRequest request)
        {
            var result = await mediator.Send(request);
            return result.Match(_ => Ok(result.ToSuccessfulApiResponse()), ReturnErrorResponse);
        }

        [HttpGet("{articleId:guid}")]
        public async Task<IActionResult> GetArticle(string articleId)
        {
            var result = await mediator.Send(new GetArticleRequest { ArticleId = articleId });
            return result.Match(_ => Ok(result.ToSuccessfulApiResponse()), ReturnErrorResponse);
        }
    }
}
