using System;
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
            List<Person> personList = new List<Person>();
            List<BusinessLocation> businessList = new List<BusinessLocation>();


            while (true)
            {

                Console.WriteLine("\n========================================\n" +
                    "\n===General===\n" +
                    "1) Load Person and Business Location Data\n" +
                    "2) Load SHN Facility Data\n" +
                    "3) List all Visitors\n" +
                    "4) List Person Details\n" +
                    "\n===SafeEntry/TraceTogether===\n" +
                    "5) Assign/Replace TraceTogether Token\n" +
                    "6) List all Business Locations\n" +
                    "7) Edit Business Location Capacity\n" +
                    "8) SafeEntry Check-in\n" +
                    "9) SafeEntry Check-out\n" +
                    "\n===TravelEntry===\n" +
                    "10) List all SHN Facilities\n" +
                    "11) Create Visitor\n" +
                    "12) Create TravelEntry Record\n" +
                    "13) Calculate SHN Charges\n");
                Console.WriteLine("========================================");
                Console.Write("Please Enter An Option: ");
                string option = Console.ReadLine();
                Console.WriteLine("========================================\n");

                //===General===
                if (option == "1")
                {
                    LoadPersonBusinessData(personList, businessList);
                }
                else if (option == "2")
                {
                    LoadSHNFacilityData(SHNFacilityList);
                }
                else if (option == "3")
                {
                    ListVisitors(personList);
                }
                else if (option == "4")
                {
                    ListPersonDetails(personList);
                }

                //===SafeEntry/TraceTogether===
                else if (option == "5")
                {
                    TraceTogetherToken(personList);
                }

                else if (option == "6")
                {
                    ListBizLocations(businessList);
                }

                else if (option == "7")
                {
                    EditBizLocationCap(businessList);
                }

                else if (option == "8")
                {
                    SafeEntryCheckIn(personList, businessList);
                }

                else if (option == "9")
                {
                    SafeEntryCheckOut(personList, businessList);
                }


                //===TravelEntry===
                else if (option == "10")
                {
                    ListAllSHNFacilities(SHNFacilityList);
                }
                else if (option == "11")
                {
                    CreateVisitor(personList);
                }
                else if (option == "12")
                {
                    CreateTravelEntryRecord(personList, SHNFacilityList);
                }
                else if (option == "13")
                {
                    CalculateSHNCharges(personList);
                }


                else
                {
                    Console.WriteLine("Invalid input, please try again.");
                }
            }


            //===General===
            static void LoadPersonBusinessData(List<Person> personList, List<BusinessLocation> businessList)
            {
                string[] csvLinesPerson = File.ReadAllLines("Person.csv");
                for (int i = 1; i < csvLinesPerson.Length; i++)
                {
                    string[] line = csvLinesPerson[i].Split(',');
                    if (line[0] == "visitor")
                    {
                        Person p = new Visitor(line[1], line[4], line[5]);

                        if (line[9] != "" && line[10] != "" && line[11] != "")
                        {
                            TravelEntry te = new TravelEntry(line[9], line[10], Convert.ToDateTime(line[11]) );
                            te.IsPaid = Convert.ToBoolean(line[14]);
                            //te.SHNStay = new SHNFacility(line[15]);
                            p.AddTravelEntry(te);
                        }


                        personList.Add(p);

                    }
                    if (line[0] == "resident")
                    {
                        Person p = new Resident(line[1], line[2], Convert.ToDateTime(line[3]));
                        if (line[9] != "" && line[10] != "" && line[11] != "")
                        {
                            TravelEntry te = new TravelEntry(line[9], line[10], Convert.ToDateTime(line[11]));
                            p.AddTravelEntry(te);
                        }
                        personList.Add(p);
                    }

                }


                string[] csvLinesBusiness = File.ReadAllLines("BusinessLocation.csv");
                for (int i = 1; i < csvLinesBusiness.Length; i++)
                {
                    string[] line = csvLinesBusiness[i].Split(',');
                    BusinessLocation b = new BusinessLocation(line[0], line[1], Convert.ToInt32(line[2]));
                    businessList.Add(b);
                }

                Console.WriteLine("All data has been loaded.");
                foreach (Person p in personList)
                {
                    Console.WriteLine(p.ToString());
                }

                foreach (BusinessLocation b in businessList)
                {
                    Console.WriteLine(b.ToString());
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

                    ListAllSHNFacilities(SHNFacilityList);

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

            static void ListPersonDetails(List<Person> personList)
            {
                Console.Write("Enter person name: ");
                string name = Console.ReadLine();
                bool found = false;
                foreach (Person p in personList)
                {

                    if (p.Name == name)
                    {
                        Console.WriteLine("Person Found");
                        Console.WriteLine(p);
                        if (p.TravelEntryList.Count == 0)
                        {
                            Console.WriteLine("No Travel Entry Details");

                        }
                        else
                        {
                            Console.WriteLine(p.TravelEntryList[p.TravelEntryList.Count - 1]);
                            Console.WriteLine(p.TravelEntryList[p.TravelEntryList.Count - 1].IsPaid);
                        }




                        found = true;
                        break;
                    }
                }
                if (!found) Console.WriteLine("Person is not found.");


            }

            //===SafeEntry/TraceTogether===
            static void TraceTogetherToken(List<Person> personList) //incomplete
            {
                Console.WriteLine("Enter your name: ");
                string name = Console.ReadLine();
                bool found = false;
                foreach (Resident r in personList)
                {
                    if (r.Name == name) //create & assign Token object if no token. replace if >6 months
                    {
                        found = true;
                        //TraceTogetherToken t = new TraceTogetherToken(serialNo, collectionLocation, expiryDate);
                        //r.AddTraceTogetherToken(t);
                        break;
                    }

                    else if (!found) Console.WriteLine("Business not found. Please try again.");
                }
            }

            static void ListBizLocations(List<BusinessLocation> businessList)
            {
                foreach (BusinessLocation b in businessList)
                {
                    Console.WriteLine(b);
                }
            }

            static void EditBizLocationCap(List<BusinessLocation> businessList)
            {
                Console.WriteLine("Enter business name: ");
                string bizname = Console.ReadLine();
                bool found = false;
                foreach (BusinessLocation b in businessList)
                {
                    if (b.BusinessName == bizname)
                    {
                        found = true;
                        Console.WriteLine("Enter the new max capacity: ");
                        int newmaxcap = Convert.ToInt32(Console.ReadLine());
                        b.MaximumCapacity = newmaxcap;
                        Console.Write("Max capacity of " +b+"has been updated.");
                        break;
                    }
                    else if (!found) Console.WriteLine("Business not found. Please try again.");
                }

            }

            static void SafeEntryCheckIn(List<Person> personList, List<BusinessLocation> businessList)
            {
                Console.WriteLine("Enter your name: ");
                string name = Console.ReadLine();
                bool found = false;
                foreach (Person p in personList)
                {
                    
                    if (p.Name == name)
                    {
                        found = true;
                        //Console.WriteLine(p.Name); test
                        foreach (BusinessLocation b in businessList)
                        {
                            Console.WriteLine(b);
                        }
                        DateTime checkin = DateTime.Now;
                        Console.WriteLine("Select business location: ");
                        string location = Console.ReadLine();
                        foreach (BusinessLocation b in businessList)
                        {
                            if (b.BusinessName == location)
                            {
                                if (b.MaximumCapacity == 0)
                                {
                                    Console.WriteLine("Location full. Please try again later.");
                                }
                                else
                                {
                                    SafeEntry e = new SafeEntry(checkin, b);
                                    p.AddSafeEntry(e);
                                    b.VisitorsNow = b.VisitorsNow + 1;
                                    Console.WriteLine("You have successfully checked-in.");
                                    break;
                                }
                            }
                        }
                    }
                    else if (!found) Console.WriteLine("Name not found. Please try again.");
                }
            }

            static void SafeEntryCheckOut(List<Person> personList, List<BusinessLocation> businessList)
            {
                Console.WriteLine("Enter your name: ");
                string name = Console.ReadLine();
                bool found = false;
                foreach (Person p in personList)
                {

                    if (p.Name == name)
                    {
                        found = true;
                        Console.WriteLine(p);
                        Console.WriteLine("Select a record to check-out.");
                        string rec = Console.ReadLine();
                        Console.WriteLine(SafeEntry.PerformCheckOut()); //error
                        foreach (BusinessLocation b in businessList)
                        {
                            b.VisitorsNow = b.VisitorsNow - 1;
                            break;
                        }
                        Console.Write("You have successfully checked-out.");
                    }

                    if (!found)
                    {
                        Console.WriteLine("Invalid input. Please try again.");
                    }

                }
            }

            //===TravelEntry===
            static void ListAllSHNFacilities(List<SHNFacility> SHNFacilityList)
            {
                foreach (SHNFacility f in SHNFacilityList)
                {
                    Console.WriteLine(f);
                }
            }


            static void CreateVisitor(List<Person> personList)
            {
                Console.Write("Enter visitor name: ");
                string name = Console.ReadLine();
                Console.Write("Enter visitor passport number: ");
                string passportNo = Console.ReadLine();
                Console.Write("Enter visitor nationality: ");
                string nationality = Console.ReadLine();
                Person p = new Visitor(name, passportNo, nationality);
                personList.Add(p);
            }

            static void CreateTravelEntryRecord(List<Person> personList, List<SHNFacility> SHNFacilityList)
            {
                Console.Write("Enter person name: ");
                string name = Console.ReadLine();
                bool found = false;
                foreach (Person p in personList)
                {
                    if (p.Name == name)
                    {
                        found = true;
                        Console.WriteLine("Enter last country of embarkation: ");
                        string lastCountry = Console.ReadLine();
                        Console.WriteLine("Enter entry mode: ");
                        string entryMode = Console.ReadLine();
                        Console.WriteLine("Enter entry date: ");
                        DateTime entryDate = Convert.ToDateTime(Console.ReadLine());
                        TravelEntry e = new TravelEntry(lastCountry, entryMode, entryDate);
                        e.CalculateSHNDuration();
                        Console.WriteLine(e.SHNEndDate);
                        if (e.SHNEndDate == e.EntryDate.AddDays(14))
                        {
                            Console.WriteLine("Loading SHN Facilities. Please Wait.");
                            LoadSHNFacilityData(SHNFacilityList);
                            Console.Write("Enter SHN Facility Name: ");
                            string fName = Console.ReadLine();
                            //e.AssignSHNFacility();


                        }
                        p.AddTravelEntry(e);
                        Console.WriteLine("Travel Entry Record Created.");
                        break;
                    }
                }
                if (!found) Console.WriteLine("Person is not found.");

            }

            static void CalculateSHNCharges(List<Person> personList)
            {
                Console.Write("Enter person name: ");
                string name = Console.ReadLine();

                bool found = false;
                foreach (Person p in personList)
                {

                    if (p.Name == name && p.TravelEntryList[0].SHNEndDate != DateTime.Now)
                    {
                        Console.WriteLine(p.TravelEntryList[0]);
                        Console.WriteLine("Please make payment. Enter 'Y' after payment is made.");
                        string payment = Console.ReadLine();
                        if (payment == "Y")
                        {
                            p.TravelEntryList[0].IsPaid = true;
                            Console.WriteLine(p.TravelEntryList[0].IsPaid);
                        }


                        found = true;
                        break;
                    }
                }
                if (!found) Console.WriteLine("Person is not found.");
            }
        }
    }
}