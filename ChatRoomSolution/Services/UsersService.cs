using Common;
using Entity;
using IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Services
{
    public class UsersService : ServiceBase, IUsersService, IService, IDisposable
    {
        public Users GetUser(int userId)
        {

            Users user = (
                from a in context.Users
                where a.Id == userId
                select a
                ).FirstOrDefault();

            return user;
        }

        public Users GetUserByOpenId(string openid)
        {

            Users user = (
                from a in context.Users
                from b in context.UserOpenIds
                where a.Id == b.UserId && b.UnionOpenId == openid
                select a
                ).FirstOrDefault();

            return user;
        }

        public Users UserRegister(Users userInfo, UserOpenIds openIds)
        {

            using (var tran = new TransactionScope())                                                           //新建事务
            {

                Users savedUserInfo = context.Users.Add(userInfo);

                context.SaveChanges();

                openIds.UserId = savedUserInfo.Id;

                context.UserOpenIds.Add(openIds);

                context.SaveChanges();
                tran.Complete();

                return savedUserInfo;
            }

        }

        public void UpdateLoginDate(DateTime date, int userId)
        {
            Users userInfo = GetUser(userId);
            userInfo.LastLoginDate = date;

            LoginRecord record = new LoginRecord()
            {
                LoginDate = DateTime.Now,
                UserId = userId
            };

            context.LoginRecord.Add(record);

            context.SaveChanges();
        }

        public void UpdateLoginOutDate(DateTime date, int userId)
        {

            LoginRecord record = (
                from a in context.LoginRecord
                where a.UserId == userId && a.LoginOutDate == null
                orderby a.LoginDate descending
                select a
                ).FirstOrDefault();

            if (record != null)
            {
                record.LoginOutDate = date;
                record.LoginTime = Convert.ToInt32((date - Convert.ToDateTime(record.LoginDate)).TotalMinutes);
                context.SaveChanges();
            }

        }

        public void UpdateLoginKey(int userId)
        {
            Users userInfo = GetUser(userId);
            userInfo.Key = Guid.NewGuid().ToString().Replace("-", "");
            context.SaveChanges();
        }

        public Users GetUserByKey(string key)
        {
            Users user = (
                from a in context.Users
                where a.Key == key
                select a
                ).FirstOrDefault();

            return user;
        }

    }
}
