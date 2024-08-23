using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Database;
using WebApplication1.Models;
using System.Threading.Tasks;
using WebApplication1.Helpers;
using WebApplication1.DTOs;

namespace WebApplication1.Services
{
    public class TravelRouteRepository : ITravelRouteRepository
    {
        private readonly AppDbContext _context;
        private readonly IPropertyMappingService _propertyMappingService;

        public TravelRouteRepository(AppDbContext context, IPropertyMappingService propertyMappingService)
        {
            _context = context;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<TravelRoute> GetTravelRouteAsync(Guid id)
        {
            return await _context.TravelRoutes.Include(t => t.TravelRoutePictures).FirstOrDefaultAsync(item => item.Id == id);
        }

        public async Task<PaginationList<TravelRoute>> GetTravelRoutesAsync(string keyword, string operatorType, int ratingValue, int pageNumber, int pageSize, string orderBy)
        {
            IQueryable<TravelRoute> result = _context.TravelRoutes            // IQueryable 用来保存 Linq 操作，不会立即执行
                                                    .Include(t => t.TravelRoutePictures);
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim();
                result = result.Where(item => item.Title.Contains(keyword));
            }
            
            if ((!string.IsNullOrEmpty(operatorType)) && ratingValue >= 0)
            {
                switch (operatorType)
                {
                    case "lessThan":
                        result = result.Where(item => item.Rating < ratingValue);
                        break;
                    case "largerThan":
                        result = result.Where(item => item.Rating > ratingValue);
                        break;
                    case "equalTo":
                        result = result.Where(item => item.Rating == ratingValue);
                        break;
                }
            }

            //// pagination
            //// skip
            //var skipAmount = (pageNumber - 1) * pageSize;
            //result = result.Skip(skipAmount);
            //// 以 pageSize 为标准显示一定量的数据
            //result = result.Take(pageSize);

            //return await result.ToListAsync();       // 当我们调用 IQueryable 的 ToList() 或 FirstOrDefault() 方法时，Linq 操作才会被执行


            if (!string.IsNullOrEmpty(orderBy))
            {
                //if (orderBy.ToLowerInvariant() == "originalprice")
                //{
                //    result = result.OrderBy(t => t.OriginalPrice);
                //}

                var travelRouteMappingDictionary = _propertyMappingService.GetPropertyMapping<TravelRouteDTO, TravelRoute>();
                result = result.ApplySort(orderBy, travelRouteMappingDictionary);
            }
            
            
            return await PaginationList<TravelRoute>.CreateAsync(pageNumber, pageSize, result);
        }

        public async Task<bool> TravelRouteExistAsync(Guid id)
        {
            return await _context.TravelRoutes.AnyAsync(item => item.Id == id);
        }

        public async Task<IEnumerable<TravelRoutePicture>> GetPicturesByTravelRouteIdAsync(Guid id)
        {
            return await _context.TravelRoutePictures.Where(item => item.TravelRouteId == id).ToListAsync();
        }

