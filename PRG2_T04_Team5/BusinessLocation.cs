using System;
using System.Collections.Generic;
using System.Text;

namespace COVID_Monitoring_System
{
    class BusinessLocation
    {
        private string businessName;

        public string BusinessName
        {
            get { return businessName; }
            set { businessName = value; }
        }

        private string branchCode;

        public string BranchCode
        {
            get { return branchCode; }
            set { branchCode = value; }
        }

        private int maximumCapacity;

        public int MaximumCapacity
        {
            get { return maximumCapacity; }
            set { maximumCapacity = value; }
        }

        private int visitorsNow;

        public int VisitorsNow
        {
            get { return visitorsNow; }
            set { visitorsNow = value; }
        }

        public BusinessLocation() { }

        public BusinessLocation(string businessName, string branchCode, int maximumCapacity)
        {
            BusinessName = businessName;
            BranchCode = branchCode;
            MaximumCapacity = maximumCapacity;
        }

        public bool IsFull()
        {
            bool full = false;
            if (VisitorsNow == MaximumCapacity)
            {
                full = true;
                return full;
            }
            return full;
        }

        public override string ToString()
        {
            return "Business Name: " + BusinessName + "\tBranch Code: " + BranchCode + "\tMaximum Capacity: " + MaximumCapacity; 
        }
    }
}
