using System.ComponentModel.DataAnnotations;

namespace CommandsService.Options;

public class GrpcPlatformOptions
{
    [Required]
    public string Url { get; set; } = string.Empty;
    [Required]
    public bool EnableHttp2 { get; set; }   
}
