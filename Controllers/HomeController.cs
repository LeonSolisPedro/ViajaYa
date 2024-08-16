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
