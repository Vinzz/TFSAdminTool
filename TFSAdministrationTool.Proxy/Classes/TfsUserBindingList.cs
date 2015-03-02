#region Using Statements
using System;
using System.Collections.Generic;
using System.ComponentModel;
#endregion

namespace TFSAdministrationTool.Proxy.Common
{
  public class TfsUserBindingList : BindingList<TfsUser>
  {
    #region Fields
    private bool m_IsSorted;
    private ListSortDirection m_SortDirection;
    private PropertyDescriptor m_SortProperty;
    #endregion

    #region Constructor
    public TfsUserBindingList() : base()
    {
      // By default sort by DisplayName
      m_SortProperty = TypeDescriptor.GetProperties(typeof(TfsUser))["DisplayName"];
      m_SortDirection = ListSortDirection.Ascending;
    }
    #endregion

    #region Methods
    public void InitializeList(List<TfsUser> users)
    {
      List<TfsUser> items = (List<TfsUser>)this.Items;

      items.Clear();      
      items.AddRange(users);

      ApplySortCore(m_SortProperty, m_SortDirection);
    }

    protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
    {
      m_SortProperty = prop;
      m_SortDirection = direction;

      List<TfsUser> items = (List<TfsUser>)this.Items;

      if (items != null)
      {
        SortComparer<TfsUser> comparer = new SortComparer<TfsUser>(prop, direction);
        items.Sort(comparer);
        m_IsSorted = true;

        OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
      }
      else
      {
        m_IsSorted = false;
      }
    }
    #endregion

    #region Properties
    protected override bool SupportsSearchingCore
    {
      get
      {
        return true;
      }
    }

    protected override bool SupportsSortingCore
    {
      get
      {
        return true;
      }
    }

    protected override bool IsSortedCore
    {
      get
      {
        return m_IsSorted;
      }
    }

    protected override ListSortDirection SortDirectionCore
    {
      get
      {
        return m_SortDirection;
      }
    }

    protected override PropertyDescriptor SortPropertyCore
    {
      get
      {
        return m_SortProperty;
      }
    }
    #endregion

    #region SortComparer class
    private class SortComparer<T> : IComparer<T>
    {
      private PropertyDescriptor m_PropDesc;
      private ListSortDirection m_Direction;

      public SortComparer(PropertyDescriptor propDesc, ListSortDirection direction)
      {
        m_PropDesc = propDesc;
        m_Direction = direction;
      }

      int IComparer<T>.Compare(T x, T y)
      {
        object xValue = m_PropDesc.GetValue(x);
        object yValue = m_PropDesc.GetValue(y);
        return CompareValues(xValue, yValue, m_Direction);
      }

      private int CompareValues(object xValue, object yValue, ListSortDirection direction)
      {
        int retValue = 0;
        if (xValue is IComparable) //can ask the x value
        {
          retValue = ((IComparable)xValue).CompareTo(yValue);
        }
        else if (yValue is IComparable) //can ask the y value
        {
          retValue = ((IComparable)yValue).CompareTo(xValue);
        }
        //not comparable, compare string representations
        else if (!xValue.Equals(yValue))
        {
          retValue = xValue.ToString().CompareTo(yValue.ToString());
        }
        if (direction == ListSortDirection.Ascending)
          return retValue;
        else
          return retValue * -1;
      }
    }
    #endregion
  } // End Class
} // End Namespace