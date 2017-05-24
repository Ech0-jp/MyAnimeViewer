﻿#pragma checksum "..\..\..\Windows\MainWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9D8944B1E54628B5758905AB860B8A50"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MahApps.Metro.Controls;
using MyAnimeViewer.Errors;
using MyAnimeViewer.MyAnimeList.API;
using MyAnimeViewer.Windows;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
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


namespace MyAnimeViewer.Windows {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 41 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MahApps.Metro.Controls.Flyout fo_Settings;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MahApps.Metro.Controls.Flyout fo_Errors;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MahApps.Metro.Controls.Flyout fo_EditListItem;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tb_title;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MahApps.Metro.Controls.SplitButton sb_Status;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tb_epWatched;
        
        #line default
        #line hidden
        
        
        #line 81 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tb_totalEp;
        
        #line default
        #line hidden
        
        
        #line 85 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MahApps.Metro.Controls.SplitButton sb_Score;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MahApps.Metro.Controls.SplitButton sb_StartDateMonth;
        
        #line default
        #line hidden
        
        
        #line 93 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MahApps.Metro.Controls.SplitButton sb_StartDateDay;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MahApps.Metro.Controls.SplitButton sb_StartDateYear;
        
        #line default
        #line hidden
        
        
        #line 98 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_InsertStartDate;
        
        #line default
        #line hidden
        
        
        #line 106 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cb_StartDateUnkown;
        
        #line default
        #line hidden
        
        
        #line 112 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MahApps.Metro.Controls.SplitButton sb_FinishDateMonth;
        
        #line default
        #line hidden
        
        
        #line 115 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MahApps.Metro.Controls.SplitButton sb_FinishDateDay;
        
        #line default
        #line hidden
        
        
        #line 118 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MahApps.Metro.Controls.SplitButton sb_FinishDateYear;
        
        #line default
        #line hidden
        
        
        #line 120 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_InsertFinishDate;
        
        #line default
        #line hidden
        
        
        #line 128 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cb_FinishDateUnkown;
        
        #line default
        #line hidden
        
        
        #line 148 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Menu;
        
        #line default
        #line hidden
        
        
        #line 266 "..\..\..\Windows\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MahApps.Metro.Controls.TransitioningContentControl tContent;
        
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
            System.Uri resourceLocater = new System.Uri("/MyAnimeViewer;component/windows/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Windows\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            
            #line 10 "..\..\..\Windows\MainWindow.xaml"
            ((MyAnimeViewer.Windows.MainWindow)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.fo_Settings = ((MahApps.Metro.Controls.Flyout)(target));
            return;
            case 3:
            this.fo_Errors = ((MahApps.Metro.Controls.Flyout)(target));
            return;
            case 4:
            this.fo_EditListItem = ((MahApps.Metro.Controls.Flyout)(target));
            return;
            case 5:
            this.tb_title = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.sb_Status = ((MahApps.Metro.Controls.SplitButton)(target));
            return;
            case 7:
            this.tb_epWatched = ((System.Windows.Controls.TextBox)(target));
            
            #line 80 "..\..\..\Windows\MainWindow.xaml"
            this.tb_epWatched.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.tb_epWatched_PreviewTextInput);
            
            #line default
            #line hidden
            
            #line 80 "..\..\..\Windows\MainWindow.xaml"
            this.tb_epWatched.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.tb_epWatched_TextChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.tb_totalEp = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 9:
            this.sb_Score = ((MahApps.Metro.Controls.SplitButton)(target));
            return;
            case 10:
            this.sb_StartDateMonth = ((MahApps.Metro.Controls.SplitButton)(target));
            return;
            case 11:
            this.sb_StartDateDay = ((MahApps.Metro.Controls.SplitButton)(target));
            return;
            case 12:
            this.sb_StartDateYear = ((MahApps.Metro.Controls.SplitButton)(target));
            return;
            case 13:
            this.btn_InsertStartDate = ((System.Windows.Controls.Button)(target));
            
            #line 98 "..\..\..\Windows\MainWindow.xaml"
            this.btn_InsertStartDate.Click += new System.Windows.RoutedEventHandler(this.InsertToday_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            this.cb_StartDateUnkown = ((System.Windows.Controls.CheckBox)(target));
            
            #line 106 "..\..\..\Windows\MainWindow.xaml"
            this.cb_StartDateUnkown.Checked += new System.Windows.RoutedEventHandler(this.DateUnkown_Changed);
            
            #line default
            #line hidden
            
            #line 106 "..\..\..\Windows\MainWindow.xaml"
            this.cb_StartDateUnkown.Unchecked += new System.Windows.RoutedEventHandler(this.DateUnkown_Changed);
            
            #line default
            #line hidden
            return;
            case 15:
            this.sb_FinishDateMonth = ((MahApps.Metro.Controls.SplitButton)(target));
            return;
            case 16:
            this.sb_FinishDateDay = ((MahApps.Metro.Controls.SplitButton)(target));
            return;
            case 17:
            this.sb_FinishDateYear = ((MahApps.Metro.Controls.SplitButton)(target));
            return;
            case 18:
            this.btn_InsertFinishDate = ((System.Windows.Controls.Button)(target));
            
            #line 120 "..\..\..\Windows\MainWindow.xaml"
            this.btn_InsertFinishDate.Click += new System.Windows.RoutedEventHandler(this.InsertToday_Click);
            
            #line default
            #line hidden
            return;
            case 19:
            this.cb_FinishDateUnkown = ((System.Windows.Controls.CheckBox)(target));
            
            #line 128 "..\..\..\Windows\MainWindow.xaml"
            this.cb_FinishDateUnkown.Checked += new System.Windows.RoutedEventHandler(this.DateUnkown_Changed);
            
            #line default
            #line hidden
            
            #line 128 "..\..\..\Windows\MainWindow.xaml"
            this.cb_FinishDateUnkown.Unchecked += new System.Windows.RoutedEventHandler(this.DateUnkown_Changed);
            
            #line default
            #line hidden
            return;
            case 20:
            
            #line 132 "..\..\..\Windows\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.EditListItemSubmit_Click);
            
            #line default
            #line hidden
            return;
            case 21:
            
            #line 133 "..\..\..\Windows\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.EditListItemCancel_Click);
            
            #line default
            #line hidden
            return;
            case 22:
            
            #line 140 "..\..\..\Windows\MainWindow.xaml"
            ((System.Windows.Controls.ScrollViewer)(target)).PreviewMouseWheel += new System.Windows.Input.MouseWheelEventHandler(this.ScrollViewer_PreviewMouseWheel);
            
            #line default
            #line hidden
            return;
            case 23:
            this.btn_Menu = ((System.Windows.Controls.Button)(target));
            
            #line 148 "..\..\..\Windows\MainWindow.xaml"
            this.btn_Menu.Loaded += new System.Windows.RoutedEventHandler(this.btn_Menu_Loaded);
            
            #line default
            #line hidden
            return;
            case 28:
            this.tContent = ((MahApps.Metro.Controls.TransitioningContentControl)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 24:
            
            #line 169 "..\..\..\Windows\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btn_MenuItem_Click);
            
            #line default
            #line hidden
            break;
            case 25:
            
            #line 170 "..\..\..\Windows\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btn_MenuItem_Click);
            
            #line default
            #line hidden
            break;
            case 26:
            
            #line 177 "..\..\..\Windows\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btn_MenuItem_Click);
            
            #line default
            #line hidden
            break;
            case 27:
            
            #line 178 "..\..\..\Windows\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btn_MenuItem_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}
