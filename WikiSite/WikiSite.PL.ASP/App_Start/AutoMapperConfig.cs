using AutoMapper;
using WikiSite.Entities;
using WikiSite.PL.ASP.Models;

namespace WikiSite.PL.ASP
{
    public class AutoMapperConfig
    {
        public static void Configurate()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ArticleBDO, ArticleVM>();
                cfg.CreateMap<ArticleVM, ArticleBDO>();
            });
        }
    }
}