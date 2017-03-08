using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationAPI.Business
{
    public class Behavior {
        private string _type;
        private string _id;
        private string _timeStamp;

        public string Type
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

        public string TimeStamp
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

        public Behavior(string _type, string _id, string _timeStamp) {
            this.Type = _type;
            this.Id = _id;
            this.TimeStamp = _timeStamp;
        }

    }
}
