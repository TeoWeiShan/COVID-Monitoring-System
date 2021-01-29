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
    class SafeEntry
    {
        private DateTime checkIn;

        public DateTime CheckIn
        {
            get { return checkIn; }
            set { checkIn = value; }
        }

        private DateTime checkOut;

        public DateTime CheckOut
        {
            get { return checkOut; }
            set { checkOut = value; }
        }

        private BusinessLocation location;

        public BusinessLocation Location
        {
            get { return location; }
            set { location = value; }
        }

        public SafeEntry(DateTime checkin) { }

        public SafeEntry(DateTime checkIn, BusinessLocation location)
        {
            CheckIn = checkIn;
            Location = location;
        }

        public void PerformCheckOut() { }

        public override string ToString()
        {
            return "check in" + CheckIn + "location" + Location;
        }

    }
}
