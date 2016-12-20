using System;
using System.Net;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// A simple web crawler with three levels of searching depth.
/// It establishes only the HTTP connections and looking for only the absolute links (URIs).
/// StdErr stream is redirected on main window load into a file whose name is defined in a constant STDERR_FILENAME.
/// </summary>

namespace WebCrawler
    {
    public partial class MainWindow : Form
        {

        private uint foundedLinksCounter;

        private TextWriter stdErrStream;
        internal const string STDERR_FILENAME = "errlog.txt";

        private string applicationPath;
        internal const string ROOT_DIRECTORY_NAME = "web";

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Invokes InitializeComponent() method that is required for Designer support method.
        /// </summary>

        public MainWindow()
            {
            InitializeComponent();

            // Touch the fields.
            this.foundedLinksCounter = 0;
            this.stdErrStream = null;
            this.applicationPath = null;
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// It tries to redirect standard error stream into a file whose name is defined in a constant STDERR_FILENAME.
        /// The file stream is opening for append.
        /// This function is used every time when the main window is formed (loaded).
        /// </summary>
        /// <returns>true if function code has been traversed, false when any error occured</returns>

        private bool tryToRedirectStdErr()
            {
            try {
                this.stdErrStream = new StreamWriter( STDERR_FILENAME, true );
                Console.SetError( this.stdErrStream );

                if ( this.stdErrStream == null ) {
                    return ( false );
                    }

                string appName = typeof( WebCrawler.Program ).Assembly.Location;
                this.applicationPath = appName;
                appName = appName.Substring(appName.LastIndexOf('\\') + 1);
                Console.Error.WriteLine();
                Console.Error.WriteLine("=============================================");
                Console.Error.WriteLine("Error log for {0}", appName);
                Console.Error.WriteLine("Timestamp: {0}", DateTime.Now);
                Console.Error.WriteLine("=============================================");
                }
            catch ( UnauthorizedAccessException x ) {
                Console.Error.WriteLine("[0] UnauthorizedAccessException: " + x.Message);
                MessageBox.Show(this,x.Message,"Redirecting StdErr");
                return ( false );
                }
            catch ( NotSupportedException x ) {
                Console.Error.WriteLine("[0] NotSupportedException: " + x.Message);
                MessageBox.Show(this, x.Message, "Redirecting StdErr");
                return ( false );
                }
            catch ( ArgumentNullException x ) {
                Console.Error.WriteLine("[0] ArgumentNullException: " + x.Message);
                MessageBox.Show(this, x.Message, "Redirecting StdErr");
                return ( false );
                }
            catch ( ArgumentException x ) {
                Console.Error.WriteLine("[0] ArgumentException: " + x.Message);
                MessageBox.Show(this, x.Message, "Redirecting StdErr");
                return ( false );
                }
            catch ( DirectoryNotFoundException x ) {
                Console.Error.WriteLine("[0] DirectoryNotFoundException: " + x.Message);
                MessageBox.Show(this, x.Message, "Redirecting StdErr");
                return ( false );
                }
            catch ( PathTooLongException x ) {
                Console.Error.WriteLine("[0] PathTooLongException: " + x.Message);
                MessageBox.Show(this, x.Message, "Redirecting StdErr");
                return ( false );
                }
            catch ( IOException x ) {
                Console.Error.WriteLine("[0] IOException: " + x.Message);
                MessageBox.Show(this, x.Message, "Redirecting StdErr");
                return ( false );
                }
            catch ( System.Security.SecurityException x ) {
                Console.Error.WriteLine("[0] SecurityException: " + x.Message);
                MessageBox.Show(this, x.Message, "Redirecting StdErr");
                return ( false );
                }
            catch ( Exception x ) {
                Console.Error.WriteLine("[0] Exception: " + x.Message);
                MessageBox.Show(this, x.Message, "Redirecting StdErr");
                return ( false );
                }

            return ( true );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// An action performed when the main window is formed.
        /// </summary>
        /// <param name="sender">The GUI component that cause the action.</param>
        /// <param name="e">Arguments of the action related with the GUI sender component.</param>

        private void MainWindow_Load( object sender, EventArgs e )
            {
            this.tryToRedirectStdErr();
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// An action performed when the main window is formed.
        /// </summary>
        /// <param name="sender">The GUI component that cause the action.</param>
        /// <param name="e">Provides data for the FormClosed event.</param>

        private void MainWindow_FormClosed( object sender, FormClosedEventArgs e )
            {
            if ( this.stdErrStream != null ) {
                Console.Error.WriteLine();
                Console.Error.WriteLine();
                this.stdErrStream.Close();
                }
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// An action performed when the 'Proceed' button is 'clicked'.
        /// </summary>
        /// <param name="sender">The GUI component that cause the action.</param>
        /// <param name="e">Arguments of the action related with the GUI sender component.</param>

        private async void proceedButton_Click( object sender, EventArgs e )
            {
            this.foundedLinksCounter = 0;
            this.foundedLinksToUpdateLabel.Text = "0";
            this.disableMainWindowControls();
            this.setCurrentStateToUpdateLabelText( "Pending" );

            if ( this.isInputtedTextValidURL() == false ) {
                this.setCurrentStateToUpdateLabelText("Unvalidated URL");
                this.enableMainWindowControls();
                return;
                }

            string websiteContent = null;
            string exceptionType = null;
            this.probeWebConnectionHavingTypedURL( out websiteContent, out exceptionType );

            if ( websiteContent == null ) {
                string currentStateLabelText = exceptionType + " on the typed URL";
                this.setCurrentStateToUpdateLabelText( currentStateLabelText );
                this.enableMainWindowControls();
                return;
                }

            uint levelOfDepth = this.retrieveTheStateOfSelectedRadioButtonLevel();
            this.setCurrentStateToUpdateLabelText( "Working" );

            // Do not encapsulate this block due to a GUI synchronization problem (refreshing).
            // Task.Factory.StartNew() is simply entering a ThreadPool.
            try {
                await Task.Factory.StartNew( () => 
                    this.crawlThroughTheSite( websiteContent, levelOfDepth ),  
                    TaskCreationOptions.LongRunning
                    );
                }
            catch ( ArgumentNullException x ) {
                Console.Error.WriteLine( "[5] ArgumentNullException: " + x.Message );
                this.setCurrentStateToUpdateLabelText( "ArgumentNullException while awaiting on Task" );
                return;
                }
            catch ( ArgumentOutOfRangeException x ) {
                Console.Error.WriteLine( "[5] ArgumentOutOfRangeException: " + x.Message );
                this.setCurrentStateToUpdateLabelText( "ArgumentOutOfRangeException while awaiting on Task" );
                return;
                }
            catch ( Exception x ) {
                Console.Error.WriteLine( "[5] Exception: " + x.Message );
                this.setCurrentStateToUpdateLabelText( "Exception while awaiting on Task" );
                return;
                }

            this.setCurrentStateToUpdateLabelText( "Done" );
            this.enableMainWindowControls();
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Try to establish a network connection having the typed URL. On successful the external data will be changed.
        /// At procedure start 'out' arguments are assigned to 'null'.
        /// </summary>
        /// <param name="websiteContent">A simple data type for external change on successful connection established.</param>
        /// <param name="exceptionType">The type of the catched exception. If no catch have been made then 'null'.</param>

        private void probeWebConnectionHavingTypedURL( out string websiteContent, out string exceptionType )
            {
            websiteContent = null;
            exceptionType = null;

            try {
                WebClient website = new WebClient();
                websiteContent = website.DownloadString( this.websiteURLTextBox.Text );
                }
            catch ( ArgumentNullException x ) {
                Console.Error.WriteLine("[1] ArgumentNullException: " + x.Message);
                MessageBox.Show(this, "The 'address' parameter is null.", "Exception during URL probing");
                MessageBox.Show(this, x.Message, "Exception during URL probing");
                exceptionType = "ArgumentNullException";
                }
            catch ( WebException x ) {
                Console.Error.WriteLine("[1] WebException: " + x.Message);
                MessageBox.Show(this, "The formed URI is invalid or an error occured while downloading the resource.", "Exception during URL probing");
                MessageBox.Show(this, x.Message, "Exception during URL probing");
                exceptionType = "WebException";
                }
            catch ( NotSupportedException x ) {
                Console.Error.WriteLine("[1] NotSupportedException: " + x.Message);
                MessageBox.Show(this, "The method has been called simultaneously on multiple threads.", "Exception during URL probing");
                MessageBox.Show(this, x.Message, "Exception during URL probing");
                exceptionType = "NotSupportedException";
                }
            catch ( Exception x ) {
                Console.Error.WriteLine("[1] Exception: " + x.Message);
                MessageBox.Show(this, "The general exception has been raised.", "Exception during URL probing");
                MessageBox.Show(this, x.Message, "Exception during URL probing");
                exceptionType = "Exception";
                }
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Validate a website URL given from the user from the main window's text box as HTTP or HTTPS URI.
        /// </summary>
        /// <returns>true if text box text is a correct URI, false otherwise</returns>

        private bool isInputtedTextValidURL()
            {
            string websiteURL = this.websiteURLTextBox.Text;

            if ( this.isURLvalid( websiteURL ) == false ) {
                MessageBox.Show(this, "The URL scheme has not been found.\nUse absolute path of \"http://www.\"", "URL scheme validation");
                return ( false );
                }

            return ( true );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Disables controls on the main window after 'Proceed' button click.
        /// </summary>

        private void disableMainWindowControls()
            {
            this.proceedButton.Enabled = false;
            this.websiteURLTextBox.Enabled = false;
            this.level1RadioButton.Enabled = false;
            this.level2RadioButton.Enabled = false;
            this.level3RadioButton.Enabled = false;
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Enables controls on the main window when the 'Proceed' button operation will be finished.
        /// </summary>

        private void enableMainWindowControls()
            {
            this.proceedButton.Enabled = true;
            this.websiteURLTextBox.Enabled = true;
            this.level1RadioButton.Enabled = true;
            this.level2RadioButton.Enabled = true;
            this.level3RadioButton.Enabled = true;
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Retrieves the state of selected radio button with a 'Level' label.
        /// This is a number from the set of { 1, 2, 3 }.
        /// If none of the proper radio button is selected, then the default value of 1 ('Level 1') will be returned.
        /// </summary>
        /// <returns>A number representing the selected radio button in the GUI.</returns>

        private uint retrieveTheStateOfSelectedRadioButtonLevel()
            {
            bool level1 = this.level1RadioButton.Checked;
            bool level2 = this.level2RadioButton.Checked;
            bool level3 = this.level3RadioButton.Checked;

            uint levelOfDepth = 1;

            levelOfDepth = (level1 == true) ? (1) : (levelOfDepth);
            levelOfDepth = (level2 == true) ? (2) : (levelOfDepth);
            levelOfDepth = (level3 == true) ? (3) : (levelOfDepth);

            return ( levelOfDepth );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// A custom event of founded new link that should be noticed. Updates GUI main window.
        /// The change itself is an incremential one.
        /// An exception handling is provided, but obsolete if Application.Exit() will not be used during threads-based processing.
        /// </summary>

        private void foundedLinksCount()
            {
            // 'catch' block may be never reached because of Application.Exit().
            try {
                this.Invoke( ( MethodInvoker ) delegate {
                    this.foundedLinksCounter++;
                    this.foundedLinksToUpdateLabel.Text = this.foundedLinksCounter.ToString();
                    this.foundedLinksToUpdateLabel.Refresh();
                    });
                }
            catch ( ObjectDisposedException x ) {
                Console.Error.WriteLine( "[7] ObjectDisposedException: " + x.Message );
                }
            catch ( Exception x ) {
                Console.Error.WriteLine( "[7] Exception: " + x.Message );
                }
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Checks whether the given string instance is a valid URL or not. Valid means an absolute URI.
        /// </summary>
        /// <param name="source">A string with the URI to check.</param>
        /// <returns>true if it is well formed URI, otherwise false.</returns>

        private bool isURLvalid( string source )
            {
            Uri uriResult;

            return ( Uri.TryCreate(source, UriKind.Absolute, out uriResult ) && 
                (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps) );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// The given website crawling procedure. It searches for the occurencies of <a> tags.
        /// </summary>
        /// <param name="websiteContent">The HTML content of the main website to be proceeded as a start page (level 0).</param>
        /// <param name="levelOfDepth">The level of depth for crawling. This should be a value from a set of {1,2,3}, 
        /// otherwise the default parameter of 1 will be assigned.</param>

        private void crawlThroughTheSite( string websiteContent, uint levelOfDepth )
            {
            // LEVEL 0
            this.setCurrentStateToUpdateLabelText( "Working... Evaluating Level 0" );
            string crawlingRootDirectory = this.createCrawlingRootDirectory();
            bool isRootDirectoryCreated = this.validateDirectoryCreation( crawlingRootDirectory );

            if ( isRootDirectoryCreated == false ) {
                return;
                }

            int firstFullStopIndex = this.websiteURLTextBox.Text.IndexOf( '.' );
            string mainPageName = this.websiteURLTextBox.Text.Substring( firstFullStopIndex + 1 );
            string mainPageDirectory = Path.Combine( crawlingRootDirectory, mainPageName );
            mainPageDirectory = this.removeWindowsFileSystemReservedCharacters( mainPageDirectory );
            this.createDirectory( mainPageDirectory );
            bool isMainPageDirectoryCreated = this.validateDirectoryCreation( mainPageDirectory );

            if ( isMainPageDirectoryCreated == false ) {
                return;
                }

            this.saveWebSiteContentTo( mainPageDirectory, mainPageName, websiteContent );

            // Search for the links on the main site.
            ISet<string> hrefLinks0 = this.getOnlyTheLinks( websiteContent );
            ISet<string> absoluteLinks0 = this.retrieveAbsoluteLinks( hrefLinks0 );

            // LEVEL 1
            this.setCurrentStateToUpdateLabelText( "Working... Evaluating Level 1" );
            string [] contentOfAbsoluteLinks0;
            ISet<string>[] absoluteLinks1 = this.grabAbsoluteLinksFromContentOf( absoluteLinks0, out contentOfAbsoluteLinks0 );
            string level0LinksPath = Path.Combine( mainPageDirectory, "lvl_0" );
            this.createDirectory( level0LinksPath );
            bool isLevel0DirectoryCreated = this.validateDirectoryCreation( level0LinksPath );

            if ( isLevel0DirectoryCreated == false ) {
                return;
                }

            this.setCurrentStateToUpdateLabelText( "Working... Saving Level 0" );
            Console.Error.Flush();

            for ( uint i=0; i<contentOfAbsoluteLinks0.Length; i++ ) {
                string currentFileName = i.ToString();
                this.saveWebSiteContentTo( level0LinksPath, currentFileName, contentOfAbsoluteLinks0[i] );
                }

            // LEVEL 2
            if ( levelOfDepth < 2 ) {
                // TODO - pages level saving correctness through directories and steps
                return;
                }

            this.setCurrentStateToUpdateLabelText( "Working... Evaluating Level 2" );
            ISet<string>[][] absoluteLinks2 = new ISet<string>[ absoluteLinks1.Rank ][];
            int [] absoluteLinks2ArrayDimension2Lengths = new int [ absoluteLinks1.Rank ];
            string level1LinksPath = Path.Combine( mainPageDirectory, "lvl_1" );
            this.createDirectory( level1LinksPath );
            bool isLevel1DirectoryCreated = this.validateDirectoryCreation( level1LinksPath );

            if ( isLevel1DirectoryCreated == false ) {
                return;
                }

            for ( int i=0; i<absoluteLinks1.Rank; i++ ) {
                this.setCurrentStateToUpdateLabelText( "Working... Evaluating Level 2" );
                string [] contentOfAbsoluteLinks1;
                ISet<string>[] currentAbsoluteLinks = this.grabAbsoluteLinksFromContentOf( absoluteLinks1[i], out contentOfAbsoluteLinks1 );
                absoluteLinks2[i] = currentAbsoluteLinks;
                absoluteLinks2ArrayDimension2Lengths[i] += currentAbsoluteLinks.Length;
                this.setCurrentStateToUpdateLabelText( "Working... Saving Level 1" );
                Console.Error.Flush();

                for ( uint j=0; j<contentOfAbsoluteLinks1.Length; j++ ) {
                    string currentFileName = i + "-" + j;
                    this.saveWebSiteContentTo( level1LinksPath, currentFileName, contentOfAbsoluteLinks1[j] );
                    }
                }

            // LEVEL 3
            if ( levelOfDepth < 3 ) {
                return;
                }

            this.setCurrentStateToUpdateLabelText( "Working... Evaluating Level 3" );
            ISet<string>[][][] absoluteLinks3 = new ISet<string>[ absoluteLinks2.Rank ][][];
            string level2LinksPath = Path.Combine( mainPageDirectory, "lvl_2" );
            this.createDirectory( level2LinksPath );
            bool isLevel2DirectoryCreated = this.validateDirectoryCreation( level2LinksPath );

            if ( isLevel2DirectoryCreated == false ) {
                return;
                }

            for ( int i=0; i<absoluteLinks2.Rank; i++ ) {
                // The 'new' keyword is used in this block to prevent from raising a NullReferenceException.
                absoluteLinks3[i] = new ISet<string>[ absoluteLinks2ArrayDimension2Lengths[i] ][];

                for ( int j=0; j<absoluteLinks2ArrayDimension2Lengths[i]; j++ ) {
                    this.setCurrentStateToUpdateLabelText( "Working... Evaluating Level 3" );
                    string [] contentOfAbsoluteLinks2;
                    ISet<string>[] currentAbsoluteLinks = this.grabAbsoluteLinksFromContentOf( absoluteLinks2[i][j], out contentOfAbsoluteLinks2 );

                    for ( int k=0; k<absoluteLinks2ArrayDimension2Lengths[i]; k++ ) {
                        absoluteLinks3[i][k] = new ISet<string>[ currentAbsoluteLinks.Length ];
                        }

                    absoluteLinks3[i][j] = currentAbsoluteLinks;
                    this.setCurrentStateToUpdateLabelText( "Working... Saving Level 2" );
                    Console.Error.Flush();

                    for ( uint k=0; k<contentOfAbsoluteLinks2.Length; k++ ) {
                        string currentFileName = i + "-" + j + "-" + k;
                        this.saveWebSiteContentTo( level2LinksPath, currentFileName, contentOfAbsoluteLinks2[k] );
                        }
                    }
                }

            // Only websites content saving is revelant here. Retrieving the absolute links is obsolete and consume CPU and memory.
            string level3LinksPath = Path.Combine( mainPageDirectory, "lvl_3" );
            this.createDirectory( level3LinksPath );
            bool isLevel3DirectoryCreated = this.validateDirectoryCreation( level3LinksPath );

            if ( isLevel3DirectoryCreated == false ) {
                return;
                }

            this.setCurrentStateToUpdateLabelText( "Working... Saving Level 3" );
            int iSet = -1;
            int jSubset = -1;
            int kCollection = -1;

            foreach ( var set in absoluteLinks3 ) {
                iSet++;
                this.setCurrentStateToUpdateLabelText( "Working... Saving Level 3 (I)... " + iSet );

                foreach ( var subset in set ) {
                    jSubset++;
                    this.setCurrentStateToUpdateLabelText( "Working... Saving Level 3 (II)... " + jSubset );

                    foreach ( var collection in subset ) {
                        kCollection++;
                        this.setCurrentStateToUpdateLabelText( "Working... Saving Level 3 (III)... " + kCollection );
                        string [] contentOfCurrentAbsoluteLinks;
                        this.grabAbsoluteLinksFromContentOf( collection, out contentOfCurrentAbsoluteLinks );
                        Console.Error.Flush();

                        for ( uint l=0; l<contentOfCurrentAbsoluteLinks.Length; l++ ) {
                            this.setCurrentStateToUpdateLabelText( "Working... Saving Level 3 (IV)... " + l );
                            string currentFileName = iSet + "-" + jSubset + "-" + kCollection + "-" + l;
                            this.saveWebSiteContentTo( level3LinksPath, currentFileName, contentOfCurrentAbsoluteLinks[l] );
                            }
                        }
                    }
                }

            // TODO - test level 3 loops complete execution
            // TODO - add a timer on a new thread for the application instance?
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Downloads every HTML page from the passed set of URLs and traverses its content for retrieving the absolute links.
        /// A non-blocking traversing operation with exceptions handling implementation is provided.
        /// </summary>
        /// <param name="absoluteLinks">A collection of absolute links to be probed by a web connection and downloaded.</param>
        /// <param name="contentOfAbsoluteLinks">The content from passed absolute links saved in an external array. It is nullified at start.</param>
        /// <returns>A collection array of absolute links retrieved from the URLs given as a parameter.</returns>

        private ISet<string>[] grabAbsoluteLinksFromContentOf( ISet<string> absoluteLinks, out string[] contentOfAbsoluteLinks )
            {
            string[] sitesContent = new string[ absoluteLinks.Count ];
            ISet<string>[] nextAbsoluteLinks = new HashSet<string>[ absoluteLinks.Count ];
            contentOfAbsoluteLinks = null;
            Thread joinedWorkThread;

            try {
                joinedWorkThread = new Thread( () =>
                    this.downloadWebPagesFromGivenAbsoluteLinks( ref absoluteLinks, ref sitesContent )
                    );

                joinedWorkThread.Start();
                joinedWorkThread.Join();
                contentOfAbsoluteLinks = sitesContent;

                joinedWorkThread = new Thread( () =>
                    this.traverseContentForRetrievingAbsoluteLinks( sitesContent, ref nextAbsoluteLinks )
                    );

                joinedWorkThread.Start();
                joinedWorkThread.Join();
                }
            catch ( ArgumentNullException x ) {
                Console.Error.WriteLine( "[6] ArgumentNullException: " + x.Message + "Critical error" );
                MessageBox.Show( null, "ArgumentNullException while working on threads. Returning all control paths.", "Critical error" );
                Application.Exit();
                }
            catch ( ThreadStateException x ) {
                Console.Error.WriteLine( "[6] ThreadStateException: " + x.Message + "Critical error" );
                MessageBox.Show( null, "ThreadStateException while working on threads. Returning all control paths.", "Critical error" );
                Application.Exit();
                }
            catch ( OutOfMemoryException x ) {
                Console.Error.WriteLine( "[6] OutOfMemoryException: " + x.Message + "Critical error" );
                MessageBox.Show( null, "OutOfMemoryException while working on threads. Returning all control paths.", "Critical error" );
                Application.Exit();
                }
            catch ( ThreadInterruptedException x ) {
                Console.Error.WriteLine( "[6] ThreadInterruptedException: " + x.Message + "Critical error" );
                MessageBox.Show( null, "ThreadInterruptedException while working on threads. Returning all control paths.", "Critical error" );
                Application.Exit();
                }
            catch ( Exception x ) {
                Console.Error.WriteLine( "[6] Exception: " + x.Message + "Critical error" );
                MessageBox.Show( null, "Exception while working on threads. Returning all control paths.", "Critical error" );
                Application.Exit();
                }

            return ( nextAbsoluteLinks );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// A procedure designed for the internal use in the method ::grabAbsoluteLinksFromContentOf().
        /// It should be used only for defining a thread function body.
        /// Downloads every web page of absolute links founded and passed as an argument.
        /// </summary>
        /// <param name="absoluteLinks">A bare bone collection of URLs for network probing.
        /// It is a reference, because it has already been passed to the caller-method ::grabAbsoluteLinksFromContentOf().</param>
        /// <param name="sitesContent">The downloaded content of web pages located through URLs in collection from the passed argument.
        /// This parameter is for the external use.</param>

        private void downloadWebPagesFromGivenAbsoluteLinks( ref ISet<string> absoluteLinks, ref string[] sitesContent )
            {
            WebClient connection = new WebClient();
            uint i = 0;

            // Download every page of absolute links founded.
            // This 'foreach' loop must stay in the body of 'while' loop.
            // This ensure that 'foreach' loop will continue its work after returning from 'catch' block with non-altered iterator.
            while ( i < absoluteLinks.Count ) {
                foreach ( var urlEntry in absoluteLinks ) {
                    if ( i >= absoluteLinks.Count ) {
                        break;
                        }

                    string currentSiteContent = "";

                    try {
                        currentSiteContent = connection.DownloadString( urlEntry );
                        }
                    catch ( ArgumentNullException x ) {
                        Console.Error.WriteLine("[2] ArgumentNullException: " + x.Message + " urlEntry=" + urlEntry);
                        continue;
                        }
                    catch ( WebException x ) {
                        Console.Error.WriteLine("[2] WebException: " + x.Message + " urlEntry=" + urlEntry);
                        continue;
                        }
                    catch ( NotSupportedException x ) {
                        Console.Error.WriteLine("[2] NotSupportedException: " + x.Message + " urlEntry=" + urlEntry);
                        continue;
                        }
                    catch ( Exception x ) {
                        Console.Error.WriteLine("[2] Exception: " + x.Message + " urlEntry=" + urlEntry);
                        continue;
                        }

                    sitesContent[i] = currentSiteContent;
                    i++;
                    }
                }
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// A procedure designed for the internal use in the method ::grabAbsoluteLinksFromContentOf().
        /// It should be used only for defining a thread function body.
        /// Traverses the site's downloaded contents for retrieving the absolute links.
        /// </summary>
        /// <param name="sitesContent">The downloaded content of web pages located through URLs.</param>
        /// <param name="nextAbsoluteLinks">A bare bone collection for the result that will be externally passed.</param>

        private void traverseContentForRetrievingAbsoluteLinks( string[] sitesContent, ref ISet<string>[] nextAbsoluteLinks )
            {
            uint i = 0;

            foreach ( var content in sitesContent ) {
                ISet<string> currentHrefLinks = this.getOnlyTheLinks( content );
                ISet<string> currentAbsoluteLinks = this.retrieveAbsoluteLinks( currentHrefLinks );
                nextAbsoluteLinks[i++] = currentAbsoluteLinks;
                }
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Retrieves only the links from tags of the given argument. It absorbses only the distinct matches.
        /// </summary>
        /// <param name="content">HTML content of the website.</param>
        /// <returns>A collection with links grabbed from the parameter.</returns>

        private ISet<string> getOnlyTheLinks( string content )
            {
            // A regex for the whole <a> tags content: "<a.*?>(.*?)<\\/a>"
            Regex regexLink = new Regex("(?<=<a\\s*?href=(?:'|\"))[^'\"]*?(?=(?:'|\"))");
            ISet<string> newLinks = new HashSet<string>();

            // Preventing an ArgumentNullException raising using Regex.Matches() for pages that returned a WebException.
            if ( content != null ) {
                foreach ( var match in regexLink.Matches( content ) ) {
                    if ( newLinks.Contains( match.ToString() ) == false ) {
                        newLinks.Add( match.ToString() );
                        }
                    }
                }

            return ( newLinks );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Traverse the given collection and searches for absolute links only. It absorbs only the distinct entries.
        /// This funtion is updating GUI by using foundedLinksCount() method.
        /// </summary>
        /// <param name="hrefLinks">A collection returned by getOnlyTheLinks( string ) method.</param>
        /// <returns>A collection with the absolute links only.</returns>

        private ISet<string> retrieveAbsoluteLinks( ISet<string> hrefLinks )
            {
            ISet<string> absoluteLinks = new HashSet<string>();

            foreach ( var entry in hrefLinks ) {
                if ((this.isURLvalid( entry ) == true) && (absoluteLinks.Contains( entry ) == false)) {
                    absoluteLinks.Add( entry );
                    this.foundedLinksCount();
                    }
                }

            return ( absoluteLinks );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// An action performed when the 'Info' is clicked.
        /// </summary>
        /// <param name="sender">The GUI component that cause the action.</param>
        /// <param name="e">Arguments of the action related with the GUI sender component.</param>

        private void InfoButton_Click( object sender, EventArgs e )
            {
            string newLine = Environment.NewLine;
            bool isStdErrRedirectedSuccessfully = ( this.stdErrStream == null ) ? ( false ) : ( true );
            string usedFramework = typeof( string ).Assembly.ImageRuntimeVersion;
            bool x64Process = Environment.Is64BitProcess;
            int currentManagedThreadID = Environment.CurrentManagedThreadId;

            string msgBoxText = "A simple web crawler with three levels of searching depth." + newLine +
                "It establishes only the HTTP connections and looking for only the absolute links (URIs)." + newLine +
                newLine +
                "StdErr stream redirected: " + isStdErrRedirectedSuccessfully + newLine +
                "StdErr file name: " + STDERR_FILENAME + newLine +
                newLine +
                "Application path: " + this.applicationPath + newLine +
                ".NET Framework: " + usedFramework + newLine +
                "64-bit process: " + x64Process + newLine +
                "Current managed thread ID: " + currentManagedThreadID + newLine;

            MessageBox.Show( this, msgBoxText, "Information" );
            // TODO - custom message box
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Sets the name corresponding component text by a passed argument value and refreshes the GUI main window.
        /// An exception handling is provided, but obsolete if Application.Exit() will not be used during threads-based processing.
        /// </summary>
        /// <param name="labelText">Text of the name corresponding label to set. Use of 'null' here is discouraged.</param>

        private void setCurrentStateToUpdateLabelText( string labelText )
            {
            // 'catch' block may be never reached because of Application.Exit().       
            try {
                this.Invoke( ( MethodInvoker ) delegate {
                    this.currentStateToUpdateLabel.Text = labelText;
                    this.currentStateToUpdateLabel.Refresh();
                    });
                }
            catch ( ObjectDisposedException x ) {
                Console.Error.WriteLine( "[8] ObjectDisposedException: " + x.Message );
                }
            catch ( Exception x ) {
                Console.Error.WriteLine( "[8] Exception: " + x.Message );
                }
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Creates a directory using passed path. This method come with an exception handling.
        /// </summary>
        /// <param name="path">An absolute path containing the directory name to create.</param>
        /// <returns>'true' if the directory has been created, 'false' otherwise or when an Exception have been raised</returns>

        private bool createDirectory( string path )
            {
            try {
                Directory.CreateDirectory( path );
                }
            catch ( DirectoryNotFoundException x ) {
                Console.Error.WriteLine( "[3] DirectoryNotFoundException: " + x.Message + " path=" + path );
                return ( false );
                }
            catch ( PathTooLongException x ) {
                Console.Error.WriteLine( "[3] PathTooLongException: " + x.Message + " path=" + path );
                return ( false );
                }
            catch ( IOException x ) {
                Console.Error.WriteLine( "[3] IOException: " + x.Message + " path=" + path );
                return ( false );
                }
            catch ( UnauthorizedAccessException x ) {
                Console.Error.WriteLine( "[3] UnauthorizedAccessException: " + x.Message + " path=" + path );
                return ( false );
                }
            catch ( ArgumentNullException x ) {
                Console.Error.WriteLine( "[3] ArgumentNullException: " + x.Message + " path=" + path );
                return ( false );
                }
            catch ( ArgumentException x ) {
                Console.Error.WriteLine( "[3] ArgumentException: " + x.Message + " path=" + path );
                return ( false );
                }
            catch ( NotSupportedException x ) {
                Console.Error.WriteLine( "[3] NotSupportedException: " + x.Message + " path=" + path );
                return ( false );
                }
            catch ( Exception x ) {
                Console.Error.WriteLine( "[3] Exception: " + x.Message + " path=" + path );
                return ( false );
                }

            return ( true );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Creates the root directory for the crawled web pages using a directory name defined in a constant ROOT_DIRECTORY_NAME.
        /// </summary>
        /// <returns>A path of the crawling root directory on successful directory creation, 'null' otherwise</returns>

        private string createCrawlingRootDirectory()
            {
            string path = null;
            string rootDirectoryName = Path.GetDirectoryName( this.applicationPath );
            string crawlingRootDirectory = Path.Combine( rootDirectoryName, ROOT_DIRECTORY_NAME );
            bool isDirectoryCreated = this.createDirectory( crawlingRootDirectory );

            if ( isDirectoryCreated == true ) {
                path = crawlingRootDirectory;
                }

            return ( crawlingRootDirectory );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Validates the directory creation. On a negative result a MessageBox.Show() and a Console.Error.WriteLine() will be involved.
        /// </summary>
        /// <param name="path">A path with the directory to check.</param>
        /// <returns>'true' if validation has been passed, 'false' otherwise</returns>

        private bool validateDirectoryCreation( string path )
            {
            bool isDirectoryExisting = Directory.Exists( path );

            if ( isDirectoryExisting == false ) {
                string message = "The directory has not been created for a path=" + path;
                Console.Error.WriteLine( message );
                MessageBox.Show( this, message, "Directory creation failed" );
                return ( false );
                }

            return ( true );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Removes all of the reserved in the Windows file system characters to an underscore ('_').
        /// Please note that this method do not removes the reserved special file names, i.e. CON, COM0, LPT0, AUX, NUL, PRN etc.
        /// </summary>
        /// <param name="path">A path to be checked.</param>
        /// <returns>Passed 'string' without Windows illegal paths and filenames characters.</returns>

        private string removeWindowsFileSystemReservedCharacters( string path )
            {
            string validWindowsPath = path;
            string regexSearch = new string( Path.GetInvalidFileNameChars() ) + new string( Path.GetInvalidPathChars() );
            Regex regex = new Regex( string.Format("[{0}]", Regex.Escape( regexSearch )) );
            path = regex.Replace( path, "" );

            return ( validWindowsPath );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Save the given website content to the specified location with the specified name. The exception handling is provided.
        /// </summary>
        /// <param name="directoryName">The absolute directory name of the target location.</param>
        /// <param name="filename">The name of the file.</param>
        /// <param name="websiteContent">A content of the website to write into a file.</param>
        /// <returns>'true' if IO operations have been done successfully, 'false' otherwise</returns>

        private bool saveWebSiteContentTo( string directoryName, string filename, string websiteContent )
            {
            string partialErrorMessage = " directoryName=" + directoryName + " filename=" + filename;

            try {
                string path = Path.Combine( directoryName, filename );

                using ( StreamWriter writer = File.CreateText( path ) ) { 
                    writer.Write( websiteContent );
                    }
                }
            catch ( UnauthorizedAccessException x ) {
                Console.Error.WriteLine( "[4] UnauthorizedAccessException: " + x.Message + partialErrorMessage );
                return ( false );
                }
            catch ( ArgumentNullException x ) {
                Console.Error.WriteLine( "[4] ArgumentNullException: " + x.Message + partialErrorMessage );
                return ( false );
                }
            catch ( ArgumentException x ) {
                Console.Error.WriteLine( "[4] ArgumentException: " + x.Message + partialErrorMessage );
                return ( false );
                }
            catch ( PathTooLongException x ) {
                Console.Error.WriteLine( "[4] PathTooLongException: " + x.Message + partialErrorMessage );
                return ( false );
                }
            catch ( DirectoryNotFoundException x ) {
                Console.Error.WriteLine( "[4] DirectoryNotFoundException: " + x.Message + partialErrorMessage );
                return ( false );
                }
            catch ( NotSupportedException x ) {
                Console.Error.WriteLine( "[4] NotSupportedException: " + x.Message + partialErrorMessage );
                return ( false );
                }
            catch ( ObjectDisposedException x ) {
                Console.Error.WriteLine( "[4] ObjectDisposedException: " + x.Message + partialErrorMessage );
                return ( false );
                }
            catch ( IOException x ) {
                Console.Error.WriteLine( "[4] IOException: " + x.Message + partialErrorMessage );
                return ( false );
                }
            catch ( Exception x ) {
                Console.Error.WriteLine( "[4] Exception: " + x.Message + partialErrorMessage );
                return ( false );
                }

            return ( true );
            }

        //______________________________________________________________________________________________________________________________

        }
    }
