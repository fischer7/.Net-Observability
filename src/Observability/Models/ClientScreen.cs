namespace Observability.Models;
public sealed record ClientScreen(
    int? Width,
    int? Height,
    int? AvailWidth,
    int? AvailHeight,
    int? ColorDepth,
    int? PixelDepth
);