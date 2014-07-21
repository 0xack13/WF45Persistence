using System;
using System.Linq;
using System.Activities;
using System.Activities.Statements;
using System.Activities.Tracking;
using System.Threading;
using CreatingFileTrackingParticipant;

namespace FileTrackingParticipant
{
    class Program
    {
        static void Main(string[] args)
        {
            TrackingProfile fileTrackingProfile = new TrackingProfile();
            fileTrackingProfile.Queries.Add(new WorkflowInstanceQuery
            {
                States = { "*" }
            });
            fileTrackingProfile.Queries.Add(new ActivityStateQuery()
            {
                States = { 
                    ActivityStates.Executing, 
                    ActivityStates.Closed
                }
            });
            FileTrackingParticipant fileTrackingParticipant = new FileTrackingParticipant();
            fileTrackingParticipant.TrackingProfile = fileTrackingProfile;
            AutoResetEvent waitHandler = new AutoResetEvent(false);
            WorkflowApplication wfapp = new WorkflowApplication(new Workflow1());
            wfapp.Unloaded = (wfAppEventArg) => { waitHandler.Set(); };
            wfapp.Extensions.Add(fileTrackingParticipant);
            wfapp.Run();
            waitHandler.WaitOne();
        }
    }
}