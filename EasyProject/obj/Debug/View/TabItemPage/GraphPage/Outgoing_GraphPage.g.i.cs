﻿#pragma checksum "..\..\..\..\..\View\TabItemPage\GraphPage\Outgoing_GraphPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "9867C36C9896A5EE2A1B2D65BDE7B277C8444D0A3CC3AC9166ED2C779C66EC7E"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using EasyProject.View.TabItemPage.GraphPage;
using LiveCharts.Wpf;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
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


namespace EasyProject.View.TabItemPage.GraphPage {
    
    
    /// <summary>
    /// Outgoing_GraphPage
    /// </summary>
    public partial class Outgoing_GraphPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 35 "..\..\..\..\..\View\TabItemPage\GraphPage\Outgoing_GraphPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RightBtn;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\..\..\View\TabItemPage\GraphPage\Outgoing_GraphPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal LiveCharts.Wpf.CartesianChart release_type_chart;
        
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
            System.Uri resourceLocater = new System.Uri("/ezIMP;component/view/tabitempage/graphpage/outgoing_graphpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\View\TabItemPage\GraphPage\Outgoing_GraphPage.xaml"
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
            this.RightBtn = ((System.Windows.Controls.Button)(target));
            
            #line 36 "..\..\..\..\..\View\TabItemPage\GraphPage\Outgoing_GraphPage.xaml"
            this.RightBtn.Click += new System.Windows.RoutedEventHandler(this.RightBtn_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.release_type_chart = ((LiveCharts.Wpf.CartesianChart)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
