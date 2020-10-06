
using System;

using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace nortekmed.ahrs {
    public class AHRSWaves {

        bool disposed = false;

        public object protect;
        public enum NSamples : int { N256 = 256, N512 = 512, N1024 = 1024, N2048 = 2048, N4096 = 4096, N8192 = 8192, N16384 = 16384, N20400 = 20400 }
        public enum State { idle, acquiringdata, processingdata };
        State state;
        AHRSControl control;
        System.Threading.Timer watchdog;
        Thread processingthread = null;
        //Thread processingthread2 = null;
        AHRSControl.AHRSControlEventHandler ahrscontrolevthandler;

        private int samplesindex;
        //private int last_samplesindex;
        private DateTime last_date;
        TimeSpan w_delta_time = TimeSpan.Zero;

        double last_timer = 0;
        double c_timer = 0;

        public byte[] b_data_accel;
        public byte[] b_data_orient;

        public bool log_details = false;
        public bool log_orient = false;
        public bool log_accel = false;
        //public bool log_spread = true;



        public DateTime startDate;

        public int num_centrale = 0;
        //public int nbr_cyle = 0;

        //public int Fs = 4;
        public double Fs = 4.0;

        public AHRSControl Control {
            get { return control; }
            set {
                lock (protect) {

                    if (state != State.idle)
                        stop();

                    ahrscontrolevthandler = new AHRSControl.AHRSControlEventHandler(control_AHRSControlEvent);
                    control = value;
                }


            }
        }
        private AHRSControl.AccAngularRateMagVectorOrientationMatrix[] samples;
        //private AHRSControl.AccAngularRateMagVectorOrientationMatrix[] samples2;

        ~AHRSWaves() {

        }

        public void Dispose() {
            Dispose(true);

            // Suppress finalization.
            GC.SuppressFinalize(this);

        }

        protected virtual void Dispose(bool disposing) {
            if (disposed)
                return;

            if (disposing) {
                control.Dispose();

                watchdog.Dispose();
            }

            disposed = true;
        }

        public AHRSWaves() {
            protect = new object();
            state = State.idle;
            control = null;
            samplesindex = 0;
            watchdog = new Timer(new TimerCallback(TimerElapsed), null, System.Threading.Timeout.Infinite,
                                    System.Threading.Timeout.Infinite);
        }

        public State Status {
            get {
                lock (protect) {
                    return state;
                }
            }
        }

        void TimerElapsed(object f) {
            lock (protect) {

                if (state == State.acquiringdata) {
#if !_DEBUG
                    //then error because it should have been finished !!
                    //watchdog.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                    //fireOnotherThreadEvent(new AHRSWavesEventArgs(AHRSWavesEventArgs.KindOfEvent.acquisitionfailed));
                    //fireOnotherThreadEvent(new AHRSWavesEventArgs(AHRSWavesEventArgs.KindOfEvent.process_fake));

                    //StartProcessing();

                    ///////////////////////////////////////////////////////
                    WavesProcessingResult processingresult = new WavesProcessingResult();
                    processingresult.Hm0 = 0;
                    processingresult.Hm0_bf = 0;
                    processingresult.Hm0_hf = 0;
                    processingresult.Tp = 0;
                    processingresult.Tp_bf = 0;
                    processingresult.Tp_hf = 0;
                    //processingresult.Starting = new DateTime();
                    processingresult.Starting = DateTime.Now;
                    processingresult.missedsamples = -1;
                    processingresult.PowerSpectrum = new double[NBins]; // power spectrum
                    processingresult.T02 = 0; // mean period
                    processingresult.T02_bf = 0; // mean period
                    processingresult.T02_hf = 0; // mean period

                    processingresult.MainDir = new double[NBins];
                    processingresult.DirT02 = 0;
                    processingresult.DirT02_bf = 0;
                    processingresult.DirT02_hf = 0;
                    processingresult.Frequencies = new double[NBins];
                    processingresult.DirTp = 0;
                    processingresult.DirTp_bf = 0;
                    processingresult.DirTp_hf = 0;
                    processingresult.Hmax = 0;
                    processingresult.THmax = 0;

                    processingresult.T01 = 0;
                    processingresult.Htier = 0;
                    processingresult.Ptier = 0;
                    processingresult.Tz = 0;
                    processingresult.Tz_bf = 0;
                    processingresult.Tz_hf = 0;
                    processingresult.NumWaves = 0;
                    processingresult.Sprd = 0;

                    processingresult.calc_sprd = new double[6];
                    for (int i = 0; i < 6; i++) {
                        processingresult.calc_sprd[i] = 0;
                    }

                    processingresult.nsamples = samplesindex;

                    AHRSWavesEventArgs evtf = new AHRSWavesEventArgs(AHRSWavesEventArgs.KindOfEvent.processingfinished);
                    evtf.Processingresult = processingresult;
                    evtf.Received_0x80 = control.count_0x80;
                    evtf.Frame_treated_ok = control.count_complet_frame;
                    evtf.Frame_uncomplet = control.count_uncompletes;
                    evtf.Frame_uncomplet_treated = control.count_treat_uncomplete;
                    evtf.Bad_checksum = control.count_badchecksum_trame;

                    fireOnotherThreadEvent(evtf);


                    try {
                        stop();
                    }
                    catch (Exception) {

                    }
#endif

                }
            }
        }
        private double freqstart;
        private double[] correctionvector;
        double DeltaFreq;
        int NBins;
        public int nsamples;//thierry

        public void startWavesAcquiring(NSamples nsamples, double[] correctionvector, double freqstart, double DeltaFreq, double NBins) {
            lock (protect) {
                if (control == null)
                    throw new AHRSException(AHRSException.ExceptionCause.AHRSControlError);
                if (state != State.idle)
                    throw new AHRSException(AHRSException.ExceptionCause.Alreadyworking);
                if (correctionvector.Length * DeltaFreq + freqstart >= 2) {
                    throw new Exception();
                }
                try {
                    control.DTR_PowerON();    ///only active if centrale is power managed
                    Thread.Sleep(1000);

                    control.set_IDLE_mode(100);                 ///only active if centrale is not power managed
                    control.active_MipParser = true;
                    Thread.Sleep(300);

                    //control.set_IDLE_mode(100);
                    //Thread.Sleep(300);

                    //control.get_BaseRate(500);
                    //Thread.Sleep(300);


                    control.set_IMU_MessageFormat(100);
                    Thread.Sleep(300);

                    control.save_IMU_Settings(100);
                    Thread.Sleep(300);

                    //control.Watchdog_Init();

                    control.count_0x80 = 0;
                    control.count_complet_frame = 0;
                    control.count_uncompletes = 0;
                    control.count_treat_uncomplete = 0;
                    control.count_badchecksum_trame = 0;

                    this.correctionvector = correctionvector;
                    this.DeltaFreq = DeltaFreq;
                    this.NBins = correctionvector.Length;
                    this.freqstart = freqstart;
                    this.nsamples = (int)nsamples;
                    control.AHRSControlEvent -= ahrscontrolevthandler;
                    control.AHRSControlEvent += ahrscontrolevthandler;

                    //int i_tmp = (int)(((int)nsamples / 4)*1000);
                    //control.t_trig_watchdog_mms = (int)(i_tmp / 60) * 1000;

#if !_DEBUG
                    //control.serialport.RtsEnable = true;
                    //Thread.Sleep(1000);
                    startDate = DateTime.Now;
                    last_date = DateTime.Now;       // For first delta time compute
                    control.send_IMU_enable(100);
                    Thread.Sleep(300);
#endif
                    samplesindex = 0;
                    samples = new AHRSControl.AccAngularRateMagVectorOrientationMatrix[(int)nsamples];
                    state = State.acquiringdata;
                    fireOnotherThreadEvent(new AHRSWavesEventArgs(AHRSWavesEventArgs.KindOfEvent.acquisitionstarted));
                    int d_time = (int)((1000 / Fs));
                    watchdog.Change(((int)nsamples + 100) * d_time, System.Threading.Timeout.Infinite);     // Test acquisition 20Hz
                    //watchdog.Change(((int)nsamples + 100) * 50, System.Threading.Timeout.Infinite);     // Test acquisition 20Hz
                    //watchdog.Change(((int)nsamples + 100) * 250, System.Threading.Timeout.Infinite);
                    //watchdog.Change(((int)nsamples + 40) * 250, System.Threading.Timeout.Infinite);
                }
                catch (Exception) {
                    throw new AHRSException(AHRSException.ExceptionCause.AHRSControlError);
                }
            }
        }

        public delegate void AHRSWavesEventHandler(object sender, AHRSWavesEventArgs evt);
        public event AHRSWavesEventHandler AHRSWavesEvent;

        //public delegate void AHRSWavesEventHandler2(object sender, AHRSWavesEventArgs evt);
        //public event AHRSWavesEventHandler2 AHRSWavesEvent2;


        void fireOnotherThreadEvent(AHRSWavesEventArgs evt) {
            ThreadPool.QueueUserWorkItem(new WaitCallback(fireOnOtherThreadEventCallBack), new object[] { this, evt });
        }

        //void fireOnotherThreadEvent2(AHRSWavesEventArgs evt) {
        //    ThreadPool.QueueUserWorkItem(new WaitCallback(fireOnOtherThreadEventCallBack2), new object[] { this, evt });
        //}

        //private static void fireOnOtherThreadEventCallBack(object tosend)
        private void fireOnOtherThreadEventCallBack(object tosend) {
            object[] tosend2 = (object[])tosend;
            AHRSWaves sender = (AHRSWaves)tosend2[0];
            AHRSWavesEventArgs evt = (AHRSWavesEventArgs)tosend2[1];
            if (sender.AHRSWavesEvent != null) {
                sender.AHRSWavesEvent(sender, evt);
            }
        }

        //private void fireOnOtherThreadEventCallBack2(object tosend) {
        //    object[] tosend2 = (object[])tosend;
        //    AHRSWaves sender = (AHRSWaves)tosend2[0];
        //    AHRSWavesEventArgs evt = (AHRSWavesEventArgs)tosend2[1];
        //    if (sender.AHRSWavesEvent != null) {
        //        sender.AHRSWavesEvent(sender, evt);
        //    }
        //}


        void StartProcessingThread() {
            try {
#if !_DEBUG
                control.active_MipParser = false;
                //Thread.Sleep(300);


                control.set_IDLE_mode(100); ///only active if centrale is power managed - sleep on
                Thread.Sleep(50);
                control.DTR_PowerOFF();     ///only active if centrale is power managed - power off
                Thread.Sleep(300);

                control.serialport.RtsEnable = false;
#endif
            }
            catch (Exception) {
                throw new AHRSException(AHRSException.ExceptionCause.SerialError);
            }

            //// For debug, create an image if sample to use it in second thread
            //for ( int i = 0; i < samples.Length; i++) {
            //    samples2[i] = samples[i];
            //}


            // send event saying data are available
            fireOnotherThreadEvent(new AHRSWavesEventArgs(AHRSWavesEventArgs.KindOfEvent.acquisitionfinished));

            // start new thread
            processingthread = new Thread(new ThreadStart(processingThreadWaves));
            processingthread.Name = "WavesThread";
            processingthread.Start();

            // start new thread
            
            
            
            //processingthread2 = new Thread(new ThreadStart(processingThreadWaves2));
            //processingthread2.Name = "WavesThread2";
            //processingthread2.Start();


            state = State.processingdata;
            
            fireOnotherThreadEvent(new AHRSWavesEventArgs(AHRSWavesEventArgs.KindOfEvent.processingwaves));

        }

        void control_AHRSControlEvent(object sender, AHRSControlEventArgs evt) {
            lock (protect) {
                if (state != State.acquiringdata) return;
                if (evt.Kind != AHRSControlEventArgs.KindOfEvent.AccAngularRateMagVectorOrientationMatrix) return;

                //w_delta_time += DateTime.Now - last_date;
                //last_date = DateTime.Now;



                samples[samplesindex++] = evt.Accangularratemagvectororientationmatrix.Clone();

                if (samplesindex == 1)
                    last_timer = samples[samplesindex - 1].timer;

                c_timer += samples[samplesindex - 1].timer;
                //c_timer = samples[samplesindex - 1].timer - c_timer;

                if (samplesindex >= samples.Length) {

                    watchdog.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);

                    // end of acquisition
                    // try stop ahrs
                    //                    try {
                    //#if !_DEBUG
                    //                        control.active_MipParser = false;
                    //                        //Thread.Sleep(300);


                    //                        control.set_IDLE_mode(100); ///only active if centrale is power managed - sleep on
                    //                        Thread.Sleep(50);
                    //                        control.DTR_PowerOFF();     ///only active if centrale is power managed - power off
                    //                        Thread.Sleep(300);

                    //                        control.serialport.RtsEnable = false;
                    //#endif
                    //                    }
                    //                    catch (Exception) {
                    //                        throw new AHRSException(AHRSException.ExceptionCause.SerialError);
                    //                    }

                    StartProcessingThread();

                    //// send event saying data are available
                    //fireOnotherThreadEvent(new AHRSWavesEventArgs(AHRSWavesEventArgs.KindOfEvent.acquisitionfinished));

                    //// start new thread
                    //processingthread = new Thread(new ThreadStart(processWaves));
                    //processingthread.Name = "WavesThread";
                    //processingthread.Start();
                    //state = State.processingdata;
                    //fireOnotherThreadEvent(new AHRSWavesEventArgs(AHRSWavesEventArgs.KindOfEvent.processingwaves));

                } else {
                    //nevt.Delta_time = delta_time / 10;
                    int modulo = 400;
                    if (((samples.Length - samplesindex) % modulo) == 0) {
                        AHRSWavesEventArgs nevt = new AHRSWavesEventArgs(AHRSWavesEventArgs.KindOfEvent.remainingsamples);
                        nevt.RemainingSamples = samples.Length - samplesindex;
                        //var delta = w_delta_time.TotalMilliseconds / modulo;

                        double delta = (c_timer - last_timer) / modulo;
                        last_timer = c_timer;

                        nevt.Delta_time = delta;



                        nevt.Received_0x80 = control.count_0x80;
                        nevt.Frame_treated_ok = control.count_complet_frame;
                        nevt.Frame_uncomplet = control.count_uncompletes;
                        nevt.Frame_uncomplet_treated = control.count_treat_uncomplete;
                        nevt.Bad_checksum = control.count_badchecksum_trame;

                        delta = 0;
                        w_delta_time = TimeSpan.Zero;

                        fireOnotherThreadEvent(nevt);
                    }
                }
            }
        }


        public void stop() {
            lock (protect) {

                if (state != State.processingdata) {
                    state = State.idle;
                    watchdog.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                    if (control == null) return;
                    control.AHRSControlEvent -= ahrscontrolevthandler;
                    try {
#if !_DEBUG
                        control.active_MipParser = false;
                        Thread.Sleep(500);
                        control.set_IDLE_mode(100);
                        Thread.Sleep(50);
                        control.DTR_PowerOFF();

                        Thread.Sleep(300);
                        control.serialport.RtsEnable = false;
#endif

                    }
                    catch (Exception) {
                        throw new AHRSException(AHRSException.ExceptionCause.AHRSControlError);
                    }
                    // temporary send event saying idle
                    fireOnotherThreadEvent(new AHRSWavesEventArgs(AHRSWavesEventArgs.KindOfEvent.idle));
                } else {
                    if (processingthread != null) {
                        try {
                            processingthread.Abort();

                        }
                        catch (Exception) {

                        }

                        try {

                            if (!processingthread.Join(3000)) throw new AHRSException(AHRSException.ExceptionCause.BIGERROR);
                            processingthread = null;
                            state = State.idle;
                            fireOnotherThreadEvent(new AHRSWavesEventArgs(AHRSWavesEventArgs.KindOfEvent.idle));

                        }
                        catch (Exception) {
                            throw new AHRSException(AHRSException.ExceptionCause.BIGERROR);
                        }
                    }
                }
            }
        }


        double[] multiplyByMatrix33(double[,] matrix, float[] vector) {

            if (matrix == null) {
                return null;
            } else {
                double[] m = new double[3];
                m[0] = vector[0] * matrix[0, 0] + vector[1] * matrix[0, 1] + vector[2] * matrix[0, 2];
                m[1] = vector[0] * matrix[1, 0] + vector[1] * matrix[1, 1] + vector[2] * matrix[1, 2];
                m[2] = vector[0] * matrix[2, 0] + vector[1] * matrix[2, 1] + vector[2] * matrix[2, 2];
                return m;
            }

        }

        double[] vectorminusvector(float[] m1, double[] m2) {
            double[] res = new double[3];

            res[0] = (double)m1[0] - (double)m2[0];
            res[1] = (double)m1[1] - (double)m2[1];
            res[2] = (double)m1[2] - (double)m2[2];
            return res;
        }

        double[,] inverseMatrix33(float[,] m) {
            /// | a11 a12 a13 |-1             |   a33a22-a32a23  -(a33a12-a32a13)   a23a12-a22a13  |
            /// | a21 a22 a23 |    =  1/DET * | -(a33a21-a31a23)   a33a11-a31a13  -(a23a11-a21a13) |
            /// | a31 a32 a33 |               |   a32a21-a31a22  -(a32a11-a31a12)   a22a11-a21a12  |
            /// with DET  =  a11(a33a22-a32a23)-a21(a33a12-a32a13)+a31(a23a12-a22a13)
            /// 

            double det = (double)m[0, 0] * ((double)m[2, 2] * (double)m[1, 1] - (double)m[2, 1] * (double)m[1, 2]) - (double)m[1, 0] * ((double)m[2, 2] * (double)m[0, 1] - (double)m[2, 1] * (double)m[0, 2]) + (double)m[2, 0] * ((double)m[1, 2] * (double)m[0, 1] - (double)m[1, 1] * (double)m[0, 2]);

            double[,] mp = new double[3, 3];
            mp[0, 0] = ((double)m[2, 2] * (double)m[1, 1] - (double)m[2, 1] * (double)m[1, 2]); mp[0, 1] = -((double)m[2, 2] * (double)m[0, 1] - (double)m[2, 1] * (double)m[0, 2]); mp[0, 2] = ((double)m[1, 2] * (double)m[0, 1] - (double)m[1, 1] * (double)m[0, 2]);
            mp[1, 0] = -((double)m[2, 2] * (double)m[1, 0] - (double)m[2, 0] * (double)m[1, 2]); mp[1, 1] = ((double)m[2, 2] * (double)m[0, 0] - (double)m[2, 0] * (double)m[0, 2]); mp[1, 2] = -((double)m[1, 2] * (double)m[0, 0] - (double)m[1, 0] * (double)m[0, 2]);
            mp[2, 0] = ((double)m[2, 1] * (double)m[1, 0] - (double)m[2, 0] * (double)m[1, 1]); mp[2, 1] = -((double)m[2, 1] * (double)m[0, 0] - (double)m[2, 0] * (double)m[0, 1]); mp[2, 2] = ((double)m[1, 1] * (double)m[0, 0] - (double)m[1, 0] * (double)m[0, 1]);

            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    mp[i, j] = mp[i, j] / det;
                    if (double.IsInfinity(mp[i, j]) || double.IsNaN(mp[i, j])) return null;
                }
            }
            return mp;



        }


        void processingThreadWaves() {

            lock (protect) {
                state = State.processingdata;
            }
            Thread.Sleep(100);

            //double[] tab_high = new double[samples.Length];

            
            double[] acc_z = new double[samples.Length];
            double[] acc_y = new double[samples.Length];
            double[] acc_x = new double[samples.Length];

            double[] save_czz = new double[samples.Length];
            double[] save_czz_lin = new double[samples.Length];

            double[] log_czz = new double[samples.Length];

            int initbin = 0;
            int startbin = 0;

            WavesProcessingResult processingresult;
            try {
                int missed = 0;
                //int k = 0;

                //double delta_samples = 1 / Fs;

                AHRSControl.AccAngularRateMagVectorOrientationMatrix[] work_samples = new AHRSControl.AccAngularRateMagVectorOrientationMatrix[samples.Length];
                AHRSControl.AccAngularRateMagVectorOrientationMatrix[] final_samples = new AHRSControl.AccAngularRateMagVectorOrientationMatrix[samples.Length];

                //int index = 0;
                double min_timer = double.MaxValue;
                int index_min_timer = 0;
                AHRSControl.AccAngularRateMagVectorOrientationMatrix current_samples = samples[0];
                bool change = false;
                for (int i = 0; i < samples.Length; i++) {
                //for (int i = 0; i < sample.Length; i++) {
                    if (samples[i].timer < min_timer) {
                        min_timer = samples[i].timer;
                        index_min_timer = i;
                        current_samples = samples[i];
                        change = true;
                    }

                    if (change) {
                        samples[i] = samples[0];
                        samples[0] = current_samples;
                        change = false;
                    }

                }
                //for (int i = 0; i < samples.Length; i++) {
                //    work_samples[i] = samples[i];
                //}

                UInt32 d_time_milli = (UInt32)((1000 / Fs));
                UInt32 d_time_milli_min = (UInt32)(0.98 * d_time_milli);
                UInt32 d_time_milli_max = (UInt32)(1.02 * d_time_milli);

                for (int i = 1; i < samples.Length; i++) {

                    UInt32 diff = (UInt32)(samples[i].timer * 1000 - samples[i - 1].timer * 1000);
                    if (diff > d_time_milli_max) {// test @ 20Hz
                        //if (diff > 255) {// one sample
                        bool solved = false;
                        // check if missing sample is not an ordering problem
                        for (int j = i + 1; j < samples.Length; j++) {
                            // if sample found simply do an exchange
                            UInt32 diff2 = (UInt32)(samples[j].timer * 1000 - samples[i - 1].timer * 1000);
                            if (diff2 > d_time_milli_min && diff2 < (2 * d_time_milli) ) {   // test @ 20Hz
                                //if (diff2 < 245 && diff2 > 750) {
                                AHRSControl.AccAngularRateMagVectorOrientationMatrix tmpswap = samples[i];
                                samples[i] = samples[j];
                                samples[j] = tmpswap;
                                solved = true;
                                break;
                            }
                        }

                        if (!solved) {
                            if (i < samples.Length - 1) {
                                for (int j = samples.Length - 2; j >= i; j--) {
                                    samples[j + 1] = samples[j];
                                }
                            }
                            samples[i] = samples[i - 1].Clone();
                            //samples[i].timer = samples[i].timer + 0.25;
                            samples[i].timer = samples[i].timer + ((double)d_time_milli) / 1000;     // test @ 20Hz
                            missed++;
                        }
                    }
                }


                //
                // INTEGRATION PART
                //

                //double[] acc_z = new double[samples.Length];
                //double[] acc_y = new double[samples.Length];
                //double[] acc_x = new double[samples.Length];


                //int Fs = 4;
                //int Fs = 20;// test @ 20Hz
                double[] tmpvect;

                // transform first the accelerations
                for (int i = 0; i < samples.Length; i++) {
                    tmpvect = multiplyByMatrix33(inverseMatrix33(samples[i].orientationmatrix), samples[i].accelerations);
                    if (tmpvect != null) {
                        acc_x[i] = tmpvect[0];
                        acc_y[i] = tmpvect[1];
                        acc_z[i] = tmpvect[2];
                    } else {
                        acc_x[i] = 0.0;
                        acc_y[i] = 0.0;
                        acc_z[i] = 0.0;

                    }
                }

                // calculate mean acceleration on all samples (removing dc component)
                double[] meanacceleration = new double[] { 0.0, 0.0, 0.0 };

                for (int i = 0; i < acc_z.Length; i++) {
                    meanacceleration[0] += acc_x[i];
                    meanacceleration[1] += acc_y[i];
                    meanacceleration[2] += acc_z[i];
                }
                meanacceleration[0] = meanacceleration[0] / samples.Length;
                meanacceleration[1] = meanacceleration[1] / samples.Length;
                meanacceleration[2] = meanacceleration[2] / samples.Length;

                // remove the mean and multiply by acceleration constant to have it in m/(s*s)
                for (int i = 0; i < samples.Length; i++) {
                    acc_x[i] = (acc_x[i] - meanacceleration[0]) * 9.80665;
                    acc_y[i] = (acc_y[i] - meanacceleration[1]) * 9.80665;
                    acc_z[i] = (acc_z[i] - meanacceleration[2]) * 9.80665;
                }

                //int NFFT = nsamples;
                int NFFT = samples.Length; // sample.Length is nsamples minus 2 margin -- try to disable AHRS noise ....
                double deltaF = Fs / (double)NFFT;
                // calculate fourier transform
                double[] cxy = (double[])acc_x.Clone();
                double[] qxy = (double[])acc_y.Clone();
                crosscorrelate(cxy, qxy, NFFT, deltaF);
                double[] czx = (double[])acc_z.Clone();
                double[] qzx = (double[])acc_x.Clone();
                crosscorrelate(czx, qzx, NFFT, deltaF);
                double[] czy = (double[])acc_z.Clone();
                double[] qzy = (double[])acc_y.Clone();
                crosscorrelate(czy, qzy, NFFT, deltaF);
                double[] cxx = (double[])acc_x.Clone();
                double[] qxx = (double[])acc_x.Clone();
                crosscorrelate(cxx, qxx, NFFT, deltaF);
                double[] cyy = (double[])acc_y.Clone();
                double[] qyy = (double[])acc_y.Clone();
                crosscorrelate(cyy, qyy, NFFT, deltaF);
                double[] czz = (double[])acc_z.Clone();
                double[] qzz = (double[])acc_z.Clone();
                crosscorrelate(czz, qzz, NFFT, deltaF);
                // determine starting low frequency
                // first find max of log10(first 20 bins)
                double maxlog10firstbins = double.MinValue;
                //int initbin = (int)Math.Ceiling(0.033 / deltaF);
                initbin = 10;



                for (int i = 1; i < initbin; i++) {
                    double tmp = 10 * Math.Log10(czz[i]);
                    if (tmp > maxlog10firstbins) maxlog10firstbins = tmp;
                }
                // then find first bins bigger
                startbin = 0;
                startbin = 34;

                //Suppress for BF research
                //for (int i = initbin; i < NFFT / 2; i++) {
                //    if (10 * Math.Log10(czz[i]) > maxlog10firstbins + 8) {
                //        startbin = i;
                //        break;
                //    }
                //}

                //for (int i = 1; i < 42; i++) {
                //    double tmp = 10 * Math.Log10(czz[i]);
                //    if (tmp > maxlog10firstbins) maxlog10firstbins = tmp;
                //}
                //// then find first bins bigger
                //int startbin = 0;
                //for (int i = 42; i < NFFT / 2; i++) {
                //    if (10 * Math.Log10(czz[i]) > maxlog10firstbins + 8) {
                //        startbin = i;
                //        break;
                //    }
                //}

                // Remove low filter because we want 0.041Hz minimum frequency ( period ~24s )
                //int startbin = (int)Math.Ceiling(0.041 / deltaF);      // 42 pts -- 24s
                //int startbin = (int)Math.Ceiling(0.033 / deltaF);      // 33 pts -- 30s
                //startbin = initbin;
                // check if starting low freq has not been found
                // if not found then assigned default 8 seconds period (0.125 Hz)
                //if (startbin == 0) {
                //    //startbin = (int)Math.Ceiling(0.125 / deltaF);       // 128 pts
                //    //    //startbin = (int)Math.Ceiling(0.033 / deltaF);
                //    //startbin = (int)Math.Ceiling(0.041 / deltaF);      // 42 pts -- 24s
                //    //    //double d_ceil = (((double)d_time_milli) / 2) / 1000;
                //    //    //startbin = (int)Math.Ceiling(d_ceil / deltaF);
                //    //    //startbin = (int)Math.Ceiling(0.025 / deltaF);// Test @ 20Hz
                //    startbin = initbin;
                //}

                

                // middlebin is abitrary 10s
                double middle_period = 10;
                int middlebin = (int)Math.Floor((1 / middle_period) / deltaF);

                // endbin is abitrary 1.4s
                int endbin = (int)Math.Floor((1 / 1.4) / deltaF);

                // calculate the omega vector
                //omega=2*pi*([0:NFFT-1]*deltaf);
                double[] omega = new double[1 + NFFT / 2];
                for (int i = 1; i < NFFT / 2; i++) { omega[i] = 2 * Math.PI * i * deltaF; }


                //double[] czz_bf = new double[middlebin+1];
                //double[] czz_hf = new double[endbin - middlebin];
                double[] czz_bf = (double[])czz.Clone();
                double[] czz_hf = (double[])czz.Clone();
                double[] czz_raw = (double[])czz.Clone();

                // Tp, Tp_bf, Tp_hf
                double Tp = startbin;
                double Tp_bf = startbin;
                double Tp_hf = startbin;
                double maxczz = double.MinValue;
                double maxczz_bf = double.MinValue;
                double maxczz_hf = double.MinValue;

                // Mo_0, Mo_0_bf, Mo0_hf
                double Mo_11 = 0.0;
                double Mo_0 = 0.0;
                double Mo_1 = 0.0;
                double Mo_2 = 0.0;

                double Mo_11_bf = 0.0;
                double Mo_0_bf = 0.0;
                double Mo_1_bf = 0.0;
                double Mo_2_bf = 0.0;

                double Mo_11_hf = 0.0;
                double Mo_0_hf = 0.0;
                double Mo_1_hf = 0.0;
                double Mo_2_hf = 0.0;

                double sumoverx = 0;
                double sumovery = 0;

                double sumoverx_bf = 0;
                double sumovery_bf = 0;

                double sumoverx_hf = 0;
                double sumovery_hf = 0;

                double ahrs_correction = 0;

                // Try to do many things in one loop
                for (int i = 0; i < NFFT; i++) {

                    // Apply AHRS correction
                    if (i <= middlebin) {
                        ahrs_correction = -(30.0 / middlebin) * (middlebin - i);
                        //ahrs_correction = -(25 / middlebin) * (middlebin - i);
                    } else
                        ahrs_correction = 0;
                    ///////////////////////////////////////////////////////////////////

                    if (i < initbin) {
                        czz[i] = 0;
                        czz_raw[i] = 0;
                        czz_bf[i] = 0;
                        czz_hf[i] = 0;

                        log_czz[i] = 0;
                        save_czz[i] = 0;
                        save_czz_lin[i] = 0;
                    }
                    else if (i < startbin) {

                        czz_raw[i] = 0;
                        czz[i] = czz[i] / (Math.Pow(omega[i], 4));

                        save_czz[i] = czz[i];                                       // for register in file

                        log_czz[i] = 10 * Math.Log10(czz[i]) + ahrs_correction;     // to apply correction

                        czz[i] = Math.Pow(10, log_czz[i] / 10);                     // return in linear view
                        save_czz_lin[i] = czz[i];                                   // for register in file

                        czz_bf[i] = 0;
                        czz_hf[i] = 0;

                        //czz[i] += ahrs_correction;

                    } 
                    else if (i < endbin && i >= startbin) {

                        czz_raw[i] = czz[i];
                        czz[i] = czz[i] / (Math.Pow(omega[i], 4));

                        save_czz[i] = czz[i];                                       // for register in file

                        log_czz[i] = 10 * Math.Log10(czz[i]) + ahrs_correction;     // to apply correction


                        save_czz_lin[i] = Math.Pow( 10, log_czz[i] / 10);           // for register in file  


                        czz[i] = save_czz_lin[i];                                   // return in linear view

                        

                        //czz[i] += ahrs_correction;


                        // initialize with 0
                        czz_bf[i] = 0;
                        czz_hf[i] = 0;

                        // search Tp
                        if (czz[i] > maxczz) {
                            maxczz = czz[i];
                            Tp = i;
                        }

                        // Mo_0
                        if (i >= 1)
                            Mo_11 += (czz[i] / (i * deltaF));
                        Mo_0 += czz[i];
                        Mo_1 += (czz[i] * i * deltaF);
                        Mo_2 += (czz[i] * (Math.Pow(i * deltaF, 2)));

                        double theta = Math.Atan2(qzx[i], qzy[i]);
                        sumoverx += Math.Cos(theta) * czz[i];
                        sumovery += Math.Sin(theta) * czz[i];

                        if (i <= middlebin) {

                            

                            czz_bf[i] = czz[i];

                            // search Tp_bf
                            if (czz_bf[i] > maxczz_bf) {
                                maxczz_bf = czz_bf[i];
                                Tp_bf = i;
                            }

                            if ( i >= 1 )
                                Mo_11_bf += (czz[i] / (i * deltaF));

                            Mo_0_bf += czz[i];
                            Mo_1_bf += (czz[i] * i * deltaF);
                            Mo_2_bf += (czz[i] * (Math.Pow(i * deltaF, 2)));

                            sumoverx_bf += Math.Cos(theta) * czz[i];
                            sumovery_bf += Math.Sin(theta) * czz[i];

                        } else {

                            czz_hf[i] = czz[i];

                            // search Tp_hf
                            if (czz_hf[i] > maxczz_hf) {
                                maxczz_hf = czz_hf[i];
                                Tp_hf = i;
                            }

                            if (i >= 1)
                                Mo_11_hf += (czz[i] / (i * deltaF));

                            Mo_0_hf += czz[i];
                            Mo_1_hf += (czz[i] * i * deltaF);
                            Mo_2_hf += (czz[i] * (Math.Pow(i * deltaF, 2)));

                            sumoverx_hf += Math.Cos(theta) * czz[i];
                            sumovery_hf += Math.Sin(theta) * czz[i];
                        }

                    } else {        // i>= endin
                        czz[i] = 0;
                        czz_raw[i] = 0;
                        czz_bf[i] = 0;
                        czz_hf[i] = 0;
                        save_czz[i] = 0;
                        log_czz[i] = 0;
                        save_czz_lin[i] = 0;
                    }

                }

                Mo_11 = Mo_11 * deltaF;
                Mo_0 = Mo_0 * deltaF;
                Mo_1 = Mo_1 * deltaF;
                Mo_2 = Mo_2 * deltaF;

                Mo_11_bf = Mo_11_bf * deltaF;
                Mo_0_bf = Mo_0_bf * deltaF;
                Mo_1_bf = Mo_1_bf * deltaF;
                Mo_2_bf = Mo_2_bf * deltaF;

                Mo_11_hf = Mo_11_hf * deltaF;
                Mo_0_hf = Mo_0_hf * deltaF;
                Mo_1_hf = Mo_1_hf * deltaF;
                Mo_2_hf = Mo_2_hf * deltaF;

                int indextp = (int)Tp;
                Tp = 1 / (Tp * deltaF);
                // dir at tp
                double dirtp = Math.Atan2(qzx[indextp], qzy[indextp]); dirtp = 90.0 - dirtp * 180.0 / Math.PI;
                if (dirtp < 0.0) dirtp = dirtp + 360.0;


                int indextp_bf = (int)Tp_bf;
                Tp_bf = 1 / (Tp_bf * deltaF);
                double dirtp_bf = Math.Atan2(qzx[indextp_bf], qzy[indextp_bf]); dirtp_bf = 90.0 - dirtp_bf * 180.0 / Math.PI;
                if (dirtp_bf < 0.0) dirtp_bf = dirtp_bf + 360.0;


                int indextp_hf = (int)Tp_hf;
                Tp_hf = 1 / (Tp_hf * deltaF);
                double dirtp_hf = Math.Atan2(qzx[indextp_hf], qzy[indextp_hf]); dirtp_hf = 90.0 - dirtp_hf * 180.0 / Math.PI;
                if (dirtp_hf < 0.0) dirtp_hf = dirtp_hf + 360.0;

                // mean period
                double T02 = Math.Sqrt(Mo_0 / Mo_2);
                double T02_bf = Math.Sqrt(Mo_0_bf / Mo_2_bf);
                double T02_hf = Math.Sqrt(Mo_0_hf / Mo_2_hf);

                //T01
                double T01 = Mo_0 / Mo_1;

                // mean dir
                //MeanDir=angle(sum(Czz(range).*exp(i*atan2(Qzx(range),Qzy(range))))/sum(Czz(range)));
                // @ TODO recoir cette formul !
                // simplification entre atan2 et e(ithta)
                //double sumoverx = 0;
                //double sumovery = 0;

                //double sumoverx_bf = 0;
                //double sumovery_bf = 0;

                //double sumoverx_hf = 0;
                //double sumovery_hf = 0;

                //for (int i = startbin; i <= endbin; i++) {
                //    double theta = Math.Atan2(qzx[i], qzy[i]);
                //    sumoverx += Math.Cos(theta) * czz[i];
                //    sumovery += Math.Sin(theta) * czz[i];
                //}
                double DirT02 = 90.0 - Math.Atan2(sumovery, sumoverx) * 180.0 / Math.PI;
                if (DirT02 < 0.0) DirT02 = DirT02 + 360.0;

                double DirT02_bf = 90.0 - Math.Atan2(sumovery_bf, sumoverx_bf) * 180.0 / Math.PI;
                if (DirT02_bf < 0.0) DirT02_bf = DirT02_bf + 360.0;

                double DirT02_hf = 90.0 - Math.Atan2(sumovery_hf, sumoverx_hf) * 180.0 / Math.PI;
                if (DirT02_hf < 0.0) DirT02_hf = DirT02_hf + 360.0;

                // rescaling
                double[] cxx_rescaled;
                double[] cxy_rescaled;
                double[] cyy_rescaled;
                double[] czz_rescaled;
                double[] qzx_rescaled;
                double[] qzy_rescaled;

                RescaleSpectra(cxx, Fs, freqstart, DeltaFreq, NBins, out cxx_rescaled);
                RescaleSpectra(cxy, Fs, freqstart, DeltaFreq, NBins, out cxy_rescaled);
                RescaleSpectra(cyy, Fs, freqstart, DeltaFreq, NBins, out cyy_rescaled);
                RescaleSpectra(czz, Fs, freqstart, DeltaFreq, NBins, out czz_rescaled);
                RescaleSpectra(qzx, Fs, freqstart, DeltaFreq, NBins, out qzx_rescaled);
                RescaleSpectra(qzy, Fs, freqstart, DeltaFreq, NBins, out qzy_rescaled);

                double[] calc_spread = new double[6];

                calc_spread[0] = indextp;
                calc_spread[1] = qzx[indextp];
                calc_spread[2] = qzy[indextp];
                calc_spread[3] = omega[indextp];// Math.Pow(omega[indextp], 4) * czz[indextp];
                calc_spread[4] = cxx[indextp];
                calc_spread[5] = cyy[indextp];



                //double sprd3 = 0;
                //double theta_i = Math.Atan2(qzx[indextp], qzy[indextp]);
                //double A1 = Math.Cos(theta_i) * czz[indextp] * Math.Pow(omega[indextp], 4);
                //double B1 = Math.Sin(theta_i) * czz[indextp] * Math.Pow(omega[indextp], 4);
                //sprd3 = 90.0 - Math.Sqrt(2 * (1 - Math.Sqrt(Math.Pow(A1, 2) + Math.Pow(B1, 2)))) * 180.0 / Math.PI;
                //if (sprd3 < 0.0) sprd3 = sprd3 + 360.0;
                //if (sprd3 > 360.0) sprd3 = sprd3 - 360.0;

                //double sprd2 = 0;
                //double A1_2 = qzx[indextp] / ( Math.Pow(omega[indextp], 4) * Math.Sqrt(czz[indextp] * (cxx[indextp] + cyy[indextp])));
                //double B1_2 = qzy[indextp] / ( Math.Pow(omega[indextp], 4) * Math.Sqrt(czz[indextp] * (cxx[indextp] + cyy[indextp])));
                //sprd2 = 180 * Math.Atan2(qzy[indextp], qzx[indextp]) / Math.PI;
                //if (sprd2 < 0.0) sprd2 = sprd2 + 360.0;
                //if (sprd2 > 360.0) sprd2 = sprd2 - 360.0;

                double sprd = 0;
                double sprd2 = 0;
                double C = Math.Pow(qzx[indextp], 2) + Math.Pow(qzy[indextp], 2);
                double D = Math.Pow(omega[indextp], 4) * czz[indextp] * (cxx[indextp] + cyy[indextp]);
                double D2 = czz_raw[indextp] * (cxx[indextp] + cyy[indextp]);
                //double D = Math.Pow(omega[indextp], 4) * czz[indextp] * (cxx[indextp] + 2*cxx[indextp]);
                double M = 0;
                double M2 = 0;
                if (D == 0) {
                    sprd = 0;
                    sprd2 = 0;
                } else {
                    M = Math.Sqrt(C / D);
                    M2 = Math.Sqrt(C / D2);
                    if (M < 1)
                        sprd = 180 * Math.Sqrt(2 - 2 * M) / Math.PI;
                    else
                        sprd = 0;

                    if (M2 < 1)
                        sprd2 = 180 * Math.Sqrt(2 - 2 * M2) / Math.PI;
                    else
                        sprd2 = 0;
                }

                sprd /= 2; // ... datawell result ...

                double[] MainDir = new double[NBins];
                for (int i = 0; i < MainDir.Length; i++) {
                    MainDir[i] = Math.Atan2(qzx_rescaled[i], qzy_rescaled[i]);
                    MainDir[i] = 90.0 - MainDir[i] * 180.0 / Math.PI;
                    if (MainDir[i] < 0.0) MainDir[i] = MainDir[i] + 360.0;
                }



                double[] frequencies = new double[NBins];
                for (int i = 0; i < NBins; i++) { frequencies[i] = freqstart + i * DeltaFreq; }

                /****************************************/
                /*       HMAX via zero upcrossing       */
                /****************************************/
                // acc_z is acceleration in good reference in m/s

                // first integration
                double[] calcbuffer1 = new double[acc_z.Length];
                HighPassFilter filter = new HighPassFilter();
                filter.Filter(acc_z, calcbuffer1);
                double[] calcbuffer2 = new double[acc_z.Length];
                calcbuffer2[0] = 0;
                for (int i = 1; i < calcbuffer1.Length; i++) {
                    calcbuffer2[i] = calcbuffer2[i - 1] + calcbuffer1[i - 1] / Fs;
                }
                // here in calcbuffer2 there is the speed
                filter = new HighPassFilter(); // to reinitialize coeff
                filter.Filter(calcbuffer2, calcbuffer1);
                // here in calcbuffer1 there is the filtered speed
                calcbuffer2[0] = 0;
                calcbuffer2[1] = 0;
                double sumcalcbuffer2 = 0;
                for (int i = 2; i < calcbuffer1.Length; i++) {
                    calcbuffer2[i] = calcbuffer2[i - 1] + calcbuffer1[i - 1] / Fs;
                    //tab_high[i] = calcbuffer2[i];
                    sumcalcbuffer2 += calcbuffer2[i];
                }
                // then we remove the mean
                sumcalcbuffer2 = sumcalcbuffer2 / calcbuffer2.Length;
                for (int i = 0; i < calcbuffer2.Length; i++) {
                    calcbuffer2[i] -= sumcalcbuffer2;
                    //tab_high[i] = calcbuffer2[i];
                }
                // in calcbuffer2 the position 

                // indices of zero up crossing
                int zeroupcrossendstart = 20;       // DBE remplace 200 par 20 pour test plus rapide !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                int currentsign = Math.Sign(calcbuffer2[zeroupcrossendstart]);
                int indexcalcbuffer1 = 0;


                for (int i = zeroupcrossendstart; i < calcbuffer2.Length; i++) {
                    if (Math.Sign(calcbuffer2[i]) != currentsign) {
                        currentsign = Math.Sign(calcbuffer2[i]);
                        if (currentsign == -1) {
                            calcbuffer1[indexcalcbuffer1++] = i;
                        }
                    }
                }
                //indexcalcbuffer1--;

                // calculate Numwaves
                int nbr_waves = indexcalcbuffer1 - 1;

                // calculate zero upcrossing period (Tz)
                double[] T_all_waves = new double[nbr_waves];
                double Tz = 0;
                double Tz_bf = 0;
                int Tz_bf_i = 0;
                double Tz_hf = 0;
                int Tz_hf_i = 0;
                for (int i = 1; i < indexcalcbuffer1; i++) {
                    double tmpTz = (calcbuffer1[i] - calcbuffer1[i - 1]) / Fs;
                    Tz += tmpTz;

                    if (tmpTz >= middle_period) {
                        Tz_bf += tmpTz;
                        Tz_bf_i++;
                    } else {
                        Tz_hf += tmpTz;
                        Tz_hf_i++;
                    }


                    T_all_waves[i - 1] = tmpTz;
                    //Tz += (calcbuffer1[i] - calcbuffer1[i - 1]) * Fs;
                    //Tz += (calcbuffer1[i] - calcbuffer1[i - 1]) / 4.0;
                }
                if ( nbr_waves > 0 )
                    Tz /= nbr_waves;

                if ( Tz_bf_i > 0 )      
                    Tz_bf /= Tz_bf_i;

                if (Tz_hf_i > 0)
                    Tz_hf /= Tz_hf_i;
                //Array.Sort(T_all_waves);



                // calculate HMax and prepare for H1/3 
                double hmax = -1.0;
                double thmax = -1.0;
                double tmax = -1.0;
                double ETAmax = -1.0;
                double ETAmin = double.MaxValue;

                double[] H_all_waves = new double[nbr_waves];  // nbr waves = nbr downcrossing - 1
                double[] P_all_waves = new double[nbr_waves];
                double[] downcrossing = new double[nbr_waves + 1];

                downcrossing[0] = calcbuffer1[0];
                for (int i = 1; i < indexcalcbuffer1; i++) {
                    double tmpmin = 0;
                    double tmpmax = 0;
                    minmaxArray(calcbuffer2, (int)calcbuffer1[i - 1], (int)calcbuffer1[i], ref tmpmin, ref tmpmax);
                    double tmphmax = tmpmax - tmpmin;
                    double tmppwave = ((double)(calcbuffer1[i] - calcbuffer1[i - 1])) / Fs;
                    //double tmppwave = ((double)(calcbuffer1[i] - calcbuffer1[i - 1])) / 4.0;
                    if (tmphmax > hmax) {
                        hmax = tmphmax;
                        thmax = tmppwave; // ((double)(calcbuffer1[i] - calcbuffer1[i - 1])) / 4.0;

                        // memorise tmpmax and tmpmin as ETAmax and ETAmin
                        ETAmax = tmpmax;
                        ETAmin = Math.Abs(tmpmin);
                    }

                    if (tmppwave > tmax)
                        tmax = tmppwave;

                    // registering all waves height
                    H_all_waves[i - 1] = tmphmax;
                    P_all_waves[i - 1] = tmppwave;
                    // make a copy of calcbuffer1
                    downcrossing[i] = calcbuffer1[i];
                }

                /////////////// Calcul du H1/3  ///////////////////////////////

                // sort all waves height in ascending order
                Array.Sort(H_all_waves, P_all_waves);

                double htier = 0;
                double ptier = 0;
                int nbr_waves_tier = (int)Math.Floor(nbr_waves / 3);
                double[] downcrossing_tier = new double[nbr_waves_tier + 1];

                for (int i = H_all_waves.Length - 1; i >= H_all_waves.Length - nbr_waves_tier; i--) {
                    htier += H_all_waves[i];
                    ptier += P_all_waves[i];
                    //downcrossing_tier[H_all_waves.Length - i - 1] = downcrossing[i+1];
                }
                //downcrossing_tier[nbr_waves_tier] = downcrossing[H_all_waves.Length - nbr_waves_tier];

                
                if (nbr_waves_tier > 0) {
                    // H1/3 result  ////////////////////////////
                    htier /= nbr_waves_tier;

                    // T1/3 result  ////////////////////////////
                    ptier /= nbr_waves_tier;
                }

                double Hs = 4 * Math.Sqrt(Mo_0);
                double Hs_bf = 4 * Math.Sqrt(Mo_0_bf);
                double Hs_hf = 4 * Math.Sqrt(Mo_0_hf);

                double Te = 0;
                if ( Mo_0 > 0 )
                    Te = Mo_11 / Mo_0;


                double Te_bf = 0;
                if ( Mo_0_bf > 0 )
                    Te_bf = Mo_11_bf / Mo_0_bf;


                double Te_hf = 0;
                if ( Mo_0_hf > 0 )
                    Te_hf = Mo_11_hf / Mo_0_hf;

                processingresult = new WavesProcessingResult();
                processingresult.err_msg = "SampleLength:" + samples.Length.ToString() + ";";

                processingresult.Hm0 = Hs; // Hm0
                processingresult.Hm0_bf = Hs_bf;
                processingresult.Hm0_hf = Hs_hf;

                processingresult.Tp = Tp; // Peak period
                processingresult.Tp_bf = Tp_bf;
                processingresult.Tp_hf = Tp_hf;

                //startDate = samples[0].receiveddate;
                //processingresult.Starting = samples[0].receiveddate; // timestamp
                processingresult.Starting = startDate;
                processingresult.missedsamples = missed; // missed samples
                processingresult.PowerSpectrum = czz_rescaled; // power spectrum
                processingresult.T02 = T02; // mean period
                processingresult.T02_bf = T02_bf; // mean period
                processingresult.T02_hf = T02_hf; // mean period

                processingresult.MainDir = MainDir; // main directions at each frequencies of interest 

                processingresult.DirT02 = DirT02; // mean direction
                processingresult.DirT02_bf = DirT02_bf;
                processingresult.DirT02_hf = DirT02_hf;

                processingresult.Frequencies = frequencies;

                processingresult.DirTp = dirtp;
                processingresult.DirTp_bf = dirtp_bf;
                processingresult.DirTp_hf = dirtp_hf;

                processingresult.Hmax = hmax;
                processingresult.ETAmax = ETAmax;
                processingresult.ETAmin = ETAmin;


                processingresult.THmax = thmax;
                processingresult.Tmax = tmax;

                processingresult.T01 = T01;
                processingresult.Htier = htier;
                processingresult.Ptier = ptier;

                processingresult.Tz = Tz;
                processingresult.Tz_bf = Tz_bf;
                processingresult.Tz_hf = Tz_hf;

                processingresult.NumWaves = nbr_waves;
                processingresult.Sprd = sprd;

                processingresult.Sprd2 = sprd2;
                //processingresult.Sprd2 = sprd2;
                //processingresult.Sprd3 = sprd3;
                processingresult.Sprd3 = 0;

                processingresult.calc_sprd = new double[6];
                processingresult.calc_sprd[0] = calc_spread[0];
                processingresult.calc_sprd[1] = calc_spread[1];
                processingresult.calc_sprd[2] = calc_spread[2];
                processingresult.calc_sprd[3] = calc_spread[3];
                processingresult.calc_sprd[4] = calc_spread[4];
                processingresult.calc_sprd[5] = calc_spread[5];


                //processingresult.nsamples = samples.Length;
                processingresult.nsamples = samplesindex;

                processingresult.Te = Te;
                processingresult.Te_bf = Te_bf;
                processingresult.Te_hf = Te_hf;

                processingresult.startbin = startbin;
                processingresult.middlebin = middlebin;
                processingresult.endbin = endbin;

            }
            catch (Exception ex) {
                string msg = ex.Message;
                processingresult = new WavesProcessingResult();
                processingresult.err_msg = ex.Message + "; SampleLength:" + samples.Length.ToString() + ";" ;
                processingresult.Hm0 = 0;
                processingresult.Tp = 0;
                //processingresult.Starting = new DateTime();
                processingresult.Starting = DateTime.Now;
                processingresult.missedsamples = 0;
                processingresult.PowerSpectrum = new double[NBins]; // power spectrum
                processingresult.T02 = 0; // mean period
                processingresult.T02_bf = 0; // mean period
                processingresult.T02_hf = 0; // mean period
                processingresult.MainDir = new double[NBins];
                processingresult.DirT02 = 0;
                processingresult.DirT02_bf = 0;
                processingresult.DirT02_hf = 0;
                processingresult.Frequencies = new double[NBins];
                processingresult.DirTp = 0;
                processingresult.DirTp_bf = 0;
                processingresult.DirTp_hf = 0;
                processingresult.Hmax = 0;
                processingresult.THmax = 0;
                processingresult.Tmax = 0;

                processingresult.T01 = 0;
                processingresult.Htier = 0;
                processingresult.Ptier = 0;
                processingresult.Tz = 0;
                processingresult.Tz_bf = 0;
                processingresult.Tz_hf = 0;
                processingresult.NumWaves = 0;
                processingresult.Sprd = 0;

                processingresult.Te = 0;
                processingresult.Te_bf = 0;
                processingresult.Te_hf = 0;

                processingresult.Sprd2 = 0;
                processingresult.Sprd3 = 0;

                processingresult.calc_sprd = new double[6];

                //processingresult.nsamples = samples.Length;
                processingresult.nsamples = samplesindex;

                processingresult.startbin = 0;
                processingresult.middlebin = 0;
                processingresult.endbin = 0;
            }

            lock (protect) {
                control.AHRSControlEvent -= ahrscontrolevthandler;
                state = State.idle;
                // at this point we can make sure the garbacollector collects
                GC.Collect();

                // temporary send event saying idle
                AHRSWavesEventArgs evtf = new AHRSWavesEventArgs(AHRSWavesEventArgs.KindOfEvent.processingfinished);
                evtf.Processingresult = processingresult;
                evtf.Received_0x80 = control.count_0x80;
                evtf.Frame_treated_ok = control.count_complet_frame;
                evtf.Frame_uncomplet = control.count_uncompletes;
                evtf.Frame_uncomplet_treated = control.count_treat_uncomplete;
                evtf.Bad_checksum = control.count_badchecksum_trame;

                fireOnotherThreadEvent(evtf);

                //DateTime mydate = DateTime.Now;
                //string dir_name = "/SD Card/Logs/" + eqp_name;
                //string file_name = mydate.Year + "-" + mydate.Month + "-" + name_param + ".txt";
                //string path = dir_name + "/" + file_name;

                //try {
                //    System.IO.Directory.CreateDirectory(dir_name);

                //    if (!File.Exists(path)) {
                //        FileStream fs = new FileStream(path, FileMode.CreateNew);
                //        fs.Close();
                //    }
                //    using (StreamWriter sw = File.AppendText(path)) {

                //        sw.Write(mydate.ToString("yy/MM/dd" + (char)32 + "HH:mm:ss") + ";" + val);
                //        if (!val.EndsWith(";"))
                //            sw.Write(";");

                //        sw.Write("\r\n");
                //        sw.Close();
                //    }

                //}
                //catch { }

                string s_rawdata = "";
                DateTime mydate = DateTime.Now;

                try {
                    string dir_name = "/SD Card/Logs/";
                    System.IO.Directory.CreateDirectory(dir_name);
                    dir_name = "/SD Card/Logs/WAVES/";
                    System.IO.Directory.CreateDirectory(dir_name);
                }
                catch { }

                //if (log_spread) {
                //    try {

                //        FileStream fs = new FileStream("SD Card/Logs/AHRS/" + mydate.Year + "-" + mydate.Month + "-" + mydate.Day + "-" + mydate.Hour + mydate.Minute + "-raw_AccelZ-Waves_" +
                //            num_centrale.ToString() + ".log", FileMode.Append);
                //        DateTime date_s = startDate;
                //        for (int i = 0; i < samples.Length; i++) {
                //            s_rawdata = "";
                //            //s_rawdata += samples[i].receiveddate.ToString("MM/dd/yyyy HH:mm:ss.fff") + ";";
                //            date_s = date_s.AddMilliseconds(250);
                //            s_rawdata += date_s.ToString("MM/dd/yyyy HH:mm:ss.fff") + ";";
                //            s_rawdata += samples[i].accelerations[2].ToString("0.00000");
                //            //for (int j = 0; j < 3; j++) {
                //            //    s_rawdata += samples[i].accelerations[j].ToString("0.00000");
                //            //    if (j != 2) s_rawdata += ";";
                //            //}
                //            s_rawdata += ";\r\n";
                //            b_data_accel = new UTF8Encoding(true).GetBytes(s_rawdata);

                //            fs.Write(b_data_accel, 0, b_data_accel.Length);
                //        }

                //        s_rawdata = "/////////////////////////////////////////////\r\n";
                //        b_data_accel = new UTF8Encoding(true).GetBytes(s_rawdata);

                //        fs.Write(b_data_accel, 0, b_data_accel.Length);
                //        fs.Close();
                //    }
                //    catch { }


                //    try {

                //        FileStream fs = new FileStream("SD Card/Logs/AHRS/" + mydate.Year + "-" + mydate.Month + "-" + mydate.Day + "-" + mydate.Hour + mydate.Minute + "-raw_Z-Waves_" +
                //            num_centrale.ToString() + ".log", FileMode.Append);

                //        DateTime date_s = startDate;
                //        for (int i = 0; i < samples.Length; i++) {
                //            s_rawdata = "";
                //            //s_rawdata += samples[i].receiveddate.ToString("MM/dd/yyyy HH:mm:ss.fff") + ";";
                //            date_s = date_s.AddMilliseconds(250);
                //            s_rawdata += date_s.ToString("MM/dd/yyyy HH:mm:ss.fff") + ";";
                //            s_rawdata += tab_high[i].ToString("0.00000");
                //            s_rawdata += ";\r\n";
                //            b_data_accel = new UTF8Encoding(true).GetBytes(s_rawdata);

                //            fs.Write(b_data_accel, 0, b_data_accel.Length);
                //        }

                //        s_rawdata = "/////////////////////////////////////////////\r\n";
                //        b_data_accel = new UTF8Encoding(true).GetBytes(s_rawdata);

                //        fs.Write(b_data_accel, 0, b_data_accel.Length);
                //        fs.Close();
                //    }
                //    catch { }
                //}

                if (log_accel) {
                    try {

                        //FileStream fs = new FileStream("SD Card/Logs/WAVES/" + mydate.Year + "-" + mydate.Month + "-" + mydate.Day + "-" + mydate.Hour + "-" + mydate.Minute
                        //                                + "-raw_Accel-Waves_" + num_centrale.ToString() + ".csv", FileMode.Append);
                        FileStream fs = new FileStream("SD Card/Logs/WAVES/" + mydate.Year + "-" + mydate.Month + "-" + mydate.Day + "-" + mydate.Hour + "-" + mydate.Minute
                                                        + "-raw_Accel-Waves" + ".csv", FileMode.Append);

                        DateTime date_s = startDate;
                        s_rawdata = "MM/dd/yyyy HH:mm;";
                        s_rawdata += date_s.ToString("MM/dd/yyyy HH:mm") + "\n";
                        s_rawdata += " Accel x; Accel y; Accel z";
                        //s_rawdata += date_s.ToString("MM/dd/yyyy HH:mm:ss.fff") + ";";
                        for (int i = 0; i < samples.Length; i++) {
                            s_rawdata = "";

                            // log acceleration adjusted with orientation matrix 
                            s_rawdata +=    acc_x[i].ToString("0.00000") + ";" 
                                            + acc_x[i].ToString("0.00000") + ";" 
                                            + acc_z[i].ToString("0.00000");

                            //date_s = date_s.AddMilliseconds(250);
                            //s_rawdata += date_s.ToString("HH:mm:ss.fff") + ";";
                            //for (int j = 0; j < 3; j++) {
                            //    s_rawdata += samples[i].accelerations[j].ToString("0.00000");
                            //    if (j != 2) s_rawdata += ";";
                            //}
                            s_rawdata += ";\r\n";
                            b_data_accel = new UTF8Encoding(true).GetBytes(s_rawdata);

                            fs.Write(b_data_accel, 0, b_data_accel.Length);
                        }

                        b_data_accel = new UTF8Encoding(true).GetBytes(s_rawdata);

                        fs.Write(b_data_accel, 0, b_data_accel.Length);
                        fs.Close();
                    }
                    catch { }
                }


                if (false) {
                    try {

                        FileStream fs = new FileStream("SD Card/Logs/WAVES/" + mydate.Year + "-" + mydate.Month + "-" + mydate.Day + "-" + mydate.Hour + "-" + mydate.Minute
                                                        + "-Czz_" + "S" + startbin.ToString() + "_" + Fs.ToString() + "Hz" + "_NFFT" + nsamples.ToString() + ".csv",
                                                        FileMode.Append);

                        DateTime date_s = startDate;
                        s_rawdata = "MM/dd/yyyy HH:mm;";
                        s_rawdata += date_s.ToString("MM/dd/yyyy HH:mm") + "\n";
                        s_rawdata += " Accel x; Accel y; Accel z";
                        //s_rawdata += date_s.ToString("MM/dd/yyyy HH:mm:ss.fff") + ";";
                        for (int i = 0; i < samples.Length; i++) {
                            s_rawdata = "";

                            // log acceleration adjusted with orientation matrix 
                            s_rawdata += save_czz[i].ToString("0.00000000") + ";";

                            //date_s = date_s.AddMilliseconds(250);
                            //s_rawdata += date_s.ToString("HH:mm:ss.fff") + ";";
                            //for (int j = 0; j < 3; j++) {
                            //    s_rawdata += samples[i].accelerations[j].ToString("0.00000");
                            //    if (j != 2) s_rawdata += ";";
                            //}
                            s_rawdata += "\r\n";
                            b_data_accel = new UTF8Encoding(true).GetBytes(s_rawdata);

                            fs.Write(b_data_accel, 0, b_data_accel.Length);
                        }

                        s_rawdata = "/////////////////////////////////////////////\r\n";
                        b_data_accel = new UTF8Encoding(true).GetBytes(s_rawdata);

                        fs.Write(b_data_accel, 0, b_data_accel.Length);
                        fs.Close();
                    }
                    catch { }
                }


                if (false) {
                    try {

                        FileStream fs = new FileStream("SD Card/Logs/WAVES/" + mydate.Year + "-" + mydate.Month + "-" + mydate.Day + "-" + mydate.Hour + "-" + mydate.Minute
                                                        + "-CzzLin_" + "S" + startbin.ToString() + "_" + Fs.ToString() + "Hz" + "_NFFT" + nsamples.ToString() + ".csv",
                                                        FileMode.Append);

                        DateTime date_s = startDate;
                        s_rawdata = "MM/dd/yyyy HH:mm;";
                        s_rawdata += date_s.ToString("MM/dd/yyyy HH:mm") + "\n";
                        s_rawdata += " Accel x; Accel y; Accel z";
                        //s_rawdata += date_s.ToString("MM/dd/yyyy HH:mm:ss.fff") + ";";
                        for (int i = 0; i < samples.Length; i++) {
                            s_rawdata = "";

                            s_rawdata += save_czz_lin[i].ToString("0.00000000") + ";";
                            s_rawdata += "\r\n";
                            b_data_accel = new UTF8Encoding(true).GetBytes(s_rawdata);

                            fs.Write(b_data_accel, 0, b_data_accel.Length);
                        }

                        s_rawdata = "/////////////////////////////////////////////\r\n";
                        b_data_accel = new UTF8Encoding(true).GetBytes(s_rawdata);

                        fs.Write(b_data_accel, 0, b_data_accel.Length);
                        fs.Close();
                    }
                    catch { }
                }


                if (false) {
                    try {

                        FileStream fs = new FileStream("SD Card/Logs/WAVES/" + mydate.Year + "-" + mydate.Month + "-" + mydate.Day + "-" + mydate.Hour + "-" + mydate.Minute
                                                        + "-CzzLog_" + "S" + startbin.ToString() + "_" + Fs.ToString() + "Hz" + "_NFFT" + nsamples.ToString() + ".csv",
                                                        FileMode.Append);

                        DateTime date_s = startDate;
                        s_rawdata = "MM/dd/yyyy HH:mm;";
                        s_rawdata += date_s.ToString("MM/dd/yyyy HH:mm") + "\n";
                        s_rawdata += " Accel x; Accel y; Accel z";
                        //s_rawdata += date_s.ToString("MM/dd/yyyy HH:mm:ss.fff") + ";";
                        for (int i = 0; i < samples.Length; i++) {
                            s_rawdata = "";

                            s_rawdata += log_czz[i].ToString("0.00000000") + ";";
                            s_rawdata += "\r\n";
                            b_data_accel = new UTF8Encoding(true).GetBytes(s_rawdata);

                            fs.Write(b_data_accel, 0, b_data_accel.Length);
                        }

                        s_rawdata = "/////////////////////////////////////////////\r\n";
                        b_data_accel = new UTF8Encoding(true).GetBytes(s_rawdata);

                        fs.Write(b_data_accel, 0, b_data_accel.Length);
                        fs.Close();
                    }
                    catch { }
                }


                if (log_orient) {
                    try {
                        FileStream fs = new FileStream("SD Card/Logs/WAVES/" + mydate.Month + "-" + mydate.Day + "-" + mydate.Year + "-" + mydate.Hour + "-" + mydate.Minute
                                                        + "-raw_Orientation-Waves_" + num_centrale.ToString() + ".log", FileMode.Append);

                        DateTime date_s = startDate;
                        //s_rawdata = "";
                        //s_rawdata += date_s.ToString("MM/dd/yyyy HH:mm:ss.fff") + ";";
                        for (int i = 0; i < samples.Length; i++) {
                            s_rawdata = "";
                            //s_rawdata += samples[i].receiveddate.ToString("MM/dd/yyyy HH:mm:ss.fff") + ";";
                            date_s = date_s.AddMilliseconds(250);
                            s_rawdata += date_s.ToString("HH:mm:ss.fff") + ";";
                            for (int j = 0; j < 3; j++) {
                                for (int k = 0; k < 3; k++) {
                                    s_rawdata += samples[i].orientationmatrix[j, k].ToString("0.00000");
                                    if (k != 2) s_rawdata += ";";
                                }
                                if (j != 2) s_rawdata += ";";
                            }
                            s_rawdata += ";\r\n";

                            b_data_orient = new UTF8Encoding(true).GetBytes(s_rawdata);

                            fs.Write(b_data_orient, 0, b_data_orient.Length);
                        }

                        s_rawdata = "/////////////////////////////////////////////\r\n";
                        b_data_orient = new UTF8Encoding(true).GetBytes(s_rawdata);

                        fs.Write(b_data_orient, 0, b_data_orient.Length);

                        fs.Close();
                    }
                    catch { }
                }




                // temporary send event saying idle
                fireOnotherThreadEvent(new AHRSWavesEventArgs(AHRSWavesEventArgs.KindOfEvent.idle));
            }



        }




        //static 
        void minmaxArray(double[] value, int istart, int iend, ref double min, ref double max) {
            min = double.MaxValue;
            max = double.MinValue;
            for (int i = istart; i <= iend; i++) {
                if (value[i] > max) max = value[i];
                if (value[i] < min) min = value[i];
            }
        }

        /// <summary>
        /// crosscorrelate
        /// </summary>
        /// <param name="X_C">X of length NFFT is modified -> Cxy on first NFFT/2</param>
        /// <param name="Y_Q">Y of length NFFT is modified -> Qxy on first NFFT/2</param>
        /// <param name="NFFT"></param>
        /// <param name="deltaF"></param>
        void crosscorrelate(double[] X_C, double[] Y_Q, int NFFT, double deltaF) {
            Exocortex.DSP.Fourier.RFFT(X_C, NFFT, Exocortex.DSP.FourierDirection.Forward);
            Exocortex.DSP.Fourier.RFFT(Y_Q, NFFT, Exocortex.DSP.FourierDirection.Forward);
            X_C[0] = X_C[0] * Y_Q[0];
            Y_Q[0] = 0.0;
            for (int i = 1; i < NFFT / 2; i++) {
                // imag inversé par rapport a matlab donc - devant
                X_C[i] = X_C[2 * i] * Y_Q[2 * i] + (-X_C[2 * i + 1]) * (-Y_Q[2 * i + 1]);
                Y_Q[i] = (-X_C[2 * i + 1]) * Y_Q[2 * i] - X_C[2 * i] * (-Y_Q[2 * i + 1]);
                X_C[i] = 2 * X_C[i] / (NFFT * NFFT * deltaF);
                Y_Q[i] = 2 * Y_Q[i] / (NFFT * NFFT * deltaF);
            }



        }

