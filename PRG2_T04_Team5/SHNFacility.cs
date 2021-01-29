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
            get { return distFromLandCheckpoint; }
            set { distFromLandCheckpoint = value; }

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
            double surchage = 1;
            if ((entryDate.TimeOfDay >= (new DateTime(0001,12,31,6,0,0)).TimeOfDay  && entryDate.TimeOfDay <= (new DateTime(0001, 12, 31, 8, 59, 0)).TimeOfDay)
                || (entryDate.TimeOfDay >= (new DateTime(0001, 12, 31, 18, 0, 0)).TimeOfDay && entryDate.TimeOfDay <= (new DateTime(0001, 12, 31, 23, 59, 0)).TimeOfDay))
            {
                surchage = 1.25;
                Console.WriteLine("25% Surchage");
            }
            else if(entryDate.TimeOfDay >= (new DateTime(0001, 12, 31, 0, 0, 0)).TimeOfDay && entryDate.TimeOfDay <= (new DateTime(0001, 12, 31, 5, 59, 0)).TimeOfDay)
            {
                surchage = 1.50;
                Console.WriteLine("50% Surchage");
            }

            double cost = 0;
            if ( entryMode == "Land")
            {
               cost = (DistFromLandCheckpoint * 0.22 + 50) * surchage;
                return cost;
            }
            else if (entryMode == "Sea")
            {
                cost = (DistFromSeaCheckpoint * 0.22 + 50) * surchage;
                return cost;
            }
            else if(entryMode == "Air")
            {
                cost = (DistFromAirCheckpoint * 0.22 + 50) * surchage;
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
