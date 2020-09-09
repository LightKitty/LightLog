using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LightLog.Test
{
    [TestClass]
    public class LogTest
    {
        [TestMethod]
        public void WriteTest()
        {
            Log.Debug("debug log");
            Log.Info("info log");
            try
            {
                string str = null;
                str = str.ToString();
            }
            catch (Exception ex)
            {
                Log.Error("error log", ex);
            }

            Thread.Sleep(1000);
        }
    }
}