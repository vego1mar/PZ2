using System;
using System.IO;
using System.Text.RegularExpressions;

namespace WebCrawler
    {

    /// <summary>
    /// Aggregates procedures for dealing with the file system manipulations on files and directories.
    /// </summary>

    internal class FileSystemFlow
        {

        internal const string ROOT_DIRECTORY_NAME = "web";

        private static string applicationFullPath = gainApplicationLocation();
        private static StdErrFlow.ExceptionInfo lastExceptionInfo;

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// The default constructor. There is no point to instantiate this class.
        /// </summary>

        private FileSystemFlow()
            {
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Tries to retrieve the application location (a path with the application file name) at runtime.
        /// </summary>
        /// <returns>The application full path or an empty string if any error occurs.</returns>

        public static string gainApplicationLocation()
            {
            string appPath = string.Empty;

            try {
                appPath = typeof( Program ).Assembly.Location;
                }
            catch ( NotSupportedException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = string.Empty;
                lastExceptionInfo.causeEvent = "Gaining the application location.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-1]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( Exception x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = string.Empty;
                lastExceptionInfo.causeEvent = "Gaining the application location.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-1]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ")" );
                StdErrFlow.writeLine( Environment.NewLine );
                }

            return ( appPath );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Removes all of the reserved in the Windows file system characters.
        /// Please note that this method do not removes the reserved special file names, i.e. CON, COM0, LPT0, AUX, NUL, PRN etc.
        /// </summary>
        /// <param name="path">A file system path to check.</param>
        /// <returns>The passed argument without Windows illegal paths and filenames characters.</returns>

        public static string removeWindowsFileSystemReservedCharacters( string path )
            {
            string validWindowsPath = path;

            try {
                string regexSearch = new string( Path.GetInvalidFileNameChars() ) + new string( Path.GetInvalidPathChars() );
                Regex regex = new Regex( string.Format("[{0}]", Regex.Escape( regexSearch )) );
                validWindowsPath = regex.Replace( path, "" );
                }
            catch ( ArgumentNullException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = path.ToString();
                lastExceptionInfo.causeEvent = "Removing Windows file system reserved characters.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-2]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( ArgumentException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = path.ToString();
                lastExceptionInfo.causeEvent = "Removing Windows file system reserved characters.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-2]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( FormatException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = path.ToString();
                lastExceptionInfo.causeEvent = "Removing Windows file system reserved characters.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-2]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( RegexMatchTimeoutException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = path.ToString();
                lastExceptionInfo.causeEvent = "Removing Windows file system reserved characters.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-2]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }

            return ( validWindowsPath );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Creates a directory using the passed path.
        /// </summary>
        /// <param name="path">An absolute path containing the directory name to create.</param>
        /// <returns>'true' if the directory has been created, 'false' otherwise.</returns>

        public static bool createDirectory( string path )
            {
            try {
                Directory.CreateDirectory( path );
                }
            catch ( DirectoryNotFoundException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = path.ToString();
                lastExceptionInfo.causeEvent = "Removing Windows file system reserved characters.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-3]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                return ( false );
                }
            catch ( PathTooLongException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = path.ToString();
                lastExceptionInfo.causeEvent = "Removing Windows file system reserved characters.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-3]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                return ( false );
                }
            catch ( IOException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = path.ToString();
                lastExceptionInfo.causeEvent = "Removing Windows file system reserved characters.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-3]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                return ( false );
                }
            catch ( UnauthorizedAccessException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = path.ToString();
                lastExceptionInfo.causeEvent = "Removing Windows file system reserved characters.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-3]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                return ( false );
                }
            catch ( ArgumentNullException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = path.ToString();
                lastExceptionInfo.causeEvent = "Removing Windows file system reserved characters.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-3]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                return ( false );
                }
            catch ( ArgumentException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = path.ToString();
                lastExceptionInfo.causeEvent = "Removing Windows file system reserved characters.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-3]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                return ( false );
                }
            catch ( NotSupportedException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = path.ToString();
                lastExceptionInfo.causeEvent = "Removing Windows file system reserved characters.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-3]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                return ( false );
                }
            catch ( Exception x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = path.ToString();
                lastExceptionInfo.causeEvent = "Removing Windows file system reserved characters.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-3]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                return ( false );
                }

            return ( true );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Validates the directory creation.
        /// </summary>
        /// <param name="path">A path with the directory to check.</param>
        /// <returns>'true' if validation has been passed, 'false' otherwise</returns>

        public static bool validateDirectoryCreation( string path )
            {
            if ( Directory.Exists( path ) == false ) {
                return ( false );
                }

            return ( true );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Getter.
        /// </summary>
        /// <returns>The application full path (an absolute directory path plus executable file name).</returns>

        public static string getApplicationFullPath()
            {
            return ( applicationFullPath );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Save the given text data to the specified location with the specified file name into a file.
        /// </summary>
        /// <param name="directoryName">The absolute directory name of the target location.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="fileContent">A content to write into a file.</param>
        /// <returns>'true' if IO operations have been done successfully, 'false' otherwise.</returns>

        public static bool saveTextToFile( string directoryName, string fileName, string fileContent )
            {
            string path = string.Empty;

            try {
                path = Path.Combine( directoryName, fileName );

                using ( StreamWriter writer = File.CreateText( path ) ) { 
                    writer.Write( fileContent );
                    }
                }
            catch ( UnauthorizedAccessException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = path.ToString();
                lastExceptionInfo.causeEvent = "Saving the text content into a file.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-4]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                return ( false );
                }
            catch ( ArgumentNullException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = path.ToString();
                lastExceptionInfo.causeEvent = "Saving the text content into a file.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-4]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                return ( false );
                }
            catch ( ArgumentException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = path.ToString();
                lastExceptionInfo.causeEvent = "Saving the text content into a file.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-4]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                return ( false );
                }
            catch ( PathTooLongException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = path.ToString();
                lastExceptionInfo.causeEvent = "Saving the text content into a file.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-4]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                return ( false );
                }
            catch ( DirectoryNotFoundException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = path.ToString();
                lastExceptionInfo.causeEvent = "Saving the text content into a file.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-4]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                return ( false );
                }
            catch ( NotSupportedException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = path.ToString();
                lastExceptionInfo.causeEvent = "Saving the text content into a file.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-4]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                return ( false );
                }
            catch ( ObjectDisposedException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = path.ToString();
                lastExceptionInfo.causeEvent = "Saving the text content into a file.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-4]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                return ( false );
                }
            catch ( IOException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = path.ToString();
                lastExceptionInfo.causeEvent = "Saving the text content into a file.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-4]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                return ( false );
                }
            catch ( Exception x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = path.ToString();
                lastExceptionInfo.causeEvent = "Saving the text content into a file.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-4]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                return ( false );
                }

            return ( true );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Getter.
        /// </summary>
        /// <returns>A 'struct' with the data about the last exception occurence.</returns>

        public static StdErrFlow.ExceptionInfo getLastExceptionInfo()
            {
            return ( lastExceptionInfo );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Creates the root directory using a directory name defined in a constant ROOT_DIRECTORY_NAME.
        /// Root directory means creating a directory in the location of executable program file location.
        /// </summary>
        /// <returns>A path of the root directory on successful directory creation, 'null' otherwise</returns>

        public static string createRootDirectory()
            {
            string path = null;
            string rootDirectoryName = Path.GetDirectoryName( getApplicationFullPath() );
            string rootDirectory = Path.Combine( rootDirectoryName, ROOT_DIRECTORY_NAME );

            if ( createDirectory( rootDirectory ) == true ) {
                path = rootDirectory;
                }

            return ( rootDirectory );
            }

        //______________________________________________________________________________________________________________________________

        }
    }
