using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ticket.Domain.Identity;
using Ticket.Domain.Models;
using Ticket.Repository.Interface;

namespace Ticket.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<TicketAppUser> _userManager;
        private readonly SignInManager<TicketAppUser> _signInManager;
        private readonly IUserRepository _userRepository;

        public AccountController(UserManager<TicketAppUser> userManager,
            SignInManager<TicketAppUser> signInManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
        }


        public IActionResult Register()
        {
            UserRegistrationDto model = new UserRegistrationDto();
            return View(model);
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Register(UserRegistrationDto request)
        {
            if (ModelState.IsValid)
            {
                var userCheck = await _userManager.FindByEmailAsync(request.Email);
                if (userCheck == null)
                {
                    var user = new TicketAppUser
                    {
                        UserName = request.Email,
                        NormalizedUserName = request.Email,
                        Email = request.Email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        Cart = new CartModel()
                    };
                    var result = await _userManager.CreateAsync(user, request.Password);
                    
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        if (result.Errors.Count() > 0)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("message", error.Description);
                            }
                        }
                        return View(request);
                    }
                }
                else
                {
                    ModelState.AddModelError("message", "Email already exists.");
                    return View(request);
                }
            }
            return View(request);

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            UserLoginDto model = new UserLoginDto();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError("message", "Email not confirmed yet");
                    return View(model);

                }
                if (await _userManager.CheckPasswordAsync(user, model.Password) == false)
                {
                    ModelState.AddModelError("message", "Invalid credentials");
                    return View(model);

                }

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);

                if (result.Succeeded)
                {
                    await _userManager.AddClaimAsync(user, new Claim("UserRole", "Administrator"));
                    return RedirectToAction("Index", "Home");
                }
                else if (result.IsLockedOut)
                {
                    return View("AccountLocked");
                }
                else
                {
                    ModelState.AddModelError("message", "Invalid login attempt");
                    return View(model);
                }
            }
            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult MakeAdmin()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> MakeAdmin(string email)
        {
            TicketAppUser user = _userRepository.GetByEmail(email);
            await _userManager.AddToRoleAsync(user,"Administrator");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ImportUsers()
        {
            var path = "./Users.xlsx";
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        var user = new TicketAppUser
                        {
                            UserName = reader.GetValue(0).ToString(),
                            NormalizedUserName = reader.GetValue(0).ToString(),
                            Email = reader.GetValue(0).ToString(),
                            EmailConfirmed = true,
                            PhoneNumberConfirmed = true,
                            Cart = new CartModel()
                        };

                        var result = await _userManager.CreateAsync(user, reader.GetValue(1).ToString());
                        if (result.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(user, reader.GetValue(3).ToString());
                        }
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
