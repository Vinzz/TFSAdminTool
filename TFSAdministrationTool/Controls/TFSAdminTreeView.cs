#region Using Statements
using System;
using System.Windows.Forms;
using TFSAdministrationTool.Proxy.Common;
#endregion

namespace TFSAdministrationTool.Controls
{  
  /// <summary>
  /// I had to subclass the TreeView control in order to disable
  /// the automatic Expand() and Collaps() when you double click on
  /// a TreeViewNode.
  /// </summary>
  public class TFSAdminTreeView: TreeView
  {
    #region Fields
    const int WM_LBUTTONDBLCLK = 0x0203;
    #endregion

    #region Methods
    /// <summary>
    /// We discard double click message to avoid automatic
    /// Expand and Collaps of the a TreeViewNode
    /// </summary>
    /// <param name="m"></param>
    protected override void WndProc(ref Message m)
    {
      if (m.Msg == WM_LBUTTONDBLCLK)
      {
        this.OnDoubleClick(new EventArgs());
        return;
      }
      base.WndProc(ref m);
    }
    #endregion
  } //End Class

  public class TFSAdminTreeNodeTag
  {
    #region Fields
    public Guid InstanceId { get; set; }
    public Uri Uri { get; set; }
    public ItemType ItemType { get; set; }    
    #endregion
  }
} //End Namespace