using cityInfo.Api.Entities;

namespace cityInfo.Api.Services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(string? name, string? searchQuery,int pageNumber,int pageSize);
        Task<City?> GetCityAsync(Guid cityId, bool includePointOfInterest);
        Task<bool> CityExistsAsync(Guid cityId);
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(Guid cityId);
        Task<PointOfInterest?> GetPointOfInterestForCityAsync(Guid cityId,
            int pointOfInterestId);
        Task AddPointOfInterestForCityAsync(Guid cityId, PointOfInterest pointOfInterest);
        void DeletePointOfInterest(PointOfInterest pointOfInterest);
        Task<bool> SaveChangesAsync();
    }
}
