using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LightLog.Test
{
    [TestClass]
    public class LoggerTest
    {
        [TestMethod]
        public void WriteTest()
        {
            Logger.Info("info log");
            try
            {
                string str = null;
                str = str.ToString();
            }
            catch (Exception ex)
            {
                Logger.Error("error log", ex);
            }

            Thread.Sleep(1000);
        }
    }
}