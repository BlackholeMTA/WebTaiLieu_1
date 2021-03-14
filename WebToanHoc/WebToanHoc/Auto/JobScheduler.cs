using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;
using System.Threading.Tasks;

namespace WebToanHoc.Auto
{
    public class JobScheduler
    {
        public static void Start()
        {
            //IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            IScheduler scheduler = schedFact.GetScheduler().GetAwaiter().GetResult();
            scheduler.Start();
            IJobDetail job = JobBuilder.Create<PostJob>().Build();
            ITrigger trigger = TriggerBuilder.Create().WithDailyTimeIntervalSchedule(
                s =>
                s.WithIntervalInHours(24)
                //s.WithIntervalInSeconds(20)
                .OnEveryDay()
                .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0))
                
                )
                .Build();
            scheduler.ScheduleJob(job, trigger);
        }
    }
}