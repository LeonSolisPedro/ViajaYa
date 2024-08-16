using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Web.Filters;

public class GlobalActionFilter : IAsyncActionFilter
{
  private readonly IHttpClientFactory _factory;
  private readonly IConfiguration _configuration;

  public GlobalActionFilter(IHttpClientFactory factory, IConfiguration configuration)
  {
    _factory = factory;
    _configuration = configuration;
  }

  public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
  {

    var resultContext = await next();

    if (resultContext.Result is ViewResult)
    {
      if (context.Controller is Controller controller)
      {
        var client = _factory.CreateClient("client");

        var responses = await Task.WhenAll([
          client.GetAsync("v1/Agency/GetInfo"),
          client.GetAsync("v1/Agency/GetCurrency"),
          client.GetAsync("v1/Agency/GetTourCategories")
        ]);

        var json1 = await (responses?.ElementAtOrDefault(0)?.Content?.ReadAsStringAsync() ?? Task.FromResult(""));
        var json2 = await (responses?.ElementAtOrDefault(1)?.Content?.ReadAsStringAsync() ?? Task.FromResult(""));
        var json3 = await (responses?.ElementAtOrDefault(2)?.Content?.ReadAsStringAsync() ?? Task.FromResult(""));

        dynamic? data1 = JsonConvert.DeserializeObject(json1);
        dynamic? data2 = JsonConvert.DeserializeObject(json2);
        dynamic? data3 = JsonConvert.DeserializeObject(json3);

        controller.ViewBag.sitio = data1;
        controller.ViewBag.currencies = data2;
        controller.ViewBag.categories = data3;

        var urlImage = _configuration.GetSection("Settings")["BaseURLImage"];
        controller.ViewBag.urlImage = urlImage;

        var nameAgency = _configuration.GetSection("Settings")["NameAgency"];
        controller.ViewBag.nameAgency = nameAgency;
      }
    }
  }
}
