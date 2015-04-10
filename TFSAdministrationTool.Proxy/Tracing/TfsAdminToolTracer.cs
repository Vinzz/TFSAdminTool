#region Using Statements
using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
#endregion

namespace TFSAdministrationTool.Proxy.Common
{
  public static class TfsAdminToolTracer
  {
    #region Fields
    private static TextBoxTraceListener m_TextBoxTraceListener;
    private static TraceSwitch m_TraceSwitch;
    #endregion

    #region Methods
    public static void Destroy()
    {
      m_TextBoxTraceListener.Flush();
      m_TextBoxTraceListener.Close();      
      Trace.Listeners.Remove(m_TextBoxTraceListener);
      FlushToLogFile();
    }

    private static void FlushToLogFile()
    {
      try
      {
        string logPath = Application.StartupPath + "\\Logs\\";
        string logFile = "TFSAdminTool-" + DateTime.Now.ToFileTime().ToString() + ".log";

        if (!Directory.Exists(logPath))
          Directory.CreateDirectory(logPath);

        File.WriteAllText(logPath + logFile, m_TextBoxTraceListener.TextBox.Text);
      }
      catch (Exception)
      {
        /// We do not want to throw an exception while closing the application
      }
    }
    
    public static void Initialize(TextBox textBox)
    {
      m_TextBoxTraceListener = new TextBoxTraceListener(textBox);
      m_TraceSwitch = new TraceSwitch("TFSAdminToolTraceSwitch", "TFS Admin Tool trace switch");
      Trace.Listeners.Add(m_TextBoxTraceListener);
    }

    public static void TraceException(bool condition, Exception ex)
    {
      string indent = "                       ";
      Trace.WriteLineIf(condition, DateTime.Now + ": Exception occurred");
      Trace.WriteLineIf(condition, indent + "Type: " + ex.GetType());
      Trace.WriteLineIf(condition, indent + "Message: " + ex.Message);
      Trace.WriteLineIf(condition, indent + "InnerException: " + ((ex.InnerException == null) ? "-": ex.InnerException.GetType().Name));
      Trace.WriteLineIf(condition, indent + "Source: " + ex.Source);
      Trace.WriteLineIf(condition, indent + "Target: " + ex.TargetSite);
      Trace.WriteLineIf(condition, indent + "Stacktrace: " + ex.StackTrace);
      
      m_TextBoxTraceListener.Flush();
    }

    public static void TraceMessage(bool condition, string message)
    {
      Trace.WriteLineIf(condition, DateTime.Now + ": " + message);
      m_TextBoxTraceListener.Flush();
    }

    #endregion

    #region Properties
    public static TraceSwitch TraceSwitch
    {
      get
      {
        return m_TraceSwitch;
      }
    }
    #endregion
  } //End Class
} //End Namespace