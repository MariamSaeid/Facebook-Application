using FacebookPro.Models;
using FacebookPro.ViewModels;
using System;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;





namespace FacebookPro.Services
{
    public class SecurityServices : ISecurityServices
    {

        private FaceBookProEntities1 _context = null;

        public SecurityServices()
        {
            _context = new FaceBookProEntities1();
        }


        public void SaveUserToDB(RegisterViewModel model)
        {

            UserDetails userDetails = new UserDetails
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                city = model.city,
                country = model.country,
                Password = model.Password,
                mobile = model.mobile,
                ImagePath = model.ImagePath,
                ImageFile= model.ImageFile

            };
           
            _context.UserDetails.Add(userDetails);
            _context.SaveChanges();
        } 
        
     
        public bool IsValidUser(SignInVeiwModel model)
        {
            UserDetails user = null;
            user = _context.UserDetails.SingleOrDefault(a => a.Email.Equals(model.Username) && a.Password.Equals(model.Password));
            if(user == null)
                return false;
            else
                return true;    
        }
     

    }
}