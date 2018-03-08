/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BaseEAM.Services
{
    public class EntityAttributeService : BaseService, IEntityAttributeService
    {
        #region Fields

        private readonly IRepository<EntityAttribute> _entityAttributeRepository;
        private readonly IDateTimeHelper _datetimeHelper;
        private readonly DapperContext _dapperContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public EntityAttributeService(IRepository<EntityAttribute> entityAttributeRepository,
            IDateTimeHelper datetimeHelper,
            DapperContext dapperContext,
            IDbContext dbContext)
        {
            this._entityAttributeRepository = entityAttributeRepository;
            this._datetimeHelper = datetimeHelper;
            this._dapperContext = dapperContext;
            this._dbContext = dbContext;
        }

        #endregion

        public virtual void UpdateEntityAttributes(long? entityId, string entityType, string json)
        {
            var entityAttributes = _entityAttributeRepository.GetAll()
                .Where(e => e.EntityId == entityId && e.EntityType == entityType)
                .ToList();
            Update(json, entityAttributes);
            //this._dbContext.SaveChanges();
        }

        private Dictionary<string, string> ParseJson(string json, List<EntityAttribute> entityAttributes)
        {
            var valuesDictionary = new Dictionary<string, string>();
            var values = json.Split('&');
            if (values.Count() > 0)
            {
                foreach (var value in values)
                {
                    var keyValue = value.Split('=');
                    if (keyValue.Count() != 2)
                    {
                        throw new BaseEamException("Not valid form.");
                    }
                    else
                    {
                        if(entityAttributes.Exists(e => e.Attribute.Name == keyValue[0]))
                        {
                            //This check is for multiselect values
                            if (valuesDictionary.ContainsKey(keyValue[0]))
                            {
                                //build a value has format '1','2','3' so it can be replaced in an IN clause
                                valuesDictionary[keyValue[0]] = "'" + valuesDictionary[keyValue[0]] + "','" + keyValue[1] + "'";
                                valuesDictionary[keyValue[0]] = valuesDictionary[keyValue[0]].Replace("''", "'");
                            }
                            else
                            {
                                valuesDictionary.Add(keyValue[0], HttpUtility.UrlDecode(keyValue[1]));
                            }
                        }                        
                    }
                }
            }

            return valuesDictionary;
        }

        private void Update(string json, List<EntityAttribute> entityAttributes)
        {
            var dict = ParseJson(json, entityAttributes);
            foreach (var entityAttribute in entityAttributes)
            {
                var attribute = entityAttribute.Attribute;
                if (dict.ContainsKey(attribute.Name) && !string.IsNullOrEmpty(dict[attribute.Name]))
                {
                    if (attribute.DataType == (int?)FieldDataType.DateTime 
                            || attribute.DataType == (int?)FieldDataType.DateTimeNullable)
                        entityAttribute.Value = _datetimeHelper.ConvertToUtcTime(Convert.ToDateTime(dict[attribute.Name]), _datetimeHelper.CurrentTimeZone).ToString();
                    else 
                        entityAttribute.Value = dict[attribute.Name];
                }
                else
                {
                    entityAttribute.Value = null;
                }
            }
        }
    }
}
