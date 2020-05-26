using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Concrete
{
    public class WorkerManager : IWorkerService
    {
        private IUserService _userService;
        private IWorkerDal _workerDal;
        private IUserRoleDal _userRoleDal;
        private IUserDal _userDal;
        private ICityService _cityService;
        private IGenderDal _genderDal;


        public WorkerManager(IUserService userService, IWorkerDal workerDal, IUserRoleDal userRoleDal, IUserDal userDal, ICityService cityService, IGenderDal genderDal)
        {
            _userService = userService;
            _workerDal = workerDal;
            _userRoleDal = userRoleDal;
            _userDal = userDal;
            _cityService = cityService;
            _genderDal = genderDal;
        }

        public IResult AddWorker(Worker worker, User user)
        {
            try
            {
                worker.HireDate = DateTime.Now;
                _userService.Add(user);
                var addedUSer = _userService.GetByMail(user.Email);
                worker.WorkerId = addedUSer.Data.Id;
                worker.QuitDate = DateTime.MinValue;
                _workerDal.Add(worker);

                var userRole = new UserRole
                {
                    RoleId = 2, //worker rolü
                    UserId = worker.WorkerId
                };
                _userRoleDal.Add(userRole);
                return new SuccessResult();
            }
            catch (Exception)
            {
                return new ErrorResult(Messages.ErrorWhileAddingEntity);
            }
        }
        public IResult AddManager(Worker worker)
        {
            try
            {
                worker.HireDate = DateTime.Now;
                worker.QuitDate = new DateTime(1900,01,01,0,0,0);
                _workerDal.Add(worker);

                var userRole = new UserRole
                {
                    RoleId = 2, //worker rolü
                    UserId = worker.WorkerId
                };
                _userRoleDal.Add(userRole);
                var userRole2 = new UserRole
                {
                    RoleId = 3, //manager rolü
                    UserId = worker.WorkerId
                };
                _userRoleDal.Add(userRole2);
                return new SuccessResult();
            }
            catch (Exception)
            {
                return new ErrorResult(Messages.ErrorWhileAddingEntity);
            }
        }

        public IDataResult<Worker> GetById(int workerId)
        {
            try
            {
                var worker = _workerDal.Get(w => w.WorkerId == workerId);
                return new SuccessDataResult<Worker>(worker);
            }
            catch (Exception)
            {
                return new ErrorDataResult<Worker>(Messages.ErrorWhileGettingEntity);
            }
        }

        public IResult Quit(Worker worker)
        {
            try
            {
                var userRole = _userRoleDal.Get(ur => ur.UserId == worker.WorkerId);
                userRole.RoleId = 1;
                _userRoleDal.Update(userRole);
                worker.QuitDate = DateTime.Now;
                _workerDal.Update(worker);
                return new SuccessResult();
            }
            catch (Exception)
            {
                return new ErrorResult(Messages.ErrorWhileUpdatingEntity);
            }
        }

        public IResult Update(Worker worker, User user)
        {
            try
            {
                _userService.Update(user);
                _workerDal.Update(worker);
                return new SuccessResult();
            }
            catch (Exception)
            {
                return new ErrorResult(Messages.ErrorWhileUpdatingEntity);
            }
        }

        public IResult DoManager(int workerId)
        {
            try
            {
                var userRole = new UserRole
                {
                    UserId = workerId,
                    RoleId = 3
                };
                _userRoleDal.Add(userRole);
                return new SuccessResult();
            }
            catch (Exception)
            {
                return new ErrorResult(Messages.ErrorWhileUpdatingEntity);
            }
        }

        public IResult DoWorker(int workerId)
        {
            try
            {
                var userRole = _userRoleDal.Get(ur => ur.UserId == workerId && ur.RoleId == 3);
                _userRoleDal.Delete(userRole);
                return new SuccessResult();
            }
            catch (Exception)
            {
                return new ErrorResult(Messages.ErrorWhileUpdatingEntity);
            }
        }

        public IDataResult<List<WorkerDetailsDto>> GetAllManagers()
        {
            try
            {
                var managerRoles = _userRoleDal.GetList(ur => ur.RoleId == 3);
                var allManagers = new List<Worker>();
                var userDetails = new List<User>();
                foreach (var managerRole in managerRoles)
                {
                    allManagers.Add(_workerDal.Get(w => w.WorkerId == managerRole.UserId));
                    userDetails.Add(_userDal.Get(u => u.Id == managerRole.UserId));
                }

                List<WorkerDetailsDto> workers = new List<WorkerDetailsDto>();
                foreach (var manager in allManagers)
                {
                    var user = userDetails.SingleOrDefault(u => u.Id == manager.WorkerId);
                    workers.Add(new WorkerDetailsDto
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        AddressNumber = manager.AddressNumber,
                        BirthDate = manager.BirthDate,
                        CityId = manager.CityId,
                        CityName = _cityService.GetCityById(manager.CityId).Data.CityName,
                        DistrictId = manager.DistrictId,
                        DistrictName = _cityService.GetDistrictById(manager.DistrictId).Data.DistrictName,
                        Email = user.Email,
                        GenderId = user.GenderId,
                        PhoneNumber = user.PhoneNumber,
                        GenderName = _genderDal.Get(g => g.GenderId == user.GenderId).GenderName,
                        HireDate = manager.HireDate,
                        QuitDate = manager.QuitDate,
                        IdentityNo = manager.IdentityNo,
                        Street = manager.Street,
                        WorkerId = manager.WorkerId
                    });
                }
                return new SuccessDataResult<List<WorkerDetailsDto>>(workers);

            }
            catch (Exception e)
            {
                return new ErrorDataResult<List<WorkerDetailsDto>>(Messages.ErrorWhileGettingEntity);
            }
        }

        public IDataResult<List<WorkerDetailsDto>> GetWorkers()
        {
            try
            {
                var managerRoles = _userRoleDal.GetList(ur => ur.RoleId == 2);

                var allManagers = new List<Worker>();
                var userDetails = new List<User>();
                foreach (var managerRole in managerRoles)
                {
                    if (managerRole.RoleId == (byte)3)
                    {
                        continue;
                    }
                    allManagers.Add(_workerDal.Get(w => w.WorkerId == managerRole.UserId));
                    userDetails.Add(_userDal.Get(u => u.Id == managerRole.UserId));
                }

                List<WorkerDetailsDto> workers = new List<WorkerDetailsDto>();
                foreach (var manager in allManagers)
                {
                    var user = userDetails.SingleOrDefault(u => u.Id == manager.WorkerId);
                    workers.Add(new WorkerDetailsDto
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        AddressNumber = manager.AddressNumber,
                        BirthDate = manager.BirthDate,
                        CityId = manager.CityId,
                        CityName = _cityService.GetCityById(manager.CityId).Data.CityName,
                        DistrictId = manager.DistrictId,
                        DistrictName = _cityService.GetDistrictById(manager.DistrictId).Data.DistrictName,
                        Email = user.Email,
                        GenderId = user.GenderId,
                        PhoneNumber = user.PhoneNumber,
                        GenderName = _genderDal.Get(g => g.GenderId == user.GenderId).GenderName,
                        HireDate = manager.HireDate,
                        QuitDate = manager.QuitDate,
                        IdentityNo = manager.IdentityNo,
                        Street = manager.Street,
                        WorkerId = manager.WorkerId
                    });
                }
                return new SuccessDataResult<List<WorkerDetailsDto>>(workers);

            }
            catch (Exception e)
            {
                return new ErrorDataResult<List<WorkerDetailsDto>>(Messages.ErrorWhileGettingEntity);
            }
        }
    }
}
