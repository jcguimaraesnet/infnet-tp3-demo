using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using profjulioguimaraes.Data;
using Microsoft.Extensions.Azure;
using Amizade.Infrastructure.Services.Blob;
using Amizade.Infrastructure.Services.Queue;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AmizadeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AmizadeDbContext") ?? throw new InvalidOperationException("Connection string 'AmizadeDbContext' not found.")));

var connStringStorageAccount = builder.Configuration.GetValue<string>("ConnectionStringStorageAccount");

builder.Services.AddScoped<IBlobService, BlobService>(provider => 
            new BlobService(connStringStorageAccount));

builder.Services.AddScoped<IQueueService, QueueService>(provider =>
            new QueueService(connStringStorageAccount));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
