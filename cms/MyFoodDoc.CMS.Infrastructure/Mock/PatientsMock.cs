using MyFoodDoc.CMS.Application.Models;
using System;
using System.Collections.Generic;

namespace MyFoodDoc.CMS.Infrastructure.Mock
{
    public static class PatientsMock
    {
        public static readonly IList<PatientModel> Default = new List<PatientModel>()
        {
            new PatientModel
            {
                Id = 0,
                Birth = DateTimeOffset.Now.Date.AddYears(-10),
                FullName = "Patient 0",
                Email = "patient0@mail.de",
                Height = 120,
                Insurance = "AOK",
                Sex = SexEnum.Male,
                Weight = new List<HistoryModel<decimal>>
                {
                    new HistoryModel<decimal>()
                    {
                        Id = 0,
                        Created = DateTimeOffset.Now.AddDays(-10),
                        Value = 30m
                    },
                    new HistoryModel<decimal>()
                    {
                        Id = 1,
                        Created = DateTimeOffset.Now.AddDays(-5),
                        Value = 31m
                    },
                    new HistoryModel<decimal>()
                    {
                        Id = 2,
                        Created = DateTimeOffset.Now.AddDays(-1),
                        Value = 31m
                    }
                },
                AbdominalGirth = new List<HistoryModel<decimal>>
                {
                    new HistoryModel<decimal>()
                    {
                        Id = 0,
                        Created = DateTimeOffset.Now.AddDays(-10),
                        Value = 60m
                    },
                    new HistoryModel<decimal>()
                    {
                        Id = 1,
                        Created = DateTimeOffset.Now.AddDays(-5),
                        Value = 62m
                    },
                    new HistoryModel<decimal>()
                    {
                        Id = 2,
                        Created = DateTimeOffset.Now.AddDays(-1),
                        Value = 62m
                    }
                },
                BloodSugar = new List<HistoryModel<int>>
                {
                    new HistoryModel<int>()
                    {
                        Id = 0,
                        Created = DateTimeOffset.Now.AddDays(-10),
                        Value = 100
                    },
                    new HistoryModel<int>()
                    {
                        Id = 1,
                        Created = DateTimeOffset.Now.AddDays(-5),
                        Value = 110
                    },
                    new HistoryModel<int>()
                    {
                        Id = 2,
                        Created = DateTimeOffset.Now.AddDays(-1),
                        Value = 95
                    }
                }
            },
            new PatientModel
            {
                Id = 1,
                Birth = DateTimeOffset.Now.Date.AddYears(-10).AddMonths(-1).AddDays(-1),
                FullName = "Patient 1",
                Email = "patient1@mail.de",
                Height = 110,
                Insurance = "AOK",
                Sex = SexEnum.Female,
                Weight = new List<HistoryModel<decimal>>
                {
                    new HistoryModel<decimal>()
                    {
                        Id = 3,
                        Created = DateTimeOffset.Now.AddDays(-10),
                        Value = 27m
                    },
                    new HistoryModel<decimal>()
                    {
                        Id = 4,
                        Created = DateTimeOffset.Now.AddDays(-5),
                        Value = 26m
                    },
                    new HistoryModel<decimal>()
                    {
                        Id = 5,
                        Created = DateTimeOffset.Now.AddDays(-1),
                        Value = 27m
                    }
                },
                AbdominalGirth = new List<HistoryModel<decimal>>
                {
                    new HistoryModel<decimal>()
                    {
                        Id = 3,
                        Created = DateTimeOffset.Now.AddDays(-10),
                        Value = 55m
                    },
                    new HistoryModel<decimal>()
                    {
                        Id = 4,
                        Created = DateTimeOffset.Now.AddDays(-5),
                        Value = 54m
                    },
                    new HistoryModel<decimal>()
                    {
                        Id = 5,
                        Created = DateTimeOffset.Now.AddDays(-1),
                        Value = 55m
                    }
                },
                BloodSugar = new List<HistoryModel<int>>
                {
                    new HistoryModel<int>()
                    {
                        Id = 3,
                        Created = DateTimeOffset.Now.AddDays(-10),
                        Value = 110
                    },
                    new HistoryModel<int>()
                    {
                        Id = 4,
                        Created = DateTimeOffset.Now.AddDays(-5),
                        Value = 110
                    },
                    new HistoryModel<int>()
                    {
                        Id = 5,
                        Created = DateTimeOffset.Now.AddDays(-1),
                        Value = 95
                    }
                }
            },
            new PatientModel
            {
                Id = 2,
                Birth = DateTimeOffset.Now.Date.AddYears(-20).AddMonths(-2).AddDays(-2),
                FullName = "Patient 2",
                Email = "patient2@mail.de",
                Height = 170,
                Insurance = "AOK",
                Sex = SexEnum.Female,
                Weight = new List<HistoryModel<decimal>>
                {
                    new HistoryModel<decimal>()
                    {
                        Id = 6,
                        Created = DateTimeOffset.Now.AddDays(-10),
                        Value = 55m
                    },
                    new HistoryModel<decimal>()
                    {
                        Id = 7,
                        Created = DateTimeOffset.Now.AddDays(-5),
                        Value = 55m
                    },
                    new HistoryModel<decimal>()
                    {
                        Id = 8,
                        Created = DateTimeOffset.Now.AddDays(-1),
                        Value = 55m
                    }
                },
                AbdominalGirth = new List<HistoryModel<decimal>>
                {
                    new HistoryModel<decimal>()
                    {
                        Id = 6,
                        Created = DateTimeOffset.Now.AddDays(-10),
                        Value = 70m
                    },
                    new HistoryModel<decimal>()
                    {
                        Id = 7,
                        Created = DateTimeOffset.Now.AddDays(-5),
                        Value = 70m
                    },
                    new HistoryModel<decimal>()
                    {
                        Id = 8,
                        Created = DateTimeOffset.Now.AddDays(-1),
                        Value = 70m
                    }
                },
                BloodSugar = new List<HistoryModel<int>>
                {
                    new HistoryModel<int>()
                    {
                        Id = 6,
                        Created = DateTimeOffset.Now.AddDays(-10),
                        Value = 110
                    },
                    new HistoryModel<int>()
                    {
                        Id = 7,
                        Created = DateTimeOffset.Now.AddDays(-5),
                        Value = 100
                    },
                    new HistoryModel<int>()
                    {
                        Id = 8,
                        Created = DateTimeOffset.Now.AddDays(-1),
                        Value = 105
                    }
                }
            },
            new PatientModel
            {
                Id = 3,
                Birth = DateTimeOffset.Now.Date.AddYears(-30).AddMonths(-3).AddDays(-3),
                FullName = "Patient 3",
                Email = "patient3@mail.de",
                Height = 180,
                Insurance = "AOK",
                Sex = SexEnum.Male,
                Weight = new List<HistoryModel<decimal>>
                {
                    new HistoryModel<decimal>()
                    {
                        Id = 9,
                        Created = DateTimeOffset.Now.AddDays(-10),
                        Value = 70m
                    },
                    new HistoryModel<decimal>()
                    {
                        Id = 10,
                        Created = DateTimeOffset.Now.AddDays(-5),
                        Value = 71m
                    },
                    new HistoryModel<decimal>()
                    {
                        Id = 11,
                        Created = DateTimeOffset.Now.AddDays(-1),
                        Value = 72m
                    }
                },
                AbdominalGirth = new List<HistoryModel<decimal>>
                {
                    new HistoryModel<decimal>()
                    {
                        Id = 9,
                        Created = DateTimeOffset.Now.AddDays(-10),
                        Value = 85m
                    },
                    new HistoryModel<decimal>()
                    {
                        Id = 10,
                        Created = DateTimeOffset.Now.AddDays(-5),
                        Value = 85m
                    },
                    new HistoryModel<decimal>()
                    {
                        Id = 11,
                        Created = DateTimeOffset.Now.AddDays(-1),
                        Value = 86m
                    }
                },
                BloodSugar = new List<HistoryModel<int>>
                {
                    new HistoryModel<int>()
                    {
                        Id = 9,
                        Created = DateTimeOffset.Now.AddDays(-10),
                        Value = 100
                    },
                    new HistoryModel<int>()
                    {
                        Id = 10,
                        Created = DateTimeOffset.Now.AddDays(-5),
                        Value = 100
                    },
                    new HistoryModel<int>()
                    {
                        Id = 11,
                        Created = DateTimeOffset.Now.AddDays(-1),
                        Value = 90
                    }
                }
            }
        };
    }
}
