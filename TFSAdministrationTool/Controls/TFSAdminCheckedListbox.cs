#region Using Statements
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
#endregion

namespace TFSAdministrationTool.Controls
{
  /// <summary>
  /// I had to subclass the CheckedListBox control in order to render
  /// the text in different colors.
  /// </summary>
  public class TFSAdminCheckedListBox : CheckedListBox
  {
    public event EventHandler<ValidateItemEventArgs> ValidateItem;

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
      //This lets the control work 
      //properly in designer mode.
      if (Items.Count == 0)
      {
        base.OnDrawItem(e);
        return;
      }

      ValidateItemEventArgs vi = new ValidateItemEventArgs(e.Index);
      if (ValidateItem != null)
        ValidateItem(this, vi);

      CheckBoxState targetState = CheckBoxState.UncheckedNormal;
      Brush targetBrush = null;
      Font targetFont = null;
      FontStyle targetStyle = FontStyle.Regular;

      switch (GetItemCheckState(e.Index))
      {
        case CheckState.Checked:
          targetState = Enabled ? CheckBoxState.CheckedNormal : CheckBoxState.CheckedDisabled;
          targetStyle = FontStyle.Bold;
          break;
        case CheckState.Unchecked:
          targetState = Enabled ? CheckBoxState.UncheckedNormal : CheckBoxState.UncheckedDisabled;
          targetStyle = FontStyle.Regular;
          break;
        case CheckState.Indeterminate:
          targetState = Enabled ? CheckBoxState.MixedNormal : CheckBoxState.MixedDisabled;
          targetStyle = FontStyle.Italic;
          break;
      }

      if (vi.IsDisabled)
      {
        targetState = CheckBoxState.UncheckedDisabled;
        targetStyle |= FontStyle.Strikeout;
      }
              
      targetFont = new Font(e.Font.FontFamily, e.Font.Size, targetStyle);

      if (vi.IsValid)
        if (vi.IsDisabled)
          targetBrush = Brushes.DarkGray;
        else
          targetBrush = Enabled ? Brushes.Black : Brushes.LightGray;
      else
        targetBrush = Brushes.Red;

      e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);
      CheckBoxRenderer.DrawCheckBox(e.Graphics, new Point(e.Bounds.X + 2, e.Bounds.Y + 2), targetState);
      e.Graphics.DrawString(Items[e.Index].ToString(), targetFont, targetBrush, e.Bounds.X + 16, e.Bounds.Y);

      //Dispose all create graphic objects
      targetFont.Dispose();
    }
  } //End Class
  
  public class ValidateItemEventArgs : EventArgs
  {
    private int m_index;
    private bool m_valid;
    private bool m_disabled;

    public int Index { get { return m_index; } }
    public bool IsValid { get { return m_valid; } set { m_valid = value; } }
    public bool IsDisabled { get { return m_disabled; } set { m_disabled = value; } }

    public ValidateItemEventArgs(int index)
    {
      m_index = index;
      m_valid = true;
      m_disabled = false;
    }
  } //End Class
} //End Namespace