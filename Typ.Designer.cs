namespace RUN
{
    partial class Typ
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
            this.btnWybierzTyp = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.rdoMaraton = new System.Windows.Forms.RadioButton();
            this.rdoPolmaraton = new System.Windows.Forms.RadioButton();
            this.rdoDziesiec = new System.Windows.Forms.RadioButton();
            this.rdoInne = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btnWybierzTyp, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(262, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnWybierzTyp
            // 
            this.btnWybierzTyp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnWybierzTyp.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnWybierzTyp.Image = global::RUN.Properties.Resources.correct;
            this.btnWybierzTyp.Location = new System.Drawing.Point(3, 318);
            this.btnWybierzTyp.Name = "btnWybierzTyp";
            this.btnWybierzTyp.Size = new System.Drawing.Size(256, 129);
            this.btnWybierzTyp.TabIndex = 0;
            this.btnWybierzTyp.UseVisualStyleBackColor = true;
            this.btnWybierzTyp.Click += new System.EventHandler(this.btnWybierzTyp_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel2.Controls.Add(this.rdoMaraton, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.rdoPolmaraton, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.rdoDziesiec, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.rdoInne, 1, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 93);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(256, 219);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // rdoMaraton
            // 
            this.rdoMaraton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rdoMaraton.AutoSize = true;
            this.rdoMaraton.Font = new System.Drawing.Font("Courier New", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rdoMaraton.Location = new System.Drawing.Point(54, 13);
            this.rdoMaraton.Name = "rdoMaraton";
            this.rdoMaraton.Size = new System.Drawing.Size(112, 27);
            this.rdoMaraton.TabIndex = 0;
            this.rdoMaraton.TabStop = true;
            this.rdoMaraton.Text = "maraton";
            this.rdoMaraton.UseVisualStyleBackColor = true;
            this.rdoMaraton.CheckedChanged += new System.EventHandler(this.rdoMaraton_CheckedChanged);
            // 
            // rdoPolmaraton
            // 
            this.rdoPolmaraton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rdoPolmaraton.AutoSize = true;
            this.rdoPolmaraton.Font = new System.Drawing.Font("Courier New", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rdoPolmaraton.Location = new System.Drawing.Point(54, 67);
            this.rdoPolmaraton.Name = "rdoPolmaraton";
            this.rdoPolmaraton.Size = new System.Drawing.Size(148, 27);
            this.rdoPolmaraton.TabIndex = 1;
            this.rdoPolmaraton.TabStop = true;
            this.rdoPolmaraton.Text = "półmaraton";
            this.rdoPolmaraton.UseVisualStyleBackColor = true;
            this.rdoPolmaraton.CheckedChanged += new System.EventHandler(this.rdoPolmaraton_CheckedChanged);
            // 
            // rdoDziesiec
            // 
            this.rdoDziesiec.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rdoDziesiec.AutoSize = true;
            this.rdoDziesiec.Font = new System.Drawing.Font("Courier New", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rdoDziesiec.Location = new System.Drawing.Point(54, 121);
            this.rdoDziesiec.Name = "rdoDziesiec";
            this.rdoDziesiec.Size = new System.Drawing.Size(88, 27);
            this.rdoDziesiec.TabIndex = 2;
            this.rdoDziesiec.TabStop = true;
            this.rdoDziesiec.Text = "10 km";
            this.rdoDziesiec.UseVisualStyleBackColor = true;
            this.rdoDziesiec.CheckedChanged += new System.EventHandler(this.rdoDziesiec_CheckedChanged);
            // 
            // rdoInne
            // 
            this.rdoInne.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rdoInne.AutoSize = true;
            this.rdoInne.Font = new System.Drawing.Font("Courier New", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rdoInne.Location = new System.Drawing.Point(54, 177);
            this.rdoInne.Name = "rdoInne";
            this.rdoInne.Size = new System.Drawing.Size(76, 27);
            this.rdoInne.TabIndex = 3;
            this.rdoInne.TabStop = true;
            this.rdoInne.Text = "inne";
            this.rdoInne.UseVisualStyleBackColor = true;
            this.rdoInne.CheckedChanged += new System.EventHandler(this.rdoInne_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Info;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(256, 84);
            this.panel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Courier New", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(27, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "wybierz rodzaj:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Typ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(262, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "Typ";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rodzaj zawodó";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnWybierzTyp;
        private System.Windows.Forms.RadioButton rdoMaraton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        public System.Windows.Forms.RadioButton rdoDziesiec;
        public System.Windows.Forms.RadioButton rdoPolmaraton;
        public System.Windows.Forms.RadioButton rdoInne;
    }
}