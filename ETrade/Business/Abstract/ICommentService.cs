using System;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Abstract
{
    public interface ICommentService
    {
        IResult Add(Comment comment);
        IDataResult<List<CommentDto>> GetByProductId(int productId);

    }
}
