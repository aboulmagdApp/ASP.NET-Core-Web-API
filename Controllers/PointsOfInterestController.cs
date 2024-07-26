using Asp.Versioning;
using AutoMapper;
using cityInfo.Api.Models;
using cityInfo.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace cityInfo.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/v{version:apiVersion}/cities/{cityId}/pointsofinterest")]
    [Authorize]
    [ApiController]
    [ApiVersion(2)]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityInfoRepository"></param>
        /// <param name="mapper"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public PointsOfInterestController(ICityInfoRepository cityInfoRepository, IMapper mapper) 
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointOfInterest(Guid cityId)
        {
            if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            var poic = await _cityInfoRepository.GetPointsOfInterestForCityAsync(cityId);
            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(poic));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="pointOfInterestId"></param>
        /// <returns></returns>
        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(
       Guid cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            var poi = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            if (poi == null) { return NotFound(); } 
            return Ok(_mapper.Map<PointOfInterestDto>(poi));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="pointOfInterest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(
            Guid cityId, PointOfInterestForCreationDto pointOfInterest)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            var finalPointOfInterest = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);
            await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, finalPointOfInterest);
            await _cityInfoRepository.SaveChangesAsync();
            var createdPointOfInterestToReturn = _mapper.Map<Models.PointOfInterestDto>(finalPointOfInterest);
            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId = cityId,
                    pointOfInterestId = createdPointOfInterestToReturn.Id
                }, createdPointOfInterestToReturn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="pointOfInterestId"></param>
        /// <param name="pointOfInterest"></param>
        /// <returns></returns>
        [HttpPut("{pointOfInterestId}")]
        public async Task<ActionResult> UpdatePointOfInterest(Guid cityId,int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest) 
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var poi = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            if (poi == null) { return NotFound(); };

            _mapper.Map(pointOfInterest, poi);
            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="pointOfInterestId"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch("{pointOfInterestId}")]
        public async Task<ActionResult> PartiallyUpdatePointOfInterest(
           Guid cityId, int pointOfInterestId,
           JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            var poiE = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            if (poiE == null) { return NotFound(); };

            var poipatch = _mapper.Map<PointOfInterestForUpdateDto>(poiE);

            patchDocument.ApplyTo(poipatch, ModelState);

            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            if (!TryValidateModel(poipatch))
            {
                return BadRequest(ModelState);
            }
            _mapper.Map(poipatch, poiE);
            await _cityInfoRepository.SaveChangesAsync();
            return NoContent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="pointOfInterestId"></param>
        /// <returns></returns>
        [HttpDelete("{pointOfInterestId}")]
        public async Task<ActionResult> DeletePointOfInterest(Guid cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            var poiE = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            if (poiE == null) { return NotFound(); };
            _cityInfoRepository.DeletePointOfInterest(poiE);
            return NoContent();
        }       
    }
}
