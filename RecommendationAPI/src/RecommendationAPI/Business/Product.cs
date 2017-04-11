using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationAPI.Business {
    public class Product {

        private int _productUID;
        private string _created;
        private string _description;
        private int _productGroup;

        public int ProductUID
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

        public int ProductGroup
        {
            get
            {
                return _productGroup;
            }

            set
            {
                _productGroup = value;
            }
        }

        public Product(int _productUID, string _created, string _description, int _productGroup) {
            this.ProductUID = _productUID;
            this.Created = _created;
            this.Description = _description;
            this.ProductGroup = _productGroup;
        }
    }
}
