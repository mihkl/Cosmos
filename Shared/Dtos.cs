namespace Shared;

public record RouteDto
{
    public required List<LegDto> Legs { get; set; }
    public required double TotalPrice { get; set; }
    public required long TotalDistance { get; set; }
    public required List<string> Companies { get; set; }
}

public record LegDto
{
    public required Guid Id { get; set; }
    public required RouteInfoDto RouteInfo { get; set; }
    public required ProviderDto Provider { get; set; }
}

public record RouteInfoDto
{
    public required Guid Id { get; set; }
    public required LocationDto From { get; set; }
    public required LocationDto To { get; set; }
    public required long Distance { get; set; }
}

public record LocationDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
}

public record ProviderDto
{
    public required Guid Id { get; set; }
    public required CompanyDto Company { get; set; }
    public required double Price { get; set; }
    public required DateTime FlightStart { get; set; }
    public required DateTime FlightEnd { get; set; }
}

public record CompanyDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
}

public record ReservationDto
{
    public required Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required List<ReservationLegDto> Legs { get; set; }
    public required double TotalQuotedPrice { get; set; }
    public required List<string> TransportationCompanyNames { get; set; }
    public required DateTime CreatedAt { get; set; }
}

public record ReservationLegDto
{
    public required Guid Id { get; set; }
    public required RouteInfoDto RouteInfo { get; set; }
    public required ProviderDto Provider { get; set; }
}