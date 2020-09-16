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
            Log.Warn("warn log");
            try
            {
                string str = null;
                str = str.ToString();
            }
            catch (Exception ex)
            {
                Log.Error("error log", ex);
            }
            Log.Fatal("fatal log");

            Thread.Sleep(1000);
        }
    }
}