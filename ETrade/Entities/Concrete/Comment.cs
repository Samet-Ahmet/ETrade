using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class Comment : IEntity
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public DateTime CommentDate { get; set; }
    }
}
