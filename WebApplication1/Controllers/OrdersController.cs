using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication1.DTOs;
using WebApplication1.Services;
using WebApplication1.ResourceParameters;

namespace WebApplication1.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITravelRouteRepository _travelRouteRepository;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;     // 在 Startup.cs 中注册服务: services.AddHttpClient();

        public OrdersController(
            IHttpContextAccessor httpContextAccessor, 
            ITravelRouteRepository travelRouteRepository,
            IMapper mapper,
            IHttpClientFactory httpClientFactory
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _travelRouteRepository = travelRouteRepository;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet(Name = "GetOrders")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetOrders([FromQuery] PaginationResourceParameters parameters)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var orderList = await _travelRouteRepository.GetOrdersAsync(userId, parameters.PageNumber, parameters.PageSize);

            return Ok(_mapper.Map<IEnumerable<OrderDTO>>(orderList));
        }

        [HttpGet("{orderId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetOrderById([FromRoute] Guid orderId)
        {
            var order = await _travelRouteRepository.GetOrderByIdAsync(orderId);

            if (order == null)
            {
                return NotFound($"The order with ID {orderId} is not found");
            }

            return Ok(_mapper.Map<OrderDTO>(order));
        }

        [HttpPost("{orderId}/placeOrder")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PlaceOrder([FromRoute] Guid orderId)
        {
            var order = await _travelRouteRepository.GetOrderByIdAsync(orderId);
            order.PaymentProcessing();
            await _travelRouteRepository.SaveAsync();


            // 向第三方提交支付请求
            var httpClient = _httpClientFactory.CreateClient();
            string url = @"http://123.56.149.216/api/FakePaymentProcess?icode={0}&orderNumber={1}&returnFault={2}";
            var response = await httpClient.PostAsync(
                string.Format(url, "F0498D9B0FD28CE3", order.Id, false)
                , null);

            // 提取支付结果，以及支付信息
            bool isApproved = false;
            string transactionMetadata = "";
            if (response.IsSuccessStatusCode)
            {
                transactionMetadata = await response.Content.ReadAsStringAsync();
                var jsonObject = (JObject)JsonConvert.DeserializeObject(transactionMetadata);
                isApproved = jsonObject["approved"].Value<bool>();
            }

            // 根据第三方支付结果改变订单状态, 返回结果
            if (isApproved)
            {
                order.PaymentApprove();
            } else
            {
                order.PaymentReject();
            }
            order.TransactionMetadata = transactionMetadata;
            await _travelRouteRepository.SaveAsync();

            return Ok(_mapper.Map<OrderDTO>(order));
        }

    }
}
