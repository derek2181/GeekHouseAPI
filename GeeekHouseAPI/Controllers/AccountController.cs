using GeeekHouseAPI.Models;
using GeeekHouseAPI.Repository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeeekHouseAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
       
        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpModel signUpModel)
        {
            var result = await _accountRepository.SignUpAsync(signUpModel);
            if (result.Succeeded)
            {
                return Ok(result.Succeeded);
            }

            return Unauthorized();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] SignInModel signInModel)
        {
            var result = await _accountRepository.LoginAsync(signInModel);
            if (result == null)
            {
                return Unauthorized(new GenericResponse<object>() {
                    code=403,
                    message="Usuario o contraseña incorrectos."
                });
            }

            var genericResponse = new GenericResponse<object>()
            {
                code = 200,
                message = "The login was successfull",
                data = result
            };

            return Ok(genericResponse);
        }
    }
}
