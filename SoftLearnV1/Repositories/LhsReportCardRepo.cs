using SoftLearnV1.Entities;
using SoftLearnV1.Helpers;
using SoftLearnV1.InterfaceRepositories;
using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using SoftLearnV1.Reusables;
using SoftLearnV1.SchoolReusables;
using SoftLearnV1.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Repositories
{
    public class LhsReportCardRepo : ILhsReportCardRepo
    {
        private readonly AppDbContext _context;
        private readonly ReportCardReUsables reUsables;

        public LhsReportCardRepo(AppDbContext context, ReportCardReUsables reUsables)
        {
            _context = context;
            this.reUsables = reUsables;
        }

        public async Task<LhsReportCardResponseModel> getReportCardDatAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long termId, long sessionId)
        {
            try
            {
                IList<CognitiveAbility> cognitive = new List<CognitiveAbility>();

                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGarade = check.checkClassGradeById(classGradeId);
                var checkSession = check.checkSessionById(sessionId);
                var checkTerm = check.checkTermById(termId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true && checkSession == true && checkTerm == true)
                {

                    //check if report card template is configured
                    ReportCardTemplates checkTemplate = _context.ReportCardTemplates.Where(x => x.SchoolId == schoolId && x.CampusId == campusId && x.ClassId == classId).FirstOrDefault();

                    if (checkTemplate != null)
                    {
                        //if the schoolSubType is JUNIOR
                        if (checkTemplate.SchoolSubTypeId == (int)EnumUtility.SchoolSubTypes.Junior)
                        {






                        }
                        //if the schoolSubType is SENIOR
                        if (checkTemplate.SchoolSubTypeId == (int)EnumUtility.SchoolSubTypes.Senior)
                        {

                        }

                        Students student = _context.Students.Where(s => s.Id == studentId).FirstOrDefault();
                        if (student != null)
                        {
                            StudentData studData = new StudentData();
                            studData.Name = student.FirstName + " " + student.LastName;






                            //Score Category
                            int scoreCategoryExam = (int)EnumUtility.ScoreCategory.Exam;
                            int scoreCategoryCA = (int)EnumUtility.ScoreCategory.CA;

                            //Score SubCategory For the School
                            //Write a Method for this in the reportcard reusable class
                            long getHwId = _context.ScoreSubCategoryConfig.Where(x => x.SchoolId == schoolId && x.CampusId == campusId && x.ClassId == classId
                            && x.TermId == termId && x.SessionId == sessionId && x.SubCategoryName.ToUpper().Trim() == "HW").FirstOrDefault().Id;

                            long getCwId = _context.ScoreSubCategoryConfig.Where(x => x.SchoolId == schoolId && x.CampusId == campusId && x.ClassId == classId
                            && x.TermId == termId && x.SessionId == sessionId && x.SubCategoryName.ToUpper().Trim() == "CW").FirstOrDefault().Id;



                            decimal total = 0;
                            decimal cw = 0;
                            decimal hw = 0;
                            decimal exam = 0;


                            //get all Subjects in the school
                            IList<SchoolSubjects> getSubjectInClass = (from s in _context.SchoolSubjects where s.SchoolId == schoolId && s.CampusId == campusId && s.ClassId == classId select s).ToList();

                            IList<CognitiveAbility> cogAbList = new List<CognitiveAbility>();

                            foreach (SchoolSubjects subjectId in getSubjectInClass)
                            {
                                CognitiveAbility cogAb = new CognitiveAbility();
                                cogAb.Subject = subjectId.SubjectName;


                                //Examination
                                IList<ExaminationScores> examScore = reUsables.getExaminationScores(student.Id, schoolId, campusId, classId, classGradeId, subjectId.Id, student.AdmissionNumber, termId, sessionId);
                                foreach (ExaminationScores exm in examScore)
                                {
                                    exam = exm.MarkObtained;
                                    cogAb.Exam = exam;
                                }

                                //HW
                                IList<ContinousAssessmentScores> hwScore = reUsables.getContinuousAssessmentScoresPerCategory(student.Id, schoolId, campusId, classId, classGradeId, subjectId.Id, student.AdmissionNumber, scoreCategoryCA, getHwId, termId, sessionId);
                                foreach (ContinousAssessmentScores hwScr in hwScore)
                                {
                                    hw = hwScr.MarkObtained;
                                    cogAb.HW = hw;
                                    ///break;
                                }
                                //CW
                                IList<ContinousAssessmentScores> cwScore = reUsables.getContinuousAssessmentScoresPerCategory(student.Id, schoolId, campusId, classId, classGradeId, subjectId.Id, student.AdmissionNumber, scoreCategoryCA, getCwId, termId, sessionId);
                                foreach (ContinousAssessmentScores cwScr in cwScore)
                                {
                                    cw = cwScr.MarkObtained;
                                    cogAb.CW = cw;
                                }

                                total = cw + hw + exam;
                                cogAb.Total = total;



                                cogAbList.Add(cogAb);
                            }

                            studData.CognitiveAbility = cogAbList;

                            return new LhsReportCardResponseModel { StatusCode = 200, StatusMessage = "Successful", StudentData = studData };

                        }

                        return new LhsReportCardResponseModel { StatusCode = 200, StatusMessage = "No Student" };

                    }

                    return new LhsReportCardResponseModel { StatusCode = 200, StatusMessage = "No Template" };

                }

                return new LhsReportCardResponseModel { StatusCode = 200, StatusMessage = "Invalid Params" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new LhsReportCardResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }
    }
}
