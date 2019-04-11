using OpenSBP_Client.Controls;
namespace OpenSBP_Client.Forms {
    partial class frmPartFileLoad {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPartFileLoad));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRecallLast = new System.Windows.Forms.Button();
            this.btnPickFile = new System.Windows.Forms.Button();
            this.btnFileStats = new System.Windows.Forms.Button();
            this.cboTabbing = new System.Windows.Forms.ComboBox();
            this.cboPlunge = new System.Windows.Forms.ComboBox();
            this.cboOffset = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtPropZ = new System.Windows.Forms.TextBox();
            this.txtPropX = new System.Windows.Forms.TextBox();
            this.txtPropY = new System.Windows.Forms.TextBox();
            this.txtPlunge = new System.Windows.Forms.TextBox();
            this.txtReps = new System.Windows.Forms.TextBox();
            this.txtPartFilename = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Monospac821 BT", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(84, 116);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Part File Load:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Monospac821 BT", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(84, 392);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(274, 22);
            this.label2.TabIndex = 2;
            this.label2.Text = "Plunge (per repetition):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Monospac821 BT", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(84, 462);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(164, 22);
            this.label3.TabIndex = 3;
            this.label3.Text = "Plunge from 0:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Monospac821 BT", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(84, 288);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 22);
            this.label4.TabIndex = 4;
            this.label4.Text = "Tabbing:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Monospac821 BT", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(84, 256);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(153, 22);
            this.label5.TabIndex = 5;
            this.label5.Text = "Proportion Z:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Monospac821 BT", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(84, 221);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(153, 22);
            this.label6.TabIndex = 6;
            this.label6.Text = "Proportion Y:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Monospac821 BT", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(84, 184);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(153, 22);
            this.label7.TabIndex = 7;
            this.label7.Text = "Proportion X:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Monospac821 BT", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(84, 151);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(219, 22);
            this.label8.TabIndex = 8;
            this.label8.Text = "Offset in 2D or 3D:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Monospac821 BT", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(84, 357);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(582, 22);
            this.label9.TabIndex = 9;
            this.label9.Text = "Parameters for \'templates\' having just XY movements.";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Monospac821 BT", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(84, 427);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(142, 22);
            this.label10.TabIndex = 10;
            this.label10.Text = "Repetitions:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Monospac821 BT", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(84, 600);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(274, 22);
            this.label11.TabIndex = 11;
            this.label11.Text = "VS - to set Speed Values";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Monospac821 BT", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(84, 530);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(186, 22);
            this.label12.TabIndex = 12;
            this.label12.Text = "Related Commands";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Monospac821 BT", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(84, 565);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(296, 22);
            this.label13.TabIndex = 13;
            this.label13.Text = "VB - to set Tabbing Values";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(639, 664);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 27);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnRecallLast
            // 
            this.btnRecallLast.BackColor = System.Drawing.Color.Cyan;
            this.btnRecallLast.Location = new System.Drawing.Point(88, 664);
            this.btnRecallLast.Margin = new System.Windows.Forms.Padding(4);
            this.btnRecallLast.Name = "btnRecallLast";
            this.btnRecallLast.Size = new System.Drawing.Size(149, 27);
            this.btnRecallLast.TabIndex = 15;
            this.btnRecallLast.Text = "Recall &Last";
            this.btnRecallLast.UseVisualStyleBackColor = false;
            this.btnRecallLast.Click += new System.EventHandler(this.BtnRecallLast_Click);
            // 
            // btnPickFile
            // 
            this.btnPickFile.Location = new System.Drawing.Point(684, 111);
            this.btnPickFile.Margin = new System.Windows.Forms.Padding(4);
            this.btnPickFile.Name = "btnPickFile";
            this.btnPickFile.Size = new System.Drawing.Size(50, 27);
            this.btnPickFile.TabIndex = 16;
            this.btnPickFile.Text = "...";
            this.btnPickFile.UseVisualStyleBackColor = true;
            this.btnPickFile.Click += new System.EventHandler(this.BtnPickFile_Click);
            // 
            // btnFileStats
            // 
            this.btnFileStats.BackColor = System.Drawing.Color.Yellow;
            this.btnFileStats.Location = new System.Drawing.Point(374, 111);
            this.btnFileStats.Margin = new System.Windows.Forms.Padding(4);
            this.btnFileStats.Name = "btnFileStats";
            this.btnFileStats.Size = new System.Drawing.Size(39, 27);
            this.btnFileStats.TabIndex = 17;
            this.btnFileStats.Text = "!";
            this.btnFileStats.UseVisualStyleBackColor = false;
            // 
            // cboTabbing
            // 
            this.cboTabbing.Font = new System.Drawing.Font("Monospac821 BT", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTabbing.FormattingEnabled = true;
            this.cboTabbing.Location = new System.Drawing.Point(421, 286);
            this.cboTabbing.Margin = new System.Windows.Forms.Padding(4);
            this.cboTabbing.Name = "cboTabbing";
            this.cboTabbing.Size = new System.Drawing.Size(255, 27);
            this.cboTabbing.TabIndex = 23;
            // 
            // cboPlunge
            // 
            this.cboPlunge.Font = new System.Drawing.Font("Monospac821 BT", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboPlunge.FormattingEnabled = true;
            this.cboPlunge.Location = new System.Drawing.Point(421, 457);
            this.cboPlunge.Margin = new System.Windows.Forms.Padding(4);
            this.cboPlunge.Name = "cboPlunge";
            this.cboPlunge.Size = new System.Drawing.Size(255, 27);
            this.cboPlunge.TabIndex = 25;
            // 
            // cboOffset
            // 
            this.cboOffset.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cboOffset.Font = new System.Drawing.Font("Monospac821 BT", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboOffset.FormattingEnabled = true;
            this.cboOffset.Location = new System.Drawing.Point(421, 146);
            this.cboOffset.Margin = new System.Windows.Forms.Padding(4);
            this.cboOffset.Name = "cboOffset";
            this.cboOffset.Size = new System.Drawing.Size(255, 27);
            this.cboOffset.TabIndex = 26;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Monospac821 BT", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(266, 31);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(239, 32);
            this.label14.TabIndex = 27;
            this.label14.Text = "PART FILE LOAD";
            // 
            // txtPropZ
            // 
            this.txtPropZ.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(238)))), ((int)(((byte)(150)))));
            this.txtPropZ.Font = new System.Drawing.Font("Monospac821 BT", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPropZ.Location = new System.Drawing.Point(421, 251);
            this.txtPropZ.Margin = new System.Windows.Forms.Padding(4);
            this.txtPropZ.Name = "txtPropZ";
            this.txtPropZ.Size = new System.Drawing.Size(255, 27);
            this.txtPropZ.TabIndex = 24;
            // 
            // txtPropX
            // 
            this.txtPropX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(238)))), ((int)(((byte)(150)))));
            this.txtPropX.Font = new System.Drawing.Font("Monospac821 BT", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPropX.Location = new System.Drawing.Point(421, 181);
            this.txtPropX.Margin = new System.Windows.Forms.Padding(4);
            this.txtPropX.Name = "txtPropX";
            this.txtPropX.Size = new System.Drawing.Size(255, 27);
            this.txtPropX.TabIndex = 22;
            // 
            // txtPropY
            // 
            this.txtPropY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(238)))), ((int)(((byte)(150)))));
            this.txtPropY.Font = new System.Drawing.Font("Monospac821 BT", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPropY.Location = new System.Drawing.Point(421, 216);
            this.txtPropY.Margin = new System.Windows.Forms.Padding(4);
            this.txtPropY.Name = "txtPropY";
            this.txtPropY.Size = new System.Drawing.Size(255, 27);
            this.txtPropY.TabIndex = 21;
            // 
            // txtPlunge
            // 
            this.txtPlunge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(238)))), ((int)(((byte)(150)))));
            this.txtPlunge.Font = new System.Drawing.Font("Monospac821 BT", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPlunge.Location = new System.Drawing.Point(421, 387);
            this.txtPlunge.Margin = new System.Windows.Forms.Padding(4);
            this.txtPlunge.Name = "txtPlunge";
            this.txtPlunge.Size = new System.Drawing.Size(255, 27);
            this.txtPlunge.TabIndex = 20;
            // 
            // txtReps
            // 
            this.txtReps.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(238)))), ((int)(((byte)(150)))));
            this.txtReps.Font = new System.Drawing.Font("Monospac821 BT", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReps.Location = new System.Drawing.Point(421, 422);
            this.txtReps.Margin = new System.Windows.Forms.Padding(4);
            this.txtReps.Name = "txtReps";
            this.txtReps.Size = new System.Drawing.Size(255, 27);
            this.txtReps.TabIndex = 19;
            // 
            // txtPartFilename
            // 
            this.txtPartFilename.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(238)))), ((int)(((byte)(150)))));
            this.txtPartFilename.Font = new System.Drawing.Font("Monospac821 BT", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPartFilename.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtPartFilename.Location = new System.Drawing.Point(421, 111);
            this.txtPartFilename.Margin = new System.Windows.Forms.Padding(4);
            this.txtPartFilename.Name = "txtPartFilename";
            this.txtPartFilename.Size = new System.Drawing.Size(255, 30);
            this.txtPartFilename.TabIndex = 1;
            this.txtPartFilename.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPartFilename.TextChanged += new System.EventHandler(this.TxtPartFilename_TextChanged);
            // 
            // frmPartFileLoad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(746, 704);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.cboOffset);
            this.Controls.Add(this.cboPlunge);
            this.Controls.Add(this.txtPropZ);
            this.Controls.Add(this.cboTabbing);
            this.Controls.Add(this.txtPropX);
            this.Controls.Add(this.txtPropY);
            this.Controls.Add(this.txtPlunge);
            this.Controls.Add(this.txtReps);
            this.Controls.Add(this.btnFileStats);
            this.Controls.Add(this.btnPickFile);
            this.Controls.Add(this.btnRecallLast);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPartFilename);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Monospac821 BT", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPartFileLoad";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Part File Load";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmPartFileLoad_FormClosing);
            this.Load += new System.EventHandler(this.FrmPartFileLload_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPartFilename;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRecallLast;
        private System.Windows.Forms.Button btnPickFile;
        private System.Windows.Forms.Button btnFileStats;
        private System.Windows.Forms.TextBox txtReps;
        private System.Windows.Forms.TextBox txtPlunge;
        private System.Windows.Forms.TextBox txtPropY;
        private System.Windows.Forms.TextBox txtPropX;
        private System.Windows.Forms.ComboBox cboTabbing;
        private System.Windows.Forms.TextBox txtPropZ;
        private System.Windows.Forms.ComboBox cboPlunge;
        private System.Windows.Forms.ComboBox cboOffset;
        private System.Windows.Forms.Label label14;
    }
}