using Autofac.Core;
using IF.Core.Persistence;
using IF.Manager.Contracts.Services;
using IF.Manager.Persistence.EF;
using IF.Manager.Service;
using IF.Persistence.EF;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ManagerDbContext>(opt => opt.UseSqlServer($"Data Source=HUAWEI-MATEBOOK\\SQLEXPRESS;Database=savas_test;Integrated Security=True"));
builder.Services.AddTransient<IRepository>(provider => new GenericRepository(provider.GetService<ManagerDbContext>()));
builder.Services.AddTransient<IEntityService, EntityService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
