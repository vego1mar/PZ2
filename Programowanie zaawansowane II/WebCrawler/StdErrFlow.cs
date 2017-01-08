using System;
using System.IO;

namespace WebCrawler
    {

    /// <summary>
    /// Aggregates procedures for dealing with the standard error stream manipulations.
    /// </summary>

    internal class StdErrFlow
        {

        internal const string STDERR_FILENAME = "errlog.txt";

        private static TextWriter oldStdErrStream = null;
        private static TextWriter newStdErrStream = null;

        private static ExceptionInfo lastExceptionInfo;
        private static bool isLastRedirectionSuccessfull = false;

        //______________________________________________________________________________________________________________________________

        public struct ExceptionInfo {
            public string typeName;
            public string methodName;
            public string argument;
            public string causeEvent;
            public string message;
            public string id;
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// The default constructor. There is no point to instantiate this class.
        /// </summary>

        private StdErrFlow()
            {
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// It tries to redirect standard error stream into a file whose name is defined in a constant STDERR_FILENAME.
        /// The file stream is opening for appending.
        /// </summary>
        /// <returns>'true' if function code has been traversed, 'false' when any error occured.</returns>

        public static bool tryToRedirectStdErr()
            {
            try {
                oldStdErrStream = Console.Error;
                newStdErrStream = new StreamWriter( STDERR_FILENAME, true );
                Console.SetError( newStdErrStream );

                if ( newStdErrStream == null ) {
                    return ( false );
                    }

                isLastRedirectionSuccessfull = true;

                string appName = FileSystemFlow.gainApplicationLocation();
                appName = appName.Substring( appName.LastIndexOf('\\') + 1 );

                writeLine( Environment.NewLine );
                writeLine( "=============================================" ); 
                writeLine( "Error log for: " + appName.ToString() );
                writeLine( "Timestamp: " + DateTime.Now.ToString() );
                writeLine( "=============================================" );
                }
            catch ( UnauthorizedAccessException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = newStdErrStream.ToString();
                lastExceptionInfo.causeEvent = "Trying to redirect the StdErr.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-2]";
                writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                writeLine( Environment.NewLine );
                isLastRedirectionSuccessfull = false;
                return ( false );
                }
            catch ( NotSupportedException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = newStdErrStream.ToString();
                lastExceptionInfo.causeEvent = "Trying to redirect the StdErr.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-2]";
                writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                writeLine( Environment.NewLine );
                isLastRedirectionSuccessfull = false;
                return ( false );
                }
            catch ( ArgumentNullException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = newStdErrStream.ToString();
                lastExceptionInfo.causeEvent = "Trying to redirect the StdErr.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-2]";
                writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                writeLine( Environment.NewLine );
                isLastRedirectionSuccessfull = false;
                return ( false );
                }
            catch ( ArgumentOutOfRangeException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = newStdErrStream.ToString();
                lastExceptionInfo.causeEvent = "Trying to redirect the StdErr.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-2]";
                writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                writeLine( Environment.NewLine );
                isLastRedirectionSuccessfull = false;
                return ( false );
                }
            catch ( ArgumentException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = newStdErrStream.ToString();
                lastExceptionInfo.causeEvent = "Trying to redirect the StdErr.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-2]";
                writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                writeLine( Environment.NewLine );
                isLastRedirectionSuccessfull = false;
                return ( false );
                }
            catch ( DirectoryNotFoundException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = newStdErrStream.ToString();
                lastExceptionInfo.causeEvent = "Trying to redirect the StdErr.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-2]";
                writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                writeLine( Environment.NewLine );
                isLastRedirectionSuccessfull = false;
                return ( false );
                }
            catch ( PathTooLongException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = newStdErrStream.ToString();
                lastExceptionInfo.causeEvent = "Trying to redirect the StdErr.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-2]";
                writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                writeLine( Environment.NewLine );
                isLastRedirectionSuccessfull = false;
                return ( false );
                }
            catch ( IOException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = newStdErrStream.ToString();
                lastExceptionInfo.causeEvent = "Trying to redirect the StdErr.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-2]";
                writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                writeLine( Environment.NewLine );
                isLastRedirectionSuccessfull = false;
                return ( false );
                }
            catch ( System.Security.SecurityException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = newStdErrStream.ToString();
                lastExceptionInfo.causeEvent = "Trying to redirect the StdErr.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-2]";
                writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                writeLine( Environment.NewLine );
                isLastRedirectionSuccessfull = false;
                return ( false );
                }
            catch ( Exception x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = newStdErrStream.ToString();
                lastExceptionInfo.causeEvent = "Trying to redirect the StdErr.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-2]";
                writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                writeLine( Environment.NewLine );
                isLastRedirectionSuccessfull = false;
                return ( false );
                }

            return ( true );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Write the given line to the standard error stream and flushes it.
        /// </summary>
        /// <param name="text">A text to write with an ending terminator line into the StdErr.</param>
        /// <returns>'true' if no exception has been raised, 'false' otherwise.</returns>

        public static bool writeLine( string text )
            {
            try {
                Console.Error.WriteLine( text );
                Console.Error.Flush();
                }
            catch ( ObjectDisposedException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = text.ToString();
                lastExceptionInfo.causeEvent = "Writing a line to the StdErr.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-1]";
                return ( false );
                }
            catch ( InvalidOperationException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = text.ToString();
                lastExceptionInfo.causeEvent = "Writing a line to the StdErr.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-1]";
                return ( false );
                }
            catch ( IOException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = text.ToString();
                lastExceptionInfo.causeEvent = "Writing a line to the StdErr.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-1]";
                return ( false );
                }
            catch ( Exception x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = text.ToString();
                lastExceptionInfo.causeEvent = "Writing a line to the StdErr.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-1]";
                return ( false );
                }

            return ( true );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Tries to retrieve the old standard error stream context.
        /// </summary>
        /// <returns>'true' if operation complete with success, 'false' otherwise.</returns>

        public static bool tryToRetrievePreviousStdErr()
            {
            try {
                if ( oldStdErrStream == null ) {
                    throw ( new ArgumentException( "The previous StdErr stream is a null." ) );
                    }

                TextWriter tmpStream = newStdErrStream;
                Console.SetError( oldStdErrStream );
                isLastRedirectionSuccessfull = true;
                newStdErrStream = oldStdErrStream;
                oldStdErrStream = tmpStream;
                }
            catch ( ObjectDisposedException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = oldStdErrStream.ToString();
                lastExceptionInfo.causeEvent = "Trying to retrieve the previous StdErr.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-3]";
                writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                writeLine( Environment.NewLine );
                isLastRedirectionSuccessfull = false;
                return ( false );
                }
            catch ( ArgumentNullException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = oldStdErrStream.ToString();
                lastExceptionInfo.causeEvent = "Trying to retrieve the previous StdErr.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-3]";
                writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                writeLine( Environment.NewLine );
                isLastRedirectionSuccessfull = false;
                return ( false );
                }
            catch ( ArgumentException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = oldStdErrStream.ToString();
                lastExceptionInfo.causeEvent = "Trying to retrieve the previous StdErr.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-3]";
                writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                writeLine( Environment.NewLine );
                isLastRedirectionSuccessfull = false;
                return ( false );
                }
            catch ( System.Security.SecurityException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = oldStdErrStream.ToString();
                lastExceptionInfo.causeEvent = "Trying to retrieve the previous StdErr.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-3]";
                writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                writeLine( Environment.NewLine );
                isLastRedirectionSuccessfull = false;
                return ( false );
                }
            catch ( IOException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = oldStdErrStream.ToString();
                lastExceptionInfo.causeEvent = "Trying to retrieve the previous StdErr.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-3]";
                writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                writeLine( Environment.NewLine );
                isLastRedirectionSuccessfull = false;
                return ( false );
                }
            catch ( Exception x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = oldStdErrStream.ToString();
                lastExceptionInfo.causeEvent = "Trying to retrieve the previous StdErr.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-3]";
                writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                writeLine( Environment.NewLine );
                isLastRedirectionSuccessfull = false;
                return ( false );
                }

            return ( true );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Disposes the streams of this flow control class.
        /// </summary>

        public static void disposeStreams()
            {
            if ( oldStdErrStream != null ) {
                oldStdErrStream.Dispose();
                }

            if ( newStdErrStream != null ) {
                newStdErrStream.Dispose();
                }
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Closes the streams of this flow control class.
        /// </summary>

        public static void closeStreams()
            {
            if ( oldStdErrStream != null ) {
                oldStdErrStream.Close();
                }

            if ( newStdErrStream != null ) {
                newStdErrStream.Close();
                }
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Getter.
        /// </summary>
        /// <returns>An internal flag of last StdErr redirection completion correctness state.</returns>

        public static bool getLastRedirectionState()
            {
            return ( isLastRedirectionSuccessfull );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Getter.
        /// </summary>
        /// <returns>A 'struct' with the data about the last exception occurence.</returns>

        public static ExceptionInfo getLastExceptionInfo()
            {
            return ( lastExceptionInfo );
            }

        //______________________________________________________________________________________________________________________________

        }

    }
