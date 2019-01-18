using Common;
using IServices;
using Newtonsoft.Json;
using Platform;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperWebSocket;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WindowWidth = 120;
            Console.BufferHeight = 1000;

            #region 参数
            /*
                name: 服务器实例的名称;
                serverType: 服务器实例的类型的完整名称;
                serverTypeName: 所选用的服务器类型在 serverTypes 节点的名字，配置节点 serverTypes 用于定义所有可用的服务器类型;
                ip: 服务器监听的ip地址。你可以设置具体的地址，也可以设置为下面的值 Any - 所有的IPv4地址 IPv6Any - 所有的IPv6地址
                port: 服务器监听的端口;
                listenBacklog: 监听队列的大小;
                mode: Socket服务器运行的模式, Tcp (默认) 或者 Udp;
                disabled: 服务器实例是否禁用了;
                startupOrder: 服务器实例启动顺序, bootstrap 将按照此值的顺序来启动多个服务器实例;
                sendTimeOut: 发送数据超时时间;
                sendingQueueSize: 发送队列最大长度, 默认值为5;
                maxConnectionNumber: 可允许连接的最大连接数;
                receiveBufferSize: 接收缓冲区大小;
                sendBufferSize: 发送缓冲区大小;
                syncSend: 是否启用同步发送模式, 默认值: false;
                logCommand: 是否记录命令执行的记录;
                logBasicSessionActivity: 是否记录session的基本活动，如连接和断开;
                clearIdleSession: true 或 false, 是否定时清空空闲会话，默认值是 false;
                clearIdleSessionInterval: 清空空闲会话的时间间隔, 默认值是120, 单位为秒;
                idleSessionTimeOut: 会话空闲超时时间; 当此会话空闲时间超过此值，同时clearIdleSession被配置成true时，此会话将会被关闭; 默认值为300，单位为秒;
                security: Empty, Tls, Ssl3. Socket服务器所采用的传输层加密协议，默认值为空  tls;
                maxRequestLength: 最大允许的请求长度，默认值为1024;
                textEncoding: 文本的默认编码，默认值是 ASCII;
                defaultCulture: 此服务器实例的默认 thread culture, 只在.Net 4.5中可用而且在隔离级别为 'None' 时无效;
                disableSessionSnapshot: 是否禁用会话快照, 默认值为 false.
                sessionSnapshotInterval: 会话快照时间间隔, 默认值是 5, 单位为秒;
                keepAliveTime: 网络连接正常情况下的keep alive数据的发送间隔, 默认值为 600, 单位为秒;
                keepAliveInterval: Keep alive失败之后, keep alive探测包的发送间隔，默认值为 60, 单位为秒;
             */
            #endregion


            var serverConfig = new ServerConfig
            {
                Port = Utils.ObjToInt(ConfigurationManager.AppSettings["SocketPort"]),
                Ip = "Any",
                Name = "ChatService_1",
                Security = "",
                LogCommand = true,
                //Certificate = new CertificateConfig
                //{
                //    FilePath = "localhost.pfx",
                //    Password = ConfigurationManager.AppSettings["TlsPassword"]
                //}
            };

            SessionPool.SocketServer = new WebSocketServer();
            if (!SessionPool.SocketServer.Setup(serverConfig))
            {
                Log.Error("Failed to setup!");
                Console.ReadKey();
                return;
            }

            SessionPool.SocketServer.NewSessionConnected += OnConnected;
            SessionPool.SocketServer.SessionClosed += OnClosed;
            SessionPool.SocketServer.NewMessageReceived += OnReceived;

            if (!SessionPool.SocketServer.Start())
            {
                Log.Error("Failed to start!");
                Console.ReadKey();
                return;
            }

            Log.Info("The server started successfully, press key 'Q' to stop it!.Port:" + serverConfig.Port);

            while (Console.ReadKey().KeyChar != 'q')
            {
                continue;
            }
            SessionPool.SocketServer.Stop();
            Log.Info("The server was stopped!");
            Console.ReadKey();

        }

        private static void OnConnected(WebSocketSession session)
        {
            session.ConnectTime = DateTime.Now;
            Log.Info("Device Connected  [ id:" + session.SessionID + " ]");
            Console.Title = ("Online " + (SessionPool.DevicesCount() + 1) + " User");
        }

        private static void OnClosed(WebSocketSession session, CloseReason value)
        {
            Log.Info("Device Closed  [ ID:" + session.SessionID + "  Reason:" + value.ToString() + " ]");
            Console.Title = ("Online " + SessionPool.DevicesCount() + " User");
            Login.LoginOut(session.UserId);
            SessionPool.Remove(session);
        }

        private static void OnReceived(WebSocketSession session, string msg)
        {
            session.LastSendTime = DateTime.Now;

            if (!msg.Contains("cmd_id"))                                                                                            //验证消息格式
            {
                Log.Error("Miss main commond");
                return;
            }

            SocketData onRecevieData = JsonConvert.DeserializeObject<SocketData>(msg);             //接收数据

            if (onRecevieData.cmd_id != MainProtocol.MessageKeep)
            {
                Log.Info("Received Message:" + msg);
            }

            switch (onRecevieData.cmd_id)
            {
                case MainProtocol.MessageKeep:
                    break;
                case MainProtocol.LoginCheckIn:
                    Login.LoginCheckIn(session, onRecevieData);
                    break;
                case MainProtocol.ReConnect:
                    Login.Reconnect(session, onRecevieData);
                    break;
                case MainProtocol.Broadcast:
                    Chat.Broadcast(session, onRecevieData);
                    break;
                default:
                    break;
            }

        }


    }
}
