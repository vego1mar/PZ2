namespace WebCrawler
    {
    partial class MainWindow
        {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
            {
            if (disposing && (components != null))
                {
                components.Dispose();
                }
            base.Dispose(disposing);
            }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
            {
            this.websiteURLLabel = new System.Windows.Forms.Label();
            this.websiteURLTextBox = new System.Windows.Forms.TextBox();
            this.levelOfDepthLabel = new System.Windows.Forms.Label();
            this.proceedButton = new System.Windows.Forms.Button();
            this.foundedLinksLabel = new System.Windows.Forms.Label();
            this.foundedLinksToUpdateLabel = new System.Windows.Forms.Label();
            this.currentStateLabel = new System.Windows.Forms.Label();
            this.currentStateToUpdateLabel = new System.Windows.Forms.Label();
            this.infoButton = new System.Windows.Forms.Button();
            this.counterToUpdateLabel = new System.Windows.Forms.Label();
            this.levelOfDepthSpinner = new System.Windows.Forms.NumericUpDown();
            this.asynchronousWebsitesDownloadCheckBox = new System.Windows.Forms.CheckBox();
            this.timerStartRadioButton = new System.Windows.Forms.RadioButton();
            this.timerStopRadioButton = new System.Windows.Forms.RadioButton();
            this.timerGroupBox = new System.Windows.Forms.GroupBox();
            this.threadsPerDepthEntrySpinner = new System.Windows.Forms.NumericUpDown();
            this.threadsPerDepthEntryLabel = new System.Windows.Forms.Label();
            this.errorLogButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.levelOfDepthSpinner)).BeginInit();
            this.timerGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.threadsPerDepthEntrySpinner)).BeginInit();
            this.SuspendLayout();
            // 
            // websiteURLLabel
            // 
            this.websiteURLLabel.AutoSize = true;
            this.websiteURLLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.websiteURLLabel.Location = new System.Drawing.Point(22, 43);
            this.websiteURLLabel.Name = "websiteURLLabel";
            this.websiteURLLabel.Size = new System.Drawing.Size(86, 13);
            this.websiteURLLabel.TabIndex = 0;
            this.websiteURLLabel.Text = "Website URL:";
            // 
            // websiteURLTextBox
            // 
            this.websiteURLTextBox.Font = new System.Drawing.Font("Roboto Condensed", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.websiteURLTextBox.Location = new System.Drawing.Point(106, 38);
            this.websiteURLTextBox.Name = "websiteURLTextBox";
            this.websiteURLTextBox.Size = new System.Drawing.Size(402, 23);
            this.websiteURLTextBox.TabIndex = 1;
            this.websiteURLTextBox.Text = "http://www.";
            // 
            // levelOfDepthLabel
            // 
            this.levelOfDepthLabel.AutoSize = true;
            this.levelOfDepthLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.levelOfDepthLabel.Location = new System.Drawing.Point(22, 107);
            this.levelOfDepthLabel.Name = "levelOfDepthLabel";
            this.levelOfDepthLabel.Size = new System.Drawing.Size(93, 13);
            this.levelOfDepthLabel.TabIndex = 4;
            this.levelOfDepthLabel.Text = "Level of depth:";
            // 
            // proceedButton
            // 
            this.proceedButton.Location = new System.Drawing.Point(400, 73);
            this.proceedButton.Name = "proceedButton";
            this.proceedButton.Size = new System.Drawing.Size(108, 23);
            this.proceedButton.TabIndex = 2;
            this.proceedButton.Text = "Proceed";
            this.proceedButton.UseVisualStyleBackColor = true;
            this.proceedButton.Click += new System.EventHandler(this.proceedButton_Click);
            // 
            // foundedLinksLabel
            // 
            this.foundedLinksLabel.AutoSize = true;
            this.foundedLinksLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.foundedLinksLabel.Location = new System.Drawing.Point(418, 156);
            this.foundedLinksLabel.Name = "foundedLinksLabel";
            this.foundedLinksLabel.Size = new System.Drawing.Size(90, 13);
            this.foundedLinksLabel.TabIndex = 7;
            this.foundedLinksLabel.Text = "Founded links:";
            // 
            // foundedLinksToUpdateLabel
            // 
            this.foundedLinksToUpdateLabel.AutoSize = true;
            this.foundedLinksToUpdateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.foundedLinksToUpdateLabel.Location = new System.Drawing.Point(188, 169);
            this.foundedLinksToUpdateLabel.MinimumSize = new System.Drawing.Size(320, 15);
            this.foundedLinksToUpdateLabel.Name = "foundedLinksToUpdateLabel";
            this.foundedLinksToUpdateLabel.Size = new System.Drawing.Size(320, 15);
            this.foundedLinksToUpdateLabel.TabIndex = 8;
            this.foundedLinksToUpdateLabel.Text = "0";
            this.foundedLinksToUpdateLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // currentStateLabel
            // 
            this.currentStateLabel.AutoSize = true;
            this.currentStateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.currentStateLabel.Location = new System.Drawing.Point(424, 207);
            this.currentStateLabel.Name = "currentStateLabel";
            this.currentStateLabel.Size = new System.Drawing.Size(84, 13);
            this.currentStateLabel.TabIndex = 9;
            this.currentStateLabel.Text = "Current state:";
            // 
            // currentStateToUpdateLabel
            // 
            this.currentStateToUpdateLabel.AutoSize = true;
            this.currentStateToUpdateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.currentStateToUpdateLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.currentStateToUpdateLabel.Location = new System.Drawing.Point(58, 220);
            this.currentStateToUpdateLabel.MinimumSize = new System.Drawing.Size(450, 15);
            this.currentStateToUpdateLabel.Name = "currentStateToUpdateLabel";
            this.currentStateToUpdateLabel.Size = new System.Drawing.Size(450, 15);
            this.currentStateToUpdateLabel.TabIndex = 10;
            this.currentStateToUpdateLabel.Text = "None";
            this.currentStateToUpdateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // infoButton
            // 
            this.infoButton.Location = new System.Drawing.Point(433, 258);
            this.infoButton.Name = "infoButton";
            this.infoButton.Size = new System.Drawing.Size(75, 23);
            this.infoButton.TabIndex = 11;
            this.infoButton.Text = "Info";
            this.infoButton.UseVisualStyleBackColor = true;
            this.infoButton.Click += new System.EventHandler(this.InfoButton_Click);
            // 
            // counterToUpdateLabel
            // 
            this.counterToUpdateLabel.AutoSize = true;
            this.counterToUpdateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.counterToUpdateLabel.Location = new System.Drawing.Point(69, 73);
            this.counterToUpdateLabel.MinimumSize = new System.Drawing.Size(325, 0);
            this.counterToUpdateLabel.Name = "counterToUpdateLabel";
            this.counterToUpdateLabel.Size = new System.Drawing.Size(325, 31);
            this.counterToUpdateLabel.TabIndex = 3;
            this.counterToUpdateLabel.Text = "0:00:00:00";
            this.counterToUpdateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // levelOfDepthSpinner
            // 
            this.levelOfDepthSpinner.BackColor = System.Drawing.SystemColors.HighlightText;
            this.levelOfDepthSpinner.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.levelOfDepthSpinner.Location = new System.Drawing.Point(25, 126);
            this.levelOfDepthSpinner.Maximum = new decimal(new int[] {
            -2,
            0,
            0,
            0});
            this.levelOfDepthSpinner.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.levelOfDepthSpinner.Name = "levelOfDepthSpinner";
            this.levelOfDepthSpinner.Size = new System.Drawing.Size(104, 21);
            this.levelOfDepthSpinner.TabIndex = 5;
            this.levelOfDepthSpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.levelOfDepthSpinner.ThousandsSeparator = true;
            this.levelOfDepthSpinner.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // asynchronousWebsitesDownloadCheckBox
            // 
            this.asynchronousWebsitesDownloadCheckBox.AutoSize = true;
            this.asynchronousWebsitesDownloadCheckBox.Location = new System.Drawing.Point(25, 273);
            this.asynchronousWebsitesDownloadCheckBox.Name = "asynchronousWebsitesDownloadCheckBox";
            this.asynchronousWebsitesDownloadCheckBox.Size = new System.Drawing.Size(186, 17);
            this.asynchronousWebsitesDownloadCheckBox.TabIndex = 6;
            this.asynchronousWebsitesDownloadCheckBox.Text = "Asynchronous websites download";
            this.asynchronousWebsitesDownloadCheckBox.UseVisualStyleBackColor = true;
            // 
            // timerStartRadioButton
            // 
            this.timerStartRadioButton.AutoSize = true;
            this.timerStartRadioButton.Location = new System.Drawing.Point(19, 19);
            this.timerStartRadioButton.Name = "timerStartRadioButton";
            this.timerStartRadioButton.Size = new System.Drawing.Size(47, 17);
            this.timerStartRadioButton.TabIndex = 12;
            this.timerStartRadioButton.TabStop = true;
            this.timerStartRadioButton.Text = "Start";
            this.timerStartRadioButton.UseVisualStyleBackColor = true;
            this.timerStartRadioButton.CheckedChanged += new System.EventHandler(this.timerStartRadioButton_CheckedChanged);
            // 
            // timerStopRadioButton
            // 
            this.timerStopRadioButton.AutoSize = true;
            this.timerStopRadioButton.Enabled = false;
            this.timerStopRadioButton.Location = new System.Drawing.Point(82, 19);
            this.timerStopRadioButton.Name = "timerStopRadioButton";
            this.timerStopRadioButton.Size = new System.Drawing.Size(47, 17);
            this.timerStopRadioButton.TabIndex = 13;
            this.timerStopRadioButton.TabStop = true;
            this.timerStopRadioButton.Text = "Stop";
            this.timerStopRadioButton.UseVisualStyleBackColor = true;
            this.timerStopRadioButton.CheckedChanged += new System.EventHandler(this.timerStopRadioButton_CheckedChanged);
            // 
            // timerGroupBox
            // 
            this.timerGroupBox.Controls.Add(this.timerStartRadioButton);
            this.timerGroupBox.Controls.Add(this.timerStopRadioButton);
            this.timerGroupBox.Location = new System.Drawing.Point(181, 107);
            this.timerGroupBox.Name = "timerGroupBox";
            this.timerGroupBox.Size = new System.Drawing.Size(152, 44);
            this.timerGroupBox.TabIndex = 14;
            this.timerGroupBox.TabStop = false;
            this.timerGroupBox.Text = "Timer control";
            // 
            // threadsPerDepthEntrySpinner
            // 
            this.threadsPerDepthEntrySpinner.Location = new System.Drawing.Point(25, 188);
            this.threadsPerDepthEntrySpinner.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.threadsPerDepthEntrySpinner.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.threadsPerDepthEntrySpinner.Name = "threadsPerDepthEntrySpinner";
            this.threadsPerDepthEntrySpinner.Size = new System.Drawing.Size(104, 20);
            this.threadsPerDepthEntrySpinner.TabIndex = 15;
            this.threadsPerDepthEntrySpinner.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.threadsPerDepthEntrySpinner.ThousandsSeparator = true;
            this.threadsPerDepthEntrySpinner.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // threadsPerDepthEntryLabel
            // 
            this.threadsPerDepthEntryLabel.AutoSize = true;
            this.threadsPerDepthEntryLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.threadsPerDepthEntryLabel.Location = new System.Drawing.Point(22, 169);
            this.threadsPerDepthEntryLabel.Name = "threadsPerDepthEntryLabel";
            this.threadsPerDepthEntryLabel.Size = new System.Drawing.Size(147, 13);
            this.threadsPerDepthEntryLabel.TabIndex = 16;
            this.threadsPerDepthEntryLabel.Text = "Threads per depth entry:";
            // 
            // errorLogButton
            // 
            this.errorLogButton.Location = new System.Drawing.Point(330, 258);
            this.errorLogButton.Name = "errorLogButton";
            this.errorLogButton.Size = new System.Drawing.Size(75, 23);
            this.errorLogButton.TabIndex = 17;
            this.errorLogButton.Text = "Error log";
            this.errorLogButton.UseVisualStyleBackColor = true;
            this.errorLogButton.Click += new System.EventHandler(this.errorLogButton_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(550, 302);
            this.Controls.Add(this.errorLogButton);
            this.Controls.Add(this.threadsPerDepthEntryLabel);
            this.Controls.Add(this.threadsPerDepthEntrySpinner);
            this.Controls.Add(this.timerGroupBox);
            this.Controls.Add(this.asynchronousWebsitesDownloadCheckBox);
            this.Controls.Add(this.levelOfDepthSpinner);
            this.Controls.Add(this.counterToUpdateLabel);
            this.Controls.Add(this.infoButton);
            this.Controls.Add(this.currentStateToUpdateLabel);
            this.Controls.Add(this.currentStateLabel);
            this.Controls.Add(this.foundedLinksToUpdateLabel);
            this.Controls.Add(this.foundedLinksLabel);
            this.Controls.Add(this.proceedButton);
            this.Controls.Add(this.levelOfDepthLabel);
            this.Controls.Add(this.websiteURLTextBox);
            this.Controls.Add(this.websiteURLLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WebCrawler";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.levelOfDepthSpinner)).EndInit();
            this.timerGroupBox.ResumeLayout(false);
            this.timerGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.threadsPerDepthEntrySpinner)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

            }

        #endregion
        private System.Windows.Forms.Label websiteURLLabel;
        private System.Windows.Forms.TextBox websiteURLTextBox;
        private System.Windows.Forms.Label levelOfDepthLabel;
        private System.Windows.Forms.Button proceedButton;
        private System.Windows.Forms.Label foundedLinksLabel;
        private System.Windows.Forms.Label foundedLinksToUpdateLabel;
        private System.Windows.Forms.Label currentStateLabel;
        private System.Windows.Forms.Label currentStateToUpdateLabel;
        private System.Windows.Forms.Button infoButton;
        private System.Windows.Forms.Label counterToUpdateLabel;
        private System.Windows.Forms.NumericUpDown levelOfDepthSpinner;
        private System.Windows.Forms.CheckBox asynchronousWebsitesDownloadCheckBox;
        private System.Windows.Forms.RadioButton timerStartRadioButton;
        private System.Windows.Forms.RadioButton timerStopRadioButton;
        private System.Windows.Forms.GroupBox timerGroupBox;
        private System.Windows.Forms.NumericUpDown threadsPerDepthEntrySpinner;
        private System.Windows.Forms.Label threadsPerDepthEntryLabel;
        private System.Windows.Forms.Button errorLogButton;
        }
    }

