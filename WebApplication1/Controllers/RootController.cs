using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApplication1.DTOs;

namespace WebApplication1.Controllers
{
    [Route("api")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet(Name ="GetRoot")]
        public IActionResult GetRoot()
        {
            var links = new List<LinkDTO>();

            // 自我链接
            links.Add(new LinkDTO()
            {
                Href = Url.Link("GetRoot", null),
                Rel = "self",
                Method = "GET"
            });

            // 一级链接 旅游路线 "GET api/touristRoutes"
            links.Add(new LinkDTO()
            {
                Href = Url.Link("GetTravelRoutes", null),
                Rel = "get_travel_itinerarys",
                Method = "GET"
            });

            // 一级链接 旅游路线 "POST api/touristRoutes"
            links.Add(new LinkDTO()
            {
                Href = Url.Link("CreateTravelRoute", null),
                Rel = "create_travel_itinerary",
                Method = "POST"
            });

            // 一级链接 购物车 "GET api/shoppingCart"  
            links.Add(new LinkDTO()
            {
                Href = Url.Link("GetShoppingCart", null),
                Rel = "get_shopping_cart",
                Method = "GET"
            });

            // 一级链接 订单 "GET api/orders"
            links.Add(new LinkDTO()
            {
                Href = Url.Link("GetOrders", null),
                Rel = "get_orders",
                Method = "GET"
            });

            return Ok(links);
        }
    }
}
