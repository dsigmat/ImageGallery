using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageGallery.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ImageGallery.Controllers
{
    public class AuthController : Controller
    {
        //Поскольку в классе Startup были добавлены сервисы Identity, то здесь в контроллере 
        //через конструктор мы можем их получить. В данном случае мы получаем сервис по 
        //управлению пользователями - сервис SignInManager, который позволяет 
        //аутентифицировать пользователя и устанавливать или удалять его куки.
        private SignInManager<IdentityUser> _signInManager;
        public AuthController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            //В Post-версии метода Login получаем данные из представления в виде модели LoginViewModel. 
            //Всю работу по аутентификации пользователя выполняет метод signInManager.
            //PasswordSignInAsync(). Этот метод принимает логин и пароль пользователя. 
            var result = await _signInManager.PasswordSignInAsync(vm.UserName, vm.Password, false, false);
            return RedirectToAction("Index", "Panel");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Gallery");
        }
    }
}