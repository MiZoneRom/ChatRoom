using Common;
using Entity;
using IServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuperWebSocket;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Platform
{
    public class Login
    {

        public static void LoginCheckIn(WebSocketSession session, SocketData socketData)
        {

            string nick = socketData.data["nick"].ToString();
            string photo = socketData.data["photo"].ToString();
            string code = socketData.data["code"].ToString();

            string appId = ConfigurationManager.AppSettings["AppId"];
            string secret = ConfigurationManager.AppSettings["Secret"];

            string httpResult = WebHelper.GetRequestData("https://api.weixin.qq.com/sns/jscode2session?appid=" + appId + "&secret=" + secret + "&js_code=" + code + "&grant_type=authorization_code", "get", "");

            Log.Info("WeChat jscode2session api received msg:" + httpResult);

            JObject sessionObj = JsonConvert.DeserializeObject(httpResult) as JObject;

            string openid = sessionObj["openid"].ToString();
            //string unionid = sessionObj["unionid"].ToString();
            string unionid = "";
            string session_key = sessionObj["session_key"].ToString();

            Users userInfo = ServiceProvider.Instance<IUsersService>.Create.GetUserByOpenId(openid);

            if (userInfo == null)
            {

                Users user = new Users()
                {
                    CreateDate = DateTime.Now,
                    Gender = 1,
                    IsOnline = 1,
                    LastLoginDate = DateTime.Now,
                    Nick = nick,
                    Photo = photo,
                    UserName = Utils.GetRamCode()
                };

                UserOpenIds openIds = new UserOpenIds()
                {
                    UnionId = unionid,
                    UnionOpenId = openid,
                    UserId = user.Id,
                    ServiceProvider = "WeiXin.SmallProgram"
                };

                userInfo = ServiceProvider.Instance<IUsersService>.Create.UserRegister(user, openIds);

            }

            WebSocketSession findOnlineUser = SessionPool.Get(userInfo.Id);

            ServiceProvider.Instance<IUsersService>.Create.UpdateLoginDate(DateTime.Now, userInfo.Id);
            ServiceProvider.Instance<IUsersService>.Create.UpdateLoginKey(userInfo.Id);

            userInfo = ServiceProvider.Instance<IUsersService>.Create.GetUserByOpenId(openid);

            if (findOnlineUser != null)
            {
                SessionPool.Remove(userInfo.Id);
            }

            SocketData sendData = new SocketData() { cmd_id = MainProtocol.LoginCheckIn, msg = "登录成功" };
            sendData.SetData(new
            {
                user_id = userInfo.Id,
                key = userInfo.Key
            });

            session.Nick = userInfo.Nick;
            session.UserId = userInfo.Id;
            session.Photo = userInfo.Photo;
            session.SessionKey = session_key;
            session.ConnectTime = DateTime.Now;

            SessionPool.Add(session);

            session.Send(sendData);

        }

        public static void Reconnect(WebSocketSession session, SocketData socketData)
        {
            string key = socketData.data["key"].ToString();

            Users userInfo = ServiceProvider.Instance<IUsersService>.Create.GetUserByKey(key);

            if (userInfo == null)
            {
                Log.Info("Can't find user. key:" + key);
                return;
            }

            WebSocketSession findOnlineUser = SessionPool.Get(userInfo.Id);

            ServiceProvider.Instance<IUsersService>.Create.UpdateLoginDate(DateTime.Now, userInfo.Id);
            ServiceProvider.Instance<IUsersService>.Create.UpdateLoginKey(userInfo.Id);

            userInfo = ServiceProvider.Instance<IUsersService>.Create.GetUser(userInfo.Id);

            if (findOnlineUser != null)
            {
                SessionPool.Remove(userInfo.Id);
            }

            SocketData sendData = new SocketData() { cmd_id = MainProtocol.LoginCheckIn, msg = "登录成功" };
            sendData.SetData(new
            {
                user_id = userInfo.Id,
                key = userInfo.Key
            });

            session.Nick = userInfo.Nick;
            session.UserId = userInfo.Id;
            session.Photo = userInfo.Photo;
            session.ConnectTime = DateTime.Now;

            SessionPool.Add(session);

            session.Send(sendData);

        }

        public static void LoginOut(int userId)
        {
            if (userId <= 0) {
                return;
            }
            ServiceProvider.Instance<IUsersService>.Create.UpdateLoginOutDate(DateTime.Now, userId);
        }

    }
}
