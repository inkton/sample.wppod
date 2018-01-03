using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace WPPod.Models
{
    public class Order : Inkton.Nester.Cloud.ManagedEntity
    {
        public Order()
            : base("order")
        {
            _orderItems = new List<OrderItem>();
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

        private long? _id = null;

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

        private User _user;

        [JsonIgnore]
        public User User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        private DateTime _visitDate;

        [JsonProperty("visit_date")]
        public DateTime VisitDate
        {
            get { return _visitDate; }
            set { SetProperty(ref _visitDate, value); }
        }

        private List<OrderItem> _orderItems;

        [JsonProperty("items")]
        public List<OrderItem> Items
        {
            get { return _orderItems; }
            set { SetProperty(ref _orderItems, value); }
        }
    }
}