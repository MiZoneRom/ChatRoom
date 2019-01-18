using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace SuperWebSocket
{
    public static class SessionPool
    {

        /// <summary>
        /// WebSocketService
        /// </summary>
        public static WebSocketServer SocketServer;

        /// <summary>
        /// Session list
        /// </summary>
        public static List<WebSocketSession> SessionList = new List<WebSocketSession>();

        /// <summary>
        /// Add session to list 
        /// </summary>
        /// <param name="item"></param>
        public static void Add(WebSocketSession item)
        {
            SessionList.Add(item);
        }

        /// <summary>
        /// Remove session
        /// </summary>
        /// <param name="item"></param>
        public static void Remove(WebSocketSession item)
        {

            item.Close();

            lock (SessionList)
            {

                try
                {

                    if (SessionList.Contains(item))
                    {
                        SessionList.Remove(item);
                    }

                }
                catch (Exception)
                {
                    Log.Info("Remove error");
                }

            }

        }

        /// <summary>
        /// Remove session by userid
        /// </summary>
        /// <param name="user_id"></param>
        public static void Remove(long userId)
        {

            lock (SessionList)
            {

                try
                {

                    for (int i = SessionList.Count - 1; i >= 0; i--)
                    {
                        WebSocketSession session = SessionList[i];

                        if (session == null)
                        {
                            continue;
                        }

                        if (session.UserId == userId)
                        {

                            Log.Info(string.Concat("Remove Client " + SessionList[i].SessionID));

                            session.Offline();

                            SessionList.Remove(session);

                        }
                    }

                }
                catch (Exception)
                {
                    Log.Info("Remove error");
                }

            }

        }

        /// <summary>
        /// Get session by userid
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static WebSocketSession Get(long userId)
        {
            lock (SessionList)
            {

                try
                {

                    for (int i = SessionList.Count - 1; i >= 0; i--)
                    {
                        if (SessionList[i] == null)
                        {
                            continue;
                        }

                        if (SessionList[i].UserId == userId)
                        {
                            return SessionList[i];
                        }

                    }

                }
                catch (Exception)
                {
                    Log.Info("Get error");
                    return null;
                }

            }

            return null;
        }

        /// <summary>
        /// Get session by sesssionid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static WebSocketSession Get(string sessionId)
        {

            for (int i = SessionList.Count - 1; i >= 0; i--)
            {
                if (SessionList[i] == null)
                {
                    continue;
                }

                if (SessionList[i].SessionID == sessionId)
                {

                    return SessionList[i];

                }

            }

            return null;
        }

        /// <summary>
        /// Get user count
        /// </summary>
        /// <returns></returns>
        public static int DevicesCount()
        {
            return SessionList.Count;
        }

        /// <summary>
        /// Broadcast message
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="mapId"></param>
        public static void Broadcast(SocketData msg)
        {

            for (int i = SessionList.Count - 1; i >= 0; i--)
            {

                if (SessionList[i] == null)
                {
                    continue;
                }

                SessionList[i].Send(msg);
            }

        }

        /// <summary>
        /// Broadcast message by userId
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="mapId"></param>
        public static void Broadcast(SocketData msg, long userId)
        {

            for (int i = SessionList.Count - 1; i >= 0; i--)
            {

                if (SessionList[i] == null)
                {
                    continue;
                }

                if (SessionList[i].UserId == userId)
                {
                    SessionList[i].Send(msg);
                }

            }

        }

        /// <summary>
        /// Broadcast message by userId
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="mapId"></param>
        public static void Broadcast(SocketData msg, long userId, out bool finded)
        {
            finded = false;

            for (int i = SessionList.Count - 1; i >= 0; i--)
            {

                if (SessionList[i] == null)
                {
                    continue;
                }

                if (SessionList[i].UserId == userId)
                {
                    SessionList[i].Send(msg);
                    finded = true;
                }

            }

        }

        /// <summary>
        /// Broadcast message to other
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="mapId"></param>
        /// <param name="userId"></param>
        public static void BroadcastToOther(SocketData msg, long userId)
        {

            for (int i = SessionList.Count - 1; i >= 0; i--)
            {

                if (SessionList[i] == null)
                {
                    continue;
                }

                if (SessionList[i].UserId != userId)
                {
                    SessionList[i].Send(msg);
                }

            }

        }

        /// <summary>
        /// Check login
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool CheckLogin(WebSocketSession context)
        {

            if (context.UserId <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// Get all sessions
        /// </summary>
        /// <returns></returns>
        public static List<WebSocketSession> GetAllSessions()
        {
            return SessionList;
        }

    }
}
