#pragma checksum "..\..\..\View\SignupPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "90F89257128757BEB664FFC2A2C91BDF671ADB3F02C2C0FA8B73451FCDFAB393"
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
using EasyProject.Model;
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
    /// SignupPage
    /// </summary>
    public partial class SignupPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\..\View\SignupPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.DialogHost SignUpDialogHost;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\View\SignupPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox name_TxtBox;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\..\View\SignupPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox deptName_ComboBoxs;
        
        #line default
        #line hidden
        
        
        #line 122 "..\..\..\View\SignupPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox id_TxtBox;
        
        #line default
        #line hidden
        
        
        #line 149 "..\..\..\View\SignupPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox password_PwBox;
        
        #line default
        #line hidden
        
        
        #line 166 "..\..\..\View\SignupPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox rePassword_PwBox;
        
        #line default
        #line hidden
        
        
        #line 179 "..\..\..\View\SignupPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button signUpBtn;
        
        #line default
        #line hidden
        
        
        #line 182 "..\..\..\View\SignupPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button backBtn;
        
        #line default
        #line hidden
        
        
        #line 184 "..\..\..\View\SignupPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button rewriteBtn;
        
        #line default
        #line hidden
        
        
        #line 192 "..\..\..\View\SignupPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.Snackbar Snackbar;
        
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
            System.Uri resourceLocater = new System.Uri("/ezIMP;component/view/signuppage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\View\SignupPage.xaml"
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
            this.SignUpDialogHost = ((MaterialDesignThemes.Wpf.DialogHost)(target));
            return;
            case 2:
            
            #line 39 "..\..\..\View\SignupPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.signUpBtn_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.name_TxtBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.deptName_ComboBoxs = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 5:
            this.id_TxtBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 124 "..\..\..\View\SignupPage.xaml"
            this.id_TxtBox.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.id_TxtBox_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 6:
            this.password_PwBox = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 7:
            this.rePassword_PwBox = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 8:
            this.signUpBtn = ((System.Windows.Controls.Button)(target));
            return;
            case 9:
            this.backBtn = ((System.Windows.Controls.Button)(target));
            return;
            case 10:
            this.rewriteBtn = ((System.Windows.Controls.Button)(target));
            return;
            case 11:
            this.Snackbar = ((MaterialDesignThemes.Wpf.Snackbar)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

