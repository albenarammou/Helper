using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Helper.Services
{
    public interface IDataStore<T>
    {
        Task<int> SaveItemAsync(T item);
        Task<int> DeleteItemAsync(T item);
        Task<T> GetItemAsync(int id);
        Task<List<T>> GetItemsNotDoneAsync();
    }
}
