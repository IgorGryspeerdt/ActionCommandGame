using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

public class MyService
{
    public MyService(HttpClient client)
    {
        Console.WriteLine($"[DEBUG] MyService constructed. BaseAddress: {client.BaseAddress}");
    }
}

class Program
{
    static void Main()
    {
        var services = new ServiceCollection();
        services.AddHttpClient<MyService>(c => c.BaseAddress = new Uri("https://localhost:7213"));
        var provider = services.BuildServiceProvider();
        var svc = provider.GetRequiredService<MyService>();
    }
}
