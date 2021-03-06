﻿#pragma checksum "..\..\..\CustomUserControls\UserControlLikePhotoByUserName.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "CADFB0E8C05C0CEC97A8902EE2B5B897"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Converters;
using FirstFloor.ModernUI.Windows.Navigation;
using GramDominator.Classes;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace GramDominator.CustomUserControls {
    
    
    /// <summary>
    /// UserControlLikePhotoByUserName
    /// </summary>
    public partial class UserControlLikePhotoByUserName : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 13 "..\..\..\CustomUserControls\UserControlLikePhotoByUserName.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal GramDominator.Classes.Validation objform;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\CustomUserControls\UserControlLikePhotoByUserName.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rdoBtn_LikeBy_PhotoUser_SingleUser;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\CustomUserControls\UserControlLikePhotoByUserName.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rdoBtn_LikeBy_PhotoUser_MultipleUser;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\CustomUserControls\UserControlLikePhotoByUserName.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Likeby_Username;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\CustomUserControls\UserControlLikePhotoByUserName.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txt_LikePhoto_Username_LoadUsersPath;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\CustomUserControls\UserControlLikePhotoByUserName.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_LikePhoto_Username_BrowseUsers;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\CustomUserControls\UserControlLikePhotoByUserName.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtMessage_Like_NoOfFollowers;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\CustomUserControls\UserControlLikePhotoByUserName.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_LikePhoto_Username_Clear;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/GramDominator;component/customusercontrols/usercontrollikephotobyusername.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\CustomUserControls\UserControlLikePhotoByUserName.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.objform = ((GramDominator.Classes.Validation)(target));
            return;
            case 2:
            this.rdoBtn_LikeBy_PhotoUser_SingleUser = ((System.Windows.Controls.RadioButton)(target));
            
            #line 40 "..\..\..\CustomUserControls\UserControlLikePhotoByUserName.xaml"
            this.rdoBtn_LikeBy_PhotoUser_SingleUser.Checked += new System.Windows.RoutedEventHandler(this.rdoBtn_LikeBy_Username_SingleUser_Checked);
            
            #line default
            #line hidden
            return;
            case 3:
            this.rdoBtn_LikeBy_PhotoUser_MultipleUser = ((System.Windows.Controls.RadioButton)(target));
            
            #line 41 "..\..\..\CustomUserControls\UserControlLikePhotoByUserName.xaml"
            this.rdoBtn_LikeBy_PhotoUser_MultipleUser.Checked += new System.Windows.RoutedEventHandler(this.rdoBtn_LikeBy_Username_MultipleUser_Checked);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Likeby_Username = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.txt_LikePhoto_Username_LoadUsersPath = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.btn_LikePhoto_Username_BrowseUsers = ((System.Windows.Controls.Button)(target));
            
            #line 44 "..\..\..\CustomUserControls\UserControlLikePhotoByUserName.xaml"
            this.btn_LikePhoto_Username_BrowseUsers.Click += new System.Windows.RoutedEventHandler(this.btn_LikePhoto_Username_BrowseUsers_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.txtMessage_Like_NoOfFollowers = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.btn_LikePhoto_Username_Clear = ((System.Windows.Controls.Button)(target));
            
            #line 57 "..\..\..\CustomUserControls\UserControlLikePhotoByUserName.xaml"
            this.btn_LikePhoto_Username_Clear.Click += new System.Windows.RoutedEventHandler(this.btn_LikePhoto_Username_Clear_click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

