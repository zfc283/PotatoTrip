using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [Route("api/travelRoutes/{travelRouteId}/pictures")]
    [ApiController]
    public class TravelRoutePicturesController : ControllerBase
    {
        private ITravelRouteRepository _travelRouteRepository;
        private readonly IMapper _mapper;

        public TravelRoutePicturesController(ITravelRouteRepository travelRouteRepository, IMapper mapper)
        {
            _travelRouteRepository = travelRouteRepository;
            _mapper = mapper;
        }

        // 获取嵌套子资源
        [HttpGet(Name = "GetPictureListForTravelRoute")]
        public async Task<IActionResult> GetPictureListForTravelRoute(Guid travelRouteId)
        {
            if (!await _travelRouteRepository.TravelRouteExistAsync(travelRouteId))
            {
                return NotFound($"The travel itinerary {travelRouteId} is not found");
            }
            var pictures = await _travelRouteRepository.GetPicturesByTravelRouteIdAsync(travelRouteId);

            if (pictures == null || pictures.Count() == 0)
            {
                return NotFound($"No pictures found for travel itinerary {travelRouteId}");
            }

            var picturesDTO = _mapper.Map<IEnumerable<TravelRoutePictureDTO>>(pictures);
            return Ok(picturesDTO);
        }

        // 单独获取嵌套子资源
        [HttpGet("{pictureId}", Name = "GetPicture")]
        public async Task<IActionResult> GetPicture(Guid travelRouteId, int pictureId)
        {
            if (!await _travelRouteRepository.TravelRouteExistAsync(travelRouteId))
            {
                return NotFound($"The travel itinerary {travelRouteId} is not found");
            }

            var picture = await _travelRouteRepository.GetPictureAsync(pictureId);

            if (picture == null)
            {
                return NotFound($"The picture with Id {pictureId} is not found");
            }

            var pictureDTO = _mapper.Map<TravelRoutePictureDTO>(picture);
            return Ok(pictureDTO);
        }

        [HttpPost(Name = "CreateTravelRoutePicture")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateTravelRoutePicture(
            [FromRoute] Guid travelRouteId, 
            [FromBody] TravelRoutePictureForCreationDTO travelRoutePictureForCreationDTO)
        {
            if (!await _travelRouteRepository.TravelRouteExistAsync(travelRouteId))
            {
                return NotFound($"The travel itinerary {travelRouteId} is not found");
            }

            var travelRoutePicture = _mapper.Map<TravelRoutePicture>(travelRoutePictureForCreationDTO);
            await _travelRouteRepository.AddTravelRoutePictureAsync(travelRouteId, travelRoutePicture);

            var travelRoutePictureDTO = _mapper.Map<TravelRoutePictureDTO>(travelRoutePicture);

            return CreatedAtRoute(
                "GetPicture",
                new { travelRouteId = travelRoutePictureDTO.TravelRouteId, pictureId = travelRoutePictureDTO.Id },
                travelRoutePictureDTO
            );
        }

        [HttpDelete("{pictureId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTravelRoutePicture([FromRoute] Guid travelRouteId, [FromRoute] int pictureId)
        {
            if (!await _travelRouteRepository.TravelRouteExistAsync(travelRouteId))
            {
                return NotFound($"The travel itinerary {travelRouteId} is not found");
            }

            var travelRoutePicture = await _travelRouteRepository.GetPictureAsync(pictureId);

            if (travelRoutePicture == null)
            {
                return NotFound($"The picture with Id {pictureId} is not found");
            }

            await _travelRouteRepository.DeleteTravelRoutePictureAsync(travelRoutePicture);

            return NoContent();
        }
    }
}
