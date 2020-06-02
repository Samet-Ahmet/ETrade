using System;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Abstract
{
    public interface IWorkerService
    {
        IResult AddWorker(Worker worker);
        IResult AddManager(Worker worker);
        IDataResult<Worker> GetById(int workerId);
        IResult Quit(Worker worker);
        IResult Update(Worker worker, User user);
        IResult DoManager(int workerId);
        IResult DoWorker(int workerId);
        IDataResult<List<WorkerDetailsDto>> GetAllManagers();
        IDataResult<List<WorkerDetailsDto>> GetWorkers();
    }
}
