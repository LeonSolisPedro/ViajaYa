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

    public async Task<IActionResult> Galeria()
    {
        var client = _factory.CreateClient("client");
        var response = await client.GetAsync("v1/Gallery/GetGalleries");
        var json = await response.Content.ReadAsStringAsync();
        dynamic? data = JsonConvert.DeserializeObject(json);
        ViewBag.imagenes = data;
        return View("galeria");
    }

    public IActionResult Nosotros()
    {
        return View("nosotros");
    }


    public IActionResult Contacto()
    {
        return View("contacto");
    }

    [Route("terminos-condiciones")]
    public async Task<IActionResult> Terminos()
    {
        var client = _factory.CreateClient("client");
        var response = await client.GetAsync("v1/Terms/TermsCondition");
        var json = await response.Content.ReadAsStringAsync();
        dynamic? data = JsonConvert.DeserializeObject(json);
        ViewBag.terminosCondiciones = data?.text;
        return View("terminos-condiciones");
    }


    [Route("aviso-privacidad")]
    public async Task<IActionResult> Privacidad()
    {
        var client = _factory.CreateClient("client");
        var response = await client.GetAsync("v1/Terms/PrivacyNotice");
        var json = await response.Content.ReadAsStringAsync();
        dynamic? data = JsonConvert.DeserializeObject(json);
        ViewBag.avisoPrivacidad = data?.text;
        return View("aviso-privacidad");
    }
}
