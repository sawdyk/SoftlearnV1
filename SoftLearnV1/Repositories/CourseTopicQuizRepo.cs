using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SoftLearnV1.Entities;
using SoftLearnV1.Helpers;
using SoftLearnV1.InterfaceRepositories;
using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using SoftLearnV1.Reusables;
using SoftLearnV1.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Repositories
{
    public class CourseTopicQuizRepo : ICourseTopicQuizRepo
    {
        private readonly AppDbContext _context;

        public CourseTopicQuizRepo(AppDbContext context)
        {
            this._context = context;
        }
        //----------------------------CourseTopicQuiz---------------------------------------------------------------

        public async Task<GenericResponseModel> createCourseTopicQuizAsync(CourseTopicQuizRequestModel obj)
        {
            try
            {
                //check if course topic or course exist
                var courseTopic = await _context.CourseTopics.Where(x => x.Id == obj.CourseTopicId).FirstOrDefaultAsync();
                var course = await _context.Courses.Where(x => x.Id == obj.CourseId).FirstOrDefaultAsync();
                if(courseTopic == null || course == null)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Topic or Course doesn't Exists" };
                }
                //check if a course topic quiz to be created already exists
                var checkResult = _context.CourseTopicQuiz.Where(x => x.CourseId == obj.CourseId && x.CourseTopicId == obj.CourseTopicId).FirstOrDefault();

                //if the course topic quiz doesnt exist, Create the course topic quiz
                if (checkResult == null)
                {
                    var courseTopicQuiz = new CourseTopicQuiz
                    {
                        CourseId = obj.CourseId,
                        CourseTopicId = obj.CourseTopicId,
                        Description = obj.Description,
                        Duration = obj.Duration,
                        LastUpdated = DateTime.Now,
                        PercentagePassMark = obj.PercentagePassMark,
                        DateCreated = DateTime.Now,
                        Status = true
                    };

                    await _context.CourseTopicQuiz.AddAsync(courseTopicQuiz);
                    await _context.SaveChangesAsync();

                    //get the Course Topic Quiz Created
                    var getCourseTopicQuiz = from cr in _context.CourseTopicQuiz
                                             where cr.Id == courseTopicQuiz.Id
                                             select new
                                             {
                                                 cr.Id,
                                                 cr.CourseId,
                                                 cr.CourseTopicId,
                                                 cr.DateCreated,
                                                 cr.LastUpdated,
                                                 cr.PercentagePassMark,
                                                 cr.Status,
                                                 cr.Description,
                                                 cr.Duration
                                             };

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Topic Quiz Created Successfully", Data = getCourseTopicQuiz.FirstOrDefault() };

                }
                else
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Topic Quiz Already Exists" };
                }
            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<GenericResponseModel> updateCourseTopicQuizAsync(long quizId, CourseTopicQuizRequestModel obj)
        {
            try
            {
                //check if course topic or course exist
                var courseTopic = await _context.CourseTopics.Where(x => x.Id == obj.CourseTopicId).FirstOrDefaultAsync();
                var course = await _context.Courses.Where(x => x.Id == obj.CourseId).FirstOrDefaultAsync();
                if (courseTopic == null || course == null)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Topic or Course doesn't Exists" };
                }
                //check if a course topic quiz to be edited exists
                var checkResult = _context.CourseTopicQuiz.Where(x => x.Id == quizId).FirstOrDefault();

                //if the course topic quiz exist, Update the course topic quiz
                if (checkResult != null)
                {
                    checkResult.CourseId = obj.CourseId;
                    checkResult.CourseTopicId = obj.CourseTopicId;
                    checkResult.Description = obj.Description;
                    checkResult.Duration = obj.Duration;
                    checkResult.LastUpdated = DateTime.Now;
                    checkResult.PercentagePassMark = obj.PercentagePassMark;
                    checkResult.Status = obj.Status;
                    await _context.SaveChangesAsync();

                    //get the Course Topic Quiz Updated
                    var getCourseTopicQuiz = from cr in _context.CourseTopicQuiz
                                             where cr.Id == quizId
                                             select new
                                             {
                                                 cr.Id,
                                                 cr.CourseId,
                                                 cr.CourseTopicId,
                                                 cr.DateCreated,
                                                 cr.LastUpdated,
                                                 cr.PercentagePassMark,
                                                 cr.Status,
                                                 cr.Description,
                                                 cr.Duration
                                             };

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Topic Quiz Updated Successfully", Data = getCourseTopicQuiz.FirstOrDefault() };

                }
                else
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Topic Quiz Doesn't Exists" };
                }
            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }
        
        public async Task<GenericResponseModel> deleteCourseTopicQuizAsync(long quizId)
        {
            try
            {
                var questions = _context.CourseTopicQuizQuestions.Where(x => x.CourseTopicQuizId == quizId).ToList();
                var quizResult = _context.CourseTopicQuizResults.Where(x => x.CourseTopicQuizId == quizId).ToList();
                if (questions.Count  <= 0 && quizResult.Count <= 0)
                {
                    var obj = _context.CourseTopicQuiz.Where(x => x.Id == quizId).FirstOrDefault();
                if (obj != null)
                {
                    _context.CourseTopicQuiz.Remove(obj);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Deleted Successfully!" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Quiz With the Specified ID!" };
                }
                return new GenericResponseModel { StatusCode = 400, StatusMessage = "Record can't be deleted because some records are depending on it!" };
            }

            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getCourseTopicQuizByTopicIdAsync(long topicId)
        {
            try
            {
                //check if the topicId is valid
                var checkResult = new CheckerValidation(_context).checkCourseTopicById(topicId);
                if (checkResult == true)
                {
                    var result = from cr in _context.CourseTopicQuiz
                                 where cr.CourseTopicId == topicId
                                 orderby cr.Id ascending
                                 select new
                                 {
                                     cr.Id,
                                     cr.CourseId,
                                     cr.CourseTopicId,
                                     cr.Description,
                                     cr.Duration,
                                     cr.PercentagePassMark,
                                     cr.Status,
                                     cr.DateCreated,
                                     cr.LastUpdated
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course Topic Quiz with the specified ID" };

            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<GenericResponseModel> getAllCourseTopicQuizByCourseIdAsync(long courseId)
        {
            try
            {
                //check if the topicId is valid
                var checkResult = new CheckerValidation(_context).checkCourseById(courseId);
                if (checkResult == true)
                {
                    var result = from cr in _context.CourseTopicQuiz
                                 where cr.CourseId == courseId
                                 orderby cr.Id ascending
                                 select new
                                 {
                                     cr.Id,
                                     cr.CourseId,
                                     cr.CourseTopicId,
                                     cr.Description,
                                     cr.Duration,
                                     cr.PercentagePassMark,
                                     cr.Status,
                                     cr.DateCreated,
                                     cr.LastUpdated
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course Topic Quiz with the specified ID" };

            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<GenericResponseModel> getAllCourseTopicQuizByFacilitatorIdAsync(Guid facilitatorId)
        {
            try
            {
                //check if the topicId is valid
                var checkResult = new CheckerValidation(_context).checkFacilitatorById(facilitatorId);
                if (checkResult == true)
                {
                    var result = from cr in _context.CourseTopicQuiz
                                 where cr.Courses.FacilitatorId == facilitatorId
                                 orderby cr.Id ascending
                                 select new
                                 {
                                     cr.Id,
                                     cr.CourseId,
                                     cr.CourseTopicId,
                                     cr.Description,
                                     cr.Duration,
                                     cr.PercentagePassMark,
                                     cr.Status,
                                     cr.DateCreated,
                                     cr.LastUpdated
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course Topic Quiz with the specified ID" };

            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<GenericResponseModel> getCourseTopicQuizByIdAsync(long quizId)
        {
            try
            {
                //check if the topicId is valid
                var checkResult = new CheckerValidation(_context).checkCourseTopicQuizById(quizId);
                if (checkResult == true)
                {
                    var result = from cr in _context.CourseTopicQuiz
                                 where cr.Id == quizId
                                 orderby cr.Id ascending
                                 select new
                                 {
                                     cr.Id,
                                     cr.CourseId,
                                     cr.CourseTopicId,
                                     cr.Description,
                                     cr.Duration,
                                     cr.PercentagePassMark,
                                     cr.Status,
                                     cr.DateCreated,
                                     cr.LastUpdated
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course Topic Quiz with the specified ID" };

            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }
        //----------------------------CourseTopicQuizQuestion-------------------------------------------------------

        public async Task<GenericResponseModel> createCourseTopicQuizQuestionAsync(CourseTopicQuizQuestionRequestModel obj)
        {
            try
            {
                //check if the questionTypeId is valid
                var questionType = new CheckerValidation(_context).checkQuestionTypeById(obj.QuestionTypeId);
                if (questionType == false)
                {
                    return new GenericResponseModel { StatusCode = 404, StatusMessage = "Question Type Doesn't Exist" };
                }
                //check if the quizId is valid
                var quiz = new CheckerValidation(_context).checkCourseTopicQuizById(obj.CourseTopicQuizId);
                if(quiz == false)
                {
                    return new GenericResponseModel { StatusCode = 404, StatusMessage = "Course Topic Quiz Doesn't Exist" };
                }
                //check if a question to be created already exists
                var checkResult = _context.CourseTopicQuizQuestions.Where(x => x.CourseTopicQuizId == obj.CourseTopicQuizId && x.QuestionTypeId == obj.QuestionTypeId && x.Answer == obj.Answer && x.Question == obj.Question).FirstOrDefault();

                //if the course topic quiz question doesnt exist, Create the question
                if (checkResult == null)
                {
                    var courseTopicQuizQuestion = new CourseTopicQuizQuestions();
                    courseTopicQuizQuestion.CourseTopicQuizId = obj.CourseTopicQuizId;
                    courseTopicQuizQuestion.QuestionTypeId = obj.QuestionTypeId;
                    courseTopicQuizQuestion.Question = obj.Question;
                    courseTopicQuizQuestion.DateCreated = DateTime.Now;
                    courseTopicQuizQuestion.LastUpdated = DateTime.Now;

                    if (obj.QuestionTypeId == (int)EnumUtility.QuestionTypes.Fill_in_the_Gap)
                    {
                        courseTopicQuizQuestion.Answer = obj.Answer;
                    }
                    else if (obj.QuestionTypeId == (int)EnumUtility.QuestionTypes.Multiple_Choice)
                    {
                        courseTopicQuizQuestion.Answer = obj.Answer;
                        if (obj.Option1 == string.Empty || obj.Option2 == string.Empty || obj.Option3 == string.Empty || obj.Option4 == string.Empty)
                        {
                            return new GenericResponseModel { StatusCode = 400, StatusMessage = "All options must have value" };
                        }
                        else
                        {
                            courseTopicQuizQuestion.Option1 = obj.Option1;
                            courseTopicQuizQuestion.Option2 = obj.Option2;
                            courseTopicQuizQuestion.Option3 = obj.Option3;
                            courseTopicQuizQuestion.Option4 = obj.Option4;
                        }
                    }
                    else if (obj.QuestionTypeId == (int)EnumUtility.QuestionTypes.True_or_False)
                    {
                        if (obj.Answer.ToLower().Trim() == "true" || obj.Answer.ToLower().Trim() == "false")
                        {
                            courseTopicQuizQuestion.Answer = obj.Answer;
                        }
                        else
                        {
                            return new GenericResponseModel { StatusCode = 400, StatusMessage = "Invalid Answer, Answer must be true or false" };
                        }
                    }
                    await _context.CourseTopicQuizQuestions.AddAsync(courseTopicQuizQuestion);
                    await _context.SaveChangesAsync();

                    //get the Question Created
                    var getCourseTopicQuizQuestion = from cr in _context.CourseTopicQuizQuestions
                                                     where cr.Id == courseTopicQuizQuestion.Id
                                                     select new
                                                     {
                                                         cr.Id,
                                                         cr.Option1,
                                                         cr.Option2,
                                                         cr.Option3,
                                                         cr.Option4,
                                                         cr.Question,
                                                         cr.QuestionTypeId,
                                                         cr.QuestionTypes.QuestionTypeName,
                                                         cr.Answer,
                                                         cr.CourseTopicQuizId,
                                                         cr.CourseTopicQuiz.Description,
                                                         cr.DateCreated,
                                                         cr.LastUpdated
                                                     };

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Topic Quiz Question Created Successfully", Data = getCourseTopicQuizQuestion.FirstOrDefault() };

                }
                else
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Topic Quiz Question Already Exists" };
                }
            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<GenericResponseModel> createBulkCourseTopicQuizQuestionAsync(BulkCourseTopicQuizQuestionRequestModel obj)
        {
            IList<object> data = new List<object>();
            try
            {
                //check if the quizId is valid
                var quiz = new CheckerValidation(_context).checkCourseTopicQuizById(obj.CourseTopicQuizId);
                if (quiz == false)
                {
                    return new GenericResponseModel { StatusCode = 404, StatusMessage = "Course Topic Quiz Doesn't Exist" };
                }
                if (obj.Questions.Count > 0)
                {
                    foreach (CourseTopicRequestModel question in obj.Questions)
                    {
                        //check if the questionTypeId is valid
                        var questionType = new CheckerValidation(_context).checkQuestionTypeById(question.QuestionTypeId);
                        if (questionType == false)
                        {
                            data.Add(new GenericResponseModel { StatusCode = 405, StatusMessage = "Question Type Doesn't Exist" });
                            continue;
                        }

                        //check if a question to be created already exists
                        var checkResult = _context.CourseTopicQuizQuestions.Where(x => x.CourseTopicQuizId == obj.CourseTopicQuizId && x.QuestionTypeId == question.QuestionTypeId && x.Answer.ToLower() == question.Answer.ToLower() && x.Question.ToLower() == question.Question.ToLower()).FirstOrDefault();

                        //if the course quiz question doesnt exist, Create the question
                        if (checkResult == null)
                        {
                            var courseTopicQuizQuestion = new CourseTopicQuizQuestions();
                            courseTopicQuizQuestion.CourseTopicQuizId = obj.CourseTopicQuizId;
                            courseTopicQuizQuestion.QuestionTypeId = question.QuestionTypeId;
                            courseTopicQuizQuestion.Question = question.Question;
                            courseTopicQuizQuestion.DateCreated = DateTime.Now;
                            courseTopicQuizQuestion.LastUpdated = DateTime.Now;

                            if (question.QuestionTypeId == (int)EnumUtility.QuestionTypes.Fill_in_the_Gap)
                            {
                                courseTopicQuizQuestion.Answer = question.Answer;
                            }
                            else if (question.QuestionTypeId == (int)EnumUtility.QuestionTypes.Multiple_Choice)
                            {
                                courseTopicQuizQuestion.Answer = question.Answer;
                                if (question.Option1 == string.Empty || question.Option2 == string.Empty || question.Option3 == string.Empty || question.Option4 == string.Empty)
                                {
                                    data.Add(new GenericResponseModel { StatusCode = 403, StatusMessage = "All options must have value" });
                                    continue;
                                }
                                else
                                {
                                    courseTopicQuizQuestion.Option1 = question.Option1;
                                    courseTopicQuizQuestion.Option2 = question.Option2;
                                    courseTopicQuizQuestion.Option3 = question.Option3;
                                    courseTopicQuizQuestion.Option4 = question.Option4;
                                }
                            }
                            else if (question.QuestionTypeId == (int)EnumUtility.QuestionTypes.True_or_False)
                            {
                                if (question.Answer.ToLower().Trim() == "true" || question.Answer.ToLower().Trim() == "false")
                                {
                                    courseTopicQuizQuestion.Answer = question.Answer;
                                }
                                else
                                {
                                    data.Add(new GenericResponseModel { StatusCode = 401, StatusMessage = "Invalid Answer, Answer must be true or false" });
                                    continue;
                                }
                            }
                            await _context.CourseTopicQuizQuestions.AddAsync(courseTopicQuizQuestion);
                            await _context.SaveChangesAsync();

                            //get the Question Created
                            var getCourseQuizQuestion = from cr in _context.CourseTopicQuizQuestions
                                                        where cr.Id == courseTopicQuizQuestion.Id
                                                        select new
                                                        {
                                                            cr.Id,
                                                            cr.Option1,
                                                            cr.Option2,
                                                            cr.Option3,
                                                            cr.Option4,
                                                            cr.Question,
                                                            cr.QuestionTypeId,
                                                            cr.QuestionTypes.QuestionTypeName,
                                                            cr.Answer,
                                                            cr.CourseTopicQuizId,
                                                            cr.CourseTopicQuiz.Description,
                                                            cr.DateCreated,
                                                            cr.LastUpdated
                                                        };

                            data.Add(new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Topic Quiz Question Created Successfully", Data = getCourseQuizQuestion.FirstOrDefault() });

                        }
                        else
                        {
                            data.Add(new GenericResponseModel { StatusCode = 201, StatusMessage = "Course Topic Quiz Question Already Exists" });
                        }
                    }
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Success", Data = data.ToList() };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 400, StatusMessage = "No question was set for this quiz" };
                }
            }
            catch (Exception exMessage)
            {
                _context.CourseTopicQuizQuestions.Local.Clear();
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                data.Add(new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" });
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Success", Data = data.ToList() };
            }
        }
        public async Task<GenericResponseModel> createBulkCourseTopicQuizQuestionFromExcelAsync(BulkQuizQuestionRequestModel obj)
        {
            IList<object> data = new List<object>();
            try
            {
                //check if the quizId is valid
                var quiz = new CheckerValidation(_context).checkCourseTopicQuizById(obj.QuizId);
                if (quiz == false)
                {
                    return new GenericResponseModel { StatusCode = 404, StatusMessage = "Course Topic Quiz Doesn't Exist" };
                }
                //the file path
                var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", obj.File.FileName);
                //copy the file to the stream and read from the file
                using (var stream = new FileStream(FilePath, FileMode.Create))
                {
                    await obj.File.CopyToAsync(stream);
                }

                FileInfo existingFile = new FileInfo(FilePath);
                using (ExcelPackage package = new ExcelPackage(existingFile))
                {
                    //get the first worksheet in the workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    int colCount = worksheet.Dimension.Columns;  //get Column Count
                    int rowCount = worksheet.Dimension.Rows;     //get row count

                    for (int row = 2; row <= rowCount; row++) // starts from the second row (Jumping the table headings)
                    {
                        //check if the question or answer or question type is valid
                        if (worksheet.Cells[row, 1].Value != null && worksheet.Cells[row, 6].Value != null && worksheet.Cells[row, 7].Value != null)
                        {
                            //check if the questionTypeId is valid
                            var checkQuestionType = _context.QuestionTypes.Where(x => x.QuestionTypeName.ToLower().Trim() == worksheet.Cells[row, 7].Value.ToString().ToLower().Trim()).FirstOrDefault();
                            if (checkQuestionType != null)
                            {
                                string question = worksheet.Cells[row, 1].Value.ToString().Trim();
                                string answer = worksheet.Cells[row, 6].Value.ToString().Trim();
                                //check if a question to be created already exists
                                var checkResult = _context.CourseTopicQuizQuestions.Where(x => x.CourseTopicQuizId == obj.QuizId && x.QuestionTypeId == checkQuestionType.Id && x.Answer.ToLower() == answer.ToLower() && x.Question.ToLower() == question.ToLower()).FirstOrDefault();

                                //if the course topic quiz question doesnt exist, Create the question
                                if (checkResult == null)
                                {
                                    var courseTopicQuizQuestion = new CourseTopicQuizQuestions();
                                    courseTopicQuizQuestion.CourseTopicQuizId = obj.QuizId;
                                    courseTopicQuizQuestion.QuestionTypeId = checkQuestionType.Id;
                                    courseTopicQuizQuestion.Question = question;
                                    courseTopicQuizQuestion.DateCreated = DateTime.Now;
                                    courseTopicQuizQuestion.LastUpdated = DateTime.Now;

                                    if (checkQuestionType.Id == (int)EnumUtility.QuestionTypes.Fill_in_the_Gap)
                                    {
                                        courseTopicQuizQuestion.Answer = answer;
                                    }
                                    else if (checkQuestionType.Id == (int)EnumUtility.QuestionTypes.Multiple_Choice)
                                    {
                                        courseTopicQuizQuestion.Answer = answer;
                                        if (worksheet.Cells[row, 2].Value == null || worksheet.Cells[row, 3].Value == null || worksheet.Cells[row, 4].Value == null || worksheet.Cells[row, 5].Value == null)
                                        {
                                            data.Add(new GenericResponseModel { StatusCode = 403, StatusMessage = "All options must have value" });
                                            continue;
                                        }
                                        else
                                        {
                                            courseTopicQuizQuestion.Option1 = worksheet.Cells[row, 2].Value.ToString().Trim();
                                            courseTopicQuizQuestion.Option2 = worksheet.Cells[row, 3].Value.ToString().Trim();
                                            courseTopicQuizQuestion.Option3 = worksheet.Cells[row, 4].Value.ToString().Trim();
                                            courseTopicQuizQuestion.Option4 = worksheet.Cells[row, 5].Value.ToString().Trim();
                                        }
                                    }
                                    else if (checkQuestionType.Id == (int)EnumUtility.QuestionTypes.True_or_False)
                                    {
                                        if (answer.ToLower().Trim() == "true" || answer.ToLower().Trim() == "false")
                                        {
                                            courseTopicQuizQuestion.Answer = answer;
                                        }
                                        else
                                        {
                                            data.Add(new GenericResponseModel { StatusCode = 401, StatusMessage = "Invalid Answer, Answer must be true or false" });
                                            continue;
                                        }
                                    }
                                    await _context.CourseTopicQuizQuestions.AddAsync(courseTopicQuizQuestion);
                                    await _context.SaveChangesAsync();

                                    //get the Question Created
                                    var getCourseTopicQuizQuestion = from cr in _context.CourseTopicQuizQuestions
                                                                where cr.Id == courseTopicQuizQuestion.Id
                                                                select new
                                                                {
                                                                    cr.Id,
                                                                    cr.Option1,
                                                                    cr.Option2,
                                                                    cr.Option3,
                                                                    cr.Option4,
                                                                    cr.Question,
                                                                    cr.QuestionTypeId,
                                                                    cr.QuestionTypes.QuestionTypeName,
                                                                    cr.Answer,
                                                                    cr.CourseTopicQuizId,
                                                                    cr.CourseTopicQuiz.Description,
                                                                    cr.DateCreated,
                                                                    cr.LastUpdated
                                                                };

                                    data.Add(new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Topic Quiz Question Created Successfully", Data = getCourseTopicQuizQuestion.FirstOrDefault() });

                                }
                                else
                                {
                                    data.Add(new GenericResponseModel { StatusCode = 201, StatusMessage = "Course Topic Quiz Question Already Exists" });
                                }

                            }
                            else
                            {
                                data.Add(new GenericResponseModel { StatusCode = 202, StatusMessage = "Question Type Doesn't Exist" });
                            }
                        }
                        else
                        {
                            data.Add(new GenericResponseModel { StatusCode = 203, StatusMessage = "Question or Answer or Question type invalid" });
                        }
                    }
                }
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Success", Data = data.ToList() };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                data.Add(new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" });
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Success", Data = data.ToList() };
            }
        }
        public async Task<GenericResponseModel> deleteCourseTopicQuizQuestionAsync(long questionId)
        {
            try
            {
                var obj = _context.CourseTopicQuizQuestions.Where(x => x.Id == questionId).FirstOrDefault();
                if (obj != null)
                {
                    _context.CourseTopicQuizQuestions.Remove(obj);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Deleted Successfully!" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Question With the Specified ID!" };
            }

            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }


        public async Task<GenericResponseModel> getAllCourseTopicQuizQuestionAsync()
        {
            try
            {
                var result = from cr in _context.CourseTopicQuizQuestions
                              select new
                              {
                                  cr.Id,
                                  cr.Option1,
                                  cr.Option2,
                                  cr.Option3,
                                  cr.Option4,
                                  cr.Question,
                                  cr.QuestionTypeId,
                                  cr.QuestionTypes.QuestionTypeName,
                                  cr.Answer,
                                  cr.CourseTopicQuizId,
                                  cr.CourseTopicQuiz.Description,
                                  cr.DateCreated,
                                  cr.LastUpdated
                              };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful!", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record!" };

            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getAllCourseTopicQuizQuestionByQuizIdAsync(long quizId)
        {
            try
            {
                //check if the quizId is valid
                var checkResult = new CheckerValidation(_context).checkCourseTopicQuizById(quizId);
                if (checkResult == true)
                {
                    var result = from cr in _context.CourseTopicQuizQuestions
                                  where cr.CourseTopicQuizId == quizId
                                  orderby cr.Id ascending
                                  select new
                                  {
                                      cr.Id,
                                      cr.Option1,
                                      cr.Option2,
                                      cr.Option3,
                                      cr.Option4,
                                      cr.Question,
                                      cr.QuestionTypeId,
                                      cr.QuestionTypes.QuestionTypeName,
                                      cr.Answer,
                                      cr.CourseTopicQuizId,
                                      cr.CourseTopicQuiz.Description,
                                      cr.DateCreated,
                                      cr.LastUpdated
                                  };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course Topic Quiz with the specified ID" };

            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<GenericResponseModel> getCourseTopicQuizQuestionByIdAsync(long questionId)
        {
            try
            {
                //check if record exists
                var checkResult = _context.CourseTopicQuizQuestions.Where(x => x.Id == questionId).FirstOrDefault();

                if (checkResult != null)
                {
                    var result = from cr in _context.CourseTopicQuizQuestions
                                 where cr.Id == questionId
                                 select new
                                 {
                                     cr.Id,
                                     cr.Option1,
                                     cr.Option2,
                                     cr.Option3,
                                     cr.Option4,
                                     cr.Question,
                                     cr.QuestionTypeId,
                                     cr.QuestionTypes.QuestionTypeName,
                                     cr.Answer,
                                     cr.CourseTopicQuizId,
                                     cr.CourseTopicQuiz.Description,
                                     cr.DateCreated,
                                     cr.LastUpdated
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course with the specified ID" };

            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }


        public async Task<GenericResponseModel> updateCourseTopicQuizQuestionAsync(long questionId, CourseTopicQuizQuestionRequestModel obj)
        {
            try
            {
                //check if the questionTypeId is valid
                var questionType = new CheckerValidation(_context).checkQuestionTypeById(obj.QuestionTypeId);
                if (questionType == false)
                {
                    return new GenericResponseModel { StatusCode = 404, StatusMessage = "Question Type Doesn't Exist" };
                }
                //check if the quizId is valid
                var quiz = new CheckerValidation(_context).checkCourseTopicQuizById(obj.CourseTopicQuizId);
                if (quiz == false)
                {
                    return new GenericResponseModel { StatusCode = 404, StatusMessage = "Course Topic Quiz Doesn't Exist" };
                }
                //check if a course topic quiz to be updated exists
                var checkResult = _context.CourseTopicQuizQuestions.Where(x => x.Id == questionId).FirstOrDefault();

                //if the record exist, Update the record
                if (checkResult != null)
                {
                    checkResult.CourseTopicQuizId = obj.CourseTopicQuizId;
                    checkResult.QuestionTypeId = obj.QuestionTypeId;
                    checkResult.Question = obj.Question;
                    checkResult.LastUpdated = DateTime.Now;

                    if (obj.QuestionTypeId == (int)EnumUtility.QuestionTypes.Fill_in_the_Gap)
                    {
                        checkResult.Answer = obj.Answer;
                        checkResult.Option1 = null;
                        checkResult.Option2 = null;
                        checkResult.Option3 = null;
                        checkResult.Option4 = null;
                    }
                    else if (obj.QuestionTypeId == (int)EnumUtility.QuestionTypes.Multiple_Choice)
                    {
                        checkResult.Answer = obj.Answer;
                        if (obj.Option1 == string.Empty || obj.Option2 == string.Empty || obj.Option3 == string.Empty || obj.Option4 == string.Empty)
                        {
                            return new GenericResponseModel { StatusCode = 400, StatusMessage = "All options must have value" };
                        }
                        else
                        {
                            checkResult.Option1 = obj.Option1;
                            checkResult.Option2 = obj.Option2;
                            checkResult.Option3 = obj.Option3;
                            checkResult.Option4 = obj.Option4;
                        }
                    }
                    else if (obj.QuestionTypeId == (int)EnumUtility.QuestionTypes.True_or_False)
                    {
                        if (obj.Answer.ToLower().Trim() == "true" || obj.Answer.ToLower().Trim() == "false")
                        {
                            checkResult.Answer = obj.Answer;
                            checkResult.Option1 = null;
                            checkResult.Option2 = null;
                            checkResult.Option3 = null;
                            checkResult.Option4 = null;
                        }
                        else
                        {
                            return new GenericResponseModel { StatusCode = 400, StatusMessage = "Invalid Answer, Answer must be true or false" };
                        }
                    }
                    await _context.SaveChangesAsync();

                    //get the Updated Question
                    var getCourseTopicQuizQuestion = from cr in _context.CourseTopicQuizQuestions
                                                     where cr.Id == questionId
                                                     select new
                                                     {
                                                         cr.Id,
                                                         cr.Option1,
                                                         cr.Option2,
                                                         cr.Option3,
                                                         cr.Option4,
                                                         cr.Question,
                                                         cr.QuestionTypeId,
                                                         cr.QuestionTypes.QuestionTypeName,
                                                         cr.Answer,
                                                         cr.CourseTopicQuizId,
                                                         cr.CourseTopicQuiz.Description,
                                                         cr.DateCreated,
                                                         cr.LastUpdated
                                                     };

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Topic Quiz Question Updated Successfully", Data = getCourseTopicQuizQuestion.FirstOrDefault() };

                }
                else
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Record Not Found" };
                }
            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        //----------------------------CourseTopicQuizAnswer-------------------------------------------------------
        public async Task<GenericResponseModel> createCourseTopicQuizResultAsync(CourseTopicQuizResultRequestModel obj)
        {
            try
            {
                var checkResult = new CheckerValidation(_context).checkLearnerById(obj.LearnerId);
                if (checkResult == false)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Learner Doesn't Exist" };
                }
                var courseTopicQuiz = await _context.CourseTopicQuiz.Where(x => x.Id == obj.CourseTopicQuizId).FirstOrDefaultAsync();
                if (courseTopicQuiz == null)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Topic Quiz Doesn't Exist" };
                }

                IList<CourseTopicQuizQuestions> questions = await _context.CourseTopicQuizQuestions.Where(x => x.CourseTopicQuizId == obj.CourseTopicQuizId).ToListAsync();
                if(obj.Data.Count > questions.Count)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "You answered more than the expected questions" };
                }
                int rightAnswers = 0;
                int wrongAnswers = 0;
                int invalidQuestions = 0;
                int totalQuestions = questions.Count;
                string status = string.Empty;
                decimal percentageScore = 0;
                if (questions.Count > 0)
                {
                    foreach (Response response in obj.Data)
                    {
                        CourseTopicQuizQuestions question = await _context.CourseTopicQuizQuestions.Where(x => x.Id == response.QuestionId && x.CourseTopicQuizId == obj.CourseTopicQuizId).FirstOrDefaultAsync();
                        if (question != null)
                        {
                            if (response.Answer.ToLower().Trim() == question.Answer.ToLower().Trim())
                            {
                                rightAnswers++;
                            }
                            else
                            {
                                wrongAnswers++;
                            }
                        }
                        else
                        {
                            invalidQuestions++;
                        }
                    }
                    percentageScore = (rightAnswers * 100) / totalQuestions;
                    if (percentageScore >= courseTopicQuiz.PercentagePassMark)
                    {
                        status = "Pass";
                    }
                    else
                    {
                        status = "Fail";
                    }
                    CourseTopicQuizResults quizResults = new CourseTopicQuizResults();
                    quizResults.CourseTopicQuizId = obj.CourseTopicQuizId;
                    quizResults.DateTaken = DateTime.Now;
                    quizResults.LearnerId = obj.LearnerId;
                    quizResults.NoOfQuestions = totalQuestions;
                    quizResults.RightAnswers = rightAnswers;
                    quizResults.WrongAnswers = wrongAnswers;
                    quizResults.InvalidQuestions = invalidQuestions;
                    quizResults.PercentageScore = percentageScore;
                    quizResults.Status = status;
                    quizResults.Score = rightAnswers;

                    await _context.CourseTopicQuizResults.AddAsync(quizResults);
                    await _context.SaveChangesAsync();

                    //get the Course Topic Quiz Result Created
                    var getCourseTopicQuiz = from cr in _context.CourseTopicQuizResults
                                             where cr.Id == quizResults.Id
                                             select new
                                             {
                                                 cr.CourseTopicQuizId,
                                                 cr.DateTaken,
                                                 cr.Id,
                                                 cr.LearnerId,
                                                 cr.NoOfQuestions,
                                                 cr.PercentageScore,
                                                 cr.RightAnswers,
                                                 cr.Score,
                                                 cr.Status,
                                                 cr.WrongAnswers,
                                                 cr.InvalidQuestions
                                             };

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Result Computed Successfully", Data = getCourseTopicQuiz.FirstOrDefault() };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No questions for the selected quiz" };
                }
            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<GenericResponseModel> getAllCourseTopicQuizResultByLearnerIdAsync(Guid learnerId)
        {
            try
            {
                //check if the learner is valid
                var checkResult = new CheckerValidation(_context).checkLearnerById(learnerId);
                if (checkResult == true)
                {
                    var result = from cr in _context.CourseTopicQuizResults
                                 where cr.LearnerId == learnerId
                                 orderby cr.Id ascending
                                 select new
                                 {
                                     cr.CourseTopicQuizId,
                                     cr.DateTaken,
                                     cr.Id,
                                     cr.LearnerId,
                                     cr.NoOfQuestions,
                                     cr.PercentageScore,
                                     cr.RightAnswers,
                                     cr.Score,
                                     cr.Status,
                                     cr.WrongAnswers
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course Topic Quiz with the specified ID" };

            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<GenericResponseModel> getCourseTopicQuizResultByIdAsync(long resultId)
        {
            try
            {
                //check if the resultId is valid
                var checkResult = new CheckerValidation(_context).checkCourseTopicQuizResultById(resultId);
                if (checkResult == true)
                {
                    var result = from cr in _context.CourseTopicQuizResults
                                 where cr.Id == resultId
                                 orderby cr.Id ascending
                                 select new
                                 {
                                     cr.CourseTopicQuizId,
                                     cr.DateTaken,
                                     cr.Id,
                                     cr.LearnerId,
                                     cr.NoOfQuestions,
                                     cr.PercentageScore,
                                     cr.RightAnswers,
                                     cr.Score,
                                     cr.Status,
                                     cr.WrongAnswers
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course Topic Quiz with the specified ID" };

            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }
    }
}
