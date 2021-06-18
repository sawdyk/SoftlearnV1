using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SoftLearnV1.DataSeed;
using SoftLearnV1.Helpers;
using SoftLearnV1.InterfaceRepositories;
using SoftLearnV1.Repositories;
using SoftLearnV1.SchoolReusables;
using SoftLearnV1.Services.Cloudinary;
using SoftLearnV1.Services.Email;
using SoftLearnV1.Utilities;

namespace SoftLearnV1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                //options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

            });

            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseMySql(Configuration.GetConnectionString("DefaultConnection"));
                //opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            // services.AddHttpsRedirection(options =>
            // {
            //     options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
            //     options.HttpsPort = 5001;
            // });

            services.AddHsts(options =>
            {
                options.Preload = false;
                options.IncludeSubDomains = false;
                options.MaxAge = TimeSpan.FromDays(30);
            });

            //------------------EMAIL AND CLOUDINARY SERVICE--------------------------------------------------

            var emailConfig = Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);

            var cloudinaryConfig = Configuration.GetSection("Cloudinary").Get<CloudinaryConfig>();
            services.AddSingleton(cloudinaryConfig);

            services.AddTransient<IEmailRepo, EmailRepo>();
            services.AddTransient<ICloudinaryRepo, CloudinaryRepo>();

            //------------------------------SERVICES---------------------------------------------------------

            services.AddTransient<IFacilitatorRepo, FacilitatorRepo>();
            services.AddTransient<ILearnerRepo, LearnerRepo>();
            services.AddTransient<ICourseCategoryRepo, CourseCategoryRepo>();
            services.AddTransient<ICourseSubCategoryRepo, CourseSubCategoryRepo>();
            services.AddTransient<ICourseLevelRepo, CourseLevelRepo>();
            services.AddTransient<ICourseTypeRepo, CourseTypeRepo>();
            services.AddTransient<ISchoolTypeRepo, SchoolTypeRepo>();
            services.AddTransient<ISchoolRepo, SchoolRepo>();
            services.AddTransient<ICourseRepo, CourseRepo>();
            services.AddTransient<IClassRepo, ClassRepo>();
            services.AddTransient<ITeacherRepo, TeacherRepo>();
            services.AddTransient<ICourseTopicsRepo, CourseTopicsRepo>();
            services.AddTransient<ICourseObjectivesRepo, CourseObjectivesRepo>();
            services.AddTransient<ICourseRequirementRepo, CourseRequirementRepo>();
            services.AddTransient<ISubjectRepo, SubjectRepo>();
            services.AddTransient<IStudentRepo, StudentRepo>();
            services.AddTransient<IParentRepo, ParentRepo>();
            services.AddTransient<IReportsRepo, ReportsRepo>();
            services.AddTransient<ICartRepo, CartRepo>();
            services.AddTransient<ICouponCodeRepo, CouponCodeRepo>();
            services.AddTransient<ICourseRatingRepo, CourseRatingRepo>();
            services.AddTransient<ICourseReviewsRepo, CourseReviewsRepo>();
            services.AddTransient<ICourseTopicQuizRepo, CourseTopicQuizRepo>();
            services.AddTransient<ICourseQuizRepo, CourseQuizRepo>();
            services.AddTransient<ISessionTermRepo, SessionTermRepo>();
            services.AddTransient<IScoresConfigRepo, ScoresConfigRepo>();
            services.AddTransient<ISystemUserRepo, SystemUserRepo>();
            services.AddTransient<ISystemDefaultRepo, SystemDefaultRepo>();
            services.AddTransient<IAssignmentRepo, AssignmentRepo>();
            services.AddTransient<ILessonNoteRepo, LessonNoteRepo>();
            services.AddTransient<IFeeTemplateRepo, FeeTemplateRepo>();
            services.AddTransient<IFeeCategoryRepo, FeeCategoryRepo>();
            services.AddTransient<ISchoolFeeRepo, SchoolFeeRepo>();
            services.AddTransient<IScoreUploadRepo,ScoreUploadRepo>();
            services.AddTransient<IExtraCurricularBehavioralScoresRepo, ExtraCurricularBehavioralScoreRepo>();
            services.AddTransient<IReportCardConfigurationRepo, ReportCardConfigurationRepo>();
            services.AddTransient<IComputerBasedTestRepo, ComputerBasedTestRepo>();
            services.AddTransient<ILhsReportCardRepo, LhsReportCardRepo>();
            services.AddTransient<IFileUploadRepo, FileUploadRepo>();
            services.AddTransient<IFinanceReportRepo, FinanceReportRepo>();
            services.AddTransient<IParentReportRepo, ParentReportRepo>();
            services.AddTransient<IPaymentDisbursementRepo, PaymentDisbursementRepo>();
            services.AddTransient<IReportCardRepo, ReportCardRepo>();
            services.AddTransient<IReportCardDataGenerateRepo, ReportCardGenerateRepo>();
            services.AddTransient<ISchoolAdminReportRepo, SchoolAdminReportRepo>();
            services.AddTransient<IClassTeacherReportRepo, ClassTeacherReportRepo>();
            services.AddTransient<IBankRepo, BankRepo>();
            services.AddTransient<IReportDashboardRepo, ReportDashboardRepo>();

            //----------------------------------------------------------------------------------

            services.AddTransient<InvoiceNumberGenerator>();
            services.AddTransient<ReportCardReUsables>();

            services.AddTransient<ServerPath>();
            services.AddTransient<CourseRating>();
            services.AddTransient<Duration>();
            services.AddTransient<EmailTemplate>();
            //---------------=-----------------SWAGGER SERVICES FOR DOCUMENTATION---------------------------------------------------

            //Get the swagger value options
            var swaggerOpt = Configuration.GetSection("SwaggerOptions").Get<SwaggerOptions>();
            // Register Swagger  
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = swaggerOpt.Title,
                    Version = swaggerOpt.Version
                });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name="JWT Authentication",
                    Description = "Enter JWT Bearer Token Only",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[]{ }}
                });
            });

            //-------------------------------JWT AUTHENTICATION AND AUTHORISATION--------------------------------------------------

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                  options.RequireHttpsMetadata = false;
                  options.SaveToken = true;
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidIssuer = Configuration["Jwt:Issuer"],
                      ValidAudience = Configuration["Jwt:Audience"],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"])),
                      ClockSkew = TimeSpan.Zero
                  };
              });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, AppDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // Enable middleware to serve generated Swagger as a JSON endpoint.  
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),  
            // specifying the Swagger JSON endpoint.  
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SOFTLEARN VERSION 1");
                
            });

            //app.UseFileServer(new FileServerOptions
            //{
            //    FileProvider = new PhysicalFileProvider(@"C:\ASPNETApplications"),
            //    RequestPath = new PathString("/uploads"),
            //    EnableDirectoryBrowsing = true
            //});

            //app.UseFileServer(new FileServerOptions
            //{
            //    FileProvider = new PhysicalFileProvider(@"\\http://173.212.213.205\SoftlearnFiles"),
            //    RequestPath = new PathString("/SoftlearnFiles/Assignments/"),
            //    EnableDirectoryBrowsing = true
            //});

            //-----------------HTTP--------------------------------

            app.Use(async (ctx, next) =>
            {
                await next();
                if (ctx.Response.StatusCode == 204)
                {
                    ctx.Response.ContentLength = 0;
                }
            });

            //---------------- CORS --------------------------------

            app.UseCors(builder => builder
             .AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader());
           
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
