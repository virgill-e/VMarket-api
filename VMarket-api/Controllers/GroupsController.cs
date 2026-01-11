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

        
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetMyGroups()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _groupService.GetGroupsAsync(userId);
            if (!result.Success)
                return BadRequest(new { Errors = result.Errors });
            
            return Ok(result.Data);
        }

        [HttpGet]
        [Authorize]
        [Route("{groupId}")]
        public async Task<ActionResult> GetGroupById([FromRoute] string groupId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _groupService.GetGroupByIdAsync(userId, groupId);
            if (!result.Success)
                return BadRequest(new { Errors = result.Errors });
            return Ok(result.Data);
        }
    }
    