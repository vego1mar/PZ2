using System;
using System.Net;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;

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
                appName = appName.Substring(appName.LastIndexOf('\\') + 1);
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

        private void proceedButton_Click( object sender, EventArgs e )
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
            this.crawlThroughTheSite( websiteContent, levelOfDepth );
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
        /// </summary>

        private void foundedLinksCount()
            {
            this.foundedLinksCounter++;
            this.foundedLinksToUpdateLabel.Text = this.foundedLinksCounter.ToString();
            this.foundedLinksToUpdateLabel.Refresh();
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
            this.setCurrentStateToUpdateLabelText("Working... Evaluating Level 0");

            // Search for the links on the main site.
            ISet<string> hrefLinks0 = this.getOnlyTheLinks( websiteContent );
            ISet<string> absoluteLinks0 = this.retrieveAbsoluteLinks( hrefLinks0 );

            // LEVEL 1
            this.setCurrentStateToUpdateLabelText( "Working... Evaluating Level 1" );
            ISet<string>[] absoluteLinks1 = this.grabAbsoluteLinksFromContentOf( absoluteLinks0 );

            // LEVEL 2
            if ( levelOfDepth < 2 ) {
                return;
                }

            this.setCurrentStateToUpdateLabelText( "Working... Evaluating Level 2" );
            ISet<string>[][] absoluteLinks2 = new ISet<string>[ absoluteLinks1.Rank ][];
            int [] absoluteLinks2ArrayDimension2Lengths = new int [ absoluteLinks1.Rank ];

            for ( int i=0; i<absoluteLinks1.Rank; i++ ) {
                ISet<string>[] currentAbsoluteLinks = this.grabAbsoluteLinksFromContentOf( absoluteLinks1[i] );
                absoluteLinks2[i] = currentAbsoluteLinks;
                absoluteLinks2ArrayDimension2Lengths[i] += currentAbsoluteLinks.Length;
                // TODO - check this addition for general correctness.
                }

            // LEVEL 3
            if ( levelOfDepth < 3 ) {
                return;
                }

            this.setCurrentStateToUpdateLabelText( "Working... Evaluating Level 3" );
            ISet<string>[][][] absoluteLinks3 = new ISet<string>[ absoluteLinks2.Rank ][][];

            for ( int i=0; i<absoluteLinks2.Rank; i++ ) {
                for ( int j=0; j<absoluteLinks2ArrayDimension2Lengths[i]; j++ ) {
                    ISet<string>[] currentAbsoluteLinks = this.grabAbsoluteLinksFromContentOf( absoluteLinks2[i][j] );
                    absoluteLinks3[i][j] = currentAbsoluteLinks;
                    // TODO - fix NullReferenceException here (dimensions)
                    }
                }

            // TODO - saving websites content into a file
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Downloads every HTML page from the passed set of URLs and traverses its content for retrieving the absolute links.
        /// A non-blocking traversing operation with exceptions handling implementation is provided.
        /// </summary>
        /// <param name="absoluteLinks">A collection of absolute links to be probed by a web connection and downloaded.</param>
        /// <returns>A collection array of absolute links retrieved from the URLs given as a parameter.</returns>

        private ISet<string>[] grabAbsoluteLinksFromContentOf( ISet<string> absoluteLinks )
            {
            WebClient connection = new WebClient();
            string[] sitesContent = new string[ absoluteLinks.Count ];
            ISet<string>[] nextAbsoluteLinks = new HashSet<string>[ absoluteLinks.Count ];
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
                        Console.Error.WriteLineAsync("[2] ArgumentNullException: " + x.Message + " urlEntry=" + urlEntry);
                        continue;
                        }
                    catch ( WebException x ) {
                        Console.Error.WriteLineAsync("[2] WebException: " + x.Message + " urlEntry=" + urlEntry);
                        continue;
                        }
                    catch ( NotSupportedException x ) {
                        Console.Error.WriteLineAsync("[2] NotSupportedException: " + x.Message + " urlEntry=" + urlEntry);
                        continue;
                        }
                    catch ( Exception x ) {
                        Console.Error.WriteLineAsync("[2] Exception: " + x.Message + " urlEntry=" + urlEntry);
                        continue;
                        }

                    sitesContent[i] = currentSiteContent;
                    i++;
                    }
                }

            i = 0;

            // Traverse the site's downloaded content for retrieving the absolute links.
            foreach ( var content in sitesContent ) {
                ISet<string> currentHrefLinks = this.getOnlyTheLinks( content );
                ISet<string> currentAbsoluteLinks = this.retrieveAbsoluteLinks( currentHrefLinks );
                nextAbsoluteLinks[i++] = currentAbsoluteLinks;
                }

            return ( nextAbsoluteLinks );
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
            string appPath = typeof( WebCrawler.Program ).Assembly.Location;
            string usedFramework = typeof( string ).Assembly.ImageRuntimeVersion;
            string osInfo = Environment.OSVersion.VersionString;
            bool x64Process = Environment.Is64BitProcess;
            int currentManagedThreadID = Environment.CurrentManagedThreadId;

            string msgBoxText = "A simple web crawler with three levels of searching depth." + newLine +
                "It establishes only the HTTP connections and looking for only the absolute links (URIs)." + newLine +
                newLine +
                "StdErr stream redirected: " + isStdErrRedirectedSuccessfully + newLine +
                "StdErr file name: " + STDERR_FILENAME + newLine +
                newLine +
                "Application path: " + appPath + newLine +
                ".NET Framework: " + usedFramework + newLine +
                "Operating system: " + osInfo + newLine +
                "64-bit process: " + x64Process + newLine +
                "Current managed thread ID: " + currentManagedThreadID + newLine;

            MessageBox.Show( this, msgBoxText, "Information" );
            // TODO - custom message box
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Sets the name corresponding component text by a passed argument value and refreshes the GUI main window.
        /// </summary>
        /// <param name="labelText">Text of the name corresponding label to set. Use of 'null' here is discouraged.</param>

        private void setCurrentStateToUpdateLabelText( string labelText )
            {
            this.currentStateToUpdateLabel.Text = labelText;
            this.currentStateToUpdateLabel.Refresh();
            }

        //______________________________________________________________________________________________________________________________

        }
    }
