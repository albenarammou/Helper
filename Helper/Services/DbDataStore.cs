using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Helper.Models;
using System.IO;
using System.Linq;

namespace Helper.Services
{
    public class DbDataStore : IDataStore<Item>
    {

        private SQLiteAsyncConnection Database;
        private static bool initialized = false;
        public DbDataStore()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            InitializeAsync().SafeFireAndForget(false);
        }
        public Task<Item> GetItemAsync(int id)
        {
            return Database.Table<Item>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Item item)
        {
            return item.Id != 0 ? Database.UpdateAsync(item) : Database.InsertAsync(item);
        }
        public Task<int> DeleteItemAsync(Item item)
        {
            return Database.DeleteAsync(item);
        }

        public Task<List<Item>> GetItemsAsync()
        {
            return Database.Table<Item>().ToListAsync();
        }

        public Task<List<Item>> GetItemsNotDoneAsync()
        {
            // SQL queries are also possible
            return Database.QueryAsync<Item>("SELECT * FROM [Items] WHERE [Done] = 0");
            //return Database.QueryAsync<Item>("SELECT * FROM [Items]");
        }

        private async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Item).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(Item)).ConfigureAwait(false);
                    initialized = true;
                }
            }
        }

    }

    public static class TaskExtensions
    {
        // NOTE: Async void is intentional here. This provides a way
        // to call an async method from the constructor while
        // communicating intent to fire and forget, and allow
        // handling of exceptions
        public static async void SafeFireAndForget(this Task task,
            bool returnToCallingContext,
            Action<Exception> onException = null)
        {
            try
            {
                await task.ConfigureAwait(returnToCallingContext);
            }

            // if the provided action is not null, catch and
            // pass the thrown exception
            catch (Exception ex) when (onException != null)
            {
                onException(ex);
            }
        }
    }
}
