using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrete;
using Entities.Dtos;

namespace WebUI.Models
{
    public class ManagerListViewModel
    {
        public List<WorkerDetailsDto> Managers { get; set; }
    }
}
