namespace ParkingApi.Domain
{
    public class PricingRequest
    {
        public Session session { get; set; }

        
        public bool hasContract { get; set; }
    }
        
}
