using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RavenTodoApp.Persistence;

namespace RavenTodoApp.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    private string GenerateUserToken()
    {
        var token = (value: "", isUnique: false);

        while (!token.isUnique)
        {
            var randomNumber = new Random().Next();

            token.value = Convert.ToBase64String(Encoding.UTF8.GetBytes(randomNumber.ToString()));
            
            token.isUnique = _userRepository.GetUserByToken(token.value) is null;
        }
        
        return token.value;
    }

    private void CreateNewUser(string userToken)
    {
        _userRepository.InsertOrUpdate(new User()
        {
            UserToken = userToken
        });
    }

    [HttpGet("signout")] 
    public async Task<IActionResult> SignOutFromSession()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return Ok();
    }
    
    [HttpGet("authorize/{userToken}")]
    public async Task<IActionResult> ValidateTokenOrHandleNewToken(string? userToken)
    {
        var isNew = false;
        
        if (userToken is null || _userRepository.GetUserByToken(userToken) is null)
        {
            userToken = GenerateUserToken();
            CreateNewUser(userToken);
            isNew = true;
        }

        var claimsIdentity = new ClaimsIdentity(
            new List<Claim> 
            {
                new (ClaimTypes.Name, userToken),
            },
            CookieAuthenticationDefaults.AuthenticationScheme);
        
        var authOptions = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.Now.AddMinutes(5)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authOptions);
        
        return Ok(new {userToken, isNew});
    }
}