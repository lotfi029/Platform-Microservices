using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace PlatformService.SyncDataServices.Http;

public class HttpCommandDataClient(HttpClient httpClient, IOptions<ApiSettings> settings) : ICommandDataClient
{
    private readonly ApiSettings _settings = settings.Value;
    public async Task SendPlatformToCommand(PlatformResponse plat) 
    {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(plat),
            Encoding.UTF8,
            "application/json"
        );

        var response = await httpClient.PostAsync(_settings.CommandService, httpContent);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("--> Sync POST to CommandService was OK!");
        }
        else
        {
            Console.WriteLine("--> Sync POST to CommandService was NOT OK!");
        }
    }
}
