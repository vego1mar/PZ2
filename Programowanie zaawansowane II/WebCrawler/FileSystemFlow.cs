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
        internal const int WINDOWS_QUALIFIED_DIRECTORY_LENGTH = 248;
        internal const int WINDOWS_QUALIFIED_FILENAME_LENGTH = 260;

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

        private static string gainApplicationLocation()
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
            catch ( Exception x ) {
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
        /// Creates all directories and subdirectories in the specified path unless they already exist.
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
                lastExceptionInfo.causeEvent = "Creating a directory.";
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
                lastExceptionInfo.causeEvent = "Creating a directory.";
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
                lastExceptionInfo.causeEvent = "Creating a directory.";
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
                lastExceptionInfo.causeEvent = "Creating a directory.";
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
                lastExceptionInfo.causeEvent = "Creating a directory.";
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
                lastExceptionInfo.causeEvent = "Creating a directory.";
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
                lastExceptionInfo.causeEvent = "Creating a directory.";
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
                lastExceptionInfo.causeEvent = "Creating a directory.";
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
        /// <param name="filePath">A path with the file name.</param>
        /// <param name="fileContent">A content to write into a file.</param>
        /// <returns>'true' if IO operations have been done successfully, 'false' otherwise.</returns>

        public static bool saveTextToFile( string filePath, string fileContent )
            {
            try {
                using ( StreamWriter writer = File.CreateText( filePath ) ) { 
                    writer.Write( fileContent );
                    }
                }
            catch ( UnauthorizedAccessException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = filePath.ToString();
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
                lastExceptionInfo.argument = filePath.ToString();
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
                lastExceptionInfo.argument = filePath.ToString();
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
                lastExceptionInfo.argument = filePath.ToString();
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
                lastExceptionInfo.argument = filePath.ToString();
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
                lastExceptionInfo.argument = filePath.ToString();
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
                lastExceptionInfo.argument = filePath.ToString();
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
                lastExceptionInfo.argument = filePath.ToString();
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
                lastExceptionInfo.argument = filePath.ToString();
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
            string rootDirectoryName = string.Empty;

            try {
                rootDirectoryName = Path.GetDirectoryName( getApplicationFullPath() );
                string rootDirectory = Path.Combine( rootDirectoryName, ROOT_DIRECTORY_NAME );

                if ( createDirectory( rootDirectory ) == true ) {
                    path = rootDirectory;
                    }
                }
            catch ( ArgumentNullException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = rootDirectoryName.ToString();
                lastExceptionInfo.causeEvent = "Creating a root directory.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-6]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( ArgumentException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = rootDirectoryName.ToString();
                lastExceptionInfo.causeEvent = "Creating a root directory.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-6]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( PathTooLongException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = rootDirectoryName.ToString();
                lastExceptionInfo.causeEvent = "Creating a root directory.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-6]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( Exception x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = rootDirectoryName.ToString();
                lastExceptionInfo.causeEvent = "Creating a root directory.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-6]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }

            return ( path );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Limits a length of the passed argument to the number of specified characters starting from the beginning.
        /// </summary>
        /// <param name="length">A length that the given text should have.</param>
        /// <param name="text">A text data to limit.</param>
        /// <returns>A substring from the beginning to the specified number of length.</returns>

        public static string limitCharactersToFirst( int length, string text )
            {
            string qualifiedText = string.Empty;

            try {
                int textLength = text.Length;
                qualifiedText = text.Substring( 0, (length > textLength - 1) ? (textLength) : (length) );
                }
            catch ( ArgumentOutOfRangeException x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = length.ToString();
                lastExceptionInfo.causeEvent = "Substringing a text.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-5]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( Exception x ) {
                lastExceptionInfo.typeName = x.GetType().ToString();
                lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                lastExceptionInfo.argument = length.ToString();
                lastExceptionInfo.causeEvent = "Substringing a text.";
                lastExceptionInfo.message = x.Message;
                lastExceptionInfo.id = "[FSF-5]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }

            return ( qualifiedText );
            }

        //______________________________________________________________________________________________________________________________

        }
    }
