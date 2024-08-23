using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using WebApplication1.DTOs;
using WebApplication1.Services;
using AutoMapper;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WebApplication1.ResourceParameters;
using WebApplication1.Models;
using System.Text.Json;
using Microsoft.AspNetCore.JsonPatch;
using WebApplication1.Helpers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using Microsoft.Net.Http.Headers;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]    // api/travelRoutes
    [ApiController]
    public class TravelRoutesController : ControllerBase
    {
        private ITravelRouteRepository _travelRouteRepository;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;
        private readonly IPropertyMappingService _propertyMappingService;

        public TravelRoutesController(
            ITravelRouteRepository travelRouteRepository, 
            IMapper mapper,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor,
            IPropertyMappingService propertyMappingService)
        {
            _travelRouteRepository = travelRouteRepository;
            _mapper = mapper;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            _propertyMappingService = propertyMappingService;
        }

        // Pagination helper

        private string GenerateTravelRouteResourceURL(
            TravelRouteResourceParameters parameters,
            PaginationResourceParameters parameters2,
            ResourceURLType type
        )
        {
            return type switch
            {
                ResourceURLType.PreviousPage => _urlHelper.Link("GetTravelRoutes",
                new
                {
                        keyword = parameters.Keyword,
                        operatorType = parameters.OperatorType,
                        rating = parameters.Rating,
                        pageNumber = parameters2.PageNumber - 1,
                        pageSize = parameters2.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields
                    }),
                ResourceURLType.NextPage => _urlHelper.Link("GetTravelRoutes",
                    new
                    {
                        keyword = parameters.Keyword,
                        operatorType = parameters.OperatorType,
                        rating = parameters.Rating,
                        pageNumber = parameters2.PageNumber + 1,
                        pageSize = parameters2.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields
                    }),
                _ => _urlHelper.Link("GetTravelRoutes",
                    new
                    {
                        keyword = parameters.Keyword,
                        operatorType = parameters.OperatorType,
                        rating = parameters.Rating,
                        pageNumber = parameters2.PageNumber,
                        pageSize = parameters2.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields
                    })
            };
        }

        // HATEOAS helper

        private IEnumerable<LinkDTO> CreateLinkForTravelRoute(Guid travelRouteId, string fields)
        {
            var links = new List<LinkDTO>();

            links.Add(new LinkDTO()
            {
                Href = _urlHelper.Link("GetTravelRoutesById", new { travelRouteId, fields }),
                Rel = "self",
                Method = "GET"
            });
            links.Add(new LinkDTO()
            {
                Href = _urlHelper.Link("UpdateTravelRoute", new { travelRouteId }),
                Rel = "update",
                Method = "POST"
            });
            links.Add(new LinkDTO()
            {
                Href = _urlHelper.Link("PartiallyUpdateTravelRoute", new { travelRouteId }),
                Rel = "partially_update",
                Method = "PATCH"
            });
            links.Add(new LinkDTO()
            {
                Href = _urlHelper.Link("DeleteTravelRoute", new { travelRouteId }),
                Rel = "delete",
                Method = "DELETE"
            });
            links.Add(new LinkDTO()
            {
                Href = _urlHelper.Link("GetPictureListForTravelRoute", new { travelRouteId }),
                Rel = "get_pictures",
                Method = "GET"
            });
            links.Add(new LinkDTO()
            {
                Href = _urlHelper.Link("CreateTravelRoutePicture", new { travelRouteId }),
                Rel = "create_picture",
                Method = "POST"
            });


            return links;
        }

        // HATEOAS helper

        private IEnumerable<LinkDTO> CreateLinksForTravelRouteList(TravelRouteResourceParameters parameters, PaginationResourceParameters parameters2)
        {
            var links = new List<LinkDTO>();

            links.Add(new LinkDTO()
            {
                Href = GenerateTravelRouteResourceURL(parameters, parameters2, ResourceURLType.CurrentPage),
                Rel = "self",
                Method = "GET"
            });
            links.Add(new LinkDTO()
            {
                Href = _urlHelper.Link("CreateTravelRoute", null),
                Rel = "create_travel_itinerary",
                Method = "POST"
            });


            return links;
        }


        // api/travelRoutes/?keyword=xxx
        // 1. application/json -> 旅游路线资源
        // 2. application/vnd.mycompany.hateoas+json
        [HttpGet(Name = "GetTravelRoutes")]
        [HttpHead]
        public async Task<IActionResult> GetTravelRoutes(
            [FromQuery] TravelRouteResourceParameters parameters, 
            [FromQuery] PaginationResourceParameters parameters2,
            [FromHeader(Name ="Accept")] string mediaType)          // Action 函数
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest("The request media type is not supported");
            }


            if (!_propertyMappingService.IsPropertyMappingExist<TravelRouteDTO, TravelRoute>(parameters.OrderBy))
            {
                return BadRequest("One or more orderBy keywords are invalid");
            }
            if (!_propertyMappingService.PropertiesExist<TravelRouteDTO>(parameters.Fields))
            {
                return BadRequest("One or more fields in fields are invalid");
            }

            var routes = await _travelRouteRepository.GetTravelRoutesAsync(parameters.Keyword, parameters.OperatorType, parameters.RatingValue, parameters2.PageNumber, parameters2.PageSize, parameters.OrderBy);
            if (routes == null || routes.Count() == 0)
            {
                return NotFound("No travel itinerary exist");
            }
            var routesDTO = _mapper.Map<IEnumerable<TravelRouteDTO>>(routes);


            // 分页导航
            var previousPageLink = routes.HasPrevious ?
                                    GenerateTravelRouteResourceURL(parameters, parameters2, ResourceURLType.PreviousPage)
                                    : null;
            var nextPageLink = routes.HasNext ?
                                GenerateTravelRouteResourceURL(parameters, parameters2, ResourceURLType.NextPage)
                                : null;

            // x-pagination
            var paginationMetadata = new
            {
                previousPageLink,
                nextPageLink,
                totalCount = routes.TotalCount,
                pageSize = routes.PageSize,
                currentPage = routes.CurrentPage,
                totalPages = routes.TotalPages
            };

            Response.Headers.Add("x-pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            // 数据塑形

            var shapedDTOList = routesDTO.ShapeData(parameters.Fields);


            // HATEOAS
            if (parsedMediaType.MediaType == "application/vnd.mycompany.hateoas+json")
            {
                var links = CreateLinksForTravelRouteList(parameters, parameters2);
                var shapedDTOWithLinksList = shapedDTOList.Select(item =>
                {
                    var dict = item as IDictionary<string, object>;
                    var itemLinks = CreateLinkForTravelRoute((Guid)dict["Id"], parameters.Fields);    // Bug: 当数据塑形没有包含 Id 时, 代码运行到这里会出错
                    dict.Add("links", itemLinks);
                    return dict;
                });

                var result = new
                {
                    value = shapedDTOWithLinksList,
                    links
                };

                return Ok(result);
            }
            
            return Ok(shapedDTOList);
        }


        // api/travelRoutes/{travelRouteId}
        [HttpGet("{travelRouteId:Guid}", Name = "GetTravelRoutesById")]
        [HttpHead("{travelRouteId:Guid}")]
        public async Task<IActionResult> GetTravelRoutesById([FromRoute] Guid travelRouteId, [FromQuery] string fields, [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest("The request media type is not supported");
            }


            if (!_propertyMappingService.PropertiesExist<TravelRouteDTO>(fields))
            {
                return BadRequest("One or more fields in fields are invalid");
            }


            var route = await _travelRouteRepository.GetTravelRouteAsync(travelRouteId);

            if (route == null)
            {
                return NotFound($"The travel itinerary {travelRouteId} is not found");
            }

            var routeDTO = _mapper.Map<TravelRouteDTO>(route);
            var result = routeDTO.ShapeData(fields);

            // HATEOAS
            if (parsedMediaType.MediaType == "application/vnd.mycompany.hateoas+json")
            {
                var linkDtos = CreateLinkForTravelRoute(travelRouteId, fields);
                var result2 = result as IDictionary<string, object>;
                result2.Add("links", linkDtos);
                return Ok(result2);
            }

            return Ok(result);
        }


        




        [HttpPost(Name = "CreateTravelRoute")]
        [Authorize(AuthenticationSchemes = "Bearer")]   // 解决 Identity 框架的多角色认证默认中间件不匹配问题
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateTravelRoute([FromBody] TravelRouteForCreationDTO travelRouteForCreationDTO, [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest("The request media type is not supported");
            }

            var travelRoute = _mapper.Map<TravelRoute>(travelRouteForCreationDTO);
            await _travelRouteRepository.AddTravelRouteAsync(travelRoute);

            var travelRouteDTO = _mapper.Map<TravelRouteDTO>(travelRoute);

            var result = travelRouteDTO.ShapeData(null);


            // HATEOAS
            if (parsedMediaType.MediaType == "application/vnd.mycompany.hateoas+json")
            {
                var links = CreateLinkForTravelRoute(travelRoute.Id, null);
                var result2 = result as IDictionary<string, object>;
                result2.Add("links", links);


                return CreatedAtRoute(              
                    "GetTravelRoutesById",
                    new { travelRouteId = travelRouteDTO.Id },
                    //travelRouteDTO
                    result2
                );
            }

            return CreatedAtRoute(              // 简单的HATEOAS: 定义 header location
                    "GetTravelRoutesById",
                    new { travelRouteId = travelRouteDTO.Id },
                    //travelRouteDTO
                    result
                );
        }

        [HttpPut("{travelRouteId}", Name = "UpdateTravelRoute")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTravelRoute([FromRoute] Guid travelRouteId, [FromBody] TravelRouteForUpdateDTO travelRouteForUpdateDTO)
        {
            if (!await _travelRouteRepository.TravelRouteExistAsync(travelRouteId))
            {
                return NotFound($"The travel itinerary {travelRouteId} is not found");
            }

            var travelRouteFromRepo = await _travelRouteRepository.GetTravelRouteAsync(travelRouteId);

            // 将 travelRouteForUpdateDTO 中的数据映射给 travelRouteFromRepo (travelRouteFromRepo 中的数据被更新为 travelRouteForUpdateDTO 中的数据)

            _mapper.Map(travelRouteForUpdateDTO, travelRouteFromRepo);

            // TravelRouteRepository 中的 _context 自动帮我们追踪模型的状态 (travelRouteFromRepo 的状态)
            // 因为 travelRouteFromRepo 中的数据发生了更新，调用 _context.SaveChanges() 会自动把这部分更新更新进入数据库

            await _travelRouteRepository.SaveAsync();

            return NoContent();
        }

        [HttpPatch("{travelRouteId}", Name = "PartiallyUpdateTravelRoute")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PartiallyUpdateTravelRoute(
            [FromRoute] Guid travelRouteId, 
            [FromBody] JsonPatchDocument<TravelRouteForUpdateDTO> patchDocument)
        {
            if (!await _travelRouteRepository.TravelRouteExistAsync(travelRouteId))
            {
                return NotFound($"The travel itinerary {travelRouteId} is not found");
            }

            var travelRouteFromRepo = await _travelRouteRepository.GetTravelRouteAsync(travelRouteId);

            var travelRouteForUpdateDTO = _mapper.Map<TravelRouteForUpdateDTO>(travelRouteFromRepo);
            patchDocument.ApplyTo(travelRouteForUpdateDTO, ModelState);   // ApplyTo() 把 ModelState 和 TravelRouteForUpdateDTO 绑定在一起 

            if (!TryValidateModel(travelRouteForUpdateDTO))   // 调用 ModelState 进行数据验证
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(travelRouteForUpdateDTO, travelRouteFromRepo);
            await _travelRouteRepository.SaveAsync();

            return NoContent();
        }

        

        [HttpDelete("{travelRouteId}", Name = "DeleteTravelRoute")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTravelRoute([FromRoute] Guid travelRouteId)
        {
            if (!await _travelRouteRepository.TravelRouteExistAsync(travelRouteId))
            {
                return NotFound($"The travel itinerary {travelRouteId} is not found");
            }

            var travelRoute = await _travelRouteRepository.GetTravelRouteAsync(travelRouteId);
            await _travelRouteRepository.DeleteTravelRouteAsync(travelRoute);

            return NoContent();
        }

        [HttpDelete("({travelRouteIds})")]           // DELETE  api/travelRoutes/(1, 2, 3, 4)
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTravelRouteByIds(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))][FromRoute] IEnumerable<Guid> travelRouteIds)
        {
            if (travelRouteIds == null)
            {
                return BadRequest();
            }

            var travelRouteList = await _travelRouteRepository.GetTravelRoutesByIDListAsync(travelRouteIds);

            if (travelRouteList.Count() == 0)
            {
                return NotFound("None of the travel itinerary matches the IDs in the provided ID list");
            }
            await _travelRouteRepository.DeleteTravelRoutesAsync(travelRouteList);

            return NoContent();
        }
    }
}
