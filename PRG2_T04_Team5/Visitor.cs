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
    class Visitor:Person
    {
        private string passportNo;

        public string PassportNo
        {
            get { return passportNo; }
            set { passportNo = value; }
        }

        private string nationality;

        public string Nationality
        {
            get { return nationality; }
            set { nationality = value; }
        }

        public Visitor(string name, string passportNo, string nationality)
        {
            Name = name;
            PassportNo = passportNo;
            Nationality = nationality;
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
                double tpCost = last.SHNStay.CalculateTravelCost(last.EntryMode, last.EntryDate);
                double SDFCost = 2000;
                Console.WriteLine("Duration of SHN: 14 Days");
                Console.WriteLine("SDF charge (before GST): $" + SDFCost);
                addCost = tpCost + SDFCost;

            }
            else if (duration == 7)
            {
                double tpCost = 80;
                Console.WriteLine("Duration of SHN: 7 Days");
                Console.WriteLine("Transporation charges (before GST): $" + tpCost);
                addCost = tpCost;
            }
            else if (duration == 0)
            {
                double tpCost = 80;
                Console.WriteLine("Duration of SHN: 0 Days");
                Console.WriteLine("Transporation charges (before GST): $" + tpCost);
                addCost = tpCost;
            }
            return (addCost + cost);
        }

        public override string ToString()
        {
            return "Name: " + Name + "\tPassport No: " + PassportNo + "\tNationality: " + Nationality;
        }


    }
}
