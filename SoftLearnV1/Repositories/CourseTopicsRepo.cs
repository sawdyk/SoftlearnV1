using SoftLearnV1.Entities;
using SoftLearnV1.Helpers;
using SoftLearnV1.InterfaceRepositories;
using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using SoftLearnV1.Reusables;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using SoftLearnV1.Services.Cloudinary;
using CloudinaryDotNet;
using SoftLearnV1.Utilities;
using System.IO;

namespace SoftLearnV1.Repositories
{
    public class CourseTopicsRepo : ICourseTopicsRepo
    {
        private readonly AppDbContext _context;
        private readonly ICloudinaryRepo _cloudinary;
        private readonly Duration duration;

        public CourseTopicsRepo(AppDbContext context, ICloudinaryRepo cloudinary, Duration duration)
        {
            _context = context;
            _cloudinary = cloudinary;
            this.duration = duration;
        }
      
        public async Task<CourseTopicsResponseModel> createCourseTopicsAsync(CourseTopicsRequestModel obj)
        {
            try
            {
                //check if a course topics exists
                var checkResult = _context.CourseTopics.Where(x => x.FacilitatorId == obj.FacilitatorId && x.CourseId == obj.CourseId && x.Topic == obj.Topic).FirstOrDefault();

                //if the course Topics doesnt exist, Create the course Topics
                if (checkResult == null)
                {

                    var crsTopics = new CourseTopics
                    {
                        FacilitatorId = obj.FacilitatorId,
                        CourseId = obj.CourseId,
                        Topic = obj.Topic,
                        Duration = obj.Duration,
                        TopicDescription = "",
                        DateCreated = DateTime.Now,
                    };

                    await _context.CourseTopics.AddAsync(crsTopics);
                    await _context.SaveChangesAsync();

                    //get all the Course Topics Created
                    var crsResult = (from cr in _context.CourseTopics
                                     .Include(c => c.CourseTopicMaterials)
                                     .Include(c => c.CourseTopicVideos)
                                     .Include(c=>c.CourseTopicQuiz)
                                     where cr.CourseId == obj.CourseId
                                     select new CourseTopicResponseModel
                                     {
                                         Id = cr.Id,
                                         FacilitatorId = cr.FacilitatorId,
                                         CourseId = cr.CourseId,
                                         CourseName = cr.Courses.CourseName,
                                         Topic = cr.Topic,
                                         DateCreated = cr.DateCreated,
                                         Material = cr.CourseTopicMaterials.Where(x => x.IsApproved == true),
                                         Video = cr.CourseTopicVideos.Where(x => x.IsApproved == true),
                                         CourseTopicQuiz = cr.CourseTopicQuiz
                                     }).OrderByDescending(c => c.Id);

                    return new CourseTopicsResponseModel { StatusCode = 200, StatusMessage = "Course Topics Created Successfully", Data = new CourseTopicData { CourseTopic = crsResult.ToList()} };

                }
                else
                {
                    return new CourseTopicsResponseModel { StatusCode = 200, StatusMessage = "Course Topics Already Exists!" };
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
                return new CourseTopicsResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<CourseTopicsResponseModel> createMultipleCourseTopicsAsync(MultipleCourseTopicsRequestModel obj)
        {
            try
            {
                foreach (var tp in obj.Topics)
                {
                    //check if a course topics exists
                    var checkResult = _context.CourseTopics.Where(x => x.FacilitatorId == obj.FacilitatorId && x.CourseId == obj.CourseId && x.Topic == tp.Topic).FirstOrDefault();

                    //if the course Topics doesnt exist, Create the course Topics
                    if (checkResult == null)
                    {
                        var crsTopics = new CourseTopics
                        {
                            FacilitatorId = obj.FacilitatorId,
                            CourseId = obj.CourseId,
                            Topic = tp.Topic,
                            Duration = tp.Duration,
                            TopicDescription = "",
                            DateCreated = DateTime.Now,
                        };

                        await _context.CourseTopics.AddAsync(crsTopics);
                        await _context.SaveChangesAsync();
                    }
                }
                
                //get all the Course Topics Created
                var crsResult = (from cr in _context.CourseTopics
                                 .Include(c => c.CourseTopicMaterials)
                                 .Include(c => c.CourseTopicVideos)
                                 .Include(c => c.CourseTopicQuiz)
                                 where cr.CourseId == obj.CourseId
                                 select new CourseTopicResponseModel
                                 {
                                     Id = cr.Id,
                                     FacilitatorId = cr.FacilitatorId,
                                     CourseId = cr.CourseId,
                                     CourseName = cr.Courses.CourseName,
                                     Topic = cr.Topic,
                                     DateCreated = cr.DateCreated,
                                     Material = cr.CourseTopicMaterials.Where(x => x.IsApproved == true),
                                     Video = cr.CourseTopicVideos.Where(x => x.IsApproved == true),
                                     CourseTopicQuiz = cr.CourseTopicQuiz
                                 }).OrderByDescending(c => c.Id);

                return new CourseTopicsResponseModel { StatusCode = 200, StatusMessage = "Course Topics Created Successfully", Data = new CourseTopicData { CourseTopic = crsResult.ToList() } };

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
                return new CourseTopicsResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }


        public async Task<GenericResponseModel> getAllCourseTopicsAsync()
        {
            try
            {
                var result = from cr in _context.CourseTopics
                             .Include(c => c.CourseTopicMaterials)
                              .Include(c => c.CourseTopicVideos)
                              .Include(c => c.CourseTopicQuiz)
                             select new CourseTopicResponseModel
                             {
                                 Id = cr.Id,
                                 FacilitatorId = cr.FacilitatorId,
                                 CourseId = cr.CourseId,
                                 CourseName = cr.Courses.CourseName,
                                 Topic = cr.Topic,
                                 DateCreated = cr.DateCreated,
                                 Material = cr.CourseTopicMaterials.Where(x => x.IsApproved == true),
                                 Video = cr.CourseTopicVideos.Where(x => x.IsApproved == true),
                                 CourseTopicQuiz = cr.CourseTopicQuiz
                             };
                IList<CourseTopicAndDurationResponseModel> responseModel = duration.CourseTopicAndDuration(result);

                if (responseModel.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful!", Data = responseModel.ToList() };
                }

                return new GenericResponseModel { StatusCode = 404, StatusMessage = "No Available Record!" };

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

        public async Task<GenericResponseModel> getAllCourseTopicsByCourseIdAsync(long courseId)
        {
            try
            {
                var checkResult = _context.CourseTopics.Where(x => x.CourseId == courseId).FirstOrDefault();
                if (checkResult != null)
                {
                    var result = from cr in _context.CourseTopics
                                 .Include(c => c.CourseTopicMaterials)
                                 .Include(c => c.CourseTopicVideos)
                                 .Include(c => c.CourseTopicQuiz)
                                 where cr.CourseId == courseId
                                 select new CourseTopicResponseModel
                                 {
                                     Id = cr.Id,
                                     FacilitatorId = cr.FacilitatorId,
                                     CourseId = cr.CourseId,
                                     CourseName = cr.Courses.CourseName,
                                     Topic = cr.Topic,
                                     DateCreated = cr.DateCreated,
                                     Material = cr.CourseTopicMaterials.Select(x => new { x.CourseId, x.CourseTopicId, x.DateUploaded, x.Description, x.FacilitatorId, x.FileName, x.FileType, x.FileUrl, x.Id, x.IsApproved, x.IsAvailable, x.IsVerified, x.NoOfPages }),
                                     Video = cr.CourseTopicVideos.Select(x => new { x.CourseId, x.CourseTopicId, x.DateUploaded, x.Description, x.FacilitatorId, x.FileName, x.FileType, x.FileUrl, x.Id, x.IsApproved, x.IsAvailable, x.IsVerified, x.Duration, x.CourseTopicVideoMaterials }),
                                     CourseTopicQuiz = cr.CourseTopicQuiz
                                 };
                    //otional bool(false) is added to calculate the entire video
                    IList<CourseTopicAndDurationResponseModel> responseModel = duration.CourseTopicAndDuration(result, false);

                    if (responseModel.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful!", Data = responseModel.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 401, StatusMessage = "No Available Record!" };
                }

                return new GenericResponseModel { StatusCode = 404, StatusMessage = "No Available Record!" };

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

        public async Task<GenericResponseModel> getAllCourseTopicsByCourseIdWithApprovedDataAsync(long courseId)
        {
            try
            {
                var checkResult = _context.CourseTopics.Where(x => x.CourseId == courseId).FirstOrDefault();
                if (checkResult != null)
                {
                    var result = from cr in _context.CourseTopics
                                 .Include(c => c.CourseTopicMaterials)
                                 .Include(c => c.CourseTopicVideos)
                                 .Include(c => c.CourseTopicQuiz)
                                 where cr.CourseId == courseId
                                 select new CourseTopicResponseModel
                                 {
                                     Id = cr.Id,
                                     FacilitatorId = cr.FacilitatorId,
                                     CourseId = cr.CourseId,
                                     CourseName = cr.Courses.CourseName,
                                     Topic = cr.Topic,
                                     DateCreated = cr.DateCreated,
                                     Material = cr.CourseTopicMaterials.Where(x => x.IsApproved == true).Select(x=> new { x.CourseId, x.CourseTopicId, x.DateUploaded, x.Description, x.FacilitatorId, x.FileName, x.FileType, x.FileUrl, x.Id, x.IsApproved, x.IsAvailable, x.IsVerified, x.NoOfPages }),
                                     Video = cr.CourseTopicVideos.Where(x => x.IsApproved == true).Select(x => new { x.CourseId, x.CourseTopicId, x.DateUploaded, x.Description, x.FacilitatorId, x.FileName, x.FileType, x.FileUrl, x.Id, x.IsApproved, x.IsAvailable, x.IsVerified, x.Duration, x.CourseTopicVideoMaterials }),
                                     CourseTopicQuiz = cr.CourseTopicQuiz
                                 };

                    IList<CourseTopicAndDurationResponseModel> responseModel = duration.CourseTopicAndDuration(result);

                    if (responseModel.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful!", Data = responseModel.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 401, StatusMessage = "No Available Record!" };
                }

                return new GenericResponseModel { StatusCode = 404, StatusMessage = "No Available Record!" };

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

        public async Task<GenericResponseModel> getAllCourseTopicsByFacilitatorIdAsync(Guid facilitatorId)
        {
            try
            {
                //check if the facilitatorId is valid
                var checkResult = new CheckerValidation(_context).checkFacilitatorById(facilitatorId);
                if (checkResult == true)
                {

                    var result = from cr in _context.CourseTopics
                                 .Include(c => c.CourseTopicMaterials)
                                 .Include(c => c.CourseTopicVideos)
                                 .Include(c => c.CourseTopicQuiz)
                                 where cr.FacilitatorId == facilitatorId
                                 select new CourseTopicResponseModel
                                 {
                                     Id = cr.Id,
                                     FacilitatorId = cr.FacilitatorId,
                                     CourseId = cr.CourseId,
                                     CourseName = cr.Courses.CourseName,
                                     Topic = cr.Topic,
                                     DateCreated = cr.DateCreated,
                                     Material = cr.CourseTopicMaterials.Where(x => x.IsApproved == true),
                                     Video = cr.CourseTopicVideos.Where(x => x.IsApproved == true),
                                     CourseTopicQuiz = cr.CourseTopicQuiz
                                 };

                    IList<CourseTopicAndDurationResponseModel> responseModel = duration.CourseTopicAndDuration(result);

                    if (responseModel.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful!", Data = responseModel.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 401, StatusMessage = "No Available Record!" };
                }

                return new GenericResponseModel { StatusCode = 404, StatusMessage = "No facilitator with the specified ID!" };

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

        public async Task<GenericResponseModel> getCourseTopicsByIdAsync(long courseTopicId)
        {
            try
            {
                var checkResult = _context.CourseTopics.Where(x => x.Id == courseTopicId).FirstOrDefault();
                if (checkResult != null)
                {
                    //CourseTopic
                    var coursTopicResult = from cr in _context.CourseTopics
                                           .Include(c=>c.CourseTopicMaterials)
                                           .Include(c => c.CourseTopicVideos)
                                           .Include(c => c.CourseTopicQuiz)
                                           where cr.Id == courseTopicId
                                           select new CourseTopicResponseModel
                                           {
                                               Id = cr.Id,
                                               FacilitatorId = cr.FacilitatorId,
                                               CourseId = cr.CourseId,
                                               CourseName = cr.Courses.CourseName,
                                               Topic = cr.Topic,
                                               DateCreated = cr.DateCreated,
                                               Material = cr.CourseTopicMaterials.Where(x => x.IsApproved == true),
                                               Video = cr.CourseTopicVideos.Where(x => x.IsApproved == true),
                                               CourseTopicQuiz = cr.CourseTopicQuiz
                                           };

                    IList<CourseTopicAndDurationResponseModel> responseModel = duration.CourseTopicAndDuration(coursTopicResult);

                    if (responseModel.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful!", Data = responseModel.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 401, StatusMessage = "No Available Record!" };
                }

                return new GenericResponseModel { StatusCode = 404, StatusMessage = "No Course Topic with the specified ID!" };

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

        //---------------------------------- COURSE TOPICS MATERIALS ---------------------------------------------

        public async Task<GenericResponseModel> approveCourseTopicMaterialAsync(long courseTopicMateriaId)
        {
            try
            {
                //check if the courseTopicMateriaId is valid
                var checkResult = await _context.CourseTopicMaterials.Where(x => x.Id == courseTopicMateriaId).FirstOrDefaultAsync();
                if (checkResult != null)
                {
                    checkResult.IsApproved = true;
                    checkResult.IsVerified = true;
                    checkResult.IsAvailable = true;

                    await _context.SaveChangesAsync();
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Topic Material Approved and Verified Successfully" };
                }

                return new GenericResponseModel { StatusCode = 404, StatusMessage = "No Course Topic Material with the specified ID" };
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

        public async Task<GenericResponseModel> createCourseTopicMaterialsAsync(CourseTopicsMaterialsRequestModel obj)
        {
            try
            {
                var checkResult = _context.CourseTopicMaterials.Where(x => x.Description == obj.Description && x.CourseId == obj.CourseId && x.CourseTopicId == obj.CourseTopicId).FirstOrDefault();
                if (checkResult == null)
                {
                    var crs = new CourseTopicMaterials
                    {
                        FacilitatorId = obj.FacilitatorId,
                        CourseId = obj.CourseId,
                        CourseTopicId = obj.CourseTopicId,
                        Description = obj.Description,
                        FileName = obj.FileName,
                        FileType = obj.FileType,
                        FileUrl = obj.FileUrl,
                        NoOfPages = obj.FileSize,
                        IsApproved = false,
                        IsVerified = false,
                        IsAvailable = false,
                        DateUploaded = DateTime.Now,
                    };

                    await _context.CourseTopicMaterials.AddAsync(crs);
                    await _context.SaveChangesAsync();

                    //get all course topic Materials Created
                    var result = (from cr in _context.CourseTopicMaterials
                                  where cr.CourseTopicId == obj.CourseTopicId
                                  select new
                                  {
                                      cr.Id,
                                      cr.FacilitatorId,
                                      cr.CourseId,
                                      cr.Courses.CourseName,
                                      cr.CourseTopicId,
                                      cr.CourseTopics.Topic,
                                      cr.Description,
                                      cr.FileUrl,
                                      cr.NoOfPages,
                                      cr.IsApproved,
                                      cr.IsVerified,
                                      cr.IsAvailable,
                                      cr.DateUploaded,
                                  }).OrderByDescending(c => c.Id);

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Topic Material Added Successfully!", Data = result.ToList() };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "Course Topic Material With Description Exists!"};
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
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }


        public async Task<GenericResponseModel> createMultipleCourseTopicMaterialsAsync(MultipleCourseTopicsMaterialsRequestModel obj)
        {
            try
            {
                foreach (var mat in obj.Materials)
                {
                    var checkResult = _context.CourseTopicMaterials.Where(x => x.Description == mat.Description && x.CourseId == obj.CourseId && x.CourseTopicId == obj.CourseTopicId).FirstOrDefault();
                    if (checkResult == null)
                    {
                        var crs = new CourseTopicMaterials
                        {
                            FacilitatorId = obj.FacilitatorId,
                            CourseId = obj.CourseId,
                            CourseTopicId = obj.CourseTopicId,
                            Description = mat.Description,
                            FileName = mat.FileName,
                            FileType = mat.FileType,
                            FileUrl = mat.FileUrl,
                            NoOfPages = mat.FileSize,
                            IsApproved = false,
                            IsVerified = false,
                            IsAvailable = false,
                            DateUploaded = DateTime.Now,
                        };

                        await _context.CourseTopicMaterials.AddAsync(crs);
                        await _context.SaveChangesAsync();
                    }
                }

                //get all course topic Materials Created
                var result = (from cr in _context.CourseTopicMaterials
                              where cr.CourseTopicId == obj.CourseTopicId
                              select new
                              {
                                  cr.Id,
                                  cr.FacilitatorId,
                                  cr.CourseId,
                                  cr.Courses.CourseName,
                                  cr.CourseTopicId,
                                  cr.CourseTopics.Topic,
                                  cr.Description,
                                  cr.FileUrl,
                                  cr.NoOfPages,
                                  cr.IsApproved,
                                  cr.IsVerified,
                                  cr.IsAvailable,
                                  cr.DateUploaded,
                              }).OrderByDescending(c => c.Id);

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Topic Material Added Successfully!", Data = result.ToList() };

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


        public async Task<GenericResponseModel> getCourseTopicMaterialsByIdAsync(long courseTopicMaterialId)
        {
            try
            {
                var checkResult = _context.CourseTopicMaterials.Where(x => x.Id == courseTopicMaterialId).FirstOrDefault();
                if (checkResult != null)
                {

                    var result = from cr in _context.CourseTopicMaterials
                                 where cr.Id == courseTopicMaterialId
                                 select new
                                 {
                                     cr.Id,
                                     cr.FacilitatorId,
                                     cr.CourseId,
                                     cr.Courses.CourseName,
                                     cr.CourseTopicId,
                                     cr.CourseTopics.Topic,
                                     cr.Description,
                                     cr.FileUrl,
                                     cr.NoOfPages,
                                     cr.IsApproved,
                                     cr.IsVerified,
                                     cr.IsAvailable,
                                     cr.DateUploaded,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful!", Data = result.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record!" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course Topic Material with the specified ID!" };

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

        public async Task<GenericResponseModel> getAllCourseTopicMaterialsByCourseIdAsync(long courseId)
        {
            try
            {
                //check if the courseID is valid
                var checkResult = new CheckerValidation(_context).checkCourseById(courseId);
                if (checkResult == true)
                {

                    var result = from cr in _context.CourseTopicMaterials
                                 where cr.CourseId == courseId
                                 select new
                                 {
                                     cr.Id,
                                     cr.FacilitatorId,
                                     cr.CourseId,
                                     cr.Courses.CourseName,
                                     cr.CourseTopicId,
                                     cr.CourseTopics.Topic,
                                     cr.Description,
                                     cr.FileUrl,
                                     cr.NoOfPages,
                                     cr.IsApproved,
                                     cr.IsVerified,
                                     cr.IsAvailable,
                                     cr.DateUploaded,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful!", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record!" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course with the specified ID!" };

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

        public async Task<GenericResponseModel> getCourseTopicMaterialsByCourseTopicIdAsync(long courseTopiclId)
        {
            try
            {
                var checkResult = _context.CourseTopicMaterials.Where(x => x.CourseTopicId == courseTopiclId).FirstOrDefault();
                if (checkResult != null)
                {

                    var result = from cr in _context.CourseTopicMaterials
                                 where cr.CourseTopicId == courseTopiclId
                                 select new
                                 {
                                     cr.Id,
                                     cr.FacilitatorId,
                                     cr.CourseId,
                                     cr.Courses.CourseName,
                                     cr.CourseTopicId,
                                     cr.CourseTopics.Topic,
                                     cr.Description,
                                     cr.FileUrl,
                                     cr.NoOfPages,
                                     cr.IsApproved,
                                     cr.IsVerified,
                                     cr.IsAvailable,
                                     cr.DateUploaded,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful!", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record!" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course Topic with the specified ID!" };

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

        //--------------------------------------------COURSE TOPIC VIDEOS----------------------------------------------------------------------

        public async Task<GenericResponseModel> approveCourseTopicVideoAsync(long courseTopicVideoId)
        {
            try
            {
                //check if the courseTopicVideoId is valid
                var checkResult = await _context.CourseTopicVideos.Where(x=>x.Id == courseTopicVideoId).FirstOrDefaultAsync();
                if (checkResult != null)
                {
                    checkResult.IsApproved = true;
                    checkResult.IsVerified = true;
                    checkResult.IsAvailable = true;

                    await _context.SaveChangesAsync();
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Topic Video Approved and Verified Successfully" };
                }

                return new GenericResponseModel { StatusCode = 404, StatusMessage = "No Course Topic Video with the specified ID" };
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

        public async Task<GenericResponseModel> createCourseTopicVideosAsync(CourseTopicsMaterialsRequestModel obj)
        {
            try
            {
                //if (obj.File != null) //If Video file is selected
                //{
                ////Video Upload to Cloudinary Instance
                //var videoUploadResult = await _cloudinary.VideosUpload(obj.File);
                //string fileExt = System.IO.Path.GetExtension(obj.File.FileName); //the file extension to determine the type e.g .pdf, .mp4, .mp3 etc
                //string fileExtension = fileExt.Replace('.',' ').Trim(); //removes "." from the file extension

                //if (obj.File.Length > 52428800) //checks if the file size is less than 50MB
                //{ 
                //    return new GenericResponseModel { StatusCode = 500, StatusMessage = "File Exceeds the minimum size to be uploaded!" };
                //}
                //else
                //{
                var checkResult = _context.CourseTopicVideos.Where(x => x.Description == obj.Description && x.CourseId == obj.CourseId && x.CourseTopicId == obj.CourseTopicId).FirstOrDefault();
                if (checkResult == null)
                {
                    var crs = new CourseTopicVideos
                    {
                        FacilitatorId = obj.FacilitatorId,
                        CourseId = obj.CourseId,
                        CourseTopicId = obj.CourseTopicId,
                        Description = obj.Description,
                        FileName = obj.FileName,
                        FileType = obj.FileType,
                        FileUrl = obj.FileUrl,
                        Duration = obj.Duration.ToString(),
                        IsApproved = false,
                        IsVerified = false,
                        IsAvailable = false,
                        DateUploaded = DateTime.Now,
                    };

                    await _context.CourseTopicVideos.AddAsync(crs);
                    await _context.SaveChangesAsync();

                    //get all course topics video
                    var result = (from cr in _context.CourseTopicVideos
                                  where cr.CourseTopicId == obj.CourseTopicId
                                  select new
                                  {
                                      cr.Id,
                                      cr.FacilitatorId,
                                      cr.CourseId,
                                      cr.Courses.CourseName,
                                      cr.CourseTopicId,
                                      cr.CourseTopics.Topic,
                                      cr.Description,
                                      cr.FileUrl,
                                      cr.Duration,
                                      cr.IsApproved,
                                      cr.IsVerified,
                                      cr.IsAvailable,
                                      cr.DateUploaded,
                                  }).OrderByDescending(c => c.Id);




                    //}

                    //}
                    //else
                    //{
                    //    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No File was selected!" };
                    //}

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Topic Video Added Successfully!", Data = result.ToList() };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Topic Video With Description Already Exists!",};
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
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }


        public async Task<GenericResponseModel> createMultipleCourseTopicVideosAsync(MultipleCourseTopicsMaterialsRequestModel obj)
        {
            try
            {
                foreach (var mat in obj.Materials)
                {
                    var checkResult = _context.CourseTopicVideos.Where(x => x.Description == mat.Description && x.CourseId == obj.CourseId && x.CourseTopicId == obj.CourseTopicId).FirstOrDefault();
                    if (checkResult == null)
                    {
                        var crs = new CourseTopicVideos
                        {
                            FacilitatorId = obj.FacilitatorId,
                            CourseId = obj.CourseId,
                            CourseTopicId = obj.CourseTopicId,
                            Description = mat.Description,
                            FileName = mat.FileName,
                            FileType = mat.FileType,
                            FileUrl = mat.FileUrl,
                            Duration = mat.Duration.ToString(),
                            IsApproved = false,
                            IsVerified = false,
                            IsAvailable = false,
                            DateUploaded = DateTime.Now,
                        };

                        await _context.CourseTopicVideos.AddAsync(crs);
                        await _context.SaveChangesAsync();
                    }
                }

                //get all course topics video
                var result = (from cr in _context.CourseTopicVideos
                              where cr.CourseTopicId == obj.CourseTopicId
                              select new
                              {
                                  cr.Id,
                                  cr.FacilitatorId,
                                  cr.CourseId,
                                  cr.Courses.CourseName,
                                  cr.CourseTopicId,
                                  cr.CourseTopics.Topic,
                                  cr.Description,
                                  cr.FileUrl,
                                  cr.Duration,
                                  cr.IsApproved,
                                  cr.IsVerified,
                                  cr.IsAvailable,
                                  cr.DateUploaded,
                              }).OrderByDescending(c => c.Id);

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Topic Video Added Successfully!", Data = result.ToList() };

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

        public async Task<GenericResponseModel> getCourseTopicVideosByIdAsync(long courseTopicVideoId)
        {
            try
            {
                var checkResult = _context.CourseTopicVideos.Where(x => x.Id == courseTopicVideoId).FirstOrDefault();
                if (checkResult != null)
                {

                    var result = from cr in _context.CourseTopicVideos
                                 where cr.Id == courseTopicVideoId
                                 select new
                                 {
                                     cr.Id,
                                     cr.FacilitatorId,
                                     cr.CourseId,
                                     cr.Courses.CourseName,
                                     cr.CourseTopicId,
                                     cr.CourseTopics.Topic,
                                     cr.Description,
                                     cr.FileUrl,
                                     cr.Duration,
                                     cr.IsApproved,
                                     cr.IsVerified,
                                     cr.IsAvailable,
                                     cr.DateUploaded,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful!", Data = result.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record!" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course Topic Video with the specified ID!" };

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

        public async Task<GenericResponseModel> getCourseTopicVideosByCourseTopicIdAsync(long courseTopicId)
        {
            try
            {
                var checkResult = _context.CourseTopicVideos.Where(x => x.CourseTopicId == courseTopicId).FirstOrDefault();
                if (checkResult != null)
                {

                    var result = from cr in _context.CourseTopicVideos
                                 where cr.CourseTopicId == courseTopicId
                                 select new
                                 {
                                     cr.Id,
                                     cr.FacilitatorId,
                                     cr.CourseId,
                                     cr.Courses.CourseName,
                                     cr.CourseTopicId,
                                     cr.CourseTopics.Topic,
                                     cr.Description,
                                     cr.FileUrl,
                                     cr.Duration,
                                     cr.IsApproved,
                                     cr.IsVerified,
                                     cr.IsAvailable,
                                     cr.DateUploaded,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful!", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record!" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course Topic with the specified ID!" };

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

        public async Task<GenericResponseModel> getAllCourseTopicVideosByCourseIdAsync(long courseId)
        {
            try
            {
                //check if the courseID is valid
                var checkResult = new CheckerValidation(_context).checkCourseById(courseId);
                if (checkResult == true)
                {

                    var result = from cr in _context.CourseTopicVideos
                                 where cr.CourseId == courseId
                                 select new
                                 {
                                     cr.Id,
                                     cr.FacilitatorId,
                                     cr.CourseId,
                                     cr.Courses.CourseName,
                                     cr.CourseTopicId,
                                     cr.CourseTopics.Topic,
                                     cr.Description,
                                     cr.FileUrl,
                                     cr.Duration,
                                     cr.IsApproved,
                                     cr.IsVerified,
                                     cr.IsAvailable,
                                     cr.DateUploaded,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful!", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record!" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course with the specified ID!" };

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


        public async Task<GenericResponseModel> deleteCourseTopicsAsync(long courseTopicId)
        {
            try
            {
                var obj = _context.CourseTopics.Where(x => x.Id == courseTopicId).FirstOrDefault();
                if (obj != null)
                {
                     _context.CourseTopics.Remove(obj);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Deleted Successfully!" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No CourseTopic With the Specified ID!" };
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

        public async Task<GenericResponseModel> deleteCourseTopicMaterialAsync(long courseTopicMaterialId)
        {
            try
            {
                var obj = _context.CourseTopicMaterials.Where(x => x.Id == courseTopicMaterialId).FirstOrDefault();
                if (obj != null)
                {
                    //get the video filename
                    var fileName = obj.FileName;
                    //delete video from Cloudinary passing the fileName as params
                    var deletionResult = await _cloudinary.DocumentDelete(fileName);

                    if (deletionResult.Result == "ok" || deletionResult.Result == "not found")
                    {
                        _context.CourseTopicMaterials.Remove(obj);
                        await _context.SaveChangesAsync();

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Deleted Successfully"};
                    }
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No CourseTopic Material With the Specified ID!" };
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

        public async Task<GenericResponseModel> deleteCourseTopicVideosAsync(long courseTopicVideoId)
        {
            try
            {
                var obj = _context.CourseTopicVideos.Where(x => x.Id == courseTopicVideoId).FirstOrDefault();
                if (obj != null)
                {
                    // Default folder    
                    //string serverRootFolder = @"/root/SoftLearnFilesRepository/CourseDocuments/Videos/";
                    //string serverRootFolder = @"root@167.86.100.22\root\SoftLearnFilesRepository\CourseDocuments\Videos\";
                    //get the video filename
                    var fileName = obj.FileName;
                    var fileType = obj.FileType;

                    var serverRootFolder = Path.Combine("root", "SoftLearnFilesRepository", "CourseDocuments", "Videos");

                    //File.Exists(@"\\102.102.112.250\some_pictures\" + apicturename + ".jpg")
                    if (File.Exists(@"\\167.86.100.22:9090/CourseDocuments/Videos/tDTelvCYaT1QbOo3AIO4PPezdOd.mp4"))
                    {
                        // If file found, delete it    
                        File.Delete(@"\\167.86.100.22:9090/CourseDocuments/Videos/tDTelvCYaT1QbOo3AIO4PPezdOd.mp4");
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Deleted Successfully on linux server" };
                    }
                    //delete video from Cloudinary passing the fileName as params
                    //var deletionResult = await _cloudinary.VideosDelete(fileName);

                    //if (deletionResult.Result == "ok" || deletionResult.Result == "not found")
                    //{
                    //    _context.CourseTopicVideos.Remove(obj);
                    //    await _context.SaveChangesAsync();

                    //    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Deleted Successfully" };
                    //}
                    //if (File.Exists(serverRootFolder + fileName + fileType))
                    //{
                    //    // If file found, delete it    
                    //    File.Delete(serverRootFolder + fileName + fileType);
                    //}
                    //_context.CourseTopicVideos.Remove(obj);
                    //await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Deleted Successfully " + serverRootFolder + "/" + fileName };


                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No CourseTopic Videos With the Specified ID!"};
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

        //-------------------------COURSE TOPICS VIDEO MATERIALS--------------------------------------------
        public async Task<GenericResponseModel> approveCourseTopicVideoMaterialAsync(long courseTopicVideoMateriaId)
        {
            try
            {
                //check if the courseTopicMateriaId is valid
                var checkResult = await _context.CourseTopicVideoMaterials.Where(x => x.Id == courseTopicVideoMateriaId).FirstOrDefaultAsync();
                if (checkResult != null)
                {
                    checkResult.IsApproved = true;
                    checkResult.IsVerified = true;
                    checkResult.IsAvailable = true;

                    await _context.SaveChangesAsync();
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Topic Video Material Approved and Verified Successfully" };
                }

                return new GenericResponseModel { StatusCode = 404, StatusMessage = "No Course Topic Video Material with the specified ID" };
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
        public async Task<GenericResponseModel> createCourseTopicVideoMaterialsAsync(CourseTopicVideoMaterialsRequestModel obj)
        {
            try
            {
                var checkResult = _context.CourseTopicVideoMaterials.Where(x => x.Description == obj.Description && x.CourseId == obj.CourseId && x.CourseTopicId == obj.CourseTopicId && x.CourseTopicVideoId == obj.CourseTopicVideoId).FirstOrDefault();
                if (checkResult == null)
                {
                    var crs = new CourseTopicVideoMaterial
                    {
                        FacilitatorId = obj.FacilitatorId,
                        CourseId = obj.CourseId,
                        CourseTopicId = obj.CourseTopicId,
                        CourseTopicVideoId = obj.CourseTopicVideoId,
                        Description = obj.Description,
                        FileName = obj.FileName,
                        FileType = obj.FileType,
                        FileUrl = obj.FileUrl,
                        NoOfPages = obj.FileSize,
                        IsApproved = false,
                        IsVerified = false,
                        IsAvailable = false,
                        DateUploaded = DateTime.Now,
                    };

                    await _context.CourseTopicVideoMaterials.AddAsync(crs);
                    await _context.SaveChangesAsync();

                    //get all course topic video Materials Created
                    var result = (from cr in _context.CourseTopicVideoMaterials
                                  where cr.CourseTopicVideoId == obj.CourseTopicVideoId
                                  select new
                                  {
                                      cr.Id,
                                      cr.FacilitatorId,
                                      cr.CourseId,
                                      cr.Courses.CourseName,
                                      cr.CourseTopicId,
                                      cr.CourseTopics.Topic,
                                      cr.CourseTopicVideoId,
                                      cr.Description,
                                      cr.FileUrl,
                                      cr.NoOfPages,
                                      cr.IsApproved,
                                      cr.IsVerified,
                                      cr.IsAvailable,
                                      cr.DateUploaded,
                                  }).OrderByDescending(c => c.Id);

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Topic Video Material Added Successfully!", Data = result.ToList() };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "Course Topic Video Material With Description Exists!" };
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
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> createMultipleCourseTopicVideoMaterialsAsync(MultipleCourseTopicVideoMaterialsRequestModel obj)
        {
            try
            {
                foreach (var mat in obj.Materials)
                {
                    var checkResult = _context.CourseTopicVideoMaterials.Where(x => x.Description == mat.Description && x.CourseId == obj.CourseId && x.CourseTopicId == obj.CourseTopicId && x.CourseTopicVideoId == obj.CourseTopicVideoId).FirstOrDefault();
                    if (checkResult == null)
                    {
                        var crs = new CourseTopicVideoMaterial
                        {
                            FacilitatorId = obj.FacilitatorId,
                            CourseId = obj.CourseId,
                            CourseTopicId = obj.CourseTopicId,
                            CourseTopicVideoId = obj.CourseTopicVideoId,
                            Description = mat.Description,
                            FileName = mat.FileName,
                            FileType = mat.FileType,
                            FileUrl = mat.FileUrl,
                            NoOfPages = mat.FileSize,
                            IsApproved = false,
                            IsVerified = false,
                            IsAvailable = false,
                            DateUploaded = DateTime.Now,
                        };

                        await _context.CourseTopicVideoMaterials.AddAsync(crs);
                        await _context.SaveChangesAsync();
                    }
                }

                //get all course topic video Materials Created
                var result = (from cr in _context.CourseTopicVideoMaterials
                              where cr.CourseTopicVideoId == obj.CourseTopicVideoId
                              select new
                              {
                                  cr.Id,
                                  cr.FacilitatorId,
                                  cr.CourseId,
                                  cr.Courses.CourseName,
                                  cr.CourseTopicId,
                                  cr.CourseTopics.Topic,
                                  cr.CourseTopicVideoId,
                                  cr.Description,
                                  cr.FileUrl,
                                  cr.NoOfPages,
                                  cr.IsApproved,
                                  cr.IsVerified,
                                  cr.IsAvailable,
                                  cr.DateUploaded,
                              }).OrderByDescending(c => c.Id);

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Topic Material Added Successfully!", Data = result.ToList() };

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

        public async Task<GenericResponseModel> getCourseTopicVideoMaterialsByIdAsync(long courseTopicVideoMaterialId)
        {
            try
            {
                var checkResult = _context.CourseTopicVideoMaterials.Where(x => x.Id == courseTopicVideoMaterialId).FirstOrDefault();
                if (checkResult != null)
                {

                    var result = from cr in _context.CourseTopicVideoMaterials
                                 where cr.Id == courseTopicVideoMaterialId
                                 select new
                                 {
                                     cr.Id,
                                     cr.FacilitatorId,
                                     cr.CourseId,
                                     cr.Courses.CourseName,
                                     cr.CourseTopicId,
                                     cr.CourseTopics.Topic,
                                     cr.CourseTopicVideoId,
                                     cr.Description,
                                     cr.FileUrl,
                                     cr.NoOfPages,
                                     cr.IsApproved,
                                     cr.IsVerified,
                                     cr.IsAvailable,
                                     cr.DateUploaded,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful!", Data = result.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 401, StatusMessage = "No Available Record!" };
                }

                return new GenericResponseModel { StatusCode = 404, StatusMessage = "No Course Topic Video Material with the specified ID!" };

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

        public async Task<GenericResponseModel> getAllCourseTopicVideoMaterialsByVideoIdAsync(long courseTopicVideoId, bool? isApproved)
        {
            try
            {
                var checkResult = await _context.CourseTopicVideos.Where(x => x.Id == courseTopicVideoId).ToListAsync();
                if (checkResult != null)
                {

                    var result = from cr in _context.CourseTopicVideoMaterials
                                 where cr.CourseTopicVideoId == courseTopicVideoId
                                 select new
                                 {
                                     cr.Id,
                                     cr.FacilitatorId,
                                     cr.CourseId,
                                     cr.Courses.CourseName,
                                     cr.CourseTopicId,
                                     cr.CourseTopics.Topic,
                                     cr.CourseTopicVideoId,
                                     cr.Description,
                                     cr.FileUrl,
                                     cr.NoOfPages,
                                     cr.IsApproved,
                                     cr.IsVerified,
                                     cr.IsAvailable,
                                     cr.DateUploaded,
                                 };
                    if (isApproved != null)
                    {
                        result = result.Where(x => x.IsApproved == isApproved);
                    }

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful!", Data = await result.ToListAsync() };
                    }

                    return new GenericResponseModel { StatusCode = 401, StatusMessage = "No Available Record!" };
                }

                return new GenericResponseModel { StatusCode = 404, StatusMessage = "No Course Topic Video with the specified ID!" };

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

        public async Task<GenericResponseModel> getAllCourseTopicVideoMaterialsByCourseTopicIdAsync(long courseTopicId, bool? isApproved)
        {
            try
            {
                var checkResult = await _context.CourseTopics.Where(x => x.Id == courseTopicId).ToListAsync();
                if (checkResult != null)
                {

                    var result = from cr in _context.CourseTopicVideoMaterials
                                 where cr.CourseTopicId == courseTopicId
                                 select new
                                 {
                                     cr.Id,
                                     cr.FacilitatorId,
                                     cr.CourseId,
                                     cr.Courses.CourseName,
                                     cr.CourseTopicId,
                                     cr.CourseTopics.Topic,
                                     cr.CourseTopicVideoId,
                                     cr.Description,
                                     cr.FileUrl,
                                     cr.NoOfPages,
                                     cr.IsApproved,
                                     cr.IsVerified,
                                     cr.IsAvailable,
                                     cr.DateUploaded,
                                 };
                    if(isApproved != null)
                    {
                        result = result.Where(x => x.IsApproved == isApproved);
                    }

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful!", Data = await result.ToListAsync() };
                    }

                    return new GenericResponseModel { StatusCode = 401, StatusMessage = "No Available Record!" };
                }

                return new GenericResponseModel { StatusCode = 404, StatusMessage = "No Course Topic Video with the specified ID!" };

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

        public async Task<GenericResponseModel> deleteCourseTopicVideoMaterialsAsync(long courseTopicVideoMaterialId)
        {
            try
            {
                var obj = _context.CourseTopicVideoMaterials.Where(x => x.Id == courseTopicVideoMaterialId).FirstOrDefault();
                if (obj != null)
                {
                    //get the material filename
                    var fileName = obj.FileName;
                    //delete file from Cloudinary passing the fileName as params
                    var deletionResult = await _cloudinary.DocumentDelete(fileName);

                    if (deletionResult.Result == "ok" || deletionResult.Result == "not found")
                    {
                        _context.CourseTopicVideoMaterials.Remove(obj);
                        await _context.SaveChangesAsync();

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Deleted Successfully" };
                    }
                }

                return new GenericResponseModel { StatusCode = 404, StatusMessage = "No CourseTopic Material With the Specified ID!" };
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

        //------------------------------- Course Topic Completed Video-----------------------------------------------------------
        public async Task<GenericResponseModel> createCourseTopicCompletedVideoAsync(CourseEnrolleeCompletedVideoRequestModel obj)
        {
            try
            {
                var checkLearner = new CheckerValidation(_context).checkLearnerById(obj.LearnerId);
                if(checkLearner == false)
                {
                    return new GenericResponseModel { StatusCode = 400, StatusMessage = "Learner doesn't exist" };
                }
                var course = await _context.Courses.Where(x => x.Id == obj.CourseId).FirstOrDefaultAsync();
                if(course == null)
                {
                    return new GenericResponseModel { StatusCode = 401, StatusMessage = "Course doesn't exist" };
                }
                var courseTopic = await _context.CourseTopics.Where(x => x.Id == obj.CourseTopicId && x.CourseId == obj.CourseId).FirstOrDefaultAsync();
                if (courseTopic == null)
                {
                    return new GenericResponseModel { StatusCode = 402, StatusMessage = "Course Topic doesn't exist or not registered under the course selected" };
                }
                var courseEnrollee = await _context.CourseEnrollees.Where(x => x.Id == obj.CourseEnrolleeId && x.LearnerId == obj.LearnerId).FirstOrDefaultAsync();
                if (courseEnrollee == null)
                {
                    return new GenericResponseModel { StatusCode = 403, StatusMessage = "Course Enrolle doesn't exist or invalid learner attached" };
                }
                var courseTopicVideo = await _context.CourseTopicVideos.Where(x => x.Id == obj.CourseTopicVideoId && x.CourseTopicId == obj.CourseTopicId).FirstOrDefaultAsync();
                if (courseTopicVideo == null)
                {
                    return new GenericResponseModel { StatusCode = 404, StatusMessage = "Course Topic Video doesn't exist or not registered under the course topic selected" };
                }
                var completedVideo = await _context.CourseEnrolleeCompletedVideos.Where(x => x.CourseEnrolleeId == obj.CourseEnrolleeId && x.CourseId == obj.CourseId && x.CourseTopicId == obj.CourseTopicId && x.CourseTopicVideoId == obj.CourseTopicVideoId && x.LearnerId == obj.LearnerId).FirstOrDefaultAsync();
                if(completedVideo == null)
                {
                    CourseEnrolleeCompletedVideo enrolleeCompletedVideo = new CourseEnrolleeCompletedVideo
                    {
                        CourseEnrolleeId = obj.CourseEnrolleeId,
                        CourseId = obj.CourseId,
                        CourseTopicId = obj.CourseTopicId,
                        CourseTopicVideoId = obj.CourseTopicVideoId,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        LearnerId = obj.LearnerId
                    };
                    _context.CourseEnrolleeCompletedVideos.Add(enrolleeCompletedVideo);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    completedVideo.UpdatedAt = DateTime.Now;
                    await _context.SaveChangesAsync();
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Completed video saved successfully" };
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
        public async Task<GenericResponseModel> getCourseTopicCompletedVideoByCourseIdAsync(long courseId, long courseEnrolleeId, Guid learnerId)
        {
            try
            {
                var checkLearner = new CheckerValidation(_context).checkLearnerById(learnerId);
                if (checkLearner == false)
                {
                    return new GenericResponseModel { StatusCode = 400, StatusMessage = "Learner doesn't exist" };
                }
                var course = await _context.Courses.Where(x => x.Id == courseId).FirstOrDefaultAsync();
                if (course == null)
                {
                    return new GenericResponseModel { StatusCode = 401, StatusMessage = "Course doesn't exist" };
                }
                var courseEnrollee = await _context.CourseEnrollees.Where(x => x.Id == courseEnrolleeId).FirstOrDefaultAsync();
                if (courseEnrollee == null)
                {
                    return new GenericResponseModel { StatusCode = 403, StatusMessage = "Course Enrollee doesn't exist" };
                }
                var result = from cr in _context.CourseEnrolleeCompletedVideos
                             where cr.CourseId == courseId && cr.CourseEnrolleeId == courseEnrolleeId && cr.LearnerId == learnerId
                             select new
                             {
                                 cr.Id,
                                 cr.LearnerId,
                                 cr.CourseId,
                                 cr.Courses.CourseName,
                                 cr.CourseTopicId,
                                 cr.CourseTopics.Topic,
                                 cr.CourseEnrolleeId,
                                 cr.CourseTopicVideoId,
                                 cr.CourseTopicVideos.Description,
                                 cr.CreatedAt,
                                 cr.UpdatedAt
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful!", Data = result.ToList() };
                }
                return new GenericResponseModel { StatusCode = 404, StatusMessage = "No record found!"};
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
        public async Task<GenericResponseModel> getCourseTopicCompletedVideoByCourseTopicIdAsync(long courseTopicId, long courseEnrolleeId, Guid learnerId)
        {
            try
            {
                var checkLearner = new CheckerValidation(_context).checkLearnerById(learnerId);
                if (checkLearner == false)
                {
                    return new GenericResponseModel { StatusCode = 400, StatusMessage = "Learner doesn't exist" };
                }
                var courseTopic = await _context.CourseTopics.Where(x => x.Id == courseTopicId).FirstOrDefaultAsync();
                if (courseTopic == null)
                {
                    return new GenericResponseModel { StatusCode = 401, StatusMessage = "Course topic doesn't exist" };
                }
                var courseEnrollee = await _context.CourseEnrollees.Where(x => x.Id == courseEnrolleeId).FirstOrDefaultAsync();
                if (courseEnrollee == null)
                {
                    return new GenericResponseModel { StatusCode = 403, StatusMessage = "Course Enrollee doesn't exist" };
                }
                var result = from cr in _context.CourseEnrolleeCompletedVideos
                             where cr.CourseTopicId == courseTopicId && cr.CourseEnrolleeId == courseEnrolleeId && cr.LearnerId == learnerId
                             select new
                             {
                                 cr.Id,
                                 cr.LearnerId,
                                 cr.CourseId,
                                 cr.Courses.CourseName,
                                 cr.CourseTopicId,
                                 cr.CourseTopics.Topic,
                                 cr.CourseEnrolleeId,
                                 cr.CourseTopicVideoId,
                                 cr.CourseTopicVideos.Description,
                                 cr.CreatedAt,
                                 cr.UpdatedAt
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful!", Data = result.ToList() };
                }
                return new GenericResponseModel { StatusCode = 404, StatusMessage = "No record found!" };
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
        public async Task<GenericResponseModel> getCourseTopicCompletedVideoByVideoIdAsync(long videoId, long courseEnrolleeId, Guid learnerId)
        {
            try
            {
                var checkLearner = new CheckerValidation(_context).checkLearnerById(learnerId);
                if (checkLearner == false)
                {
                    return new GenericResponseModel { StatusCode = 400, StatusMessage = "Learner doesn't exist" };
                }
                var courseTopic = await _context.CourseTopicVideos.Where(x => x.Id == videoId).FirstOrDefaultAsync();
                if (courseTopic == null)
                {
                    return new GenericResponseModel { StatusCode = 401, StatusMessage = "Course topic video doesn't exist" };
                }
                var courseEnrollee = await _context.CourseEnrollees.Where(x => x.Id == courseEnrolleeId).FirstOrDefaultAsync();
                if (courseEnrollee == null)
                {
                    return new GenericResponseModel { StatusCode = 403, StatusMessage = "Course Enrollee doesn't exist" };
                }
                var result = from cr in _context.CourseEnrolleeCompletedVideos
                             where cr.CourseTopicVideoId == videoId && cr.CourseEnrolleeId == courseEnrolleeId && cr.LearnerId == learnerId
                             select new
                             {
                                 cr.Id,
                                 cr.LearnerId,
                                 cr.CourseId,
                                 cr.Courses.CourseName,
                                 cr.CourseTopicId,
                                 cr.CourseTopics.Topic,
                                 cr.CourseEnrolleeId,
                                 cr.CourseTopicVideoId,
                                 cr.CourseTopicVideos.Description,
                                 cr.CreatedAt,
                                 cr.UpdatedAt
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful!", Data = result.FirstOrDefault() };
                }
                return new GenericResponseModel { StatusCode = 404, StatusMessage = "No record found!" };
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
    }
}
