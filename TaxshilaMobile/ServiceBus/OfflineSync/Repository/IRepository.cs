using SQLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace TaxshilaMobile.ServiceBus.OfflineSync
{
    public interface IRepository<T> where T : ModelBase, new()
    {
        List<T> GetItems();
        T GetItemById(int id);
        List<T> GetItemsByQuery<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null);
        T GetItemByQuery(Expression<Func<T, bool>> predicate);
        TableQuery<T> AsQueryable();
        int Insert(T entity);
        int Update(T entity);
        int Upsert(T entity);
        int Delete(T entity);
        int InsertAll(System.Collections.IEnumerable collection);
        int UpdateAll(System.Collections.IEnumerable collection);
        //void UpsertAll(System.Collections.IEnumerable collection);
        void DeleteAll(System.Collections.IEnumerable collection);
        bool UpsertAll(System.Collections.IEnumerable collection);
        int DropTable();

        // Extensions
        void InsertWithChildren(T entity);

        void UpdateWithChildren(T entity);
        void InsertOrReplaceWithChildren(T entity);
        int InsertOrReplace(System.Collections.IEnumerable collection);
        void InsertAllWithChildren(System.Collections.IEnumerable collection);
        void UpdateAllWithChildren(System.Collections.IEnumerable collection);
        void InsertOrReplaceAllWithChildren(System.Collections.IEnumerable collection);
        List<T> GetItemsWithChildren(Expression<Func<T, bool>> predicate = null);
        T GetItemWithChildrenById(int id);
        T GetItemWithChildrenByExternalId(int externalId);

        bool UpsertAllForSyncOnly(System.Collections.IEnumerable collection);
        int InsertWithTransaction(T entity);
        void InsertAllWithTransaction(System.Collections.IEnumerable collection);
        void UpdateWithTransaction(T entity);
    }
}
