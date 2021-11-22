using System;
using System.Collections.Generic;
using System.Text;
using NLog;
//Logger//
namespace SynetecAssessmentApi.Logger
{
    public class Logger : ILogger
    {
        private static NLog.ILogger logger = LogManager.GetCurrentClassLogger();

        public Logger()
        {
        }

        public void Information(string message)
        {
            logger.Info(message);
        }

        public void Warning(string message)
        {
            logger.Warn(message);
        }

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }

        public void Error(Exception ex)
        {
            logger.Error(ex.Message.ToString());
        }

        public void Error(string message,Exception ex)
        {
            logger.Error(message + "  " + ex.Message.ToString());
        }
    }
}
