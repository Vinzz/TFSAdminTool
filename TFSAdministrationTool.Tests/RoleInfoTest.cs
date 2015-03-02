using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TFSAdministrationTool.Proxy.Common;

namespace TFSAdministrationTool.Tests
{
  [TestClass()]
  public class RoleInfoTest
  {
    public TestContext TestContext { get; set; }

    #region RoleInto Class unit tests
    [TestMethod]
    public void RoleInfoConstructorWithRoleNameTest()
    {
      RoleInfo roleInfo = new RoleInfo("Project Administrators");

      Assert.AreEqual("Project Administrators", roleInfo.Name, "RoleInfo.Name property not initialized correctly");
      Assert.AreEqual(true, roleInfo.Enabled, "RoleInfo.Enabled property not initialized correctly");
    }

    [TestMethod]
    public void RoleInfoConstructorWithDetailsTest()
    {
      RoleInfo roleInfo = new RoleInfo("Service Accounts", false);

      Assert.AreEqual("Service Accounts", roleInfo.Name, "RoleInfo.Name property not initialized correctly");
      Assert.AreEqual(false, roleInfo.Enabled, "RoleInfo.Enabled property not initialized correctly");
    }
    #endregion

    #region RoleInfoCollection Class unit tests
    [TestMethod]
    public void RoleInfoCollectionConstructorTest()
    {
      RoleInfoCollection roleCollection = InitializeRoleInfoCollection(null);

      Assert.AreEqual<int>(0, roleCollection.All.Count, "RoleInfoCollection constructor does not create an empty collection");
    }

    [TestMethod]
    public void RoleInfoCollectionAddTest()
    {
      RoleInfoCollection roleCollection = InitializeRoleInfoCollection(new string[] { "Project Administrators" });

      Assert.AreEqual<int>(1, roleCollection.All.Count, "RoleInfoCollection Add method did not add the new RoleInfo to the collection");
      Assert.AreEqual<bool>(true, roleCollection.Contains("Project Administrators"), "RoleInfoCollection does not contain the newly added RoleInfo");
    }

    [TestMethod]
    public void RoleInfoCollectionClearTest()
    {
      RoleInfoCollection roleCollection = InitializeRoleInfoCollection(new string[] { "Project Administrators", "Contributors" });
      roleCollection.Clear();

      Assert.AreEqual<int>(0, roleCollection.All.Count, "RoleInfoCollection Clear did not remove all items from the colleciton");
    }

    [TestMethod]
    public void RoleInfoCollectionContainsTest()
    {
      RoleInfoCollection roleCollection = InitializeRoleInfoCollection(new string[] { "Project Administrators" });

      Assert.AreEqual<bool>(true, roleCollection.Contains("Project Administrators"), "RoleInfoCollection Contains did not return the correct value");
    }

    [TestMethod]
    public void RoleInfoCollectionGetDeltaTest()
    {
      RoleInfoCollection sourceCollection = InitializeRoleInfoCollection(new string[] { "Project Administrators" });
      RoleInfoCollection targetCollection = InitializeRoleInfoCollection(new string[] { "Contributors" });

      Dictionary<string, ChangeType> delta = RoleInfoCollection.GetDelta(sourceCollection, targetCollection);

      Assert.AreEqual<int>(2, delta.Count, "GetDelta did not return the correct number changes");

      Assert.AreEqual<bool>(true, delta.ContainsKey("Project Administrators"), "GetDelta did not return the collection with the correct item");
      Assert.AreEqual<bool>(true, delta.ContainsKey("Contributors"), "GetDelta did not return the collection with the correct item");

      Assert.AreEqual<ChangeType>(ChangeType.Delete, delta["Project Administrators"], "GetDelta did not return the collection with the correct ChangeType");
      Assert.AreEqual<ChangeType>(ChangeType.Add, delta["Contributors"], "GetDelta did not return the collection with the correct ChangeType");
    }  
    
    [TestMethod]
    public void RoleInfoCollectionRemoveTest()
    {
      RoleInfoCollection roleCollection = InitializeRoleInfoCollection(new string[] { "Project Administrators", "Contributors" });
      roleCollection.Remove("Project Administrators");

      Assert.AreEqual<int>(1, roleCollection.All.Count, "RoleInfoCollection Remove method did not remove the RoleInfo to the collection");
      Assert.AreEqual<bool>(false, roleCollection.Contains("Project Administrators"), "RoleInfoCollection still contains the removed RoleInfo");
    }

    [TestMethod]
    public void RoleInfoCollectionSortTest()
    {
      RoleInfoCollection roleCollection = InitializeRoleInfoCollection(new string[] { "Project Administrators", "Contributors" });
      roleCollection.Sort();

      Assert.AreEqual<string>("Contributors,Project Administrators", roleCollection.ToString(), "RoleInfoCollection Sort does not sor the items");
    }
    
    [TestMethod]
    public void RoleInfoCollectionToStringTest()
    {
      RoleInfoCollection roleCollection = InitializeRoleInfoCollection(new string[] { "Project Administrators", "Contributors" });
      Assert.AreEqual<string>("Contributors,Project Administrators", roleCollection.ToString(), "RoleInfoCollection ToString does not work with more than one item in the collection");
    }
    #endregion

    #region Helper Methods
    private RoleInfoCollection InitializeRoleInfoCollection(string[] roles)
    {
      RoleInfoCollection collectiton = new RoleInfoCollection();

      if (roles != null && roles.Length > 0)
      {
        foreach(string r in roles) {
          collectiton.Add(r);
        }
      }

      return collectiton;
    }
    #endregion
  }
}