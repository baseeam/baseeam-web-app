using BaseEAM.Core;
using BaseEAM.Core.Domain;
using BaseEAM.Core.Kendoui;
using System.Collections.Generic;

namespace BaseEAM.Services
{
    public interface IUnitConversionService : IBaseService
    {
        PagedResult<UnitConversion> GetUnitConversions(string expression,
           dynamic parameters,
           int pageIndex = 0,
           int pageSize = 2147483647,
           IEnumerable<Sort> sort = null);

        UnitConversion GetUnitConversion(long fromUnitOfMeasureId, long toUnitOfMeasureId);

        List<UnitOfMeasure> GetFromUOMs(long toUnitOfMeasureId);
    }
}
