using ParkingApi.Domain;

namespace ParkingApi.Repositories
{
    public interface ICustomerRepository
    {
        Customer GetCustomer(string customerId);
    }
}