/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using BaseEAM.Core.Data;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using BaseEAM.Data;
using System.Linq;

namespace BaseEAM.Services
{
    public class AttachmentService : BaseService, IAttachmentService
    {
        #region Fields

        private readonly IRepository<Attachment> _attachmentRepository;
        private readonly IRepository<EntityAttachment> _entityAttachmentRepository;
        private readonly DapperContext _dapperContext;
        private readonly IDbContext _dbContext;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public AttachmentService(IRepository<Attachment> attachmentRepository,
            IRepository<EntityAttachment> entityAttachmentRepository,
            DapperContext dapperContext,
            IDbContext dbContext)
        {
            this._attachmentRepository = attachmentRepository;
            this._entityAttachmentRepository = entityAttachmentRepository;
            this._dapperContext = dapperContext;
            this._dbContext = dbContext;
        }

        #endregion

        #region Methods

        public virtual void CopyAttachments(long fromEntityId, string fromEntityType, long toEntityId, string toEntityType)
        {
            var from = _entityAttachmentRepository.GetAll()
                .Where(e => e.EntityId == fromEntityId && e.EntityType == fromEntityType)
                .ToList();
            foreach(var ea in from)
            {
                _entityAttachmentRepository.Insert(new EntityAttachment
                {
                    EntityId = toEntityId,
                    EntityType = toEntityType,
                    AttachmentId = ea.AttachmentId
                });
            } 
        }

        #endregion
    }
}
