﻿using WWW.Domain.Entity;
using WWW.Domain.Response;
using WWW.Service.Interfaces;
using WWW.DAL.Interfaces;
using WWW.Domain.Enum;
using Microsoft.EntityFrameworkCore;

using WWW.DAL.Repositories;

namespace WWW.Service.Implementations
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly UserRepository _userRepository;

        public ArticleService(IArticleRepository articleRepository, UserRepository userRepository )
        {
            _articleRepository = articleRepository;
            _userRepository = userRepository;
        }


        public async Task<BaseResponse<IEnumerable<Article>>> GetAll()
        {
            var BaseResponse = new BaseResponse<IEnumerable<Article>>();
            try {
                var Articles = await _articleRepository.GetAll();
                if (Articles.Count() == 0)
                {
                    BaseResponse.ErrorDescription = "Found 0 elements";
                    BaseResponse.StatusCode = (StatusCode)StatusCode.OK;
                }
                else
                {
                    BaseResponse.Data = Articles;
                    BaseResponse.StatusCode = (StatusCode)StatusCode.OK;
                }
                return BaseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Article>>()
                {
                    ErrorDescription = $"[Articles.GetAll]:{ex.Message}",
                };
            }
        }
        public async Task<BaseResponse<IEnumerable<Article>>> GetByCategoryName(string CatName)
        {
            BaseResponse<IEnumerable<Article>> BaseResponse = new BaseResponse<IEnumerable<Article>>();
            try
            {
                BaseResponse.Data = await _articleRepository.GetByCategoryName(CatName);
                BaseResponse.StatusCode = StatusCode.OK;
            }
            catch(Exception ex) { 
                BaseResponse.ErrorDescription += ex.Message;
                BaseResponse.StatusCode = StatusCode.InternalServerError;
            }
            return BaseResponse;
        }

        //public async Task<BaseResponse<IEnumerable<Article>>> foo(int i)
        //{
        //    var Data = await _articleRepository.GetALL().Where(a => a.Id == i).ToListAsync();
        //    var baseres = new BaseResponse<IEnumerable<Article>>()
        //    {
        //        Data = await _articleRepository.GetALL().Where(a => a.Id == i).ToListAsync(),
        //        StatusCode = StatusCode.OK
        //    };
        //    if (Data.Count > 0)
        //    {
        //        baseres.Data = Data;
        //        baseres.StatusCode = StatusCode.OK;
        //    }
        //    else
        //    {
        //        baseres.StatusCode= StatusCode.InternalServerError;
        //    }
        //    return baseres;
        //}

        public async Task<bool> Create(Article entity)
        {
            entity.slug = $"{entity.Title}-{entity.Id}";
            entity.DateOfCreation = entity.DateOfModification = DateTime.Now;
            entity.Author = _userRepository.GetALL().First();

            return await _articleRepository.Create(entity);  
        }

        public async Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
