﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TFSAdministrationTool.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("TFSAdministrationTool.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to TFS Administration Tool Home Page: http://tfsadmin.codeplex.com/
        ///
        ///TFS Administration Tool is a joint Microsoft and community effort to bring you a Power Tool for Team Foundation Server..
        /// </summary>
        internal static string AboutDescription {
            get {
                return ResourceManager.GetString("AboutDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to About TFS Administraion Tool.
        /// </summary>
        internal static string AboutTitle {
            get {
                return ResourceManager.GetString("AboutTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap aboutToolStripButton {
            get {
                object obj = ResourceManager.GetObject("aboutToolStripButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        internal static System.Drawing.Icon App {
            get {
                object obj = ResourceManager.GetObject("App", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Bad smtp configuration.
        /// </summary>
        internal static string BadSMTPCaption {
            get {
                return ResourceManager.GetString("BadSMTPCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}\nCheck the config file.
        /// </summary>
        internal static string BadSMTPPrompt {
            get {
                return ResourceManager.GetString("BadSMTPPrompt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Team project(s) {0} has/have uncommited changes. If you unselect any of these team project(s), the uncommited changes will be lost. Would you like to continue?.
        /// </summary>
        internal static string ConnectPendingChangesExistsPrompt {
            get {
                return ResourceManager.GetString("ConnectPendingChangesExistsPrompt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Changes exists.
        /// </summary>
        internal static string DiscardChangesCaption {
            get {
                return ResourceManager.GetString("DiscardChangesCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You have made changes that are uncommited do you wish to discard them?.
        /// </summary>
        internal static string DiscardChangesPrompt {
            get {
                return ResourceManager.GetString("DiscardChangesPrompt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occured when connecting to {0} for the selected team project, to prevent any data corruption you will not be able to administrate this project!.
        /// </summary>
        internal static string ErrorTierConnection {
            get {
                return ResourceManager.GetString("ErrorTierConnection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap failed {
            get {
                object obj = ResourceManager.GetObject("failed", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        internal static System.Drawing.Icon Group {
            get {
                object obj = ResourceManager.GetObject("Group", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap historyClearToolStripButton {
            get {
                object obj = ResourceManager.GetObject("historyClearToolStripButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap historyTab {
            get {
                object obj = ResourceManager.GetObject("historyTab", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Getting list of Team Projects from server....
        /// </summary>
        internal static string LoadingProjectInfoStatusPrompt {
            get {
                return ResourceManager.GetString("LoadingProjectInfoStatusPrompt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Loading user information....
        /// </summary>
        internal static string LoadingUserInfoStatusPrompt {
            get {
                return ResourceManager.GetString("LoadingUserInfoStatusPrompt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not send mail to {0}\n{1}.
        /// </summary>
        internal static string MailCouldNotBeSentError {
            get {
                return ResourceManager.GetString("MailCouldNotBeSentError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The team project you are trying to connect to have no {0} associated with it. Any missing tier will be disabled from editing and greyed out..
        /// </summary>
        internal static string MissingTierConnection {
            get {
                return ResourceManager.GetString("MissingTierConnection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;html xmlns:v=&quot;urn:schemas-microsoft-com:vml&quot; xmlns:o=&quot;urn:schemas-microsoft-com:office:office&quot; xmlns:w=&quot;urn:schemas-microsoft-com:office:word&quot; xmlns:m=&quot;http://schemas.microsoft.com/office/2004/12/omml&quot; xmlns=&quot;http://www.w3.org/TR/REC-html40&quot;&gt;
        ///   &lt;head&gt;
        ///      &lt;meta http-equiv=Content-Type content=&quot;text/html; charset=utf-8&quot;&gt;
        ///      &lt;meta name=Generator content=&quot;Microsoft Word 15 (filtered medium)&quot;&gt;
        ///      &lt;style&gt;
        ///         &lt;!--
        ///            /* Font Definitions */
        ///            @font-face
        ///            	{font [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string NotificationBody {
            get {
                return ResourceManager.GetString("NotificationBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;p class=Pucesniv1 style=&apos;mso-list:l0 level1 lfo1&apos;&gt;
        ///	&lt;![if !supportLists]&gt;&lt;span lang=EN-US style=&apos;font-size:10.0pt;font-family:Wingdings;color:#FF6600&apos;&gt;&lt;span style=&apos;mso-list:Ignore&apos;&gt;§&lt;span style=&apos;font:7.0pt &quot;Times New Roman&quot;&apos;&gt;&amp;nbsp; &lt;/span&gt;&lt;/span&gt;&lt;/span&gt;&lt;![endif]&gt;
        ///	&lt;span lang=EN-US&gt;
        ///	   Project {0}\{1} : &lt;b&gt;{2} role {3}&lt;/b&gt; in tier &lt;i&gt;{4}&lt;/i&gt; 
        ///	   &lt;o:p&gt;&lt;/o:p&gt;
        ///	&lt;/span&gt;
        /// &lt;/p&gt;.
        /// </summary>
        internal static string NotificationLine {
            get {
                return ResourceManager.GetString("NotificationLine", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Changes in your TFS subscription.
        /// </summary>
        internal static string NotificationSubject {
            get {
                return ResourceManager.GetString("NotificationSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap outputClearToolStripButton {
            get {
                object obj = ResourceManager.GetObject("outputClearToolStripButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap outputTab {
            get {
                object obj = ResourceManager.GetObject("outputTab", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap passed {
            get {
                object obj = ResourceManager.GetObject("passed", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Adding.
        /// </summary>
        internal static string PendingChangeActionAdd {
            get {
                return ResourceManager.GetString("PendingChangeActionAdd", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Deleting.
        /// </summary>
        internal static string PendingChangeActionDelete {
            get {
                return ResourceManager.GetString("PendingChangeActionDelete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap pendingChangesCommitToolStripButton {
            get {
                object obj = ResourceManager.GetObject("pendingChangesCommitToolStripButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Team Foundation Server {0} hs some uncommited changes. If you disconnect from the server, the changes will be lost. Would you like to continue?.
        /// </summary>
        internal static string PendingChangesExistsPrompt {
            get {
                return ResourceManager.GetString("PendingChangesExistsPrompt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap pendingchangesTab {
            get {
                object obj = ResourceManager.GetObject("pendingchangesTab", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap pendingChangesUndoToolStripButton {
            get {
                object obj = ResourceManager.GetObject("pendingChangesUndoToolStripButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Refreshing list of Team Projects from server....
        /// </summary>
        internal static string RefreshServerListStatusPrompt {
            get {
                return ResourceManager.GetString("RefreshServerListStatusPrompt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap server {
            get {
                object obj = ResourceManager.GetObject("server", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Connecting to server....
        /// </summary>
        internal static string ServerConnectStatusPrompt {
            get {
                return ResourceManager.GetString("ServerConnectStatusPrompt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap serverConnectToolStripButton {
            get {
                object obj = ResourceManager.GetObject("serverConnectToolStripButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Disconnecting from server....
        /// </summary>
        internal static string ServerDisconnectStatusPrompt {
            get {
                return ResourceManager.GetString("ServerDisconnectStatusPrompt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap serverDisconnectToolStripButton {
            get {
                object obj = ResourceManager.GetObject("serverDisconnectToolStripButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap serverRefreshToolStripButton {
            get {
                object obj = ResourceManager.GetObject("serverRefreshToolStripButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please use Team Explorer to edit server permissions for Team Foundation Server 2005/2008 or TFS Administration Console to edit server or team project collection permissions for Team Foundation Server 2010 and later..
        /// </summary>
        internal static string ServerWarningPrompt {
            get {
                return ResourceManager.GetString("ServerWarningPrompt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Done.
        /// </summary>
        internal static string TaskDoneStatusPrompt {
            get {
                return ResourceManager.GetString("TaskDoneStatusPrompt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap teamproject {
            get {
                object obj = ResourceManager.GetObject("teamproject", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unauthorized connection to {0}.
        /// </summary>
        internal static string UnauthorizedTier {
            get {
                return ResourceManager.GetString("UnauthorizedTier", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You are unauthorized to administrate {0} for the selected team project, to prevent any data corruption you will not be able to administrate this project!.
        /// </summary>
        internal static string UnauthorizedTierConnection {
            get {
                return ResourceManager.GetString("UnauthorizedTierConnection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occured.
        /// </summary>
        internal static string UnexpectedExceptionCaption {
            get {
                return ResourceManager.GetString("UnexpectedExceptionCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unexpected error occured, please consult the logs and file a bug with the project team..
        /// </summary>
        internal static string UnexpectedExceptionPrompt {
            get {
                return ResourceManager.GetString("UnexpectedExceptionPrompt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        internal static System.Drawing.Icon User {
            get {
                object obj = ResourceManager.GetObject("User", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        internal static System.Drawing.Icon User_Added {
            get {
                object obj = ResourceManager.GetObject("User_Added", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        internal static System.Drawing.Icon User_Deleted {
            get {
                object obj = ResourceManager.GetObject("User_Deleted", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        internal static System.Drawing.Icon User_Edited {
            get {
                object obj = ResourceManager.GetObject("User_Edited", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This user already exist..
        /// </summary>
        internal static string UserAlreadyExistCaption {
            get {
                return ResourceManager.GetString("UserAlreadyExistCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The user {0} already exists in {1} do you wish to reset the assigned roles?\r\nPlease note that only roles not in a indetermine state will be added to the new role assignment!.
        /// </summary>
        internal static string UserAlreadyExistPrompt {
            get {
                return ResourceManager.GetString("UserAlreadyExistPrompt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Editing selected users....
        /// </summary>
        internal static string UserEdtingStatusPrompt {
            get {
                return ResourceManager.GetString("UserEdtingStatusPrompt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This user has pending changes..
        /// </summary>
        internal static string UserHasPendingChangesCaption {
            get {
                return ResourceManager.GetString("UserHasPendingChangesCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The user {0} already has pending changes that are uncomitted, please persist the changes before adding this user!.
        /// </summary>
        internal static string UserHasPendingChangesPrompt {
            get {
                return ResourceManager.GetString("UserHasPendingChangesPrompt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This user already exists in target team project.
        /// </summary>
        internal static string UserImportExistsChangesCaption {
            get {
                return ResourceManager.GetString("UserImportExistsChangesCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The user {0} already exists in the {1} team project! Importing this user will overwrite the user&apos;s permission(s) in the {1} team project..
        /// </summary>
        internal static string UserImportExistsChangesPrompt {
            get {
                return ResourceManager.GetString("UserImportExistsChangesPrompt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The current team project have selected users..
        /// </summary>
        internal static string UserImportSelectionExistsChangesCaption {
            get {
                return ResourceManager.GetString("UserImportSelectionExistsChangesCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The current team project have users selected for import, if you change project you will loose these changes. Do you wish to continue?.
        /// </summary>
        internal static string UserImportSelectionExistsChangesPrompt {
            get {
                return ResourceManager.GetString("UserImportSelectionExistsChangesPrompt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please wait while resolving user identity..
        /// </summary>
        internal static string UserResolvmentStatusPrompt {
            get {
                return ResourceManager.GetString("UserResolvmentStatusPrompt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap usersAddToolStripButton {
            get {
                object obj = ResourceManager.GetObject("usersAddToolStripButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap usersDeleteToolStripButton {
            get {
                object obj = ResourceManager.GetObject("usersDeleteToolStripButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap usersEditToolStripButton {
            get {
                object obj = ResourceManager.GetObject("usersEditToolStripButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User(s) ({0}) cannot be marked for deletion due to one of the following reasons:
        ///  1) User is already marked for deletion
        ///  2) User has pending changes
        ///  3) User is part of the Service Accounts group
        ///  4) Cannot remove the current user from Team Foundation Administrators group
        ///    .
        /// </summary>
        internal static string UsersGridDeleteWarningMessage {
            get {
                return ResourceManager.GetString("UsersGridDeleteWarningMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User(s) ({0}) cannot be edited due to one of the following reasons:
        ///  1) User is already marked for deletion
        ///  2) User is part of the Service Accounts group
        ///  3) Current user cannot be edited at server level
        ///    .
        /// </summary>
        internal static string UsersGridEditWarningMessage {
            get {
                return ResourceManager.GetString("UsersGridEditWarningMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap usersRefreshStripButton {
            get {
                object obj = ResourceManager.GetObject("usersRefreshStripButton", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User(s) ({0}) cannot be resolved..
        /// </summary>
        internal static string UsersUnresolvedMessage {
            get {
                return ResourceManager.GetString("UsersUnresolvedMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Warning.
        /// </summary>
        internal static string WarningCaption {
            get {
                return ResourceManager.GetString("WarningCaption", resourceCulture);
            }
        }
    }
}
