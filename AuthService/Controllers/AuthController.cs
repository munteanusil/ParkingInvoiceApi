using AuthService.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("auth")]
public class AuthController : Controller
{
    private readonly UserManager<AppUser> _users;
    private readonly SignInManager<AppUser> _signIn;

    public AuthController(UserManager<AppUser> users, SignInManager<AppUser> signIn)
    { _users = users; _signIn = signIn; }

    [HttpGet("register")]
    public IActionResult Register() => View();

    public record RegisterVm(string Email, string Password, string? CustomerNumber);

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterVm vm)
    {
        var user = new AppUser { UserName = vm.Email, Email = vm.Email, CustomerNumber = vm.CustomerNumber };
        var res = await _users.CreateAsync(user, vm.Password);
        if (!res.Succeeded) { foreach (var e in res.Errors) ModelState.AddModelError("", e.Description); return View(vm); }

        var token = await _users.GenerateEmailConfirmationTokenAsync(user);
        var link = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, token }, Request.Scheme)!;
        // TODO: trimite link pe email (SMTP/provider)

        TempData["msg"] = "Cont creat. Verifică emailul pentru confirmare.";
        return RedirectToAction("Login");
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        var user = await _users.FindByIdAsync(userId);
        if (user == null) return NotFound();
        var res = await _users.ConfirmEmailAsync(user, token);
        return res.Succeeded ? View("ConfirmOk") : BadRequest("Token invalid");
    }

    [HttpGet("login")]
    public IActionResult Login() => View();

    public record LoginVm(string Email, string Password, bool RememberMe);

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginVm vm, string? returnUrl = null)
    {
        var user = await _users.FindByEmailAsync(vm.Email);
        if (user == null || !await _users.IsEmailConfirmedAsync(user))
        { ModelState.AddModelError("", "Email sau parolă incorecte / email neconfirmat."); return View(vm); }

        var res = await _signIn.PasswordSignInAsync(user, vm.Password, vm.RememberMe, lockoutOnFailure: true);
        if (!res.Succeeded)
        { ModelState.AddModelError("", res.IsLockedOut ? "Cont blocat temporar." : "Credențiale invalide."); return View(vm); }

        return Redirect(returnUrl ?? "/");
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    { await _signIn.SignOutAsync(); return Redirect("/"); }
}
