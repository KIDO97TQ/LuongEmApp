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
builder.Services.AddScoped<IDeleteProductService, CallService>();
builder.Services.AddScoped<IUpdateProductByIdService, CallService>(); 
builder.Services.AddScoped<IUpdateReturnAllOrderService, CallService>();
builder.Services.AddScoped<IGetUserInfo1Service, CallService>();
builder.Services.AddScoped<IUpdateUserService, CallService>(); 
builder.Services.AddScoped<UpdateReturnAllOrder1, CallService>(); 
builder.Services.AddScoped<IGetParamaterMakeupService, CallService>();
builder.Services.AddScoped<IInsertRevenueService, CallService>(); 
builder.Services.AddScoped<IGetSumRevenueService, CallService>(); 
builder.Services.AddScoped<IGetListMakeupService, CallService>(); 

#endregion

builder.Services.AddSingleton<LoadingService>();

builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddHttpClient();

builder.Services.AddBlazoredSessionStorage();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationService>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//var port = Environment.GetEnvironmentVariable("PORT") ?? "80";
//builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

//var port = Environment.GetEnvironmentVariable("PORT");
//if (string.IsNullOrEmpty(port))
//{
//    throw new Exception("PORT environment variable is not set.");
//}
//builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

if (app.Environment.IsProduction())
{
    //app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapGet("/ping", () => Results.Ok("pong"));

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
