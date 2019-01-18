using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SuperWebSocket
{
    /// <summary>
    /// 主协议
    /// </summary>
    public enum MainProtocol
    {
        MessageKeep = 0,
        Login = 1000,
        LoginCheckIn = 1001,
        ReConnect = 1002,
        Chat = 2000,
        Broadcast = 2001
    }

    /// <summary>
    /// 消息格式
    /// </summary>
    public class SocketData
    {
        /// <summary>
        /// 命令ID
        /// </summary>
        public MainProtocol cmd_id { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public Hashtable data { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 错误码
        /// </summary>
        public object error_code { get; set; }

        public object GetValue(string propertyname)
        {
            return this.data[propertyname];
        }

        /// <summary>
        /// 获取数据值
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public int GetIntValue(string propertyName)
        {
            return Utils.ObjToInt(GetValue(propertyName));
        }

        /// <summary>
        /// 获取数据值
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public int GetLongValue(string propertyName)
        {
            return Utils.ObjToInt(GetValue(propertyName));
        }

        /// <summary>
        /// 获取数据值
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public string GetStrValue(string propertyName)
        {
            return Utils.ObjectToStr(GetValue(propertyName));
        }

        public void SetData(object obj)
        {
            this.data = CommonHelper.Object2Hashtable(obj);
        }

    }
}
