using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using nortekmed.ahrs;

namespace AHRS_viewer
{
    class Viewer
    {
        public AHRSWaves ahrs_wave;
        public List<WavesProcessingResult> wave_results;

        public Viewer()
        {
            ahrs_wave = new AHRSWaves();
            //ahrs_wave.register_result_evt += new nortekmed.ahrs.AHRSWaves.register_result(Register_wave_result);
                //.Station_Airmar200.airmar_WX200.airmar_gps(Register_wave_result);

        }

        //void TraiteEvent_airmar_gps(double lat, double lon, byte gps_qual, byte nb_sat)
        //{
        //    try
        //    {
        //        if (this.InvokeRequired)
        //        {
        //            this.BeginInvoke(new nortekmed.Station_Airmar200.airmar_WX200.airmar_gps(TraiteEvent_airmar_gps), new object[] { lat, lon, gps_qual, nb_sat });
        //            return;
        //        }
        private void Register_wave_result(WavesProcessingResult res)
        {
            try
            {
                wave_results.Add(res);
                //if (this.InvokeRequired)
                //{
                //    this.BeginInvoke(new nortekmed.Station_Airmar200.airmar_WX200.airmar_gps(TraiteEvent_airmar_gps), new object[] { lat, lon, gps_qual, nb_sat });
                //    return;
                //}
            }
            catch(Exception) { }
        }

    }
}
