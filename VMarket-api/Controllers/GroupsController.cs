using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VMarket_api.Models.DTOs;
using VMarket_api.Services;

namespace UserApi.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost]
        [RequestSizeLimit(10 * 1024 * 1024)] // 10Mo
        public async Task<ActionResult<GroupDto>> CreateGroup([FromForm] CreateGroupDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _groupService.CreateGroupAsync(userId, dto);

            if (!result.Success)
                return BadRequest(new { Errors = result.Errors });

            return Created("api/groups", null);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetGroup(string id)
        {
            // Implementation for retrieving a group by id
            return Ok();
        }
    }
    