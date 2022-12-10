namespace SmartTran.Forms
{
    partial class Option
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox_AGS_VOICE = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox_AGS_VOICE);
            this.groupBox1.Location = new System.Drawing.Point(23, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(363, 199);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Text Option";
            // 
            // checkBox_AGS_VOICE
            // 
            this.checkBox_AGS_VOICE.AutoSize = true;
            this.checkBox_AGS_VOICE.Checked = true;
            this.checkBox_AGS_VOICE.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AGS_VOICE.Location = new System.Drawing.Point(16, 51);
            this.checkBox_AGS_VOICE.Name = "checkBox_AGS_VOICE";
            this.checkBox_AGS_VOICE.Size = new System.Drawing.Size(311, 28);
            this.checkBox_AGS_VOICE.TabIndex = 1;
            this.checkBox_AGS_VOICE.Text = "AGS Voice Macro Filtering";
            this.checkBox_AGS_VOICE.UseVisualStyleBackColor = true;
            // 
            // Option
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 369);
            this.Controls.Add(this.groupBox1);
            this.Name = "Option";
            this.Text = "Option";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox_AGS_VOICE;
    }
}