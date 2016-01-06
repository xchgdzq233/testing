namespace CalculonBack
{
    partial class TestMain
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
            this.lblTopic = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTopic
            // 
            this.lblTopic.AutoSize = true;
            this.lblTopic.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTopic.Location = new System.Drawing.Point(17, 20);
            this.lblTopic.Name = "lblTopic";
            this.lblTopic.Size = new System.Drawing.Size(123, 20);
            this.lblTopic.TabIndex = 0;
            this.lblTopic.Text = "document title";
            // 
            // TestMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(784, 662);
            this.Controls.Add(this.lblTopic);
            this.Name = "TestMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TestMain";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTopic;
    }
}