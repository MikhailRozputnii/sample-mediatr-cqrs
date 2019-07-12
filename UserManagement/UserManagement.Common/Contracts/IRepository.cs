using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Common.Contracts;

namespace UserManagement.Common.Contracts
{
    /// <summary>
    /// This interface contain CRUD operations 
    /// </summary>
    public interface IRepository<T> where T : class, IEntity
    {
        void Create(T item);
        IEnumerable<T> GetAll();
        T Get(Guid id);
        void Remove(T id);
        void Update(T item);
    }
}
