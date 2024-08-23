using Newtonsoft.Json.Linq;
using System.Diagnostics.Eventing.Reader;
using System.Text.RegularExpressions;

namespace WebApplication1.ResourceParameters
{
    public class TravelRouteResourceParameters         // api/travelRoutes?keyword=xxx&rating=xxx
    {
        public string Keyword { get; set; }
        public string OperatorType { get; private set; } = "";
        public int RatingValue { get; private set; } = -1;
        private string _rating;

        // Rating 需要同时提供 get 和 set 方法
        // ASP.NET Core uses model binding to automatically create instances of complex types from incoming requests (like query parameters, form data, etc.)
        // To allow model binding to work, the property Rating needs to provide both a getter and a setter (although we don't need the getter explicitly)

        public string Rating {         
            get
            {
                return _rating;
            }
            set {
                if (!string.IsNullOrEmpty(value)) {
                    Regex regex = new Regex(@"([A-Za-z0-9\-]+)(\d+)");
                    Match match = regex.Match(value);
                    if (match.Success) {
                        OperatorType = match.Groups[1].Value;
                        RatingValue = int.Parse(match.Groups[2].Value);
                    }
                }
            }
        }

        public string OrderBy { get; set; }

        public string Fields { get; set; }
    }
}
