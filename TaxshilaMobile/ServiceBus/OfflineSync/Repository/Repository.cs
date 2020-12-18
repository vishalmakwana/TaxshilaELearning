using TaxshilaMobile.ServiceBus.Services;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace TaxshilaMobile.ServiceBus.OfflineSync.Repository
{
    public class Repository<T> : IRepository<T> where T : ModelBase, new()
    {
        IBaseUrl _baseUrl;
        // Note : iOS Simulator - use static dbManager with null check below
        //private static SQLiteConnection _dbManager;
        // private readonly SQLiteConnection _dbManager;

        //private const SQLite.SQLiteOpenFlags Flags =
        //// open the database in read/write mode
        //SQLite.SQLiteOpenFlags.ReadWrite |
        //// create the database if it doesn't exist
        //SQLite.SQLiteOpenFlags.Create |
        //// enable multi-threaded database access
        //SQLite.SQLiteOpenFlags.SharedCache;

        private const SQLite.SQLiteOpenFlags ReadOnlyFlags = SQLiteOpenFlags.ReadOnly;
        private const SQLiteOpenFlags WriteOnlyFlags = SQLite.SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.FullMutex;

        #region Constructor
        public Repository(IBaseUrl baseUrl)
        {
            _baseUrl = baseUrl;


            using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), WriteOnlyFlags, storeDateTimeAsTicks: false))
            {

                //if (_dbManager == null)
                //{
                //    _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), Flags);
                //}

                //if (_dbManager != null)
                //{
                _dbManager.CreateTable<T>();
                //}
            }
        }
        #endregion

        #region Read Only Connection
        public TableQuery<T> AsQueryable()
        {
            using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), ReadOnlyFlags, storeDateTimeAsTicks: false))
            {
                return _dbManager.Table<T>();
            }
        }

        public List<T> GetItems()
        {
            using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), ReadOnlyFlags, storeDateTimeAsTicks: false))
            {
                return _dbManager.Table<T>().ToList();
            }
        }

        public List<T> GetItemsByQuery<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null)
        {
            TableQuery<T> query = null;

            using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), ReadOnlyFlags, storeDateTimeAsTicks: false))
            {
                query = _dbManager.Table<T>();

                if (predicate != null)
                    query = query.Where(predicate);

                if (orderBy != null)
                    query = query.OrderBy(orderBy);

                return query.ToList();
            }


        }

        public T GetItemById(int localId)
        {
            using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), ReadOnlyFlags, storeDateTimeAsTicks: false))
            {
                return _dbManager.Find<T>(localId);
            }
        }

        public T GetItemByQuery(Expression<Func<T, bool>> predicate)
        {
            using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), ReadOnlyFlags, storeDateTimeAsTicks: false))
            {
                return _dbManager.Find(predicate);
            }
        }
        public List<T> GetItemsWithChildren(Expression<Func<T, bool>> predicate = null)
        {
            using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), ReadOnlyFlags, storeDateTimeAsTicks: false))
            {
                return _dbManager.GetAllWithChildren(predicate, true);
            }
        }

        public T GetItemWithChildrenById(int id)
        {
            using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), ReadOnlyFlags, storeDateTimeAsTicks: false))
            {
                return _dbManager.GetWithChildren<T>(id, true);
            }
        }

        public T GetItemWithChildrenByExternalId(int externalId)
        {
            using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), ReadOnlyFlags, storeDateTimeAsTicks: false))
            {
                return _dbManager.GetAllWithChildren<T>(x => x.ServerId == externalId, true).FirstOrDefault();
            }
        }
        #endregion Read Only Connection

        #region Write Only Connection
        public int Insert(T entity)
        {

            //using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), WriteOnlyFlags))
            using (SQLiteConnectionWithLock _dbManager = new SQLiteConnectionWithLock(new SQLiteConnectionString(_baseUrl.GetDatabasePath(), false, null), WriteOnlyFlags))
            {
                using (_dbManager.Lock())
                {
                    try
                    {
                        return _dbManager.Insert(entity);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"SQLiteError: {ex.Message}");
                        return -1;
                    }
                }
            }
        }

        public void InsertAllWithTransaction(System.Collections.IEnumerable collection)
        {
            using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), false, null))
            {
                foreach (T item in collection)
                {
                    bool isRetry = true;
                    int retryCount = 0;
                    while (isRetry && retryCount < 50)
                    {
                        try
                        {
                            var existingItem = _dbManager.Find<T>(f => f.ServerId == item.ServerId && item.ServerId != 0);
                            _dbManager.BeginTransaction();
                            if (existingItem == null)
                            {
                                _dbManager.Insert(item);
                            }
                            else
                            {
                                item.LocalId = existingItem.LocalId;
                                _dbManager.Update(item);
                            }
                            _dbManager.Commit();
                            isRetry = false;
                        }
                        catch (Exception ex)
                        {
                            _dbManager.Rollback();
                            isRetry = true;
                            retryCount++;
                            Debug.WriteLine($"SQLiteError: {ex.Message}");
                        }
                    }
                }
            }
        }


        public void UpdateWithTransaction(T entity)
        {
            //using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), WriteOnlyFlags))
            using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), false, null))
            {
                bool isRetry = true;
                int retryCount = 0;
                while (isRetry && retryCount < 50)
                {
                    try
                    {
                        _dbManager.BeginTransaction();
                        _dbManager.Update(entity);
                        _dbManager.Commit();
                        isRetry = false;
                    }
                    catch (Exception ex)
                    {
                        _dbManager.Rollback();
                        isRetry = true;
                        retryCount++;
                        Debug.WriteLine($"SQLiteError: {ex.Message}");
                    }
                }
            }
        }

        public int InsertWithTransaction(T entity)
        {
            //using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), WriteOnlyFlags))
            using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), false, null))
            {
                bool isRetry = true;
                int retryCount = 0;
                int inserted = -1;
                while (isRetry && retryCount < 50)
                {
                    try
                    {
                        var existingItem = _dbManager.Find<T>(f => f.ServerId == entity.ServerId && entity.ServerId != 0);
                        _dbManager.BeginTransaction();
                        if (existingItem == null)
                        {
                            _dbManager.Insert(entity);
                            inserted = entity.LocalId;
                        }
                        else
                        {
                            entity.LocalId = existingItem.LocalId;
                            _dbManager.Update(entity);
                            inserted = existingItem.LocalId;
                        }
                        _dbManager.Commit();
                        isRetry = false;
                    }
                    catch (Exception ex)
                    {
                        _dbManager.Rollback();
                        isRetry = true;
                        retryCount++;
                        Debug.WriteLine($"SQLiteError: {ex.Message}");
                    }
                }
                return inserted;
            }
        }

        public int InsertOrReplace(System.Collections.IEnumerable collection)
        {
            using (SQLiteConnectionWithLock _dbManager = new SQLiteConnectionWithLock(new SQLiteConnectionString(_baseUrl.GetDatabasePath(), false, null), WriteOnlyFlags))
            {
                //using (_dbManager.Lock())
                //{
                return _dbManager.InsertOrReplace(collection);
            }
        }
        public int Update(T entity)
        {
            //using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), WriteOnlyFlags))
            using (SQLiteConnectionWithLock _dbManager = new SQLiteConnectionWithLock(new SQLiteConnectionString(_baseUrl.GetDatabasePath(), false, null), WriteOnlyFlags))
            {
                using (_dbManager.Lock())
                {
                    try
                    {
                        return _dbManager.Update(entity);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"SQLiteError: {ex.Message}");
                        return -1;
                    }
                }
            }
        }

        public int Upsert(T entity)
        {
            //using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), WriteOnlyFlags))
            using (SQLiteConnectionWithLock _dbManager = new SQLiteConnectionWithLock(new SQLiteConnectionString(_baseUrl.GetDatabasePath(), false, null), WriteOnlyFlags))
            {
                using (_dbManager.Lock())
                {
                    try
                    {
                        return entity.LocalId == 0 ? _dbManager.Insert(entity) : _dbManager.Update(entity);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"SQLiteError: {ex.Message}");
                        return -1;
                    }
                }
            }
        }

        public int Delete(T entity)
        {
            //using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), WriteOnlyFlags))
            using (SQLiteConnectionWithLock _dbManager = new SQLiteConnectionWithLock(new SQLiteConnectionString(_baseUrl.GetDatabasePath(), false, null), WriteOnlyFlags))
            {
                using (_dbManager.Lock())
                {
                    try
                    {
                        return _dbManager.Delete(entity);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"SQLiteError: {ex.Message}");
                        return -1;
                    }
                }
            }
        }

        public int InsertAll(System.Collections.IEnumerable collection)
        {
            //using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), WriteOnlyFlags))
            using (SQLiteConnectionWithLock _dbManager = new SQLiteConnectionWithLock(new SQLiteConnectionString(_baseUrl.GetDatabasePath(), false, null), WriteOnlyFlags))
            {
                using (_dbManager.Lock())
                {
                    try
                    {
                        return _dbManager.InsertAll(collection);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"SQLiteError: {ex.Message}");
                        return -1;
                    }
                }
            }
        }

        public int UpdateAll(System.Collections.IEnumerable collection)
        {
            //using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), WriteOnlyFlags))
            using (SQLiteConnectionWithLock _dbManager = new SQLiteConnectionWithLock(new SQLiteConnectionString(_baseUrl.GetDatabasePath(), false, null), WriteOnlyFlags))
            {
                using (_dbManager.Lock())
                {
                    try
                    {
                        return _dbManager.UpdateAll(collection);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"SQLiteError: {ex.Message}");
                        return -1;
                    }
                }
            }
        }
        public bool UpsertAllForSyncOnly(System.Collections.IEnumerable collection)
        {
            foreach (T item in collection)
            {
                using (SQLiteConnectionWithLock _dbManager = new SQLiteConnectionWithLock(new SQLiteConnectionString(_baseUrl.GetDatabasePath(), false, null), WriteOnlyFlags))
                {
                    bool itemExists = false;


                    if (item.LocalId == 0)
                    {
                        //using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), WriteOnlyFlags))

                        using (_dbManager.Lock())
                        {
                            try
                            {
                                _dbManager.InsertWithChildren(item);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"SQLiteError: {ex.Message}");
                                return false;
                            }
                        }
                    }
                    else
                    {
                        //using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), WriteOnlyFlags))
                        //using (SQLiteConnectionWithLock _dbManager = new SQLiteConnectionWithLock(new SQLiteConnectionString(_baseUrl.GetDatabasePath(), false, null), SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex | SQLiteOpenFlags.SharedCache))
                        //{
                        using (_dbManager.Lock())
                        {
                            try
                            {
                                itemExists = _dbManager.Find<T>(item.LocalId) != null;

                                if (itemExists)
                                    _dbManager.InsertOrReplaceWithChildren(item);
                                else
                                    _dbManager.InsertOrReplaceWithChildren(item);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"SQLiteError: {ex.Message}");
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }
        public bool UpsertAll(System.Collections.IEnumerable collection)
        {
            foreach (T item in collection)
            {
                if (item.LocalId == 0)
                {
                    //using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), WriteOnlyFlags))
                    using (SQLiteConnectionWithLock _dbManager = new SQLiteConnectionWithLock(new SQLiteConnectionString(_baseUrl.GetDatabasePath(), false, null), SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex))
                    {
                        using (_dbManager.Lock())
                        {
                            try
                            {
                                _dbManager.Insert(item);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"SQLiteError: {ex.Message}");
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    //using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), WriteOnlyFlags))
                    using (SQLiteConnectionWithLock _dbManager = new SQLiteConnectionWithLock(new SQLiteConnectionString(_baseUrl.GetDatabasePath(), false, null), SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex))
                    {
                        using (_dbManager.Lock())
                        {
                            try
                            {
                                _dbManager.Update(item);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"SQLiteError: {ex.Message}");
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        public void DeleteAll(System.Collections.IEnumerable collection)
        {
            foreach (var item in collection)
            {
                //using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), WriteOnlyFlags))
                using (SQLiteConnectionWithLock _dbManager = new SQLiteConnectionWithLock(new SQLiteConnectionString(_baseUrl.GetDatabasePath(), false, null), SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex))
                {
                    using (_dbManager.Lock())
                    {
                        try
                        {
                            _dbManager.Delete(item);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"SQLiteError: {ex.Message}");
                        }
                    }
                }
            }
        }

        public int DropTable()
        {
            //using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), WriteOnlyFlags))
            using (SQLiteConnectionWithLock _dbManager = new SQLiteConnectionWithLock(new SQLiteConnectionString(_baseUrl.GetDatabasePath(), false, null), SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex))
            {
                using (_dbManager.Lock())
                {
                    try
                    {
                        return _dbManager.DropTable<T>();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"SQLiteError: {ex.Message}");
                        return -1;
                    }
                }
            }
        }

        // Extensions
        public void InsertWithChildren(T entity)
        {
            //using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), WriteOnlyFlags))
            using (SQLiteConnectionWithLock _dbManager = new SQLiteConnectionWithLock(new SQLiteConnectionString(_baseUrl.GetDatabasePath(), false, null), SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex))
            {
                using (_dbManager.Lock())
                {
                    try
                    {
                        _dbManager.InsertWithChildren(entity, true);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"SQLiteError: {ex.Message}");
                    }
                }
            }
        }

        public void UpdateWithChildren(T entity)
        {
            //using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), WriteOnlyFlags))
            using (SQLiteConnectionWithLock _dbManager = new SQLiteConnectionWithLock(new SQLiteConnectionString(_baseUrl.GetDatabasePath(), false, null), SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex))
            {
                using (_dbManager.Lock())
                {
                    try
                    {
                        _dbManager.UpdateWithChildren(entity);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"SQLiteError: {ex.Message}");
                    }
                }
            }
        }

        public void InsertOrReplaceWithChildren(T entity)
        {
            //using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), WriteOnlyFlags))
            using (SQLiteConnectionWithLock _dbManager = new SQLiteConnectionWithLock(new SQLiteConnectionString(_baseUrl.GetDatabasePath(), false, null), SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex))
            {
                using (_dbManager.Lock())
                {
                    try
                    {
                        _dbManager.InsertOrReplaceWithChildren(entity, true);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"SQLiteError: {ex.Message}");
                    }
                }
            }
        }

        public void InsertOrReplaceAllWithChildren(System.Collections.IEnumerable collection)
        {
            //using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), WriteOnlyFlags))
            using (SQLiteConnectionWithLock _dbManager = new SQLiteConnectionWithLock(new SQLiteConnectionString(_baseUrl.GetDatabasePath(), false, null), SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex))
            {
                using (_dbManager.Lock())
                {
                    try
                    {
                        _dbManager.InsertOrReplaceAllWithChildren(collection, true);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"SQLiteError: {ex.Message}");
                    }
                }
            }
        }

        public void InsertAllWithChildren(System.Collections.IEnumerable collection)
        {
            //using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), WriteOnlyFlags))
            using (SQLiteConnectionWithLock _dbManager = new SQLiteConnectionWithLock(new SQLiteConnectionString(_baseUrl.GetDatabasePath(), false, null), SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex))
            {
                using (_dbManager.Lock())
                {
                    try
                    {
                        _dbManager.InsertAllWithChildren(collection, true);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"SQLiteError: {ex.Message}");
                    }
                }
            }
        }

        public void UpdateAllWithChildren(System.Collections.IEnumerable collection)
        {
            foreach (T item in collection)
            {
                //using (SQLiteConnection _dbManager = new SQLiteConnection(_baseUrl.GetDatabasePath(), WriteOnlyFlags))
                using (SQLiteConnectionWithLock _dbManager = new SQLiteConnectionWithLock(new SQLiteConnectionString(_baseUrl.GetDatabasePath(), false, null), SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex))
                {
                    using (_dbManager.Lock())
                    {
                        try
                        {
                            _dbManager.UpdateWithChildren(item);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"SQLiteError: {ex.Message}");
                        }
                    }
                }
            }
        }
        #endregion Write Only Connection


    }
}
