using System;
using System.Collections;
using System.Collections.Generic;
using WebApplication1.Models;
using System.Threading.Tasks;
using WebApplication1.Helpers;

namespace WebApplication1.Services
{
    public interface ITravelRouteRepository
    {
        Task<PaginationList<TravelRoute>> GetTravelRoutesAsync(string keyword, string operatorType, int ratingValue, int pageNumber, int pageSize, string orderBy);
        Task<TravelRoute> GetTravelRouteAsync(Guid id);
        Task<IEnumerable<TravelRoute>> GetTravelRoutesByIDListAsync(IEnumerable<Guid> Ids);
        Task<bool> TravelRouteExistAsync(Guid id);
        Task<IEnumerable<TravelRoutePicture>> GetPicturesByTravelRouteIdAsync(Guid id);
        Task<TravelRoutePicture> GetPictureAsync(int id);
        Task AddTravelRouteAsync(TravelRoute travelRoute);
        Task AddTravelRoutePictureAsync(Guid travelRouteId, TravelRoutePicture travelRoutePicture);
        Task DeleteTravelRouteAsync(TravelRoute travelRoute);
        Task DeleteTravelRoutePictureAsync(TravelRoutePicture travelRoutePicture);
        Task DeleteTravelRoutesAsync(IEnumerable<TravelRoute> travelRouteList);
        Task<ShoppingCart> GetShoppingCartByUserIdAsync(string userId);
        Task CreateShoppingCartAsync(ShoppingCart shoppingCart);
        Task AddShoppingCartItemAsync(LineItem lineItem);
        Task<LineItem> GetShoppingCartItemAsync(Guid cartId, int itemId);
        Task DeleteShoppingCartItemAsync(LineItem lineItem);
        Task<IEnumerable<LineItem>> GetShoppingCartItemsByIdsAsync(Guid cartId, IEnumerable<int> itemIds);
        Task DeleteShoppingCartItemsByIdsAsync(IEnumerable<LineItem> itemList);
        Task AddOrderAsync(Order order);
        Task<PaginationList<Order>> GetOrdersAsync(string userId, int pageNumber, int pageSize);
        Task<Order> GetOrderByIdAsync(Guid orderId);
        Task<bool> SaveAsync();
    }
}
