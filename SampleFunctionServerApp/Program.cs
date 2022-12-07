using LaserCatEyes.AzureFunctionEndpointListener;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = new ConfigurationBuilder()
    .AddUserSecrets<Program>();
IConfiguration configuration = builder.Build();

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(builder =>
    {
        builder.Services.AddLaserCatAzureFunctionEyesEndpointListener(configuration.GetSection("LaserCatEyesOptions:AppKey").Value);
        builder.UseMiddleware<AzureFunctionEndpointListenerMiddleware>();
    })
    .Build();

host.Run();