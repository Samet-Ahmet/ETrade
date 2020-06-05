using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Abstract;
using Entities.Concrete;

namespace Entities.Dtos
{
    public class CommentDto : IDto
    {
        public Comment Comment { get; set; }
        public string FullName { get; set; }
    }
}
