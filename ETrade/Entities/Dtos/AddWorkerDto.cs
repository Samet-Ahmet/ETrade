using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Abstract;

namespace Entities.Dtos
{
    public class AddWorkerDto : IDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long IdentityNo { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime HireDate { get; set; }
        public byte CityId { get; set; }
        public short DistrictId { get; set; }
        public string Street { get; set; }
        public string AddressNumber { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public byte GenderId { get; set; }
        public string Password { get; set; }
        public string BirthDateDay { get; set; }
        public string BirthDateMounth { get; set; }
        public string BirthDateYear { get; set; }
    }
}
