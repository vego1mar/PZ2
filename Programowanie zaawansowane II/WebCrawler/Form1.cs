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
        private System.Globalization.NumberFormatInfo numberFormatInfo;

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

            try {
                this.numberFormatInfo = new System.Globalization.CultureInfo( "en-US", true ).NumberFormat;
                this.numberFormatInfo.NumberDecimalDigits = 0;
                }
            catch ( ArgumentNullException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = numberFormatInfo.GetType().FullName + "~" + nameof( numberFormatInfo );
                this.lastExceptionInfo.argValue = numberFormatInfo.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-8]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( ArgumentOutOfRangeException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = numberFormatInfo.GetType().FullName + "~" + nameof( numberFormatInfo );
                this.lastExceptionInfo.argValue = numberFormatInfo.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-8]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( System.Globalization.CultureNotFoundException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = numberFormatInfo.GetType().FullName + "~" + nameof( numberFormatInfo );
                this.lastExceptionInfo.argValue = numberFormatInfo.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-8]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( InvalidOperationException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = numberFormatInfo.GetType().FullName + "~" + nameof( numberFormatInfo );
                this.lastExceptionInfo.argValue = numberFormatInfo.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-8]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( Exception x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = numberFormatInfo.GetType().FullName + "~" + nameof( numberFormatInfo );
                this.lastExceptionInfo.argValue = numberFormatInfo.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-8]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
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
                MessageBox.Show( this, exception.message, "Establishing connection failed" );
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
                    int threadsPerDepthEntryValue = this.gainThreadsPerDepthEntryValue();

                    if ( threadsPerDepthEntryValue != 0 ) {
                        crawler.setMaximumRunningTasksPerDepthEntry( threadsPerDepthEntryValue );

                        crawler.NewSetOfLinksFounded += ( object senderObj, SiteCrawler.SiteCrawlerEventArgs eventArgs ) => {
                            this.foundedLinksNumber += uint.Parse( Math.Abs(eventArgs.geNumberOfFoundedLinks()).ToString() );
                            this.foundedLinksNumberUpdate();
                            };

                        crawler.crawlThroughSite();
                        }
                    },
                    TaskCreationOptions.LongRunning
                    );
                }
            catch ( ArgumentNullException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = this.websiteURLTextBox.GetType().FullName + "~" + nameof( this.websiteURLTextBox );
                this.lastExceptionInfo.argValue = this.websiteURLTextBox.Text.ToString();
                string cause = lastExceptionInfo.typeName + " was thrown while awaiting on a Task.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-1]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                this.setCurrentStateToUpdateLabelText( cause );
                MessageBox.Show( this, this.lastExceptionInfo.message, "Task execution abruption" );
                return;
                }
            catch ( ObjectDisposedException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = this.websiteURLTextBox.GetType().FullName + "~" + nameof( this.websiteURLTextBox );
                this.lastExceptionInfo.argValue = this.websiteURLTextBox.Text.ToString();
                string cause = lastExceptionInfo.typeName + " was thrown while awaiting on a Task.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-1]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                this.setCurrentStateToUpdateLabelText( cause );
                MessageBox.Show( this, this.lastExceptionInfo.message, "Task execution abruption" );
                return;
                }
            catch ( ArgumentOutOfRangeException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = this.websiteURLTextBox.GetType().FullName + "~" + nameof( this.websiteURLTextBox );
                this.lastExceptionInfo.argValue = this.websiteURLTextBox.Text.ToString();
                string cause = lastExceptionInfo.typeName + " was thrown while awaiting on a Task.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-1]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                this.setCurrentStateToUpdateLabelText( cause );
                MessageBox.Show( this, this.lastExceptionInfo.message, "Task execution abruption" );
                return;
                }
            catch ( Exception x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = this.websiteURLTextBox.GetType().FullName + "~" + nameof( this.websiteURLTextBox );
                this.lastExceptionInfo.argValue = this.websiteURLTextBox.Text.ToString();
                string cause = lastExceptionInfo.typeName + " was thrown while awaiting on a Task.";
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-1]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                this.setCurrentStateToUpdateLabelText( cause );
                MessageBox.Show( this, this.lastExceptionInfo.message, "Task execution abruption" );
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
            this.threadsPerDepthEntrySpinner.Enabled = false;
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
            this.threadsPerDepthEntrySpinner.Enabled = true;
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
                this.lastExceptionInfo.argName = this.levelOfDepthSpinner.GetType().Name + ".Value";
                this.lastExceptionInfo.argValue = this.levelOfDepthSpinner.Value.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-2]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( OverflowException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = this.levelOfDepthSpinner.GetType().Name + ".Value";
                this.lastExceptionInfo.argValue = this.levelOfDepthSpinner.Value.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-2]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( Exception x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = this.levelOfDepthSpinner.GetType().Name + ".Value";
                this.lastExceptionInfo.argValue = this.levelOfDepthSpinner.Value.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-2]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }

            return ( currentValue );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Updates the 'foundedLinksNumber' field by refreshing the 'foundedLinksToUpdateLabel' UI component with a current value.
        /// </summary>

        private void foundedLinksNumberUpdate()
            {
            try {
                this.BeginInvoke( ( MethodInvoker ) delegate {
                    this.foundedLinksToUpdateLabel.Text = this.foundedLinksNumber.ToString( "N", this.numberFormatInfo );
                    this.foundedLinksToUpdateLabel.Refresh();
                    });
                }
            catch ( ObjectDisposedException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = this.foundedLinksNumber.GetType().FullName + "~" + nameof( this.foundedLinksNumber );
                this.lastExceptionInfo.argValue = this.foundedLinksNumber.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-3]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( InvalidOperationException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = this.foundedLinksNumber.GetType().FullName + "~" + nameof( this.foundedLinksNumber );
                this.lastExceptionInfo.argValue = this.foundedLinksNumber.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-3]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( FormatException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = this.foundedLinksNumber.GetType().FullName + "~" + nameof( this.foundedLinksNumber );
                this.lastExceptionInfo.argValue = this.foundedLinksNumber.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-3]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( Exception x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = this.foundedLinksNumber.GetType().FullName + "~" + nameof( this.foundedLinksNumber );
                this.lastExceptionInfo.argValue = this.foundedLinksNumber.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-3]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
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
            string newLine = Environment.NewLine;
            string usedFramework = typeof( string ).Assembly.ImageRuntimeVersion;
            bool x64Process = Environment.Is64BitProcess;

            string msgBoxText = "A simple web crawler using a concurrent recursive model of work." + newLine +
                "It looking for only the absolute links (URIs)." + newLine +
                "The downloaded sites content will be saved to the directory of application execution." + newLine +
                newLine +
                "Only the distinct websites will be saved into files." + newLine +
                "Also, the website file will not be create if any exception has been raised, so the file must be empty." + newLine +
                newLine +
                "----------------------------------------------------------------------------" + newLine +
                newLine +
                "Level of depth" + newLine +
                "Specifies how many recursive entries level will be considered for crawling." + newLine +
                newLine +
                "Threads per depth entry" + newLine +
                "Specifies the maximum number of concurrently executed tasks per recursion entry each." + newLine +
                newLine +
                "Founded links" + newLine +
                "The number of absolute links founded at the deepest recursion entry point. " +
                "It does not mean the total amount of websites downloaded and saved." + newLine +
                newLine +
                "Asynchronous websites download" + newLine +
                "Check this option to indicate using an asynchronous version of downloading the websites. " +
                "Please note that it may cause an abnormal behavior at many errors occurence or threads creation." + newLine +
                newLine +
                "----------------------------------------------------------------------------" + newLine +
                newLine +
                "StdErr stream last redirection status: " + StdErrFlow.getLastRedirectionState() + newLine +
                "StdErr file name: " + StdErrFlow.STDERR_FILENAME + newLine +
                "Crawler root directory name: " + FileSystemFlow.ROOT_DIRECTORY_NAME + newLine +
                newLine +
                "----------------------------------------------------------------------------" + newLine +
                newLine +
                "Application location: " + FileSystemFlow.getApplicationFullPath() + newLine +
                ".NET Framework: " + usedFramework + newLine +
                "64-bit process: " + x64Process + newLine +
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
                this.lastExceptionInfo.argName = labelText.GetType().FullName + "~" + nameof( labelText );
                this.lastExceptionInfo.argValue = labelText.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-4]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( InvalidOperationException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = labelText.GetType().FullName + "~" + nameof( labelText );
                this.lastExceptionInfo.argValue = labelText.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-4]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( Exception x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = labelText.GetType().FullName + "~" + nameof( labelText );
                this.lastExceptionInfo.argValue = labelText.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-4]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
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
                this.lastExceptionInfo.argName = text.GetType().FullName + "~" + nameof( text );
                this.lastExceptionInfo.argValue = text.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-5]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( InvalidOperationException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = text.GetType().FullName + "~" + nameof( text );
                this.lastExceptionInfo.argValue = text.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-5]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( Exception x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = text.GetType().FullName + "~" + nameof( text );
                this.lastExceptionInfo.argValue = text.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-5]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
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
                    Thread.Sleep( 750 );
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
                this.lastExceptionInfo.argName = e.GetType().FullName + "~" + nameof( e );
                this.lastExceptionInfo.argValue = e.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-6]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( ArgumentOutOfRangeException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = e.GetType().FullName + "~" + nameof( e );
                this.lastExceptionInfo.argValue = e.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-6]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( ArgumentException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = e.GetType().FullName + "~" + nameof( e );
                this.lastExceptionInfo.argValue = e.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-6]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( ThreadStateException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = e.GetType().FullName + "~" + nameof( e );
                this.lastExceptionInfo.argValue = e.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-6]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( FormatException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = e.GetType().FullName + "~" + nameof( e );
                this.lastExceptionInfo.argValue = e.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-6]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( OutOfMemoryException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = e.GetType().FullName + "~" + nameof( e );
                this.lastExceptionInfo.argValue = e.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-6]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( ObjectDisposedException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = e.GetType().FullName + "~" + nameof( e );
                this.lastExceptionInfo.argValue = e.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-6]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( Exception x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = e.GetType().FullName + "~" + nameof( e );
                this.lastExceptionInfo.argValue = e.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-6]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
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
                this.lastExceptionInfo.argName = e.GetType().FullName + "~" + nameof( e );
                this.lastExceptionInfo.argValue = e.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-7]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( ArgumentOutOfRangeException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = e.GetType().FullName + "~" + nameof( e );
                this.lastExceptionInfo.argValue = e.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-7]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( ArgumentException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = e.GetType().FullName + "~" + nameof( e );
                this.lastExceptionInfo.argValue = e.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-7]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( ThreadStateException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = e.GetType().FullName + "~" + nameof( e );
                this.lastExceptionInfo.argValue = e.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-7]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( FormatException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = e.GetType().FullName + "~" + nameof( e );
                this.lastExceptionInfo.argValue = e.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-7]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( OutOfMemoryException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = e.GetType().FullName + "~" + nameof( e );
                this.lastExceptionInfo.argValue = e.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-7]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( ObjectDisposedException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = e.GetType().FullName + "~" + nameof( e );
                this.lastExceptionInfo.argValue = e.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-7]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( Exception x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = e.GetType().FullName + "~" + nameof( e );
                this.lastExceptionInfo.argValue = e.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-7]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Retrieves the current value of the numeric up-down 'threadsPerDepthEntrySpinner' component.
        /// </summary>
        /// <returns>0 if exception has been raised, a value of range [Minimum; Maximum] from the properties otherwise.</returns>

        private int gainThreadsPerDepthEntryValue()
            {
            int currentValue = 0;

            try {
                currentValue = Convert.ToInt32( this.threadsPerDepthEntrySpinner.Value );
                }
            catch ( ArgumentOutOfRangeException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = this.threadsPerDepthEntrySpinner.GetType().Name + ".Value";
                this.lastExceptionInfo.argValue = this.threadsPerDepthEntrySpinner.Value.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-9]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( OverflowException x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = this.threadsPerDepthEntrySpinner.GetType().Name + ".Value";
                this.lastExceptionInfo.argValue = this.threadsPerDepthEntrySpinner.Value.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-9]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }
            catch ( Exception x ) {
                this.lastExceptionInfo.typeName = x.GetType().ToString();
                this.lastExceptionInfo.methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                this.lastExceptionInfo.argName = this.threadsPerDepthEntrySpinner.GetType().Name + ".Value";
                this.lastExceptionInfo.argValue = this.threadsPerDepthEntrySpinner.Value.ToString();
                this.lastExceptionInfo.message = x.Message;
                this.lastExceptionInfo.id = "[UI-9]";
                string args = lastExceptionInfo.argName + "=" + lastExceptionInfo.argValue;
                StdErrFlow.writeLine( lastExceptionInfo.id + " " + x.ToString() + " (" + lastExceptionInfo.methodName + ") " + args + Environment.NewLine );
                }

            return ( currentValue );
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// An action performed at the 'Error log' button click.
        /// </summary>
        /// <param name="sender">The GUI component that cause the action.</param>
        /// <param name="e">Arguments of the action related with the GUI sender component.</param>        

        private void errorLogButton_Click( object sender, EventArgs e )
            {
            CustomMessageBox.CustomMessageBox.ShowBox( StdErrFlow.getCurrentStdErrLogContent(), "Error log" );
            }

        //______________________________________________________________________________________________________________________________

        }
    }
