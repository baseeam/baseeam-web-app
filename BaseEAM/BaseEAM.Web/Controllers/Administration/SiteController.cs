/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core;
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Data;
using BaseEAM.Services;
using BaseEAM.Web.Framework.Controllers;
using System.Linq;
using System.Web.Mvc;

namespace BaseEAM.Web.Controllers.Administration
{
    public class SiteController : BaseController
    {
        #region Fields

        private readonly IRepository<Site> _siteRepository;
        private readonly IRepository<Location> _locationRepository;
        private readonly IRepository<Core.Domain.Asset> _assetRepository;
        private readonly IRepository<Store> _storeRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Team> _teamRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Constructors

        public SiteController(IRepository<Site> siteRepository,
            IRepository<Location> locationRepository,
            IRepository<Core.Domain.Asset> assetRepository,
            IRepository<Store> storeRepository,
            IRepository<User> userRepository,
            IRepository<Team> teamRepository,
            ILocalizationService localizationService,
            IWorkContext workContext,
            IDbContext dbContext)
        {
            this._siteRepository = siteRepository;
            this._locationRepository = locationRepository;
            this._assetRepository = assetRepository;
            this._storeRepository = storeRepository;
            this._userRepository = userRepository;
            this._teamRepository = teamRepository;
            this._localizationService = localizationService;
            this._workContext = workContext;
            this._dbContext = dbContext;
        }

        #endregion

        /// <summary>
        /// Get the list of sites that the current user can access to
        /// based on the security group
        /// <param name="param">The text input from user</param>
        /// </summary>
        [HttpPost]
        public JsonResult SiteList(string param)
        {
            var securityGroupIds = this._workContext.CurrentUser.SecurityGroups.Select(s => s.Id).ToList();
            var sites = _siteRepository.GetAll()
                .Where(s => s.SecurityGroups.Any(g => securityGroupIds.Contains(g.Id))
                            && s.Name.Contains(param))
                .Select(s => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Id.ToString()
                })
                .ToList();
            if (sites.Count > 0)
            {
                sites.Insert(0, new SelectListItem { Value = "", Text = "" });
            }
            return Json(sites);
        }

        /// <summary>
        /// Get the list of locations of a site
        /// </summary>
        /// <param name="parentValue">siteId</param>
        /// <param name="param">The text input from user</param>
        [HttpPost]
        public JsonResult LocationList(long parentValue, string param)
        {
            var locations = _locationRepository.GetAll()
                .Where(l => l.SiteId == parentValue && l.Name.Contains(param))
                .Select(l => new SelectListItem { Text = l.Name, Value = l.Id.ToString() })
                .ToList();
            if (locations.Count > 0)
            {
                locations.Insert(0, new SelectListItem { Value = "", Text = "" });
            }
            return Json(locations);
        }

        /// <summary>
        /// Get the list of assets of a site
        /// </summary>
        /// <param name="parentValue">siteId</param>
        /// <param name="param">The text input from user</param>
        [HttpPost]
        public JsonResult AssetList(long parentValue, string param)
        {
            var assets = _assetRepository.GetAll()
                .Where(l => l.SiteId == parentValue && l.Name.Contains(param))
                .Select(l => new SelectListItem { Text = l.Name, Value = l.Id.ToString() })
                .ToList();
            if (assets.Count > 0)
            {
                assets.Insert(0, new SelectListItem { Value = "", Text = "" });
            }
            return Json(assets);
        }

        /// <summary>
        /// Get the list of stores of a site
        /// </summary>
        /// <param name="parentValue">siteId</param>
        /// <param name="param">The text input from user</param>
        [HttpPost]
        public JsonResult StoreList(long parentValue, string param)
        {
            var stores = _storeRepository.GetAll()
                .Where(l => l.SiteId == parentValue && l.Name.Contains(param))
                .Select(l => new SelectListItem { Text = l.Name, Value = l.Id.ToString() })
                .ToList();
            if (stores.Count > 0)
            {
                stores.Insert(0, new SelectListItem { Value = "", Text = "" });
            }
            return Json(stores);
        }

        /// <summary>
        /// Get the list of users of a site
        /// </summary>
        /// <param name="parentValue">siteId</param>
        /// <param name="param">The text input from user</param>
        [HttpPost]
        public JsonResult UserList(long parentValue, string param)
        {
            var users = _userRepository.GetAll()
                .Where(u => u.SecurityGroups.Any(sg => sg.Sites.Any(s => s.Id == parentValue))
                                && u.Name.Contains(param))
                .Select(u => new SelectListItem { Text = u.Name, Value = u.Id.ToString() })
                .ToList();
            if (users.Count > 0)
            {
                users.Insert(0, new SelectListItem { Value = "", Text = "" });
            }
            return Json(users);
        }

        /// <summary>
        /// Get the list of teams of a site
        /// </summary>
        /// <param name="parentValue">siteId</param>
        /// <param name="param">The text input from user</param>
        [HttpPost]
        public JsonResult TeamList(long parentValue, string param)
        {
            var teams = _teamRepository.GetAll()
                .Where(l => l.SiteId == parentValue && l.Name.Contains(param))
                .Select(l => new SelectListItem { Text = l.Name, Value = l.Id.ToString() })
                .ToList();
            if (teams.Count > 0)
            {
                teams.Insert(0, new SelectListItem { Value = "", Text = "" });
            }
            return Json(teams);
        }
    }
}