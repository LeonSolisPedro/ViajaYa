using Web.Filters;

//Services
var builder = WebApplication.CreateBuilder(args);
var mvcBuilder = builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<GlobalActionFilter>();
});
if (builder.Environment.IsDevelopment()) mvcBuilder.AddRazorRuntimeCompilation();
builder.Services.AddHttpClient("client", x => { x.BaseAddress = new Uri($"{builder.Configuration["Settings:BaseAPIURL"]}/"); });

//App
var app = builder.Build();
if (app.Environment.IsDevelopment()) mvcBuilder.AddRazorRuntimeCompilation();
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{action=Index}/{id?}",
    defaults: new { controller = "Home" });
app.Run();
