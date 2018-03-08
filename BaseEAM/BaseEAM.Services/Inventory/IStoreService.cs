using BaseEAM.Core;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using System;
using System.Collections.Generic;

namespace BaseEAM.Services
{
    public interface IStoreService : IBaseService
    {
        #region Store

        PagedResult<Store> GetStores(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null); //Int32.MaxValue

        PagedResult<StoreItemBalance> GetStoreItemBalances(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null);

        PagedResult<StoreLocatorItemBalance> GetStoreLocatorItemBalances(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null);

        PagedResult<StoreLocatorItemLog> GetStoreLocatorItemLogs(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null);

        StoreItem AddStoreItem(long? storeId, long? itemId);

        PagedResult<LowInventoryStoreItem> GetLowInventoryStoreItems();

        #endregion

        #region StoreLocator
        #endregion

        #region StoreLocatorItem

        decimal GetTotalQuantity(long? storeId, long? storeLocatorId, long? itemId);

        decimal GetTotalCost(long? storeId, long? storeLocatorId, long? itemId);

        List<StoreLocatorItemAdjustment> CheckSufficientQuantity(List<StoreLocatorItemAdjustment> adjustments);

        List<StoreLocatorItem> FindStoreLocatorItems(int costingType, long? storeLocatorId, long? itemId);

        List<StoreLocatorItemRemoval> FindStoreLocatorItemRemovals(
            long? storeId,
            long? storeLocatorId,
            long? itemId,
            decimal? quantityRemoved,
            out decimal? totalCost
            );

        List<StoreLocatorItemRemoval> RemoveStoreLocatorItem(
            long? storeId,
            long? storeLocatorId,
            long? itemId,
            decimal? quantityRemoved,
            string transactionType,
            long? transactionId,
            string transactionNumber,
            DateTime? transactionDate,
            long? transactionItemId,
            out decimal? totalCost
            );

        void PostToInventory(List<StoreLocatorItemLog> logs);

        #endregion

        #region StoreLocatorItemLog

        StoreLocatorItemLog AddStoreLocatorItemLog(
            long? storeLocatorItemId,
            DateTime? batchDate,
            long? storeId,
            long? storeLocatorId,
            long? itemId,
            decimal? unitPrice,
            decimal? quantityChanged,
            decimal? costChanged,
            string transactionType,
            long? transactionId,
            string transactionNumber,
            DateTime? transactionDate,
            long? transactionItemId);

        #endregion
    }
}
