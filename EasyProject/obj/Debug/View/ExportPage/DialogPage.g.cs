#pragma checksum "..\..\..\..\View\ExportPage\DialogPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "F01454DC9DD6EB55A8AA1821780ABFDC937B5B755B88835A4934628ABE483AA1"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using EasyProject;
using EasyProject.Util;
using EasyProject.ViewModel;
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
using System.Windows.Interactivity;
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


namespace EasyProject {
    
    
    /// <summary>
    /// DialogPage
    /// </summary>
    public partial class DialogPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 57 "..\..\..\..\View\ExportPage\DialogPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button InventoryRevise_Btn;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\..\View\ExportPage\DialogPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock modify;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\..\..\View\ExportPage\DialogPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Release_Btn;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\..\View\ExportPage\DialogPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock release;
        
        #line default
        #line hidden
        
        
        #line 81 "..\..\..\..\View\ExportPage\DialogPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button goOrder_Btn;
        
        #line default
        #line hidden
        
        
        #line 84 "..\..\..\..\View\ExportPage\DialogPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock order_form;
        
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
            System.Uri resourceLocater = new System.Uri("/ezIMP;component/view/exportpage/dialogpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\ExportPage\DialogPage.xaml"
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
            this.InventoryRevise_Btn = ((System.Windows.Controls.Button)(target));
            
            #line 57 "..\..\..\..\View\ExportPage\DialogPage.xaml"
            this.InventoryRevise_Btn.Click += new System.Windows.RoutedEventHandler(this.InventoryRevise_Btn_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.modify = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.Release_Btn = ((System.Windows.Controls.Button)(target));
            
            #line 69 "..\..\..\..\View\ExportPage\DialogPage.xaml"
            this.Release_Btn.Click += new System.Windows.RoutedEventHandler(this.Release_Btn_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.release = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.goOrder_Btn = ((System.Windows.Controls.Button)(target));
            
            #line 81 "..\..\..\..\View\ExportPage\DialogPage.xaml"
            this.goOrder_Btn.Click += new System.Windows.RoutedEventHandler(this.goOrder_Btn_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.order_form = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

