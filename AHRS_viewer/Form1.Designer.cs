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
            this.label_middlebin = new System.Windows.Forms.Label();
            this.label_endbin = new System.Windows.Forms.Label();
            this.label_startbin = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ahrs_corr_periode = new System.Windows.Forms.ComboBox();
            this.label_corr = new System.Windows.Forms.Label();
            this.ahrs_corr = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.FS_select = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.listFileName = new System.Windows.Forms.ListBox();
            this.tChart1 = new Steema.TeeChart.TChart();
            this.fastLine2 = new Steema.TeeChart.Styles.FastLine();
            this.fastLine3 = new Steema.TeeChart.Styles.FastLine();
            this.fastLine4 = new Steema.TeeChart.Styles.FastLine();
            this.fastLine5 = new Steema.TeeChart.Styles.FastLine();
            this.fastLine6 = new Steema.TeeChart.Styles.FastLine();
            this.label_Hm0 = new System.Windows.Forms.Label();
            this.label_Tp = new System.Windows.Forms.Label();
            this.label_TM02 = new System.Windows.Forms.Label();
            this.label_H3 = new System.Windows.Forms.Label();
            this.label_Tz = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label_sprd2 = new System.Windows.Forms.Label();
            this.label_sprd = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBox_sprd_val = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tChart5 = new Steema.TeeChart.TChart();
            this.fastLine1 = new Steema.TeeChart.Styles.FastLine();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.read_spectre = new System.Windows.Forms.Button();
            this.b_write_huv = new System.Windows.Forms.Button();
            this.tChart2 = new Steema.TeeChart.TChart();
            this.isoSurface1 = new Steema.TeeChart.Styles.IsoSurface();
            this.colorGrid1 = new Steema.TeeChart.Styles.ColorGrid();
            this.extraLegend1 = new Steema.TeeChart.Tools.ExtraLegend();
            this.gridTranspose1 = new Steema.TeeChart.Tools.GridTranspose();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label_middlebin);
            this.panel1.Controls.Add(this.label_endbin);
            this.panel1.Controls.Add(this.label_startbin);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.ahrs_corr_periode);
            this.panel1.Controls.Add(this.label_corr);
            this.panel1.Controls.Add(this.ahrs_corr);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.FS_select);
            this.panel1.Location = new System.Drawing.Point(15, 213);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(230, 163);
            this.panel1.TabIndex = 0;
            // 
            // label_middlebin
            // 
            this.label_middlebin.AutoSize = true;
            this.label_middlebin.Location = new System.Drawing.Point(15, 115);
            this.label_middlebin.Name = "label_middlebin";
            this.label_middlebin.Size = new System.Drawing.Size(54, 13);
            this.label_middlebin.TabIndex = 16;
            this.label_middlebin.Text = "middlebin:";
            // 
            // label_endbin
            // 
            this.label_endbin.AutoSize = true;
            this.label_endbin.Location = new System.Drawing.Point(15, 128);
            this.label_endbin.Name = "label_endbin";
            this.label_endbin.Size = new System.Drawing.Size(42, 13);
            this.label_endbin.TabIndex = 15;
            this.label_endbin.Text = "endbin:";
            // 
            // label_startbin
            // 
            this.label_startbin.AutoSize = true;
            this.label_startbin.Location = new System.Drawing.Point(15, 102);
            this.label_startbin.Name = "label_startbin";
            this.label_startbin.Size = new System.Drawing.Size(44, 13);
            this.label_startbin.TabIndex = 14;
            this.label_startbin.Text = "startbin:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "CorrPeriod";
            // 
            // ahrs_corr_periode
            // 
            this.ahrs_corr_periode.FormattingEnabled = true;
            this.ahrs_corr_periode.Location = new System.Drawing.Point(76, 71);
            this.ahrs_corr_periode.Name = "ahrs_corr_periode";
            this.ahrs_corr_periode.Size = new System.Drawing.Size(56, 21);
            this.ahrs_corr_periode.TabIndex = 12;
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
            // ahrs_corr
            // 
            this.ahrs_corr.FormattingEnabled = true;
            this.ahrs_corr.Location = new System.Drawing.Point(76, 44);
            this.ahrs_corr.Name = "ahrs_corr";
            this.ahrs_corr.Size = new System.Drawing.Size(56, 21);
            this.ahrs_corr.TabIndex = 10;
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
            this.button1.Location = new System.Drawing.Point(151, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 29);
            this.button1.TabIndex = 8;
            this.button1.Text = "open file";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // listFileName
            // 
            this.listFileName.FormattingEnabled = true;
            this.listFileName.Location = new System.Drawing.Point(14, 47);
            this.listFileName.Name = "listFileName";
            this.listFileName.ScrollAlwaysVisible = true;
            this.listFileName.Size = new System.Drawing.Size(231, 160);
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
            this.tChart1.Location = new System.Drawing.Point(8, 316);
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
            this.tChart1.Series.Add(this.fastLine6);
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
            // fastLine6
            // 
            this.fastLine6.Color = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(64)))), ((int)(((byte)(107)))));
            this.fastLine6.ColorEach = false;
            // 
            // 
            // 
            this.fastLine6.LinePen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(64)))), ((int)(((byte)(107)))));
            this.fastLine6.OriginalCursor = null;
            this.fastLine6.Title = "fastLine5";
            this.fastLine6.TreatNulls = Steema.TeeChart.Styles.TreatNullsStyle.Ignore;
            // 
            // 
            // 
            this.fastLine6.XValues.DataMember = "X";
            this.fastLine6.XValues.Order = Steema.TeeChart.Styles.ValueListOrder.Ascending;
            // 
            // 
            // 
            this.fastLine6.YValues.DataMember = "Y";
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
            // panel2
            // 
            this.panel2.Controls.Add(this.label_sprd2);
            this.panel2.Controls.Add(this.label_sprd);
            this.panel2.Controls.Add(this.label_Tz);
            this.panel2.Controls.Add(this.label_H3);
            this.panel2.Controls.Add(this.label_TM02);
            this.panel2.Controls.Add(this.label_Tp);
            this.panel2.Controls.Add(this.label_Hm0);
            this.panel2.Location = new System.Drawing.Point(3, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(218, 151);
            this.panel2.TabIndex = 9;
            // 
            // label_sprd2
            // 
            this.label_sprd2.AutoSize = true;
            this.label_sprd2.Location = new System.Drawing.Point(15, 127);
            this.label_sprd2.Name = "label_sprd2";
            this.label_sprd2.Size = new System.Drawing.Size(43, 13);
            this.label_sprd2.TabIndex = 10;
            this.label_sprd2.Text = "SPRD2";
            this.label_sprd2.Click += new System.EventHandler(this.label4_Click);
            // 
            // label_sprd
            // 
            this.label_sprd.AutoSize = true;
            this.label_sprd.Location = new System.Drawing.Point(13, 114);
            this.label_sprd.Name = "label_sprd";
            this.label_sprd.Size = new System.Drawing.Size(37, 13);
            this.label_sprd.TabIndex = 9;
            this.label_sprd.Text = "SPRD";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(15, 382);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(234, 231);
            this.tabControl1.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(226, 205);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Waves";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.textBox_sprd_val);
            this.tabPage2.Controls.Add(this.textBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(226, 205);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Spread";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBox_sprd_val
            // 
            this.textBox_sprd_val.Location = new System.Drawing.Point(0, 91);
            this.textBox_sprd_val.Multiline = true;
            this.textBox_sprd_val.Name = "textBox_sprd_val";
            this.textBox_sprd_val.Size = new System.Drawing.Size(225, 113);
            this.textBox_sprd_val.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(226, 89);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "spread formula: \r\nsprd = sqrt(2-2M)    ( *180/pi for °)\r\nM=sqrt( C/ D )\r\nwith C =" +
    " qzx[iTp]^2 + qzy[iTp]^2\r\n         D = czz_raw[iTp] * ( cxx[iTp] + cyy[iTp] )\r\n";
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Location = new System.Drawing.Point(248, 3);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1013, 639);
            this.tabControl2.TabIndex = 11;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tChart5);
            this.tabPage3.Controls.Add(this.tChart1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1005, 613);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
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
            this.tChart5.Location = new System.Drawing.Point(6, 10);
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
            this.tChart5.TabIndex = 4;
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
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.read_spectre);
            this.tabPage4.Controls.Add(this.b_write_huv);
            this.tabPage4.Controls.Add(this.tChart2);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1005, 613);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // read_spectre
            // 
            this.read_spectre.Location = new System.Drawing.Point(131, 588);
            this.read_spectre.Name = "read_spectre";
            this.read_spectre.Size = new System.Drawing.Size(86, 19);
            this.read_spectre.TabIndex = 5;
            this.read_spectre.Text = "Read spectre";
            this.read_spectre.UseVisualStyleBackColor = true;
            this.read_spectre.Click += new System.EventHandler(this.Read_Spectre);
            // 
            // b_write_huv
            // 
            this.b_write_huv.Enabled = false;
            this.b_write_huv.Location = new System.Drawing.Point(16, 588);
            this.b_write_huv.Name = "b_write_huv";
            this.b_write_huv.Size = new System.Drawing.Size(86, 19);
            this.b_write_huv.TabIndex = 4;
            this.b_write_huv.Text = "PUV";
            this.b_write_huv.UseVisualStyleBackColor = true;
            this.b_write_huv.Click += new System.EventHandler(this.write_huv);
            // 
            // tChart2
            // 
            // 
            // 
            // 
            this.tChart2.Aspect.ColorPaletteIndex = 0;
            this.tChart2.Aspect.View3D = false;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart2.Axes.Bottom.Title.Caption = "seconde";
            this.tChart2.Axes.Bottom.Title.Lines = new string[] {
        "seconde"};
            // 
            // 
            // 
            this.tChart2.Axes.Depth.Visible = true;
            // 
            // 
            // 
            this.tChart2.Axes.Left.MinimumRound = true;
            // 
            // 
            // 
            this.tChart2.Axes.Left.Title.Caption = "°";
            this.tChart2.Axes.Left.Title.Lines = new string[] {
        "°"};
            // 
            // 
            // 
            this.tChart2.Header.Visible = false;
            // 
            // 
            // 
            this.tChart2.Legend.Visible = false;
            this.tChart2.Location = new System.Drawing.Point(16, 22);
            this.tChart2.Name = "tChart2";
            // 
            // 
            // 
            this.tChart2.Panel.BorderRound = 10;
            this.tChart2.Panel.MarginBottom = 0D;
            this.tChart2.Panel.MarginLeft = 1D;
            this.tChart2.Panel.MarginRight = 8D;
            this.tChart2.Panel.MarginTop = 2D;
            this.tChart2.Series.Add(this.isoSurface1);
            this.tChart2.Series.Add(this.colorGrid1);
            this.tChart2.Size = new System.Drawing.Size(983, 524);
            this.tChart2.TabIndex = 3;
            this.tChart2.Tools.Add(this.extraLegend1);
            this.tChart2.Click += new System.EventHandler(this.tChart2_Click);
            this.tChart2.DoubleClick += new System.EventHandler(this.tChart2_DClick);
            // 
            // isoSurface1
            // 
            // 
            // 
            // 
            this.isoSurface1.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(117)))), ((int)(((byte)(138)))));
            this.isoSurface1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.isoSurface1.ColorEach = false;
            this.isoSurface1.NumXValues = 30;
            this.isoSurface1.NumZValues = 30;
            this.isoSurface1.OriginalCursor = null;
            this.isoSurface1.PaletteMin = 0D;
            this.isoSurface1.PaletteStep = 0D;
            this.isoSurface1.PaletteSteps = 250;
            this.isoSurface1.PaletteStyle = Steema.TeeChart.Styles.PaletteStyles.Rainbow;
            this.isoSurface1.SeriesData = resources.GetString("isoSurface1.SeriesData");
            this.isoSurface1.TimesZOrder = 5;
            this.isoSurface1.Title = "iso-Surface1";
            this.isoSurface1.UseColorRange = false;
            this.isoSurface1.UsePalette = true;
            this.isoSurface1.UseYPosition = false;
            this.isoSurface1.Visible = false;
            // 
            // 
            // 
            this.isoSurface1.XValues.DataMember = "X";
            this.isoSurface1.YPosition = 0D;
            // 
            // 
            // 
            this.isoSurface1.YValues.DataMember = "Y";
            // 
            // 
            // 
            this.isoSurface1.ZValues.DataMember = "Z";
            // 
            // colorGrid1
            // 
            // 
            // 
            // 
            this.colorGrid1.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.colorGrid1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.colorGrid1.ColorEach = false;
            // 
            // 
            // 
            this.colorGrid1.Marks.ArrowLength = 0;
            this.colorGrid1.NumXValues = 30;
            this.colorGrid1.NumZValues = 30;
            this.colorGrid1.OriginalCursor = null;
            this.colorGrid1.PaletteMin = 0D;
            this.colorGrid1.PaletteStep = 0D;
            this.colorGrid1.PaletteSteps = 64;
            this.colorGrid1.PaletteStyle = Steema.TeeChart.Styles.PaletteStyles.Rainbow;
            // 
            // 
            // 
            this.colorGrid1.Pen.Visible = false;
            this.colorGrid1.TimesZOrder = 5;
            this.colorGrid1.Title = "colorGrid1";
            this.colorGrid1.UseColorRange = false;
            this.colorGrid1.UsePalette = true;
            this.colorGrid1.XStep = 1;
            // 
            // 
            // 
            this.colorGrid1.XValues.DataMember = "X";
            // 
            // 
            // 
            this.colorGrid1.YValues.DataMember = "Y";
            this.colorGrid1.ZStep = 1;
            // 
            // 
            // 
            this.colorGrid1.ZValues.DataMember = "Z";
            // 
            // extraLegend1
            // 
            this.extraLegend1.Series = this.colorGrid1;
            // 
            // gridTranspose1
            // 
            this.gridTranspose1.Series = this.isoSurface1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1604, 881);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.listFileName);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
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
        private Steema.TeeChart.Styles.FastLine fastLine2;
        private Steema.TeeChart.Styles.FastLine fastLine3;
        private Steema.TeeChart.Styles.FastLine fastLine4;
        private Steema.TeeChart.Styles.FastLine fastLine5;
        private System.Windows.Forms.Label label_Hm0;
        private System.Windows.Forms.Label label_Tp;
        private System.Windows.Forms.Label label_TM02;
        private System.Windows.Forms.Label label_H3;
        private System.Windows.Forms.Label label_Tz;
        private System.Windows.Forms.Label label_corr;
        private System.Windows.Forms.ComboBox ahrs_corr;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ahrs_corr_periode;
        private System.Windows.Forms.Label label_sprd2;
        private System.Windows.Forms.Label label_sprd;
        private System.Windows.Forms.Label label_middlebin;
        private System.Windows.Forms.Label label_endbin;
        private System.Windows.Forms.Label label_startbin;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox textBox_sprd_val;
        private System.Windows.Forms.TextBox textBox1;
        private Steema.TeeChart.Styles.FastLine fastLine6;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private Steema.TeeChart.TChart tChart2;
        private Steema.TeeChart.TChart tChart5;
        private Steema.TeeChart.Styles.FastLine fastLine1;
        private Steema.TeeChart.Styles.IsoSurface isoSurface1;
        private System.Windows.Forms.Button b_write_huv;
        private System.Windows.Forms.Button read_spectre;
        private Steema.TeeChart.Styles.ColorGrid colorGrid1;
        private Steema.TeeChart.Tools.GridTranspose gridTranspose1;
        private Steema.TeeChart.Tools.ExtraLegend extraLegend1;
    }
}

