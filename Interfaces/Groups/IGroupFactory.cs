using Api.DTO.Groups;
using Api.Entities.Schools;

namespace Api.Interfaces.Groups
{
    public interface IGroupFactory
    {
        Group Create(GroupManagementDto data);
    }
}