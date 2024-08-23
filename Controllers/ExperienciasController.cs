using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Controllers;

public class ExperienciasController : Controller
{
    private readonly IHttpClientFactory _factory;

    public ExperienciasController(IHttpClientFactory factory)
    {
        _factory = factory;
    }

    [Route("experiencias")]
    [Route("experiencias/{CategoryName}/{id}")]
    public async Task<IActionResult> Index(string? CategoryName, int? id)
    {
        var client = _factory.CreateClient("client");
        if (id.HasValue)
        {
            var response1 = await client.GetAsync("v1/Agency/GetTourCategories");
            var json1 = await response1.Content.ReadAsStringAsync();
            dynamic? data1 = JsonConvert.DeserializeObject(json1);
            var url = $"/experiencias/{CategoryName}/{id}";
            if (data1 is not IEnumerable<dynamic> dataList || !dataList.Any(x => x.url == url))
                return NotFound();
            ViewBag.categoria = (data1 as IEnumerable<dynamic>)?.FirstOrDefault(x => x.url == url)?.name ?? "";
        }
        var requestURL = $"v1/Tours/GetList{(id.HasValue ? "?idCategory=" + id : "")}";
        var response2 = await client.GetAsync(requestURL);
        var json2 = await response2.Content.ReadAsStringAsync();
        dynamic? data2 = JsonConvert.DeserializeObject(json2);
        ViewBag.experiencias = data2;
        return View();
    }

    [Route("tours/{CityName}/{TourTitle}/{id}")]
    public async Task<IActionResult> Details(string CityName, string TourTitle, int id)
    {
        var client = _factory.CreateClient("client");

        var responses = await Task.WhenAll([
          client.GetAsync($"v1/Tours/GetTour/{id}"),
          client.GetAsync($"v1/Tours/GetDates/{id}")
        ]);

        var json1 = await (responses?.ElementAtOrDefault(0)?.Content?.ReadAsStringAsync() ?? Task.FromResult(""));
        var json2 = await (responses?.ElementAtOrDefault(1)?.Content?.ReadAsStringAsync() ?? Task.FromResult(""));

        dynamic? data1 = JsonConvert.DeserializeObject(json1);
        dynamic? data2 = JsonConvert.DeserializeObject(json2);

        if (data1 == null)
            return NotFound();

        var includes = (data1.tourIncludes as IEnumerable<dynamic>)?.Where(x => x.includeType == 0).ToList();
        var excludes = (data1.tourIncludes as IEnumerable<dynamic>)?.Where(x => x.includeType == 1).ToList();
        var recomendations = (data1.tourIncludes as IEnumerable<dynamic>)?.Where(x => x.includeType == 2).ToList();


        for (int i = 0; i < includes?.Count; i++)
            includes[i]["index"] = i;

        for (int i = 0; i < excludes?.Count; i++)
            excludes[i]["index"] = i;

        for (int i = 0; i < recomendations?.Count; i++)
            recomendations[i]["index"] = i;


        for (int i = 0; i < data1.tourGalleryImages.Count; i++)
            data1.tourGalleryImages[i]["index"] = i;


        for (int i = 0; i < data1.tourGalleryImages.Count; i++)
            data1.tourGalleryImages[i]["index"] = i;


        for (int i = 0; i < data1.tourItineraries.Count; i++)
            data1.tourItineraries[i]["index"] = i;

        ViewBag.tour = data1;
        ViewBag.includes = includes;
        ViewBag.excludes = excludes;
        ViewBag.recomendations = recomendations;

        return View("info-tour");
    }
}
