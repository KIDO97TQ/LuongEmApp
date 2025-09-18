using FistWeb.Components;
using FistWeb.Components.Services;
using FistWeb.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

//var tester = new ConnectionTester(connectionString);
//await tester.TestAsync();
builder.Services.AddScoped<IUserService, CallService>();
builder.Services.AddScoped<IThongKeService, CallService>();

// Lấy port từ biến môi trường PORT, nếu không có thì mặc định 80
var port = Environment.GetEnvironmentVariable("PORT") ?? "80";

// Cấu hình app lắng nghe đúng port này
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
