using AutoMapper;
using TaskMasterAPI.BLL.Dtos;
using TaskMasterAPI.BLL.Modules.Clients.Commands.RegistrationClient;
using TaskMasterAPI.Models.Clients;

namespace TaskMasterAPI.BLL.MappingProfiles;

public class ClientMappingProfile : Profile
{
    public ClientMappingProfile()
    {
        CreateMap<Client, ClientDto>()
            .ReverseMap();

        CreateMap<RegistrationClientCommand, Client>()
            .ReverseMap();
    }
}