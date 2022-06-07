using Marmara.API.Authentication;
using Marmara.Common;
using Marmara.Common.Model;
using Marmara.Data.Concrete;
using Marmara.Data.Entity;
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

namespace Marmara.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("createAdmin")]
        public async Task<IActionResult> CreateAdmin()
        {
            var model = new RegisterModel()
            {
                Username = "admin",
                Password = "Admin@1",
                Email = "admin@admin.com"
            };
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await roleManager.RoleExistsAsync(Const.RoleAdmin))
                await roleManager.CreateAsync(new IdentityRole(Const.RoleAdmin));
            if (!await roleManager.RoleExistsAsync(Const.RoleUser))
                await roleManager.CreateAsync(new IdentityRole(Const.RoleUser));

            if (await roleManager.RoleExistsAsync(Const.RoleAdmin))
            {
                await userManager.AddToRoleAsync(user, Const.RoleAdmin);
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {

            //"username":"mehmet",
            //"email":"mehmet.sensoy@outlook.com",
            //"password":"Mehmet@1"
            //model.Username = "mehmet";
            //model.Password = "Mehmet@1";
            //model.Username = "admin";
            //model.Password = "Admin@1";

            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            Console.WriteLine($"Model Username: {model.Username}");
            Console.WriteLine($"Model Password: {model.Password}");
            Console.WriteLine($"Model Email: {model.Email}");

            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            ControlUser controlUser = new ControlUser()
            {
                Email = model.Email,
                Username = model.Username
            };

            ControlUserRepository controlUserRepository = new ControlUserRepository();
            var res = await controlUserRepository.CreateNewUserAsync(controlUser);
            Console.WriteLine(res == 1 ? "Created" : "Error");

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await roleManager.RoleExistsAsync(Const.RoleAdmin))
                await roleManager.CreateAsync(new IdentityRole(Const.RoleAdmin));
            if (!await roleManager.RoleExistsAsync(Const.RoleUser))
                await roleManager.CreateAsync(new IdentityRole(Const.RoleUser));

            if (await roleManager.RoleExistsAsync(Const.RoleAdmin))
            {
                await userManager.AddToRoleAsync(user, Const.RoleAdmin);
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }


        [HttpPost]
        [Route("changePassword")]
        public async Task<string> ChangePassword([FromBody] SettingPasswordModel model)
        {
            Console.WriteLine("ov:" + model.ToString());
            Console.WriteLine("id:" + model.AppUserGuid.ToString());
            Console.WriteLine("pass:" + model.Password);
            Console.WriteLine("new pass:" + model.NewPassword);
            var appUser = await userManager.FindByIdAsync(model.AppUserGuid);
            var result = await userManager.ChangePasswordAsync(appUser, model.Password, model.NewPassword);
            if (result.Succeeded)
            {
                return 1.ToString(); ;
            }
            else
            {
                return 0.ToString(); ;
            }
        }


        [HttpGet]
        [Route("deleteUser")]
        public async Task<string> DeleteUser(int id)
        {
            ControlUserRepository controlUserRepository = new ControlUserRepository();
            var res = await controlUserRepository.DeleteUserAsync(id);
            var controlUser = await controlUserRepository.GetUserAsync(id);
            Console.WriteLine($"controlUser Id : {controlUser.Id}");
            Console.WriteLine($"controlUser Username : {controlUser.Username}");
            var appUser = await userManager.FindByNameAsync(controlUser.Username);
            Console.WriteLine($"appUser Username : {appUser.Id}");


            var result = await userManager.DeleteAsync(appUser);
            if (result.Succeeded && res>0)
            {
                return 1.ToString(); ;
            }
            else
            {
                return 0.ToString(); ;
            }
        }

        [HttpGet]
        [Route("getUserId")]
        public async Task<string> GetUserId(string username)
        {
            var appUser = await userManager.FindByNameAsync(username);
            return appUser.Id;
        }

        [HttpGet]
        [Route("getUserList")]
        public async Task<List<ControlUser>> GetUserList()
        {
            ControlUserRepository controlUserRepository = new ControlUserRepository();
            var list = await controlUserRepository.GetUserListAsync();
            return list;
        }
    }
}