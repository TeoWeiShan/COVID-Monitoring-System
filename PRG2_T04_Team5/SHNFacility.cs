using System;
using System.Collections.Generic;
using System.Text;

namespace COVID_Monitoring_System
{
    class SHNFacility
    {
        private string facilityName;

        public string FacilityName
        {
            get { return facilityName; }
            set { facilityName = value; }
        }

        private int facilityCapacity;

        public int FacilityCapacity
        {
            get { return facilityCapacity; }
            set { facilityCapacity = value; }
        }

        private int facilityVacancy;

        public int FacilityVacancy
        {
            get { return facilityVacancy; }
            set { facilityVacancy = value; }
        }

        private double distFromAirCheckpoint;

        public double DistFromAirCheckpoint
        {
            get { return distFromAirCheckpoint; }
            set { distFromAirCheckpoint = value; }
        }

        private double distFromSeaCheckpoint;

        public double DistFromSeaCheckpoint
        {
            get { return distFromSeaCheckpoint; }
            set { distFromSeaCheckpoint = value; }
        }

        private double distFromLandCheckpoint;

        public double DistFromLandCheckpoint
        {
            get { return distFromSeaCheckpoint; }
            set { distFromSeaCheckpoint = value; }

        }

        public SHNFacility() { }

        public SHNFacility(string facilityName, int facilityCapacity, double distFromAirCheckpoint, double distFromSeaCheckpoint, double distFromLandCheckpoint)
        {
            FacilityName = facilityName;
            FacilityCapacity = facilityCapacity;
            DistFromAirCheckpoint = distFromAirCheckpoint;
            DistFromSeaCheckpoint = distFromSeaCheckpoint;
            DistFromLandCheckpoint = distFromLandCheckpoint;

        }

        public double CalculateTravelCost(string entryMode, DateTime entryDate)
        {
            double cost = 0;
            if ( entryMode == "Land")
            {
               cost = (DistFromLandCheckpoint * 0.22 + 50) * 1.25;
                return cost;
            }
            else if (entryMode == "Sea")
            {
                cost = (DistFromSeaCheckpoint * 0.22 + 50) * 1.25;
                return cost;
            }
            else if(entryMode == "Air")
            {
                cost = (DistFromAirCheckpoint * 0.22 + 50) * 1.25;
                return cost;
            }

            return cost;
        }

        public bool IsAvailable()
        {
            bool avail = true;
            if (FacilityVacancy == 0)
            {
                avail = false;
                return avail;
            }
            return avail;
            
        }

        public override string ToString()
        {
            return "Facility Name: " + FacilityName + "\tFacility Capacity: " + FacilityCapacity;
        }
    }
}
