using FacebookPro.Models;
using FacebookPro.Services;
using FacebookPro.ViewModels;
using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Security;

namespace FacebookPro.Controllers
{
    public class SecurityController : Controller
    {
        FaceBookProEntities1 db = new FaceBookProEntities1();

        private ISecurityServices _services = null;
        public SecurityController(ISecurityServices services)
        {
            _services = services;
        }
        // GET: Security
        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignIn(SignInVeiwModel model)
        {
            if (ModelState.IsValid)
            {
                if (_services.IsValidUser(model))
                {

                    this.Session.Add("Username", model.Username);

                    FormsAuthentication.SetAuthCookie(model.Username, false); //to have authentication to directed to homepage
                    return RedirectToAction("HomePage", "HomePage", new { user = model.Username });
                }
                else
                {
                    ModelState.AddModelError(" ", "Invalid Username or passwod");
                }
            }

            return View(model);
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {

            string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
            string extension = Path.GetExtension(model.ImageFile.FileName); //get extenation
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;//to avoid the duplicate filename 
            model.ImagePath = "~/Image/" + fileName; // save image in image file 
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            model.ImageFile.SaveAs(fileName);//save image to image folder 

            if (ModelState.IsValid)
            {
                /*هنا بتاكد انه لما يملى الفورم يملاها كلها لو صح هيسجلها في الداتا بيز*/
                _services.SaveUserToDB(model);
                ViewBag.msg = "Your Account is CREATED Successfully SignIn now";
                return View();
            }

            return View(model);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            this.Session.Clear();
            return RedirectToAction("SignIn", "Security");
        }


    }
}