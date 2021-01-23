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
            throw new NotImplementedException();

        }

        public override string ToString()
        {
            return "Name: " + Name + "\tPassport No: " + PassportNo + "\tNationality: " + Nationality;
        }


    }
}
