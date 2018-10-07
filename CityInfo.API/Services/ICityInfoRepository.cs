using CityInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        Task<bool> CityExistsAsync(int cityId);
        Task<IEnumerable<City>> GetCitiesAsync(bool includePointsOfInterest);
        Task<City> GetCityAsync(int cityId, bool includePointsOfInterest);

        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId);
        Task<PointOfInterest> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);

        Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);
        Task<bool> SaveAsync();
        Task DeletePointOfInterestAsync(PointOfInterest existingPointOfInterest);
    }
}
