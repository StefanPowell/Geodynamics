using EarthQuake.Models;

namespace EarthQuake.Computations.Seismology.Global_Stresses
{
    public class FaultStress
    {
        // use fault stress repo to save fault stress information in database


        //Coulomb Failure Stress
        public float stressbetweenFaults(Fault fault1,  Fault fault2)
        {
            return float.PositiveInfinity;
        }

        public float CoulombFailureStress(float coeffecient, float faultPlaneStress, float normalstressChange)
        {
            return faultPlaneStress - (coeffecient * normalstressChange);
        }

        public float MohrCoulombFailureCriterion(float coeffecient, float normalstress, float cohesion)
        {
            return (coeffecient * normalstress) + cohesion;
        }

    }
}
