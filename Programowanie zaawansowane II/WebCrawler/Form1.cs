using System;
using System.Net;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// A web crawler of searching for the phrase with three levels of depth at most.
/// </summary>
namespace WebCrawler
    {
    public partial class MainWindow : Form
        {

        private uint secondsForProceedTimer;
        private uint minutesForProceedTimer;
        private uint foundedLinksCounter;
        private TextWriter stdErrStream;

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Invokes InitializeComponent() method that is required for Designer support method.
        /// </summary>
        public MainWindow()
            {
            InitializeComponent();

            // Touch the fields.
            this.secondsForProceedTimer = 0;
            this.minutesForProceedTimer = 0;
            this.foundedLinksCounter = 0;
            this.stdErrStream = null;
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// It tries to redirect standard error stream into a file "errlog.txt" (to append its content).
        /// This function is used every time when the main window is formed (loaded).
        /// </summary>
        /// <returns>true if function code has been traversed, false when any error occured</returns>

        private bool tryToRedirectStdErr()
            {
            try {
                // Create a new stream for the file.
                this.stdErrStream = new StreamWriter("errlog.txt", true);
                Console.SetError( this.stdErrStream );

                if ( this.stdErrStream == null ) {
                    return (false);
                    }

                // Write a header file.
                string appName = typeof( WebCrawler.Program ).Assembly.Location;
                appName = appName.Substring(appName.LastIndexOf('\\') + 1);
                Console.Error.WriteLine("=============================================");
                Console.Error.WriteLine("Error log for {0}", appName);
                Console.Error.WriteLine("Timestamp: {0}", DateTime.UtcNow);
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

        private void MainWindow_Load(object sender, EventArgs e)
            {
            this.tryToRedirectStdErr();
            this.proceedTimer.Enabled = true;
            this.proceedTimer.Start();
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

            this.proceedTimer.Enabled = false;
            this.proceedTimer.Stop();
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// An action performed when the 'Proceed' button is 'clicked'.
        /// </summary>
        /// <param name="sender">The GUI component that cause the action.</param>
        /// <param name="e">Arguments of the action related with the GUI sender component.</param>
        private void proceedButton_Click( object sender, EventArgs e )
            {
            // Preparing the fields.
            this.minutesForProceedTimer = 0;
            this.secondsForProceedTimer = 0;
            this.foundedLinksCounter = 0;
            
            // Change the 'Proceed' button to an unavailable one.
            //this.proceedTimer.Enabled = true;
            //this.proceedTimer.Start();
            this.proceedButton.Enabled = false;
            this.currentStateToUpdateLabel.Text = "Preparing";
            this.foundedLinksToUpdateLabel.Text = "0";

            // Get the website URL from the text box.
            string websiteURL = this.websiteURLTextBox.Text;

            // Validate website URL given from the user as HTTP or HTTPS URI.
            if ( this.isURLvalid( websiteURL ) == false ) {
                MessageBox.Show(this, "The URL scheme has not been found.\nUse absolute path of \"http://www.\"", "URL scheme validation");
                this.proceedButton.Enabled = true;
                this.currentStateToUpdateLabel.Text = "Returned";
                //this.proceedTimer.Stop();
                return;
                }

            // Probe the given URL by opening a connection.
            string content = null;

            try {
                WebClient website = new WebClient();
                content = website.DownloadString( websiteURL );
                }
            catch ( ArgumentNullException x ) {
                Console.Error.WriteLine("[1] ArgumentNullException: " + x.Message);
                MessageBox.Show(this, "The 'address' parameter is null.", "Exception during URL probing");
                MessageBox.Show(this, x.Message, "Exception during URL probing");
                this.proceedButton.Enabled = true;
                this.currentStateToUpdateLabel.Text = "Returned";
                //this.proceedTimer.Stop();
                return;
                }
            catch ( WebException x ) {
                Console.Error.WriteLine("[1] WebException: " + x.Message);
                MessageBox.Show(this, "The formed URI is invalid or an error occured while downloading the resource.", "Exception during URL probing");
                MessageBox.Show(this, x.Message, "Exception during URL probing");
                this.proceedButton.Enabled = true;
                this.currentStateToUpdateLabel.Text = "Returned";
                //this.proceedTimer.Stop();
                return;
                }
            catch ( NotSupportedException x ) {
                Console.Error.WriteLine("[1] NotSupportedException: " + x.Message);
                MessageBox.Show(this, "The method has been called simultaneously on multiple threads.", "Exception during URL probing");
                MessageBox.Show(this, x.Message, "Exception during URL probing");
                this.proceedButton.Enabled = true;
                this.currentStateToUpdateLabel.Text = "Returned";
                //this.proceedTimer.Stop();
                return;
                }

            // Retrieve the state of selected level.
            bool level1 = this.level1RadioButton.Checked;
            bool level2 = this.level2RadioButton.Checked;
            bool level3 = this.level3RadioButton.Checked;
            uint levelOfDepth = 0;
            levelOfDepth = (level1 == true) ? (1) : (levelOfDepth);
            levelOfDepth = (level2 == true) ? (2) : (levelOfDepth);
            levelOfDepth = (level3 == true) ? (3) : (levelOfDepth);

            // Retrieve the phrase from the user.
            string phrase = this.phraseToSearchForTextBox.Text;

            if ( phrase == null || phrase == "" ) {
                MessageBox.Show(this,"The given phrase to search for cannot be empty.","Phrase validation");
                this.proceedButton.Enabled = true;
                this.currentStateToUpdateLabel.Text = "Returned";
                //this.proceedTimer.Stop();
                return;
                }

            // Proceed with crawling.
            this.currentStateToUpdateLabel.Text = "Pending";
            this.crawlThroughTheSite( content, phrase, levelOfDepth );

            // Switch the 'Proceed' button to active again.
            this.proceedButton.Enabled = true;
            this.currentStateToUpdateLabel.Text = "Done";
            //this.proceedTimer.Enabled = false;
            //this.proceedTimer.Stop();
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

            return ( Uri.TryCreate(source, UriKind.Absolute, out uriResult) && 
                (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps) );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// An action performed when the timer event is raised after 1 second.
        /// </summary>
        /// <param name="sender">The GUI component that cause the action.</param>
        /// <param name="e">Arguments of the action related with the GUI sender component.</param>

        private void proceedTimer_Tick( object sender, EventArgs e )
            {
            // Check current seconds and minutes.
            if ( this.secondsForProceedTimer >= 59 ) { 
                this.minutesForProceedTimer++;
                this.secondsForProceedTimer = 0; 
                }
            else { 
                this.secondsForProceedTimer++; 
                }

            // Construct the 'time' string and update GUI.
            string timePulse = this.minutesForProceedTimer + " : " + this.secondsForProceedTimer;
            this.elapsedTimeToUpdateLabel.Text = timePulse;
            this.elapsedTimeToUpdateLabel.Refresh();
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// The given website crawling procedure. It searches for the occurencies of <a> tags.
        /// </summary>
        /// <param name="content">The HTML content of the main website to be proceeded as a start page (level 0).</param>
        /// <param name="phrase">A string that has been searched in the HTML web contents at all levels.</param>
        /// <param name="levelOfDepth">The level of depth for crawling. This should be a value from a set of {1,2,3}, 
        /// otherwise the default parameter of 1 will be assigned.</param>

        private void crawlThroughTheSite( string content, string phrase, uint levelOfDepth )
            {
            // Validate the levelOfDepth argument.
            if ( (levelOfDepth < 1) || (levelOfDepth > 3) ) {
                levelOfDepth = 1;
                }

            // LEVEL 0
            // Search for the links on the main site.
            ISet<string> hrefLinks0 = this.getOnlyTheLinks( content );
            ISet<string> absoluteLinks0 = this.retrieveAbsoluteLinks( hrefLinks0 );

            // LEVEL 1
            WebClient connection1 = new WebClient();
            string [] sitesContent1 = new string [ absoluteLinks0.Count ];
            ISet<string> [] absoluteLinks1 = new HashSet<string>[ absoluteLinks0.Count ];
            uint i = 0;

            // Download every page of absolute links founded.
            foreach ( var urlEntry in absoluteLinks0 ) {
                string currentSiteContent = connection1.DownloadString( urlEntry );
                sitesContent1[i++] = currentSiteContent;
                }

            i = 0;

            // Traverse the site's downloaded content for retrieving the absolute links.
            foreach ( var content1 in sitesContent1 ) {
                ISet<string> currentHrefLinks1 = this.getOnlyTheLinks( content1 );
                ISet<string> currentAbsoluteLinks1 = this.retrieveAbsoluteLinks( currentHrefLinks1 );
                absoluteLinks1[i++] = currentAbsoluteLinks1;
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

            foreach ( var match in regexLink.Matches( content ) ) {
                if ( newLinks.Contains( match.ToString() ) == false ) {
                    newLinks.Add( match.ToString() );
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

        }
    }
