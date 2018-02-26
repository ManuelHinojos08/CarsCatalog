using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CarsCatalog.DDL.Repositories
{
    public class UsersRepository : IRepository<Users, int>
    {
        
        private CarCatalogEntities context { get; set; }

        public UsersRepository(CarCatalogEntities context)
        {
            this.context = context;
        }

        public IEnumerable<Users> Get()
        {            
            return context.Users.ToList().Where(x => x.Active == 1);
        }

        public Users Get(int id)
        {
            return context.Users.Find(id);
        }

        public string Add(Users entity, out bool saved)
        {
            string Message = string.Empty;
            try
            {
                context.Users.Add(entity);
                context.SaveChanges();
                saved = true;
                return Message;
            }
            catch (Exception e)
            {
                Message = e.Message;
                saved = false;
                return Message;
            }
        }

        public string Remove(int userId, out bool saved)
        {            
            string Message = string.Empty;
            try
            {
                Users obj = context.Users.Find(userId);
                if (obj == null)
                {
                    saved = false;
                    Message = "User not found";
                    return Message;
                }
                obj.Active = 0;
                Message = this.Update(obj, out saved);
                return Message;
            }
            catch (Exception e)
            {
                Message = e.Message;
                saved = false;
                return Message;
            }
        }

        public string Update(Users entity, out bool saved)
        {            
            string Message = string.Empty;
            try
            {
                var obj = context.Users.Find(entity.UserId);
                if (obj == null)
                {
                    Message = "User not found";
                    saved = false;
                    return Message;
                }
                context.Entry(obj).CurrentValues.SetValues(entity);
                context.SaveChanges();
                saved = true;
                return Message;
            }
            catch (Exception e)
            {
                Message = e.Message;
                saved = false;
                return Message;
            }
        }

        public Users GetUserbyUserNameOrEmail(string userName)
        {
            return this.context.Users.
                Where(x => x.Active == 1 &&
                    (x.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase) ||
                     x.Email.Equals(userName, StringComparison.OrdinalIgnoreCase))
                    )
                .FirstOrDefault<Users>();
        }
    }
}
