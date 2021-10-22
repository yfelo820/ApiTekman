using System.Collections.Generic;

namespace Api.DTO.Teachers
{
    public class ExcelGroupDTO
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public IEnumerable<ExcelStudentDTO> Students { get; set; } = new List<ExcelStudentDTO>();
    }

    public class ExcelStudentDTO
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public int Password { get; set; }
    }
}