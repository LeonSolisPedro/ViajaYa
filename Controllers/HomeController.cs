using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.Models;

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _factory;

    public HomeController(IHttpClientFactory factory)
    {
        _factory = factory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _factory.CreateClient("client");

        var responses = await Task.WhenAll([
          client.GetAsync("v1/Home/GetCarousel"),
          client.GetAsync("v1/Home/GetPopularTours"),
          client.GetAsync("v1/Home/GetOtherTours"),
          client.GetAsync("v1/Home/GetGalleries"),
          client.GetAsync("v1/Home/GetOffers")
        ]);

        var json1 = await (responses?.ElementAtOrDefault(0)?.Content?.ReadAsStringAsync() ?? Task.FromResult(""));
        var json2 = await (responses?.ElementAtOrDefault(1)?.Content?.ReadAsStringAsync() ?? Task.FromResult(""));
        var json3 = await (responses?.ElementAtOrDefault(2)?.Content?.ReadAsStringAsync() ?? Task.FromResult(""));
        var json4 = await (responses?.ElementAtOrDefault(3)?.Content?.ReadAsStringAsync() ?? Task.FromResult(""));
        var json5 = await (responses?.ElementAtOrDefault(4)?.Content?.ReadAsStringAsync() ?? Task.FromResult(""));


        dynamic? data1 = JsonConvert.DeserializeObject(json1);
        dynamic? data2 = JsonConvert.DeserializeObject(json2);
        dynamic? data3 = JsonConvert.DeserializeObject(json3);
        dynamic? data4 = JsonConvert.DeserializeObject(json4);
        dynamic? data5 = JsonConvert.DeserializeObject(json5);


        ViewBag.slider = data1;
        ViewBag.countPopular = data2?.Count ?? 0;
        ViewBag.popularTours = data2;
        ViewBag.countOther = data3?.Count ?? 0;
        ViewBag.otherTours = data3;
        ViewBag.countGallery = data4?.Count ?? 0;
        ViewBag.topGallery = data4;
        ViewBag.countOffers = data5?.Count ?? 0;
        ViewBag.offers = data5;

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

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("gracias-email")]
    public async Task<IActionResult> Gracias(Contacto model)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Favor de completar el formulario";
            return RedirectToAction("Contacto");
        }
        var client = _factory.CreateClient("client");
        var response = await client.PostAsJsonAsync("v1/Contact/SendInfo", model);
        var json = await response.Content.ReadAsStringAsync();
        dynamic? data = JsonConvert.DeserializeObject(json);
        bool isSuccess = data?.success ?? false;
        if (!isSuccess)
        {
            TempData["ErrorMessage"] = "Ocurrió un error";
            return RedirectToAction("Contacto");
        }
        return View("gracias-email");
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
