using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SupplierManagement
{
    public class Log
    {
        private static log4net.ILog myLogger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        //错误级别:Info
        public static void Info(Exception ex, string message)
        {
            myLogger.Info(AppendMessage(ex, message));
        }

        public static void Info(string message)
        {
            myLogger.Info(message);
        }
        //错误级别:Debug
        public static void Debug(Exception ex, string message)
        {
            myLogger.Debug(AppendMessage(ex, message));
        }
        //错误级别:Warn
        public static void Warn(Exception ex, string message)
        {
            myLogger.Warn(AppendMessage(ex, message));
        }
        //错误级别:Fatal
        public static void Fatal(Exception ex, string message)
        {
            myLogger.Fatal(AppendMessage(ex, message));
        }
        //错误级别:Error
        public static void Error(Exception ex, string message)
        {
            myLogger.Error(AppendMessage(ex, message));
        }
        public static void Error(string message)
        {
            myLogger.Error(message);
        }
        public static void Error(Exception ex)
        {
            myLogger.Error(ex.Message +Environment.NewLine+ " 【所在位置】:" + ex.StackTrace);
        }

        //拼接错误信息
        private static string AppendMessage(Exception ex, string message)
        {

            return "错误信息：" + message + "。系统错误信息:" + ex.Message + ",发生时间：" +
                   DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

   
    }
}