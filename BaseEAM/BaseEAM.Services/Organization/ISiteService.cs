/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using System;
using System.Collections.Generic;

namespace BaseEAM.Services
{
    public interface ISiteService : IBaseService
    {
        PagedResult<Site> GetSites(string expression,
            dynamic parameters,
            int pageIndex = 0,
            int pageSize = 2147483647,
            IEnumerable<Sort> sort = null); //Int32.MaxValue

        /// <summary>
        /// Get the list of Sites that can be accessed
        /// by the current user
        /// </summary>
        List<Site> GetSites(User user, DateTime? modifiedDateTime);

        /// <summary>
        /// Get the list of Assets that can be accessed
        /// by the current user
        /// </summary>
        List<Asset> GetAssets(User user, DateTime? modifiedDateTime);

        /// <summary>
        /// Get the list of Locations that can be accessed
        /// by the current user
        /// </summary>
        List<Location> GetLocations(User user, DateTime? modifiedDateTime);

        /// <summary>
        /// Get the list of Teams that can be accessed
        /// by the current user
        /// </summary>
        List<Team> GetTeams(User user, DateTime? modifiedDateTime);

        /// <summary>
        /// Get the list of Users that can be accessed
        /// by the current user
        /// </summary>
        List<User> GetUsers(User user, DateTime? modifiedDateTime);

        /// <summary>
        /// Get the list of Technicians that can be accessed
        /// by the current user
        /// </summary>
        List<Technician> GetTechnicians(User user, DateTime? modifiedDateTime);

        /// <summary>
        /// Get the list of Points that can be accessed
        /// by the current user
        /// </summary>
        List<Point> GetPoints(User user, DateTime? modifiedDateTime);
    }
}
