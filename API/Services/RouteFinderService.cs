using Shared;

namespace API.Services;

public static class RouteFinder
{
    public static List<List<(Leg Leg, Provider Provider)>> FindRoutes(Planet start, Planet destination, List<Leg> legs, int maxHops = 3)
    {
        var graph = legs.GroupBy(leg => Enum.Parse<Planet>(leg.RouteInfo.From.Name, true))
                        .ToDictionary(g => g.Key, g => g.Select(leg => (Enum.Parse<Planet>(leg.RouteInfo.To.Name, true), leg)).ToList());

        var results = new List<List<(Leg, Provider)>>();
        var queue = new Queue<(Planet Planet, List<(Leg, Provider)> Route, HashSet<Planet> Visited, int Hops, DateTime LastFlightEnd)>();

        queue.Enqueue((start, new List<(Leg, Provider)>(), new HashSet<Planet> { start }, 0, DateTime.MinValue));

        while (queue.Count > 0)
        {
            var (planet, route, visited, hops, lastFlightEnd) = queue.Dequeue();

            if (planet == destination)
            {
                results.Add([.. route]);
                continue;
            }

            if (hops >= maxHops || !graph.TryGetValue(planet, out var connections))
            {
                continue;
            }

            foreach (var (nextPlanet, leg) in connections)
            {
                if (visited.Contains(nextPlanet))
                {
                    continue;
                }

                foreach (var provider in leg.Providers.Where(p => p.FlightStart >= lastFlightEnd))
                {
                    var newVisited = new HashSet<Planet>(visited) { nextPlanet };
                    var newRoute = new List<(Leg, Provider)>(route) { (leg, provider) };
                    queue.Enqueue((nextPlanet, newRoute, newVisited, hops + 1, provider.FlightEnd));
                }
            }
        }

        return results;
    }
}
