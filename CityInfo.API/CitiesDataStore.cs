using CityInfo.API.Models;
using System.Collections.Generic;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public static CitiesDataStore Current { get; } = new CitiesDataStore();
        public List<CityDto> Cities { get; set; }

        public CitiesDataStore()
        {
            Cities = new List<CityDto> {
                new CityDto{ Id = 1, Name = "City 1", Description = "City 1 description", PointsOfInterest = new List<PointOfInterestDto>{ new PointOfInterestDto { Id = 1, Name = "interest 1"} } },
                new CityDto{ Id = 2, Name = "City 2", Description = "City 2 description" ,PointsOfInterest = new List<PointOfInterestDto>{ new PointOfInterestDto { Id = 2, Name = "interest 2"} } }
            };
        }
    }
}
