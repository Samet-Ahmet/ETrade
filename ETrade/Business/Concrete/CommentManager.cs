using System;
using System.Collections.Generic;
using System.Text;
using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Concrete
{
    public class CommentManager : ICommentService
    {
        private ICommentDal _commentDal;
        private IUserService _userService;

        public CommentManager(ICommentDal commentDal, IUserService userService)
        {
            _commentDal = commentDal;
            _userService = userService;
        }

        public IResult Add(Comment comment)
        {
            try
            {
                comment.CommentDate = DateTime.Now;
                _commentDal.Add(comment);
                return new SuccessResult();
            }
            catch (Exception)
            {
                return new ErrorResult(Messages.ErrorWhileAddingEntity);
            }
        }

        public IDataResult<List<CommentDto>> GetByProductId(int productId)
        {
            var commentList = _commentDal.GetList(c => c.ProductId == productId);
            var commentDtoList = new List<CommentDto>();
            foreach (var comment in commentList)
            {
                var user = _userService.GetById(comment.UserId).Data;
                commentDtoList.Add(new CommentDto
                {
                    Comment = comment,
                    FullName = user.FirstName + " " + user.LastName
                });
            }
            return  new SuccessDataResult<List<CommentDto>>(commentDtoList);
        }
    }
}
