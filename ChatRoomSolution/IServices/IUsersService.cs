using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IUsersService : IService, IDisposable
    {
        Users GetUser(int userId);

        Users GetUserByOpenId(string openid);

        Users UserRegister(Users userInfo,UserOpenIds openIds);
        void UpdateLoginDate(DateTime date, int userId);

        void UpdateLoginOutDate(DateTime date, int userId);

        void UpdateLoginKey(int userId);

        Users GetUserByKey(string key);
    }
}
