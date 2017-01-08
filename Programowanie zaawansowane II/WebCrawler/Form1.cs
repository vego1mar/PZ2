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

        private ulong foundedLinksCounter;

        Thread timerThread;

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// The default constructor.
        /// Invokes InitializeComponent() method that is required for Designer support method.
        /// </summary>

        public MainWindow()
            {
            InitializeComponent();

            this.foundedLinksCounter = 0;
            this.timerThread = null;

            this.notifyUserToStartTimerOrNot();
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// An action performed when the main window is formed.
        /// </summary>
        /// <param name="sender">The GUI component that cause the action.</param>
        /// <param name="e">Arguments of the action related with the GUI sender component.</param>

        private void MainWindow_Load( object sender, EventArgs e )
            {
            try { 
                Thread redirectingStdErrThread = new Thread( () => {
                    StdErrFlow.tryToRedirectStdErr();
                    });

                redirectingStdErrThread.Start();
                }
            catch ( ArgumentNullException x ) {
                StdErrFlow.writeLine( "[10] ArgumentNullException: " + x.Message + "object=redirectingStdErrThread" );
                }
            catch ( ThreadStateException x ) {
                StdErrFlow.writeLine( "[10] ThreadStateException: " + x.Message + "object=redirectingStdErrThread" );
                }
            catch ( OutOfMemoryException x ) {
                StdErrFlow.writeLine( "[10] OutOfMemoryException: " + x.Message + "object=redirectingStdErrThread" );
                }
            catch ( Exception x ) {
                StdErrFlow.writeLine( "[10] Exception: " + x.Message + "object=redirectingStdErrThread" );
                }
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// An action performed when the main window is formed.
        /// </summary>
        /// <param name="sender">The GUI component that cause the action.</param>
        /// <param name="e">Provides data for the FormClosed event.</param>

        private void MainWindow_FormClosed( object sender, FormClosedEventArgs e )
            {
            StdErrFlow.writeLine( Environment.NewLine );
            StdErrFlow.tryToRetrievePreviousStdErr();
            StdErrFlow.closeStreams();

            Application.ExitThread();
            Application.Exit();
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

            // Do not encapsulate this block due to a GUI synchronization problem with refreshing.
            // Task.Factory.StartNew() simply means entering a ThreadPool.
            try {
                await Task.Factory.StartNew( () => 
                    this.crawlThroughTheSite( websiteContent, levelOfDepth ),  
                    TaskCreationOptions.LongRunning
                    );
                }
            catch ( ArgumentNullException x ) {
                StdErrFlow.writeLine( "[5] ArgumentNullException: " + x.Message );
                this.setCurrentStateToUpdateLabelText( "ArgumentNullException while awaiting on Task" );
                return;
                }
            catch ( ObjectDisposedException x ) {
                StdErrFlow.writeLine( "[5] ObjectDisposedException: " + x.Message );
                this.setCurrentStateToUpdateLabelText( "ObjectDisposedException while awaiting on Task" );
                return;
                }
            catch ( ArgumentOutOfRangeException x ) {
                StdErrFlow.writeLine( "[5] ArgumentOutOfRangeException: " + x.Message );
                this.setCurrentStateToUpdateLabelText( "ArgumentOutOfRangeException while awaiting on Task" );
                return;
                }
            catch ( Exception x ) {
                StdErrFlow.writeLine( "[5] Exception: " + x.Message );
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
                StdErrFlow.writeLine("[1] ArgumentNullException: " + x.Message);
                MessageBox.Show(this, "The 'address' parameter is null.", "Exception during URL probing");
                MessageBox.Show(this, x.Message, "Exception during URL probing");
                exceptionType = "ArgumentNullException";
                }
            catch ( WebException x ) {
                StdErrFlow.writeLine("[1] WebException: " + x.Message);
                MessageBox.Show(this, "The formed URI is invalid or an error occured while downloading the resource.", "Exception during URL probing");
                MessageBox.Show(this, x.Message, "Exception during URL probing");
                exceptionType = "WebException";
                }
            catch ( NotSupportedException x ) {
                StdErrFlow.writeLine("[1] NotSupportedException: " + x.Message);
                MessageBox.Show(this, "The method has been called simultaneously on multiple threads.", "Exception during URL probing");
                MessageBox.Show(this, x.Message, "Exception during URL probing");
                exceptionType = "NotSupportedException";
                }
            catch ( Exception x ) {
                StdErrFlow.writeLine("[1] Exception: " + x.Message);
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

            if ( SiteCrawler.isAbsoluteURL( websiteURL ) == false ) {
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
            try {
                this.Invoke( ( MethodInvoker ) delegate {
                    this.foundedLinksCounter++;
                    this.foundedLinksToUpdateLabel.Text = this.foundedLinksCounter.ToString();
                    this.foundedLinksToUpdateLabel.Refresh();
                    });
                }
            catch ( ObjectDisposedException x ) {
                StdErrFlow.writeLine( "[7] ObjectDisposedException: " + x.Message );
                }
            catch ( InvalidOperationException x ) {
                StdErrFlow.writeLine( "[7] InvalidOperationException: " + x.Message );
                }
            catch ( Exception x ) {
                StdErrFlow.writeLine( "[7] Exception: " + x.Message );
                }
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
            bool isRootDirectoryCreated = FileSystemFlow.validateDirectoryCreation( crawlingRootDirectory );

            if ( isRootDirectoryCreated == false ) {
                return;
                }

            int firstFullStopIndex = this.websiteURLTextBox.Text.IndexOf( '.' );
            string mainPageName = this.websiteURLTextBox.Text.Substring( firstFullStopIndex + 1 );
            string mainPageDirectory = Path.Combine( crawlingRootDirectory, mainPageName );
            mainPageDirectory = FileSystemFlow.removeWindowsFileSystemReservedCharacters( mainPageDirectory );
            FileSystemFlow.createDirectory( mainPageDirectory );
            bool isMainPageDirectoryCreated = FileSystemFlow.validateDirectoryCreation( mainPageDirectory );

            if ( isMainPageDirectoryCreated == false ) {
                return;
                }

            FileSystemFlow.saveTextToFile( mainPageDirectory, mainPageName, websiteContent );

            // Search for the links on the main site.
            ISet<string> hrefLinks0 = this.getOnlyTheLinks( websiteContent );
            ISet<string> absoluteLinks0 = this.retrieveAbsoluteLinks( hrefLinks0 );

            // LEVEL 1
            this.setCurrentStateToUpdateLabelText( "Working... Evaluating Level 1" );
            string [] contentOfAbsoluteLinks0;
            ISet<string>[] absoluteLinks1 = this.grabAbsoluteLinksFromContentOf( absoluteLinks0, out contentOfAbsoluteLinks0 );
            string level0LinksPath = Path.Combine( mainPageDirectory, "lvl_0" );
            FileSystemFlow.createDirectory( level0LinksPath );
            bool isLevel0DirectoryCreated = FileSystemFlow.validateDirectoryCreation( level0LinksPath );

            if ( isLevel0DirectoryCreated == false ) {
                return;
                }

            this.setCurrentStateToUpdateLabelText( "Working... Saving Level 0" );

            for ( uint i=0; i<contentOfAbsoluteLinks0.Length; i++ ) {
                string currentFileName = i.ToString();
                FileSystemFlow.saveTextToFile( level0LinksPath, currentFileName, contentOfAbsoluteLinks0[i] );
                }

            // LEVEL 2
            if ( levelOfDepth < 2 ) {
                return;
                }

            this.setCurrentStateToUpdateLabelText( "Working... Evaluating Level 2" );
            ISet<string>[][] absoluteLinks2 = new ISet<string>[ absoluteLinks1.Rank ][];
            int [] absoluteLinks2ArrayDimension2Lengths = new int [ absoluteLinks1.Rank ];
            string level1LinksPath = Path.Combine( mainPageDirectory, "lvl_1" );
            FileSystemFlow.createDirectory( level1LinksPath );
            bool isLevel1DirectoryCreated = FileSystemFlow.validateDirectoryCreation( level1LinksPath );

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

                for ( uint j=0; j<contentOfAbsoluteLinks1.Length; j++ ) {
                    string currentFileName = i + "-" + j;
                    FileSystemFlow.saveTextToFile( level1LinksPath, currentFileName, contentOfAbsoluteLinks1[j] );
                    }
                }

            // LEVEL 3
            if ( levelOfDepth < 3 ) {
                return;
                }

            this.setCurrentStateToUpdateLabelText( "Working... Evaluating Level 3" );
            ISet<string>[][][] absoluteLinks3 = new ISet<string>[ absoluteLinks2.Rank ][][];
            string level2LinksPath = Path.Combine( mainPageDirectory, "lvl_2" );
            FileSystemFlow.createDirectory( level2LinksPath );
            bool isLevel2DirectoryCreated = FileSystemFlow.validateDirectoryCreation( level2LinksPath );

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

                    for ( uint k=0; k<contentOfAbsoluteLinks2.Length; k++ ) {
                        string currentFileName = i + "-" + j + "-" + k;
                        FileSystemFlow.saveTextToFile( level2LinksPath, currentFileName, contentOfAbsoluteLinks2[k] );
                        }
                    }
                }

            // Only websites content saving is revelant here. Retrieving the absolute links is obsolete and consume CPU and memory.
            string level3LinksPath = Path.Combine( mainPageDirectory, "lvl_3" );
            FileSystemFlow.createDirectory( level3LinksPath );
            bool isLevel3DirectoryCreated = FileSystemFlow.validateDirectoryCreation( level3LinksPath );

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

                        for ( uint l=0; l<contentOfCurrentAbsoluteLinks.Length; l++ ) {
                            this.setCurrentStateToUpdateLabelText( "Working... Saving Level 3 (IV)... " + l );
                            string currentFileName = iSet + "-" + jSubset + "-" + kCollection + "-" + l;
                            FileSystemFlow.saveTextToFile( level3LinksPath, currentFileName, contentOfCurrentAbsoluteLinks[l] );
                            }
                        }
                    }
                }
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
                StdErrFlow.writeLine( "[6] ArgumentNullException: " + x.Message + "Critical error" );
                MessageBox.Show( null, "ArgumentNullException while working on threads. Returning all control paths.", "Critical error" );
                Application.Exit();
                }
            catch ( ThreadStateException x ) {
                StdErrFlow.writeLine( "[6] ThreadStateException: " + x.Message + "Critical error" );
                MessageBox.Show( null, "ThreadStateException while working on threads. Returning all control paths.", "Critical error" );
                Application.Exit();
                }
            catch ( OutOfMemoryException x ) {
                StdErrFlow.writeLine( "[6] OutOfMemoryException: " + x.Message + "Critical error" );
                MessageBox.Show( null, "OutOfMemoryException while working on threads. Returning all control paths.", "Critical error" );
                Application.Exit();
                }
            catch ( ThreadInterruptedException x ) {
                StdErrFlow.writeLine( "[6] ThreadInterruptedException: " + x.Message + "Critical error" );
                MessageBox.Show( null, "ThreadInterruptedException while working on threads. Returning all control paths.", "Critical error" );
                Application.Exit();
                }
            catch ( Exception x ) {
                StdErrFlow.writeLine( "[6] Exception: " + x.Message + "Critical error" );
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
                        StdErrFlow.writeLine("[2] ArgumentNullException: " + x.Message + " urlEntry=" + urlEntry);
                        continue;
                        }
                    catch ( WebException x ) {
                        StdErrFlow.writeLine("[2] WebException: " + x.Message + " urlEntry=" + urlEntry);
                        continue;
                        }
                    catch ( NotSupportedException x ) {
                        StdErrFlow.writeLine("[2] NotSupportedException: " + x.Message + " urlEntry=" + urlEntry);
                        continue;
                        }
                    catch ( Exception x ) {
                        StdErrFlow.writeLine("[2] Exception: " + x.Message + " urlEntry=" + urlEntry);
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
                if ((SiteCrawler.isAbsoluteURL( entry ) == true) && (absoluteLinks.Contains( entry ) == false)) {
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
            string usedFramework = typeof( string ).Assembly.ImageRuntimeVersion;
            bool x64Process = Environment.Is64BitProcess;
            int currentManagedThreadID = Environment.CurrentManagedThreadId;

            string msgBoxText = "A simple web crawler with three levels of searching depth." + newLine +
                "It establishes only the HTTP connections and looking for only the absolute links (URIs)." + newLine +
                "The downloaded sites content will be saved to the directory of application execution." + newLine +
                newLine +
                "----------------------------------------------------------------------------" + newLine +
                newLine +
                "IMPORTANT INFORMATION ABOUT PROGRAM WORKFLOW!" + newLine +
                newLine +
                "Level 0 means the main page, its links and its content." + newLine +
                "Level 1 means searching for links in the content of the Level 0." + newLine +
                "Level 2 means grabbing links from the Level 1 results." + newLine +
                "Level 3 means processing links from the Level 2 output." + newLine +
                newLine +
                "Level 1: main page and links from it will be saved (lvl_0)" + newLine +
                "Level 2: links from the Level 1 will be saved (lvl_1)" + newLine +
                "Level 3: links from the Level 2 and Level 3 will be saved (lvl_2, lvl_3)" + newLine +
                newLine +
                "Be aware of the Level 2 working effects peculiarity!" + newLine +
                "Also, please note that only the distinct websites per level will be saved." + newLine +
                newLine +
                "----------------------------------------------------------------------------" + newLine +
                newLine +
                "StdErr stream redirected: " + StdErrFlow.getLastRedirectionState() + newLine +
                "StdErr file name: " + StdErrFlow.STDERR_FILENAME + newLine +
                "Crawler root directory name: " + FileSystemFlow.ROOT_DIRECTORY_NAME + newLine +
                newLine +
                "----------------------------------------------------------------------------" + newLine +
                newLine +
                "Application path: " + FileSystemFlow.getApplicationFullPath() + newLine +
                ".NET Framework: " + usedFramework + newLine +
                "64-bit process: " + x64Process + newLine +
                "Current managed thread ID: " + currentManagedThreadID + newLine +
                newLine;

            CustomMessageBox.CustomMessageBox.ShowBox( msgBoxText, "Information" );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Sets the name corresponding component text by a passed argument value and refreshes the GUI main window.
        /// An exception handling is provided, but obsolete if Application.Exit() will not be used during threads-based processing.
        /// </summary>
        /// <param name="labelText">Text of the name corresponding label to set. Use of 'null' here is discouraged.</param>

        private void setCurrentStateToUpdateLabelText( string labelText )
            {
            try {
                this.Invoke( ( MethodInvoker ) delegate {
                    this.currentStateToUpdateLabel.Text = labelText;
                    this.currentStateToUpdateLabel.Refresh();
                    });
                }
            catch ( ObjectDisposedException x ) {
                StdErrFlow.writeLine( "[8] ObjectDisposedException: " + x.Message );
                }
            catch ( InvalidOperationException x ) {
                StdErrFlow.writeLine( "[8] InvalidOperationException: " + x.Message );
                }
            catch ( Exception x ) {
                StdErrFlow.writeLine( "[8] Exception: " + x.Message );
                }
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Creates the root directory for the crawled web pages using a directory name defined in a constant ROOT_DIRECTORY_NAME.
        /// </summary>
        /// <returns>A path of the crawling root directory on successful directory creation, 'null' otherwise</returns>

        private string createCrawlingRootDirectory()
            {
            string path = null;
            string rootDirectoryName = Path.GetDirectoryName( FileSystemFlow.getApplicationFullPath() );
            string crawlingRootDirectory = Path.Combine( rootDirectoryName, FileSystemFlow.ROOT_DIRECTORY_NAME );
            bool isDirectoryCreated = FileSystemFlow.createDirectory( crawlingRootDirectory );

            if ( isDirectoryCreated == true ) {
                path = crawlingRootDirectory;
                }

            return ( crawlingRootDirectory );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Sets a text to the corresponding name component and refreshes it. This procedure will always be working on the UI thread.
        /// An exception handling is provided for this.
        /// </summary>
        /// <param name="text">A text to assign to the mentioned label.</param>

        private void setCounterToUpdateLabelText( string text )
            {
            try {
                this.Invoke( ( MethodInvoker ) delegate {
                    this.counterToUpdateLabel.Text = text;
                    this.counterToUpdateLabel.Refresh();
                    });
                }
            catch ( ObjectDisposedException x ) {
                StdErrFlow.writeLine( "[11] ObjectDisposedException: " + x.Message );
                }
            catch ( InvalidOperationException x ) {
                StdErrFlow.writeLine( "[11] InvalidOperationException: " + x.Message );
                }
            catch ( Exception x ) {
                StdErrFlow.writeLine( "[11] Exception: " + x.Message );
                }
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Defines the internal timer function body and its correlated thread function body.
        /// Use this procedure to define a counter working on the defined UI component 'counterToUpdateLabel'.
        /// </summary>

        private void defineThreadWorkingUICounter()
            {
            this.timerThread = new Thread( () => {
                System.Timers.Timer timer = new System.Timers.Timer();
                ushort numberOfSeconds = 0;
                ushort numberOfMinutes = 0;
                ushort numberOfHours = 0;
                ulong numberOfDays = 0;

                timer.Elapsed += ( object sender, System.Timers.ElapsedEventArgs e ) => {
                    numberOfSeconds++;

                    if ( numberOfSeconds > 59 ) {
                        numberOfSeconds = 0;
                        numberOfMinutes++;
                        }

                    if ( numberOfMinutes > 59 ) {
                        numberOfMinutes = 0;
                        numberOfHours++;
                        }

                    if ( numberOfHours > 23 ) {
                        numberOfHours = 0;
                        numberOfDays++;
                        }

                    string numberOfHoursText = numberOfHours.ToString("00");
                    string numberOfMinutesText = numberOfMinutes.ToString("00");
                    string numberOfSecondsText = numberOfSeconds.ToString("00");
                    string numberOfDaysText = numberOfDays.ToString();
                    string counterText = numberOfDaysText + ":" + numberOfHoursText + ":" + numberOfMinutesText + ":" + numberOfSecondsText;
                    this.setCounterToUpdateLabelText( counterText );
                    };

                timer.Interval = 1000;
                timer.Enabled = true;
                timer.Start();

                while ( this.Disposing == false ) {
                    // This loop may consume most of the CPU time.
                    try {
                        Thread.Sleep( 900 );
                        }
                    catch ( ArgumentOutOfRangeException x ) {
                        StdErrFlow.writeLine( "[13] ArgumentOutOfRangeException: " + x.Message );
                        }
                    catch ( Exception x ) {
                        StdErrFlow.writeLine( "[13] Exception: " + x.Message );
                        }
                    }

                timer.Enabled = false;
                timer.Stop();
                });
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Shows a Yes/No optioned message box that lets the user to choose whether to start the timer or not.
        /// On negative 'counterToUpdateLabel' component will be hidden.
        /// An exception handling is provided.
        /// </summary>

        private void notifyUserToStartTimerOrNot()
            {
            try { 
                string message = "This application may executes a timer-threaded loop with the UI corresponding label refreshing." +
                    " Because of nature of this solution, that may cause a consuming most of the CPU time." +
                    " Do you want to start the timer?";
                DialogResult dialogChoise = MessageBox.Show( message, "Starting a timer", MessageBoxButtons.YesNo );

                if ( dialogChoise == DialogResult.Yes ) {
                    this.defineThreadWorkingUICounter();
                    this.timerThread.Start();
                    }
                else {
                    this.counterToUpdateLabel.Visible = false;
                    }
                }
            catch ( ThreadStateException x ) {
                StdErrFlow.writeLine( "[12] ThreadStateException: " + x.Message );
                MessageBox.Show( this, "ThreadStateException has been raised while starting a timer thread", "Starting a timer" );
                }
            catch ( OutOfMemoryException x ) {
                StdErrFlow.writeLine( "[12] OutOfMemoryException: " + x.Message );
                MessageBox.Show( this, "OutOfMemoryException has been raised while starting a timer thread", "Starting a timer" );
                }
            catch ( InvalidOperationException x ) {
                StdErrFlow.writeLine( "[12] InvalidOperationException: " + x.Message );
                MessageBox.Show( this, "InvalidOperationException has been raised while starting a timer thread", "Starting a timer" );
                }
            catch ( System.ComponentModel.InvalidEnumArgumentException x ) {
                StdErrFlow.writeLine( "[12] InvalidEnumArgumentException: " + x.Message );
                MessageBox.Show( this, "InvalidEnumArgumentException has been raised while starting a timer thread", "Starting a timer" );
                }
            catch ( Exception x ) {
                StdErrFlow.writeLine( "[12] Exception: " + x.Message );
                MessageBox.Show( this, "Exception has been raised while starting a timer thread", "Starting a timer" );
                }
            }

        //______________________________________________________________________________________________________________________________

        }
    }
