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
        /// 日志级别
        /// </summary>
        private enum LogLevel { Debug, Info, Warn, Error, Fatal };

        public Logger()
        {
            if (!Directory.Exists(logFolderPath)) Directory.CreateDirectory(logFolderPath); //判断并创建日志文件夹
        }

        public Logger(string path)
        {
            logFolderPath = (path.EndsWith("\\") || path.EndsWith("/")) ? path : path + "\\";
            if (!Directory.Exists(logFolderPath)) Directory.CreateDirectory(logFolderPath); //判断并创建日志文件夹
        }

        /// <summary>
        /// 写调试日志
        /// </summary>
        /// <param name="msg"></param>
        public void Debug(string msg, Exception ex = null)
        {
            Task.Run(() =>
            {
                WriteLog(LogLevel.Debug, msg, ex);
            });
        }

        /// <summary>
        /// 写信息日志
        /// </summary>
        /// <param name="msg"></param>
        public void Info(string msg, Exception ex = null)
        {
            Task.Run(() =>
            {
                WriteLog(LogLevel.Info, msg, ex);
            });
        }

        /// <summary>
        /// 写警告日志
        /// </summary>
        /// <param name="msg"></param>
        public void Warn(string msg, Exception ex = null)
        {
            Task.Run(() =>
            {
                WriteLog(LogLevel.Warn, msg, ex);
            });
        }

        /// <summary>
        /// 写错误日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public void Error(string msg, Exception ex = null)
        {
            Task.Run(() =>
            {
                WriteLog(LogLevel.Error, msg, ex);
            });
        }

        /// <summary>
        /// 写致命日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public void Fatal(string msg, Exception ex = null)
        {
            Task.Run(() =>
            {
                WriteLog(LogLevel.Fatal, msg, ex);
            });
        }

        /// <summary>
        /// 获取日志内容
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private string GetLogContent(LogLevel logLevel, string msg, Exception ex)
        {
            return GetLogHead(LogLevel.Error) + msg + Environment.NewLine + (ex == null ? string.Empty : (ex.ToString() + Environment.NewLine)); //ex.ToString()内容最详细
        }

        /// <summary>
        /// 日志头部信息
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        private string GetLogHead(LogLevel logLevel)
        {
            return $"[{DateTime.Now.ToString("HH:mm:ss")} {logLevel.ToString()}]";
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
        /// <param name="logLevel"></param>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        private void WriteLog(LogLevel logLevel, string msg, Exception ex)
        {
            WriteLog(GetLogFilePath(), GetLogContent(LogLevel.Error, msg, ex)); //格式化日志、写日志
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
