using CarsCatalog.BBL.Models;
using CarsCatalog.DDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsCatalog.BBL.Services
{
    public class UserService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public IEnumerable<Users> GetUsersList()
        {         
            return this.unitOfWork.UsersRepository.Get();
        }        

        public Users GetUser(int id)
        {
            return this.unitOfWork.UsersRepository.Get(id);
        }

        public SaveMessagesModel AddUser(Users user)
        {
            bool saved = false;
            return new SaveMessagesModel()
            {
                ErrorMessage = this.unitOfWork.UsersRepository.Add(user, out saved),
                Saved = saved
            };
        }

        public SaveMessagesModel RemoveUser(int userId)
        {
            bool saved = false;
            return new SaveMessagesModel()
            {
                ErrorMessage = this.unitOfWork.UsersRepository.Remove(userId, out saved),
                Saved = saved
            };
        }

        public SaveMessagesModel UpdateUser(Users user)
        {
            bool saved = false;
            return new SaveMessagesModel()
            {
                ErrorMessage = this.unitOfWork.UsersRepository.Update(user, out saved),
                Saved = saved
            };
        }

        public UserLoginModel UserAuthentication(string userName, string pass)
        {
            Users user = this.unitOfWork.UsersRepository.GetUserbyUserNameOrEmail(userName);
            UserLoginModel model = new UserLoginModel() { HasValidAccount = false};
            if (user == null)
            {
                model.LoginMessage = "The username or email does not exist";
            }
            else if (user.Password != pass)
            {
                model.LoginMessage = "Incorrect password";
            }
            else if (user != null && user.Password == pass)
            {
                model.User = user.UserName;
                model.HasValidAccount = true;
                model.LoginMessage = string.Empty;
            }
            return model;
        }

        public bool CanEditAds(int userId, int AdId)
        {
            bool canEdit = false;

            DDL.Users user = this.GetUser(userId);
            //If the user is Administrator or the user is the Ad owner)
            canEdit = (user != null && (user.AccountTypeId == 1 || user.Ads.Any(x => x.AdsId == AdId)) );

            return canEdit;
        }

        public bool IsAdministrator(int userId)
        {
            bool canEdit = false;

            DDL.Users user = this.GetUser(userId);
            //If the user is Administrator or the user is the Ad owner)
            canEdit = (user != null && user.AccountTypeId == 1);

            return canEdit;
        }        

        public void Dispose()
        {
            this.unitOfWork.Dispose();
        }
    }
}
