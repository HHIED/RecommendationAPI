using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationAPI.Business
{
    public class Visitor
    {
        private string _UID;
        private string _ProfileUID;
        private string _CustomerUID;

        public string UID
        {
            get
            {
                return _UID;
            }

            set
            {
                _UID = value;
            }
        }

        public string ProfileUID
        {
            get
            {
                return _ProfileUID;
            }

            set
            {
                _ProfileUID = value;
            }
        }

        public string CustomerUID
        {
            get
            {
                return _CustomerUID;
            }

            set
            {
                _CustomerUID = value;
            }
        }

        public Visitor(string uID, string profileUID, string customerUID) {
            _UID = uID;
            _ProfileUID = profileUID;
            _CustomerUID = customerUID;
        }


    }
}
