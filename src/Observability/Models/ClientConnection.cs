namespace Observability.Models;
public sealed record ClientConnection(
    string? EffectiveType,
    double? Downlink,
    double? Rtt,
    bool SaveData
);