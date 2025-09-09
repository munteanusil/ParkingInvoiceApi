using System.Collections.Generic;
using ParkingApi.Domain;

namespace ParkingApi.Repositories
{
    public interface ISessionsRepository
    {
        void AddSession(Session session);
        List<Session> GetSessions(string parkingFacilityId);
    }
}