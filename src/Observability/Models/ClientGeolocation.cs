namespace Observability.Models;
public sealed record ClientGeolocation(
    double? Latitude,
    double? Longitude,
    double? Accuracy
);