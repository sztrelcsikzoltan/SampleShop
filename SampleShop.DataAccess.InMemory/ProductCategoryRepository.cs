using SampleShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace SampleShop.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory> productCategories;

        public ProductCategoryRepository()
        {
            productCategories = cache["productCategories"] as List<ProductCategory>;
            if (productCategories == null)
            {
                productCategories = new List<ProductCategory>();
            }
        }

        public void Commit()
        {
            cache["productCategories"] = productCategories;
        }

        // CRUD methods
        public IQueryable<ProductCategory> GetProducts()
        {
            return productCategories.AsQueryable();
        }

        public ProductCategory Get(string id)
        {
            ProductCategory productCategory = productCategories.FirstOrDefault(p => p.Id == id);
            if (productCategory == null)
            {
                throw new Exception("Product category not found!");
            }

            return productCategory;
        }

        public void Insert(ProductCategory productCategory)
        {
            productCategories.Add(productCategory);
        }

        public void Update(ProductCategory productCategory, string id)
        {
            ProductCategory productCategoryToUpdate = productCategories.FirstOrDefault(p => p.Id == id);

            if (productCategoryToUpdate == null)
            {
                throw new Exception("Product category not found!");
            }

            productCategoryToUpdate = productCategory;
        }

        public void Delete(string id)
        {
            ProductCategory productCategory = productCategories.FirstOrDefault(p => p.Id == id);

            if (productCategory == null)
            {
                throw new Exception("Product category not found!");
            }

            productCategories.Remove(productCategory);
        }

    }
}
