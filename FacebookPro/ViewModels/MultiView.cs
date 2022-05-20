using FacebookPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookPro.ViewModels
{
    public class MultiView
    {
        public List<UserDetails> UserDetails { get; set; }
        public List<Posts> Posts { get; set; }
        public List<Comments> Comments { get; set; }
        public List<SendRequest> SendRequest { get; set; }
        public List<FriendsRel> FriendsRel { get; set; }
        public MultiView()
        {
            this.Posts = new List<Posts>();
            this.Comments = new List<Comments>();
            this.UserDetails = new List<UserDetails>();
            this.SendRequest = new List<SendRequest>();
            this.FriendsRel = new List<FriendsRel>();


        }
       
    }
}