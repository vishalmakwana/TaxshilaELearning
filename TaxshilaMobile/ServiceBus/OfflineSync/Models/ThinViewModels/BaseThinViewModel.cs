using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaxshilaMobile.ServiceBus.OfflineSync.Models.ThinViewModels
{
    public interface IBaseViewModel
    {
        int LocalId { get; set; }
        DateTime? CreatedAt { get; set; }
        DateTime? ModifiedAt { get; set; }
        bool IsActive { get; set; }
    }
    /// <summary>
    /// Only for Observable Collections within a view model. 
    /// </summary>
    /// 

    public class BaseThinViewModel : BaseViewModel, IViewModelExtensions, IBaseViewModel
    {

        private int _localId;
        [ModelProperty(nameof(ModelBase.LocalId))]
        public int LocalId
        {
            get => _localId;
            set { SetProperty(ref _localId, value); }
        }
        private int _serverId;
        [ModelProperty(nameof(ModelBase.ServerId))]
        public int ServerId
        {
            get => _serverId;
            set { SetProperty(ref _serverId, value); }
        }
        private string _modifiedBy;
        [ModelProperty(nameof(ModelBase.ModifiedBy))]
        public string ModifiedBy
        {
            get => _modifiedBy;
            set { SetProperty(ref _modifiedBy, value); }
        }
        private string _modifiedByFullname;
        [ModelProperty(nameof(ModelBase.ModifiedByFullName))]
        public string ModifiedByFullName
        {
            get => _modifiedByFullname;
            set { SetProperty(ref _modifiedByFullname, value); }
        }
        private DateTime? _modifiedAt;
        [ModelProperty(nameof(ModelBase.ModifiedAt))]
        public DateTime? ModifiedAt
        {
            get => _modifiedAt;
            set { SetProperty(ref _modifiedAt, value); }
        }
        private string _createdBy;
        [ModelProperty(nameof(ModelBase.CreatedBy))]
        public string CreatedBy
        {
            get => _createdBy;
            set { SetProperty(ref _createdBy, value); }
        }
        private DateTime? _createdAt;
        [ModelProperty(nameof(ModelBase.CreatedAt))]
        public DateTime? CreatedAt
        {
            get => _createdAt;
            set { SetProperty(ref _createdAt, value); }
        }
        private string _createdByFullname;
        [ModelProperty(nameof(ModelBase.CreatedByFullName))]
        public string CreatedByFullName
        {
            get => _createdByFullname;
            set { SetProperty(ref _createdByFullname, value); }
        }
        private Operation _operation;
        [ModelProperty(nameof(ModelBase.Operation))]
        public Operation Operation
        {
            get => _operation;
            set { SetProperty(ref _operation, value); }
        }
        private bool _isActive;
        [ModelProperty(nameof(ModelBase.IsActive))]
        public bool IsActive
        {
            get => _isActive;
            set { SetProperty(ref _isActive, value); }
        }

        private bool _isDelete;
        [ModelProperty(nameof(ModelBase.IsDelete))]
        public bool IsDelete
        {
            get => _isDelete;
            set { SetProperty(ref _isDelete, value); }
        }
    }

    public class CascadeRelatedItem
    {
        public int AssocTypeId { get; set; }
        public string ObjectId { get; set; }
        public string ControlValue { get; set; }
        public string Name { get => ControlValue; set { ControlValue = value; } }
    }
}
