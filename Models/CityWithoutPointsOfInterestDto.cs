namespace cityInfo.Api.Models
{
    /// <summary>
    /// المدينة بدون المعالم السياحية
    /// </summary>
    public class CityWithoutPointsOfInterestDto
    {
        /// <summary>
        /// معرف المدينة
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// اسم المدينة
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// وصف المدينة
        /// </summary>
        public string? Description { get; set; }
    }
}
