using System;
using System.Collections.Generic;
using System.Text;

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

        private List<SafeEntry> safeEntry;

        public List<SafeEntry> SafeEntry
        {
            get { return safeEntry; }
            set { safeEntry = value; }
        }

        private List<TravelEntry> travelEntry;

        public List<TravelEntry> TravelEntry
        {
            get { return travelEntry; }
            set { travelEntry = value; }
        }

        public Person() { }
        public Person(string name)
        {
            Name = name;
        }

        public void AddTravelEntry()
        {

        }

        public void AddSafeEntry()
        {

        }

        public abstract double CalculateSHNCharges();

        public override string ToString()
        {
            return "Name: " + Name; 
        }


    }
}
