using System;
using Crypto.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using Crypto.Application.Banks.Queries;
using Crypto.Application.Cities.Queries;
using Crypto.Application.Common.Exceptions;
using Crypto.Application.Common.Interfaces;
using Crypto.Application.Provinces.Queries;
using Crypto.Application.Users.Documents.Commands;
using Crypto.Application.Users.Documents.Queries;
using Crypto.Application.Users.FinancialInformation.Commands;
using Crypto.Application.Users.FinancialInformation.Queries;
using Crypto.Application.Users.Profile.Commands;
using Crypto.Application.Users.Profile.Queries;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebUI.Models;
using ControllerBase = WebUI.Controllers.ControllerBase;

namespace WebUI.Areas.Panel.Controllers
{
    [Area("panel")]
    [Route("panel/user")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager, IMapper mapper,
            IIdentityService identityService, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _identityService = identityService;
            _signInManager = signInManager;
        }

        [Route("profile")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var model = await Mediator.Send(new GetUserProfileQuery {UserId = user.Id});

            ViewBag.ProvinceId = new SelectList(await Mediator.Send(new GetProvincesQuery()),
                "Id", "Title");
            ViewBag.CityId = new SelectList(await Mediator.Send(new GetCitiesByProvinceQuery
                {ProvinceId = model.ProvinceId}), "Id", "Title");

            return View(model);
        }

        [Route("profile")]
        [HttpPost]
        public async Task<IActionResult> Index(UpdateProfileCommand command)
        {
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var phoneNumberChanged = await Mediator.Send(command);
                if (phoneNumberChanged)
                {
                    var result = await _identityService.SendConfirmationSmsAsync(user.Id, command.PhoneNumber);
                    return result ? RedirectToAction("ConfirmPhoneNumber") : throw new Exception("خطا!");
                }

                return RedirectToAction("Index");
            }
            catch (ValidationException exception)
            {
                foreach (var (key, value) in exception.Errors)
                {
                    ModelState.AddModelError(key, value[0]);
                }

                ViewBag.ProvinceId = new SelectList(await Mediator.Send(new GetProvincesQuery()), "Id", "Title");
                return View(_mapper.Map<UpdateProfileCommand, UserProfileVm>(command));
            }
        }

        [Route("financial")]
        public async Task<IActionResult> Financial()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.BankId = new SelectList(await Mediator.Send(new GetBanksQuery()), "Id", "Title");
            return View(await Mediator.Send(new GetFinancialInformationQuery {UserId = user.Id}));
        }

        [Route("financial")]
        [HttpPost]
        public async Task<IActionResult> Financial(UpdateFinancialInfoCommand command)
        {
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                command.UserId = user.Id;
                await Mediator.Send(command);
                return RedirectToAction("Financial");
            }
            catch (ValidationException exception)
            {
                foreach (var (key, value) in exception.Errors)
                {
                    ModelState.AddModelError(key, value[0]);
                }

                return View(_mapper.Map<UpdateFinancialInfoCommand, FinancialInfoVm>(command));
            }
        }

        [Route("documents")]
        public async Task<IActionResult> Documents()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return View(await Mediator.Send(new GetDocumentQuery {UserId = user.Id}));
        }

        [Route("documents")]
        [HttpPost]
        public async Task<IActionResult> Documents(UpdateDocumentCommand command)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            try
            {
                command.UserId = user.Id;
                await Mediator.Send(command);
                return RedirectToAction("Documents");
            }
            catch (ValidationException exception)
            {
                foreach (var (key, value) in exception.Errors)
                {
                    ModelState.AddModelError(key, value[0]);
                }

                var model = await Mediator.Send(new GetDocumentQuery {UserId = user.Id});
                if (model == null) return View();

                model.NationalCode = command.NationalCode;
                model.BirthDate = command.BirthDate;
                return View(model);
            }
        }

        [Route("change_password")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Route("change_password")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var result = await _identityService.ChangePasswordAsync(user.Id, model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }

                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user);
            return RedirectToAction("Index", "Dashboard");
        }

        [Route("confirm_phone")]
        public IActionResult ConfirmPhoneNumber(string phoneNumber)
        {
            ViewData["PhoneNumber"] = phoneNumber;
            return View();
        }

        [Route("confirm_phone")]
        [HttpPost]
        public async Task<IActionResult> ConfirmPhoneNumber(ConfirmPhoneViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var result = await _identityService.ConfirmPhoneAsync(user.Id, model.PhoneNumber, model.Token);
                if (result)
                {
                    var command = new ConfirmPhoneNumberCommand {PhoneNumber = model.PhoneNumber};
                    await Mediator.Send(command);

                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("Token", "کد وارد شده معتبر نیست");
                return View(model);
            }

            return View(model);
        }

        [Route("load_cities")]
        [HttpPost]
        public async Task<IActionResult> LoadCities(int? provinceId)
        {
            if (provinceId == null)
                throw new Exception();

            return Json(await Mediator.Send(new GetCitiesByProvinceQuery {ProvinceId = provinceId}));
        }
    }
}