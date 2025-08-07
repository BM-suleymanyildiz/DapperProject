var builder = WebApplication.CreateBuilder(args);

// EPPlus lisans ayarı
OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Dapper Context
builder.Services.AddScoped<DapperProject.Context.DapperContext>();

// Add Sales Service
builder.Services.AddScoped<DapperProject.Services.ISalesService, DapperProject.Services.SalesService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
