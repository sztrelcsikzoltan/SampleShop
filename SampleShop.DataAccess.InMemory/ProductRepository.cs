using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using SampleShop.Core;
using SampleShop.Core.Models;
using System.Net.Http;

namespace SampleShop.DataAccess.InMemory
{
    public class ProductRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<Product> products;

        public ProductRepository()
        {
            products = cache["products"] as List<Product>;
            if (products == null)
            {
                products = new List<Product>();
            }
        }

        public void Commit()
        {
            cache["products"] = products;
        }

        // CRUD methods
        public IQueryable<Product> GetProducts()
        {
            return products.AsQueryable();
        }
        
        public Product Get(string id)
        {
            Product product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                throw new Exception("Product not found!");
            }

            return product;
        }
                        
        public void Insert(Product product)
        {
            products.Add(product);
        }
        
        public void Update(Product product, string id)
        {
            Product productToUpdate = products.FirstOrDefault(p => p.Id == id);

            if (productToUpdate == null)
            {
                throw new Exception("Product not found!");
            }

            productToUpdate = product;
        }

        public void Delete(string id)
        {
            Product product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                throw new Exception("Product not found!");
            }

            products.Remove(product);
        }

    }
}
