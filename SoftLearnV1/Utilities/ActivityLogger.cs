using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Utilities
{
    public class ActivityLogger
    {

        public ActivityLog logActivity(Guid userId, string platform, string remark)
        {
            var activityLog = new ActivityLog
            {
                UserId = userId,
                Platform = platform,
                Remark = remark,
                ActivityDate = DateTime.Now
            };

            return activityLog;
        }
    }
}
