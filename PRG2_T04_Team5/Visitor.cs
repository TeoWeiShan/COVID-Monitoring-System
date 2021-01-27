using System;
using System.Collections.Generic;
using System.Text;

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
            if ((TravelEntryList[TravelEntryList.Count - 1].SHNEndDate - TravelEntryList[TravelEntryList.Count - 1].EntryDate).Days == 14)
            {
                addCost = 2000;
                //addCost = SHNFacility.CalculateTravelCost(TravelEntryList[TravelEntryList.Count - 1].EntryMode, TravelEntryList[TravelEntryList.Count - 1].EntryDate);
                Console.WriteLine("14 Days");
            }
            else if ((TravelEntryList[TravelEntryList.Count - 1].SHNEndDate - TravelEntryList[TravelEntryList.Count - 1].EntryDate).Days == 7)
            {
                addCost = 80;
                Console.WriteLine("7 Days");
            }
            else if ((TravelEntryList[TravelEntryList.Count - 1].SHNEndDate - TravelEntryList[TravelEntryList.Count - 1].EntryDate).Days == 0)
            {
                addCost = 80;
                Console.WriteLine("0 Days");
            }

            return (addCost + cost);

        }

        public override string ToString()
        {
            return "Name: " + Name + "\tPassport No: " + PassportNo + "\tNationality: " + Nationality;
        }


    }
}
