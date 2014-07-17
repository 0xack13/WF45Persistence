using System.Activities;

namespace UseBookmark{
    public class MyReadLine : NativeActivity<string>{

        [RequiredArgument]
        public InArgument<string> BookmarkName { get; set; }
        public OutArgument<string> RequestStatusArg { get; set; }

        protected override void Execute(
            NativeActivityContext context)
        {
            context.CreateBookmark(BookmarkName.Get(context),
                     new BookmarkCallback(OnResumeBookmark));
        }
        protected override bool CanInduceIdle
        {
            get
            {
                { return true;}
            }
        }
        public void OnResumeBookmark(
            NativeActivityContext context,
            Bookmark bookmark,
            object obj)
        {
            Result.Set(context, (string)obj);
        }
    }
}