using CRM.Core;
using CRM.Domain;


var builder = WebApplication.CreateBuilder(args);

// adding services
builder.Services.AddServices();
builder.Services.AddDataBase(builder.Configuration);
builder.Services.AddControllersWithViews();

var app = builder.Build();
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath:"rotativa");

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
