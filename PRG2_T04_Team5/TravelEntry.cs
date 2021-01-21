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
            
        }

        public void CalculateSHNDuration()
        {
            if (LastCountryOfEmbarkation == "New Zealand" || LastCountryOfEmbarkation == "Vietnam")
            {
                
            }
        }

        public override string ToString()
        {
            return "Last Country of Embarkation" + LastCountryOfEmbarkation + "Entry Mode" + EntryMode + "Entry Date" + EntryDate + "SHNEndDate" + SHNEndDate + "SHN Stay" + (SHNStay) ;
        }
    }
}
