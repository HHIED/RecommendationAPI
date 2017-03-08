using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationAPI.Business
{
    public class Visitor
    {
        private string _uID;
        private string _profileUID;
        private string _customerUID;
        private List<Behavior> _behaviors;

        public string UID
        {
            get
            {
                return _uID;
            }

            set
            {
                _uID = value;
            }
        }

        public string ProfileUID
        {
            get
            {
                return _profileUID;
            }

            set
            {
                _profileUID = value;
            }
        }

        public string CustomerUID
        {
            get
            {
                return _customerUID;
            }

            set
            {
                _customerUID = value;
            }
        }

        public List<Behavior> Behaviors
        {
            get
            {
                return _behaviors;
            }

            set
            {
                _behaviors = value;
            }
        }

        public Visitor(string _uID, string _profileUID, string _customerUID, List<Behavior> _behaviors) {
            this.UID = _uID;
            this.ProfileUID = _profileUID;
            this.CustomerUID = _customerUID;
            this.Behaviors = _behaviors;
        }
    }
}
