﻿#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DIMSApis.Models.Data;
using DIMSApis.Interfaces;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;

namespace DIMSApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _auth;
        private readonly ITokenService _tokenService;
        private readonly DIMSContext _context;

        public AuthController(IAuth auth, ITokenService tokenService , DIMSContext context)
        {
            _auth = auth;
            _tokenService = tokenService;
            _context   = context;
        }
        // GET: api/Users
        [HttpGet("GetAllUsers_cheat")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterInput user)
        {
            if (await _auth.UserExists(user.Email))
                return BadRequest("Email already exists");
            if (await _auth.Register(user))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("login-user")]
        public async Task<IActionResult> Login(LoginInput userIn)
        {
            var user = await _auth.Login(userIn);

            if (user == null)
                return Unauthorized();

            LoginOutput login = new()
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            };
            return Ok(login);
        }

        [HttpPost("login-admin")]
        public async Task<IActionResult> LoginAdmin(LoginInput userIn)
        {
            var user = await _auth.LoginAdmin(userIn);

            if (user == null)
                return Unauthorized();

            LoginOutput login = new()
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            };
            return Ok(login);
        }

        [HttpPost("forgot-code-mail")]
        public async Task<IActionResult> ForgotCodeMailSend(ForgotCodeMailInput mail)
        {
            if (await _auth.GetForgotCodeMailSend(mail))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("forgot-code-vertify")]
        public async Task<IActionResult> ForgotCodeVertify(ForgotCodeVertifyInput code)
        {
            if (await _auth.ForgotCodeVertify(code))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("forgot-pass-change")]
        public async Task<IActionResult> ForgoPassChange(ForgotPassInput pass)
        {
            if (await _auth.UpdateNewPass(pass))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

    }   
}
