using AutoMapper;
using PropertyManager.Api.Interfaces;
using PropertyManagerApi.Models;
using PropertyManagerApi.Models.DTOs.Tenant;
using System;

namespace PropertyManagerApi.Profiles
{
    public class TenantProfile : Profile
    {
        public TenantProfile()
        {
            CreateMap<Tenant, Tenant_CreateDto>().ReverseMap();
            // Feck knows if this will work....
            CreateMap<Tenant, Tenant_DetailDto>()
                .ForMember(d => d.Image, opt => opt.MapFrom(o => o.Profile_Url))//.AfterMap<FileResolver>()
                .ForMember(x => x.ContactNumber, opt => opt.MapFrom(opt => opt.ContactNumber))
                .ForMember(x => x.CreatedDateTime, opt => opt.MapFrom(opt => opt.CreatedDateTime))
                .ForMember(x => x.EmailAddress, opt => opt.MapFrom(opt => opt.EmailAddress))
                .ForMember(x => x.FirstName, opt => opt.MapFrom(opt => opt.FirstName))
                .ForMember(x => x.Id, opt => opt.MapFrom(opt => opt.Id))
                .ForMember(x => x.IsActive, opt => opt.MapFrom(opt => opt.IsActive))
                .ForMember(x => x.LastName, opt => opt.MapFrom(opt => opt.LastName))
                .ForMember(x => x.Notes, opt => opt.MapFrom(opt => opt.Notes))
                .ForMember(x => x.Profession, opt => opt.MapFrom(opt => opt.Profession))
                .ForMember(x => x.TenancyEndDate, opt => opt.MapFrom(opt => opt.TenancyEndDate))
                .ForMember(x => x.TenancyStartDate, opt => opt.MapFrom(opt => opt.TenancyStartDate))
                .ForMember(x => x.Title, opt => opt.MapFrom(opt => opt.Title))
                .ForMember(x => x.UpdatedDateTime, opt => opt.MapFrom(opt => opt.UpdatedDateTime))
                .ReverseMap();
            //CreateMap<Tenant, Tenant_Detail>().ReverseMap();
        }
    }

    public class FileResolver : IMappingAction<Tenant, Tenant_DetailDto>
    {
        private readonly IFileService _fileService;

        public FileResolver(IFileService fileService)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        public void Process(Tenant source, Tenant_DetailDto destination, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.Profile_Url))
            {
                destination.Image = "";
            }
            destination.Image = _fileService.FileToBase64String(source.Profile_Url).Result;
        }
    }
}