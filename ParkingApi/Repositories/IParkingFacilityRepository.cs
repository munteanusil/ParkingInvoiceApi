using ParkingApi.Domain;

namespace ParkingApi.Repositories
{
    public interface IParkingFacilityRepository
    {
        ServiceProfile GetServiceProfile(string parkingFacilityId);
    }
}