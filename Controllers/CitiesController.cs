using Asp.Versioning;
using AutoMapper;
using cityInfo.Api.Models;
using cityInfo.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace cityInfo.Api.Controllers
{
    /// <summary>
    /// CitiesController
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion(1)]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityInfoRepository"></param>
        /// <param name="mapper"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// عمل بحث على جميع المدن 
        /// </summary>
        /// <param name="name">البحث باسم المدينة</param>
        /// <param name="searchQuery">بحث عام</param>
        /// <param name="pageNumber">رقم الصفحة</param>
        /// <param name="pageSize">اجمالى الصفحات</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityDto>>> GetCities(
            string? name, 
            string? searchQuery, 
            int pageNumber = 1, 
            int pageSize = 10)
        {
            if (pageSize > maxCitiesPageSize)
            {
                pageSize = maxCitiesPageSize;
            }
            var userId = User.Claims.FirstOrDefault(u => u.Type == "sub")?.Value;
            var (cityEntites, paginationMetadata) = await _cityInfoRepository
                .GetCitiesAsync(name, searchQuery, pageNumber, pageSize);
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntites));
        }

        /// <summary>
        /// الحصول على بيانات المدينة من خلال المعرف الخاص بها
        /// </summary>
        /// <param name="id">المعرف الخاص بالمدينة</param>
        /// <param name="includePointOfInterest">المعالم الرئيسية فى المدينة</param>
        /// <returns>استرجاع بيانات المدن بمعالمها او بدون</returns>
        /// <response code="200">استرجاع بيانات المدن بنجاح</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCity(Guid id, bool includePointOfInterest = false)
        {
            var city = await _cityInfoRepository.GetCityAsync(id, includePointOfInterest);
            if(city == null) { return NotFound(); }
            if (includePointOfInterest)
            {
                return Ok(_mapper.Map<CityDto>(city));
            }
            return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
        }
    }
}
