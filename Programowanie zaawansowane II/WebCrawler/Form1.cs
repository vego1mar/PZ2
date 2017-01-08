using System;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace WebCrawler
    {

    /// <summary>
    /// An UI design control for the application.
    /// </summary>

    public partial class MainWindow : Form
        {

        private Thread timerThread;
        private volatile bool timerDeadFlag;

        private ulong foundedLinksCounter;

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// The default constructor. Initializes the fields.
        /// </summary>

        public MainWindow()
            {
            InitializeComponent();

            this.timerThread = null;
            this.timerDeadFlag = false;
            this.foundedLinksCounter = 0;

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
            StdErrFlow.tryToRedirectStdErr();
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// An action performed when the main window is closed.
        /// </summary>
        /// <param name="sender">The GUI component that cause the action.</param>
        /// <param name="e">Provides data for the FormClosed event.</param>

        private void MainWindow_FormClosed( object sender, FormClosedEventArgs e )
            {
            this.timerDeadFlag = true;
            StdErrFlow.writeLine( Environment.NewLine );
            StdErrFlow.tryToRetrievePreviousStdErr();
            StdErrFlow.closeStreams();
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

            if ( SiteCrawler.isAbsoluteURL( this.websiteURLTextBox.Text ) == false ) {
                this.setCurrentStateToUpdateLabelText( "Unvalidated URL" );
                MessageBox.Show( this, "The URL scheme has not been found. Use absolute path of \"http://www.\"", "URL scheme validation" );
                this.enableMainWindowControls();
                return;
                }

            SiteCrawler site = new SiteCrawler();
            site.setSiteURL( this.websiteURLTextBox.Text );
            site.probeNetworkConnection();
            string websiteContent = site.getSiteContent();

            if ( websiteContent == string.Empty ) {
                StdErrFlow.ExceptionInfo x = site.getLastExceptionInfo();
                string currentStateLabelText = x.typeName + " on the typed URL";
                this.setCurrentStateToUpdateLabelText( currentStateLabelText );
                this.enableMainWindowControls();
                return;
                }

            uint levelOfDepth = this.retrieveTheStateOfSelectedRadioButtonLevel();
            this.setCurrentStateToUpdateLabelText( "Working" );

            // Do not encapsulate this block due to an UI synchronization problem with refreshing.
            try {
                await Task.Factory.StartNew( () => {
                    SiteCrawler crawler = new SiteCrawler();
                    crawler.setLevelOfDepth( levelOfDepth );
                    crawler.setSiteURL( this.websiteURLTextBox.Text );
                    crawler.crawlThroughSite();
                    },
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

                while ( this.timerDeadFlag == false ) {
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
