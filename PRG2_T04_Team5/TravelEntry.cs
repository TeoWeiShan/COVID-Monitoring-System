using System;
using System.Collections.Generic;
using System.Text;

namespace COVID_Monitoring_System
{
    class TravelEntry
    {
        private string lastCountryOfEmbarkation;

        public string LastCountryOfEmbarkation
        {
            get { return lastCountryOfEmbarkation; }
            set { lastCountryOfEmbarkation = value; }
        }

        private string entryMode;

        public string EntryMode
        {
            get { return entryMode; }
            set { entryMode = value; }
        }

        private DateTime entryDate;

        public DateTime EntryDate
        {
            get { return entryDate; }
            set { entryDate = value; }
        }

        private DateTime shnEndDate;

        public DateTime SHNEndDate
        {
            get { return shnEndDate; }
            set { shnEndDate = value; }
        }

        private SHNFacility shnStay;

        public SHNFacility SHNStay
        {
            get { return shnStay; }
            set { shnStay = value; }
        }

        private bool isPaid;

        public bool IsPaid
        {
            get { return isPaid; }
            set { isPaid = value; }
        }

        public TravelEntry() { }

        public TravelEntry(string lastCountryOfEmbarkation, string entryMode, DateTime entryDate)
        {
            LastCountryOfEmbarkation = lastCountryOfEmbarkation;
            EntryMode = entryMode;
            EntryDate = entryDate;
        }

        public void AssignSHNFacility(SHNFacility SHNStay)
        {
            throw new NotImplementedException();
        }

        public void CalculateSHNDuration()
        {
            if (LastCountryOfEmbarkation == "New Zealand" || LastCountryOfEmbarkation == "Vietnam")
            {
                Console.WriteLine("0 Days of SHN");
SHNEndDate = EntryDate.AddDays(0);
                
            }
            else if (LastCountryOfEmbarkation == "Macao SAR")
            {
                Console.WriteLine("7 Days of SHN");
                SHNEndDate = EntryDate.AddDays(7);
            }
            else
            {
                Console.WriteLine("14 Days of SHN");
                SHNEndDate = EntryDate.AddDays(14);
            }
        }

        public override string ToString()
        {
            return "Last Country of Embarkation: " + LastCountryOfEmbarkation + "\tEntry Mode: " + EntryMode + "\tEntry Date: " + EntryDate ;
        }
    }
}
