﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationAPI.Business
{
    public class Behavior {
        public enum BehaviorTypes { ProductView, ProductGroupView, BrandView, VariantView };
        private BehaviorTypes _type;
        private string _id;
        private DateTime _timeStamp;

        public BehaviorTypes Type
        {
            get
            {
                return _type;
            }

            set
            {
                _type = value;
            }
        }

        public string Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public DateTime TimeStamp
        {
            get
            {
                return _timeStamp;
            }

            set
            {
                _timeStamp = value;
            }
        }

        public Behavior(BehaviorTypes _type, string _id, DateTime _timeStamp) {
            this.Type = _type;
            this.Id = _id;
            this.TimeStamp = _timeStamp;
        }

    }
}