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
	            cfg.CreateMap<CommentVM, ArticleCommentDTO>();
	            cfg.CreateMap<ArticleCommentDTO, CommentVM>();
	            cfg.CreateMap<AVersionVoteVM, VersionVoteDTO>()
		           .ForMember(dto => dto.IsVoteFor, config => config.MapFrom(vm => vm.IsLike));
	            cfg.CreateMap<VersionVoteDTO, AVersionVoteVM>()
		           .ForMember(vm => vm.IsLike, config => config.MapFrom(dto => dto.IsVoteFor));
            });
        }
    }
}