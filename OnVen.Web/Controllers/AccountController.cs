using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnVen.Common.Entities;
using OnVen.Common.Enum;
using OnVen.Common.Responses;
using OnVen.Web.Data;
using OnVen.Web.Data.Entities;
using OnVen.Web.Helper;
using OnVen.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnVen.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IMailHelper _mailHelper;

        public AccountController(
            DataContext context,
            IUserHelper userHelper,
            IImageHelper imageHelper,
            ICombosHelper combosHelper,
            IMailHelper mailHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _combosHelper = combosHelper;
            _mailHelper = mailHelper;
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users
                .Include(u => u.City)
                .ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            AddUserViewModel model = new AddUserViewModel
            {
                Countries = _combosHelper.GetComboCountries(),
                Departments = _combosHelper.GetComboDepartments(0),
                Cities = _combosHelper.GetComboCities(0),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                string imageId = string.Empty;

                if (model.ImageFile != null)
                {
                    string guid = Guid.NewGuid().ToString() + ".jpg";
                    string ruta = "wwwroot\\Users";
                    imageId = await _imageHelper.UploadImage(model.ImageFile, ruta, guid);
                    model.ImageId = imageId;
                }

                User user = await _userHelper.AddUserAsync(model, imageId, UserType.Admin);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "This email is already used.");
                    model.Countries = _combosHelper.GetComboCountries();
                    model.Departments = _combosHelper.GetComboDepartments(model.CountryId);
                    model.Cities = _combosHelper.GetComboCities(model.DepartmentId);
                    return View(model);
                }

                string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                string tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendMail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                    $"To allow the user, " +
                    $"plase click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");
                if (response.IsSuccess)
                {
                    ViewBag.Message = "The instructions to allow your user has been sent to email.";
                    return View(model);
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            model.Countries = _combosHelper.GetComboCountries();
            model.Departments = _combosHelper.GetComboDepartments(model.CountryId);
            model.Cities = _combosHelper.GetComboCities(model.DepartmentId);

            return View(model);
        }




        //Para Redireccionamiento de paginas No Encontradas
        public IActionResult NotAuthorized()
        {
            return View();
        }

        public async Task<IActionResult> ConfirmEmailSend()
        {
            return View();
        }


        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel());
        }

        //Valida el Correo del usuario para Activar la cuenta
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(new Guid(userId));
            if (user == null)
            {
                return NotFound();
            }

            IdentityResult result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            return View();
        }

        //Recuperacion de la Clave del Usuario con Token de Seguridad
        public IActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userHelper.GetUserAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email doesn't correspont to a registered user.");
                    return View(model);
                }

                string myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
                string link = Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);
                _mailHelper.SendMail(model.Email, "Password Reset", $"<h1>Password Reset</h1>" +
                    $"To reset the password click in this link:</br></br>" +
                    $"<a href = \"{link}\">Reset Password</a>");
                ViewBag.Message = "The instructions to recover your password has been sent to email.";
                return View();

            }

            return View(model);
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            User user = await _userHelper.GetUserAsync(model.UserName);
            if (user != null)
            {
                IdentityResult result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    ViewBag.Message = "Password reset successful.";
                    return View();
                }

                ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }

            ViewBag.Message = "User not found.";
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Email or password incorrect.");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            AddUserViewModel model = new AddUserViewModel
            {
                Countries = _combosHelper.GetComboCountries(),
                Departments = _combosHelper.GetComboDepartments(0),
                Cities = _combosHelper.GetComboCities(0),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                string imageId = string.Empty;

                if (model.ImageFile != null)
                {
                    string guid = Guid.NewGuid().ToString() + ".jpg";
                    string ruta = "wwwroot\\Users";
                    imageId = await _imageHelper.UploadImage(model.ImageFile, ruta, guid);
                    model.ImageId = imageId;
                }

                User user = await _userHelper.AddUserAsync(model, imageId, UserType.User);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "This email is already used.");
                    model.Countries = _combosHelper.GetComboCountries();
                    model.Departments = _combosHelper.GetComboDepartments(model.CountryId);
                    model.Cities = _combosHelper.GetComboCities(model.DepartmentId);
                    return View(model);
                }

                //Para validar el Correo y poder Activar la cuenta del cliente
                string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                string tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendMail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                    $"To allow the user, " +
                    $"plase click in this link:<p><a href = \"{tokenLink}\">Confirm Email</a></p>");
                if (response.IsSuccess)
                {
                    //ViewBag.Message = "The instructions to allow your user has been sent to email.";
                    //return View(model);
                    return RedirectToAction("ConfirmEmailSend");
                }

                ModelState.AddModelError(string.Empty, response.Message);

            }

            model.Countries = _combosHelper.GetComboCountries();
            model.Departments = _combosHelper.GetComboDepartments(model.CountryId);
            model.Cities = _combosHelper.GetComboCities(model.DepartmentId);
            ViewBag.Message = "Somthing went wrong!!!";
            return View(model);
        }


        public async Task<IActionResult> ChangeUser()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            Department department = await _context.Departments.FirstOrDefaultAsync(d => d.Cities.FirstOrDefault(c => c.CityId == user.City.CityId) != null);
            if (department == null)
            {
                department = await _context.Departments.FirstOrDefaultAsync();
            }

            Country country = await _context.Countries.FirstOrDefaultAsync(c => c.Departments.FirstOrDefault(d => d.DepartmentId == department.DepartmentId) != null);
            if (country == null)
            {
                country = await _context.Countries.FirstOrDefaultAsync();
            }

            EditUserViewModel model = new EditUserViewModel
            {
                Address = user.Address,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                ImageId = user.ImageId,
                Cities = _combosHelper.GetComboCities(department.DepartmentId),
                CityId = user.City.CityId,
                Countries = _combosHelper.GetComboCountries(),
                CountryId = country.CountryId,
                DepartmentId = department.DepartmentId,
                Departments = _combosHelper.GetComboDepartments(country.CountryId),
                Id = user.Id,
                Document = user.Document
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                string imageId = model.ImageId;

                if (model.ImageFile != null)
                {
                    string guid;

                    if (model.ImageId == null)
                    {
                        guid = Guid.NewGuid().ToString() + ".jpg";
                    }
                    else
                    {
                        guid = model.ImageId;
                    }
                    string ruta = "wwwroot\\Users";
                    imageId = await _imageHelper.UploadImage(model.ImageFile, ruta, guid);
                    model.ImageId = imageId;
                }

                User user = await _userHelper.GetUserAsync(User.Identity.Name);

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Address = model.Address;
                user.PhoneNumber = model.PhoneNumber;
                user.ImageId = imageId;
                user.City = await _context.Cities.FindAsync(model.CityId);
                user.Document = model.Document;

                await _userHelper.UpdateUserAsync(user);
                return RedirectToAction("Index", "Home");
            }

            model.Cities = _combosHelper.GetComboCities(model.DepartmentId);
            model.Countries = _combosHelper.GetComboCountries();
            model.Departments = _combosHelper.GetComboDepartments(model.CityId);
            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserAsync(User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User no found.");
                }
            }

            return View(model);
        }


        public JsonResult GetDepartments(int idcontry)
        {
            //Country country = _context.Countries
            //    .Include(c => c.Departments)
            //    .FirstOrDefault(c => c.CountryId == countryId);
            //if (country == null)
            //{
            //    return null;
            //}

            var data = _context.Departments.Where(o => o.CountryId == idcontry);
            if (data == null)
            {
                return null;
            }

            return Json(data.OrderBy(o => o.Name));
        }

        public JsonResult GetCities(int IdDepartment)
        {
            //Department department = _context.Departments
            //    .Include(d => d.Cities)
            //    .FirstOrDefault(d => d.DepartmentId == IdDepartment);

            var data = _context.Cities.Where(c => c.DepartmentId == IdDepartment);
            if (data == null)
            {
                return null;
            }

            return Json(data.OrderBy(c => c.Name));
        }

    }
}
