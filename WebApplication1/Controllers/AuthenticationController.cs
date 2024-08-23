using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;      // 注入 appsettings.json 配置服务依赖
        private readonly UserManager<ApplicationUser> _userManager;      // Identity 框架的 UserManager 工具 (类似于数据仓库 Repository)
        private readonly SignInManager<ApplicationUser> _signInManager;
        private ITravelRouteRepository _travelRouteRepository;

        public AuthenticationController(
            IConfiguration configuration, 
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITravelRouteRepository travelRouteRepository)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _travelRouteRepository = travelRouteRepository;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            // 1. 验证用户名与密码
            var loginResult = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, false, false);

            if (!loginResult.Succeeded)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByNameAsync(loginDTO.Email);     // 获得用户数据


            // 2. 创建 jwt
            // header
            var signingAlgorithm = SecurityAlgorithms.HmacSha256;
            // payload
            var claims = new List<Claim>
            {
                // sub
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                // new Claim(ClaimTypes.Role, "Admin")
            };
            var roleNames = await _userManager.GetRolesAsync(user);

            foreach(var roleName in roleNames)
            {
                var roleClaim = new Claim(ClaimTypes.Role, roleName);
                claims.Add(roleClaim);
            }


            // signature
            var secretByte = Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]);
            var signingKey = new SymmetricSecurityKey(secretByte);
            var signingCredentials = new SigningCredentials(signingKey, signingAlgorithm);

            var token = new JwtSecurityToken(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims,
                notBefore: DateTime.Now,     // 发布时间
                expires: DateTime.Now.AddDays(1),
                signingCredentials
            );

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            // 3. 返回 200 ok + jwt
            return Ok(tokenStr);
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            // 1. 使用用户名创建用户对象
            var user = new ApplicationUser()
            {
                UserName = registerDTO.Email,
                Email = registerDTO.Email
            };

            // 2. Hash密码，把 Hash 后的密码和用户对象保存进入数据库中
            var result = await _userManager.CreateAsync(user, registerDTO.Password);    // 密码要求包含大小写和特殊字符

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            // 3. 创建购物车

            var shoppingCart = new ShoppingCart()
            {
                Id = Guid.NewGuid(),
                UserId = user.Id
            };

            await _travelRouteRepository.CreateShoppingCartAsync(shoppingCart);

            // 4. return 200
            return Ok();
        } 
    }
}
