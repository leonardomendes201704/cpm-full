using AppMobileCPM.Models;

namespace AppMobileCPM.ViewModels;

public sealed class HomePageViewModel
{
    public required IReadOnlyList<ServiceCategory> Categories { get; init; }
    public required IReadOnlyDictionary<string, string> SiteContent { get; init; }

    public string Content(string key, string fallbackValue)
    {
        if (SiteContent.TryGetValue(key, out var value) && !string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        return fallbackValue;
    }
}
