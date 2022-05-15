using API.DTOs;
using API.Interfaces;
using AutoMapper;
using Data;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers() // Gets multiple users
        {
            var users = await _userRepository.GetMembersAsync();
            return Ok(users);  // Placed "Ok" because the resulting IEnumerable is a different type than the function's IEnumerable return type
        }

        // api/users/:id
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username) // Gets one user
        {
            return await _userRepository.GetMemberAsync(username);
        }
    }
}
