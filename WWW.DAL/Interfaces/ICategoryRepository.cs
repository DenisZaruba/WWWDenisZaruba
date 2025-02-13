﻿using WWW.Domain.Entity;

namespace WWW.DAL.Interfaces
{
    public interface ICategoryRepository:IBaseRepository<Category>
    {
        //Category GetCategoryByName(string name);
        Task<IEnumerable<Category>> GetNotEmptyCategory();
    }
    
}
