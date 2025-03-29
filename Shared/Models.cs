using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Shared;

public class PriceList
{
    [JsonProperty("id")]
    [Key]
    public required Guid Id { get; set; }

    [JsonProperty("validUntil")]
    public required DateTime ValidUntil { get; set; }

    [JsonProperty("legs")]
    public required List<Leg> Legs { get; set; }
}

public class Leg
{
    [JsonProperty("id")]
    [Key]
    public required Guid Id { get; set; }

    [JsonProperty("routeInfo")]
    public required RouteInfo RouteInfo { get; set; }

    [JsonProperty("providers")]
    public required List<Provider> Providers { get; set; }
}

public class RouteInfo
{
    [JsonProperty("id")]
    [Key]
    public required Guid Id { get; set; }

    [JsonProperty("from")]
    public required Location From { get; set; }

    [JsonProperty("to")]
    public required Location To { get; set; }

    [JsonProperty("distance")]
    public required long Distance { get; set; }
}

public class Location
{
    [JsonProperty("id")]
    [Key]
    public required Guid Id { get; set; }

    [JsonProperty("name")]
    public required string Name { get; set; }
}

public class Provider
{
    
    [JsonProperty("id")]
    [Key]
    public required Guid Id { get; set; }

    [JsonProperty("company")]
    public required Company Company { get; set; }

    [JsonProperty("price")]
    public required double Price { get; set; }

    [JsonProperty("flightStart")]
    public required DateTime FlightStart { get; set; }

    [JsonProperty("flightEnd")]
    public required DateTime FlightEnd { get; set; }
}

public class Company
{
    [JsonProperty("id")]
    [Key]
    public required Guid Id { get; set; }

    [JsonProperty("name")]
    public required SpaceCompany Name { get; set; }
}

public enum SpaceCompany
{
    [EnumMember(Value = "Space Voyager")]
    SpaceVoyager,
    [EnumMember(Value = "Explore Origin")]
    ExploreOrigin,
    [EnumMember(Value = "Space Piper")]
    SpacePiper,
    [EnumMember(Value = "Space Odyssey")]
    SpaceOdyssey,
    [EnumMember(Value = "SpaceX")]
    SpaceX,
    [EnumMember(Value = "Explore Dynamite")]
    ExploreDynamite,
    [EnumMember(Value = "Spacelux")]
    Spacelux,
    [EnumMember(Value = "Spacegenix")]
    Spacegenix,
    [EnumMember(Value = "Galaxy Express")]
    GalaxyExpress,
    [EnumMember(Value = "Travel Nova")]
    TravelNova
}

public enum Planet
{
    Earth,
    Mars,
    Venus,
    Jupiter,
    Saturn,
    Uranus,
    Neptune,
    Mercury
}

public class RouteDto
{
    public required List<LegDto> Legs { get; set; }
    public required double TotalPrice { get; set; }
    public required long TotalDistance { get; set; }
    public required List<string> Companies { get; set; }
}

public class LegDto
{
    public required Guid Id { get; set; }
    public required RouteInfoDto RouteInfo { get; set; }
    public required ProviderDto Provider { get; set; }
}

public class RouteInfoDto
{
    public required Guid Id { get; set; }
    public required LocationDto From { get; set; }
    public required LocationDto To { get; set; }
    public required long Distance { get; set; }
}

public class LocationDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
}

public class ProviderDto
{
    public required Guid Id { get; set; }
    public required CompanyDto Company { get; set; }
    public required double Price { get; set; }
    public required DateTime FlightStart { get; set; }
    public required DateTime FlightEnd { get; set; }
}

public class CompanyDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
}
