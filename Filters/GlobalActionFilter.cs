using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Web.Filters;

public class GlobalActionFilter : IAsyncActionFilter
{
  private readonly IHttpClientFactory _factory;

  public GlobalActionFilter(IHttpClientFactory factory)
  {
    _factory = factory;
  }

  public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
  {

    var resultContext = await next();

    if (resultContext.Result is ViewResult)
    {
      if (context.Controller is Controller controller)
      {
        var client = _factory.CreateClient("client");
        var response = await client.GetAsync("v1/Agency/GetInfo");
        var json = await response.Content.ReadAsStringAsync();
        dynamic obj = JsonConvert.DeserializeObject(json);
        controller.ViewBag.nose = obj;
      }
    }
  }
}
