﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Shared;

public record PriceList
{
    [JsonProperty("id")]
    [Key]
    [Column(TypeName = "uuid")]
    public required Guid Id { get; set; }

    [JsonProperty("validUntil")]
    public required DateTime ValidUntil { get; set; }

    [JsonProperty("legs")]
    public required List<Leg> Legs { get; set; }
    public List<Reservation> Reservations { get; set; } = [];
}

public record Leg
{
    [JsonProperty("id")]
    [Key]
    [Column(TypeName = "uuid")]
    public required Guid Id { get; set; }

    [JsonProperty("routeInfo")]
    public required RouteInfo RouteInfo { get; set; }

    [JsonProperty("providers")]
    public required List<Provider> Providers { get; set; }
}

public record RouteInfo
{
    [JsonProperty("id")]
    [Key]
    [Column(TypeName = "uuid")]
    public required Guid Id { get; set; }

    [JsonProperty("from")]
    public required Location From { get; set; }

    [JsonProperty("to")]
    public required Location To { get; set; }

    [JsonProperty("distance")]
    public required long Distance { get; set; }
}

public record Location
{
    [JsonProperty("id")]
    [Key]
    [Column(TypeName = "uuid")]
    public required Guid Id { get; set; }

    [JsonProperty("name")]
    public required string Name { get; set; }
}

public record Provider
{

    [JsonProperty("id")]
    [Key]
    [Column(TypeName = "uuid")]
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

public record Company
{
    [JsonProperty("id")]
    [Key]
    [Column(TypeName = "uuid")]
    public required Guid Id { get; set; }

    [JsonProperty("name")]
    public required SpaceCompany Name { get; set; }
}

public record ReservationRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required List<LegDto> Legs { get; set; }
    public required double TotalQuotedPrice { get; set; }
    public required List<string> TransportationCompanyNames { get; set; }
}

public record ReservationLeg
{
    [Key]
    [Column(TypeName = "uuid")]
    public required Guid Id { get; set; }
    public required Provider Provider { get; set; }
    public required Leg Leg { get; set; }
}
public record Reservation
{
    [Key]
    [Column(TypeName = "uuid")]
    public required Guid Id { get; set; }
    [Column(TypeName = "uuid")]
    public required Guid PriceListId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required List<ReservationLeg> ReservationLegs { get; set; }
    public required double TotalQuotedPrice { get; set; }
    public required List<string> TransportationCompanyNames { get; set; }
    public required DateTime CreatedAt { get; set; }
}