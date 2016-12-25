using System;
using System.Windows.Forms;

/// <summary>
/// A custom message box designed for the needs of an application behind the WebCrawler namespace.
/// </summary>

namespace CustomMessageBox
    {
    public partial class CustomMessageBox : Form
        {

        private static CustomMessageBox webCrawlerMsgBox;
        private const int CP_NOCLOSE_BUTTON = 0x200;

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// The default constructor.
        /// Invokes InitializeComponent() method that is required for Designer support method.
        /// </summary>

        public CustomMessageBox()
            {
            InitializeComponent();
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Show dialog of this custom message box.
        /// </summary>
        /// <param name="msg">A message to display in the message box area.</param>
        /// <param name="caption">A title of the message box title bar.</param>

        public static void ShowBox( string msg, string caption )
            {
            webCrawlerMsgBox = new CustomMessageBox();
            webCrawlerMsgBox.messageTextBox.Text = msg;
            webCrawlerMsgBox.Text = caption;
            webCrawlerMsgBox.AutoScroll = true;
            webCrawlerMsgBox.ShowDialog();
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// An action performed when the 'OK' button is 'clicked'.
        /// </summary>
        /// <param name="sender">The GUI component that cause the action.</param>
        /// <param name="e">Arguments of the action related with the GUI sender component.</param>

        private void okButton_Click( object sender, EventArgs e )
            {
            webCrawlerMsgBox.Dispose();
            }

        //______________________________________________________________________________________________________________________________

        /// <summary>
        /// Overrides the get functionality of the property CreateParams.
        /// This will cause in disabling the system 'Close' button on the title bar.
        /// </summary>

        protected override CreateParams CreateParams
        {
            get
            {
               CreateParams myCp = base.CreateParams;
               myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON ;
               return myCp;
            }
        }

        //______________________________________________________________________________________________________________________________

        }
    }
