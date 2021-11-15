using EjemploApiRest.WebApi.DTOs;
using MedicalApp.Application;
using MedicalApp.Entities;
using MedicalApp.Services;
using MedicalApp.WebApi.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        ITokenHandlerService _tokenHandlerService;
        IDoctorApplication _doctorApplication;

        public AuthController(UserManager<ApplicationUser> userManager, ITokenHandlerService tokenHandlerService, IDoctorApplication doctorApplication)
        {
            _userManager = userManager;
            _tokenHandlerService = tokenHandlerService;
            _doctorApplication = doctorApplication;
        }

        [HttpPost]
        [Route("RegisterDoctor")]
        public async Task<IActionResult> RegisterDoctor([FromBody] RegisterDoctorRequestDTO registerDoctorRequestDTO)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(registerDoctorRequestDTO.Email);
                if (existingUser != null)
                {
                    return BadRequest(new RegisterDoctorResponseDTO() { 
                        Register = false,
                        Errors = new List<string>()
                        {
                            "El correo electronico indicado ya existe"
                        }
                    });
                }

                var user = new ApplicationUser()
                {
                     UserName = registerDoctorRequestDTO.Email,
                     Email = registerDoctorRequestDTO.Email,
                     IsActive = true
                };
                var isCreated = await _userManager.CreateAsync(user, registerDoctorRequestDTO.Password);

                if (isCreated.Succeeded)
                {
                    var res = await _userManager.AddToRoleAsync(user, "Doctor");

                    var d = new Doctor()
                    {
                        Name = registerDoctorRequestDTO.Name,
                        LastName = registerDoctorRequestDTO.LastName,
                        BirthDate = registerDoctorRequestDTO.BirthDate,
                        Gender = registerDoctorRequestDTO.Gender,
                        Phone = registerDoctorRequestDTO.Phone,
                        Speciality = registerDoctorRequestDTO.Speciality,
                        Summary = registerDoctorRequestDTO.Summary,
                        IsAvailable = true,
                        UserId = user.Id,
                        CreatedDate = DateTime.Now
                    };

                    await _doctorApplication.SaveAsync(d);

                    return Ok(new RegisterDoctorResponseDTO() { 
                        Register = true
                    });
                }
                else
                {
                    return BadRequest(new RegisterDoctorResponseDTO(){
                        Register = false,
                        Errors = isCreated.Errors.Select(x => x.Description).ToList()
                    });
                }
            }
            else
            {
                return BadRequest(new RegisterDoctorResponseDTO() { 
                    Register = false,
                    Errors = new List<string>()
                    {
                        "Se produjo algun error al registrar el usuario"
                    }
                });
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDTO user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);                
                
                if (existingUser == null)
                {
                    return BadRequest(new UserLoginResponseDTO()
                    {
                        Login = false,
                        Errors = new List<String>()
                        {
                            "Correo electrónico o contraseña incorrecto"
                        }
                    });
                }

                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);
                if (isCorrect)
                {
                    var roles = await _userManager.GetRolesAsync(existingUser);
                    return Ok(new UserLoginResponseDTO()
                    {
                        Login = true,
                        UserId = existingUser.Id,
                        UserName = existingUser.UserName,
                        Token = string.Empty,
                        Roles = roles.ToList()
                    });
                }
                else
                {
                    return BadRequest(new UserLoginResponseDTO()
                    {
                        Login = false,
                        Errors = new List<String>()
                        {
                            "Correo electrónico o contraseña incorrecto"
                        }
                    });

                }
            }
            else
            {
                return BadRequest(new UserLoginResponseDTO()
                {
                    Login = false,
                    Errors = new List<String>()
                    {
                        "Correo electrónico o contraseña incorrecto"
                    }
                });

            }
        }
    }
}
