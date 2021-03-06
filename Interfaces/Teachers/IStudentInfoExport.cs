using System;
using System.Threading.Tasks;

namespace Api.Interfaces.Teachers
{
    public interface IStudentInfoExport
    {
        Task<byte[]> Export(Guid groupId);
    }
}