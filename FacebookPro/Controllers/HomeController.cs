using FacebookPro.Models;
using FacebookPro.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FacebookPro.Controllers
{
    public class HomeController : Controller
    {
        FaceBookProEntities1 db = new FaceBookProEntities1();
        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult imageProfile()
        //{

        //    return View();
        //}
        //public ActionResult imageProfile(profileViewModel model)
        //{

        //    return View(model.image);
        //}
        //[HttpPost]
        //public ActionResult imageProfile(profileViewModel model, HttpPostedFileBase file)
        //{
        //    if (file != null)
        //    {
        //        model.image = new byte[file.ContentLength];
        //        file.InputStream.Read(model.image, 0, file.ContentLength);
        //    }

        //    db.SaveChanges();
        //    return View(model);
        //}



    }
}