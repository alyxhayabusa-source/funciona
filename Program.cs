using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using ProfileApp;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Configurar la ruta base para GitHub Pages
var baseAddress = new Uri(builder.HostEnvironment.BaseAddress);
var basePath = "/funciona/";

// Solo usar ruta raíz para localhost
if (baseAddress.Host.Contains("localhost"))
{
    basePath = "/";
}

// Forzar https en producción
if (!baseAddress.Host.Contains("localhost"))
{
    baseAddress = new Uri($"https://{baseAddress.Host}{baseAddress.Port != 80 ? $":{baseAddress.Port}" : ""}{basePath}");
}

// Configurar el servicio HttpClient
builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri($"{baseAddress.Scheme}://{baseAddress.Host}{baseAddress.Port != 80 ? $":{baseAddress.Port}" : ""}{basePath}")

// Configurar el elemento base
builder.Services.AddScoped(provider =>
{
    var navigationManager = provider.GetRequiredService<NavigationManager>();
    return new BaseAddressService(navigationManager, basePath);
});

// Registrar servicios
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configurar el servicio de JavaScript
builder.Services.AddScoped<BlazorService>();

var host = builder.Build();

// Configurar la ruta base en el cliente
var jsRuntime = host.Services.GetRequiredService<IJSRuntime>();
await jsRuntime.InvokeVoidAsync("eval", $"window.baseUrl = '{basePath}';");

await host.RunAsync();

// Clase para manejar la ruta base
public class BaseAddressService
{
    private readonly NavigationManager _navigationManager;
    private readonly string _basePath;

    public BaseAddressService(NavigationManager navigationManager, string basePath)
    {
        _navigationManager = navigationManager;
        _basePath = basePath.TrimEnd('/');
    }

    public string GetBaseAddress()
    {
        var baseUri = _navigationManager.BaseUri;
        if (baseUri.EndsWith(_basePath + "/"))
        {
            return baseUri;
        }
        return $"{baseUri.TrimEnd('/')}{_basePath}/";
    }
}

// Servicio para interactuar con JavaScript
public class BlazorService
{
    private readonly IJSRuntime _jsRuntime;

    public BlazorService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public ValueTask InitializeAsync()
    {
        return _jsRuntime.InvokeVoidAsync("eval", @"
            if (window.Blazor) {
                Blazor.defaultReconnectionHandler._reconnectCallback = function() {
                    location.reload();
                };
            }
        ");
    }
}
