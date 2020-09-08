using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightLog
{
    public class Logger
    {
        /// <summary>
        /// 写日志锁（全局唯一）
        /// </summary>
        private static readonly object writeLock = new object();

        /// <summary>
        /// 日志文件夹
        /// </summary>
        private string logFolderPath = "log\\";

        /// <summary>
        /// 日志类型
        /// </summary>
        private enum LogType { Info, Error };

        public Logger()
        {
            if (!Directory.Exists(logFolderPath)) Directory.CreateDirectory(logFolderPath);
        }

        public Logger(string path)
        {
            logFolderPath = (path.EndsWith("\\") || path.EndsWith("/")) ? path : path + "\\";
            if (!Directory.Exists(logFolderPath)) Directory.CreateDirectory(logFolderPath);
        }

        /// <summary>
        /// 写信息日志
        /// </summary>
        /// <param name="msg"></param>
        public void Info(string msg)
        {
            Task.Run(() =>
            {
                msg = GetMsgHead(LogType.Info) + msg + Environment.NewLine;
                WriteLog(GetLogFilePath(), msg);
            });
        }

        /// <summary>
        /// 写错误日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public void Error(string msg, Exception ex)
        {
            Task.Run(() =>
            {
                msg = GetMsgHead(LogType.Error) + msg + Environment.NewLine + ex.ToString() +Environment.NewLine;
                WriteLog(GetLogFilePath(), msg);
            });
        }

        /// <summary>
        /// 日志头部信息
        /// </summary>
        /// <param name="logType"></param>
        /// <returns></returns>
        private string GetMsgHead(LogType logType)
        {
            return $"[{DateTime.Now.ToString("HH:mm:ss")} {logType.ToString()}]";
        }

        /// <summary>
        /// 获取日志文件路径
        /// </summary>
        /// <returns></returns>
        private string GetLogFilePath()
        {
            return logFolderPath + DateTime.Now.ToString("yyyyMMdd") + ".log";
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="path"></param>
        /// <param name="msg"></param>
        private void WriteLog(string path, string msg)
        {
            lock (writeLock)
            {
                if (File.Exists(path))
                { //存在日志文件
                    using (StreamWriter sw = new StreamWriter(path, true, Encoding.UTF8))
                    {
                        sw.Write(msg); //写日志
                    }
                }
                else
                { //不存在日志文件
                    using (var fw = File.Create(path)) //创建日志文件
                    using (StreamWriter sw = new StreamWriter(fw, Encoding.UTF8))
                    {
                        sw.Write(msg); //写日志
                    } 
                }
            }
        }
    }
}
