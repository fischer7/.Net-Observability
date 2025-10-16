namespace Observability.Models;
public sealed record ClientNavigator(
    string? UserAgent,
    string? Platform,
    string? Language,
    IEnumerable<string>? Languages,
    bool CookieEnabled,
    int? HardwareConcurrency,
    double? DeviceMemory
);