using CRM.Core;
using CRM.Domain;


var builder = WebApplication.CreateBuilder(args);

// adding services
builder.Services.AddServices();
builder.Services.AddDataBase(builder.Configuration);
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
