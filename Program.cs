using Web.Filters;

//Services
var builder = WebApplication.CreateBuilder(args);
var mvcBuilder = builder.Services.AddControllersWithViews(options => {
    options.Filters.Add<GlobalActionFilter>();
});
if (builder.Environment.IsDevelopment()) mvcBuilder.AddRazorRuntimeCompilation();
builder.Services.AddHttpClient("client", x => { x.BaseAddress = new Uri("http://localhost:7283/api/"); });

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
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
