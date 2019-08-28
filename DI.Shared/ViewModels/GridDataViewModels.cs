using DI.Shared.Interfaces;
using System;

namespace DI.Shared.ViewModels
{
    public class InsuranceCompanyViewModel : ISelectable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Cost { get; set; }
    }

    public class CarViewModel : ISelectable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class EventViewModel : ISelectable
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public string Date { get; set; }

        public string Status { get; set; }
    }

    public class BonusLogItemViewModel : ISelectable
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public string Date { get; set; }

        public string Description { get; set; }
    }

    public class SystemMessageViewModel : ISelectable
    {
        public int Id { get; set; }

        public string Description { get; set; }
    }

    public class TripViewModel : ISelectable
    {
        public int Id { get; set; }

        public int TripNumber { get; set; }

        public string AddressStart { get; set; }

        public string AddressFinish { get; set; }

        public string TimeStart { get; set; }

        public string TimeFinish { get; set; }

        public double Distance { get; set; }

        public int Duration { get; set; }

        public float AvgSpeed { get; set; }

        public float MaxSpeed { get; set; }

        public int SharpAccelCount { get; set; }

        public int SharpBrakeCount { get; set; }

        public DateTime Date { get; set; }
    }
}
