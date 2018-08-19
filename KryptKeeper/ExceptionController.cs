using System;

namespace KryptKeeper
{
    internal class ExceptionController
    {
        private readonly Status _status;
        public ExceptionController(Status status)
        {
            _status = status;
        }
        public void Handle(Exception ex, string path, string workingPath)
        {
            _status.WriteLine("*** UNHANDLED EXCEPTION: " + ex.Message);
            _status.WriteLine("* STACKTRACE: " + ex.StackTrace);
            _status.WriteLine("Preserving " + workingPath + " for debugging purposes."); // Keep temp file
        }
    }
}