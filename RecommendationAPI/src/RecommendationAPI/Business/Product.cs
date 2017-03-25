using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationAPI.Business
{
    public class Product
    {

        private string _productUID;
        private string _created;
        private string _description;

        public string ProductUID
        {
            get
            {
                return _productUID;
            }

            set
            {
                _productUID = value;
            }
        }

        public string Created
        {
            get
            {
                return _created;
            }

            set
            {
                _created = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
            }
        }

        public Product(string _productUID, string _created, string _description) {
            this.ProductUID = _productUID;
            this.Created = _created;
            this.Description = _description;
        }
    }
}
