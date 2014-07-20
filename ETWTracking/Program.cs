using System;
using System.Linq;
using System.Activities;
using System.Activities.Statements;
using System.Activities.Tracking;
using System.Threading;

namespace ETWTracking
{

    class Program
    {
        static void Main(string[] args)
        {
            
            #region ETW tracking setup
            TrackingProfile trackingProfile = new TrackingProfile();
            trackingProfile.Queries.Add(new WorkflowInstanceQuery
            {
                States = { "*" }
            });
            trackingProfile.Queries.Add(new ActivityStateQuery
            {
                States = { "*" }
            });
            trackingProfile.Queries.Add(new CustomTrackingQuery
            {
                ActivityName = "*",
                Name = "*"
            });
            EtwTrackingParticipant etwTrackingParticipant =
                new EtwTrackingParticipant();
            etwTrackingParticipant.TrackingProfile = trackingProfile;
            #endregion

            #region Workflow Application
            AutoResetEvent waitHandler = new AutoResetEvent(false);
            WorkflowApplication wfApp =
                new WorkflowApplication(new Workflow1());
            wfApp.Completed = (arg) => { waitHandler.Set(); };
            wfApp.Extensions.Add(etwTrackingParticipant);
            wfApp.Run();
            waitHandler.WaitOne();
            #endregion
        }
    }
}
