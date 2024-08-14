using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _factory;

    public HomeController(IHttpClientFactory factory)
    {
        _factory = factory;
    }

    public IActionResult Index()
    {
        return View();
    }


    public async Task<IActionResult> Privacy()
    {
        var client = _factory.CreateClient("client");
        var response = await client.GetAsync("v1/Terms/PrivacyNotice");
        var json = await response.Content.ReadAsStringAsync();
        dynamic obj = JsonConvert.DeserializeObject(json);
        ViewBag.aviso = obj.text;
        return View();
    }
}
