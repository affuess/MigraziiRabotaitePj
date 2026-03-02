using AutoMapper;
using MigraziiRabotaitePj.DTO;
using MigraziiRabotaitePj.Models;

namespace MigraziiRabotaitePj.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity -> DTO (для відповіді)
            CreateMap<Product, ProductReadDTO>();
            CreateMap<Models.Characteristics, CharacteristicsDTO>();

            // DTO -> Entity (для створення/оновлення)
            CreateMap<ProductCreateDTO, Product>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore());

            CreateMap<ProductUpdateDTO, Product>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore());

            CreateMap<CharacteristicsDTO, Characteristics>();
        }
    }
}
