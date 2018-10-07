using CityInfo.API.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API
{
    public static class CityInfoContextExtensions
    {
        public static void EnsureSeedDataForContext(this CityInfoContext context)
        {
            if (context.Cities.Any())
            {
                return;
            }

            var cities = new List<City> {
                new City{ Name = "City 1", Description = "City 1 description", PointsOfInterest = new List<PointOfInterest>{ new PointOfInterest { Name = "interest 1"} } },
                new City{ Name = "City 2", Description = "City 2 description" ,PointsOfInterest = new List<PointOfInterest>{ new PointOfInterest { Name = "interest 2"} } }
            };

            context.Cities.AddRange(cities);
            context.SaveChanges();
        }
    }
}
