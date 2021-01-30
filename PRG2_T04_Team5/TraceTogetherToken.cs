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
    class TraceTogetherToken
    {

        private string serialNo;

        public string SerialNo
        {
            get { return serialNo; }
            set { serialNo = value; }
        }

        private string collectionLocation;

        public string CollectionLocation
        {
            get { return collectionLocation; }
            set { collectionLocation = value; }
        }

        private DateTime expiryDate;

        public DateTime ExpiryDate
        {
            get { return expiryDate; }
            set { expiryDate = value; }
        }

        public TraceTogetherToken() { }

        public TraceTogetherToken(string serialNo, string collectionLocation, DateTime expiryDate)
        {
            SerialNo = serialNo;
            CollectionLocation = collectionLocation;
            ExpiryDate = expiryDate;
        }

        public bool IsEligibleForReplacement()
        {
            bool eligibility = false;
            if(ExpiryDate == DateTime.Today)
            {
                eligibility = true;
                return eligibility;
            }
            return eligibility;
        }

        public void ReplaceToken(string serialNo, string collectionLocation)
        {
            SerialNo = serialNo;
            CollectionLocation = collectionLocation;
        }

        public override string ToString()
        {
            return "Token Serial Number: " + serialNo + "\tToken Collection Location: " + collectionLocation + "\tToken Expiry Date: " + ExpiryDate;
        }
    }
}
