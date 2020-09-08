using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace LightLog.Test
{
    [TestClass]
    public class LoggerTest
    {
        private static Logger logger = new Logger();

        [TestMethod]
        public void WriteTest()
        {
            logger.Info("info log");
            try
            {
                string str = null;
                str = str.ToString();
            }
            catch(Exception ex)
            {
                logger.Error("error log", ex);
            }
            
            Thread.Sleep(1000);
        }
    }
}
