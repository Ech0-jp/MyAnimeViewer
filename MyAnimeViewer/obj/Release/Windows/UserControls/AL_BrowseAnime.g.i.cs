﻿#pragma checksum "..\..\..\..\Windows\UserControls\AL_BrowseAnime.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "617D41D323C2A04C32C1F742B696B4CA"
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
using MyAnimeViewer.Enums.AniList;
using MyAnimeViewer.Utility;
using MyAnimeViewer.Windows.UserControls;
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


namespace MyAnimeViewer.Windows.UserControls {
    
    
    /// <summary>
    /// AL_BrowseAnime
    /// </summary>
    public partial class AL_BrowseAnime : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 39 "..\..\..\..\Windows\UserControls\AL_BrowseAnime.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tb_YearSelectedItem;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\..\Windows\UserControls\AL_BrowseAnime.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_RemoveYear;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\..\Windows\UserControls\AL_BrowseAnime.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView lv_Year;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\..\Windows\UserControls\AL_BrowseAnime.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MahApps.Metro.Controls.SplitButton sb_Season;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\..\Windows\UserControls\AL_BrowseAnime.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MahApps.Metro.Controls.SplitButton sb_Status;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\..\Windows\UserControls\AL_BrowseAnime.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MahApps.Metro.Controls.SplitButton sb_Type;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\..\Windows\UserControls\AL_BrowseAnime.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MahApps.Metro.Controls.SplitButton sb_Sort;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\..\Windows\UserControls\AL_BrowseAnime.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView lv_Genres;
        
        #line default
        #line hidden
        
        
        #line 95 "..\..\..\..\Windows\UserControls\AL_BrowseAnime.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tb_Search;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\..\..\Windows\UserControls\AL_BrowseAnime.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ItemsControl ic_Browse;
        
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
            System.Uri resourceLocater = new System.Uri("/MyAnimeViewer;component/windows/usercontrols/al_browseanime.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Windows\UserControls\AL_BrowseAnime.xaml"
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
            this.tb_YearSelectedItem = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.btn_RemoveYear = ((System.Windows.Controls.Button)(target));
            return;
            case 3:
            this.lv_Year = ((System.Windows.Controls.ListView)(target));
            return;
            case 4:
            this.sb_Season = ((MahApps.Metro.Controls.SplitButton)(target));
            return;
            case 5:
            this.sb_Status = ((MahApps.Metro.Controls.SplitButton)(target));
            return;
            case 6:
            this.sb_Type = ((MahApps.Metro.Controls.SplitButton)(target));
            return;
            case 7:
            this.sb_Sort = ((MahApps.Metro.Controls.SplitButton)(target));
            return;
            case 8:
            this.lv_Genres = ((System.Windows.Controls.ListView)(target));
            return;
            case 9:
            this.tb_Search = ((System.Windows.Controls.TextBox)(target));
            return;
            case 10:
            this.ic_Browse = ((System.Windows.Controls.ItemsControl)(target));
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
            case 11:
            
            #line 109 "..\..\..\..\Windows\UserControls\AL_BrowseAnime.xaml"
            ((System.Windows.Controls.Canvas)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.AnimeItem_MouseDown);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

