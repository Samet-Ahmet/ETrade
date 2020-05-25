using System;
using System.Collections.Generic;
using System.Text;
using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class CityManager : ICityService
    {
        private ICityDal _cityDal;
        private IDistrictDal _districtDal;

        public CityManager(ICityDal cityDal, IDistrictDal districtDal)
        {
            _cityDal = cityDal;
            _districtDal = districtDal;
        }

        public IDataResult<City> GetCityById(int cityId)
        {
            try
            {
                return new SuccessDataResult<City>(_cityDal.Get(c=>c.CityId==cityId));
            }
            catch (Exception)
            {
                return new ErrorDataResult<City>(Messages.ErrorWhileGettingEntity);
            }
        }

        public IDataResult<List<City>> GetAll()
        {
            try
            {
                return new SuccessDataResult<List<City>>(_cityDal.GetList());
            }
            catch (Exception)
            {
                return new ErrorDataResult<List<City>>(Messages.ErrorWhileGettingEntity);
            }
        }

        public IDataResult<District> GetDistrictById(int districtId)
        {
            try
            {
                return new SuccessDataResult<District>(_districtDal.Get(d=>d.DistrictId == districtId));
            }
            catch (Exception)
            {
                return new ErrorDataResult<District>(Messages.ErrorWhileGettingEntity);
            }
        }

        public IDataResult<List<District>> GetDistrictsByCityId(int cityId)
        {
            try
            {
                return new SuccessDataResult<List<District>>(_districtDal.GetList(d=>d.CityId == cityId));
            }
            catch (Exception e)
            {
                return new ErrorDataResult<List<District>>(Messages.ErrorWhileGettingEntity);
            }
        }
    }
}
