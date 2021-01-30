using System;
using System.Collections.Generic;
using System.Text;

//============================================================
// Student Number : S10205156 , S10205409
// Student Name : Teo Wei Shan, Angelica Sim
// Module Group : T04
//============================================================

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
            TravelEntry last = TravelEntryList[TravelEntryList.Count - 1];
            int duration = (last.SHNEndDate - last.EntryDate).Days;
            Console.WriteLine("Swab test charges (before GST): $" + cost);
            if (duration == 14)
            {
                double tpCost = 20;
                double SDFCost = 1000;
                Console.WriteLine("Duration of SHN: 14 Days");
                Console.WriteLine("Transporation charges (before GST): $" + tpCost);
                Console.WriteLine("SDF charge (before GST): $" + SDFCost);
                addCost = tpCost + SDFCost;

            }
            else if (duration == 7)
            {
                double tpCost = 20;
                Console.WriteLine("Duration of SHN: 7 Days");
                Console.WriteLine("Transporation charges (before GST): $" + tpCost);
                addCost = tpCost;
            }
            else if (duration == 0)
            {
                Console.WriteLine("Duration of SHN: 0 Days");
            }
            return (addCost + cost);
        }

        public override string ToString()
        {
            return "Name: " + Name + "\tAddress: " + Address + "\tLast Left Country: " + LastLeftCountry.ToString("dd/MM/yyyy");
        }
    }
}
