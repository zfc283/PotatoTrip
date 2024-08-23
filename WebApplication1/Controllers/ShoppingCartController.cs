using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Helpers;
using System.Linq;
using Stateless.Graph;

namespace WebApplication1.Controllers
{
    [Route("api/shoppingCart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;      // 可获取当前用户
        private readonly ITravelRouteRepository _travelRouteRepository;
        private readonly IMapper _mapper;

        public ShoppingCartController(
            IHttpContextAccessor httpContextAccessor, 
            ITravelRouteRepository travelRouteRepository,
            IMapper mapper
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _travelRouteRepository = travelRouteRepository;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetShoppingCart")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetShoppingCart()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var shoppingCart = await _travelRouteRepository.GetShoppingCartByUserIdAsync(userId);

            return Ok(_mapper.Map<ShoppingCartDTO>(shoppingCart));
        }

        [HttpPost("items")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddShoppingCartItem([FromBody] AddShoppingCartItemDTO addShoppingCartItemDTO)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var shoppingCart = await _travelRouteRepository.GetShoppingCartByUserIdAsync(userId);

            var travelRoute = await _travelRouteRepository.GetTravelRouteAsync(addShoppingCartItemDTO.TravelRouteId);
            if (travelRoute == null)
            {
                return NotFound($"The travel Route with Id {addShoppingCartItemDTO.TravelRouteId} is not found");
            }

            var lineItem = new LineItem()
            {
                TravelRouteId = addShoppingCartItemDTO.TravelRouteId,
                TravelRoute = travelRoute,
                ShoppingCartId = shoppingCart.Id,
                OriginalPrice = travelRoute.OriginalPrice,
                DiscountPercent = travelRoute.DiscountPercent
            };

            await _travelRouteRepository.AddShoppingCartItemAsync(lineItem);

            return Ok(_mapper.Map<ShoppingCartDTO>(shoppingCart));
        }


        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> DeleteShoppingCartItem([FromRoute] int itemId)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var shoppingCart = await _travelRouteRepository.GetShoppingCartByUserIdAsync(userId);

            var lineItem = await _travelRouteRepository.GetShoppingCartItemAsync(shoppingCart.Id, itemId);
            if (lineItem == null)
            {
                return NotFound($"The cart item with Id {itemId} is not found");
            }

            await _travelRouteRepository.DeleteShoppingCartItemAsync(lineItem);

            return NoContent();
        }


        [HttpDelete("items/({itemIds})")]           
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteShoppingCartItemsByIds(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))][FromRoute] IEnumerable<int> itemIds)
        {
            if (itemIds == null)
            {
                return BadRequest();
            }

            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var shoppingCart = await _travelRouteRepository.GetShoppingCartByUserIdAsync(userId);

            var itemList = await _travelRouteRepository.GetShoppingCartItemsByIdsAsync(shoppingCart.Id, itemIds);

            if (itemList.Count() == 0)
            {
                return NotFound("None of the cart item matches the IDs in the provided ID list");
            }
            await _travelRouteRepository.DeleteShoppingCartItemsByIdsAsync(itemList);

            return NoContent();
        }


        [HttpPost("checkout")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Checkout()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var shoppingCart = await _travelRouteRepository.GetShoppingCartByUserIdAsync(userId);

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                OrderItems = shoppingCart.ShoppingCartItems,
                State = OrderStateEnum.Pending,
                CreateTime = DateTime.Now,
            };

            foreach (var litem in order.OrderItems)
            {
                litem.ShoppingCartId = null;
                litem.OrderId = order.Id;
            }

            shoppingCart.ShoppingCartItems = null;

            await _travelRouteRepository.AddOrderAsync(order);

            return Ok(_mapper.Map<OrderDTO>(order));

        }
    }
}
