﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace WcfService_WebDev_FromTemplate.WSBaseRef {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.81.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="WSBaseSoap", Namespace="http://www2.arnes.si/~ljc3m2/igor/iglib/index.html")]
    public partial class WSBase : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback TestCountOperationCompleted;
        
        private System.Threading.SendOrPostCallback TestServiceOperationCompleted;
        
        private System.Threading.SendOrPostCallback TestServiceArgOperationCompleted;
        
        private System.Threading.SendOrPostCallback TestServiceArgsOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public WSBase() {
            this.Url = global::WcfService_WebDev_FromTemplate.Properties.Settings.Default.WSMyService_WSBaseRef_WSBase;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event TestCountCompletedEventHandler TestCountCompleted;
        
        /// <remarks/>
        public event TestServiceCompletedEventHandler TestServiceCompleted;
        
        /// <remarks/>
        public event TestServiceArgCompletedEventHandler TestServiceArgCompleted;
        
        /// <remarks/>
        public event TestServiceArgsCompletedEventHandler TestServiceArgsCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www2.arnes.si/~ljc3m2/igor/iglib/index.html/TestCount", RequestNamespace="http://www2.arnes.si/~ljc3m2/igor/iglib/index.html", ResponseNamespace="http://www2.arnes.si/~ljc3m2/igor/iglib/index.html", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string TestCount() {
            object[] results = this.Invoke("TestCount", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void TestCountAsync() {
            this.TestCountAsync(null);
        }
        
        /// <remarks/>
        public void TestCountAsync(object userState) {
            if ((this.TestCountOperationCompleted == null)) {
                this.TestCountOperationCompleted = new System.Threading.SendOrPostCallback(this.OnTestCountOperationCompleted);
            }
            this.InvokeAsync("TestCount", new object[0], this.TestCountOperationCompleted, userState);
        }
        
        private void OnTestCountOperationCompleted(object arg) {
            if ((this.TestCountCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.TestCountCompleted(this, new TestCountCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www2.arnes.si/~ljc3m2/igor/iglib/index.html/TestService", RequestNamespace="http://www2.arnes.si/~ljc3m2/igor/iglib/index.html", ResponseNamespace="http://www2.arnes.si/~ljc3m2/igor/iglib/index.html", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string TestService() {
            object[] results = this.Invoke("TestService", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void TestServiceAsync() {
            this.TestServiceAsync(null);
        }
        
        /// <remarks/>
        public void TestServiceAsync(object userState) {
            if ((this.TestServiceOperationCompleted == null)) {
                this.TestServiceOperationCompleted = new System.Threading.SendOrPostCallback(this.OnTestServiceOperationCompleted);
            }
            this.InvokeAsync("TestService", new object[0], this.TestServiceOperationCompleted, userState);
        }
        
        private void OnTestServiceOperationCompleted(object arg) {
            if ((this.TestServiceCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.TestServiceCompleted(this, new TestServiceCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www2.arnes.si/~ljc3m2/igor/iglib/index.html/TestServiceArg", RequestNamespace="http://www2.arnes.si/~ljc3m2/igor/iglib/index.html", ResponseNamespace="http://www2.arnes.si/~ljc3m2/igor/iglib/index.html", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string TestServiceArg(string commandlineArguments) {
            object[] results = this.Invoke("TestServiceArg", new object[] {
                        commandlineArguments});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void TestServiceArgAsync(string commandlineArguments) {
            this.TestServiceArgAsync(commandlineArguments, null);
        }
        
        /// <remarks/>
        public void TestServiceArgAsync(string commandlineArguments, object userState) {
            if ((this.TestServiceArgOperationCompleted == null)) {
                this.TestServiceArgOperationCompleted = new System.Threading.SendOrPostCallback(this.OnTestServiceArgOperationCompleted);
            }
            this.InvokeAsync("TestServiceArg", new object[] {
                        commandlineArguments}, this.TestServiceArgOperationCompleted, userState);
        }
        
        private void OnTestServiceArgOperationCompleted(object arg) {
            if ((this.TestServiceArgCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.TestServiceArgCompleted(this, new TestServiceArgCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www2.arnes.si/~ljc3m2/igor/iglib/index.html/TestServiceArgs", RequestNamespace="http://www2.arnes.si/~ljc3m2/igor/iglib/index.html", ResponseNamespace="http://www2.arnes.si/~ljc3m2/igor/iglib/index.html", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string TestServiceArgs(string[] commandlineArguments) {
            object[] results = this.Invoke("TestServiceArgs", new object[] {
                        commandlineArguments});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void TestServiceArgsAsync(string[] commandlineArguments) {
            this.TestServiceArgsAsync(commandlineArguments, null);
        }
        
        /// <remarks/>
        public void TestServiceArgsAsync(string[] commandlineArguments, object userState) {
            if ((this.TestServiceArgsOperationCompleted == null)) {
                this.TestServiceArgsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnTestServiceArgsOperationCompleted);
            }
            this.InvokeAsync("TestServiceArgs", new object[] {
                        commandlineArguments}, this.TestServiceArgsOperationCompleted, userState);
        }
        
        private void OnTestServiceArgsOperationCompleted(object arg) {
            if ((this.TestServiceArgsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.TestServiceArgsCompleted(this, new TestServiceArgsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.81.0")]
    public delegate void TestCountCompletedEventHandler(object sender, TestCountCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.81.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TestCountCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal TestCountCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.81.0")]
    public delegate void TestServiceCompletedEventHandler(object sender, TestServiceCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.81.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TestServiceCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal TestServiceCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.81.0")]
    public delegate void TestServiceArgCompletedEventHandler(object sender, TestServiceArgCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.81.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TestServiceArgCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal TestServiceArgCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.81.0")]
    public delegate void TestServiceArgsCompletedEventHandler(object sender, TestServiceArgsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.81.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class TestServiceArgsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal TestServiceArgsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591