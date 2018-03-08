/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

namespace BaseEAM.Core.Domain
{
    public partial class UserPasswordHistory : BaseEntity
    {
        /// <summary>
        /// The hashed Password string.
        /// </summary>
        public string LoginPassword { get; set; }

        public long? UserId { get; set; }
        public virtual User Owner { get; set; }
    }
}
