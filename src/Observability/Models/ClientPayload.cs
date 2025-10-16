namespace Observability.Models;
public sealed record ClientPayload(
    DateTimeOffset Timestamp,
    string? PublicIp,
    ClientNavigator? Navigator,
    ClientScreen? Screen,
    ClientConnection? Connection,
    string? Timezone,
    ClientUrl? Url,
    string? Referrer,
    ClientGeolocation? Geolocation,
    ClientMeta? Meta
);
