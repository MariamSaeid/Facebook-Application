using FacebookPro.Models;
using FacebookPro.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;

namespace FacebookPro.Controllers
{
    [Authorize] //to not allow to everyone to access the hompage
    public class HomePageController : Controller
    {

        FaceBookProEntities1 db = new FaceBookProEntities1();

        List<UserDetails> myUser = new List<UserDetails>();
        string userEmail;


        [OutputCache(Duration = 0, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult HomePage(string user)
        {

            UserDetails userID = db.UserDetails.Find(Convert.ToInt32(Session["userid"]));
            myUser = db.UserDetails.Where(a => a.Email.Equals(user)).ToList();
            Session["UserEmail"] = user;
            var userId = db.UserDetails.Where(m => m.Email.Equals(user)).Select(m => m.ID).SingleOrDefault();
            Session["user_id"] = userId;
            return RedirectToAction("EditProfile");


        }

        public ActionResult MyProfile()
        {

            UserDetails Model = new UserDetails();
            Model = db.UserDetails.Find(Convert.ToInt32(Session["userid"]));

            return View(Model);
        }
        public ActionResult EditProfile()
        {
            userEmail = Convert.ToString(Session["UserEmail"]);
            var u = userEmail;
            return View(db.UserDetails.Where(a => a.Email.Equals(u)).ToList());
        }

        public ActionResult Edit(int? Id)
        {

            Id = Convert.ToInt32(Session["user_id"]);
            var user = db.UserDetails.Where(s => s.ID == Id).FirstOrDefault();

            return View(user);
        }
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Email,Password,mobile,city,country,ImagePath,ImageFile")] UserDetails userDetails)
        {

            string fileName = Path.GetFileNameWithoutExtension(userDetails.ImageFile.FileName);
            string extension = Path.GetExtension(userDetails.ImageFile.FileName); //get extenation
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;//to avoid the duplicate filename 
            userDetails.ImagePath = "~/Image/" + fileName; // save image in image file 
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            userDetails.ImageFile.SaveAs(fileName);//save image to image folder 
            if (ModelState.IsValid)
            {
                db.Entry(userDetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Logout", "Security");
            }
            return View(userDetails);
        }


        public ActionResult Search1(string search)
        {


            var user = db.UserDetails.Where(s => s.Email.Contains(search) || s.mobile.ToString().Contains(search)).ToList();

            return View(user);

        }

        [HttpGet]
        public ActionResult viewProfile(int? id)
        {
            int Flag = 0;
            int IdSession = Convert.ToInt32(Session["user_id"]);
            List<FriendsRel> friendsRels = db.FriendsRel.Where(m => m.FSender == IdSession || m.FReceiver == IdSession).ToList();
            List<SendRequest> requests = db.SendRequest.Where(m => m.ReseverID == IdSession || m.SenderID == IdSession).ToList();

            Session["IDViewProfile"] = id;
            UserDetails Model = new UserDetails();
            Model = db.UserDetails.Where(x => x.ID == id).FirstOrDefault();
            if (Session["textRequest"] == null)
            {
                ViewBag.msg = "Add Friend";
            }
            else
            {
                ViewBag.msg = Session["textRequest"];
            }

            if (id.Equals(Session["user_id"]))
            {
                ViewBag.msg = null;
            }
            foreach (var friend in friendsRels)
            {
                if (friend.FReceiver == id || friend.FSender == id)
                {
                    ViewBag.rel = "You are Friends ";
                    ViewBag.msg = null;

                }
            }
            foreach (var request in requests)
            {
                if (request.SenderID == id)
                {
                    ViewBag.msg = null;
                }
                else if (request.ReseverID == id)
                {
                    ViewBag.msg = "Wait or Cancle Request";
                }
            }
            // idsender / idreciver

            return View(Model);
        }
        public ActionResult MyFriends()
        {
            int MyID = Convert.ToInt32(Session["user_id"]);
            List<FriendsRel> friendsRels = db.FriendsRel.Where(m => m.FSender == MyID || m.FReceiver == MyID).ToList();
            UserDetails model = new UserDetails();
            List<UserDetails> users = new List<UserDetails>();
            List<UserDetails> temp = new List<UserDetails>();

            foreach (var user in friendsRels)
            {
                temp = db.UserDetails.Where(a => (a.ID == user.FSender || a.ID == user.FReceiver) && a.ID != MyID).ToList();
                users.Add(temp[0]);
            }
            return View(users);

        }
        [HttpGet]
        public ActionResult details(int id)
        {

            UserDetails imageModel1 = new UserDetails();
            imageModel1 = db.UserDetails.Where(x => x.ID == id).FirstOrDefault();

            return View(imageModel1);

        }


        public ActionResult sendRequest()
        {
            int count = 0;
            int SenderID = Convert.ToInt32(Session["user_id"]);
            int ReseverID = Convert.ToInt32(Session["IDViewProfile"]);
            MultiView model = new MultiView();

            model.SendRequest = db.SendRequest.Where(m => m.ReseverID == ReseverID && m.SenderID == SenderID).ToList();
            if (model.SendRequest.Count == 0)
            {
                count = 1;
                ViewBag.msg = "Add Friend";
                SendRequest sendRequest = new SendRequest();
                sendRequest.ReseverID = Convert.ToInt32(Session["IDViewProfile"]);

                sendRequest.SenderID = Convert.ToInt32(Session["user_id"]);

                db.SendRequest.Add(sendRequest);
                db.SaveChanges();
                Session["textRequest"] = ViewBag.msg;


            }
            else
            {
                ViewBag.msg = "Add Friend";
                Session["textRequest"] = ViewBag.msg;
                count = 2;
                try
                {
                    db.SendRequest.Remove(model.SendRequest[0]);
                    db.SaveChanges();

                }
                catch
                {

                }
            }

            return Redirect(Request.UrlReferrer.ToString());

        }

        public ActionResult FriendsRequest()
        {

            int ReseverID = Convert.ToInt32(Session["user_id"]);
            UserDetails model = new UserDetails();
            List<UserDetails> users = new List<UserDetails>();
            List<UserDetails> temp = new List<UserDetails>();
            model.SendRequest = db.SendRequest.Where(m => m.ReseverID == ReseverID).ToList();
            foreach (SendRequest user in model.SendRequest)
            {
                temp = db.UserDetails.Where(a => a.ID == user.SenderID).ToList();
                users.Add(temp[0]);
            }
            return View(users);
        }
        public ActionResult Accept(int? id)
        {
            FriendsRel friend = new FriendsRel();

            int ReseverID = Convert.ToInt32(Session["user_id"]);
            MultiView model = new MultiView();
            model.SendRequest = db.SendRequest.Where(m => m.ReseverID == ReseverID && m.SenderID == id).ToList();
            friend.FReceiver = ReseverID;
            friend.FSender = (int)id;
            try
            {
                db.SendRequest.Remove(model.SendRequest[0]);
                db.FriendsRel.Add(friend);
                db.SaveChanges();

            }
            catch
            {
                return Content("accept error ");
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult Reject(int? id)
        {


            int ReseverID = Convert.ToInt32(Session["user_id"]);
            MultiView model = new MultiView();
            model.SendRequest = db.SendRequest.Where(m => m.ReseverID == ReseverID && m.SenderID == id).ToList();

            try
            {
                db.SendRequest.Remove(model.SendRequest[0]);
                db.SaveChanges();

            }
            catch
            {
                return Content("reject error ");
            }
            return Redirect(Request.UrlReferrer.ToString());
        }


    }
}