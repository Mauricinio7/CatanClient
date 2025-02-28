﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CatanClient.ChatService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GameDto", Namespace="http://schemas.datacontract.org/2004/07/CatanService.DataTransferObject")]
    [System.SerializableAttribute()]
    public partial class GameDto : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AccessKeyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdAdminGameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> MaxNumberPlayersField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
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
        public string AccessKey {
            get {
                return this.AccessKeyField;
            }
            set {
                if ((object.ReferenceEquals(this.AccessKeyField, value) != true)) {
                    this.AccessKeyField = value;
                    this.RaisePropertyChanged("AccessKey");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int IdAdminGame {
            get {
                return this.IdAdminGameField;
            }
            set {
                if ((this.IdAdminGameField.Equals(value) != true)) {
                    this.IdAdminGameField = value;
                    this.RaisePropertyChanged("IdAdminGame");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> MaxNumberPlayers {
            get {
                return this.MaxNumberPlayersField;
            }
            set {
                if ((this.MaxNumberPlayersField.Equals(value) != true)) {
                    this.MaxNumberPlayersField = value;
                    this.RaisePropertyChanged("MaxNumberPlayers");
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
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ChatService.IChatServiceEndpoint", CallbackContract=typeof(CatanClient.ChatService.IChatServiceEndpointCallback))]
    public interface IChatServiceEndpoint {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatServiceEndpoint/JoinChatAsync")]
        void JoinChatAsync(CatanClient.ChatService.GameDto gameClientDto, string namePlayer);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatServiceEndpoint/JoinChatAsync")]
        System.Threading.Tasks.Task JoinChatAsyncAsync(CatanClient.ChatService.GameDto gameClientDto, string namePlayer);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatServiceEndpoint/SendMessageToChatAsync")]
        void SendMessageToChatAsync(CatanClient.ChatService.GameDto gameClientDto, string namePlayer, string message);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatServiceEndpoint/SendMessageToChatAsync")]
        System.Threading.Tasks.Task SendMessageToChatAsyncAsync(CatanClient.ChatService.GameDto gameClientDto, string namePlayer, string message);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatServiceEndpoint/LeaveChatAsync")]
        void LeaveChatAsync(CatanClient.ChatService.GameDto gameClientDto, string namePlayer);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatServiceEndpoint/LeaveChatAsync")]
        System.Threading.Tasks.Task LeaveChatAsyncAsync(CatanClient.ChatService.GameDto gameClientDto, string namePlayer);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IChatServiceEndpointCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatServiceEndpoint/ReceiveMessage")]
        void ReceiveMessage(string name, string message);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatServiceEndpoint/ReceiveMessageJoinPlayerToChat")]
        void ReceiveMessageJoinPlayerToChat(string name);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatServiceEndpoint/ReceiveMessageLeftPlayerToChat")]
        void ReceiveMessageLeftPlayerToChat(string name);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IChatServiceEndpointChannel : CatanClient.ChatService.IChatServiceEndpoint, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ChatServiceEndpointClient : System.ServiceModel.DuplexClientBase<CatanClient.ChatService.IChatServiceEndpoint>, CatanClient.ChatService.IChatServiceEndpoint {
        
        public ChatServiceEndpointClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public ChatServiceEndpointClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public ChatServiceEndpointClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ChatServiceEndpointClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ChatServiceEndpointClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public void JoinChatAsync(CatanClient.ChatService.GameDto gameClientDto, string namePlayer) {
            base.Channel.JoinChatAsync(gameClientDto, namePlayer);
        }
        
        public System.Threading.Tasks.Task JoinChatAsyncAsync(CatanClient.ChatService.GameDto gameClientDto, string namePlayer) {
            return base.Channel.JoinChatAsyncAsync(gameClientDto, namePlayer);
        }
        
        public void SendMessageToChatAsync(CatanClient.ChatService.GameDto gameClientDto, string namePlayer, string message) {
            base.Channel.SendMessageToChatAsync(gameClientDto, namePlayer, message);
        }
        
        public System.Threading.Tasks.Task SendMessageToChatAsyncAsync(CatanClient.ChatService.GameDto gameClientDto, string namePlayer, string message) {
            return base.Channel.SendMessageToChatAsyncAsync(gameClientDto, namePlayer, message);
        }
        
        public void LeaveChatAsync(CatanClient.ChatService.GameDto gameClientDto, string namePlayer) {
            base.Channel.LeaveChatAsync(gameClientDto, namePlayer);
        }
        
        public System.Threading.Tasks.Task LeaveChatAsyncAsync(CatanClient.ChatService.GameDto gameClientDto, string namePlayer) {
            return base.Channel.LeaveChatAsyncAsync(gameClientDto, namePlayer);
        }
    }
}
