using Blazored.LocalStorage;
using Blazored.SessionStorage;
using FistWeb.Components;
using FistWeb.Data;
using FistWeb.Data.Services;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

#region khai bao services
builder.Services.AddScoped<IThongKeService, CallService>();
builder.Services.AddScoped<GetListThueDo, CallService>();
builder.Services.AddScoped<IGetParamaterService, CallService>();
builder.Services.AddScoped<IGetParaUserService, CallService>(); 
builder.Services.AddScoped<IAddParaService, CallService>();
builder.Services.AddScoped<IDeleteParaService, CallService>();
builder.Services.AddScoped<IInsertSPService, CallService>();
builder.Services.AddScoped<IGetSumWHService, CallService>(); 
builder.Services.AddScoped<IGetUserInfoService, CallService>(); 
builder.Services.AddScoped<IGetProductIDService, CallService>(); 
builder.Services.AddScoped<IStockQTYService, CallService>();
builder.Services.AddScoped<IInsertOrdersService, CallService>();
builder.Services.AddScoped<IInserUserService, CallService>();
builder.Services.AddScoped<IGetUserIDService, CallService>();
builder.Services.AddScoped<IUpdateReturnOderService, CallService>();
builder.Services.AddScoped<IUpdatePWService, CallService>();
#endregion

builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddHttpClient();

builder.Services.AddBlazoredSessionStorage();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationService>();


//var port = Environment.GetEnvironmentVariable("PORT") ?? "80";
//builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

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

app.MapGet("/ping", () => Results.Ok("pong"));

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
