﻿#pragma checksum "..\..\..\..\Pages\PagePhoto\UserControlLikePhoto.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C850A761B8ECF27950734365EF07EA77"
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


namespace GramDominator.Pages.PagePhoto {
    
    
    /// <summary>
    /// UserControlLikePhoto
    /// </summary>
    public partial class UserControlLikePhoto : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 13 "..\..\..\..\Pages\PagePhoto\UserControlLikePhoto.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal GramDominator.Classes.Validation objform;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\..\Pages\PagePhoto\UserControlLikePhoto.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Select_To_LikePhoto;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\..\Pages\PagePhoto\UserControlLikePhoto.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtMessage_Like_NoOfThreads;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\..\..\Pages\PagePhoto\UserControlLikePhoto.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtMessage_Like_DelayMin;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\..\Pages\PagePhoto\UserControlLikePhoto.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtMessage_Like_DelayMax;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\..\..\Pages\PagePhoto\UserControlLikePhoto.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnMessage_Like_Start;
        
        #line default
        #line hidden
        
        
        #line 103 "..\..\..\..\Pages\PagePhoto\UserControlLikePhoto.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnMessage_Like_Stop;
        
        #line default
        #line hidden
        
        
        #line 122 "..\..\..\..\Pages\PagePhoto\UserControlLikePhoto.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar Unfollower_Progess;
        
        #line default
        #line hidden
        
        
        #line 128 "..\..\..\..\Pages\PagePhoto\UserControlLikePhoto.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dtGrdLikePhoto_AccountsReport;
        
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
            System.Uri resourceLocater = new System.Uri("/GramDominator;component/pages/pagephoto/usercontrollikephoto.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Pages\PagePhoto\UserControlLikePhoto.xaml"
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
            this.Select_To_LikePhoto = ((System.Windows.Controls.ComboBox)(target));
            
            #line 45 "..\..\..\..\Pages\PagePhoto\UserControlLikePhoto.xaml"
            this.Select_To_LikePhoto.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.Select_To_LikePhoto_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.txtMessage_Like_NoOfThreads = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.txtMessage_Like_DelayMin = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.txtMessage_Like_DelayMax = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.btnMessage_Like_Start = ((System.Windows.Controls.Button)(target));
            
            #line 94 "..\..\..\..\Pages\PagePhoto\UserControlLikePhoto.xaml"
            this.btnMessage_Like_Start.Click += new System.Windows.RoutedEventHandler(this.btnMessage_Like_Start_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnMessage_Like_Stop = ((System.Windows.Controls.Button)(target));
            
            #line 103 "..\..\..\..\Pages\PagePhoto\UserControlLikePhoto.xaml"
            this.btnMessage_Like_Stop.Click += new System.Windows.RoutedEventHandler(this.btnMessage_Like_Stop_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.Unfollower_Progess = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 9:
            this.dtGrdLikePhoto_AccountsReport = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 10:
            
            #line 132 "..\..\..\..\Pages\PagePhoto\UserControlLikePhoto.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.RefreshAccountreport_PhotoLike_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 133 "..\..\..\..\Pages\PagePhoto\UserControlLikePhoto.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.DeleteAccountModule_PhotoLike_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 134 "..\..\..\..\Pages\PagePhoto\UserControlLikePhoto.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ExportPhotoLike_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

