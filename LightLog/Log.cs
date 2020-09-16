using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LightLog
{
    /// <summary>
    /// log. 日志
    /// </summary>
    public static class Log
    {
        #region private fileds

        /// <summary>
        /// Write lock. 写日志锁
        /// </summary>
        private static readonly object writeLock = new object();

        /// <summary>
        /// Log folder path. 日志文件夹路径
        /// </summary>
        private static string folderPath = "log\\";

        /// <summary>
        /// Log level. 日志级别
        /// </summary>
        private enum LogLevel { Debug, Info, Warn, Error, Fatal };

        #endregion

        #region public methods

        /// <summary>
        /// Set log folder path. 设置日志路径
        /// </summary>
        /// <param name="path">Log Folder Path. 日志路径</param>
        public static void SetFolderPath(string path)
        {
            folderPath = (path.EndsWith("\\") || path.EndsWith("/")) ? path : path + "\\";
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath); //判断并创建日志文件夹
        }

        /// <summary>
        /// Write debug log. 写调试日志
        /// </summary>
        /// <param name="msg">Log message. 日志信息</param>
        /// <param name="ex">Exception. 异常</param>
        public static void Debug(string msg, Exception ex = null)
        {
            Task.Run(() =>
            {
                Write(LogLevel.Debug, msg, ex);
            });
        }

        /// <summary>
        /// Write info log. 写信息日志
        /// </summary>
        /// <param name="msg">Log message. 日志信息</param>
        /// <param name="ex">Exception. 异常</param>
        public static void Info(string msg, Exception ex = null)
        {
            Task.Run(() =>
            {
                Write(LogLevel.Info, msg, ex);
            });
        }

        /// <summary>
        /// Write warn log. 写警告日志
        /// </summary>
        /// <param name="msg">Log message. 日志信息</param>
        /// <param name="ex">Exception. 异常</param>
        public static void Warn(string msg, Exception ex = null)
        {
            Task.Run(() =>
            {
                Write(LogLevel.Warn, msg, ex);
            });
        }

        /// <summary>
        /// Write error log. 写错误日志
        /// </summary>
        /// <param name="msg">Log message. 日志信息</param>
        /// <param name="ex">Exception. 异常</param>
        public static void Error(string msg, Exception ex = null)
        {
            Task.Run(() =>
            {
                Write(LogLevel.Error, msg, ex);
            });
        }

        /// <summary>
        /// Write faltal log. 写致命日志
        /// </summary>
        /// <param name="msg">Log message. 日志信息</param>
        /// <param name="ex">Exception. 异常</param>
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
        /// Format log content. 格式化日志内容
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
        /// Log head text. 日志头部文本
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        private static string GetHead(LogLevel logLevel)
        {
            return $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} {logLevel.ToString()}] ";
        }

        /// <summary>
        /// Get log file name. 获取日志文件名称
        /// </summary>
        /// <returns></returns>
        private static string GetName()
        {
            return DateTime.Now.ToString("yyyyMMdd") + ".log";
        }

        /// <summary>
        /// Write log. 写日志
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        private static void Write(LogLevel logLevel, string msg, Exception ex)
        {
            Write(GetName(), GetContent(logLevel, msg, ex)); //格式化日志、写日志
        }

        /// <summary>
        /// Write log. 写日志
        /// </summary>
        /// <param name="name"></param>
        /// <param name="msg"></param>
        private static void Write(string name, string msg)
        {
            string path = folderPath + name; //日志路径
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
                    if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath); //判断并创建日志文件夹
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
