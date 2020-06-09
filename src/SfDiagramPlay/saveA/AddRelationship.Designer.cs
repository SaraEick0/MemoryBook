namespace SfDiagramPlay
{
    partial class AddRelationship
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboMember1 = new System.Windows.Forms.ComboBox();
            this.comboMember2 = new System.Windows.Forms.ComboBox();
            this.comboRelType1 = new System.Windows.Forms.ComboBox();
            this.comboRelType2 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(226, 244);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnOk);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(411, 244);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.OnCancel);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(134, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Member 1:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(134, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Member 2:";
            // 
            // comboMember1
            // 
            this.comboMember1.FormattingEnabled = true;
            this.comboMember1.Location = new System.Drawing.Point(202, 70);
            this.comboMember1.Name = "comboMember1";
            this.comboMember1.Size = new System.Drawing.Size(121, 21);
            this.comboMember1.TabIndex = 4;
            // 
            // comboMember2
            // 
            this.comboMember2.FormattingEnabled = true;
            this.comboMember2.Location = new System.Drawing.Point(202, 102);
            this.comboMember2.Name = "comboMember2";
            this.comboMember2.Size = new System.Drawing.Size(121, 21);
            this.comboMember2.TabIndex = 5;
            // 
            // comboRelType1
            // 
            this.comboRelType1.FormattingEnabled = true;
            this.comboRelType1.Location = new System.Drawing.Point(382, 70);
            this.comboRelType1.Name = "comboRelType1";
            this.comboRelType1.Size = new System.Drawing.Size(121, 21);
            this.comboRelType1.TabIndex = 6;
            // 
            // comboRelType2
            // 
            this.comboRelType2.FormattingEnabled = true;
            this.comboRelType2.Location = new System.Drawing.Point(382, 102);
            this.comboRelType2.Name = "comboRelType2";
            this.comboRelType2.Size = new System.Drawing.Size(121, 21);
            this.comboRelType2.TabIndex = 7;
            // 
            // AddRelationship
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.comboRelType2);
            this.Controls.Add(this.comboRelType1);
            this.Controls.Add(this.comboMember2);
            this.Controls.Add(this.comboMember1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "AddRelationship";
            this.Text = "AddRelationship";
            this.Load += new System.EventHandler(this.AddRelationship_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboMember1;
        private System.Windows.Forms.ComboBox comboMember2;
        private System.Windows.Forms.ComboBox comboRelType1;
        private System.Windows.Forms.ComboBox comboRelType2;
    }
}