using FacebookPro.Models;
using FacebookPro.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace FacebookPro.Controllers
{
    public class Posts1Controller : Controller
    {
        private FaceBookProEntities1 db = new FaceBookProEntities1();


        public ActionResult Index()
        {


            int userId = (int)Session["user_id"];
            MultiView model = new MultiView();
            model.FriendsRel = db.FriendsRel.Where(a => a.FSender == userId || a.FReceiver == userId).ToList();
            List<Posts> temp = new List<Posts>();
            List<Comments> temp1 = new List<Comments>();
            int flag = 0;
            foreach (var f in model.FriendsRel)
            {

                if (f.FSender == userId)
                {
                    f.FSender = f.FReceiver;
                    temp = db.Posts.Where(a => a.User_ID == f.FSender || a.User_ID == f.FReceiver).ToList();
                }
                else if (f.FReceiver == userId)
                {
                    f.FReceiver = f.FSender;
                    temp = db.Posts.Where(a => a.User_ID == f.FSender || a.User_ID == f.FReceiver).ToList();
                }
                temp1 = db.Comments.Where(a => a.User_ID == f.FSender || a.User_ID == f.FReceiver).ToList();
                if (temp.Count > 0)
                {
                    foreach (var post in temp)
                    {
                        model.Posts.Add(post);
                    }
                }
                if (temp1.Count > 0)
                {
                    foreach (var comment in temp1)
                    {
                        model.Comments.Add(comment);
                    }
                }
            }

            temp = db.Posts.Where(a => a.User_ID == userId).ToList();
            if (temp.Count > 0)
            {
                foreach (var post in temp)
                {
                    model.Posts.Add(post);
                }
            }
            // model.Posts = db.Posts.Where(a => a.User_ID == userId ).ToList();
            //model.Comments = db.Comments.Where(a => a.User_ID == userId).ToList();

            return View(model);
        }



        // GET: Posts1/Create
        public ActionResult CreatePost()
        {
            return View();
        }

        // POST: Posts1/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePost([Bind(Include = "Post_ID,Post_txt,Post_Like,Post_Date,User_ID,Privacy")] Posts posts)
        {
            if (ModelState.IsValid)
            {
                Session["Post_ID"] = posts.Post_ID;
                posts.User_ID = Convert.ToInt32(Session["user_id"]);
                posts.Post_Like = 0;
                posts.Post_Date = DateTime.Now;
                posts.Privacy = 1;
                db.Posts.Add(posts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(posts);
        }
        public ActionResult privacyPost(int idPost)
        {
            List<Posts> posts = db.Posts.Where(postt => postt.Post_ID == idPost).ToList();
            if (posts[0].Privacy == 0)
            {
                int p = 1;
                try
                {
                    var result = db.Posts.SqlQuery("Update dbo.Posts SET Privacy = " + p + " WHERE Post_ID =  " + idPost).ToList();
                }
                catch (Exception ex)
                {
                }
            }
            else
            {

                int p = 0;
                try
                {
                    var result = db.Posts.SqlQuery("Update dbo.Posts SET Privacy = " + p + " WHERE Post_ID =  " + idPost).ToList();
                }
                catch (Exception ex)
                {
                }
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult Like(int id)
        {
            Posts Update = db.Posts.ToList().Find(P => P.Post_ID == id);
            Update.Post_Like += 1;
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult DisLike(int id)
        {
            Posts Update = db.Posts.ToList().Find(P => P.Post_ID == id);
            Update.Post_Like -= 1;
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }
        // GET: Comments
        public ActionResult ShowComment()
        {
            var comments = db.Comments.Include(c => c.Posts).Include(c => c.UserDetails);
            return View(comments.ToList());
        }

        // GET: Comments/Create
        public ActionResult CreateComment()
        {
            ViewBag.Post_ID = new SelectList(db.Posts, "Post_ID", "Post_txt");
            ViewBag.User_ID = new SelectList(db.UserDetails, "ID", "FirstName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment([Bind(Include = "ID,Comment_txt,User_ID,Post_ID")] Comments comments)
        {


            if (ModelState.IsValid)
            {

                Session["Comment_ID"] = comments.ID;
                comments.User_ID = Convert.ToInt32(Session["user_id"]);
                //comments.Post_ID = Convert.ToInt32(Posts.Current.Request.Form["txtpostId"] ) ; 
                comments.Post_ID = 1;
                //comments.Post_ID = PostID;
                //comments.Comment_Date = DateTime.Now;
                db.Comments.Add(comments);
                db.SaveChanges();
                return RedirectToAction(Request.UrlReferrer.ToString());
            }

            ViewBag.Post_ID = new SelectList(db.Posts, "Post_ID", "Post_txt", comments.Post_ID);
            ViewBag.User_ID = new SelectList(db.UserDetails, "ID", "FirstName", comments.User_ID);
            return RedirectToAction("Index");

        }
        public ActionResult CreatComment([Bind(Include = "ID,Comment_txt,User_ID,Post_ID")] Comments comments, int commen, string textcomment)
        {


            if (ModelState.IsValid)
            {

                Session["Comment_ID"] = comments.ID;
                comments.User_ID = Convert.ToInt32(Session["user_id"]);

                comments.Post_ID = commen;

                comments.Comment_txt = textcomment;

                db.Comments.Add(comments);
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.ToString());
            }

            return Content("Comment empty");

        }

    }
}
