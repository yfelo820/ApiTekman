using System;

namespace Api.Entities.Schools
{
    public class StudentGroup : BaseEntity
	{
        public Guid? TkStudentId { get; set; }
        public string UserName { get; set; }
        public int AccessNumber { get; set; }
        public Guid GroupId { get; set; }
		public virtual Group Group { get; set; }
		
		public StudentGroup() {}

		public StudentGroup(string userName, Guid groupId, int accessNumber) {
			UserName = userName;
			GroupId = groupId;
			AccessNumber = accessNumber;
		}

        public StudentGroup(Guid tkId, string userName, Guid groupId, int accessNumber)
        {
            TkStudentId = tkId;
            UserName = userName;
            GroupId = groupId;
            AccessNumber = accessNumber;
        }
    }
}