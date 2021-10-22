using System;
using System.Collections.Generic;

namespace Api.DTO.Teachers
{
	public class LicensesUserApiDTO
	{
		public List<LicenseDTO> licencias { get; set; }
	}

	public class LicenseDTO
	{
		public SchoolYearDto curso_escolar { get; set; }
      	public List<CourseDto> cursos { get; set; }
	}

	public class SchoolYearDto {
		public string id { get; set; }
		public string nombre { get; set; }
		public string region { get; set; }
		public string fecha_inicio { get; set; }
		public string fecha_fin { get; set; }
	}

	public class CourseDto {
		public string etapa { get; set; }
		public string curso { get; set; }
		public int licencias { get; set; } 
		public string idioma { get; set; }
		public string productName { get; set; }
	}
}