using FacebookPro.ViewModels;

namespace FacebookPro.Services
{
    public interface ISecurityServices
    {
        void SaveUserToDB(RegisterViewModel model);
        bool IsValidUser(SignInVeiwModel model);
        
    }

}