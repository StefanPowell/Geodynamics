namespace EarthQuake.Computatiuons
{
    public class Common
    {
        public long convertToEpoch(DateTime date)
        {
            DateTime epoch = new DateTime(1970, 1, 1);
            return (long)(date - epoch).TotalSeconds;
        }
    }
}
