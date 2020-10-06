﻿namespace AHRS_viewer
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.FS_select = new System.Windows.Forms.ComboBox();
            this.listFileName = new System.Windows.Forms.ListBox();
            this.tChart1 = new Steema.TeeChart.TChart();
            this.fastLine2 = new Steema.TeeChart.Styles.FastLine();
            this.fastLine3 = new Steema.TeeChart.Styles.FastLine();
            this.fastLine4 = new Steema.TeeChart.Styles.FastLine();
            this.fastLine5 = new Steema.TeeChart.Styles.FastLine();
            this.tChart5 = new Steema.TeeChart.TChart();
            this.fastLine1 = new Steema.TeeChart.Styles.FastLine();
            this.label_Hm0 = new System.Windows.Forms.Label();
            this.label_Tp = new System.Windows.Forms.Label();
            this.label_TM02 = new System.Windows.Forms.Label();
            this.label_H3 = new System.Windows.Forms.Label();
            this.label_Tz = new System.Windows.Forms.Label();
            this.ahrs_corr = new System.Windows.Forms.ComboBox();
            this.label_corr = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label_corr);
            this.panel1.Controls.Add(this.ahrs_corr);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.FS_select);
            this.panel1.Location = new System.Drawing.Point(14, 317);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(220, 76);
            this.panel1.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(118, 28);
            this.button2.TabIndex = 9;
            this.button2.Text = "select directory";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(140, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 29);
            this.button1.TabIndex = 8;
            this.button1.Text = "open file";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Fs";
            // 
            // FS_select
            // 
            this.FS_select.FormattingEnabled = true;
            this.FS_select.Location = new System.Drawing.Point(76, 10);
            this.FS_select.Name = "FS_select";
            this.FS_select.Size = new System.Drawing.Size(56, 21);
            this.FS_select.TabIndex = 0;
            // 
            // listFileName
            // 
            this.listFileName.FormattingEnabled = true;
            this.listFileName.Location = new System.Drawing.Point(14, 47);
            this.listFileName.Name = "listFileName";
            this.listFileName.ScrollAlwaysVisible = true;
            this.listFileName.Size = new System.Drawing.Size(221, 264);
            this.listFileName.TabIndex = 0;
            this.listFileName.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.File_Selected);
            // 
            // tChart1
            // 
            // 
            // 
            // 
            this.tChart1.Aspect.View3D = false;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Bottom.Title.Caption = "seconde";
            this.tChart1.Axes.Bottom.Title.Lines = new string[] {
        "seconde"};
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Left.Title.Caption = "Amp";
            this.tChart1.Axes.Left.Title.Lines = new string[] {
        "Amp"};
            // 
            // 
            // 
            this.tChart1.Header.Visible = false;
            // 
            // 
            // 
            this.tChart1.Legend.Visible = false;
            this.tChart1.Location = new System.Drawing.Point(238, 309);
            this.tChart1.Name = "tChart1";
            // 
            // 
            // 
            this.tChart1.Panel.MarginBottom = 0D;
            this.tChart1.Panel.MarginLeft = 1D;
            this.tChart1.Panel.MarginRight = 0D;
            this.tChart1.Panel.MarginTop = 2D;
            this.tChart1.Series.Add(this.fastLine2);
            this.tChart1.Series.Add(this.fastLine3);
            this.tChart1.Series.Add(this.fastLine4);
            this.tChart1.Series.Add(this.fastLine5);
            this.tChart1.Size = new System.Drawing.Size(984, 304);
            this.tChart1.TabIndex = 3;
            this.tChart1.DoubleClick += new System.EventHandler(this.tChart1_DClick);
            // 
            // fastLine2
            // 
            this.fastLine2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(102)))), ((int)(((byte)(163)))));
            this.fastLine2.ColorEach = false;
            // 
            // 
            // 
            this.fastLine2.LinePen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(102)))), ((int)(((byte)(163)))));
            this.fastLine2.OriginalCursor = null;
            this.fastLine2.Title = "fastLine1";
            this.fastLine2.TreatNulls = Steema.TeeChart.Styles.TreatNullsStyle.Ignore;
            // 
            // 
            // 
            this.fastLine2.XValues.DataMember = "X";
            this.fastLine2.XValues.Order = Steema.TeeChart.Styles.ValueListOrder.Ascending;
            // 
            // 
            // 
            this.fastLine2.YValues.DataMember = "Y";
            // 
            // fastLine3
            // 
            this.fastLine3.Color = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(156)))), ((int)(((byte)(53)))));
            this.fastLine3.ColorEach = false;
            // 
            // 
            // 
            this.fastLine3.LinePen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(156)))), ((int)(((byte)(53)))));
            this.fastLine3.OriginalCursor = null;
            this.fastLine3.Title = "fastLine2";
            this.fastLine3.TreatNulls = Steema.TeeChart.Styles.TreatNullsStyle.Ignore;
            // 
            // 
            // 
            this.fastLine3.XValues.DataMember = "X";
            this.fastLine3.XValues.Order = Steema.TeeChart.Styles.ValueListOrder.Ascending;
            // 
            // 
            // 
            this.fastLine3.YValues.DataMember = "Y";
            // 
            // fastLine4
            // 
            this.fastLine4.Color = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(76)))), ((int)(((byte)(20)))));
            this.fastLine4.ColorEach = false;
            // 
            // 
            // 
            this.fastLine4.LinePen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(76)))), ((int)(((byte)(20)))));
            this.fastLine4.OriginalCursor = null;
            this.fastLine4.Title = "fastLine3";
            this.fastLine4.TreatNulls = Steema.TeeChart.Styles.TreatNullsStyle.Ignore;
            // 
            // 
            // 
            this.fastLine4.XValues.DataMember = "X";
            this.fastLine4.XValues.Order = Steema.TeeChart.Styles.ValueListOrder.Ascending;
            // 
            // 
            // 
            this.fastLine4.YValues.DataMember = "Y";
            // 
            // fastLine5
            // 
            this.fastLine5.Color = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(151)))), ((int)(((byte)(168)))));
            this.fastLine5.ColorEach = false;
            // 
            // 
            // 
            this.fastLine5.LinePen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(151)))), ((int)(((byte)(168)))));
            this.fastLine5.OriginalCursor = null;
            this.fastLine5.Title = "fastLine4";
            this.fastLine5.TreatNulls = Steema.TeeChart.Styles.TreatNullsStyle.Ignore;
            // 
            // 
            // 
            this.fastLine5.XValues.DataMember = "X";
            this.fastLine5.XValues.Order = Steema.TeeChart.Styles.ValueListOrder.Ascending;
            // 
            // 
            // 
            this.fastLine5.YValues.DataMember = "Y";
            // 
            // tChart5
            // 
            // 
            // 
            // 
            this.tChart5.Aspect.View3D = false;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart5.Axes.Bottom.Title.Caption = "seconde";
            this.tChart5.Axes.Bottom.Title.Lines = new string[] {
        "seconde"};
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart5.Axes.Left.Title.Caption = "m/s^2";
            this.tChart5.Axes.Left.Title.Lines = new string[] {
        "m/s^2"};
            // 
            // 
            // 
            this.tChart5.Header.Visible = false;
            // 
            // 
            // 
            this.tChart5.Legend.Visible = false;
            this.tChart5.Location = new System.Drawing.Point(238, 3);
            this.tChart5.Name = "tChart5";
            // 
            // 
            // 
            this.tChart5.Panel.MarginBottom = 0D;
            this.tChart5.Panel.MarginLeft = 1D;
            this.tChart5.Panel.MarginRight = 1D;
            this.tChart5.Panel.MarginTop = 2D;
            this.tChart5.Series.Add(this.fastLine1);
            this.tChart5.Size = new System.Drawing.Size(984, 300);
            this.tChart5.TabIndex = 2;
            this.tChart5.DoubleClick += new System.EventHandler(this.tChart5_DClick);
            // 
            // fastLine1
            // 
            this.fastLine1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(102)))), ((int)(((byte)(163)))));
            this.fastLine1.ColorEach = false;
            // 
            // 
            // 
            this.fastLine1.LinePen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(102)))), ((int)(((byte)(163)))));
            this.fastLine1.OriginalCursor = null;
            this.fastLine1.Title = "fastLine1";
            this.fastLine1.TreatNulls = Steema.TeeChart.Styles.TreatNullsStyle.Ignore;
            // 
            // 
            // 
            this.fastLine1.XValues.DataMember = "X";
            this.fastLine1.XValues.Order = Steema.TeeChart.Styles.ValueListOrder.Ascending;
            // 
            // 
            // 
            this.fastLine1.YValues.DataMember = "Y";
            // 
            // label_Hm0
            // 
            this.label_Hm0.AutoSize = true;
            this.label_Hm0.Location = new System.Drawing.Point(11, 60);
            this.label_Hm0.Name = "label_Hm0";
            this.label_Hm0.Size = new System.Drawing.Size(29, 13);
            this.label_Hm0.TabIndex = 4;
            this.label_Hm0.Text = "Hm0";
            // 
            // label_Tp
            // 
            this.label_Tp.AutoSize = true;
            this.label_Tp.Location = new System.Drawing.Point(11, 73);
            this.label_Tp.Name = "label_Tp";
            this.label_Tp.Size = new System.Drawing.Size(20, 13);
            this.label_Tp.TabIndex = 5;
            this.label_Tp.Text = "Tp";
            // 
            // label_TM02
            // 
            this.label_TM02.AutoSize = true;
            this.label_TM02.Location = new System.Drawing.Point(11, 86);
            this.label_TM02.Name = "label_TM02";
            this.label_TM02.Size = new System.Drawing.Size(35, 13);
            this.label_TM02.TabIndex = 6;
            this.label_TM02.Text = "TM02";
            // 
            // label_H3
            // 
            this.label_H3.AutoSize = true;
            this.label_H3.Location = new System.Drawing.Point(11, 23);
            this.label_H3.Name = "label_H3";
            this.label_H3.Size = new System.Drawing.Size(21, 13);
            this.label_H3.TabIndex = 7;
            this.label_H3.Text = "H3";
            // 
            // label_Tz
            // 
            this.label_Tz.AutoSize = true;
            this.label_Tz.Location = new System.Drawing.Point(13, 39);
            this.label_Tz.Name = "label_Tz";
            this.label_Tz.Size = new System.Drawing.Size(19, 13);
            this.label_Tz.TabIndex = 8;
            this.label_Tz.Text = "Tz";
            // 
            // ahrs_corr
            // 
            this.ahrs_corr.FormattingEnabled = true;
            this.ahrs_corr.Location = new System.Drawing.Point(76, 44);
            this.ahrs_corr.Name = "ahrs_corr";
            this.ahrs_corr.Size = new System.Drawing.Size(56, 21);
            this.ahrs_corr.TabIndex = 10;
            // 
            // label_corr
            // 
            this.label_corr.AutoSize = true;
            this.label_corr.Location = new System.Drawing.Point(15, 47);
            this.label_corr.Name = "label_corr";
            this.label_corr.Size = new System.Drawing.Size(55, 13);
            this.label_corr.TabIndex = 11;
            this.label_corr.Text = "Correction";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label_Tz);
            this.panel2.Controls.Add(this.label_H3);
            this.panel2.Controls.Add(this.label_TM02);
            this.panel2.Controls.Add(this.label_Tp);
            this.panel2.Controls.Add(this.label_Hm0);
            this.panel2.Location = new System.Drawing.Point(14, 399);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(218, 127);
            this.panel2.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1904, 961);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.tChart5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tChart1);
            this.Controls.Add(this.listFileName);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox FS_select;
        private System.Windows.Forms.ListBox listFileName;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private Steema.TeeChart.TChart tChart1;
        private Steema.TeeChart.TChart tChart5;
        private Steema.TeeChart.Styles.FastLine fastLine2;
        private Steema.TeeChart.Styles.FastLine fastLine3;
        private Steema.TeeChart.Styles.FastLine fastLine4;
        private Steema.TeeChart.Styles.FastLine fastLine1;
        private Steema.TeeChart.Styles.FastLine fastLine5;
        private System.Windows.Forms.Label label_Hm0;
        private System.Windows.Forms.Label label_Tp;
        private System.Windows.Forms.Label label_TM02;
        private System.Windows.Forms.Label label_H3;
        private System.Windows.Forms.Label label_Tz;
        private System.Windows.Forms.Label label_corr;
        private System.Windows.Forms.ComboBox ahrs_corr;
        private System.Windows.Forms.Panel panel2;
    }
}

