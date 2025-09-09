using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using ParkingApi.Domain;

namespace ParkingApi.Repositories
{
    public static class FakeData
    {
        public static List<Session> GetSeedSessions()
        {
            var customers = GetSeedCustomers();

            return new List<Session>
            {
                                // John (c001) — parcarea pf001, 2 h 30 min
                new Session
                {
                    ParkingFacilityId = customers["c001"].ContractedParkingFacilityIds[0],
                    CustomerId = customers["c001"].Id,
                    StartDateTime = new System.DateTime(2025,6,21,8,0,0),
                    EndDateTime = new System.DateTime(2025,6,21,10,30,0)
                },
                                // Sarah (c002) — parcarea pf001, 5 h
                new Session
                {
                    ParkingFacilityId = customers["c002"].ContractedParkingFacilityIds[0],
                    CustomerId = customers["c002"].Id,
                    StartDateTime = new System.DateTime(2025,6,21,14,0,0),
                    EndDateTime = new System.DateTime(2025,6,21,19,0,0)
                },
                                // Sarah (c002) — parcarea pf002, 2 h 30 min
                new Session
                {
                    ParkingFacilityId = customers["c002"].ContractedParkingFacilityIds[1],
                    CustomerId = customers["c002"].Id,
                    StartDateTime = new DateTime(2025, 6, 20, 9, 0, 0),
                    EndDateTime = new DateTime(2025, 6, 20, 11, 30, 0)
                },
                               // Andrea (c003) — parcarea pf002, 2 h 45 min
                new Session
                {
                    ParkingFacilityId = customers["c003"].ContractedParkingFacilityIds[0],
                    CustomerId = customers["c003"].Id,
                    StartDateTime = new DateTime(2025, 6, 21, 15, 0, 0),
                    EndDateTime = new DateTime(2025, 6, 21, 17, 45, 0)
                }

            };
        }


        public static Dictionary<string, Customer> GetSeedCustomers() =>
            new Dictionary<string, Customer>
            {
                {
                    "c001", new Customer("c001", "John", "pf001")
                },
                {
                    "c002", new Customer("c002", "Sarah", "pf001", "pf002")
                },
                {
                    "c003", new Customer("c003", "Andrea", "pf002")
                },
                {
                    "c004", new Customer("c004", "Peter")
                }
            };

        public static Dictionary<string, ServiceProfile> GetSeedServiceProfiles() =>
            new Dictionary<string, ServiceProfile>
            {
                {
                    "pf001",
                    new ServiceProfile
                    {
                        OverrunWeekDaysPrices = new List<TimeslotPrice>
                        {
                            new TimeslotPrice(0, 7, 1.5m), // betweeen midnight and 7 AM price is 1.5 eur/hour
                            new TimeslotPrice(7, 18, 3.5m), // betweeen 7 AM and 6 PM price is 3.5 eur/hour
                            new TimeslotPrice(18, 24, 2.5m) // betweeen 6 PM and midnight price is 2.5 eur/hour
                        },
                        OverrunWeekendPrices = new List<TimeslotPrice>
                        {
                            new TimeslotPrice(0, 7, 1.8m),
                            new TimeslotPrice(7, 18, 3.8m),
                            new TimeslotPrice(18, 24, 2.8m)
                        },
                        WeekDaysPrices = new List<TimeslotPrice>
                        {
                            new TimeslotPrice(0, 7, 0.5m),
                            new TimeslotPrice(7, 18, 2.5m),
                            new TimeslotPrice(18, 24, 1.5m)
                        },
                        WeekendPrices = new List<TimeslotPrice>
                        {
                            new TimeslotPrice(0, 7, 0.8m),
                            new TimeslotPrice(7, 18, 2.8m),
                            new TimeslotPrice(18, 24, 1.8m)
                        },
                    }
                },
                {
                    "pf002",
                    new ServiceProfile
                    {
                        OverrunWeekDaysPrices = new List<TimeslotPrice>
                        {
                            new TimeslotPrice(0, 8, 1.5m),
                            new TimeslotPrice(8, 17, 3.5m),
                            new TimeslotPrice(17, 24, 2.5m)
                        },
                        OverrunWeekendPrices = new List<TimeslotPrice>
                        {
                            new TimeslotPrice(0, 8, 1.8m),
                            new TimeslotPrice(8, 17, 3.8m),
                            new TimeslotPrice(17, 24, 2.8m)
                        },
                        WeekDaysPrices = new List<TimeslotPrice>
                        {
                            new TimeslotPrice(0, 8, 0.5m),
                            new TimeslotPrice(8, 17, 2.5m),
                            new TimeslotPrice(17, 24, 1.5m)
                        },
                        WeekendPrices = new List<TimeslotPrice>
                        {
                            new TimeslotPrice(0, 8, 0.8m),
                            new TimeslotPrice(8, 17, 2.8m),
                            new TimeslotPrice(17, 24, 1.8m)
                        },
                    }
                }
            };
    }
}
