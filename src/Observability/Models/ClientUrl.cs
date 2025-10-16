namespace Observability.Models;
public sealed record ClientUrl(
    string? Href,
    string? Origin,
    string? Pathname,
    string? Search
);