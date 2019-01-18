using Common;
using IServices;
using SuperWebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform
{
    public class Chat
    {
        public static void Broadcast(WebSocketSession session, SocketData socketData)
        {

            string msg = socketData.msg;

            SocketData sendMsg = new SocketData()
            {
                cmd_id = MainProtocol.Broadcast,
                msg = msg,
                success = true
            };

            sendMsg.SetData(new
            {
                user_id = session.UserId,
                photo = session.Photo,
                nick = session.Nick,
                msg_id = Utils.GetRamCode()
            });

            SessionPool.Broadcast(sendMsg);

            ServiceProvider.Instance<IChatService>.Create.AddChatRecord(session.UserId, msg, 1);

        }
    }
}
