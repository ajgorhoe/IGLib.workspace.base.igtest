﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1434
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 2.0.50727.1434.
// 
#pragma warning disable 1591

namespace TestWS_GUI.EFA_WS {
    using System.Diagnostics;
    using System.Web.Services;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System;
    using System.Xml.Serialization;
    using System.Data;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ServiceSoap", Namespace="http://www.posta.si/EFA")]
    public partial class Service : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback PosljiPaketOperationCompleted;
        
        private System.Threading.SendOrPostCallback PosljiPaketZipOperationCompleted;
        
        private System.Threading.SendOrPostCallback StevilkaSejeOperationCompleted;
        
        private System.Threading.SendOrPostCallback PaketStatusOperationCompleted;
        
        private System.Threading.SendOrPostCallback PaketShemaOperationCompleted;
        
        private System.Threading.SendOrPostCallback PaketStatusShemaOperationCompleted;
        
        private System.Threading.SendOrPostCallback PaketTipZalogaVrednostiOperationCompleted;
        
        private System.Threading.SendOrPostCallback PaketStatusZalogaVrednostiOperationCompleted;
        
        private System.Threading.SendOrPostCallback PodrocjaZalogaVrednostiOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Service() {
            this.Url = global::TestWS_GUI.Properties.Settings.Default.TestWS_EFA_WS_Service;
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
        public event PosljiPaketCompletedEventHandler PosljiPaketCompleted;
        
        /// <remarks/>
        public event PosljiPaketZipCompletedEventHandler PosljiPaketZipCompleted;
        
        /// <remarks/>
        public event StevilkaSejeCompletedEventHandler StevilkaSejeCompleted;
        
        /// <remarks/>
        public event PaketStatusCompletedEventHandler PaketStatusCompleted;
        
        /// <remarks/>
        public event PaketShemaCompletedEventHandler PaketShemaCompleted;
        
        /// <remarks/>
        public event PaketStatusShemaCompletedEventHandler PaketStatusShemaCompleted;
        
        /// <remarks/>
        public event PaketTipZalogaVrednostiCompletedEventHandler PaketTipZalogaVrednostiCompleted;
        
        /// <remarks/>
        public event PaketStatusZalogaVrednostiCompletedEventHandler PaketStatusZalogaVrednostiCompleted;
        
        /// <remarks/>
        public event PodrocjaZalogaVrednostiCompletedEventHandler PodrocjaZalogaVrednostiCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.posta.si/EFA/PosljiPaket", RequestNamespace="http://www.posta.si/EFA", ResponseNamespace="http://www.posta.si/EFA", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string PosljiPaket(string paketXml) {
            object[] results = this.Invoke("PosljiPaket", new object[] {
                        paketXml});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void PosljiPaketAsync(string paketXml) {
            this.PosljiPaketAsync(paketXml, null);
        }
        
        /// <remarks/>
        public void PosljiPaketAsync(string paketXml, object userState) {
            if ((this.PosljiPaketOperationCompleted == null)) {
                this.PosljiPaketOperationCompleted = new System.Threading.SendOrPostCallback(this.OnPosljiPaketOperationCompleted);
            }
            this.InvokeAsync("PosljiPaket", new object[] {
                        paketXml}, this.PosljiPaketOperationCompleted, userState);
        }
        
        private void OnPosljiPaketOperationCompleted(object arg) {
            if ((this.PosljiPaketCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.PosljiPaketCompleted(this, new PosljiPaketCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.posta.si/EFA/PosljiPaketZip", RequestNamespace="http://www.posta.si/EFA", ResponseNamespace="http://www.posta.si/EFA", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string PosljiPaketZip([System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] paketZip) {
            object[] results = this.Invoke("PosljiPaketZip", new object[] {
                        paketZip});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void PosljiPaketZipAsync(byte[] paketZip) {
            this.PosljiPaketZipAsync(paketZip, null);
        }
        
        /// <remarks/>
        public void PosljiPaketZipAsync(byte[] paketZip, object userState) {
            if ((this.PosljiPaketZipOperationCompleted == null)) {
                this.PosljiPaketZipOperationCompleted = new System.Threading.SendOrPostCallback(this.OnPosljiPaketZipOperationCompleted);
            }
            this.InvokeAsync("PosljiPaketZip", new object[] {
                        paketZip}, this.PosljiPaketZipOperationCompleted, userState);
        }
        
        private void OnPosljiPaketZipOperationCompleted(object arg) {
            if ((this.PosljiPaketZipCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.PosljiPaketZipCompleted(this, new PosljiPaketZipCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.posta.si/EFA/StevilkaSeje", RequestNamespace="http://www.posta.si/EFA", ResponseNamespace="http://www.posta.si/EFA", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int StevilkaSeje() {
            object[] results = this.Invoke("StevilkaSeje", new object[0]);
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void StevilkaSejeAsync() {
            this.StevilkaSejeAsync(null);
        }
        
        /// <remarks/>
        public void StevilkaSejeAsync(object userState) {
            if ((this.StevilkaSejeOperationCompleted == null)) {
                this.StevilkaSejeOperationCompleted = new System.Threading.SendOrPostCallback(this.OnStevilkaSejeOperationCompleted);
            }
            this.InvokeAsync("StevilkaSeje", new object[0], this.StevilkaSejeOperationCompleted, userState);
        }
        
        private void OnStevilkaSejeOperationCompleted(object arg) {
            if ((this.StevilkaSejeCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.StevilkaSejeCompleted(this, new StevilkaSejeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.posta.si/EFA/PaketStatus", RequestNamespace="http://www.posta.si/EFA", ResponseNamespace="http://www.posta.si/EFA", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string PaketStatus(decimal paketid) {
            object[] results = this.Invoke("PaketStatus", new object[] {
                        paketid});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void PaketStatusAsync(decimal paketid) {
            this.PaketStatusAsync(paketid, null);
        }
        
        /// <remarks/>
        public void PaketStatusAsync(decimal paketid, object userState) {
            if ((this.PaketStatusOperationCompleted == null)) {
                this.PaketStatusOperationCompleted = new System.Threading.SendOrPostCallback(this.OnPaketStatusOperationCompleted);
            }
            this.InvokeAsync("PaketStatus", new object[] {
                        paketid}, this.PaketStatusOperationCompleted, userState);
        }
        
        private void OnPaketStatusOperationCompleted(object arg) {
            if ((this.PaketStatusCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.PaketStatusCompleted(this, new PaketStatusCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.posta.si/EFA/PaketShema", RequestNamespace="http://www.posta.si/EFA", ResponseNamespace="http://www.posta.si/EFA", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] PaketShema() {
            object[] results = this.Invoke("PaketShema", new object[0]);
            return ((byte[])(results[0]));
        }
        
        /// <remarks/>
        public void PaketShemaAsync() {
            this.PaketShemaAsync(null);
        }
        
        /// <remarks/>
        public void PaketShemaAsync(object userState) {
            if ((this.PaketShemaOperationCompleted == null)) {
                this.PaketShemaOperationCompleted = new System.Threading.SendOrPostCallback(this.OnPaketShemaOperationCompleted);
            }
            this.InvokeAsync("PaketShema", new object[0], this.PaketShemaOperationCompleted, userState);
        }
        
        private void OnPaketShemaOperationCompleted(object arg) {
            if ((this.PaketShemaCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.PaketShemaCompleted(this, new PaketShemaCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.posta.si/EFA/PaketStatusShema", RequestNamespace="http://www.posta.si/EFA", ResponseNamespace="http://www.posta.si/EFA", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] PaketStatusShema() {
            object[] results = this.Invoke("PaketStatusShema", new object[0]);
            return ((byte[])(results[0]));
        }
        
        /// <remarks/>
        public void PaketStatusShemaAsync() {
            this.PaketStatusShemaAsync(null);
        }
        
        /// <remarks/>
        public void PaketStatusShemaAsync(object userState) {
            if ((this.PaketStatusShemaOperationCompleted == null)) {
                this.PaketStatusShemaOperationCompleted = new System.Threading.SendOrPostCallback(this.OnPaketStatusShemaOperationCompleted);
            }
            this.InvokeAsync("PaketStatusShema", new object[0], this.PaketStatusShemaOperationCompleted, userState);
        }
        
        private void OnPaketStatusShemaOperationCompleted(object arg) {
            if ((this.PaketStatusShemaCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.PaketStatusShemaCompleted(this, new PaketStatusShemaCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.posta.si/EFA/PaketTipZalogaVrednosti", RequestNamespace="http://www.posta.si/EFA", ResponseNamespace="http://www.posta.si/EFA", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataSet PaketTipZalogaVrednosti() {
            object[] results = this.Invoke("PaketTipZalogaVrednosti", new object[0]);
            return ((System.Data.DataSet)(results[0]));
        }
        
        /// <remarks/>
        public void PaketTipZalogaVrednostiAsync() {
            this.PaketTipZalogaVrednostiAsync(null);
        }
        
        /// <remarks/>
        public void PaketTipZalogaVrednostiAsync(object userState) {
            if ((this.PaketTipZalogaVrednostiOperationCompleted == null)) {
                this.PaketTipZalogaVrednostiOperationCompleted = new System.Threading.SendOrPostCallback(this.OnPaketTipZalogaVrednostiOperationCompleted);
            }
            this.InvokeAsync("PaketTipZalogaVrednosti", new object[0], this.PaketTipZalogaVrednostiOperationCompleted, userState);
        }
        
        private void OnPaketTipZalogaVrednostiOperationCompleted(object arg) {
            if ((this.PaketTipZalogaVrednostiCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.PaketTipZalogaVrednostiCompleted(this, new PaketTipZalogaVrednostiCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.posta.si/EFA/PaketStatusZalogaVrednosti", RequestNamespace="http://www.posta.si/EFA", ResponseNamespace="http://www.posta.si/EFA", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataSet PaketStatusZalogaVrednosti() {
            object[] results = this.Invoke("PaketStatusZalogaVrednosti", new object[0]);
            return ((System.Data.DataSet)(results[0]));
        }
        
        /// <remarks/>
        public void PaketStatusZalogaVrednostiAsync() {
            this.PaketStatusZalogaVrednostiAsync(null);
        }
        
        /// <remarks/>
        public void PaketStatusZalogaVrednostiAsync(object userState) {
            if ((this.PaketStatusZalogaVrednostiOperationCompleted == null)) {
                this.PaketStatusZalogaVrednostiOperationCompleted = new System.Threading.SendOrPostCallback(this.OnPaketStatusZalogaVrednostiOperationCompleted);
            }
            this.InvokeAsync("PaketStatusZalogaVrednosti", new object[0], this.PaketStatusZalogaVrednostiOperationCompleted, userState);
        }
        
        private void OnPaketStatusZalogaVrednostiOperationCompleted(object arg) {
            if ((this.PaketStatusZalogaVrednostiCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.PaketStatusZalogaVrednostiCompleted(this, new PaketStatusZalogaVrednostiCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.posta.si/EFA/PodrocjaZalogaVrednosti", RequestNamespace="http://www.posta.si/EFA", ResponseNamespace="http://www.posta.si/EFA", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataSet PodrocjaZalogaVrednosti() {
            object[] results = this.Invoke("PodrocjaZalogaVrednosti", new object[0]);
            return ((System.Data.DataSet)(results[0]));
        }
        
        /// <remarks/>
        public void PodrocjaZalogaVrednostiAsync() {
            this.PodrocjaZalogaVrednostiAsync(null);
        }
        
        /// <remarks/>
        public void PodrocjaZalogaVrednostiAsync(object userState) {
            if ((this.PodrocjaZalogaVrednostiOperationCompleted == null)) {
                this.PodrocjaZalogaVrednostiOperationCompleted = new System.Threading.SendOrPostCallback(this.OnPodrocjaZalogaVrednostiOperationCompleted);
            }
            this.InvokeAsync("PodrocjaZalogaVrednosti", new object[0], this.PodrocjaZalogaVrednostiOperationCompleted, userState);
        }
        
        private void OnPodrocjaZalogaVrednostiOperationCompleted(object arg) {
            if ((this.PodrocjaZalogaVrednostiCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.PodrocjaZalogaVrednostiCompleted(this, new PodrocjaZalogaVrednostiCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    public delegate void PosljiPaketCompletedEventHandler(object sender, PosljiPaketCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PosljiPaketCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal PosljiPaketCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    public delegate void PosljiPaketZipCompletedEventHandler(object sender, PosljiPaketZipCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PosljiPaketZipCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal PosljiPaketZipCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    public delegate void StevilkaSejeCompletedEventHandler(object sender, StevilkaSejeCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class StevilkaSejeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal StevilkaSejeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    public delegate void PaketStatusCompletedEventHandler(object sender, PaketStatusCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PaketStatusCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal PaketStatusCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    public delegate void PaketShemaCompletedEventHandler(object sender, PaketShemaCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PaketShemaCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal PaketShemaCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public byte[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((byte[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    public delegate void PaketStatusShemaCompletedEventHandler(object sender, PaketStatusShemaCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PaketStatusShemaCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal PaketStatusShemaCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public byte[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((byte[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    public delegate void PaketTipZalogaVrednostiCompletedEventHandler(object sender, PaketTipZalogaVrednostiCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PaketTipZalogaVrednostiCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal PaketTipZalogaVrednostiCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Data.DataSet Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Data.DataSet)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    public delegate void PaketStatusZalogaVrednostiCompletedEventHandler(object sender, PaketStatusZalogaVrednostiCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PaketStatusZalogaVrednostiCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal PaketStatusZalogaVrednostiCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Data.DataSet Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Data.DataSet)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    public delegate void PodrocjaZalogaVrednostiCompletedEventHandler(object sender, PodrocjaZalogaVrednostiCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1434")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PodrocjaZalogaVrednostiCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal PodrocjaZalogaVrednostiCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Data.DataSet Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Data.DataSet)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591