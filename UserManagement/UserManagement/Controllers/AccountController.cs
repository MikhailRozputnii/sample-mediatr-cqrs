using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserManagement.Commands;
using UserManagement.Common.Exceptions;
using UserManagement.Domain;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        private readonly SignInManager<User> _signInManager;

        public AccountController(ILoggerFactory logger, IMediator mediator, SignInManager<User> signInManager)
        {
            _mediator = mediator;
            _logger = logger.CreateLogger(nameof(AccountController));
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            _logger.LogInformation($"Attempting to registration user on the system. {model.Email}");
            if (ModelState.IsValid)
            {
                try
                {
                    await _mediator.Send(new RegisterUser
                    {
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Password = model.Password
                    });

                    return RedirectToAction("Index","User");
                }
                catch (ModelValidationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(model);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Unexpected error {ex.Message}. Trace {ex.StackTrace}");
                    return View(new RegisterViewModel { Message = MessagesFields.BadRequest });
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl= returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            _logger.LogInformation($"Attempting to sign in of the account. {model.Email}");
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        if (!String.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        _logger.LogInformation($"Incorrect login {model.Email} and / or password {model.Password}");
                        ModelState.AddModelError("", "Incorrect login and / or password");
                    }
            }
            return View(model);
        }

        public async Task<IActionResult> Logoff()
        {
            _logger.LogInformation($"Attempting to sign out of the account. {User.Identity.Name}");
            await _signInManager.SignOutAsync();
            if (User.Identity.IsAuthenticated)
            {
                _logger.LogWarning($"There was an error logging out of the system. {User.Identity.Name}");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}