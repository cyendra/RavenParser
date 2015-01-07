namespace RavenParser.ExForm {
    partial class Input {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.runBtn = new System.Windows.Forms.Button();
            this.codeText = new System.Windows.Forms.TextBox();
            this.consoleText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // runBtn
            // 
            this.runBtn.Location = new System.Drawing.Point(523, 341);
            this.runBtn.Name = "runBtn";
            this.runBtn.Size = new System.Drawing.Size(75, 23);
            this.runBtn.TabIndex = 0;
            this.runBtn.Text = "运行";
            this.runBtn.UseVisualStyleBackColor = true;
            this.runBtn.Click += new System.EventHandler(this.runBtn_Click);
            // 
            // codeText
            // 
            this.codeText.AcceptsReturn = true;
            this.codeText.AcceptsTab = true;
            this.codeText.Location = new System.Drawing.Point(12, 12);
            this.codeText.Multiline = true;
            this.codeText.Name = "codeText";
            this.codeText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.codeText.Size = new System.Drawing.Size(586, 240);
            this.codeText.TabIndex = 1;
            this.codeText.WordWrap = false;
            // 
            // consoleText
            // 
            this.consoleText.Location = new System.Drawing.Point(12, 258);
            this.consoleText.Multiline = true;
            this.consoleText.Name = "consoleText";
            this.consoleText.ReadOnly = true;
            this.consoleText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.consoleText.Size = new System.Drawing.Size(586, 77);
            this.consoleText.TabIndex = 2;
            // 
            // Input
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 376);
            this.Controls.Add(this.consoleText);
            this.Controls.Add(this.codeText);
            this.Controls.Add(this.runBtn);
            this.Name = "Input";
            this.Text = "Input";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button runBtn;
        private System.Windows.Forms.TextBox codeText;
        private System.Windows.Forms.TextBox consoleText;
    }
}