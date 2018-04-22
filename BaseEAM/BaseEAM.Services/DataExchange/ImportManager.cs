/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Services.DataExchange.Help;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using BaseEAM.Data;
using Dapper;

namespace BaseEAM.Services
{
    public class ImportManager : IImportManager
    {
        #region raw SQL query
        private const string UPDATE_COMPANY_ADDRESS = "UPDATE Company_Address SET CompanyId = {0}, AddressId= {1} WHERE CompanyId = {0}";
        private const string INSERT_COMPANY_ADDRESS = "INSERT INTO Company_Address(CompanyId, AddressId) Values({0},{1})";
        #endregion
        private readonly IRepository<ImportProfile> _importProfileRepository;
        private readonly IRepository<UnitOfMeasure> _unitOfMeasureRepository;
        private readonly IDapperRepository<UnitOfMeasure> _unitOfMeasureDapperRepository;
        private readonly IRepository<ValueItem> _valueItemRepository;
        private readonly IDapperRepository<ValueItem> _valueItemDapperRepository;
        private readonly IRepository<ValueItemCategory> _valueItemCategoryRepository;
        private readonly IDapperRepository<ValueItemCategory> _valueItemCategoryDapperRepository;
        private readonly IRepository<ItemGroup> _itemGroupRepository;
        private readonly IDapperRepository<ItemGroup> _itemGroupDapperRepository;
        private readonly IRepository<Item> _itemRepository;
        private readonly IDapperRepository<Item> _itemDapperRepository;
        private readonly IRepository<Location> _locationRepository;
        private readonly IDapperRepository<Location> _locationDapperRepository;
        private readonly IRepository<Company> _companyRepository;
        private readonly IDapperRepository<Company> _companyDapperRepository;
        private readonly IRepository<Site> _siteRepository;
        private readonly IRepository<Address> _addressRepository;
        private readonly IDapperRepository<Address> _addressDapperRepository;
        private readonly IRepository<Contact> _contactRepository;
        private readonly IDapperRepository<Contact> _contactDapperRepository;
        private readonly IRepository<Meter> _meterRepository;
        private readonly IDapperRepository<Meter> _meterDapperRepository;
        private readonly IRepository<Store> _storeRepository;
        private readonly IDapperRepository<Store> _storeDapperRepository;
        private readonly IDapperRepository<StoreLocator> _storeLocatorDapperRepository;
        private readonly IRepository<Asset> _assetRepository;
        private readonly IDapperRepository<Asset> _assetDapperRepository;
        private readonly IRepository<Currency> _currencyRepository;
        private readonly IUnitOfMeasureService _unitOfMeasureService;
        private readonly IWorkContext _workContext;
        private readonly DapperContext _dapperContext;

        public ImportManager(IRepository<ImportProfile> importProfileRepository,
            IDapperRepository<UnitOfMeasure> unitOfMeasureDapperRepository,
            IRepository<ValueItem> valueItemRepository,
            IDapperRepository<ValueItem> valueItemDapperRepository,
            IRepository<ValueItemCategory> valueItemCategoryRepository,
            IDapperRepository<ValueItemCategory> valueItemCategoryDapperRepository,
            IRepository<ItemGroup> itemGroupRepository,
            IDapperRepository<ItemGroup> itemGroupDapperRepository,
            IRepository<Item> itemRepository,
            IDapperRepository<Item> itemDapperRepository,
            IRepository<Location> locationRepository,
            IDapperRepository<Location> locationDapperRepository,
            IRepository<Company> companyRepository,
            IDapperRepository<Company> companyDapperRepository,
            IRepository<Site> siteRepository,
            IRepository<UnitOfMeasure> unitOfMeasureRepository,
            IRepository<Address> addressRepository,
            IDapperRepository<Address> addressDapperRepository,
            IRepository<Contact> contactRepository,
            IDapperRepository<Contact> contactDapperRepository,
            IRepository<Meter> meterRepository,
            IDapperRepository<Meter> meterDapperRepository,
            IRepository<Store> storeRepository,
            IDapperRepository<Store> storeDapperRepository,
            IDapperRepository<StoreLocator> storeLocatorDapperRepository,
            IRepository<Asset> assetRepository,
            IDapperRepository<Asset> assetDapperRepository,
            IRepository<Currency> currencyRepository,
            IUnitOfMeasureService unitOfMeasureService,
            IWorkContext workContext,
            DapperContext dapperContext)
        {
            this._importProfileRepository = importProfileRepository;
            this._unitOfMeasureRepository = unitOfMeasureRepository;
            this._unitOfMeasureDapperRepository = unitOfMeasureDapperRepository;
            this._valueItemRepository = valueItemRepository;
            this._valueItemDapperRepository = valueItemDapperRepository;
            this._valueItemCategoryRepository = valueItemCategoryRepository;
            this._valueItemCategoryDapperRepository = valueItemCategoryDapperRepository;
            this._itemGroupRepository = itemGroupRepository;
            this._itemGroupDapperRepository = itemGroupDapperRepository;
            this._itemRepository = itemRepository;
            this._itemDapperRepository = itemDapperRepository;
            this._locationRepository = locationRepository;
            this._locationDapperRepository = locationDapperRepository;
            this._companyRepository = companyRepository;
            this._companyDapperRepository = companyDapperRepository;
            this._siteRepository = siteRepository;
            this._addressRepository = addressRepository;
            this._addressDapperRepository = addressDapperRepository;
            this._contactRepository = contactRepository;
            this._contactDapperRepository = contactDapperRepository;
            this._meterRepository = meterRepository;
            this._meterDapperRepository = meterDapperRepository;
            this._storeRepository = storeRepository;
            this._storeDapperRepository = storeDapperRepository;
            this._storeLocatorDapperRepository = storeLocatorDapperRepository;
            this._currencyRepository = currencyRepository;
            this._unitOfMeasureService = unitOfMeasureService;
            this._assetRepository = assetRepository;
            this._assetDapperRepository = assetDapperRepository;
            this._workContext = workContext;
            this._dapperContext = dapperContext;
        }

        public virtual void Execute(long importProfileId)
        {
            // get EntityType and call the corresponding import method.
            var importProfile = _importProfileRepository.GetById(importProfileId);
            var entityType = importProfile.EntityType;

            switch (entityType)
            {
                case EntityType.UnitOfMeasure:
                    ImportUnitOfMeasureFromXlsx(importProfile);
                    break;
                case EntityType.ValueItem:
                    ImportValueItemFromXlsx(importProfile);
                    break;
                case EntityType.ItemGroup:
                    ImportItemGroupFromXlsx(importProfile);
                    break;
                case EntityType.Item:
                    ImportItemFromXlsx(importProfile);
                    break;
                case EntityType.Location:
                    ImportLocationFromXlsx(importProfile);
                    break;
                case EntityType.Meter:
                    ImportMeterFromXlsx(importProfile);
                    break;
                case EntityType.Company:
                    ImportCompanyFromXlsx(importProfile);
                    break;
                case EntityType.Store:
                    ImportStoreFromXlsx(importProfile);
                    break;
                case EntityType.Asset:
                    ImportAssetFromXlsx(importProfile);
                    break;
            }
        }

