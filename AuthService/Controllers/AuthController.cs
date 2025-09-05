using AuthService.Domain;
using AuthService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
   
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _users;
        private readonly SignInManager<AppUser> _signIn;

        public AuthController(UserManager<AppUser> _users, SignInManager<AppUser> signIn)
        {
            this._users = _users;
            _signIn = signIn;
        }
        [HttpGet("register")]
        public IActionResult Register() => View();

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterVm vm)
        {
            var user = new AppUser { UserName = vm.Email, Email = vm.Email, CustomerName = vm.CustomerNumber };
            var res = await _users.CreateAsync(user, vm.Password);
            if (!res.Succeeded) 
            { foreach(var e in res.Errors) 
                    ModelState.AddModelError("", e.Description);
                return View(vm); 
            }

            var token = await _users.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, token }, Request.Scheme)!;
            // TODO: trimite link pe email (SMTP/provider)

            TempData["msg"] = "Cont creat. Verifică emailul pentru confirmare.";
            return RedirectToAction("Login");
        }
        //[HttpGet("confirm-email")]
        //public async Task<IActionResult> ConfirmEmail(string userId,string token)
        //{
            
        //}

        //[HttpPost("logout")]
        //public async Task<IActionResult> Logout()
        //{

        //}

    }
}
