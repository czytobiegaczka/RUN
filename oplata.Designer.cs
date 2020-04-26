namespace RUN
{
    partial class Oplata
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblOplataNazwa = new System.Windows.Forms.Label();
            this.lblOplata = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lblOplataNazwa, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblOplata, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(167, 228);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblOplataNazwa
            // 
            this.lblOplataNazwa.AutoSize = true;
            this.lblOplataNazwa.BackColor = System.Drawing.Color.MediumTurquoise;
            this.lblOplataNazwa.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblOplataNazwa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOplataNazwa.Font = new System.Drawing.Font("Courier New", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblOplataNazwa.Location = new System.Drawing.Point(3, 124);
            this.lblOplataNazwa.Name = "lblOplataNazwa";
            this.lblOplataNazwa.Size = new System.Drawing.Size(161, 104);
            this.lblOplataNazwa.TabIndex = 0;
            this.lblOplataNazwa.Text = "opłata\r\nstartowa";
            this.lblOplataNazwa.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOplata
            // 
            this.lblOplata.AutoSize = true;
            this.lblOplata.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblOplata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOplata.Font = new System.Drawing.Font("Courier New", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblOplata.Location = new System.Drawing.Point(3, 20);
            this.lblOplata.Name = "lblOplata";
            this.lblOplata.Size = new System.Drawing.Size(161, 104);
            this.lblOplata.TabIndex = 1;
            this.lblOplata.Text = "50,00 zł";
            this.lblOplata.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Oplata
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(167, 228);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Oplata";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Oplata_FormClosed);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblOplataNazwa;
        public System.Windows.Forms.Label lblOplata;
    }
}