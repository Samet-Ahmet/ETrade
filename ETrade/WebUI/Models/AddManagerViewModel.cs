using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebUI.Models
{
    public class AddManagerViewModel
    {
        public WorkerDetailsDto Manager { get; set; }
        public List<SelectListItem> GenderNamesSelectItems { get; set; }
        public List<SelectListItem> CitiesSelectItems { get; set; }
        public List<SelectListItem> DistrictsSelectItems { get; set; }
    }
}
