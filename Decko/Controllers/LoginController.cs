using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Decko.Factory;
using Decko.Models.Login;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Decko.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        [HttpGet, AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var userManager = new DeckoUserManager(new DeckoUserStore(new Entities()));
                var user = await userManager.FindAsync(model.User, model.Pass);

                if (user != null)
                {
                    var authenticationManager = HttpContext.GetOwinContext().Authentication;
                    authenticationManager.SignOut( new[] {
                        DefaultAuthenticationTypes.ApplicationCookie,
                        DefaultAuthenticationTypes.ExternalCookie,
                        DefaultAuthenticationTypes.ExternalBearer
                    });
                    var claimsIdentity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = model.RememberMe }, claimsIdentity);
                    return RedirectToLocal(returnUrl);
                }
            }
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        public ActionResult LogOut()
        {
            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut( new[] {
                DefaultAuthenticationTypes.ApplicationCookie,
                DefaultAuthenticationTypes.ExternalCookie,
                DefaultAuthenticationTypes.ExternalBearer
            });

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPassViewModel model)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost, AllowAnonymous]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (HttpContext.Request.Path.Contains("Register"))
            {
                if (ModelState.IsValid)
                {
                    var userManager = new DeckoUserManager(new DeckoUserStore(new Entities()));
                    DeckoUser newUser = new DeckoUser() {
                        UserName = model.User,
                        Password = model.Pass,
                        Email = model.Email,
                        Dci = "",
                        Admin = false,
                        Fname = "",
                        Lname = "",
                        Gender = "",
                        IpAddress = HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? HttpContext.Request.ServerVariables["REMOTE_ADDR"]
                    };
                    IdentityResult result = await userManager.CreateAsync(newUser, model.Pass);

                    if (result.Succeeded)
                    {
                        var authenticationManager = HttpContext.GetOwinContext().Authentication;
                        authenticationManager.SignOut( new[] {
                            DefaultAuthenticationTypes.ApplicationCookie,
                            DefaultAuthenticationTypes.ExternalCookie,
                            DefaultAuthenticationTypes.ExternalBearer
                        });
                        var claimsIdentity = await userManager.CreateIdentityAsync(newUser, DefaultAuthenticationTypes.ApplicationCookie);
                        authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = false }, claimsIdentity);
                        return RedirectToAction("", "Home");
                    }
                }

                ModelState.AddModelError("", "Registration could not be completed.");
                return RedirectToAction("", "Login", new LoginViewModel());
            }
            else
            {
                return View();
            }
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult Index(LoginViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        User u = (from x in db.Users
        //                  where x.username == model.User
        //                  where x.password == model.Pass
        //                  select x).FirstOrDefault();
        //        if (u == null)
        //        {
        //            return RedirectToAction("Index", model);
        //        }

        //        //var identity = new ClaimsIdentity(new[] {
        //        //            new Claim(ClaimTypes.Name, input.Username),
        //        //        },
        //        //        DefaultAuthenticationTypes.ApplicationCookie,
        //        //        ClaimTypes.Name, ClaimTypes.Role);

        //        //// if you want roles, just add as many as you want here (for loop maybe?)
        //        //identity.AddClaim(new Claim(ClaimTypes.Role, "guest"));
        //        //// tell OWIN the identity provider, optional
        //        //// identity.AddClaim(new Claim(IdentityProvider, "Simplest Auth"));

        //        //System.Security.Authentication.SignIn(new AuthenticationProperties
        //        //{
        //        //    IsPersistent = input.RememberMe
        //        //}, identity);

        //        return RedirectToAction("", "Home");
        //    }
        //    return View(model);
        //}
	}
}