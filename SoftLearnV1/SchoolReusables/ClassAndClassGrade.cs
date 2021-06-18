using SoftLearnV1.Entities;
using SoftLearnV1.Helpers;
using SoftLearnV1.Reusables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.SchoolReusables
{
    public class ClassAndClassGrade
    {
        private readonly AppDbContext _context;

        public ClassAndClassGrade(AppDbContext context)
        {
            _context = context;
        }

        //retreive the student current class and classGrade
        public GradeStudents studentClassAndClassGrade(Guid studentId, long schoolId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);

                GradeStudents grdStd = null;

                //get School Current Session and Term
                long currentSessionId = new SessionAndTerm(_context).getCurrentSessionId(schoolId);
                if (currentSessionId > 0 && checkSchool == true)
                {
                    //get the Student Class and ClassGrade
                    GradeStudents getStudent = _context.GradeStudents.Where(x => x.StudentId == studentId && x.SessionId == currentSessionId).FirstOrDefault();
                    grdStd = getStudent;
                }

                return grdStd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
