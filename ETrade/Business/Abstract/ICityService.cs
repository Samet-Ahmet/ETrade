using System;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ICityService
    {
        IDataResult<City> GetCityById(int cityId);
        IDataResult<List<City>> GetAll();
        IDataResult<District> GetDistrictById(int districtId);
        IDataResult<List<District>> GetDistrictsByCityId(int cityId);
    }
}
