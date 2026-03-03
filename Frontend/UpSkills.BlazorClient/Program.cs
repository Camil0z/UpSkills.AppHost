using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using UpSkills.BlazorClient;
using UpSkills.BlazorClient.Repositories;
using UpSkills.BlazorClient.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var envConfig = new EnviromentConfig() { ApiServer = "https://localhost:7055/api/" };

builder.Services.AddSingleton<EnviromentConfig>(envConfig);
builder.Services.AddScoped(sp =>
    new HttpClient()
    {
        BaseAddress = new Uri(envConfig.ApiServer)
    }
);

builder.Services.AddScoped<IInternalApiRepository, InternalApiRepository>();

await builder.Build().RunAsync();
