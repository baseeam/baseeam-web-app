/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Data
{
    public static class SqlTemplate
    {

        #region AutoNumber

        public static string AutoNumberSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServer.CommonSearch, "AutoNumber");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySql.CommonSearch, "AutoNumber");

            return template;
        }

        public static string AutoNumberSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServer.CommonCount, "AutoNumber");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySql.CommonCount, "AutoNumber");

            return template;
        }

        #endregion

        #region Store

        public static string StoreSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServer.StoreSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySql.StoreSearch;

            return template;
        }

        public static string StoreSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServer.StoreSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySql.StoreSearchCount;

            return template;
        }

        #endregion

        #region Asset

        public static string AssetSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServer.AssetSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySql.AssetSearch;

            return template;
        }

        public static string AssetSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServer.AssetSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySql.AssetSearchCount;

            return template;
        }

        #endregion

        #region Company

        public static string CompanySearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServer.CompanySearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySql.CompanySearch;

            return template;
        }

        public static string CompanySearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServer.CompanySearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySql.CompanySearchCount;

            return template;
        }

        #endregion

        #region Item

        public static string ItemSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServer.ItemSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySql.ItemSearch;

            return template;
        }

        public static string ItemSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServer.ItemCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySql.ItemCount;

            return template;
        }

        #endregion

        #region ItemGroup

        public static string ItemGroupSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServer.CommonSearch, "ItemGroup");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySql.CommonSearch, "ItemGroup");

            return template;
        }

        public static string ItemGroupSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServer.CommonCount, "ItemGroup");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySql.CommonCount, "ItemGroup");

            return template;
        }

        #endregion

        #region MessageTemplate

        public static string MessageTemplateSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServer.CommonSearch, "MessageTemplate");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySql.CommonSearch, "MessageTemplate");

            return template;
        }

        public static string MessageTemplateSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServer.CommonCount, "MessageTemplate");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySql.CommonCount, "MessageTemplate");

            return template;
        }

        #endregion

        #region WorkflowDefinition

        public static string WorkflowDefinitionSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServer.CommonSearch, "WorkflowDefinition");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySql.CommonSearch, "WorkflowDefinition");

            return template;
        }

        public static string WorkflowDefinitionSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServer.CommonCount, "WorkflowDefinition");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySql.CommonCount, "WorkflowDefinition");

            return template;
        }

        #endregion

        #region UserGroup

        public static string UserGroupSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServer.CommonSearch, "UserGroup");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySql.CommonSearch, "UserGroup");

            return template;
        }

        public static string UserGroupSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServer.CommonCount, "UserGroup");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySql.CommonCount, "UserGroup");

            return template;
        }

        #endregion

        #region Attribute

        public static string AttributeSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServer.CommonSearch, "Attribute");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySql.CommonSearch, "Attribute");

            return template;
        }

        public static string AttributeSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServer.CommonCount, "Attribute");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySql.CommonCount, "Attribute");

            return template;
        }

        #endregion

        #region MeterGroup

        public static string MeterGroupSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServer.CommonSearch, "MeterGroup");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySql.CommonSearch, "MeterGroup");

            return template;
        }

        public static string MeterGroupSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServer.CommonCount, "MeterGroup");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySql.CommonCount, "MeterGroup");

            return template;
        }

        #endregion

        #region Meter

        public static string MeterSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServer.MeterSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySql.MeterSearch;

            return template;
        }

        public static string MeterSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServer.MeterSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySql.MeterSearchCount;

            return template;
        }

        #endregion

        #region Location

        public static string LocationSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServer.LocationSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySql.LocationSearch;

            return template;
        }

        public static string LocationSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServer.LocationSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySql.LocationSearchCount;

            return template;
        }

        #endregion

        #region User

        public static string UserSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServer.UserSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySql.UserSearch;

            return template;
        }

        public static string UserSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServer.UserSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySql.UserSearchCount;

            return template;
        }

        #endregion

        #region PermissionRecord

        public static string PermissionRecordSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServer.CommonSearch, "PermissionRecord");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySql.CommonSearch, "PermissionRecord");

            return template;
        }

        public static string PermissionRecordSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServer.CommonCount, "PermissionRecord");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySql.CommonCount, "PermissionRecord");

            return template;
        }

        #endregion

        #region SecurityGroup

        public static string SecurityGroupSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServer.CommonSearch, "SecurityGroup");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySql.CommonSearch, "SecurityGroup");

            return template;
        }

        public static string SecurityGroupSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServer.CommonCount, "SecurityGroup");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySql.CommonCount, "SecurityGroup");

            return template;
        }

        #endregion

        #region Organization

        public static string OrganizationSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServer.CommonSearch, "Organization");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySql.CommonSearch, "Organization");

            return template;
        }

        public static string OrganizationSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServer.CommonCount, "Organization");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySql.CommonCount, "Organization");

            return template;
        }

        #endregion

        #region Site

        public static string SiteSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServer.CommonSearch, "Site");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySql.CommonSearch, "Site");

            return template;
        }

        #endregion

        #region Language

        public static string LanguageSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServer.CommonSearch, "Language");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySql.CommonSearch, "Language");

            return template;
        }

        #endregion

        #region Log

        public static string LogSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServer.LogSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySql.LogSearch;

            return template;
        }

        public static string LogSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServer.LogSearchCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySql.LogSearchCount;

            return template;
        }

        #endregion

        #region Module

        public static string ModuleSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServer.ModuleSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySql.ModuleSearch;

            return template;
        }

        #endregion

        #region ValueItem

        public static string ValueItemSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServer.ValueItemSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySql.ValueItemSearch;

            return template;
        }

        public static string ValueItemSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServer.CommonCount, "ValueItem");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySql.CommonCount, "ValueItem");

            return template;
        }
        #endregion

        #region UnitOfMeasure
        public static string UnitOfMeasureSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServer.CommonSearch, "UnitOfMeasure");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySql.CommonSearch, "UnitOfMeasure");

            return template;
        }

        public static string UnitOfMeasureSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = string.Format(SqlServer.CommonCount, "UnitOfMeasure");
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = string.Format(MySql.CommonCount, "UnitOfMeasure");

            return template;
        }
        #endregion

        #region UnitConversion

        public static string UnitConversionSearch()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServer.UnitConversionSearch;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySql.UnitConversionSearch;

            return template;
        }

        public static string UnitConversionSearchCount()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServer.UnitConversionCount;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySql.UnitConversionCount;

            return template;
        }

        #endregion

        #region Common

        public static string GetById()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServer.GetById;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySql.GetById;

            return template;
        }

        public static string DeleteById()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServer.DeleteById;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySql.DeleteById;

            return template;
        }

        public static string GetByColumn()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServer.GetByColumn;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySql.GetByColumn;

            return template;
        }

        public static string GetAll()
        {
            string template = "";
            if (DataSettings.DataProvider == "System.Data.SqlClient")
                template = SqlServer.GetAll;
            else if (DataSettings.DataProvider == "MySql.Data.MySqlClient")
                template = MySql.GetAll;

            return template;
        }

        #endregion

        public static class SqlServer
        {
            public static readonly string StoreSearch =
                "SELECT * FROM Store" +
                 " INNER JOIN Valueitem AS StoreType on Store.StoreTypeId = StoreType.Id" +
                 " INNER JOIN Site ON Store.SiteId = Site.Id" +
                " /**where**/" +
                ActiveCondition("Store", "StoreType", "Site") +
                " /**orderby**/" +
                " OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";

            public static readonly string StoreSearchCount =
                "SELECT COUNT(1) FROM Store" +
                 " INNER JOIN Valueitem AS StoreType on Store.StoreTypeId = StoreType.Id" +
                 " INNER JOIN Site ON Store.SiteId = Site.Id" +
                " /**where**/" +
                ActiveCondition("Store", "StoreType", "Site") +
                " /**orderby**/";

            public static readonly string AssetSearch =
                "SELECT * FROM Asset" +
                 " INNER JOIN Valueitem AS AssetType on Asset.AssetTypeId = AssetType.Id" +
                 " INNER JOIN Valueitem AS AssetStatus on Asset.AssetStatusId = AssetStatus.Id" +
                 " INNER JOIN Location  ON Asset.LocationId = Location.Id" +
                 " INNER JOIN Site ON Asset.SiteId = Site.Id" +
                " /**where**/" +
                ActiveCondition("Asset", "AssetType", "AssetStatus", "Location", "Site") +
                " /**orderby**/" +
                " OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";

            public static readonly string AssetSearchCount =
                "SELECT COUNT(1) FROM Asset" +
                 " INNER JOIN Valueitem AS AssetType on Asset.AssetTypeId = AssetType.Id" +
                 " INNER JOIN Valueitem AS AssetStatus on Asset.AssetStatusId = AssetStatus.Id" +
                 " INNER JOIN Location  ON Asset.LocationId = Location.Id" +
                 " INNER JOIN Site ON Asset.SiteId = Site.Id" +
                " /**where**/" +
                ActiveCondition("Asset", "AssetType", "AssetStatus", "Location", "Site") +
                " /**orderby**/";

            public static readonly string CompanySearch =
                "SELECT * FROM Company" +
                " INNER JOIN ValueItem ON Company.CompanyTypeId = ValueItem.Id" +
                " /**where**/" +
                ActiveCondition("Company", "ValueItem") +
                " /**orderby**/" +
                " OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";

            public static readonly string CompanySearchCount =
                "SELECT COUNT(1) FROM Company" +
                " INNER JOIN ValueItem ON Company.CompanyTypeId = ValueItem.Id" +
                " /**where**/" +
                ActiveCondition("Company", "ValueItem") +
                " /**orderby**/";

            public static readonly string ItemSearch =
                "SELECT * FROM Item INNER JOIN ItemGroup ON Item.ItemGroupId = ItemGroup.Id" +
                " INNER JOIN UnitOfMeasure ON Item.UnitOfMeasureId = UnitOfMeasure.Id" +
                " /**where**/" +
                ActiveCondition("Item", "ItemGroup", "UnitOfMeasure") +
                " /**orderby**/" +
                " OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";

            public static readonly string ItemCount =
                "SELECT COUNT(1) FROM Item INNER JOIN ItemGroup ON Item.ItemGroupId = ItemGroup.Id" +
                " INNER JOIN UnitOfMeasure ON Item.UnitOfMeasureId = UnitOfMeasure.Id" +
                " /**where**/" +
                ActiveCondition("Item", "ItemGroup", "UnitOfMeasure") +
                " /**orderby**/" +
                " OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";

            public static readonly string MeterSearch =
                "SELECT * FROM Meter" +
                " INNER JOIN ValueItem ON Meter.MeterTypeId = ValueItem.Id" +
                " /**where**/" +
                ActiveCondition("Meter", "ValueItem") +
                " /**orderby**/" +
                " OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";

            public static readonly string MeterSearchCount =
                "SELECT COUNT(1) FROM Meter" +
                " INNER JOIN ValueItem ON Meter.MeterTypeId = ValueItem.Id" +
                " /**where**/" +
                ActiveCondition("Meter", "ValueItem") +
                " /**orderby**/";

            public static readonly string CommonSearch =
                "SELECT * FROM [{0}] /**where**/ AND {0}.IsDeleted = 0 AND {0}.IsNew = 0 /**orderby**/" +
                " OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";

            public static readonly string CommonCount =
                "SELECT COUNT(1) FROM [{0}] /**where**/ AND {0}.IsDeleted = 0 AND {0}.IsNew = 0 /**orderby**/";

            public static readonly string LogSearch =
                "SELECT * FROM Log INNER JOIN [User] ON Log.UserId = [User].Id /**where**/" +
                ActiveCondition("Log", "User") +
                " /**orderby**/" +
                " OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";

            public static readonly string LogSearchCount =
                "SELECT COUNT(1) FROM Log INNER JOIN [User] ON Log.UserId = [User].Id /**where**/" +
                ActiveCondition("Log", "User") +
                " /**orderby**/";

            public static readonly string ModuleSearch =
                "SELECT DISTINCT Module.* FROM Module INNER JOIN Feature ON Module.Id = Feature.ModuleId /**where**/" +
                ActiveCondition("Module", "Feature") +
                " /**orderby**/" +
                " OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";

            public static readonly string ModuleSearchCount =
                "SELECT COUNT(DISTINCT Module.Id) FROM Module INNER JOIN Feature ON Module.Id = Feature.ModuleId /**where**/" +
                ActiveCondition("Module", "Feature") +
                " /**orderby**/";

            public static readonly string ValueItemSearch =
               "SELECT * FROM ValueItem INNER JOIN ValueItemCategory ON ValueItem.ValueItemCategoryId = ValueItemCategory.Id /**where**/" +
                ActiveCondition("ValueItem", "ValueItemCategory") +
                " /**orderby**/" +
                " OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";

            public static readonly string ValueItemSearchCount =
                "SELECT COUNT(1) FROM ValueItem INNER JOIN ValueItemCategory ON ValueItem.ValueItemCategoryId = ValueItemCategory.Id /**where**/" +
                ActiveCondition("ValueItem", "ValueItemCategory") +
                " /**orderby**/";

            public static readonly string UnitConversionSearch =
               "SELECT * FROM UnitConversion INNER JOIN UnitOfMeasure AS FromUnitOfMeasure ON UnitConversion.FromUnitOfMeasureId = FromUnitOfMeasure.Id INNER JOIN UnitOfMeasure AS ToUnitOfMeasure ON UnitConversion.ToUnitOfMeasureId = ToUnitOfMeasure.Id /**where**/" +
                ActiveCondition("UnitConversion", "FromUnitOfMeasure", "ToUnitOfMeasure") +
                " /**orderby**/" +
                " OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";

            public static readonly string UnitConversionCount =
              "SELECT COUNT(1) FROM UnitConversion INNER JOIN UnitOfMeasure AS FromUnitOfMeasure ON UnitConversion.FromUnitOfMeasureId = FromUnitOfMeasure.Id INNER JOIN UnitOfMeasure AS ToUnitOfMeasure ON UnitConversion.ToUnitOfMeasureId = ToUnitOfMeasure.Id /**where**/" +
                ActiveCondition("UnitConversion", "FromUnitOfMeasure", "ToUnitOfMeasure") +
                " /**orderby**/";

            public static readonly string UserSearch =
                "SELECT * FROM User" +
                " INNER JOIN SecurityGroup_User ON User.Id = SecurityGroup_User.UserId" +
                " /**where**/" +
                ActiveCondition("User") +
                " /**orderby**/" +
                " OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";

            public static readonly string UserSearchCount =
                "SELECT COUNT(1) FROM User" +
                " INNER JOIN SecurityGroup_User ON User.Id = SecurityGroup_User.UserId" +
                " /**where**/" +
                ActiveCondition("User") +
                " /**orderby**/";

            public static readonly string LocationSearch =
                "SELECT * FROM Location" +
                " INNER JOIN ValueItem AS LocationType ON Location.LocationTypeId = LocationType.Id" +
                " INNER JOIN ValueItem AS LocationStatus ON Location.LocationStatusId = LocationStatus.Id" +
                " INNER JOIN Site ON Location.SiteId = Site.Id" +
                " /**where**/" +
                ActiveCondition("Location", "LocationType", "LocationStatus", "Site") +
                " /**orderby**/" +
                " OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY";

            public static readonly string LocationSearchCount =
                "SELECT COUNT(1) FROM Location" +
                " INNER JOIN ValueItem AS LocationType ON Location.LocationTypeId = LocationType.Id" +
                " INNER JOIN ValueItem AS LocationStatus ON Location.LocationStatusId = LocationStatus.Id" +
                " INNER JOIN Site ON Location.SiteId = Site.Id" +
                " /**where**/" +
                ActiveCondition("Location", "LocationType", "LocationStatus", "Site") +
                " /**orderby**/";

            #region Common

            public static readonly string GetById =
                "SELECT * FROM [{0}] WHERE Id = {1}";

            public static readonly string DeleteById =
                "DELETE FROM [{0}] WHERE Id = {1}";

            public static readonly string GetByColumn =
                "SELECT * FROM [{0}] WHERE [{1}] = {2}";

            public static readonly string GetAll =
                "SELECT [{0}], [{1}] FROM [{2}] WHERE IsDeleted = 0 AND IsNew = 0 AND [{1}] LIKE '%{3}%'";

            #endregion

            public static string ActiveCondition(params string[] tables)
            {
                string result = "";
                foreach (string table in tables)
                {
                    result = result + string.Format(" AND {0}.IsDeleted = 0 AND {0}.IsNew = 0", table);
                }
                return result;
            }
        }

        public static class MySql
        {
            public static readonly string StoreSearch =
                "SELECT * FROM Store" +
                " INNER JOIN Valueitem AS StoreType on Store.StoreTypeId = StoreType.Id" +
                " INNER JOIN Site ON Store.SiteId = Site.Id" +
                " /**where**/" +
                ActiveCondition("Store", "StoreType", "Site") +
                " /**orderby**/" +
                " LIMIT @skip, @take";

            public static readonly string StoreSearchCount =
                 "SELECT COUNT(1) FROM Store" +
                 " INNER JOIN Valueitem AS StoreType on Store.StoreTypeId = StoreType.Id" +
                 " INNER JOIN Site ON Store.SiteId = Site.Id" +
                 " /**where**/" +
                 ActiveCondition("Store", "StoreType", "Site") +
                 " /**orderby**/" +
                 " LIMIT @skip, @take";

            public static readonly string AssetSearch =
                "SELECT * FROM Asset" +
                " INNER JOIN Valueitem AS AssetType on Asset.AssetTypeId = AssetType.Id" +
                " INNER JOIN Valueitem AS AssetStatus on Asset.AssetStatusId = AssetStatus.Id" +
                " INNER JOIN Location  ON Asset.LocationId = Location.Id" +
                " INNER JOIN Site ON Asset.SiteId = Site.Id" +
                " /**where**/" +
                ActiveCondition("Asset", "AssetType", "AssetStatus", "Location", "Site") +
                " /**orderby**/" +
                " LIMIT @skip, @take";

            public static readonly string AssetSearchCount =
                 "SELECT COUNT(1) FROM Asset" +
                 " INNER JOIN Valueitem AS AssetType on Asset.AssetTypeId = AssetType.Id" +
                 " INNER JOIN Valueitem AS AssetStatus on Asset.AssetStatusId = AssetStatus.Id" +
                 " INNER JOIN Location  ON Asset.LocationId = Location.Id" +
                 " INNER JOIN Site ON Asset.SiteId = Site.Id" +
                 " /**where**/" +
                 ActiveCondition("Asset", "AssetType", "AssetStatus", "Location", "Site") +
                 " /**orderby**/" +
                 " LIMIT @skip, @take";

            public static readonly string CompanySearch =
                "SELECT * FROM Company" +
                " INNER JOIN ValueItem ON Company.CompanyTypeId = ValueItem.Id" +
                " /**where**/" +
                ActiveCondition("Company", "ValueItem") +
                " /**orderby**/" +
                " LIMIT @skip, @take";

            public static readonly string CompanySearchCount =
                "SELECT COUNT(1) FROM Company" +
                " INNER JOIN ValueItem ON Company.CompanyTypeId = ValueItem.Id" +
                " /**where**/" +
                ActiveCondition("Company", "ValueItem") +
                " /**orderby**/";

            public static readonly string ItemSearch =
               "SELECT * FROM Item INNER JOIN ItemGroup ON Item.ItemGroupId = ItemGroup.Id" +
                " INNER JOIN UnitOfMeasure ON Item.UnitOfMeasureId = UnitOfMeasure.Id" +
                " /**where**/" +
                ActiveCondition("Item", "ItemGroup", "UnitOfMeasure") +
                " /**orderby**/" +
                " LIMIT @skip, @take";

            public static readonly string ItemCount =
               "SELECT COUNT(1) FROM Item INNER JOIN ItemGroup ON Item.ItemGroupId = ItemGroup.Id" +
                " INNER JOIN UnitOfMeasure ON Item.UnitOfMeasureId = UnitOfMeasure.Id" +
                " /**where**/" +
                ActiveCondition("Item", "ItemGroup", "UnitOfMeasure") +
                " /**orderby**/" +
                " LIMIT @skip, @take";

            public static readonly string MeterSearch =
                "SELECT * FROM Meter" + 
                " INNER JOIN ValueItem ON Meter.MeterTypeId = ValueItem.Id" +
                " /**where**/" +
                ActiveCondition("Meter", "ValueItem") +
                " /**orderby**/" +
                " LIMIT @skip, @take";

            public static readonly string MeterSearchCount =
                "SELECT COUNT(1) FROM Meter" +
                " INNER JOIN ValueItem ON Meter.MeterTypeId = ValueItem.Id" +
                " /**where**/" +
                ActiveCondition("Meter", "ValueItem") +
                " /**orderby**/";

            public static readonly string CommonSearch =
                "SELECT * FROM {0} /**where**/ AND {0}.IsDeleted = 0 AND {0}.IsNew = 0 /**orderby**/" +
                " LIMIT @skip, @take";

            public static readonly string CommonCount =
                "SELECT COUNT(1) FROM {0} /**where**/ AND {0}.IsDeleted = 0 AND {0}.IsNew = 0 /**orderby**/";

            public static readonly string LogSearch =
                "SELECT * FROM Log INNER JOIN User ON Log.UserId = User.Id /**where**/" +
                ActiveCondition("Log", "User") +
                " /**orderby**/" +
                " LIMIT @skip, @take";

            public static readonly string LogSearchCount =
                "SELECT COUNT(1) FROM Log INNER JOIN User ON Log.UserId = User.Id /**where**/" +
                ActiveCondition("Log", "User") +
                " /**orderby**/";

            public static readonly string ModuleSearch =
                "SELECT DISTINCT Module.* FROM Module INNER JOIN Feature ON Module.Id = Feature.ModuleId /**where**/" +
                ActiveCondition("Module", "Feature") +
                " /**orderby**/" +
                " LIMIT @skip, @take";

            public static readonly string ModuleSearchCount =
                "SELECT COUNT(DISTINCT Module.Id) FROM Module INNER JOIN Feature ON Module.Id = Feature.ModuleId /**where**/" +
                ActiveCondition("Module", "Feature") +
                " /**orderby**/";

            public static readonly string ValueItemSearch =
                "SELECT * FROM ValueItem INNER JOIN ValueItemCategory ON ValueItem.ValueItemCategoryId = ValueItemCategory.Id /**where**/" +
                ActiveCondition("ValueItem", "ValueItemCategory") +
                " /**orderby**/" +
                " LIMIT @skip, @take";

            public static readonly string ValueItemSearchCount =
                "SELECT COUNT(1) FROM ValueItem INNER JOIN ValueItemCategory ON ValueItem.ValueItemCategoryId = ValueItemCategory.Id /**where**/" +
                ActiveCondition("ValueItem", "ValueItemCategory") +
                " /**orderby**/";

            public static readonly string UnitConversionSearch =
                "SELECT * FROM UnitConversion INNER JOIN UnitOfMeasure AS FromUnitOfMeasure ON UnitConversion.FromUnitOfMeasureId = FromUnitOfMeasure.Id INNER JOIN UnitOfMeasure AS ToUnitOfMeasure ON UnitConversion.ToUnitOfMeasureId = ToUnitOfMeasure.Id /**where**/" +
                ActiveCondition("UnitConversion", "FromUnitOfMeasure", "ToUnitOfMeasure") +
                " /**orderby**/" +
                " LIMIT @skip, @take";

            public static readonly string UnitConversionCount =
               "SELECT COUNT(1) FROM UnitConversion INNER JOIN UnitOfMeasure AS FromUnitOfMeasure ON UnitConversion.FromUnitOfMeasureId = FromUnitOfMeasure.Id INNER JOIN UnitOfMeasure AS ToUnitOfMeasure ON UnitConversion.ToUnitOfMeasureId = ToUnitOfMeasure.Id /**where**/" +
                ActiveCondition("UnitConversion", "FromUnitOfMeasure", "ToUnitOfMeasure") +
                " /**orderby**/";

            public static readonly string UserSearch =
                "SELECT * FROM User" +
                " INNER JOIN SecurityGroup_User ON User.Id = SecurityGroup_User.UserId" +
                " /**where**/" +
                ActiveCondition("User") +
                " /**orderby**/" +
                " LIMIT @skip, @take";

            public static readonly string UserSearchCount =
                "SELECT COUNT(1) FROM User" +
                " INNER JOIN SecurityGroup_User ON User.Id = SecurityGroup_User.UserId" +
                " /**where**/" +
                ActiveCondition("User") +
                " /**orderby**/";

            public static readonly string LocationSearch =
                "SELECT * FROM Location" +
                " INNER JOIN ValueItem AS LocationType ON Location.LocationTypeId = LocationType.Id" +
                " INNER JOIN ValueItem AS LocationStatus ON Location.LocationStatusId = LocationStatus.Id" +
                " INNER JOIN Site ON Location.SiteId = Site.Id" +
                " /**where**/" +
                ActiveCondition("Location", "LocationType", "LocationStatus", "Site") +
                " /**orderby**/" +
                " LIMIT @skip, @take";

            public static readonly string LocationSearchCount =
                "SELECT COUNT(1) FROM Location" +
                " INNER JOIN ValueItem AS LocationType ON Location.LocationTypeId = LocationType.Id" +
                " INNER JOIN ValueItem AS LocationStatus ON Location.LocationStatusId = LocationStatus.Id" +
                " INNER JOIN Site ON Location.SiteId = Site.Id" +
                " /**where**/" +
                ActiveCondition("Location", "LocationType", "LocationStatus", "Site") +
                " /**orderby**/";

            #region Common

            public static readonly string GetById =
                "SELECT * FROM {0} WHERE Id = {1}";

            public static readonly string DeleteById =
                "DELETE FROM {0} WHERE Id = {1}";

            public static readonly string GetByColumn =
                "SELECT * FROM {0} WHERE {1} = {2}";

            public static readonly string GetAll =
                "SELECT {0}, {1} FROM {2} WHERE IsDeleted = 0 AND IsNew = 0 AND {1} LIKE '%{3}%'";

            #endregion

            public static string ActiveCondition(params string[] tables)
            {
                string result = "";
                foreach (string table in tables)
                {
                    result = result + string.Format(" AND {0}.IsDeleted = 0 AND {0}.IsNew = 0", table);
                }
                return result;
            }
        }
    }
}
