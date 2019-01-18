using IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace Services
{
    public class ChatService : ServiceBase, IChatService, IService, IDisposable
    {
        public void AddChatRecord(int userId, string msg, int msgType)
        {
            ChatHistory history = new ChatHistory()
            {
                UserId = userId,
                Msg = msg,
                AddDate = DateTime.Now,
                isRead = 0,
                MsgType = msgType
            };

            if (msgType == 1)
            {
                history.ChatGroupId = 0;
            }

            context.ChatHistory.Add(history);
            context.SaveChanges();

        }
    }
}
