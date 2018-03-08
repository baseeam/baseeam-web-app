/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System;

namespace BaseEAM.Core
{
    /// <summary>
    /// Base class for entities
    /// </summary>
    public abstract partial class BaseEntity
    {
        /// <summary>
        /// The primary key of this entity.
        /// </summary>
        public long Id { get; set; }

        public int Version { get; set; }

        /// <summary>
        /// The entity name, which is usually the display name of the entity. 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// BaseEAM will create new entity and persist when user click 'Create ...'
        /// The IsNew is used to indicate this newly created entity.
        /// If user doesn't save this entity by clicking 'Save', the IsNew flag
        /// is used for garbage data collector cron task.
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// A flag to indicate if this entity has been deactivated (or
        /// soft delete) from the database.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// The user who first created this entity.
        /// </summary>
        public string CreatedUser { get; set; }

        /// <summary>
        /// The date and time this entity was first created.
        /// </summary>
        public DateTime? CreatedDateTime { get; set; }

        /// <summary>
        /// The user who last modified this entity.
        /// </summary>
        public string ModifiedUser { get; set; }

        /// <summary>
        /// The date and time this entity was last modified.
        /// </summary>
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary>
        /// RowVersion is used for concurrency handling
        /// </summary>
        //public byte[] RowVersion { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity);
        }

        private static bool IsTransient(BaseEntity obj)
        {
            return obj != null && Equals(obj.Id, default(long));
        }

        private Type GetUnproxiedType()
        {
            return GetType();
        }

        public virtual bool Equals(BaseEntity other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Equals(Id, other.Id))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                return thisType.IsAssignableFrom(otherType) ||
                        otherType.IsAssignableFrom(thisType);
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (Equals(Id, default(long)))
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        public static bool operator ==(BaseEntity x, BaseEntity y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(BaseEntity x, BaseEntity y)
        {
            return !(x == y);
        }
    }
}
