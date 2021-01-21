﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;


namespace COVID_Monitoring_System
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to COVID Monitoring System");

            List<SHNFacility> SHNFacilityList = new List<SHNFacility>();
            List<Person> personList = new List<Person> ();
            List<BusinessLocation> businessList = new List<BusinessLocation>();

            

            
            while (true)
            {
                
                Console.WriteLine("\n========================================\n" +
                    "1) Load Person and Business Location Data\n" +
                    "2) Load SHN Facility Data\n" +
                    "3) List all Visitors\n" +
                    "4) List Person Details");
                Console.WriteLine("========================================");
                Console.Write("Please Enter An Option: ");
                string option = Console.ReadLine();
                Console.WriteLine("========================================\n");

                if (option == "1")
                {
                    LoadPersonBusinessData(personList);
                    //***Invalid input will pop up***
                }
                if (option == "2")
                {
                    LoadSHNFacilityData(SHNFacilityList);
                    //***Invalid input will pop up***
                }
                if (option == "3")
                {
                    ListVisitors(personList);
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again.");
                }

            }

            static void LoadPersonBusinessData(List<Person> personList)
            {
                string[] csvLinesPerson = File.ReadAllLines("Person.csv");
                for (int i = 1; i < csvLinesPerson.Length; i++)
                {
                    string[] line = csvLinesPerson[i].Split(',');
                    if (line[0] == "visitor")
                    {
                        Person p = new Visitor(line[1], line[4], line[5]);
                        personList.Add(p);

                    }
                    if (line[0] == "resident")
                    {
                        Person p = new Resident(line[1], line[2], Convert.ToDateTime(line[3]));
                        personList.Add(p);
                    }

                }
                Console.WriteLine("All data has been loaded.");
                foreach (Person p in personList)
                {
                    Console.WriteLine(p.ToString());
                }

            }
            static void LoadSHNFacilityData(List<SHNFacility> SHNFacilityList)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://covidmonitoringapiprg2.azurewebsites.net");
                    Task<HttpResponseMessage> responseTask = client.GetAsync("/facility");
                    responseTask.Wait();
                    HttpResponseMessage result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        Task<string> readTask = result.Content.ReadAsStringAsync();
                        readTask.Wait();
                        string data = readTask.Result;
                        SHNFacilityList = JsonConvert.DeserializeObject<List<SHNFacility>>(data);
                        
                    }

                }

            }
            static void ListVisitors(List<Person> personList)
            {
                if (personList.Count == 0)
                {
                    Console.WriteLine("No data has been loaded.");
                }
                else
                {
                    foreach (Person p in personList)
                    {
                        if (p is Visitor)
                        {
                            Console.WriteLine(p.ToString());
                        }
                        
                    }
                }
                
            }



        }
    }
}
