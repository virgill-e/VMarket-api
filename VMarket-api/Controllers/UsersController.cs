using Microsoft.AspNetCore.Mvc;
using VMarket_api.Services;

namespace UserApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public ActionResult<string> Get()
    {
        return Ok(_userService.GetHelloWorld());
    }
}