using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace WPPod.Models
{
    public class Order : Inkton.Nester.Cloud.ManagedEntity
    {
        private long? _id = null;
        private User _user;
        private DateTime _visitDate;
        private ObservableCollection<OrderItem> _orderItems;

        public Order()
            : base("order")
        {
            _orderItems = new ObservableCollection<OrderItem>();
        }

        public override string Key
        {
            get { return _id.ToString(); }
        }

        override public string Collection
        {
            get
            {
                if (_user != null)
                {
                    return _user.CollectionKey + base.Collection;
                }
                else
                {
                    return base.Collection;
                }
            }
        }

        override public string CollectionKey
        {
            get
            {
                if (_user != null)
                {
                    return _user.CollectionKey + base.CollectionKey;
                }
                else
                {
                    return base.CollectionKey;
                }
            }
        }

        [JsonProperty("id")]
        public long? Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        [JsonProperty("user_id")]
        public long? UserId
        {
            get { return _user.Id; }
        }

        [JsonIgnore]
        public User User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        [JsonProperty("visit_date")]
        public DateTime VisitDate
        {
            get { return _visitDate; }
            set { SetProperty(ref _visitDate, value); }
        }

        [JsonIgnore]
        public ObservableCollection<OrderItem> Items
        {
            get { return _orderItems; }
            set { SetProperty(ref _orderItems, value); }
        }
    }
}