


using Microsoft.EntityFrameworkCore;
using ProniaShop.DAL;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer("server=SAMIR\\SQLEXPRESS;database=ProniaDb;trusted_connection=true;TrustServerCertificate=True;");
});
var app = builder.Build();
app.UseStaticFiles();
app.MapControllerRoute(
    "default",
   "{controller=home}/{action=index}/{id?}"

);
 app.Run();
        
    

