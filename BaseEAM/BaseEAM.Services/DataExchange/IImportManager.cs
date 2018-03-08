/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/

using BaseEAM.Core.Domain;

namespace BaseEAM.Services
{
    public interface IImportManager
    {
        void Execute(long importProfileId);

        #region UnitOfMeasure

        void ImportUnitOfMeasureFromXlsx(ImportProfile importProfile);

        #endregion
    }
}
