using Shared;

namespace API.Mappers;

public static class Mappers
{
    public static RouteDto ToDto(this List<(Leg Leg, Provider Provider)> route)
    {
        return new RouteDto
        {
            Legs = route.Select(legProvider => legProvider.ToDto()).ToList(),
            TotalPrice = route.Sum(legProvider => legProvider.Provider.Price),
            TotalDistance = route.Sum(legProvider => legProvider.Leg.RouteInfo.Distance),
            Companies = [.. route.Select(legProvider => legProvider.Provider.Company.Name.ToString()).Distinct()]
        };
    }

    public static LegDto ToDto(this (Leg Leg, Provider Provider) legProvider)
    {
        return new LegDto
        {
            Id = legProvider.Leg.Id,
            RouteInfo = legProvider.Leg.RouteInfo.ToDto(),
            Provider = legProvider.Provider.ToDto()
        };
    }

    public static RouteInfoDto ToDto(this RouteInfo routeInfo)
    {
        return new RouteInfoDto
        {
            Id = routeInfo.Id,
            From = routeInfo.From.ToDto(),
            To = routeInfo.To.ToDto(),
            Distance = routeInfo.Distance
        };
    }

    public static LocationDto ToDto(this Location location)
    {
        return new LocationDto
        {
            Id = location.Id,
            Name = location.Name
        };
    }

    public static ProviderDto ToDto(this Provider provider)
    {
        return new ProviderDto
        {
            Id = provider.Id,
            Company = provider.Company.ToDto(),
            Price = provider.Price,
            FlightStart = provider.FlightStart,
            FlightEnd = provider.FlightEnd
        };
    }

    public static CompanyDto ToDto(this Company company)
    {
        return new CompanyDto
        {
            Id = company.Id,
            Name = company.Name.ToString()
        };
    }

    public static ReservationDto ToDto(this Reservation reservation)
    {
        return new ReservationDto
        {
            Id = reservation.Id,
            FirstName = reservation.FirstName,
            LastName = reservation.LastName,
            Legs = [.. reservation.ReservationLegs.Select(leg => leg.ToDto()).OrderBy(l => l.Provider.FlightStart)],
            TotalQuotedPrice = reservation.TotalQuotedPrice,
            TransportationCompanyNames = [.. reservation.TransportationCompanyNames],
            CreatedAt = reservation.CreatedAt
        };
    }

    public static ReservationLegDto ToDto(this ReservationLeg reservationLeg)
    {
        return new ReservationLegDto
        {
            Id = reservationLeg.Id,
            RouteInfo = reservationLeg.Leg.RouteInfo.ToDto(),
            Provider = reservationLeg.Provider.ToDto()
        };
    }
}
