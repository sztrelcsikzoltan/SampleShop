using SampleShop.Core.Contracts;
using SampleShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleShop.WebUI.Tests.Mocks
{
    public class MockContext<T> : IRepository<T> where T : BaseEntity
    {
        List<T> items;
        string className;

        public MockContext()
        {
            className = typeof(T).Name;
            items = new List<T>();
        }

        public void Commit()
        {
            return;
        }

        public IQueryable<T> GetAll()
        {
            return items.AsQueryable();
        }

        public T Get(string id)
        {
            T t = items.Find(i => i.Id == id);
            if (t == null)
            {
                throw new Exception(className + " not found!");
            }
            return t;
        }

        public void Create(T t)
        {
            items.Add(t);
        }

        public void Update(T t)
        {
            T tToUpdate = items.Find(i => i.Id == t.Id);

            if (tToUpdate == null)
            {
                throw new Exception(className + " not found!");
            }

            tToUpdate = t;
        }

        public void Delete(string id)
        {
            T tToDelete = items.Find(i => i.Id == id);

            if (tToDelete == null)
            {
                throw new Exception(className + " not found!");
            }
            items.Remove(tToDelete);
        }
    }
}