        public async Task<TravelRoutePicture> GetPictureAsync(int id)
        {
            return await _context.TravelRoutePictures.Where(item => item.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddTravelRouteAsync(TravelRoute travelRoute)
        {
            if (travelRoute == null)
            {
                throw new ArgumentNullException(nameof(travelRoute));
            }

            // 同时创建父子资源
            // 当我们同时创建父子资源时，参数传入的 travelRoute 中的每一个 TravelRoutePicture 的"Id"和"TravelRouteId"都为空值（默认值)
            // 当我们执行 _context.TravelRoutes.Add(travelRoute) 后，travelItineray 被添加进入数据库中，同时 TravelRoutePictures 列表也会被添加进入数据库中
            // 当 TravelRoutePictures 被添加进入数据库时，因为每一项 TravelRoutePicture 的"Id"都被标记为 [DatabaseGenerated(DatabaseGeneratedOption.Identity)]，EF 会自动帮我们创建每一项 TravelRoutePicture 的"Id"
            // 同时 EF 也会智能地通过我们定义 Model 的方式了解到 TravelRoutePictures 列表中的每一项 TravelRoutePicture 都和原来的 TravelItineray 有关联 (因为我们把 "TravelRouteId" 标记为 [ForeignKey("TravelRouteId")])。EF 能智能地推断模型与模型之间的关系。因此 EF 能正确设置每一项加入数据库的 TravelRoutePicture 的 TravelRouteId

            await _context.TravelRoutes.AddAsync(travelRoute);       
            await SaveAsync();
        }

        public async Task AddTravelRoutePictureAsync(Guid travelRouteId, TravelRoutePicture travelRoutePicture)
        {
            if (travelRouteId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(travelRouteId));
            }
            if (travelRoutePicture == null)
            {
                throw new ArgumentNullException(nameof(travelRoutePicture));
            }
            travelRoutePicture.TravelRouteId = travelRouteId;

            await _context.TravelRoutePictures.AddAsync(travelRoutePicture);     // Notice the use of AddAsync()
            await SaveAsync();
        }

        public async Task DeleteTravelRouteAsync(TravelRoute travelRoute)
        {
            _context.TravelRoutes.Remove(travelRoute);       // EF 会把嵌套的子资源也全部一并删除
            await SaveAsync();
        }

        public async Task DeleteTravelRoutePictureAsync(TravelRoutePicture travelRoutePicture)
        {
            _context.TravelRoutePictures.Remove(travelRoutePicture);
            await SaveAsync();
        }

        public async Task<IEnumerable<TravelRoute>> GetTravelRoutesByIDListAsync(IEnumerable<Guid> Ids)
        {
            return await _context.TravelRoutes.Where(item => Ids.Contains(item.Id)).ToListAsync();
        }

        public async Task DeleteTravelRoutesAsync(IEnumerable<TravelRoute> travelRouteList)
        {
            _context.TravelRoutes.RemoveRange(travelRouteList);
            await SaveAsync();
        }

        public async Task<ShoppingCart> GetShoppingCartByUserIdAsync(string userId)
        {
            return await _context.ShoppingCarts.Include(s => s.User)
                                               .Include(s => s.ShoppingCartItems).ThenInclude(litem => litem.TravelRoute)
                                               .Where(s => s.UserId == userId)
                                               .FirstOrDefaultAsync();
        }

        public async Task CreateShoppingCartAsync(ShoppingCart shoppingCart)
        {
            await _context.ShoppingCarts.AddAsync(shoppingCart);
            await SaveAsync();
        }

        public async Task AddShoppingCartItemAsync(LineItem lineItem)
        {
            await _context.LineItems.AddAsync(lineItem);
            await SaveAsync();
        }

        public async Task<LineItem> GetShoppingCartItemAsync(Guid cartId, int itemId)
        {
            return await _context.LineItems.Where(item => item.ShoppingCartId == cartId && item.Id == itemId).FirstOrDefaultAsync();
        }
        
        public async Task DeleteShoppingCartItemAsync(LineItem lineItem)
        {
            _context.LineItems.Remove(lineItem);
            await SaveAsync();
        }

        public async Task<IEnumerable<LineItem>> GetShoppingCartItemsByIdsAsync(Guid cartId, IEnumerable<int> itemIds)
        {
            return await _context.LineItems.Where(item => item.ShoppingCartId == cartId && itemIds.Contains(item.Id)).ToListAsync();
        }

        public async Task DeleteShoppingCartItemsByIdsAsync(IEnumerable<LineItem> itemList)
        {
            _context.LineItems.RemoveRange(itemList);
            await SaveAsync();
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await SaveAsync();
        }

        public async Task<PaginationList<Order>> GetOrdersAsync(string userId, int pageNumber, int pageSize)
        {
            // return await _context.Orders.Where(item => item.UserId == userId).ToListAsync();
            
            IQueryable<Order> result = _context.Orders.Where(item => item.UserId == userId);
            return await PaginationList<Order>.CreateAsync(pageNumber, pageSize, result);
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await _context.Orders
                .Include(t => t.OrderItems).ThenInclude(orderItem => orderItem.TravelRoute)
                .Where(item => item.Id == orderId).FirstOrDefaultAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
