﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CatanClient.GameService {
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ProfileDto", Namespace="http://schemas.datacontract.org/2004/07/CatanService.DataTransferObject")]
    [System.SerializableAttribute()]
    public partial class ProfileDto : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> IdField;
        
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PicturePathField;
        
        private string PreferredLanguageField;
        
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
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
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
        public string PicturePath {
            get {
                return this.PicturePathField;
            }
            set {
                if ((object.ReferenceEquals(this.PicturePathField, value) != true)) {
                    this.PicturePathField = value;
                    this.RaisePropertyChanged("PicturePath");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string PreferredLanguage {
            get {
                return this.PreferredLanguageField;
            }
            set {
                if ((object.ReferenceEquals(this.PreferredLanguageField, value) != true)) {
                    this.PreferredLanguageField = value;
                    this.RaisePropertyChanged("PreferredLanguage");
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="OperationResultDto", Namespace="http://schemas.datacontract.org/2004/07/CatanService.DataTransferObject")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CatanClient.GameService.OperationResultGameDto))]
    public partial class OperationResultDto : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsSuccessField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MessageResponseField;
        
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
        public bool IsSuccess {
            get {
                return this.IsSuccessField;
            }
            set {
                if ((this.IsSuccessField.Equals(value) != true)) {
                    this.IsSuccessField = value;
                    this.RaisePropertyChanged("IsSuccess");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MessageResponse {
            get {
                return this.MessageResponseField;
            }
            set {
                if ((object.ReferenceEquals(this.MessageResponseField, value) != true)) {
                    this.MessageResponseField = value;
                    this.RaisePropertyChanged("MessageResponse");
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="OperationResultGameDto", Namespace="http://schemas.datacontract.org/2004/07/CatanService.DataTransferObject")]
    [System.SerializableAttribute()]
    public partial class OperationResultGameDto : CatanClient.GameService.OperationResultDto {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private CatanClient.GameService.GameDto GameDtoField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CatanClient.GameService.GameDto GameDto {
            get {
                return this.GameDtoField;
            }
            set {
                if ((object.ReferenceEquals(this.GameDtoField, value) != true)) {
                    this.GameDtoField = value;
                    this.RaisePropertyChanged("GameDto");
                }
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="GameService.IGameEndPoint")]
    public interface IGameEndPoint {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGameEndPoint/CreateGame", ReplyAction="http://tempuri.org/IGameEndPoint/CreateGameResponse")]
        CatanClient.GameService.OperationResultGameDto CreateGame(CatanClient.GameService.GameDto gameClientDto, CatanClient.GameService.ProfileDto profileClientDto);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGameEndPoint/CreateGame", ReplyAction="http://tempuri.org/IGameEndPoint/CreateGameResponse")]
        System.Threading.Tasks.Task<CatanClient.GameService.OperationResultGameDto> CreateGameAsync(CatanClient.GameService.GameDto gameClientDto, CatanClient.GameService.ProfileDto profileClientDto);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGameEndPoint/FinishGame", ReplyAction="http://tempuri.org/IGameEndPoint/FinishGameResponse")]
        CatanClient.GameService.OperationResultDto FinishGame(CatanClient.GameService.GameDto gameClientDto, CatanClient.GameService.ProfileDto profileClientDto);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGameEndPoint/FinishGame", ReplyAction="http://tempuri.org/IGameEndPoint/FinishGameResponse")]
        System.Threading.Tasks.Task<CatanClient.GameService.OperationResultDto> FinishGameAsync(CatanClient.GameService.GameDto gameClientDto, CatanClient.GameService.ProfileDto profileClientDto);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGameEndPoint/QuitGame", ReplyAction="http://tempuri.org/IGameEndPoint/QuitGameResponse")]
        CatanClient.GameService.OperationResultDto QuitGame(CatanClient.GameService.GameDto gameClientDto, CatanClient.GameService.ProfileDto profileClientDto);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGameEndPoint/QuitGame", ReplyAction="http://tempuri.org/IGameEndPoint/QuitGameResponse")]
        System.Threading.Tasks.Task<CatanClient.GameService.OperationResultDto> QuitGameAsync(CatanClient.GameService.GameDto gameClientDto, CatanClient.GameService.ProfileDto profileClientDto);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGameEndPoint/JoinGame", ReplyAction="http://tempuri.org/IGameEndPoint/JoinGameResponse")]
        CatanClient.GameService.OperationResultGameDto JoinGame(string accessKey, CatanClient.GameService.ProfileDto profileClientDto);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGameEndPoint/JoinGame", ReplyAction="http://tempuri.org/IGameEndPoint/JoinGameResponse")]
        System.Threading.Tasks.Task<CatanClient.GameService.OperationResultGameDto> JoinGameAsync(string accessKey, CatanClient.GameService.ProfileDto profileClientDto);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGameEndPoint/StartGame", ReplyAction="http://tempuri.org/IGameEndPoint/StartGameResponse")]
        void StartGame(CatanClient.GameService.GameDto gameClientDto, CatanClient.GameService.ProfileDto profileClientDto);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGameEndPoint/StartGame", ReplyAction="http://tempuri.org/IGameEndPoint/StartGameResponse")]
        System.Threading.Tasks.Task StartGameAsync(CatanClient.GameService.GameDto gameClientDto, CatanClient.GameService.ProfileDto profileClientDto);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IGameEndPointChannel : CatanClient.GameService.IGameEndPoint, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GameEndPointClient : System.ServiceModel.ClientBase<CatanClient.GameService.IGameEndPoint>, CatanClient.GameService.IGameEndPoint {
        
        public GameEndPointClient() {
        }
        
        public GameEndPointClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public GameEndPointClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GameEndPointClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GameEndPointClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public CatanClient.GameService.OperationResultGameDto CreateGame(CatanClient.GameService.GameDto gameClientDto, CatanClient.GameService.ProfileDto profileClientDto) {
            return base.Channel.CreateGame(gameClientDto, profileClientDto);
        }
        
        public System.Threading.Tasks.Task<CatanClient.GameService.OperationResultGameDto> CreateGameAsync(CatanClient.GameService.GameDto gameClientDto, CatanClient.GameService.ProfileDto profileClientDto) {
            return base.Channel.CreateGameAsync(gameClientDto, profileClientDto);
        }
        
        public CatanClient.GameService.OperationResultDto FinishGame(CatanClient.GameService.GameDto gameClientDto, CatanClient.GameService.ProfileDto profileClientDto) {
            return base.Channel.FinishGame(gameClientDto, profileClientDto);
        }
        
        public System.Threading.Tasks.Task<CatanClient.GameService.OperationResultDto> FinishGameAsync(CatanClient.GameService.GameDto gameClientDto, CatanClient.GameService.ProfileDto profileClientDto) {
            return base.Channel.FinishGameAsync(gameClientDto, profileClientDto);
        }
        
        public CatanClient.GameService.OperationResultDto QuitGame(CatanClient.GameService.GameDto gameClientDto, CatanClient.GameService.ProfileDto profileClientDto) {
            return base.Channel.QuitGame(gameClientDto, profileClientDto);
        }
        
        public System.Threading.Tasks.Task<CatanClient.GameService.OperationResultDto> QuitGameAsync(CatanClient.GameService.GameDto gameClientDto, CatanClient.GameService.ProfileDto profileClientDto) {
            return base.Channel.QuitGameAsync(gameClientDto, profileClientDto);
        }
        
        public CatanClient.GameService.OperationResultGameDto JoinGame(string accessKey, CatanClient.GameService.ProfileDto profileClientDto) {
            return base.Channel.JoinGame(accessKey, profileClientDto);
        }
        
        public System.Threading.Tasks.Task<CatanClient.GameService.OperationResultGameDto> JoinGameAsync(string accessKey, CatanClient.GameService.ProfileDto profileClientDto) {
            return base.Channel.JoinGameAsync(accessKey, profileClientDto);
        }
        
        public void StartGame(CatanClient.GameService.GameDto gameClientDto, CatanClient.GameService.ProfileDto profileClientDto) {
            base.Channel.StartGame(gameClientDto, profileClientDto);
        }
        
        public System.Threading.Tasks.Task StartGameAsync(CatanClient.GameService.GameDto gameClientDto, CatanClient.GameService.ProfileDto profileClientDto) {
            return base.Channel.StartGameAsync(gameClientDto, profileClientDto);
        }
    }
}
