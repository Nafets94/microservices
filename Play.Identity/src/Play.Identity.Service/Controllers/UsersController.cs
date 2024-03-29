﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Play.Identity.Service.Dtos;
using Play.Identity.Service.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Duende.IdentityServer.IdentityServerConstants;

namespace Play.Identity.Service.Controllers
{
    [ApiController]
    [Route("users")]
    [Authorize(Policy = LocalApi.PolicyName)]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> Get()
        {
            var users = _userManager.Users.ToList()
                                          .Select(user => user.AsDto());

            return Ok(users);
        }

        // /users/{123}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetByIdAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null) return NotFound();

            return user.AsDto();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, [FromBody]UpdateUserDto userDto)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null) return NotFound();

            user.Email = userDto.Email;
            user.UserName = userDto.Email;
            user.Gil = userDto.Gil;

            await _userManager.UpdateAsync(user);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsnyc(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null) return NotFound();

            await _userManager.DeleteAsync(user);

            return NoContent();
        }
    }
}
