using System;
using System.Threading.Tasks;

namespace Api.Interfaces.Teachers
{
    public interface IStudentResultsExport
    {
        Task<byte[]> Export(Guid groupId);
    }
}