        public virtual void ImportUnitOfMeasureFromXlsx(ImportProfile importProfile)
        {
            var rootPath = ConfigurationManager.AppSettings["ImportFolder"];
            var importProfileFolderPath = Path.Combine(rootPath, string.Format("Profile{0}", importProfile.Id));
            var importProfilePath = Path.Combine(importProfileFolderPath, importProfile.ImportFileName);
            var importResult = new ImportResult();
            importResult.LastRunStartDateTime = DateTime.UtcNow;
            if (File.Exists(importProfilePath))
            {
                using (var stream = new FileStream(importProfilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var xlPackage = new ExcelPackage(stream))
                    {
                        // get the first worksheet in the workbook
                        var worksheet = xlPackage.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet == null)
                        {
                            importResult.AddError(0, "No worksheet found");
                            throw new Exception("No worksheet found");
                        }

                        //the columns
                        var properties = new List<PropertyByName<UnitOfMeasure>>();
                        var poz = 1;
                        while (true)
                        {
                            try
                            {
                                var cell = worksheet.Cells[1, poz];
                                var test = new PropertyByName<string>(cell.Value.ToString());
                                if (cell == null || cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString()))
                                    break;

                                poz += 1;
                                properties.Add(new PropertyByName<UnitOfMeasure>(cell.Value.ToString()));
                            }
                            catch
                            {
                                break;
                            }
                        }
                        var manager = new PropertyManager<UnitOfMeasure>(properties.ToArray());
                        var row = 2;

                        // get all uoms
                        var uoms = _unitOfMeasureRepository.GetAll().ToList();
                        while (true)
                        {
                            var allColumnsAreEmpty = manager.GetProperties
                                .Select(property => worksheet.Cells[row, property.PropertyOrderPosition])
                                .All(cell => cell == null || cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString()));

                            if (allColumnsAreEmpty)
                                break;

                            manager.ReadFromXlsx(worksheet, row);
                            var uomProperty = manager.GetProperty("Name");
                            var uomName = uomProperty.StringValue;
                            if (string.IsNullOrEmpty(uomName))
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Name' field is required. Skipping row.");
                                ++row;
                                continue;
                            }
                            UnitOfMeasure uom = uoms.Where(u => u.Name == uomName).FirstOrDefault();
                            var isNew = uom == null;
                            uom = uom ?? new UnitOfMeasure();
                            foreach (var property in manager.GetProperties)
                            {

                                switch (property.PropertyName)
                                {
                                    case "Name":
                                        uom.Name = property.StringValue;
                                        break;
                                    case "Abbreviation":
                                        uom.Abbreviation = property.StringValue;
                                        break;
                                    case "Description":
                                        uom.Description = property.StringValue;
                                        break;
                                }
                            }
                            if (isNew)
                            {
                                try
                                {
                                    _unitOfMeasureDapperRepository.Insert(uom);
                                    // add new uom into uoms cache
                                    uoms.Add(uom);
                                    ++importResult.TotalRecords;
                                }
                                catch (Exception ex)
                                {
                                    ++importResult.SkippedRecords;
                                    importResult.AddError(row, ex.Message);
                                }
                            }
                            else
                            {
                                try
                                {
                                    _unitOfMeasureDapperRepository.Update(uom);
                                    ++importResult.TotalRecords;

                                }
                                catch (Exception ex)
                                {
                                    ++importResult.SkippedRecords;
                                    importResult.AddError(row, ex.Message);
                                }
                            }
                            ++row;
                        }
                    }
                }
            }
            importResult.LastRunEndDateTime = DateTime.UtcNow;
            WriteLogFile(importProfile, importResult);
        }

        public virtual void ImportValueItemFromXlsx(ImportProfile importProfile)
        {
            var rootPath = ConfigurationManager.AppSettings["ImportFolder"];
            var importProfileFolderPath = Path.Combine(rootPath, string.Format("Profile{0}", importProfile.Id));
            var importProfilePath = Path.Combine(importProfileFolderPath, importProfile.ImportFileName);
            var importResult = new ImportResult();
            importResult.LastRunStartDateTime = DateTime.UtcNow;
            if (File.Exists(importProfilePath))
            {
                using (var stream = new FileStream(importProfilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var xlPackage = new ExcelPackage(stream))
                    {
                        // get the first worksheet in the workbook
                        var worksheet = xlPackage.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet == null)
                        {
                            importResult.AddError(0, "No worksheet found");
                            throw new Exception("No worksheet found");
                        }

                        //the columns
                        var properties = new List<PropertyByName<ValueItem>>();
                        var poz = 1;
                        while (true)
                        {
                            try
                            {
                                var cell = worksheet.Cells[1, poz];
                                var test = new PropertyByName<string>(cell.Value.ToString());
                                if (cell == null || cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString()))
                                    break;

                                poz += 1;
                                properties.Add(new PropertyByName<ValueItem>(cell.Value.ToString()));
                            }
                            catch
                            {
                                break;
                            }
                        }
                        var manager = new PropertyManager<ValueItem>(properties.ToArray());
                        var row = 2;

                        // get all valueItems, valueItemCategories
                        var valueItems = _valueItemRepository.GetAll().ToList();
                        var valueItemCategories = _valueItemCategoryRepository.GetAll().ToList();
                        while (true)
                        {
                            var allColumnsAreEmpty = manager.GetProperties
                                .Select(property => worksheet.Cells[row, property.PropertyOrderPosition])
                                .All(cell => cell == null || cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString()));

                            if (allColumnsAreEmpty)
                                break;

                            manager.ReadFromXlsx(worksheet, row);
                            var viCategory = manager.GetProperty("ValueItemCategory");
                            var viCategoryName = viCategory.StringValue;
                            if (string.IsNullOrEmpty(viCategoryName))
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Value Item Category Name' field is required. Skipping row.");
                                ++row;
                                continue;
                            }
                            var vi = manager.GetProperty("ValueItem");
                            var viName = vi.StringValue;
                            if (string.IsNullOrEmpty(viName))
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Value Item Name' field is required. Skipping row.");
                                ++row;
                                continue;
                            }
                            ValueItem valueItem = valueItems.Where(v => v.Name == viName).FirstOrDefault();
                            ValueItemCategory valueItemCategory = valueItemCategories.Where(vc => vc.Name == viCategoryName).FirstOrDefault();

                            //Create a vicaterogy if it does not find in valueItemCategory table.
                            if (valueItemCategory == null)
                            {
                                valueItemCategory = new ValueItemCategory
                                {
                                    Name = viCategoryName,
                                    CreatedUser = this._workContext.CurrentUser.Name,
                                    CreatedDateTime = DateTime.UtcNow,
                                    ModifiedUser = this._workContext.CurrentUser.Name,
                                    ModifiedDateTime = DateTime.UtcNow,
                                    Version = 1
                                };
                                valueItemCategories.Add(valueItemCategory);
                                try
                                {
                                    _valueItemCategoryDapperRepository.Insert(valueItemCategory);
                                }
                                catch (Exception ex)
                                {
                                    ++importResult.SkippedRecords;
                                    importResult.AddError(row, ex.Message);
                                    ++row;
                                    continue;
                                }
                            }
                            //Create a vi if it does not find in valueItem table.
                            if (valueItem == null)
                            {
                                valueItem = new ValueItem
                                {
                                    Name = viName,
                                    ValueItemCategoryId = valueItemCategory.Id,
                                    CreatedUser = this._workContext.CurrentUser.Name,
                                    CreatedDateTime = DateTime.UtcNow,
                                    ModifiedUser = this._workContext.CurrentUser.Name,
                                    ModifiedDateTime = DateTime.UtcNow,
                                    Version = 1
                                };
                                valueItems.Add(valueItem);
                                try
                                {
                                    _valueItemDapperRepository.Insert(valueItem);
                                    ++importResult.TotalRecords;
                                }
                                catch (Exception ex)
                                {
                                    ++importResult.SkippedRecords;
                                    importResult.AddError(row, ex.Message);
                                }
                            }
                            ++row;
                        }
                    }
                }
            }
            importResult.LastRunEndDateTime = DateTime.UtcNow;
            WriteLogFile(importProfile, importResult);
        }

        public virtual void ImportItemGroupFromXlsx(ImportProfile importProfile)
        {
            var rootPath = ConfigurationManager.AppSettings["ImportFolder"];
            var importProfileFolderPath = Path.Combine(rootPath, string.Format("Profile{0}", importProfile.Id));
            var importProfilePath = Path.Combine(importProfileFolderPath, importProfile.ImportFileName);
            var importResult = new ImportResult();
            importResult.LastRunStartDateTime = DateTime.UtcNow;
            if (File.Exists(importProfilePath))
            {
                using (var stream = new FileStream(importProfilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var xlPackage = new ExcelPackage(stream))
                    {
                        // get the first worksheet in the workbook
                        var worksheet = xlPackage.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet == null)
                        {
                            importResult.AddError(0, "No worksheet found");
                            throw new Exception("No worksheet found");
                        }

                        //the columns
                        var properties = new List<PropertyByName<ItemGroup>>();
                        var poz = 1;
                        while (true)
                        {
                            try
                            {
                                var cell = worksheet.Cells[1, poz];
                                var test = new PropertyByName<string>(cell.Value.ToString());
                                if (cell == null || cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString()))
                                    break;

                                poz += 1;
                                properties.Add(new PropertyByName<ItemGroup>(cell.Value.ToString()));
                            }
                            catch
                            {
                                break;
                            }
                        }
                        var manager = new PropertyManager<ItemGroup>(properties.ToArray());
                        var row = 2;

                        // get all itemGroups
                        var itemGroups = _itemGroupRepository.GetAll().ToList();
                        while (true)
                        {
                            var allColumnsAreEmpty = manager.GetProperties
                                .Select(property => worksheet.Cells[row, property.PropertyOrderPosition])
                                .All(cell => cell == null || cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString()));

                            if (allColumnsAreEmpty)
                                break;

                            manager.ReadFromXlsx(worksheet, row);
                            var itemGroupProperty = manager.GetProperty("Name");
                            var itemGroupName = itemGroupProperty.StringValue;
                            if (string.IsNullOrEmpty(itemGroupName))
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Name' field is required. Skipping row.");
                                ++row;
                                continue;
                            }

                            var itemGroup = itemGroups.Where(v => v.Name == itemGroupName).FirstOrDefault();
                            var isNew = itemGroup == null;
                            itemGroup = itemGroup ?? new ItemGroup();
                            foreach (var property in manager.GetProperties)
                            {
                                switch (property.PropertyName)
                                {
                                    case "Name":
                                        itemGroup.Name = property.StringValue;
                                        break;
                                    case "Description":
                                        itemGroup.Description = property.StringValue;
                                        break;
                                }
                            }

                            if (isNew)
                            {
                                try
                                {
                                    itemGroup.CreatedUser = this._workContext.CurrentUser.Name;
                                    itemGroup.CreatedDateTime = DateTime.UtcNow;
                                    itemGroup.ModifiedUser = this._workContext.CurrentUser.Name;
                                    itemGroup.ModifiedDateTime = DateTime.UtcNow;
                                    _itemGroupDapperRepository.Insert(itemGroup);
                                    // add new item Group into itemGroups cache
                                    itemGroups.Add(itemGroup);
                                    ++importResult.TotalRecords;
                                }
                                catch (Exception ex)
                                {
                                    ++importResult.SkippedRecords;
                                    importResult.AddError(row, ex.Message);
                                }
                            }
                            else
                            {
                                try
                                {
                                    itemGroup.ModifiedUser = this._workContext.CurrentUser.Name;
                                    itemGroup.ModifiedDateTime = DateTime.UtcNow;
                                    _itemGroupDapperRepository.Update(itemGroup);
                                    ++importResult.TotalRecords;

                                }
                                catch (Exception ex)
                                {
                                    ++importResult.SkippedRecords;
                                    importResult.AddError(row, ex.Message);
                                }
                            }

                            ++row;
                        }
                    }
                }
            }
            importResult.LastRunEndDateTime = DateTime.UtcNow;
            WriteLogFile(importProfile, importResult);
        }

        public virtual void ImportItemFromXlsx(ImportProfile importProfile)
        {
            var rootPath = ConfigurationManager.AppSettings["ImportFolder"];
            var importProfileFolderPath = Path.Combine(rootPath, string.Format("Profile{0}", importProfile.Id));
            var importProfilePath = Path.Combine(importProfileFolderPath, importProfile.ImportFileName);
            var importResult = new ImportResult();
            importResult.LastRunStartDateTime = DateTime.UtcNow;
            if (File.Exists(importProfilePath))
            {
                using (var stream = new FileStream(importProfilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var xlPackage = new ExcelPackage(stream))
                    {
                        // get the first worksheet in the workbook
                        var worksheet = xlPackage.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet == null)
                        {
                            importResult.AddError(0, "No worksheet found");
                            throw new Exception("No worksheet found");
                        }

                        //the columns
                        var properties = new List<PropertyByName<Item>>();
                        var poz = 1;
                        while (true)
                        {
                            try
                            {
                                var cell = worksheet.Cells[1, poz];
                                var test = new PropertyByName<string>(cell.Value.ToString());
                                if (cell == null || cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString()))
                                    break;

                                poz += 1;
                                properties.Add(new PropertyByName<Item>(cell.Value.ToString()));
                            }
                            catch
                            {
                                break;
                            }
                        }
                        var manager = new PropertyManager<Item>(properties.ToArray());
                        var row = 2;

                        // get all items, itemGroups, uoms, valueItems, manufacturers
                        var items = _itemRepository.GetAll().ToList();
                        var itemGroups = _itemGroupRepository.GetAll().ToList();
                        var uoms = _unitOfMeasureRepository.GetAll().ToList();
                        var valueItems = _valueItemRepository.GetAll().Where(v => v.ValueItemCategory.Name == "Item Status").ToList();
                        var manufacturers = _companyRepository.GetAll().ToList();
                        while (true)
                        {
                            var allColumnsAreEmpty = manager.GetProperties
                                .Select(property => worksheet.Cells[row, property.PropertyOrderPosition])
                                .All(cell => cell == null || cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString()));

                            if (allColumnsAreEmpty)
                                break;

                            manager.ReadFromXlsx(worksheet, row);

                            //get and validate item name
                            var itemNameProperty = manager.GetProperty("Name");
                            var itemName = itemNameProperty.StringValue;
                            if (string.IsNullOrEmpty(itemName))
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Name' field is required. Skipping row.");
                                ++row;
                                continue;
                            }

                            //get and validate item category name
                            var itemCategoryProperty = manager.GetProperty("ItemCategory");
                            var itemCategory = itemCategoryProperty.StringValue;
                            if (string.IsNullOrEmpty(itemCategory))
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Item Category Name' field is required. Skipping row.");
                                ++row;
                                continue;
                            }
                            var isItemCategoryValid = Enum.IsDefined(typeof(ItemCategory), itemCategory);
                            if (!isItemCategoryValid)
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Item Category' field is not valid. Skipping row.");
                                ++row;
                                continue;
                            }

                            //get and validate item group name
                            var itemGroupProperty = manager.GetProperty("ItemGroup");
                            var itemGroupName = itemGroupProperty.StringValue;
                            if (string.IsNullOrEmpty(itemGroupName))
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Item Group' field is required. Skipping row.");
                                ++row;
                                continue;
                            }
                            var itemGroup = itemGroups.Where(g => g.Name == itemGroupName).FirstOrDefault();
                            if (itemGroup == null)
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Item Group' field is not valid. Skipping row.");
                                ++row;
                                continue;
                            }

                            //get and validate uom name
                            var uomProperty = manager.GetProperty("UnitOfMeasure");
                            var uomName = uomProperty.StringValue;
                            if (string.IsNullOrEmpty(uomName))
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Unit Of Measure' field is required. Skipping row.");
                                ++row;
                                continue;
                            }
                            var uom = uoms.Where(g => g.Name == uomName).FirstOrDefault();
                            if (uom == null)
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Unit Of Measure' field is not valid. Skipping row.");
                                ++row;
                                continue;
                            }

                            //get and validate ItemStatus name
                            var itemStatusProperty = manager.GetProperty("ItemStatus");
                            var itemStatusName = itemStatusProperty.StringValue;
                            if (string.IsNullOrEmpty(itemStatusName))
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Item Status' field is required. Skipping row.");
                                ++row;
                                continue;
                            }
                            var itemStatus = valueItems.Where(g => g.Name == itemStatusName).FirstOrDefault();
                            if (itemStatus == null)
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Item Status' field is not valid. Skipping row.");
                                ++row;
                                continue;
                            }

                            var item = items.Where(v => v.Name == itemName).FirstOrDefault();
                            var isNew = item == null;
                            item = item ?? new Item();
                            foreach (var property in manager.GetProperties)
                            {
                                switch (property.PropertyName)
                                {
                                    case "Name":
                                        item.Name = property.StringValue;
                                        break;
                                    case "ItemCategory":
                                        item.ItemCategory = (int)Enum.Parse(typeof(ItemCategory), property.StringValue, true);
                                        break;
                                    case "Description":
                                        item.Description = property.StringValue;
                                        break;
                                    case "Barcode":
                                        item.Barcode = property.StringValue;
                                        break;
                                    case "UnitPrice":
                                        item.UnitPrice = property.DecimalValueNullable;
                                        break;
                                    case "ItemGroup":
                                        item.ItemGroupId = itemGroup.Id;
                                        break;
                                    case "Manufacturer":
                                        var manufacturer = manufacturers.Where(m => m.Name == property.StringValue).FirstOrDefault();
                                        item.ManufacturerId = manufacturer != null ? (long?)manufacturer.Id : null;
                                        break;
                                    case "UnitOfMeasure":
                                        item.UnitOfMeasureId = uom.Id;
                                        break;
                                    case "ItemStatus":
                                        item.ItemStatusId = itemStatus.Id;
                                        break;
                                }
                            }

                            if (isNew)
                            {
                                try
                                {
                                    item.CreatedUser = this._workContext.CurrentUser.Name;
                                    item.CreatedDateTime = DateTime.UtcNow;
                                    item.ModifiedUser = this._workContext.CurrentUser.Name;
                                    item.ModifiedDateTime = DateTime.UtcNow;
                                    _itemDapperRepository.Insert(item);
                                    // Add new item into items cache
                                    items.Add(item);
                                    ++importResult.TotalRecords;
                                }
                                catch (Exception ex)
                                {
                                    ++importResult.SkippedRecords;
                                    importResult.AddError(row, ex.Message);
                                }
                            }
                            else
                            {
                                try
                                {
                                    item.ModifiedUser = this._workContext.CurrentUser.Name;
                                    item.ModifiedDateTime = DateTime.UtcNow;
                                    _itemDapperRepository.Update(item);
                                    ++importResult.TotalRecords;

                                }
                                catch (Exception ex)
                                {
                                    ++importResult.SkippedRecords;
                                    importResult.AddError(row, ex.Message);
                                }
                            }

                            ++row;
                        }
                    }
                }
            }
            importResult.LastRunEndDateTime = DateTime.UtcNow;
            WriteLogFile(importProfile, importResult);
        }

        public virtual void ImportLocationFromXlsx(ImportProfile importProfile)
        {
            var rootPath = ConfigurationManager.AppSettings["ImportFolder"];
            var importProfileFolderPath = Path.Combine(rootPath, string.Format("Profile{0}", importProfile.Id));
            var importProfilePath = Path.Combine(importProfileFolderPath, importProfile.ImportFileName);
            var importResult = new ImportResult();
            importResult.LastRunStartDateTime = DateTime.UtcNow;
            if (File.Exists(importProfilePath))
            {
                using (var stream = new FileStream(importProfilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var xlPackage = new ExcelPackage(stream))
                    {
                        // get the first worksheet in the workbook
                        var worksheet = xlPackage.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet == null)
                        {
                            importResult.AddError(0, "No worksheet found");
                            throw new Exception("No worksheet found");
                        }

                        //the columns
                        var properties = new List<PropertyByName<Location>>();
                        var poz = 1;
                        while (true)
                        {
                            try
                            {
                                var cell = worksheet.Cells[1, poz];
                                var test = new PropertyByName<string>(cell.Value.ToString());
                                if (cell == null || cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString()))
                                    break;

                                poz += 1;
                                properties.Add(new PropertyByName<Location>(cell.Value.ToString()));
                            }
                            catch
                            {
                                break;
                            }
                        }
                        var manager = new PropertyManager<Location>(properties.ToArray());
                        var row = 2;

                        // get all sites, valueItems, locations
                        var sites = _siteRepository.GetAll().ToList();
                        var valueItems = _valueItemRepository.GetAll().ToList();
                        var locations = _locationRepository.GetAll().ToList();
                        while (true)
                        {
                            var allColumnsAreEmpty = manager.GetProperties
                                .Select(property => worksheet.Cells[row, property.PropertyOrderPosition])
                                .All(cell => cell == null || cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString()));

                            if (allColumnsAreEmpty)
                                break;

                            manager.ReadFromXlsx(worksheet, row);

                            //get and validate location from path
                            var locationPathProperty = manager.GetProperty("Path");
                            var locationPath = locationPathProperty.StringValue;
                            if (string.IsNullOrEmpty(locationPath))
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Path Name' field is required. Skipping row.");
                                ++row;
                                continue;
                            }
                            string[] paths = locationPath.Split('>');
                            string locationName = paths[paths.Length - 1].Trim();
                            if (string.IsNullOrEmpty(locationName))
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Location Name' field is required. Skipping row.");
                                ++row;
                                continue;
                            }
                            //Check to see that we have parentId or not.
                            Location parentLocation = null;
                            if (paths.Count() > 1)
                            {
                                var parentLocationName = paths[paths.Length - 2].Trim();
                                //-3: the length " > "
                                var parentLocationPath = locationPath.Substring(0, locationPath.Length - locationName.Length - 3).Trim();
                                parentLocation = locations.Where(l => l.HierarchyNamePath == parentLocationPath).FirstOrDefault();
                                if (parentLocation == null)
                                {
                                    ++importResult.SkippedRecords;
                                    importResult.AddError(row, "The 'Parent Location Name' value is not valid. Skipping row.");
                                    ++row;
                                    continue;
                                }
                            }

                            //get and validate site
                            var siteNameProperty = manager.GetProperty("Site");
                            var siteName = siteNameProperty.StringValue;
                            if (string.IsNullOrEmpty(siteName))
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Site Name' field is required. Skipping row.");
                                ++row;
                                continue;
                            }

                            var site = sites.Where(l => l.Name == siteName).FirstOrDefault();

                            if (site == null)
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Site Name' field is not valid. Skipping row.");
                                ++row;
                                continue;
                            }

                            //get and validate LocationType
                            var typeNameProperty = manager.GetProperty("Type");
                            var typeName = typeNameProperty.StringValue;
                            if (string.IsNullOrEmpty(typeName))
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Type Name' field is required. Skipping row.");
                                ++row;
                                continue;
                            }

                            var type = valueItems.Where(v => v.Name == typeName).FirstOrDefault();

                            if (type == null)
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Type Name' field is not valid. Skipping row.");
                                ++row;
                                continue;
                            }

                            //get and validate Status
                            var statusNameProperty = manager.GetProperty("Status");
                            var statusName = statusNameProperty.StringValue;
                            if (string.IsNullOrEmpty(statusName))
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Status Name' field is required. Skipping row.");
                                ++row;
                                continue;
                            }

                            var status = valueItems.Where(v => v.Name == statusName).FirstOrDefault();

                            if (status == null)
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Status Name' field is not valid. Skipping row.");
                                ++row;
                                continue;
                            }

                            // we dont require address info now
                            //get and validate address1
                            var address1Property = manager.GetProperty("Address1");
                            var address1Name = address1Property.StringValue;
                            //if (string.IsNullOrEmpty(address1Name))
                            //{
                            //    ++importResult.SkippedRecords;
                            //    importResult.AddError(row, "The 'Address1 Name' field is required. Skipping row.");
                            //    ++row;
                            //    continue;
                            //}

                            //get and validate phone number
                            var phoneProperty = manager.GetProperty("PhoneNumber");
                            var phoneNumber = phoneProperty.StringValue;
                            //if (string.IsNullOrEmpty(phoneNumber))
                            //{
                            //    ++importResult.SkippedRecords;
                            //    importResult.AddError(row, "The 'Phone Number' field is required. Skipping row.");
                            //    ++row;
                            //    continue;
                            //}

                            //get and validate email
                            var emailProperty = manager.GetProperty("Email");
                            var email = emailProperty.StringValue;
                            //if (string.IsNullOrEmpty(email))
                            //{
                            //    ++importResult.SkippedRecords;
                            //    importResult.AddError(row, "The 'Email' field is required. Skipping row.");
                            //    ++row;
                            //    continue;
                            //}                            

                            var location = locations.Where(c => c.Name == locationName).FirstOrDefault();
                            var isNew = location == null;
                            location = location ?? new Location();
                            var isNewAddress = location.Address == null;
                            var address = location.Address != null ? location.Address : new Address();
                            foreach (var property in manager.GetProperties)
                            {
                                switch (property.PropertyName)
                                {
                                    case "Path":
                                        location.Name = locationName;
                                        location.ParentId = parentLocation != null ? (long?)parentLocation.Id : null;
                                        location.HierarchyNamePath = locationPath;
                                        break;
                                    case "Site":
                                        location.SiteId = site.Id;
                                        break;
                                    case "Type":
                                        location.LocationTypeId = type.Id;
                                        break;
                                    case "Status":
                                        location.LocationStatusId = status.Id;
                                        break;
                                    case "Country":
                                        address.Country = property.StringValue;
                                        break;
                                    case "StateProvince":
                                        address.StateProvince = property.StringValue;
                                        break;
                                    case "City":
                                        address.City = property.StringValue;
                                        break;
                                    case "Address1":
                                        address.Address1 = property.StringValue;
                                        break;
                                    case "Address2":
                                        address.Address2 = property.StringValue;
                                        break;
                                    case "ZipPostalCode":
                                        address.ZipPostalCode = property.StringValue;
                                        break;
                                    case "FaxNumber":
                                        address.FaxNumber = property.StringValue;
                                        break;
                                    case "Email":
                                        address.Email = property.StringValue;
                                        break;
                                }
                            }

                            if (isNewAddress)
                            {
                                try
                                {
                                    address.CreatedUser = this._workContext.CurrentUser.Name;
                                    address.CreatedDateTime = DateTime.UtcNow;
                                    address.ModifiedUser = this._workContext.CurrentUser.Name;
                                    address.ModifiedDateTime = DateTime.UtcNow;
                                    _addressDapperRepository.Insert(address);
                                }
                                catch (Exception ex)
                                {
                                    ++importResult.SkippedRecords;
                                    importResult.AddError(row, ex.Message);
                                    ++row;
                                    continue;
                                }
                            }
                            else
                            {
                                try
                                {
                                    address.ModifiedUser = this._workContext.CurrentUser.Name;
                                    address.ModifiedDateTime = DateTime.UtcNow;
                                    _addressDapperRepository.Update(address);

                                }
                                catch (Exception ex)
                                {
                                    ++importResult.SkippedRecords;
                                    importResult.AddError(row, ex.Message);
                                    ++row;
                                    continue;
                                }
                            }
                            location.AddressId = address.Id;
                            if (isNew)
                            {
                                try
                                {

                                    location.CreatedUser = this._workContext.CurrentUser.Name;
                                    location.CreatedDateTime = DateTime.UtcNow;
                                    location.ModifiedUser = this._workContext.CurrentUser.Name;
                                    location.ModifiedDateTime = DateTime.UtcNow;
                                    _locationDapperRepository.Insert(location);
                                    location.HierarchyIdPath = location.ParentId != null ? parentLocation.HierarchyIdPath + " > " + location.Id.ToString() : location.Id.ToString();
                                    _locationDapperRepository.Update(location);
                                    ++importResult.TotalRecords;
                                }
                                catch (Exception ex)
                                {
                                    ++importResult.SkippedRecords;
                                    importResult.AddError(row, ex.Message);
                                }
                            }
                            else
                            {
                                try
                                {
                                    location.ModifiedUser = this._workContext.CurrentUser.Name;
                                    location.ModifiedDateTime = DateTime.UtcNow;
                                    location.HierarchyIdPath = location.ParentId != null ? parentLocation.HierarchyIdPath + " > " + location.Id.ToString() : location.Id.ToString();
                                    _locationDapperRepository.Update(location);
                                    ++importResult.TotalRecords;

                                }
                                catch (Exception ex)
                                {
                                    ++importResult.SkippedRecords;
                                    importResult.AddError(row, ex.Message);
                                }
                            }

                            ++row;
                        }
                    }
                }
            }
            importResult.LastRunEndDateTime = DateTime.UtcNow;
            WriteLogFile(importProfile, importResult);
        }

        public virtual void ImportMeterFromXlsx(ImportProfile importProfile)
        {
            var rootPath = ConfigurationManager.AppSettings["ImportFolder"];
            var importProfileFolderPath = Path.Combine(rootPath, string.Format("Profile{0}", importProfile.Id));
            var importProfilePath = Path.Combine(importProfileFolderPath, importProfile.ImportFileName);
            var importResult = new ImportResult();
            importResult.LastRunStartDateTime = DateTime.UtcNow;
            if (File.Exists(importProfilePath))
            {
                using (var stream = new FileStream(importProfilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var xlPackage = new ExcelPackage(stream))
                    {
                        // get the first worksheet in the workbook
                        var worksheet = xlPackage.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet == null)
                        {
                            importResult.AddError(0, "No worksheet found");
                            throw new Exception("No worksheet found");
                        }

                        //the columns
                        var properties = new List<PropertyByName<Meter>>();
                        var poz = 1;
                        while (true)
                        {
                            try
                            {
                                var cell = worksheet.Cells[1, poz];
                                var test = new PropertyByName<string>(cell.Value.ToString());
                                if (cell == null || cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString()))
                                    break;

                                poz += 1;
                                properties.Add(new PropertyByName<Meter>(cell.Value.ToString()));
                            }
                            catch
                            {
                                break;
                            }
                        }
                        var manager = new PropertyManager<Meter>(properties.ToArray());
                        var row = 2;

                        // get all meters, valueItems, uoms
                        var meters = _meterRepository.GetAll().ToList();
                        var valueItems = _valueItemRepository.GetAll().ToList();
                        var uoms = _unitOfMeasureRepository.GetAll().ToList();
                        while (true)
                        {
                            var allColumnsAreEmpty = manager.GetProperties
                                .Select(property => worksheet.Cells[row, property.PropertyOrderPosition])
                                .All(cell => cell == null || cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString()));

                            if (allColumnsAreEmpty)
                                break;

                            manager.ReadFromXlsx(worksheet, row);

                            //get and validate location from path
                            var meterProperty = manager.GetProperty("Name");
                            var meterName = meterProperty.StringValue;
                            if (string.IsNullOrEmpty(meterName))
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Meter Name' field is required. Skipping row.");
                                ++row;
                                continue;
                            }

                            //get and validate LocationType
                            var typeNameProperty = manager.GetProperty("Type");
                            var typeName = typeNameProperty.StringValue;
                            if (string.IsNullOrEmpty(typeName))
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Type Name' field is required. Skipping row.");
                                ++row;
                                continue;
                            }

                            var type = valueItems.Where(v => v.Name == typeName).FirstOrDefault();

                            if (type == null)
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Type Name' field is not valid. Skipping row.");
                                ++row;
                                continue;
                            }

                            //get and validate UnitOfMeasure
                            var uomNameProperty = manager.GetProperty("UnitOfMeasure");
                            var uomName = uomNameProperty.StringValue;
                            if (string.IsNullOrEmpty(uomName))
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Unit Of Measure Name' field is required. Skipping row.");
                                ++row;
                                continue;
                            }

                            var uom = uoms.Where(v => v.Name == uomName).FirstOrDefault();

                            if (uom == null)
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Unit Of Measure Name' field is not valid. Skipping row.");
                                ++row;
                                continue;
                            }

                            var meter = meters.Where(c => c.Name == meterName).FirstOrDefault();
                            var isNew = meter == null;
                            meter = meter ?? new Meter();
                            foreach (var property in manager.GetProperties)
                            {
                                switch (property.PropertyName)
                                {
                                    case "Name":
                                        meter.Name = property.StringValue;
                                        break;
                                    case "Type":
                                        meter.MeterTypeId = type.Id;
                                        break;
                                    case "Description":
                                        meter.Description = property.StringValue;
                                        break;
                                    case "UnitOfMeasure":
                                        meter.UnitOfMeasureId = uom.Id;
                                        break;
                                }
                            }

                            if (isNew)
                            {
                                try
                                {

                                    meter.CreatedUser = this._workContext.CurrentUser.Name;
                                    meter.CreatedDateTime = DateTime.UtcNow;
                                    meter.ModifiedUser = this._workContext.CurrentUser.Name;
                                    meter.ModifiedDateTime = DateTime.UtcNow;
                                    _meterDapperRepository.Insert(meter);
                                    // add new meter into meters cache
                                    meters.Add(meter);
                                    ++importResult.TotalRecords;
                                }
                                catch (Exception ex)
                                {
                                    ++importResult.SkippedRecords;
                                    importResult.AddError(row, ex.Message);
                                }
                            }
                            else
                            {
                                try
                                {
                                    meter.ModifiedUser = this._workContext.CurrentUser.Name;
                                    meter.ModifiedDateTime = DateTime.UtcNow;
                                    _meterDapperRepository.Update(meter);
                                    ++importResult.TotalRecords;

                                }
                                catch (Exception ex)
                                {
                                    ++importResult.SkippedRecords;
                                    importResult.AddError(row, ex.Message);
                                }
                            }

                            ++row;
                        }
                    }
                }
            }
            importResult.LastRunEndDateTime = DateTime.UtcNow;
            WriteLogFile(importProfile, importResult);
        }

        public virtual void ImportCompanyFromXlsx(ImportProfile importProfile)
        {
            var rootPath = ConfigurationManager.AppSettings["ImportFolder"];
            var importProfileFolderPath = Path.Combine(rootPath, string.Format("Profile{0}", importProfile.Id));
            var importProfilePath = Path.Combine(importProfileFolderPath, importProfile.ImportFileName);
            var importResult = new ImportResult();
            importResult.LastRunStartDateTime = DateTime.UtcNow;
            if (File.Exists(importProfilePath))
            {
                using (var stream = new FileStream(importProfilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var xlPackage = new ExcelPackage(stream))
                    {
                        // get the first worksheet in the workbook
                        var worksheet = xlPackage.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet == null)
                        {
                            importResult.AddError(0, "No worksheet found");
                            throw new Exception("No worksheet found");
                        }

                        //the columns
                        var properties = new List<PropertyByName<Company>>();
                        var poz = 1;
                        while (true)
                        {
                            try
                            {
                                var cell = worksheet.Cells[1, poz];
                                var test = new PropertyByName<string>(cell.Value.ToString());
                                if (cell == null || cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString()))
                                    break;

                                poz += 1;
                                properties.Add(new PropertyByName<Company>(cell.Value.ToString()));
                            }
                            catch
                            {
                                break;
                            }
                        }
                        var manager = new PropertyManager<Company>(properties.ToArray());
                        var row = 2;

                        // get all companies, valueItems, currencies
                        var companies = _companyRepository.GetAll().ToList();
                        var valueItems = _valueItemRepository.GetAll().ToList();
                        var currencies = _currencyRepository.GetAll().ToList();
                        var addresses = _addressRepository.GetAll().ToList();
                        var contacts = _contactRepository.GetAll().ToList();
                        while (true)
                        {
                            var allColumnsAreEmpty = manager.GetProperties
                                .Select(property => worksheet.Cells[row, property.PropertyOrderPosition])
                                .All(cell => cell == null || cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString()));

                            if (allColumnsAreEmpty)
                                break;

                            manager.ReadFromXlsx(worksheet, row);

                            //get and validate location from path
                            var companyNameProperty = manager.GetProperty("Name");
                            var companyName = companyNameProperty.StringValue;
                            if (string.IsNullOrEmpty(companyName))
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Company Name' field is required. Skipping row.");
                                ++row;
                                continue;
                            }

                            //get and validate CompanyType
                            var typeNameProperty = manager.GetProperty("Type");
                            var typeName = typeNameProperty.StringValue;
                            if (string.IsNullOrEmpty(typeName))
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Type Name' field is required. Skipping row.");
                                ++row;
                                continue;
                            }

                            var type = valueItems.Where(v => v.Name == typeName).FirstOrDefault();

                            if (type == null)
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Type Name' field is not valid. Skipping row.");
                                ++row;
                                continue;
                            }

                            //get and validate Currency
                            var currencyNameProperty = manager.GetProperty("Currency");
                            var currencyName = currencyNameProperty.StringValue;
                            var currency = _currencyRepository.GetAll().Where(v => v.Name == currencyName).FirstOrDefault();

                            if (currency == null)
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Currency Name' field is not valid. Skipping row.");
                                ++row;
                                continue;
                            }

                            var company = companies.Where(c => c.Name == companyName).FirstOrDefault();
                            var isNew = company == null;
                            company = company ?? new Company();

                            foreach (var property in manager.GetProperties)
                            {
                                switch (property.PropertyName)
                                {
                                    case "Name":
                                        company.Name = property.StringValue;
                                        break;
                                    case "Type":
                                        company.CompanyTypeId = type.Id;
                                        break;
                                    case "Description":
                                        company.Description = property.StringValue;
                                        break;
                                    case "Currency":
                                        company.CurrencyId = currency != null ? (long?)currency.Id : null;
                                        break;
                                    case "Website":
                                        company.Website = property.StringValue;
                                        break;

                                }
                            }

                            //Validate Address
                            Address address = null;
                            var address1Name = manager.GetProperty("Address1").StringValue;
                            if (string.IsNullOrEmpty(address1Name))
                            {
                                importResult.AddWarning(row, "The 'Address1 Name' field is not valid. Skipping row.");
                            }
                            var phoneNumber = manager.GetProperty("PhoneNumber").StringValue;
                            if (string.IsNullOrEmpty(phoneNumber))
                            {
                                importResult.AddWarning(row, "The 'Phone Number' field is not valid. Skipping row.");
                            }
                            var email = manager.GetProperty("Email").StringValue;
                            if (string.IsNullOrEmpty(email))
                            {
                                importResult.AddWarning(row, "The 'Email' field is not valid. Skipping row.");
                            }
                            address = addresses.Where(a => a.Address1 == address1Name && a.PhoneNumber == phoneNumber && a.Email == email).FirstOrDefault();
                            address = address ?? new Address();

                            foreach (var property in manager.GetProperties)
                            {
                                switch (property.PropertyName)
                                {
                                    case "Country":
                                        address.Country = property.StringValue;
                                        break;
                                    case "StateProvince":
                                        address.StateProvince = property.StringValue;
                                        break;
                                    case "City":
                                        address.City = property.StringValue;
                                        break;
                                    case "Address1":
                                        address.Address1 = property.StringValue;
                                        break;
                                    case "Address2":
                                        address.Address2 = property.StringValue;
                                        break;
                                    case "ZipPostalCode":
                                        address.ZipPostalCode = property.StringValue;
                                        break;
                                    case "PhoneNumber":
                                        address.PhoneNumber = property.StringValue;
                                        break;
                                    case "FaxNumber":
                                        address.FaxNumber = property.StringValue;
                                        break;
                                    case "Email":
                                        address.Email = property.StringValue;
                                        break;

                                }
                            }
                            try
                            {
                                _addressDapperRepository.Insert(address);
                            }
                            catch (Exception ex)
                            {
                                importResult.AddError(row, ex.Message);
                            }

                            //Validate Contact
                            Contact contact = null;
                            var contactName = manager.GetProperty("ContactName").StringValue;
                            if (string.IsNullOrEmpty(contactName))
                            {
                                importResult.AddWarning(row, "The 'Contact Name' field is not valid. Skipping row.");
                            }
                            contact = contacts.Where(c => c.Name == contactName).FirstOrDefault();
                            contact = contact ?? new Contact();

                            foreach (var property in manager.GetProperties)
                            {
                                switch (property.PropertyName)
                                {
                                    case "ContactName":
                                        contact.Name = property.StringValue;
                                        break;
                                    case "ContactPosition":
                                        contact.Position = property.StringValue;
                                        break;
                                    case "ContactEmail":
                                        contact.Email = property.StringValue;
                                        break;
                                    case "ContactPhone":
                                        contact.Phone = property.StringValue;
                                        break;
                                    case "ContactFaxNumber":
                                        contact.Fax = property.StringValue;
                                        break;
                                }
                            }
                            try
                            {
                                _contactDapperRepository.Insert(contact);
                            }
                            catch (Exception ex)
                            {
                                importResult.AddError(row, ex.Message);
                            }

                            if (isNew)
                            {
                                try
                                {
                                    company.CreatedUser = this._workContext.CurrentUser.Name;
                                    company.CreatedDateTime = DateTime.UtcNow;
                                    company.ModifiedUser = this._workContext.CurrentUser.Name;
                                    company.ModifiedDateTime = DateTime.UtcNow;

                                    _companyDapperRepository.Insert(company);
                                    // add new company into companies cache
                                    companies.Add(company);
                                    ++importResult.TotalRecords;
                                    if (address != null)
                                    {
                                        try
                                        {
                                            var sqlQuery = string.Format(INSERT_COMPANY_ADDRESS, company.Id, address.Id);
                                            using (var connection = _dapperContext.GetOpenConnection())
                                            {
                                                connection.Query(sqlQuery);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            importResult.AddError(row, ex.Message);
                                        }
                                    }
                                    try
                                    {
                                        //Update Contact
                                        contact.CompanyId = company.Id;
                                        _contactDapperRepository.Update(contact);
                                    }
                                    catch (Exception ex)
                                    {
                                        importResult.AddError(row, ex.Message);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ++importResult.SkippedRecords;
                                    importResult.AddError(row, ex.Message);
                                }
                            }
                            else
                            {
                                try
                                {
                                    company.ModifiedUser = this._workContext.CurrentUser.Name;
                                    company.ModifiedDateTime = DateTime.UtcNow;
                                    try
                                    {
                                        contact.CompanyId = company.Id;
                                        _contactDapperRepository.Update(contact);
                                    }
                                    catch (Exception ex)
                                    {
                                        importResult.AddError(row, ex.Message);
                                    }
                                    _companyDapperRepository.Update(company);
                                    ++importResult.TotalRecords;
                                    //Insert Company_Address
                                    if (address != null)
                                    {
                                        try
                                        {

                                            var sqlQuery = string.Format(UPDATE_COMPANY_ADDRESS, company.Id, address.Id);
                                            using (var connection = _dapperContext.GetOpenConnection())
                                            {
                                                connection.Query(sqlQuery);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            importResult.AddError(row, ex.Message);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ++importResult.SkippedRecords;

                                    importResult.AddError(row, ex.Message);
                                }
                            }

                            ++row;
                        }
                    }
                }
            }
            importResult.LastRunEndDateTime = DateTime.UtcNow;
            WriteLogFile(importProfile, importResult);
        }

        public virtual void ImportStoreFromXlsx(ImportProfile importProfile)
        {
            var rootPath = ConfigurationManager.AppSettings["ImportFolder"];
            var importProfileFolderPath = Path.Combine(rootPath, string.Format("Profile{0}", importProfile.Id));
            var importProfilePath = Path.Combine(importProfileFolderPath, importProfile.ImportFileName);
            var importResult = new ImportResult();
            importResult.LastRunStartDateTime = DateTime.UtcNow;
            if (File.Exists(importProfilePath))
            {
                using (var stream = new FileStream(importProfilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var xlPackage = new ExcelPackage(stream))
                    {
                        // get the first worksheet in the workbook
                        var worksheet = xlPackage.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet == null)
                        {
                            importResult.AddError(0, "No worksheet found");
                            throw new Exception("No worksheet found");
                        }

                        //the columns
                        var properties = new List<PropertyByName<Store>>();
                        var poz = 1;
                        while (true)
                        {
                            try
                            {
                                var cell = worksheet.Cells[1, poz];
                                var test = new PropertyByName<string>(cell.Value.ToString());
                                if (cell == null || cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString()))
                                    break;

                                poz += 1;
                                properties.Add(new PropertyByName<Store>(cell.Value.ToString()));
                            }
                            catch
                            {
                                break;
                            }
                        }
                        var manager = new PropertyManager<Store>(properties.ToArray());
                        var row = 2;

                        // get all stores, valueItems, storeTypes, sites, locations
                        var stores = _storeRepository.GetAll().ToList();
                        var storeTypes = _valueItemRepository.GetAll().Where(s => s.ValueItemCategory.Name == "Store Type").ToList();
                        var sites = _siteRepository.GetAll().ToList();
                        var locations = _locationRepository.GetAll().ToList();
                        while (true)
                        {
                            var allColumnsAreEmpty = manager.GetProperties
                                .Select(property => worksheet.Cells[row, property.PropertyOrderPosition])
                                .All(cell => cell == null || cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString()));

                            if (allColumnsAreEmpty)
                                break;

                            manager.ReadFromXlsx(worksheet, row);

                            //get and validate store name
                            var storeNameProperty = manager.GetProperty("Name");
                            var storeName = storeNameProperty.StringValue;
                            if (string.IsNullOrEmpty(storeName))
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Store Name' field is required. Skipping row.");
                                ++row;
                                continue;
                            }
                            //get and validate StoreType
                            var typeNameProperty = manager.GetProperty("Type");
                            var typeName = typeNameProperty.StringValue;
                            if (string.IsNullOrEmpty(typeName))
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Type Name' field is required. Skipping row.");
                                ++row;
                                continue;
                            }

                            var type = storeTypes.Where(v => v.Name == typeName).FirstOrDefault();

                            if (type == null)
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Type Name' field is not valid. Skipping row.");
                                ++row;
                                continue;
                            }

                            //get and validate site name
                            var siteNameProperty = manager.GetProperty("Site");
                            var siteName = siteNameProperty.StringValue;
                            var site = sites.Where(v => v.Name == siteName).FirstOrDefault();

                            if (site == null)
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Site Name' field is not valid. Skipping row.");
                                ++row;
                                continue;
                            }

                            //get and validate location name
                            var locationNameProperty = manager.GetProperty("Location");
                            var locationName = locationNameProperty.StringValue;
                            var location = locations.Where(v => v.Name == locationName).FirstOrDefault();

                            if (location != null && location.SiteId != site.Id)
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Location Name' field does not belong to this site. Skipping row.");
                                ++row;
                                continue;
                            }

                            var store = stores.Where(c => c.Name == storeName).FirstOrDefault();
                            var isNew = store == null;
                            store = store ?? new Store();

                            foreach (var property in manager.GetProperties)
                            {
                                switch (property.PropertyName)
                                {
                                    case "Name":
                                        store.Name = property.StringValue;
                                        break;
                                    case "Type":
                                        store.StoreTypeId = type.Id;
                                        break;
                                    case "Description":
                                        store.Description = property.StringValue;
                                        break;
                                    case "Site":
                                        store.SiteId = site.Id;
                                        break;
                                    case "Location":
                                        store.LocationId = location != null ? (long?)location.Id : null;
                                        break;
                                    case "IsActive":
                                        store.IsActive = property.BooleanValue;
                                        break;

                                }
                            }

                            if (isNew)
                            {
                                try
                                {
                                    store.CreatedUser = this._workContext.CurrentUser.Name;
                                    store.CreatedDateTime = DateTime.UtcNow;
                                    store.ModifiedUser = this._workContext.CurrentUser.Name;
                                    store.ModifiedDateTime = DateTime.UtcNow;
                                    _storeDapperRepository.Insert(store);

                                    // Add new store into stores cache
                                    stores.Add(store);

                                    //Create a default store locator
                                    var defStoreLocator = new StoreLocator();
                                    defStoreLocator.StoreId = store.Id;
                                    defStoreLocator.IsDefault = true;
                                    defStoreLocator.CreatedUser = this._workContext.CurrentUser.Name;
                                    defStoreLocator.CreatedDateTime = DateTime.UtcNow;
                                    defStoreLocator.ModifiedUser = this._workContext.CurrentUser.Name;
                                    defStoreLocator.ModifiedDateTime = DateTime.UtcNow;
                                    defStoreLocator.Name = "DEF";
                                    try
                                    {
                                        _storeLocatorDapperRepository.Insert(defStoreLocator);
                                    }
                                    catch (Exception ex)
                                    {
                                        importResult.AddWarning(row, ex.Message);
                                    }
                                    ++importResult.TotalRecords;
                              
                                }
                                catch (Exception ex)
                                {
                                    ++importResult.SkippedRecords;
                                    importResult.AddError(row, ex.Message);
                                }
                            }
                            else
                            {
                                try
                                {
                                    store.ModifiedUser = this._workContext.CurrentUser.Name;
                                    store.ModifiedDateTime = DateTime.UtcNow;
                                    _storeDapperRepository.Update(store);
                                    ++importResult.TotalRecords;
                                }
                                catch (Exception ex)
                                {
                                    ++importResult.SkippedRecords;

                                    importResult.AddError(row, ex.Message);
                                }
                            }

                            ++row;
                        }
                    }
                }
            }
            importResult.LastRunEndDateTime = DateTime.UtcNow;
            WriteLogFile(importProfile, importResult);
        }

        public virtual void ImportAssetFromXlsx(ImportProfile importProfile)
        {
            var rootPath = ConfigurationManager.AppSettings["ImportFolder"];
            var importProfileFolderPath = Path.Combine(rootPath, string.Format("Profile{0}", importProfile.Id));
            var importProfilePath = Path.Combine(importProfileFolderPath, importProfile.ImportFileName);
            var importResult = new ImportResult();
            importResult.LastRunStartDateTime = DateTime.UtcNow;
            if (File.Exists(importProfilePath))
            {
                using (var stream = new FileStream(importProfilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var xlPackage = new ExcelPackage(stream))
                    {
                        // get the first worksheet in the workbook
                        var worksheet = xlPackage.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet == null)
                        {
                            importResult.AddError(0, "No worksheet found");
                            throw new Exception("No worksheet found");
                        }

                        //the columns
                        var properties = new List<PropertyByName<Asset>>();
                        var poz = 1;
                        while (true)
                        {
                            try
                            {
                                var cell = worksheet.Cells[1, poz];
                                cell.Style.Numberformat.Format = "dd/MM/yyyy HH:mm";
                                var test = new PropertyByName<string>(cell.Value.ToString());
                                if (cell == null || cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString()))
                                    break;

                                poz += 1;
                                properties.Add(new PropertyByName<Asset>(cell.Value.ToString()));
                            }
                            catch
                            {
                                break;
                            }
                        }
                        var manager = new PropertyManager<Asset>(properties.ToArray());
                        var row = 2;

                        // get all stores, valueItems, assetTypes, assetStatus, sites, locations, companies
                        var assets = _assetRepository.GetAll().ToList();
                        var valueItems = _valueItemRepository.GetAll().ToList();
                        var assetTypes = valueItems.Where(v => v.ValueItemCategory.Name == "Asset Type").ToList();
                        var assetStatus = valueItems.Where(v => v.ValueItemCategory.Name == "Asset Status").ToList();
                        var sites = _siteRepository.GetAll().ToList();
                        var locations = _locationRepository.GetAll().ToList();
                        var companies = _companyRepository.GetAll().ToList();
                        while (true)
                        {
                            var allColumnsAreEmpty = manager.GetProperties
                                .Select(property => worksheet.Cells[row, property.PropertyOrderPosition])
                                .All(cell => cell == null || cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString()));

                            if (allColumnsAreEmpty)
                                break;

                            manager.ReadFromXlsx(worksheet, row);

                            //get and validate asset name
                            var assetNameProperty = manager.GetProperty("Name");
                            var assetName = assetNameProperty.StringValue;
                            if (string.IsNullOrEmpty(assetName))
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Asset Name' field is required. Skipping row.");
                                ++row;
                                continue;
                            }
                            //validate parentId
                            var parentAssetNameProperty = manager.GetProperty("Parent");
                            var parentAssetName = parentAssetNameProperty.StringValue;
                            Asset parent = null;
                            if (!string.IsNullOrEmpty(parentAssetName))
                            {
                                parent = assets.Where(v => v.Name == parentAssetName).FirstOrDefault();
                                if (parent == null)
                                {
                                    ++importResult.SkippedRecords;
                                    importResult.AddError(row, "The 'Parent Name' field is not valid. Skipping row.");
                                    ++row;
                                    continue;
                                }
                            }

                            //get and validate asset type
                            var typeNameProperty = manager.GetProperty("Type");
                            var typeName = typeNameProperty.StringValue;
                            if (string.IsNullOrEmpty(typeName))
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Type Name' field is required. Skipping row.");
                                ++row;
                                continue;
                            }

                            var type = assetTypes.Where(v => v.Name == typeName).FirstOrDefault();

                            if (type == null)
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Type Name' field is not valid. Skipping row.");
                                ++row;
                                continue;
                            }

                            //get and validate asset status
                            var statusNameProperty = manager.GetProperty("Status");
                            var statusName = statusNameProperty.StringValue;
                            if (string.IsNullOrEmpty(statusName))
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Status Name' field is required. Skipping row.");
                                ++row;
                                continue;
                            }

                            var status = assetStatus.Where(v => v.Name == statusName).FirstOrDefault();

                            if (status == null)
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Status Name' field is not valid. Skipping row.");
                                ++row;
                                continue;
                            }

                            //get and validate site name
                            var siteNameProperty = manager.GetProperty("Site");
                            var siteName = siteNameProperty.StringValue;
                            var site = sites.Where(v => v.Name == siteName).FirstOrDefault();

                            if (site == null)
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Site Name' field is not valid. Skipping row.");
                                ++row;
                                continue;
                            }

                            //get and validate location name
                            var locationNameProperty = manager.GetProperty("Location");
                            var locationName = locationNameProperty.StringValue;
                            var location = locations.Where(v => v.HierarchyNamePath == locationName).FirstOrDefault();

                            if (location != null && location.SiteId != site.Id)
                            {
                                ++importResult.SkippedRecords;
                                importResult.AddError(row, "The 'Location Name' field does not belong to this site. Skipping row.");
                                ++row;
                                continue;
                            }

                            //validate manufacturer
                            var manufacturerNameProperty = manager.GetProperty("Manufacturer");
                            var manufacturerName = manufacturerNameProperty.StringValue;
                            Company manufacturer = null;
                            if (!string.IsNullOrEmpty(manufacturerName))
                            {
                                manufacturer = companies.Where(v => v.Name == manufacturerName).FirstOrDefault();

                                if (manufacturer == null)
                                {
                                    ++importResult.SkippedRecords;
                                    importResult.AddError(row, "The 'Manufacturer Name' field is not valid. Skipping row.");
                                    ++row;
                                    continue;
                                }
                            }

                            //validate vendor
                            var vendorNameProperty = manager.GetProperty("Vendor");
                            var vendorName = vendorNameProperty.StringValue;
                            Company vendor = null;
                            if (!string.IsNullOrEmpty(vendorName))
                            {
                                vendor = companies.Where(v => v.Name == vendorName).FirstOrDefault();
                                if (vendor == null)
                                {
                                    ++importResult.SkippedRecords;
                                    importResult.AddError(row, "The 'Vendor Name' field is not valid. Skipping row.");
                                    ++row;
                                    continue;
                                }
                            }
                            var asset = assets.Where(c => c.Name == assetName).FirstOrDefault();
                            var isNew = asset == null;
                            asset = asset ?? new Asset();

                            foreach (var property in manager.GetProperties)
                            {
                                switch (property.PropertyName)
                                {
                                    case "Name":
                                        asset.Name = property.StringValue;
                                        break;
                                    case "Type":
                                        asset.AssetTypeId = type.Id;
                                        break;
                                    case "Status":
                                        asset.AssetStatusId = status.Id;
                                        break;
                                    case "Parent":
                                        asset.ParentId = parent != null ? (long?)parent.Id : null;
                                        break;
                                    case "SerialNumber":
                                        asset.SerialNumber = property.StringValue;
                                        break;
                                    case "Manufacturer":
                                        asset.ManufacturerId = manufacturer != null ? (long?)manufacturer.Id : null;
                                        break;
                                    case "Vendor":
                                        asset.VendorId = vendor != null ? (long?)vendor.Id : null;
                                        break;
                                    case "Site":
                                        asset.SiteId = site != null ? (long?)site.Id : null;
                                        break;
                                    case "Location":
                                        asset.LocationId = location != null ? (long?)location.Id : null;
                                        break;
                                    case "InstallationDate":
                                        asset.InstallationDate = property.DateTimeNullable;
                                        break;
                                    case "InstallationCost":
                                        asset.InstallationCost = property.DecimalValueNullable;
                                        break;
                                    case "Price":
                                        asset.PurchasePrice = property.DecimalValueNullable;
                                        break;
                                    case "Period":
                                        asset.Period = property.IntValue;
                                        break;
                                    case "WarrantyStartDate":
                                        asset.WarrantyStartDate = property.DateTimeNullable;
                                        break;
                                    case "WarrantyEndDate":
                                        asset.WarrantyEndDate = property.DateTimeNullable;
                                        break;
                                }
                            }

                            //update HierarchyNamePath 
                            asset.HierarchyNamePath = parent != null ? parent.HierarchyNamePath + " > " + asset.Name: asset.Name;
                            if (isNew)
                            {
                                try
                                {
                                    asset.CreatedUser = this._workContext.CurrentUser.Name;
                                    asset.CreatedDateTime = DateTime.UtcNow;
                                    asset.ModifiedUser = this._workContext.CurrentUser.Name;
                                    asset.ModifiedDateTime = DateTime.UtcNow;
                                    _assetDapperRepository.Insert(asset);
                                    asset.HierarchyIdPath = parent != null ? parent.HierarchyIdPath + " > " + asset.Id.ToString() : asset.Id.ToString();
                                    _assetDapperRepository.Update(asset);
                                    ++importResult.TotalRecords;
                                    // Add new asset into assets cache
                                    assets.Add(asset);
                                }
                                catch (Exception ex)
                                {
                                    ++importResult.SkippedRecords;
                                    importResult.AddError(row, ex.Message);
                                }
                            }
                            else
                            {
                                try
                                {
                                    asset.ModifiedUser = this._workContext.CurrentUser.Name;
                                    asset.ModifiedDateTime = DateTime.UtcNow;
                                    asset.HierarchyIdPath = parent != null ? parent.HierarchyIdPath + " > " + asset.Id.ToString() : asset.Id.ToString();
                                    _assetDapperRepository.Update(asset);
                                    ++importResult.TotalRecords;
                                }
                                catch (Exception ex)
                                {
                                    ++importResult.SkippedRecords;

                                    importResult.AddError(row, ex.Message);
                                }
                            }

                            ++row;
                        }
                    }
                }
            }
            importResult.LastRunEndDateTime = DateTime.UtcNow;
            WriteLogFile(importProfile, importResult);
        }

        private string GenerateLogHead(ImportProfile importProfile)
        {
            var logHead = new StringBuilder();
            logHead.AppendLine();
            logHead.AppendLine(new string('-', 40));
            logHead.AppendLine("BaseEam:\t\tv." + BaseEamVersion.CurrentVersion);
            logHead.AppendLine(string.Format("Import profile:\t\t" + "Profile{0}", importProfile.Id));
            logHead.AppendLine("Entity:\t\t\t" + importProfile.EntityType);
            logHead.AppendLine("File:\t\t\t" + Path.GetFileName(importProfile.ImportFileName));

            var user = _workContext.CurrentUser;
            logHead.Append("Executed by:\t\t" + (!string.IsNullOrEmpty(user.Email) ? user.Email : "BackgroundTask"));
            return logHead.ToString();
        }

        private void WriteLogFile(ImportProfile importProfile, ImportResult importResult)
        {
            var rootPath = ConfigurationManager.AppSettings["ImportFolder"];
            var importProfileFolderPath = Path.Combine(rootPath, string.Format("Profile{0}", importProfile.Id));
            var logFilePath = Path.Combine(importProfileFolderPath, "log.txt");

            File.Delete(logFilePath);

            var append = File.Exists(logFilePath);

            var infoMessage = new StringBuilder();
            var errorMessage = new StringBuilder();
            var warningMessage = new StringBuilder();
            var result = importResult;
            foreach (var message in result.Messages)
            {
                if (message.MessageType == ImportMessageType.Error)
                {
                    errorMessage.AppendLine(message.Message);
                }
                else if (message.MessageType == ImportMessageType.Warning)
                {
                    warningMessage.AppendLine(message.Message);
                }
                else
                {
                    infoMessage.AppendLine(message.Message);
                }
            }
            using (var streamWriter = new StreamWriter(logFilePath, append, Encoding.UTF8))
            {

                var sb = new StringBuilder();
                sb.Append(GenerateLogHead(importProfile));
                sb.AppendLine();
                sb.AppendFormat("Started:\t\t{0}\r\n", result.LastRunStartDateTime.Value);
                sb.AppendFormat("Finished:\t\t{0}\r\n", result.LastRunEndDateTime.Value);
                sb.AppendFormat("Duration:\t\t{0}\r\n", (result.LastRunEndDateTime.Value - result.LastRunStartDateTime.Value).ToString("g"));
                sb.AppendLine();
                sb.AppendFormat("Total rows:\t\t{0}\r\n", result.TotalRecords);
                sb.AppendFormat("SkippedRecords:\t\t{0}\r\n", result.SkippedRecords);
                sb.AppendLine();
                sb.AppendFormat("Warnings:\t\t{0}\r\n", result.Warnings);
                sb.AppendLine(warningMessage.ToString());
                sb.AppendFormat("Errors:\t\t\t{0}\r\n", result.Errors);
                sb.AppendLine(errorMessage.ToString());
                streamWriter.WriteLine(sb.ToString());
                streamWriter.Flush();
                streamWriter.Close();
            }
            importProfile.LogFileName = "log.txt";
            importProfile.LastRunStartDateTime = importResult.LastRunStartDateTime;
            importProfile.LastRunEndDateTime = importResult.LastRunEndDateTime;
            _importProfileRepository.UpdateAndCommit(importProfile);
        }
    }
}
