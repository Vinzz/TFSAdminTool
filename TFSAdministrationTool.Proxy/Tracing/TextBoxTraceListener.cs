#region Using Statements
using System;
using System.Diagnostics;
using System.Windows.Forms;
#endregion

namespace TFSAdministrationTool.Proxy.Common
{
  public class TextBoxTraceListener: TraceListener
  {
    #region Fields
    private TextBox m_TextBox;
    #endregion

    #region Constructors
    public TextBoxTraceListener(TextBox textBox)
    {
      m_TextBox = textBox;
    }
    #endregion

    #region Methods
    public override void Write(string message)
    {
      m_TextBox.Text += message;    
    }

    public override void WriteLine(string message)
    {
      m_TextBox.Text += message + Environment.NewLine;
    }
    #endregion

    #region Properties
    public TextBox TextBox
    {
      get
      {
        return m_TextBox;
      }
    }
    #endregion
  } //End Class
} //End Namespace
