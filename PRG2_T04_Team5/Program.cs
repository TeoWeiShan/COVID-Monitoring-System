using System;
using System.Collections.Generic;
using System.IO;

namespace COVID_Monitoring_System
{
    class Program
    {
        static void Main(string[] args)
        {

            List<SHNFacility> SHNFacilityList = new List<SHNFacility>();
            List<Person> personList = new List<Person> ();
            List<BusinessLocation> businessList = new List<BusinessLocation>();

            string[] csvLinesPerson = File.ReadAllLines("Person.csv");
            for (int i = 1; i < csvLinesPerson.Length; i++)
            {
                string[] line = csvLinesPerson[i].Split(',');
                Person p = new Person(
                Console.WriteLine("{0,10}  {1,10}  {2,10}  {3,10}", marks[0],
                        marks[1], marks[2], average.ToString("0.00"));
            }


            string[]


        }
    }
}
