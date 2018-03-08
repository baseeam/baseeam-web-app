/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Core.Domain
{
    /// <summary>
    /// Only allow one device type + platform per user
    /// </summary>
    public class UserDevice : BaseEntity
    {
        public long? UserId { get; set; }
        public virtual User User { get; set; }
        public string Platform { get; set; } //iOS, Android, ...
        public string DeviceType { get; set; } //iPhone, iPad, ...
        public string DeviceToken { get; set; }
    }
}
