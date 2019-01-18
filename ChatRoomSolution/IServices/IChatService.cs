using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface IChatService : IService, IDisposable
    {
        void AddChatRecord(int userId,string msg,int msgType);
    }
}
