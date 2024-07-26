using cityInfo.Api.DbContexts;
using cityInfo.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace cityInfo.Api.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _ctx;
        public CityInfoRepository(CityInfoContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }
        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _ctx.Cities.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(
            string? name,
            string? searchQuery, 
            int pageNumber, 
            int pageSize)
        {
            var collection = _ctx.Cities as IQueryable<City>;
            if(!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                collection = collection.Where(c => c.Name == name);
            }
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(a => a.Name.Contains(searchQuery) ||
                (a.Description != null && a.Description.Contains(searchQuery)));
            }
            var totalItemCount = await collection.CountAsync();
            var paginationMetadata = new PaginationMetadata(
                totalItemCount, pageSize, pageNumber
                );
            var collectionToReturn = await collection.OrderBy(c => c.Name)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();
            return (collectionToReturn, paginationMetadata);
        }

        public async Task<City?> GetCityAsync(Guid cityId, bool includePointOfInterest)
        {
            return includePointOfInterest ? await  _ctx.Cities.Include(c => c.PointsOfInterest).Where(c => c.Id == cityId).FirstOrDefaultAsync() : await _ctx.Cities.Where(c => c.Id == cityId).FirstOrDefaultAsync();  
        }

        public async Task<bool> CityExistsAsync(Guid cityId)
        {
            return await _ctx.Cities.AnyAsync(c => c.Id == cityId);
        }

        public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(Guid cityId, int pointOfInterestId)
        {
            return await _ctx.PointsOfInterest.Where(p => p.CityId == cityId && p.Id == pointOfInterestId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(Guid cityId)
        {
            return await _ctx.PointsOfInterest.Where(p => p.CityId == cityId).ToListAsync();
        }

        public async Task AddPointOfInterestForCityAsync(Guid cityId, PointOfInterest pointOfInterest)
        {
            var city = await GetCityAsync(cityId, false);
            if(city != null) 
            { 
                city.PointsOfInterest.Add(pointOfInterest);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _ctx.SaveChangesAsync() >= 0);
        }

        public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            _ctx.PointsOfInterest.Remove(pointOfInterest);
        }
    }
}
