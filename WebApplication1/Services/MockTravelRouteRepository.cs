//using System;
//using System.Collections.Generic;
//using System.Linq;
//using WebApplication1.Models;

//namespace WebApplication1.Services
//{
//    public class MockTravelRouteRepository : ITravelRouteRepository
//    {
//        private List<TravelRoute> _routes;

//        public MockTravelRouteRepository()
//        {
//            if (_routes == null)
//            {
//                InitializeTouristRoutes();
//            }
//        }

//        private void InitializeTouristRoutes()
//        {
//            _routes = new List<TravelRoute>
//            {
//                new TravelRoute {
//                    Id = Guid.NewGuid(),
//                    Title = "黄山",
//                    Description="黄山真好玩",
//                    OriginalPrice = 1299,
//                    Features = "<p>吃住行游购娱</p>",
//                    Fees = "<p>交通费用自理</p>",
//                    Notes="<p>小心危险</p>"
//                },
//                new TravelRoute {
//                    Id = Guid.NewGuid(),
//                    Title = "华山",
//                    Description="华山真好玩",
//                    OriginalPrice = 1299,
//                    Features = "<p>吃住行游购娱</p>",
//                    Fees = "<p>交通费用自理</p>",
//                    Notes="<p>小心危险</p>"
//                }
//            };
//        }

//        public IEnumerable<TravelRoute> GetTravelRoutes()
//        {
//            return _routes;
//        }

//        public TravelRoute GetTravelRoute(Guid id)
//        {
//            // linq
//            return _routes.FirstOrDefault(n => n.Id == id);
//        }
//    }
//}