#if DEBUGSIMULATOR
        public static void ExportToMatlab(double[] array)
        {
            StringBuilder strb = new StringBuilder();
            
            strb.Append("[");
            strb.Append(array[1].ToString("0.0000000"));
            for (int i=1;i<array.Length;i++)
            {
                strb.Append("," + array[i].ToString("0.0000000"));
            }
            strb.Append("]");
            Console.WriteLine(strb.ToString());
        }
#endif

        //
        // Rescale spectra
        //
        void RescaleSpectra(double[] OriginSpectra, double FsOriginal, double FreqStart, double DeltaF, int NBins, out double[] NewSpectra) {
            //deltaf=FsOriginal/length(OriginSpectra);
            double deltaforiginal = FsOriginal / OriginSpectra.Length;
            NewSpectra = new double[NBins];
            for (int i = 0; i < NewSpectra.Length; i++) NewSpectra[i] = 0;
            //newspectra=zeros(1,Nbins);
            //n=0;
            int n = 0;
            //currentfrequency=Freqstart-DeltaF/2;
            double currentfrequency = FreqStart - DeltaF / 2;
            //currentbin = floor(0.5 + currentfrequency / deltaf);
            int currentbin = (int)Math.Floor(0.5 + currentfrequency / deltaforiginal);
            //while (n < Nbins)
            while (n < NBins) {
                //    if ((Freqstart+(n+0.5)*DeltaF)>( (currentbin + 0.5) * deltaf))
                if ((FreqStart + (n + 0.5) * DeltaF) > ((currentbin + 0.5) * deltaforiginal)) {
                    //        newspectra(n+1) =newspectra(n+1)+ OriginSpectra(abs(currentbin)+1) * ((currentbin + 0.5) * deltaf - currentfrequency);
                    NewSpectra[n] = NewSpectra[n] + OriginSpectra[Math.Abs(currentbin)] * ((currentbin + 0.5) * deltaforiginal - currentfrequency);
                    //        currentfrequency=(currentbin+0.5)*deltaf;
                    currentfrequency = (currentbin + 0.5) * deltaforiginal;
                    //        currentbin=currentbin+1;
                    currentbin = currentbin + 1;
                }
                    //    else
                else {
                    //        newspectra(n+1) = newspectra(n+1)+OriginSpectra(abs(currentbin)+1) * (Freqstart+(n + 0.5) * DeltaF - currentfrequency);
                    NewSpectra[n] = NewSpectra[n] + OriginSpectra[Math.Abs(currentbin)] * (FreqStart + (n + 0.5) * DeltaF - currentfrequency);
                    //        newspectra(n+1) = newspectra(n+1) / DeltaF;
                    NewSpectra[n] = NewSpectra[n] / DeltaF;
                    //        currentfrequency = Freqstart+(n + 0.5) * DeltaF;
                    currentfrequency = FreqStart + (n + 0.5) * DeltaF;
                    //        n=n+1;
                    n = n + 1;
                    //    end
                }
                //end
            }


        }

    }
    public struct WavesProcessingResult {
        public DateTime Starting;
        public string err_msg;
        public int nsamples;
        public int missedsamples;

        public double Hm0; //Hm0
        public double Hm0_bf; //Hm0
        public double Hm0_hf; //Hm0

        public double Hmax;
        public double ETAmax;
        public double ETAmin;
        public double THmax;
        public double Tmax;



        public double Tp; // peak period
        public double Tp_bf;
        public double Tp_hf;

        public double Te; // Energy period
        public double Te_bf;
        public double Te_hf;

        public double T02; // mean period
        public double T02_bf;
        public double T02_hf;
        public double DirT02; // mean direction
        public double DirT02_bf; // mean direction
        public double DirT02_hf; // mean direction
        
        public double[] Frequencies;
        public double[] PowerSpectrum; // power spectrum
        public double[] MainDir; // direction in each frequency band
        public double SpreadTp; //spread at tp

        public double DirTp; // dir at tp
        public double DirTp_bf;
        public double DirTp_hf;

        public double T01;
        public double Htier;
        public double Ptier;

        public double Tz; // mean period ( upcrossing )
        public double Tz_bf;
        public double Tz_hf;

        public int NumWaves;
        public double Sprd;

        public double Sprd2;
        public double Sprd3;

        public double[] calc_sprd;

        public int startbin;
        public int middlebin;
        public int endbin;

    }

    public class AHRSWavesEventArgs : EventArgs {
        public enum KindOfEvent {
            acquisitionstarted, acquisitionfinished, idle, remainingsamples,
            acquisitionfailed, processingwaves, processingfinished, protocol_error, process_fake
        }

        KindOfEvent kind;
        int remainingsamples;
        double delta_time = 0;
        int reveived_0x80 = 0;
        int frame_treated_ok = 0;
        int frame_uncomplet = 0;
        int frame_uncomplet_treated = 0;
        int bad_checsum = 0;
        WavesProcessingResult processingresult;

        public WavesProcessingResult Processingresult {
            get { return processingresult; }
            set { processingresult = value; }
        }

        public int RemainingSamples {
            get { return remainingsamples; }
            set { remainingsamples = value; }
        }

        public double Delta_time {
            get { return delta_time; }
            set { delta_time = value; }
        }

        public int Received_0x80 {
            get { return reveived_0x80; }
            set { reveived_0x80 = value; }
        }

        public int Frame_treated_ok {
            get { return frame_treated_ok; }
            set { frame_treated_ok = value; }
        }

        public int Frame_uncomplet {
            get { return frame_uncomplet; }
            set { frame_uncomplet = value; }
        }

        public int Frame_uncomplet_treated {
            get { return frame_uncomplet_treated; }
            set { frame_uncomplet_treated = value; }
        }

        public int Bad_checksum {
            get { return bad_checsum; }
            set { bad_checsum = value; }
        }

        public KindOfEvent Kind {
            get { return kind; }
            set { kind = value; }
        }

        public AHRSWavesEventArgs(KindOfEvent kind) {
            this.kind = kind;
        }

    }
    public class AHRSException : Exception {
        public enum ExceptionCause { Unknown, Alreadyworking, SerialError, AHRSControlError, BIGERROR };
        ExceptionCause cause = ExceptionCause.Unknown;
        public ExceptionCause Cause {
            get { return cause; }
        }
        public AHRSException(ExceptionCause cause) {
            this.cause = cause;
        }

    }


}
