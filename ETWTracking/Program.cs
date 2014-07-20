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

        /*
         * Enable Analytic and Debug Logs:
         * Event Viewer -> Applications and Services Logs -> 
         * Microsoft -> Windows -> Application Server-Applications
         * Right-click "Application Server-Applications" -> select View ->
         * Show Analytic and Debug Logs -> Refresh 
         * */
        static void Main(string[] args)
        {
            //Tracking Configuration
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
 
            // Run workflow app "Workflow1.xaml"
            AutoResetEvent waitHandler = new AutoResetEvent(false);
            WorkflowApplication wfApp =
                new WorkflowApplication(new Workflow1());
            wfApp.Completed = (arg) => { waitHandler.Set(); };
            wfApp.Extensions.Add(etwTrackingParticipant);
            wfApp.Run();
            waitHandler.WaitOne();
        }
    }
}
