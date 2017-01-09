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

        private ulong foundedLinksNumber;
        private StdErrFlow.ExceptionInfo lastExceptionInfo;

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// The default constructor. Initializes the fields.
        /// </summary>

        public MainWindow()
            {
            InitializeComponent();

            this.timerThread = null;
            this.timerDeadFlag = true;
            this.foundedLinksNumber = 0;
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
        /// An action performed when the 'Proceed' button is clicked.
        /// </summary>
        /// <param name="sender">The GUI component that cause the action.</param>
        /// <param name="e">Arguments of the action related with the GUI sender component.</param>

        private async void proceedButton_Click( object sender, EventArgs e )
            {
            this.foundedLinksNumber = 0;
            this.foundedLinksToUpdateLabel.Text = "0";
            this.disableMainWindowControls();
            this.setCurrentStateToUpdateLabelText( "Pending" );

            if ( SiteCrawler.isAbsoluteURL( this.websiteURLTextBox.Text ) == false ) {
                this.setCurrentStateToUpdateLabelText( "Unvalidated URL" );
                MessageBox.Show( this, "The typed data is not a proper absolute URL.", "URL scheme validation" );
                this.enableMainWindowControls();
                return;
                }

            SiteCrawler site = new SiteCrawler();
            site.setSiteURL( this.websiteURLTextBox.Text );
            site.probeNetworkConnection();
            string websiteContent = site.getProbedSiteContent();

            if ( websiteContent == string.Empty ) {
                StdErrFlow.ExceptionInfo exception = site.getLastExceptionInfo();
                this.setCurrentStateToUpdateLabelText( exception.typeName + " caused by the typed URL" );
                this.enableMainWindowControls();
                return;
                }

            this.setCurrentStateToUpdateLabelText( "Working" );

            // Do not encapsulate this block due to an UI synchronization problem with refreshing.
            try {
                await Task.Factory.StartNew( () => {
                    SiteCrawler crawler = new SiteCrawler();
                    crawler.setLevelOfDepth( this.gainLevelOfDepthSpinnerValue() );
                    crawler.setSiteURL( this.websiteURLTextBox.Text );
                    crawler.setAsynchronousDownloadUse( this.asynchronousWebsitesDownloadCheckBox.Checked );
                    crawler.crawlThroughSite();
                    },
                    TaskCreationOptions.LongRunning
                    );
                }
            catch ( ArgumentNullException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = this.websiteURLTextBox.Text.ToString();
                this.lastExceptionInfo.causeEvent = lastExceptionInfo.typeName + " was thrown while awaiting on a Task.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-1]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                this.setCurrentStateToUpdateLabelText( lastExceptionInfo.causeEvent );
                return;
                }
            catch ( ObjectDisposedException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = this.websiteURLTextBox.Text.ToString();
                this.lastExceptionInfo.causeEvent = lastExceptionInfo.typeName + " was thrown while awaiting on a Task.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-1]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                this.setCurrentStateToUpdateLabelText( lastExceptionInfo.causeEvent );
                return;
                }
            catch ( ArgumentOutOfRangeException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = this.websiteURLTextBox.Text.ToString();
                this.lastExceptionInfo.causeEvent = lastExceptionInfo.typeName + " was thrown while awaiting on a Task.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-1]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                this.setCurrentStateToUpdateLabelText( lastExceptionInfo.causeEvent );
                return;
                }
            catch ( Exception x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = this.websiteURLTextBox.Text.ToString();
                this.lastExceptionInfo.causeEvent = lastExceptionInfo.typeName + " was thrown while awaiting on a Task.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-1]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                this.setCurrentStateToUpdateLabelText( lastExceptionInfo.causeEvent );
                return;
                }

            this.setCurrentStateToUpdateLabelText( "Done" );
            this.enableMainWindowControls();
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Disable controls on the main window after 'Proceed' button click.
        /// </summary>

        private void disableMainWindowControls()
            {
            this.proceedButton.Enabled = false;
            this.websiteURLTextBox.Enabled = false;
            this.levelOfDepthSpinner.Enabled = false;
            this.asynchronousWebsitesDownloadCheckBox.Enabled = false;
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Enable controls on the main window when the 'Proceed' button operation had finished.
        /// </summary>

        private void enableMainWindowControls()
            {
            this.proceedButton.Enabled = true;
            this.websiteURLTextBox.Enabled = true;
            this.levelOfDepthSpinner.Enabled = true;
            this.asynchronousWebsitesDownloadCheckBox.Enabled = true;
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Retrieves the current value of the numeric up-down 'level of depth' component.
        /// </summary>
        /// <returns>0 if exception has been raised, a value of range [Minimum; Maximum] from the properties otherwise.</returns>

        private uint gainLevelOfDepthSpinnerValue()
            {
            uint currentValue = 0;

            try {
                currentValue = Convert.ToUInt32( this.levelOfDepthSpinner.Value );
                }
            catch ( ArgumentOutOfRangeException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = this.levelOfDepthSpinner.Value.ToString();
                this.lastExceptionInfo.causeEvent = "Retrieving the value from the NumericUpDown component.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-2]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( OverflowException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = this.levelOfDepthSpinner.Value.ToString();
                this.lastExceptionInfo.causeEvent = "Retrieving the value from the NumericUpDown component.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-2]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( Exception x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = this.levelOfDepthSpinner.Value.ToString();
                this.lastExceptionInfo.causeEvent = "Retrieving the value from the NumericUpDown component.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-2]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }

            return ( currentValue );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Updates the 'foundedLinksNumber' field by adding to it the parameter and refreshing the 'foundedLinksToUpdateLabel' UI component.
        /// </summary>
        /// <param name="numberOfLinks">A number to add to the 'foundedLinksNumber' field.</param>

        private void foundedLinksNumberUpdate( uint numberOfLinks )
            {
            try {
                this.BeginInvoke( ( MethodInvoker ) delegate {
                    this.foundedLinksNumber += numberOfLinks;
                    this.foundedLinksToUpdateLabel.Text = this.foundedLinksNumber.ToString();
                    this.foundedLinksToUpdateLabel.Refresh();
                    });
                }
            catch ( ObjectDisposedException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = "foundedLinksNumber";
                this.lastExceptionInfo.causeEvent = "Refreshing the UI label asynchronously.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-3]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( InvalidOperationException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = "foundedLinksNumber";
                this.lastExceptionInfo.causeEvent = "Refreshing the UI label asynchronously.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-3]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( Exception x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = "foundedLinksNumber";
                this.lastExceptionInfo.causeEvent = "Refreshing the UI label asynchronously.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-3]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// An action performed when the 'Info' button is clicked.
        /// </summary>
        /// <param name="sender">The GUI component that cause the action.</param>
        /// <param name="e">Arguments of the action related with the GUI sender component.</param>

        private void InfoButton_Click( object sender, EventArgs e )
            {
            // TASK: change the info
            // TASK: foundedLinksNumber functionality (event on the SiteCrawler side)
            // TASK: see errors log UI functionality
            // TASK: subdirectory with the main site name

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
        /// Set a text to the 'currentStateToUpdateLabel' UI component and refreshes it. This procedure will always work on the UI thread.
        /// </summary>
        /// <param name="labelText">A text to assign to the UI label. Use of 'null' here is discouraged.</param>

        private void setCurrentStateToUpdateLabelText( string labelText )
            {
            try {
                this.BeginInvoke( ( MethodInvoker ) delegate {
                    this.currentStateToUpdateLabel.Text = labelText;
                    this.currentStateToUpdateLabel.Refresh();
                    });
                }
            catch ( ObjectDisposedException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = labelText.ToString();
                this.lastExceptionInfo.causeEvent = "Refreshing the UI label asynchronously.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-4]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( InvalidOperationException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = labelText.ToString();
                this.lastExceptionInfo.causeEvent = "Refreshing the UI label asynchronously.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-4]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( Exception x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = labelText.ToString();
                this.lastExceptionInfo.causeEvent = "Refreshing the UI label asynchronously.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-4]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Set a text to the 'counterToUpdateLabel' UI component and refreshes it. This procedure will always work on the UI thread.
        /// </summary>
        /// <param name="text">A text to assign to the UI label.</param>

        private void setCounterToUpdateLabelText( string text )
            {
            try {
                this.BeginInvoke( ( MethodInvoker ) delegate {
                    this.counterToUpdateLabel.Text = text;
                    this.counterToUpdateLabel.Refresh();
                    });
                }
            catch ( ObjectDisposedException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = text.ToString();
                this.lastExceptionInfo.causeEvent = "Refreshing the UI label asynchronously.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-5]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( InvalidOperationException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = text.ToString();
                this.lastExceptionInfo.causeEvent = "Refreshing the UI label asynchronously.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-5]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( Exception x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = text.ToString();
                this.lastExceptionInfo.causeEvent = "Refreshing the UI label asynchronously.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-5]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Creates a new thread for the specified field and defines its function body. 
        /// This UI timer works with the 'counterToUpdateLabel' component.
        /// </summary>

        private void defineUICounterThread()
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
                    Thread.Sleep( 900 );
                    }

                timer.Enabled = false;
                timer.Stop();
                });
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// An action performed when the 'Start' radio button is checked.
        /// </summary>
        /// <param name="sender">The GUI component that cause the action.</param>
        /// <param name="e">Arguments of the action related with the GUI sender component.</param>

        private void timerStartRadioButton_CheckedChanged( object sender, EventArgs e )
            {
            try {
                if ( this.timerStartRadioButton.Checked == true ) {
                    this.timerStopRadioButton.Checked = false;
                    this.timerStopRadioButton.Enabled = true;
                    this.timerStartRadioButton.Enabled = false;
                    this.timerDeadFlag = false;
                    this.defineUICounterThread();
                    this.timerThread.Start();
                    }
                }
            catch ( ArgumentNullException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = e.ToString();
                this.lastExceptionInfo.causeEvent = "Starting the UI timer.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-6]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( ArgumentOutOfRangeException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = e.ToString();
                this.lastExceptionInfo.causeEvent = "Starting the UI timer.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-6]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( ArgumentException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = e.ToString();
                this.lastExceptionInfo.causeEvent = "Starting the UI timer.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-6]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( ThreadStateException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = e.ToString();
                this.lastExceptionInfo.causeEvent = "Starting the UI timer.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-6]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( FormatException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = e.ToString();
                this.lastExceptionInfo.causeEvent = "Starting the UI timer.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-6]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( OutOfMemoryException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = e.ToString();
                this.lastExceptionInfo.causeEvent = "Starting the UI timer.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-6]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( ObjectDisposedException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = e.ToString();
                this.lastExceptionInfo.causeEvent = "Starting the UI timer.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-6]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( Exception x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = e.ToString();
                this.lastExceptionInfo.causeEvent = "Starting the UI timer.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-6]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// An action performed when the 'Stop' radio button is checked.
        /// </summary>
        /// <param name="sender">The GUI component that cause the action.</param>
        /// <param name="e">Arguments of the action related with the GUI sender component.</param>

        private void timerStopRadioButton_CheckedChanged( object sender, EventArgs e )
            {
            try {
                if ( this.timerStopRadioButton.Checked == true ) {
                    this.timerStartRadioButton.Checked = false;
                    this.timerStartRadioButton.Enabled = true;
                    this.timerStopRadioButton.Enabled = false;
                    this.timerDeadFlag = true;
                    this.timerThread = null;
                    }
                }
            catch ( ArgumentNullException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = e.ToString();
                this.lastExceptionInfo.causeEvent = "Stopping the UI timer.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-7]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( ArgumentOutOfRangeException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = e.ToString();
                this.lastExceptionInfo.causeEvent = "Stopping the UI timer.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-7]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( ArgumentException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = e.ToString();
                this.lastExceptionInfo.causeEvent = "Stopping the UI timer.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-7]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( ThreadStateException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = e.ToString();
                this.lastExceptionInfo.causeEvent = "Stopping the UI timer.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-7]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( FormatException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = e.ToString();
                this.lastExceptionInfo.causeEvent = "Stopping the UI timer.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-7]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( OutOfMemoryException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = e.ToString();
                this.lastExceptionInfo.causeEvent = "Stopping the UI timer.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-7]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( ObjectDisposedException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = e.ToString();
                this.lastExceptionInfo.causeEvent = "Stopping the UI timer.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-7]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            catch ( Exception x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argument = e.ToString();
                this.lastExceptionInfo.causeEvent = "Stopping the UI timer.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-7]";
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") arg=" + lastExceptionInfo.argument );
                StdErrFlow.writeLine( Environment.NewLine );
                }
            }

        //______________________________________________________________________________________________________________________________

        }
    }
