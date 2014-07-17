using System;
using System.Linq;
using System.Activities;
using System.Activities.DurableInstancing;
using System.Activities.Statements;
using System.Threading;
using System.Runtime.DurableInstancing;


namespace UseBookmark
{

    class Program
    {
        static void Main(string[] args)
        {
            AutoResetEvent syncEvent =
                new AutoResetEvent(false);
            string bookmarkName = "HostGreeting";
            WorkflowApplication wfApp =
                new WorkflowApplication(new Workflow1()
                {
                    BookmarkNameInArg = bookmarkName
                });
            /*
             * To clean up the persistence database to have a fresh database, run the scripts in:
             * %WINDIR%\Microsoft.NET\Framework\v4.xxx\SQL\EN in the following order.
                    SqlWorkflowInstanceStoreSchema.sql
                    SqlWorkflowInstanceStoreLogic.sql
             * */
            string connectionString = "Server=.\\SQLSERVER14;Initial Catalog=Persistence45;Integrated Security=SSPI";
            Console.WriteLine("Workflow ID: " + wfApp.Id);
            wfApp.InstanceStore = (InstanceStore)new SqlWorkflowInstanceStore(connectionString);
            wfApp.PersistableIdle = delegate(WorkflowApplicationIdleEventArgs e)
            {
                syncEvent.Set();
                Console.WriteLine("Persisting..");
                return PersistableIdleAction.Persist;
            };
            wfApp.Completed = delegate(
                WorkflowApplicationCompletedEventArgs e)
            {
               syncEvent.Set();
               Console.WriteLine("Completing..");
            };
            wfApp.Run();
            wfApp.ResumeBookmark(bookmarkName,
                Console.ReadLine());

            syncEvent.WaitOne();
            syncEvent.WaitOne();

        }
    }
}
