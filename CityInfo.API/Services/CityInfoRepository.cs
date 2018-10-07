using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        CityInfoContext context;

        public CityInfoRepository(CityInfoContext ctx)
        {
            context = ctx;
        }

        public async Task<bool> CityExistsAsync(int cityId)
        {
            return await context.Cities.Where(c => c.Id == cityId).AnyAsync();
        }

        public async Task<IEnumerable<City>> GetCitiesAsync(bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return await context.Cities.Include(c => c.PointsOfInterest).OrderBy(c => c.Name).ToListAsync();
            }
            return await context.Cities.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<City> GetCityAsync(int cityId, bool includePointsOfInterest)
        {
            var query = context.Cities.Where(c => c.Id == cityId);
            
            if (includePointsOfInterest)
            {

                return await query.Include(c => c.PointsOfInterest).FirstOrDefaultAsync();
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<PointOfInterest> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
        {
            return await context.PointsOfInterest.Where(p => p.CityId == cityId && p.Id == pointOfInterestId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId)
        {
            return await context.PointsOfInterest.Where(p => p.CityId == cityId).ToListAsync();
        }

        public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest)
        {
            var city = await this.GetCityAsync(cityId, false);
            if (city == null)
            {
                return;
            }

            city.PointsOfInterest.Add(pointOfInterest);
        }

        public async Task DeletePointOfInterestAsync(PointOfInterest existingPointOfInterest)
        {
            await Task.Run(() => context.PointsOfInterest.Remove(existingPointOfInterest));
        }

        public async Task<bool> SaveAsync()
        {
            var changedRecords = await context.SaveChangesAsync();
            return changedRecords >= 0;
        }
    }
}
