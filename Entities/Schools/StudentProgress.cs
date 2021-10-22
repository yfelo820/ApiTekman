namespace Api.Entities.Schools
{
    public class StudentProgress : BaseEntity
    {
        public string UserName { get; set; }
        public int Session { get; set; }
        public int Course { get; set; }
        public string SubjectKey { get; set; }
        public string LanguageKey { get; set; }
        public DiagnosisTestState DiagnosisTestState { get; set; }
        
        public bool IsProgressing(int activitySession,int activityCourse)
        {
            return Course == activityCourse && Session <= activitySession;
        }
        
        public void SetNextSession(int courseSessions, int subjectCourses)
        {
            if (Session >= courseSessions && Course >= subjectCourses) 
                return;
            
            Session++;

            if (Session > courseSessions) 
            { 
                Course++; 
                Session = 1;
            }
        }
    }

    public enum DiagnosisTestState {
        Completed,
        Pending,
        NotDefined
    }
}
