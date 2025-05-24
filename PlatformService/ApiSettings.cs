using System.ComponentModel.DataAnnotations;

namespace PlatformService;

public class ApiSettings
{
    [Required]
    public string CommandService { get; set; } = string.Empty;
}

