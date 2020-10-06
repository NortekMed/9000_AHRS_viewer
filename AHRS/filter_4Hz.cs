namespace nortekmed.ahrs
{
   public class HighPassFilter
   {
      #region Fields
      double t, xi;
      double IC00 = 0, IC01 = 0;
      double IC10 = 0, IC11 = 0;
      double IC20 = 0, IC21 = 0;
      #endregion

      #region Methods
      // requires output array y be already created by the calling function
      public void Filter(double[] x, double[] y)
      {
         int N = x.Length;
         for (int i=0; i<N; i++)
         {
            xi = x[i];
            // Stage 0
            t = (0.064873078316597832 * xi) - (-0.93512692168340217 * IC00) - (0 * IC01);
            xi = (1 * t) + (-1 * IC00) + (0 * IC01);
            IC01 = IC00;
            IC00 = t;
            // Stage 1
            t = (0.057326788965756971 * xi) - (-1.9548326273362424 * IC10) - (0.95945011669765012 * IC11);
            xi = (1 * t) + (-1.9997768247482064 * IC10) + (1 * IC11);
            IC11 = IC10;
            IC10 = t;
            // Stage 2
            t = xi - (-1.8928674461449981 * IC20) - (0.89720771539443644 * IC21);
            xi = (1 * t) + (-1.9999147516996902 * IC20) + (1 * IC21);
            IC21 = IC20;
            IC20 = t;
            y[i] = 241.25196380593411 * xi;
         }
      }
      #endregion
   }
}
