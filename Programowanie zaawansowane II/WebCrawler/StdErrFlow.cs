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
            public string argName;
            public string argValue;
            public string message;
            public string id;
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// It tries to redirect standard error stream into a file whose name is defined in a constant STDERR_FILENAME.
        /// </summary>
        /// <returns>'true' if function code has been traversed, 'false' when any error occured.</returns>

        public static bool tryToRedirectStdErr()
            {
            try {
                oldStdErrStream = Console.Error;
                newStdErrStream = new StreamWriter( STDERR_FILENAME, false );
                Console.SetError( newStdErrStream );

                if ( newStdErrStream == null ) {
                    return ( false );
                    }

                isLastRedirectionSuccessfull = true;

                string appName = FileSystemFlow.getApplicationFullPath();
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
                lastExceptionInfo.argName = newStdErrStream.GetType().FullName + "~" + nameof( newStdErrStream );
                lastExceptionInfo.argValue = newStdErrStream.ToString();
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
                lastExceptionInfo.argName = newStdErrStream.GetType().FullName + "~" + nameof( newStdErrStream );
                lastExceptionInfo.argValue = newStdErrStream.ToString();
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
                lastExceptionInfo.argName = newStdErrStream.GetType().FullName + "~" + nameof( newStdErrStream );
                lastExceptionInfo.argValue = newStdErrStream.ToString();
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
                lastExceptionInfo.argName = newStdErrStream.GetType().FullName + "~" + nameof( newStdErrStream );
                lastExceptionInfo.argValue = newStdErrStream.ToString();
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
                lastExceptionInfo.argName = newStdErrStream.GetType().FullName + "~" + nameof( newStdErrStream );
                lastExceptionInfo.argValue = newStdErrStream.ToString();
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
                lastExceptionInfo.argName = newStdErrStream.GetType().FullName + "~" + nameof( newStdErrStream );
                lastExceptionInfo.argValue = newStdErrStream.ToString();
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
                lastExceptionInfo.argName = newStdErrStream.GetType().FullName + "~" + nameof( newStdErrStream );
                lastExceptionInfo.argValue = newStdErrStream.ToString();
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
                lastExceptionInfo.argName = newStdErrStream.GetType().FullName + "~" + nameof( newStdErrStream );
                lastExceptionInfo.argValue = newStdErrStream.ToString();
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
                lastExceptionInfo.argName = newStdErrStream.GetType().FullName + "~" + nameof( newStdErrStream );
                lastExceptionInfo.argValue = newStdErrStream.ToString();
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
                lastExceptionInfo.argName = newStdErrStream.GetType().FullName + "~" + nameof( newStdErrStream );
                lastExceptionInfo.argValue = newStdErrStream.ToString();
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
                lastExceptionInfo.argName = text.GetType().FullName + "~" + nameof( text );
                lastExceptionInfo.argValue = text.ToString();
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-1]";
                return ( false );
                }
            catch ( InvalidOperationException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argName = text.GetType().FullName + "~" + nameof( text );
                lastExceptionInfo.argValue = text.ToString();
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-1]";
                return ( false );
                }
            catch ( IOException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argName = text.GetType().FullName + "~" + nameof( text );
                lastExceptionInfo.argValue = text.ToString();
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-1]";
                return ( false );
                }
            catch ( Exception x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argName = text.GetType().FullName + "~" + nameof( text );
                lastExceptionInfo.argValue = text.ToString();
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
                lastExceptionInfo.argName = oldStdErrStream.GetType().FullName + "~" + nameof( oldStdErrStream );
                lastExceptionInfo.argValue = oldStdErrStream.ToString();
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
                lastExceptionInfo.argName = oldStdErrStream.GetType().FullName + "~" + nameof( oldStdErrStream );
                lastExceptionInfo.argValue = oldStdErrStream.ToString();
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
                lastExceptionInfo.argName = oldStdErrStream.GetType().FullName + "~" + nameof( oldStdErrStream );
                lastExceptionInfo.argValue = oldStdErrStream.ToString();
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
                lastExceptionInfo.argName = oldStdErrStream.GetType().FullName + "~" + nameof( oldStdErrStream );
                lastExceptionInfo.argValue = oldStdErrStream.ToString();
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
                lastExceptionInfo.argName = oldStdErrStream.GetType().FullName + "~" + nameof( oldStdErrStream );
                lastExceptionInfo.argValue = oldStdErrStream.ToString();
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
                lastExceptionInfo.argName = oldStdErrStream.GetType().FullName + "~" + nameof( oldStdErrStream );
                lastExceptionInfo.argValue = oldStdErrStream.ToString();
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

        /// <summary>
        /// Temporarily redirects standard error stream to the previous handle and read the whole of the error log file content.
        /// </summary>
        /// <returns>The current content of the error log.</returns>

        public static string getCurrentStdErrLogContent()
            {
            string stdErrLogContent = string.Empty;

            try {
                Console.SetError( oldStdErrStream );
                newStdErrStream.Close();

                using ( var reader = new StreamReader( STDERR_FILENAME, System.Text.Encoding.UTF8 ) ) {
                    stdErrLogContent = reader.ReadToEnd();
                    }

                newStdErrStream = new StreamWriter( STDERR_FILENAME, true );
                Console.SetError( newStdErrStream );
                }
            catch ( ArgumentNullException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argName = stdErrLogContent.GetType().FullName + "~" + nameof( stdErrLogContent );
                lastExceptionInfo.argValue = stdErrLogContent.ToString();
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-4]";
                writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                writeLine( Environment.NewLine );
                }
            catch ( UnauthorizedAccessException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argName = stdErrLogContent.GetType().FullName + "~" + nameof( stdErrLogContent );
                lastExceptionInfo.argValue = stdErrLogContent.ToString();
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-4]";
                writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                writeLine( Environment.NewLine );
                }
            catch ( ArgumentException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argName = stdErrLogContent.GetType().FullName + "~" + nameof( stdErrLogContent );
                lastExceptionInfo.argValue = stdErrLogContent.ToString();
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-4]";
                writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                writeLine( Environment.NewLine );
                }
            catch ( FileNotFoundException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argName = stdErrLogContent.GetType().FullName + "~" + nameof( stdErrLogContent );
                lastExceptionInfo.argValue = stdErrLogContent.ToString();
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-4]";
                writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                writeLine( Environment.NewLine );
                }
            catch ( DirectoryNotFoundException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argName = stdErrLogContent.GetType().FullName + "~" + nameof( stdErrLogContent );
                lastExceptionInfo.argValue = stdErrLogContent.ToString();
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-4]";
                writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                writeLine( Environment.NewLine );
                }
            catch ( OutOfMemoryException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argName = stdErrLogContent.GetType().FullName + "~" + nameof( stdErrLogContent );
                lastExceptionInfo.argValue = stdErrLogContent.ToString();
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-4]";
                writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                writeLine( Environment.NewLine );
                }
            catch ( NotSupportedException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argName = stdErrLogContent.GetType().FullName + "~" + nameof( stdErrLogContent );
                lastExceptionInfo.argValue = stdErrLogContent.ToString();
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-4]";
                writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                writeLine( Environment.NewLine );
                }
            catch ( PathTooLongException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argName = stdErrLogContent.GetType().FullName + "~" + nameof( stdErrLogContent );
                lastExceptionInfo.argValue = stdErrLogContent.ToString();
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-4]";
                writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                writeLine( Environment.NewLine );
                }
            catch ( System.Security.SecurityException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argName = stdErrLogContent.GetType().FullName + "~" + nameof( stdErrLogContent );
                lastExceptionInfo.argValue = stdErrLogContent.ToString();
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-4]";
                writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                writeLine( Environment.NewLine );
                }
            catch ( IOException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argName = stdErrLogContent.GetType().FullName + "~" + nameof( stdErrLogContent );
                lastExceptionInfo.argValue = stdErrLogContent.ToString();
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-4]";
                writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                writeLine( Environment.NewLine );
                }
            catch ( Exception x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argName = stdErrLogContent.GetType().FullName + "~" + nameof( stdErrLogContent );
                lastExceptionInfo.argValue = stdErrLogContent.ToString();
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[SEF-4]";
                writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                writeLine( Environment.NewLine );
                }

            return ( stdErrLogContent );
            }

        //______________________________________________________________________________________________________________________________

        }

    }
