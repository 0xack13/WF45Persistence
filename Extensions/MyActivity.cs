using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Diagnostics;

namespace Extensions
{

    public sealed class MyActivity : CodeActivity
    {
        // Define an activity input argument of type string
        public InArgument<string> Text { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            ExecutionCountExtension ext = context.GetExtension<ExecutionCountExtension>();
            if (ext != null)
            {
                Debug.WriteLine(context.ActivityInstanceId + ": Activity Executed!");
                ext.Register(context.WorkflowInstanceId);
            }
        }

    }
}
