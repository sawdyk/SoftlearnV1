using Microsoft.AspNetCore.Http;
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
    public class CourseQuizRepo : ICourseQuizRepo
    {
        private readonly AppDbContext _context;

        public CourseQuizRepo(AppDbContext context)
        {
            this._context = context;
        }
        //----------------------------CourseQuiz---------------------------------------------------------------

        public async Task<GenericResponseModel> createCourseQuizAsync(CourseQuizRequestModel obj)
        {
            try
            {
                //check if course exist
                var course = await _context.Courses.Where(x => x.Id == obj.CourseId).FirstOrDefaultAsync();
                if (course == null)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course doesn't Exists" };
                }
                //check if a course quiz to be created already exists
                var checkResult = _context.CourseQuiz.Where(x => x.CourseId == obj.CourseId).FirstOrDefault();

                //if the course quiz doesnt exist, Create the course quiz
                if (checkResult == null)
                {
                    var courseQuiz = new CourseQuiz
                    {
                        CourseId = obj.CourseId,
                        Description = obj.Description,
                        Duration = obj.Duration,
                        LastUpdated = DateTime.Now,
                        PercentagePassMark = obj.PercentagePassMark,
                        DateCreated = DateTime.Now,
                        Status = true
                    };

                    await _context.CourseQuiz.AddAsync(courseQuiz);
                    await _context.SaveChangesAsync();

                    //get the Course Quiz Created
                    var getCourseQuiz = from cr in _context.CourseQuiz
                                        where cr.Id == courseQuiz.Id
                                        select new
                                        {
                                            cr.Id,
                                            cr.CourseId,
                                            cr.DateCreated,
                                            cr.LastUpdated,
                                            cr.PercentagePassMark,
                                            cr.Status,
                                            cr.Description,
                                            cr.Duration
                                        };

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Quiz Created Successfully", Data = getCourseQuiz.FirstOrDefault() };

                }
                else
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Quiz Already Exists" };
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

        public async Task<GenericResponseModel> updateCourseQuizAsync(long quizId, CourseQuizRequestModel obj)
        {
            try
            {
                //check if course exist
                var course = await _context.Courses.Where(x => x.Id == obj.CourseId).FirstOrDefaultAsync();
                if (course == null)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course doesn't Exists" };
                }
                //check if a course quiz to be edited exists
                var checkResult = _context.CourseQuiz.Where(x => x.Id == quizId).FirstOrDefault();

                //if the course quiz exist, Update the course quiz
                if (checkResult != null)
                {
                    checkResult.CourseId = obj.CourseId;
                    checkResult.Description = obj.Description;
                    checkResult.Duration = obj.Duration;
                    checkResult.LastUpdated = DateTime.Now;
                    checkResult.PercentagePassMark = obj.PercentagePassMark;
                    checkResult.Status = obj.Status;
                    await _context.SaveChangesAsync();

                    //get the Course Quiz Updated
                    var getCourseQuiz = from cr in _context.CourseQuiz
                                        where cr.Id == quizId
                                        select new
                                        {
                                            cr.Id,
                                            cr.CourseId,
                                            cr.DateCreated,
                                            cr.LastUpdated,
                                            cr.PercentagePassMark,
                                            cr.Status,
                                            cr.Description,
                                            cr.Duration
                                        };

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Quiz Updated Successfully", Data = getCourseQuiz.FirstOrDefault() };

                }
                else
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Quiz Doesn't Exists" };
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

        public async Task<GenericResponseModel> deleteCourseQuizAsync(long quizId)
        {
            try
            {
                var questions = _context.CourseQuizQuestions.Where(x => x.CourseQuizId == quizId).ToList();
                var quizResult = _context.CourseQuizResults.Where(x => x.CourseQuizId == quizId).ToList();
                if (questions.Count <= 0 && quizResult.Count <= 0)
                {
                    var obj = _context.CourseQuiz.Where(x => x.Id == quizId).FirstOrDefault();
                    if (obj != null)
                    {
                        _context.CourseQuiz.Remove(obj);
                        await _context.SaveChangesAsync();

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Deleted Successfully!" };
                    }

                    return new GenericResponseModel { StatusCode = 300, StatusMessage = "No Quiz With the Specified ID!" };
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

        public async Task<GenericResponseModel> getCourseQuizByCourseIdAsync(long courseId)
        {
            try
            {
                //check if the courseId is valid
                var checkResult = new CheckerValidation(_context).checkCourseById(courseId);
                if (checkResult == true)
                {
                    var result = from cr in _context.CourseQuiz
                                 where cr.CourseId == courseId
                                 orderby cr.Id ascending
                                 select new
                                 {
                                     cr.Id,
                                     cr.CourseId,
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

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course Quiz with the specified ID" };

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


        public async Task<GenericResponseModel> getAllCourseQuizByFacilitatorIdAsync(Guid facilitatorId)
        {
            try
            {
                //check if the courseId is valid
                var checkResult = new CheckerValidation(_context).checkFacilitatorById(facilitatorId);
                if (checkResult == true)
                {
                    var result = from cr in _context.CourseQuiz
                                 where cr.Courses.FacilitatorId == facilitatorId
                                 orderby cr.Id ascending
                                 select new
                                 {
                                     cr.Id,
                                     cr.CourseId,
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

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Facilitator with the specified ID" };

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

        public async Task<GenericResponseModel> getCourseQuizByIdAsync(long quizId)
        {
            try
            {
                //check if the quizId is valid
                var checkResult = new CheckerValidation(_context).checkCourseQuizById(quizId);
                if (checkResult == true)
                {
                    var result = from cr in _context.CourseQuiz
                                 where cr.Id == quizId
                                 orderby cr.Id ascending
                                 select new
                                 {
                                     cr.Id,
                                     cr.CourseId,
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

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course Quiz with the specified ID" };

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
        //----------------------------CourseQuizQuestion-------------------------------------------------------

        public async Task<GenericResponseModel> createCourseQuizQuestionAsync(CourseQuizQuestionRequestModel obj)
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
                var quiz = new CheckerValidation(_context).checkCourseQuizById(obj.CourseQuizId);
                if (quiz == false)
                {
                    return new GenericResponseModel { StatusCode = 404, StatusMessage = "Course Quiz Doesn't Exist" };
                }
                //check if a question to be created already exists
                var checkResult = _context.CourseQuizQuestions.Where(x => x.CourseQuizId == obj.CourseQuizId && x.QuestionTypeId == obj.QuestionTypeId && x.Answer == obj.Answer && x.Question == obj.Question).FirstOrDefault();

                //if the course quiz question doesnt exist, Create the question
                if (checkResult == null)
                {
                    var courseQuizQuestion = new CourseQuizQuestions();
                    courseQuizQuestion.CourseQuizId = obj.CourseQuizId;
                    courseQuizQuestion.QuestionTypeId = obj.QuestionTypeId;
                    courseQuizQuestion.Question = obj.Question;
                    courseQuizQuestion.DateCreated = DateTime.Now;
                    courseQuizQuestion.LastUpdated = DateTime.Now;

                    if (obj.QuestionTypeId == (int)EnumUtility.QuestionTypes.Fill_in_the_Gap)
                    {
                        courseQuizQuestion.Answer = obj.Answer;
                    }
                    else if (obj.QuestionTypeId == (int)EnumUtility.QuestionTypes.Multiple_Choice)
                    {
                        courseQuizQuestion.Answer = obj.Answer;
                        if (obj.Option1 == string.Empty || obj.Option2 == string.Empty || obj.Option3 == string.Empty || obj.Option4 == string.Empty)
                        {
                            return new GenericResponseModel { StatusCode = 400, StatusMessage = "All options must have value" };
                        }
                        else
                        {
                            courseQuizQuestion.Option1 = obj.Option1;
                            courseQuizQuestion.Option2 = obj.Option2;
                            courseQuizQuestion.Option3 = obj.Option3;
                            courseQuizQuestion.Option4 = obj.Option4;
                        }
                    }
                    else if (obj.QuestionTypeId == (int)EnumUtility.QuestionTypes.True_or_False)
                    {
                        if (obj.Answer.ToLower().Trim() == "true" || obj.Answer.ToLower().Trim() == "false")
                        {
                            courseQuizQuestion.Answer = obj.Answer;
                        }
                        else
                        {
                            return new GenericResponseModel { StatusCode = 400, StatusMessage = "Invalid Answer, Answer must be true or false" };
                        }
                    }
                    await _context.CourseQuizQuestions.AddAsync(courseQuizQuestion);
                    await _context.SaveChangesAsync();

                    //get the Question Created
                    var getCourseQuizQuestion = from cr in _context.CourseQuizQuestions
                                                where cr.Id == courseQuizQuestion.Id
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
                                                    cr.CourseQuizId,
                                                    cr.CourseQuiz.Description,
                                                    cr.DateCreated,
                                                    cr.LastUpdated
                                                };

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Quiz Question Created Successfully", Data = getCourseQuizQuestion.FirstOrDefault() };

                }
                else
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Quiz Question Already Exists" };
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

        public async Task<GenericResponseModel> createBulkCourseQuizQuestionAsync(BulkCourseQuizQuestionRequestModel obj)
        {
            IList<object> data = new List<object>();
            try
            {
                //check if the quizId is valid
                var quiz = new CheckerValidation(_context).checkCourseQuizById(obj.CourseQuizId);
                if (quiz == false)
                {
                    return new GenericResponseModel { StatusCode = 404, StatusMessage = "Course Quiz Doesn't Exist" };
                }
                if (obj.Questions.Count > 0)
                {
                    foreach (CourseRequestModel question in obj.Questions)
                    {
                        //check if the questionTypeId is valid
                        var questionType = new CheckerValidation(_context).checkQuestionTypeById(question.QuestionTypeId);
                        if (questionType == false)
                        {
                            data.Add(new GenericResponseModel { StatusCode = 405, StatusMessage = "Question Type Doesn't Exist" });
                            continue;
                        }

                        //check if a question to be created already exists
                        var checkResult = _context.CourseQuizQuestions.Where(x => x.CourseQuizId == obj.CourseQuizId && x.QuestionTypeId == question.QuestionTypeId && x.Answer.ToLower() == question.Answer.ToLower() && x.Question.ToLower() == question.Question.ToLower()).FirstOrDefault();

                        //if the course quiz question doesnt exist, Create the question
                        if (checkResult == null)
                        {
                            var courseQuizQuestion = new CourseQuizQuestions();
                            courseQuizQuestion.CourseQuizId = obj.CourseQuizId;
                            courseQuizQuestion.QuestionTypeId = question.QuestionTypeId;
                            courseQuizQuestion.Question = question.Question;
                            courseQuizQuestion.DateCreated = DateTime.Now;
                            courseQuizQuestion.LastUpdated = DateTime.Now;

                            if (question.QuestionTypeId == (int)EnumUtility.QuestionTypes.Fill_in_the_Gap)
                            {
                                courseQuizQuestion.Answer = question.Answer;
                            }
                            else if (question.QuestionTypeId == (int)EnumUtility.QuestionTypes.Multiple_Choice)
                            {
                                courseQuizQuestion.Answer = question.Answer;
                                if (question.Option1 == string.Empty || question.Option2 == string.Empty || question.Option3 == string.Empty || question.Option4 == string.Empty)
                                {
                                    data.Add(new GenericResponseModel { StatusCode = 403, StatusMessage = "All options must have value" });
                                    continue;
                                }
                                else
                                {
                                    courseQuizQuestion.Option1 = question.Option1;
                                    courseQuizQuestion.Option2 = question.Option2;
                                    courseQuizQuestion.Option3 = question.Option3;
                                    courseQuizQuestion.Option4 = question.Option4;
                                }
                            }
                            else if (question.QuestionTypeId == (int)EnumUtility.QuestionTypes.True_or_False)
                            {
                                if (question.Answer.ToLower().Trim() == "true" || question.Answer.ToLower().Trim() == "false")
                                {
                                    courseQuizQuestion.Answer = question.Answer;
                                }
                                else
                                {
                                    data.Add(new GenericResponseModel { StatusCode = 401, StatusMessage = "Invalid Answer, Answer must be true or false" });
                                    continue;
                                }
                            }
                            await _context.CourseQuizQuestions.AddAsync(courseQuizQuestion);
                            await _context.SaveChangesAsync();

                            //get the Question Created
                            var getCourseQuizQuestion = from cr in _context.CourseQuizQuestions
                                                        where cr.Id == courseQuizQuestion.Id
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
                                                            cr.CourseQuizId,
                                                            cr.CourseQuiz.Description,
                                                            cr.DateCreated,
                                                            cr.LastUpdated
                                                        };

                            data.Add(new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Quiz Question Created Successfully", Data = getCourseQuizQuestion.FirstOrDefault() });

                        }
                        else
                        {
                            data.Add(new GenericResponseModel { StatusCode = 201, StatusMessage = "Course Quiz Question Already Exists" });
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
                _context.CourseQuizQuestions.Local.Clear();
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
        public async Task<GenericResponseModel> createBulkCourseQuizQuestionFromExcelAsync(BulkQuizQuestionRequestModel obj)
        {
            IList<object> data = new List<object>();
            try
            {
                //check if the quizId is valid
                var quiz = new CheckerValidation(_context).checkCourseQuizById(obj.QuizId);
                if (quiz == false)
                {
                    return new GenericResponseModel { StatusCode = 404, StatusMessage = "Course Quiz Doesn't Exist" };
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
                                var checkResult = _context.CourseQuizQuestions.Where(x => x.CourseQuizId == obj.QuizId && x.QuestionTypeId == checkQuestionType.Id && x.Answer.ToLower() == answer.ToLower() && x.Question.ToLower() == question.ToLower()).FirstOrDefault();

                                //if the course quiz question doesnt exist, Create the question
                                if (checkResult == null)
                                {
                                    var courseQuizQuestion = new CourseQuizQuestions();
                                    courseQuizQuestion.CourseQuizId = obj.QuizId;
                                    courseQuizQuestion.QuestionTypeId = checkQuestionType.Id;
                                    courseQuizQuestion.Question = question;
                                    courseQuizQuestion.DateCreated = DateTime.Now;
                                    courseQuizQuestion.LastUpdated = DateTime.Now;

                                    if (checkQuestionType.Id == (int)EnumUtility.QuestionTypes.Fill_in_the_Gap)
                                    {
                                        courseQuizQuestion.Answer = answer;
                                    }
                                    else if (checkQuestionType.Id == (int)EnumUtility.QuestionTypes.Multiple_Choice)
                                    {
                                        courseQuizQuestion.Answer = answer;
                                        if (worksheet.Cells[row, 2].Value == null || worksheet.Cells[row, 3].Value == null || worksheet.Cells[row, 4].Value == null || worksheet.Cells[row, 5].Value == null)
                                        {
                                            data.Add(new GenericResponseModel { StatusCode = 403, StatusMessage = "All options must have value" });
                                            continue;
                                        }
                                        else
                                        {
                                            courseQuizQuestion.Option1 = worksheet.Cells[row, 2].Value.ToString().Trim();
                                            courseQuizQuestion.Option2 = worksheet.Cells[row, 3].Value.ToString().Trim();
                                            courseQuizQuestion.Option3 = worksheet.Cells[row, 4].Value.ToString().Trim();
                                            courseQuizQuestion.Option4 = worksheet.Cells[row, 5].Value.ToString().Trim();
                                        }
                                    }
                                    else if (checkQuestionType.Id == (int)EnumUtility.QuestionTypes.True_or_False)
                                    {
                                        if (answer.ToLower().Trim() == "true" || answer.ToLower().Trim() == "false")
                                        {
                                            courseQuizQuestion.Answer = answer;
                                        }
                                        else
                                        {
                                            data.Add(new GenericResponseModel { StatusCode = 401, StatusMessage = "Invalid Answer, Answer must be true or false" });
                                            continue;
                                        }
                                    }
                                    await _context.CourseQuizQuestions.AddAsync(courseQuizQuestion);
                                    await _context.SaveChangesAsync();

                                    //get the Question Created
                                    var getCourseQuizQuestion = from cr in _context.CourseQuizQuestions
                                                                where cr.Id == courseQuizQuestion.Id
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
                                                                    cr.CourseQuizId,
                                                                    cr.CourseQuiz.Description,
                                                                    cr.DateCreated,
                                                                    cr.LastUpdated
                                                                };

                                    data.Add(new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Quiz Question Created Successfully", Data = getCourseQuizQuestion.FirstOrDefault() });

                                }
                                else
                                {
                                    data.Add(new GenericResponseModel { StatusCode = 201, StatusMessage = "Course Quiz Question Already Exists" });
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
        public async Task<GenericResponseModel> deleteCourseQuizQuestionAsync(long questionId)
        {
            try
            {
                var obj = _context.CourseQuizQuestions.Where(x => x.Id == questionId).FirstOrDefault();
                if (obj != null)
                {
                    _context.CourseQuizQuestions.Remove(obj);
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


        public async Task<GenericResponseModel> getAllCourseQuizQuestionAsync()
        {
            try
            {
                var result = from cr in _context.CourseQuizQuestions
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
                                 cr.CourseQuizId,
                                 cr.CourseQuiz.Description,
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

        public async Task<GenericResponseModel> getAllCourseQuizQuestionByQuizIdAsync(long quizId)
        {
            try
            {
                //check if the quizId is valid
                var checkResult = new CheckerValidation(_context).checkCourseQuizById(quizId);
                if (checkResult == true)
                {
                    var result = from cr in _context.CourseQuizQuestions
                                 where cr.CourseQuizId == quizId
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
                                     cr.CourseQuizId,
                                     cr.CourseQuiz.Description,
                                     cr.DateCreated,
                                     cr.LastUpdated
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course Quiz with the specified ID" };

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

        public async Task<GenericResponseModel> getCourseQuizQuestionByIdAsync(long questionId)
        {
            try
            {
                //check if record exists
                var checkResult = _context.CourseQuizQuestions.Where(x => x.Id == questionId).FirstOrDefault();

                if (checkResult != null)
                {
                    var result = from cr in _context.CourseQuizQuestions
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
                                     cr.CourseQuizId,
                                     cr.CourseQuiz.Description,
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


        public async Task<GenericResponseModel> updateCourseQuizQuestionAsync(long questionId, CourseQuizQuestionRequestModel obj)
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
                var quiz = new CheckerValidation(_context).checkCourseQuizById(obj.CourseQuizId);
                if (quiz == false)
                {
                    return new GenericResponseModel { StatusCode = 404, StatusMessage = "Course Quiz Doesn't Exist" };
                }
                //check if a course quiz to be updated exists
                var checkResult = _context.CourseQuizQuestions.Where(x => x.Id == questionId).FirstOrDefault();

                //if the record exist, Update the record
                if (checkResult != null)
                {
                    checkResult.CourseQuizId = obj.CourseQuizId;
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
                    var getCourseQuizQuestion = from cr in _context.CourseQuizQuestions
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
                                                    cr.CourseQuizId,
                                                    cr.CourseQuiz.Description,
                                                    cr.DateCreated,
                                                    cr.LastUpdated
                                                };

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Quiz Question Updated Successfully", Data = getCourseQuizQuestion.FirstOrDefault() };

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

        //----------------------------CourseQuizResult-------------------------------------------------------
        public async Task<GenericResponseModel> createCourseQuizResultAsync(CourseQuizResultRequestModel obj)
        {
            try
            {
                var checkResult = new CheckerValidation(_context).checkLearnerById(obj.LearnerId);
                if (checkResult == false)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Learner Doesn't Exist" };
                }
                var courseQuiz = await _context.CourseQuiz.Where(x => x.Id == obj.CourseQuizId).FirstOrDefaultAsync();
                if (courseQuiz == null)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Quiz Doesn't Exist" };
                }

                IList<CourseQuizQuestions> questions = await _context.CourseQuizQuestions.Where(x => x.CourseQuizId == obj.CourseQuizId).ToListAsync();
                if (obj.Data.Count > questions.Count)
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
                    foreach (CourseResponse response in obj.Data)
                    {
                        CourseQuizQuestions question = await _context.CourseQuizQuestions.Where(x => x.Id == response.QuestionId && x.CourseQuizId == obj.CourseQuizId).FirstOrDefaultAsync();
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
                    if (percentageScore >= courseQuiz.PercentagePassMark)
                    {
                        status = "Pass";
                    }
                    else
                    {
                        status = "Fail";
                    }
                    CourseQuizResults quizResults = new CourseQuizResults();
                    quizResults.CourseQuizId = obj.CourseQuizId;
                    quizResults.DateTaken = DateTime.Now;
                    quizResults.LearnerId = obj.LearnerId;
                    quizResults.NoOfQuestions = totalQuestions;
                    quizResults.RightAnswers = rightAnswers;
                    quizResults.WrongAnswers = wrongAnswers;
                    quizResults.InvalidQuestions = invalidQuestions;
                    quizResults.PercentageScore = percentageScore;
                    quizResults.Status = status;
                    quizResults.Score = rightAnswers;

                    await _context.CourseQuizResults.AddAsync(quizResults);
                    await _context.SaveChangesAsync();

                    //get the Course Quiz Result Created
                    var getCourseQuiz = from cr in _context.CourseQuizResults
                                        where cr.Id == quizResults.Id
                                        select new
                                        {
                                            cr.CourseQuizId,
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

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Result Computed Successfully", Data = getCourseQuiz.FirstOrDefault() };
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

        public async Task<GenericResponseModel> getAllCourseQuizResultByLearnerIdAsync(Guid learnerId)
        {
            try
            {
                //check if the learner is valid
                var checkResult = new CheckerValidation(_context).checkLearnerById(learnerId);
                if (checkResult == true)
                {
                    var result = from cr in _context.CourseQuizResults
                                 where cr.LearnerId == learnerId
                                 orderby cr.Id ascending
                                 select new
                                 {
                                     cr.CourseQuizId,
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

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course Quiz with the specified ID" };

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

        public async Task<GenericResponseModel> getCourseQuizResultByIdAsync(long resultId)
        {
            try
            {
                //check if the resultId is valid
                var checkResult = new CheckerValidation(_context).checkCourseQuizResultById(resultId);
                if (checkResult == true)
                {
                    var result = from cr in _context.CourseQuizResults
                                 where cr.Id == resultId
                                 orderby cr.Id ascending
                                 select new
                                 {
                                     cr.CourseQuizId,
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

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course Quiz with the specified ID" };

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
