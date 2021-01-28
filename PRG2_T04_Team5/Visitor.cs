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
            TravelEntry last = TravelEntryList[TravelEntryList.Count - 1];
            int duration = (last.SHNEndDate - last.EntryDate).Days;
            if ( duration== 14)
            {
                addCost = last.SHNStay.CalculateTravelCost(last.EntryMode, last.EntryDate) + 2000;
                Console.WriteLine("14 Days");
            }
            else if (duration == 7)
            {
                addCost = 80;
                Console.WriteLine("7 Days");
            }
            else if (duration == 0)
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
