using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers;

public class AccountController : Controller
{
    private UserManager<IdentityUser> userManager;
    private SignInManager<IdentityUser> signInManager;
    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    public ViewResult Login(string returnUrl)
    {
        return View(new LoginModel {ReturnUrl = returnUrl});
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        if(ModelState.IsValid)
        {
            IdentityUser user = await userManager.FindByNameAsync(loginModel.Name);
            if (user is not null)
            {
                await signInManager.SignOutAsync();
                if ((await signInManager.PasswordSignInAsync(user, loginModel.Password, false, false)).Succeeded)
                {
                    return Redirect(loginModel?.ReturnUrl ?? "/Admin");
                }
            }
            ModelState.AddModelError("", "Invalid name or password");
        }
        return View(loginModel);
    }

    [Authorize]
    public async Task<RedirectResult> Logout(string returnUrl = "/")
    {
        await signInManager.SignOutAsync();
        return Redirect(returnUrl);
    }
}