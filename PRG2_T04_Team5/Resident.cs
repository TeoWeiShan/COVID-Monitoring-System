using System;
using System.Collections.Generic;
using System.Text;

namespace COVID_Monitoring_System
{
    class Resident:Person
    {
        private string address;

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        private DateTime lastLeftCountry;

        public DateTime LastLeftCountry
        {
            get { return lastLeftCountry; }
            set { lastLeftCountry = value; }
        }

        private TraceTogetherToken token;

        public TraceTogetherToken Token
        {
            get { return token; }
            set { token = value; }
        }

        public Resident (string name, string address, DateTime lastLeftCountry)
        {
            Name = name;
            Address = address;
            LastLeftCountry = lastLeftCountry;
        }

        public override double CalculateSHNCharges()
        {
            double cost = 200;
            double addCost = 0;
            if ((TravelEntryList[TravelEntryList.Count - 1].SHNEndDate - TravelEntryList[TravelEntryList.Count - 1].EntryDate).Days == 14)
            {
                Console.WriteLine("14 Days");
                addCost = 1020;

            }
            else if ((TravelEntryList[TravelEntryList.Count - 1].SHNEndDate - TravelEntryList[TravelEntryList.Count - 1].EntryDate).Days == 7)
            {
                Console.WriteLine("7 Days");
                addCost = 20;
            }
            else if ((TravelEntryList[TravelEntryList.Count - 1].SHNEndDate - TravelEntryList[TravelEntryList.Count - 1].EntryDate).Days == 0)
            {
                Console.WriteLine("0 Days");
            }

            return (addCost + cost);

            //throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "Name:  " + Name + "\tAddress: " + Address + "\tlastLeftCountry: " + LastLeftCountry.ToString("dd/MM/yyyy");
        }
    }
}
