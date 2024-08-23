using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebApplication1.Controllers
{
    [Route("api/api2")]
    // public class TestController
    // [Controller]
    // public class TestController : Controller

    // 规范: 文件名和类名都以 Controller 结尾，并且让类继承于 Controller 父类，以便使用 Controller 父类的功能


    public class TestController : Controller          // Controller 类需要被标记为 public
    {
        [HttpGet]
        public IEnumerable<string> Get()             // Action 函数需要被标记为 public
        {
            return new string[] { "value1", "value2" };
        }
    }
}
