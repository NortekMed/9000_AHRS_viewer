﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

using nortekmed.ahrs;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AHRS_viewer
{
    public partial class Form1 : Form
    {
        private Viewer viewer;
        List<string> listFilePath;

        string last_directory_path;
        string last_file_path;

        public Form1()
        {
            InitializeComponent();

            FS_select.Items.Add("4.0");
            FS_select.Items.Add("5.34");
            FS_select.SelectedIndex = 0;


            for( int i = 0; i < 301; i++)
            {
                ahrs_corr.Items.Add((30.0 - i / 10.0).ToString("0.0"));
            }
            ahrs_corr.SelectedIndex = 0;

            listFilePath = new List<string>();

            viewer = new Viewer();
            viewer.ahrs_wave.register_result_evt += new nortekmed.ahrs.AHRSWaves.register_result(Register_wave_result);



        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if ( last_file_path == "")
                    openFileDialog.InitialDirectory = "E:\\NortekMed_DEV\\20 - Projects\\1741_DUNK_EDF\\recup_apres_test_bfhf_05-10-2020\\Logs";//E:\NortekMed_DEV\20 - Projects\1741_DUNK_EDF\recup_apres_test_bfhf_05 - 10 - 2020\Logs
                //else
                //{
                //    last_file_path = 
                //}
                
                openFileDialog.Filter = "csv files (*.txt)|*.csv|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    

                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    last_file_path = System.IO.Path.GetDirectoryName(filePath);

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {

                        //fileContent = reader.ReadToEnd();
                        List<string> arguments = new List<string>();
                        string line = "";
                        while ((line = reader.ReadLine()) != null)
                        {
                            if ((line.Length != 0) && !line.Contains("//"))
                                arguments.Add(line);
                        }

                        ProcessFile(arguments);

                        //string[] argTab = arguments.ToArray();
                        //string[] accel_line = arguments.ToArray();

                        //AccAngularRateMagVectorOrientationMatrix sample;
                        //viewer.ahrs_wave.Fs = 4.0;
                        //viewer.ahrs_wave.nsamples = 4096;
                        //viewer.ahrs_wave.samples = new AccAngularRateMagVectorOrientationMatrix[4096];


                        //string value = "";
                        //float[] tmpvect = new float[3];
                        //double time = 0;
                        //for (int i = 0; i < 4096; i++)
                        //{
                        //    for (int j = 0; j < 3; j++)
                        //    {
                        //        accel_line[j] = argTab[i].Split(';')[j];

                        //        float.TryParse(accel_line[j], out tmpvect[j]);
                        //    }

                        //    sample = new AccAngularRateMagVectorOrientationMatrix();
                        //    sample.accelerations = tmpvect;
                        //    time += (1 / viewer.ahrs_wave.Fs) * 1000;
                        //    sample.timer = time;
                        //    sample.angularrates = new float[3];
                        //    sample.mag = new float[3];
                        //    sample.orientationmatrix = new float[3, 3];

                        //    //viewer.ahrs_wave.samples[i] = new AccAngularRateMagVectorOrientationMatrix();
                        //    viewer.ahrs_wave.samples[i] = sample.Clone();
                        //}



                        //viewer.ahrs_wave.StartProcessingThread();

                        /////////////////////////////////////////////////////////////////////
                        ///



                    }



                }
            }
        }

        private void ProcessFile(List<string> argTab)
        {
            //fileContent = reader.ReadToEnd();
            //List<string> arguments = new List<string>();
            //string line = "";
            //while ((line = reader.ReadLine()) != null)
            //{
            //    if ((line.Length != 0) && !line.Contains("//"))
            //        arguments.Add(line);
            //}

            //string[] argTab = arguments.ToArray();
            /* string[] accel_line;*/// = arguments.ToArray();

            double.TryParse(FS_select.Items[FS_select.SelectedIndex].ToString(), out viewer.ahrs_wave.Fs);
            double.TryParse(ahrs_corr.Items[ahrs_corr.SelectedIndex].ToString(), out viewer.ahrs_wave.correction_value);



            //string val = FS_select.Invoke((MethodInvoker)(() => FS_select.Items[FS_select.SelectedIndex].ToString()));

            AccAngularRateMagVectorOrientationMatrix sample;
            //viewer.ahrs_wave.Fs = 5.3;
            viewer.ahrs_wave.nsamples = 4096;
            viewer.ahrs_wave.samples = new AccAngularRateMagVectorOrientationMatrix[viewer.ahrs_wave.nsamples];
            string[] accel_line = new string[viewer.ahrs_wave.nsamples];

            //string value = "";
            float[] tmpvect = new float[3];
            double time = 0;
            for (int i = 0; i < viewer.ahrs_wave.nsamples; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    accel_line[j] = argTab[i].Split(';')[j];

                    float.TryParse(accel_line[j], out tmpvect[j]);
                }

                sample = new AccAngularRateMagVectorOrientationMatrix();
                sample.accelerations = tmpvect;
                time += (1 / viewer.ahrs_wave.Fs);
                sample.timer = time;
                sample.angularrates = new float[3];
                sample.mag = new float[3];
                sample.orientationmatrix = new float[3, 3];

                //viewer.ahrs_wave.samples[i] = new AccAngularRateMagVectorOrientationMatrix();
                viewer.ahrs_wave.samples[i] = sample.Clone();
            }

            

            viewer.ahrs_wave.StartProcessingThread();

            //Thread.Sleep(5000);

            


        }

        private void Register_wave_result(WavesProcessingResult res)
        {
            //if (this.InvokeRequired)
            //{
            //    this.BeginInvoke(new(Register_wave_result), new object[] { res });
            //    return;
            //}
            try
            {
                //viewer.wave_results.Add(res);

                label_Hm0.Invoke((MethodInvoker)(() => label_Hm0.Text = "Hm0: " + res.Hm0.ToString("0.00")));
                label_Tp.Invoke((MethodInvoker)(() => label_Tp.Text = "Tp: " + res.Tp.ToString("0.00")));
                label_TM02.Invoke((MethodInvoker)(() => label_TM02.Text = "TM02: " + res.T02.ToString("0.00")));
                label_H3.Invoke((MethodInvoker)(() => label_H3.Text = "H3: " + res.Htier.ToString("0.00")));
                label_Tz.Invoke((MethodInvoker)(() => label_Tz.Text = "Tz: " + res.Tz.ToString("0.00")));


                float[] accel = new float[viewer.ahrs_wave.nsamples];
                double[] timing = new double[viewer.ahrs_wave.nsamples];
                for (int i = 0; i < viewer.ahrs_wave.nsamples; i++)
                {
                    accel[i] = viewer.ahrs_wave.samples[i].accelerations[2];
                    timing[i] = viewer.ahrs_wave.samples[i].timer;
                }

                fastLine1.Clear();
                fastLine1.Add(timing, accel);


                double deltaf = viewer.ahrs_wave.Fs / viewer.ahrs_wave.nsamples;
                for (int i = 0; i < viewer.ahrs_wave.nsamples; i++)
                {
                    timing[i] = i * deltaf;
                }


                fastLine2.Clear();
                fastLine2.Add(timing, viewer.ahrs_wave.t2_ahrs_correction);
                fastLine2.Title = "AHRS correction: 10^(corr/10)";
                fastLine2.Legend.Visible = true;

                fastLine3.Clear();
                fastLine3.Add(timing, viewer.ahrs_wave.raw_czz);
                fastLine3.Title = "Czz raw";
                fastLine3.Legend.Visible = true;

                fastLine4.Clear();
                fastLine4.Add(timing, viewer.ahrs_wave.omega_czz);
                fastLine4.Title = "Czz omega";
                fastLine4.Legend.Visible = true;

                fastLine5.Clear();
                fastLine5.Add(timing, viewer.ahrs_wave.final_czz);
                fastLine5.Title = "Czz: omega + ahrs correction";
                fastLine5.Legend.Visible = true;

                tChart1.Legend.Visible = true;

            }
            catch (Exception ex) { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listFileName.Items.Clear();
            listFilePath.Clear();

            using (var fbd = new FolderBrowserDialog())
            {
                if ( last_directory_path != "")
                    fbd.SelectedPath = last_directory_path;

                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    last_directory_path = fbd.SelectedPath;

                    string[] files = Directory.GetFiles(fbd.SelectedPath);
                   
                    for ( int i = 0; i < files.Length; i++)
                    {
                        string file_path = files[i];

                        if (file_path.Contains("Accel"))
                        {
                            var fileStream = files[i];

                            listFileName.Items.Add(files[i].Split('\\').Last());
                            listFilePath.Add(files[i]);

                        }
                    }
                }
            }
        }

        


        private void tChart5_DClick(object sender, EventArgs e)
        {
            tChart5.ShowEditor();
        }

        private void tChart1_DClick(object sender, EventArgs e)
        {
            tChart1.ShowEditor();
        }




        private void File_Selected(object sender, MouseEventArgs e)
        {
            // Get the currently selected item in the ListBox.
            string curItem = listFileName.SelectedItem.ToString();

            int indexitem = listFileName.SelectedIndex;
            string file_to_read = listFilePath[indexitem];

            using (StreamReader reader = new StreamReader(file_to_read))
            {

                //fileContent = reader.ReadToEnd();
                List<string> arguments = new List<string>();
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    if ((line.Length != 0) && !line.Contains("//"))
                        arguments.Add(line);
                }

                ProcessFile(arguments);
            }



            // Find the string in ListBox2.
            int index = listFileName.FindString(curItem);
            // If the item was not found in ListBox 2 display a message box, otherwise select it in ListBox2.
            if (index == -1)
                MessageBox.Show("Item is not available in ListBox2");
            else
                listFileName.SetSelected(index, true);
        }
    }
}
