using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightLog
{
    /// <summary>
    /// 日志记录
    /// </summary>
    public static class Log
    {
        #region private fileds

        /// <summary>
        /// 写日志锁
        /// </summary>
        private static readonly object writeLock = new object();

        /// <summary>
        /// 日志文件夹路径
        /// </summary>
        private static string folderPath = "log\\";

        /// <summary>
        /// 日志级别
        /// </summary>
        private enum LogLevel { Debug, Info, Warn, Error, Fatal };

        #endregion

        #region public methods

        /// <summary>
        /// 设置日志路径
        /// </summary>
        /// <param name="path">日志路径</param>
        public static void SetFolderPath(string path)
        {
            folderPath = (path.EndsWith("\\") || path.EndsWith("/")) ? path : path + "\\";
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath); //判断并创建日志文件夹
        }

        /// <summary>
        /// 写调试日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常</param>
        public static void Debug(string msg, Exception ex = null)
        {
            Task.Run(() =>
            {
                Write(LogLevel.Debug, msg, ex);
            });
        }

        /// <summary>
        /// 写信息日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常</param>
        public static void Info(string msg, Exception ex = null)
        {
            Task.Run(() =>
            {
                Write(LogLevel.Info, msg, ex);
            });
        }

        /// <summary>
        /// 写警告日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常</param>
        public static void Warn(string msg, Exception ex = null)
        {
            Task.Run(() =>
            {
                Write(LogLevel.Warn, msg, ex);
            });
        }

        /// <summary>
        /// 写错误日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常</param>
        public static void Error(string msg, Exception ex = null)
        {
            Task.Run(() =>
            {
                Write(LogLevel.Error, msg, ex);
            });
        }

        /// <summary>
        /// 写致命日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常</param>
        public static void Fatal(string msg, Exception ex = null)
        {
            Task.Run(() =>
            {
                Write(LogLevel.Fatal, msg, ex);
            });
        }

        #endregion

        #region private methods

        /// <summary>
        /// 格式化日志内容
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private static string GetContent(LogLevel logLevel, string msg, Exception ex)
        {
            return GetHead(logLevel) + msg + Environment.NewLine + (ex == null ? string.Empty : (ex.ToString() + Environment.NewLine)); //ex.ToString()内容最详细
        }

        /// <summary>
        /// 日志头部信息
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        private static string GetHead(LogLevel logLevel)
        {
            return $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} {logLevel.ToString()}] ";
        }

        /// <summary>
        /// 获取日志文件路径
        /// </summary>
        /// <returns></returns>
        private static string GetPath()
        {
            return folderPath + DateTime.Now.ToString("yyyyMMdd") + ".log";
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        private static void Write(LogLevel logLevel, string msg, Exception ex)
        {
            Write(GetPath(), GetContent(logLevel, msg, ex)); //格式化日志、写日志
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="path"></param>
        /// <param name="msg"></param>
        private static void Write(string path, string msg)
        {
            lock (writeLock)
            { //保证单线程写磁盘文件
                if (File.Exists(path))
                { //存在日志文件
                    using (StreamWriter sw = new StreamWriter(path, true, Encoding.UTF8)) //追加
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

        #endregion
    }
}
