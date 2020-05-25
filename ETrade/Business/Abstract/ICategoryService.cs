using System;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ICategoryService
    {
        IDataResult<List<Category>> GetAll();
        IDataResult<List<Category>> GetMainCategories();
        IDataResult<List<Category>> GetSubCategories(int categoryId);
        bool IsEndCategory(int categoryId);
        IResult AddCategory(Category category, int mainCategoryId = -1);
        IResult DeleteCategory(Category category);
        IResult UpdateCategory(Category category, int newMainCategoryId = -1);
        IDataResult<Category> GetMainCategory(int categoryId);
    }
}
