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
            this.components = new System.ComponentModel.Container();
            this.websiteURLLabel = new System.Windows.Forms.Label();
            this.websiteURLTextBox = new System.Windows.Forms.TextBox();
            this.phraseToSearchForLabel = new System.Windows.Forms.Label();
            this.phraseToSearchForTextBox = new System.Windows.Forms.TextBox();
            this.levelOfDepthLabel = new System.Windows.Forms.Label();
            this.proceedButton = new System.Windows.Forms.Button();
            this.foundedLinksLabel = new System.Windows.Forms.Label();
            this.foundedLinksToUpdateLabel = new System.Windows.Forms.Label();
            this.currentStateLabel = new System.Windows.Forms.Label();
            this.currentStateToUpdateLabel = new System.Windows.Forms.Label();
            this.elapsedTimeLabel = new System.Windows.Forms.Label();
            this.elapsedTimeToUpdateLabel = new System.Windows.Forms.Label();
            this.level1RadioButton = new System.Windows.Forms.RadioButton();
            this.level2RadioButton = new System.Windows.Forms.RadioButton();
            this.level3RadioButton = new System.Windows.Forms.RadioButton();
            this.proceedTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // websiteURLLabel
            // 
            this.websiteURLLabel.AutoSize = true;
            this.websiteURLLabel.Location = new System.Drawing.Point(49, 29);
            this.websiteURLLabel.Name = "websiteURLLabel";
            this.websiteURLLabel.Size = new System.Drawing.Size(74, 13);
            this.websiteURLLabel.TabIndex = 0;
            this.websiteURLLabel.Text = "Website URL:";
            // 
            // websiteURLTextBox
            // 
            this.websiteURLTextBox.Location = new System.Drawing.Point(129, 26);
            this.websiteURLTextBox.Name = "websiteURLTextBox";
            this.websiteURLTextBox.Size = new System.Drawing.Size(379, 20);
            this.websiteURLTextBox.TabIndex = 1;
            this.websiteURLTextBox.Text = "http://www.";
            // 
            // phraseToSearchForLabel
            // 
            this.phraseToSearchForLabel.AutoSize = true;
            this.phraseToSearchForLabel.Location = new System.Drawing.Point(18, 66);
            this.phraseToSearchForLabel.Name = "phraseToSearchForLabel";
            this.phraseToSearchForLabel.Size = new System.Drawing.Size(105, 13);
            this.phraseToSearchForLabel.TabIndex = 2;
            this.phraseToSearchForLabel.Text = "Phrase to search for:";
            // 
            // phraseToSearchForTextBox
            // 
            this.phraseToSearchForTextBox.Location = new System.Drawing.Point(129, 63);
            this.phraseToSearchForTextBox.Name = "phraseToSearchForTextBox";
            this.phraseToSearchForTextBox.Size = new System.Drawing.Size(379, 20);
            this.phraseToSearchForTextBox.TabIndex = 3;
            // 
            // levelOfDepthLabel
            // 
            this.levelOfDepthLabel.AutoSize = true;
            this.levelOfDepthLabel.Location = new System.Drawing.Point(34, 131);
            this.levelOfDepthLabel.Name = "levelOfDepthLabel";
            this.levelOfDepthLabel.Size = new System.Drawing.Size(78, 13);
            this.levelOfDepthLabel.TabIndex = 5;
            this.levelOfDepthLabel.Text = "Level of depth:";
            // 
            // proceedButton
            // 
            this.proceedButton.Location = new System.Drawing.Point(400, 101);
            this.proceedButton.Name = "proceedButton";
            this.proceedButton.Size = new System.Drawing.Size(108, 23);
            this.proceedButton.TabIndex = 9;
            this.proceedButton.Text = "Proceed";
            this.proceedButton.UseVisualStyleBackColor = true;
            this.proceedButton.Click += new System.EventHandler(this.proceedButton_Click);
            // 
            // foundedLinksLabel
            // 
            this.foundedLinksLabel.AutoSize = true;
            this.foundedLinksLabel.Location = new System.Drawing.Point(347, 158);
            this.foundedLinksLabel.Name = "foundedLinksLabel";
            this.foundedLinksLabel.Size = new System.Drawing.Size(76, 13);
            this.foundedLinksLabel.TabIndex = 10;
            this.foundedLinksLabel.Text = "Founded links:";
            // 
            // foundedLinksToUpdateLabel
            // 
            this.foundedLinksToUpdateLabel.AutoSize = true;
            this.foundedLinksToUpdateLabel.Location = new System.Drawing.Point(438, 158);
            this.foundedLinksToUpdateLabel.MinimumSize = new System.Drawing.Size(70, 15);
            this.foundedLinksToUpdateLabel.Name = "foundedLinksToUpdateLabel";
            this.foundedLinksToUpdateLabel.Size = new System.Drawing.Size(70, 15);
            this.foundedLinksToUpdateLabel.TabIndex = 11;
            this.foundedLinksToUpdateLabel.Text = "0";
            this.foundedLinksToUpdateLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // currentStateLabel
            // 
            this.currentStateLabel.AutoSize = true;
            this.currentStateLabel.Location = new System.Drawing.Point(347, 181);
            this.currentStateLabel.Name = "currentStateLabel";
            this.currentStateLabel.Size = new System.Drawing.Size(70, 13);
            this.currentStateLabel.TabIndex = 12;
            this.currentStateLabel.Text = "Current state:";
            // 
            // currentStateToUpdateLabel
            // 
            this.currentStateToUpdateLabel.AutoSize = true;
            this.currentStateToUpdateLabel.Location = new System.Drawing.Point(438, 181);
            this.currentStateToUpdateLabel.MinimumSize = new System.Drawing.Size(70, 15);
            this.currentStateToUpdateLabel.Name = "currentStateToUpdateLabel";
            this.currentStateToUpdateLabel.Size = new System.Drawing.Size(70, 15);
            this.currentStateToUpdateLabel.TabIndex = 13;
            this.currentStateToUpdateLabel.Text = "None";
            this.currentStateToUpdateLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // elapsedTimeLabel
            // 
            this.elapsedTimeLabel.AutoSize = true;
            this.elapsedTimeLabel.Location = new System.Drawing.Point(347, 205);
            this.elapsedTimeLabel.Name = "elapsedTimeLabel";
            this.elapsedTimeLabel.Size = new System.Drawing.Size(70, 13);
            this.elapsedTimeLabel.TabIndex = 14;
            this.elapsedTimeLabel.Text = "Elapsed time:";
            // 
            // elapsedTimeToUpdateLabel
            // 
            this.elapsedTimeToUpdateLabel.AutoSize = true;
            this.elapsedTimeToUpdateLabel.Location = new System.Drawing.Point(438, 205);
            this.elapsedTimeToUpdateLabel.MinimumSize = new System.Drawing.Size(70, 15);
            this.elapsedTimeToUpdateLabel.Name = "elapsedTimeToUpdateLabel";
            this.elapsedTimeToUpdateLabel.Size = new System.Drawing.Size(70, 15);
            this.elapsedTimeToUpdateLabel.TabIndex = 15;
            this.elapsedTimeToUpdateLabel.Text = "N/A";
            this.elapsedTimeToUpdateLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // level1RadioButton
            // 
            this.level1RadioButton.AutoSize = true;
            this.level1RadioButton.Checked = true;
            this.level1RadioButton.Location = new System.Drawing.Point(37, 156);
            this.level1RadioButton.Name = "level1RadioButton";
            this.level1RadioButton.Size = new System.Drawing.Size(57, 17);
            this.level1RadioButton.TabIndex = 16;
            this.level1RadioButton.TabStop = true;
            this.level1RadioButton.Text = "Level I";
            this.level1RadioButton.UseVisualStyleBackColor = true;
            // 
            // level2RadioButton
            // 
            this.level2RadioButton.AutoSize = true;
            this.level2RadioButton.Location = new System.Drawing.Point(37, 180);
            this.level2RadioButton.Name = "level2RadioButton";
            this.level2RadioButton.Size = new System.Drawing.Size(60, 17);
            this.level2RadioButton.TabIndex = 17;
            this.level2RadioButton.TabStop = true;
            this.level2RadioButton.Text = "Level II";
            this.level2RadioButton.UseVisualStyleBackColor = true;
            // 
            // level3RadioButton
            // 
            this.level3RadioButton.AutoSize = true;
            this.level3RadioButton.Location = new System.Drawing.Point(37, 203);
            this.level3RadioButton.Name = "level3RadioButton";
            this.level3RadioButton.Size = new System.Drawing.Size(63, 17);
            this.level3RadioButton.TabIndex = 18;
            this.level3RadioButton.TabStop = true;
            this.level3RadioButton.Text = "Level III";
            this.level3RadioButton.UseVisualStyleBackColor = true;
            // 
            // proceedTimer
            // 
            this.proceedTimer.Interval = 1000;
            this.proceedTimer.Tick += new System.EventHandler(this.proceedTimer_Tick);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(550, 277);
            this.Controls.Add(this.level3RadioButton);
            this.Controls.Add(this.level2RadioButton);
            this.Controls.Add(this.level1RadioButton);
            this.Controls.Add(this.elapsedTimeToUpdateLabel);
            this.Controls.Add(this.elapsedTimeLabel);
            this.Controls.Add(this.currentStateToUpdateLabel);
            this.Controls.Add(this.currentStateLabel);
            this.Controls.Add(this.foundedLinksToUpdateLabel);
            this.Controls.Add(this.foundedLinksLabel);
            this.Controls.Add(this.proceedButton);
            this.Controls.Add(this.levelOfDepthLabel);
            this.Controls.Add(this.phraseToSearchForTextBox);
            this.Controls.Add(this.phraseToSearchForLabel);
            this.Controls.Add(this.websiteURLTextBox);
            this.Controls.Add(this.websiteURLLabel);
            this.Name = "MainWindow";
            this.Text = "WebCrawler";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

            }

        #endregion
        private System.Windows.Forms.Label websiteURLLabel;
        private System.Windows.Forms.TextBox websiteURLTextBox;
        private System.Windows.Forms.Label phraseToSearchForLabel;
        private System.Windows.Forms.TextBox phraseToSearchForTextBox;
        private System.Windows.Forms.Label levelOfDepthLabel;
        private System.Windows.Forms.Button proceedButton;
        private System.Windows.Forms.Label foundedLinksLabel;
        private System.Windows.Forms.Label foundedLinksToUpdateLabel;
        private System.Windows.Forms.Label currentStateLabel;
        private System.Windows.Forms.Label currentStateToUpdateLabel;
        private System.Windows.Forms.Label elapsedTimeLabel;
        private System.Windows.Forms.Label elapsedTimeToUpdateLabel;
        private System.Windows.Forms.RadioButton level1RadioButton;
        private System.Windows.Forms.RadioButton level2RadioButton;
        private System.Windows.Forms.RadioButton level3RadioButton;
        private System.Windows.Forms.Timer proceedTimer;
        }
    }

