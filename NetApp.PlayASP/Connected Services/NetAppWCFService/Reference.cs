﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NetApp.PlayASP.NetAppWCFService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Product", Namespace="http://schemas.datacontract.org/2004/07/NetApp.PlayASPAPI.Models")]
    [System.SerializableAttribute()]
    public partial class Product : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime UpdateTimeField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Id {
            get {
                return this.IdField;
            }
            set {
                if ((object.ReferenceEquals(this.IdField, value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime UpdateTime {
            get {
                return this.UpdateTimeField;
            }
            set {
                if ((this.UpdateTimeField.Equals(value) != true)) {
                    this.UpdateTimeField = value;
                    this.RaisePropertyChanged("UpdateTime");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="NetAppWCFService.INetAppWCF")]
    public interface INetAppWCF {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/INetAppWCF/Products", ReplyAction="http://tempuri.org/INetAppWCF/ProductsResponse")]
        NetApp.PlayASP.NetAppWCFService.Product[] Products(int startIndex, int count);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/INetAppWCF/Products", ReplyAction="http://tempuri.org/INetAppWCF/ProductsResponse")]
        System.Threading.Tasks.Task<NetApp.PlayASP.NetAppWCFService.Product[]> ProductsAsync(int startIndex, int count);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/INetAppWCF/UpdateProduct", ReplyAction="http://tempuri.org/INetAppWCF/UpdateProductResponse")]
        void UpdateProduct(NetApp.PlayASP.NetAppWCFService.Product product);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/INetAppWCF/UpdateProduct", ReplyAction="http://tempuri.org/INetAppWCF/UpdateProductResponse")]
        System.Threading.Tasks.Task UpdateProductAsync(NetApp.PlayASP.NetAppWCFService.Product product);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/INetAppWCF/SayHello")]
        void SayHello(string message);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/INetAppWCF/SayHello")]
        System.Threading.Tasks.Task SayHelloAsync(string message);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface INetAppWCFChannel : NetApp.PlayASP.NetAppWCFService.INetAppWCF, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class NetAppWCFClient : System.ServiceModel.ClientBase<NetApp.PlayASP.NetAppWCFService.INetAppWCF>, NetApp.PlayASP.NetAppWCFService.INetAppWCF {
        
        public NetAppWCFClient() {
        }
        
        public NetAppWCFClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public NetAppWCFClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NetAppWCFClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NetAppWCFClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public NetApp.PlayASP.NetAppWCFService.Product[] Products(int startIndex, int count) {
            return base.Channel.Products(startIndex, count);
        }
        
        public System.Threading.Tasks.Task<NetApp.PlayASP.NetAppWCFService.Product[]> ProductsAsync(int startIndex, int count) {
            return base.Channel.ProductsAsync(startIndex, count);
        }
        
        public void UpdateProduct(NetApp.PlayASP.NetAppWCFService.Product product) {
            base.Channel.UpdateProduct(product);
        }
        
        public System.Threading.Tasks.Task UpdateProductAsync(NetApp.PlayASP.NetAppWCFService.Product product) {
            return base.Channel.UpdateProductAsync(product);
        }
        
        public void SayHello(string message) {
            base.Channel.SayHello(message);
        }
        
        public System.Threading.Tasks.Task SayHelloAsync(string message) {
            return base.Channel.SayHelloAsync(message);
        }
    }
}
