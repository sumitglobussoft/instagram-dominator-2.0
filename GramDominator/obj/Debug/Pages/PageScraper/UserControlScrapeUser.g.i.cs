﻿#pragma checksum "..\..\..\..\Pages\PageScraper\UserControlScrapeUser.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5016CBD345C1C09FF98282A0A06E8E61"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace GramDominator.Pages.PageScraper {
    
    
    /// <summary>
    /// UserControlScrapeUser
    /// </summary>
    public partial class UserControlScrapeUser : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\..\Pages\PageScraper\UserControlScrapeUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal GramDominator.Classes.Validation objform;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\..\Pages\PageScraper\UserControlScrapeUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Select_To_Scrapeuser;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\..\Pages\PageScraper\UserControlScrapeUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtMessage_ScrapeUser_NoOfThreads;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\..\Pages\PageScraper\UserControlScrapeUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtMessage_ScrapeUser_DelayMin;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\..\Pages\PageScraper\UserControlScrapeUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtMessage_ScrapeUser_DelayMax;
        
        #line default
        #line hidden
        
        
        #line 92 "..\..\..\..\Pages\PageScraper\UserControlScrapeUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnMessage_ScrapeUser_Start;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\..\..\Pages\PageScraper\UserControlScrapeUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnMessage_ScrapeUser_Stop;
        
        #line default
        #line hidden
        
        
        #line 121 "..\..\..\..\Pages\PageScraper\UserControlScrapeUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar Unfollower_Progess;
        
        #line default
        #line hidden
        
        
        #line 127 "..\..\..\..\Pages\PageScraper\UserControlScrapeUser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dtGrdScrapeUser_ScrapeUser_AccountsReport;
        
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
            System.Uri resourceLocater = new System.Uri("/GramDominator;component/pages/pagescraper/usercontrolscrapeuser.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Pages\PageScraper\UserControlScrapeUser.xaml"
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
            this.Select_To_Scrapeuser = ((System.Windows.Controls.ComboBox)(target));
            
            #line 44 "..\..\..\..\Pages\PageScraper\UserControlScrapeUser.xaml"
            this.Select_To_Scrapeuser.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.Select_To_ScrapeUser_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.txtMessage_ScrapeUser_NoOfThreads = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.txtMessage_ScrapeUser_DelayMin = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.txtMessage_ScrapeUser_DelayMax = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.btnMessage_ScrapeUser_Start = ((System.Windows.Controls.Button)(target));
            
            #line 92 "..\..\..\..\Pages\PageScraper\UserControlScrapeUser.xaml"
            this.btnMessage_ScrapeUser_Start.Click += new System.Windows.RoutedEventHandler(this.btnMessage_ScrapeUSer_Start_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnMessage_ScrapeUser_Stop = ((System.Windows.Controls.Button)(target));
            
            #line 101 "..\..\..\..\Pages\PageScraper\UserControlScrapeUser.xaml"
            this.btnMessage_ScrapeUser_Stop.Click += new System.Windows.RoutedEventHandler(this.btnMessage_ScrapeUser_Stop_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.Unfollower_Progess = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 9:
            this.dtGrdScrapeUser_ScrapeUser_AccountsReport = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 10:
            
            #line 131 "..\..\..\..\Pages\PageScraper\UserControlScrapeUser.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.RefreshAccountreport_ScrapeUser_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 132 "..\..\..\..\Pages\PageScraper\UserControlScrapeUser.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.DeleteAccountModule_ScrapeUser_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 133 "..\..\..\..\Pages\PageScraper\UserControlScrapeUser.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ExportScrapeUser_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
