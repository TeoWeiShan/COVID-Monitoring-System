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
    abstract class Person
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private List<SafeEntry> safeEntryList;

        public List<SafeEntry> SafeEntryList
        {
            get { return safeEntryList; }
            set { safeEntryList = value; }
        }

        private List<TravelEntry> travelEntryList;

        public List<TravelEntry> TravelEntryList
        {
            get { return travelEntryList; }
            set { travelEntryList = value; }
        }

        public Person() { TravelEntryList = new List<TravelEntry>(); SafeEntryList = new List<SafeEntry>(); }
        public Person(string name)
        {
            Name = name;
            TravelEntryList = new List<TravelEntry>();
            SafeEntryList = new List<SafeEntry>();
        }

        public void AddTravelEntry(TravelEntry entry)
        {
            TravelEntryList.Add(entry);
        }

        public void AddSafeEntry(SafeEntry entry)
        {
            SafeEntryList.Add(entry);
        }

        public abstract double CalculateSHNCharges();

        public override string ToString()
        {
            return "Name: " + Name; 
        }


    }
}
