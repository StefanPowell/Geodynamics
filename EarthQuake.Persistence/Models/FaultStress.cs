namespace EarthQuake.Models
{
    public class FaultStress
    {
        public int Id { get; set; }                     // Primary key
        public string FaultName { get; set; }           // Fault name
        public double? SlipRate_m_per_yr { get; set; }  // Slip rate in meters per year
        public double? LockedThickness_m { get; set; }  // Locked thickness in meters
        public double? RuptureLength_m { get; set; }    // Fault rupture length in meters
        public double? ShearStressRate_MPa_per_yr { get; set; } // Shear stress rate in MPa per year
        public double? AccumulatedStress_MPa { get; set; }      // Accumulated stress over chosen period (MPa)
        public double? ExpectedSlip_m { get; set; }             // Expected slip in meters
        public double? MomentMagnitude_Mw { get; set; }         // Moment magnitude
    }
}
