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
            List<SafeEntry> safeEntryList = new List<SafeEntry>();



            //Force users to load facility API to ensure program can work
            bool loadedAPI = false;
            while (loadedAPI == false)
            {
                Console.WriteLine("\n========================================\n" +
                   "\n===General===\n" +

                   "1) Load SHN Facility Data\n");

                Console.WriteLine("========================================");
                Console.Write("Please enter '1' to load data: ");
                string option = Console.ReadLine();
                Console.WriteLine("========================================\n");
                //Enter 1 to confirm loading of data, else data will not be loaded and cannot access program
                if (option == "1")
                {
                    SHNFacilityList = LoadSHNFacilityData();
                    Console.WriteLine("Displaying all loaded data...\n");
                    ListAllSHNFacilities(SHNFacilityList);
                    loadedAPI = true;
                }
                else
                {
                    //Validation: only 1 to proceed with program
                    Console.WriteLine("Invalid input. Please try again.");
                }

            }
            //Force users to load person business data to ensure program can work
            bool loadedCSV = false;
            while (loadedCSV == false)
            {
                Console.WriteLine("\n========================================\n" +
                   "\n===General===\n" +
                   "2) Load Person and Business Location Data\n");
                Console.WriteLine("========================================");
                Console.Write("Please enter '2' to load data: ");
                string option = Console.ReadLine();
                Console.WriteLine("========================================\n");
                //Enter 1 to confirm loading of data, else data will not be loaded and cannot access program
                if (option == "2")
                {
                    LoadPersonBusinessData(personList, businessList, SHNFacilityList);
                    loadedCSV = true;


                }
                else
                {
                    //Validation: only 2 to proceed with program
                    Console.WriteLine("Invalid input. Please try again.");
                }


            }

            //Display menu
            while (true)
            {

                Console.WriteLine("\n========================================\n" +
                    "\n===General===\n" +

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

                if (option == "3")
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
                    TraceTogetherToken(personList, safeEntryList);
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
                    //Validation
                    Console.WriteLine("Invalid input, please try again.");
                }
            }


            //===General===
            static void LoadPersonBusinessData(List<Person> personList, List<BusinessLocation> businessList, List<SHNFacility> SHNFacilityList)
            {
                try
                {
                    //Extract data from csv and append to make new Person objects
                    string[] csvLinesPerson = File.ReadAllLines("Person.csv");
                    for (int i = 1; i < csvLinesPerson.Length; i++)
                    {
                        string[] line = csvLinesPerson[i].Split(',');
                        if (line[0] == "visitor")
                        {
                            //Input data to Visitor
                            Person p = new Visitor(line[1], line[4], line[5]);

                            //Input data to Vistor with Travel Entry
                            if (line[9] != "" && line[10] != "" && line[11] != "")
                            {
                                TravelEntry te = new TravelEntry(line[9], line[10], Convert.ToDateTime(line[11]));
                                te.SHNEndDate = Convert.ToDateTime(line[12]);
                                te.IsPaid = Convert.ToBoolean(line[13]);
                                //Input data to Vistor staying at SHN
                                if (line[14] != "")
                                {
                                    foreach (SHNFacility f in SHNFacilityList)
                                    {
                                        if (f.FacilityName == line[14])
                                        {
                                            te.AssignSHNFacility(new SHNFacility(f.FacilityName, f.FacilityCapacity, f.DistFromAirCheckpoint, f.DistFromSeaCheckpoint, f.DistFromLandCheckpoint)); ;
                                        }
                                    }
                                }
                                p.AddTravelEntry(te);

                            }


                            personList.Add(p);

                        }
                        if (line[0] == "resident")
                        {
                            //Input data to Resident
                            Person p = new Resident(line[1], line[2], Convert.ToDateTime(line[3]));
                            //Input data to Resident with Travel Entry
                            if (line[9] != "" && line[10] != "" && line[11] != "")
                            {
                                TravelEntry te = new TravelEntry(line[9], line[10], Convert.ToDateTime(line[11]));
                                te.SHNEndDate = Convert.ToDateTime(line[12]);
                                te.IsPaid = Convert.ToBoolean(line[13]);

                                p.AddTravelEntry(te);

                            }
                            personList.Add(p);
                        }

                    }

                    //Extract data from csv and append to make new BuisnessLocation objects
                    string[] csvLinesBusiness = File.ReadAllLines("BusinessLocation.csv");
                    for (int i = 1; i < csvLinesBusiness.Length; i++)
                    {
                        //Input data to BuisnessLocation
                        string[] line = csvLinesBusiness[i].Split(',');
                        BusinessLocation b = new BusinessLocation(line[0], line[1], Convert.ToInt32(line[2]));
                        businessList.Add(b);
                    }


                }
                //Validation: File unavailable
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Files cannot be found. Exiting program...");
                    Environment.Exit(0);
                }
                //Successfully loaded data, display data loaded for Person and Business
                Console.WriteLine("Successfully loaded data."
                    + "\nDisplaying all loaded data...\n");
                Console.WriteLine("People Data: \n");
                foreach (Person p in personList)
                {
                    Console.WriteLine(p.ToString());
                }
                Console.WriteLine("\nBuisness Data: \n");
                foreach (BusinessLocation b in businessList)
                {
                    Console.WriteLine(b.ToString());

                }

            }

            static List<SHNFacility> LoadSHNFacilityData()
            {
                List<SHNFacility> shnList = new List<SHNFacility>();
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        //Extract data from API and append to make new SHNFacility objects
                        client.BaseAddress = new Uri("https://covidmonitoringapiprg2.azurewebsites.net");
                        Task<HttpResponseMessage> responseTask = client.GetAsync("/facility");
                        responseTask.Wait();
                        HttpResponseMessage result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            Task<string> readTask = result.Content.ReadAsStringAsync();
                            readTask.Wait();
                            string data = readTask.Result;
                            shnList = JsonConvert.DeserializeObject<List<SHNFacility>>(data);
                            //Create FacilityCapacity for each object
                            foreach (SHNFacility f in shnList)
                            {
                                f.FacilityVacancy = f.FacilityCapacity;
                            }
                            Console.WriteLine("Successfully loaded data.");
                        }
                        //Validaion: for data that cannot be loaded
                        else
                        {
                            Console.WriteLine("Data cannot be loaded. Exiting program...");
                            Environment.Exit(0);
                        }
                    }
                    //Validaion: for data that cannot be loaded
                    catch (AggregateException)
                    {
                        Console.WriteLine("Data cannot be loaded. Exiting program...");
                        Environment.Exit(0);
                    }


                    return shnList;


                }

            }

            static void ListVisitors(List<Person> personList)
            {
                //Displays all vistors
                Console.WriteLine("Displaying all visitors: ");
                foreach (Person p in personList)
                {
                    if (p is Visitor)
                    {
                        Console.WriteLine(p.ToString());
                    }
                }

            }

            static void ListPersonDetails(List<Person> personList)
            {
                //Displays a person detail based on name search
                
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
                            Console.WriteLine("Most recent travel entry detail: ");
                            TravelEntry last = p.TravelEntryList[p.TravelEntryList.Count - 1];
                            Console.WriteLine(last.ToString(), "SHN Fee Paid: ", last.IsPaid);
                            if (last.SHNStay != null)
                            {
                                Console.WriteLine(last.SHNStay);
                            }
                            else
                            {
                                Console.WriteLine("No SHN Facility Stay Details");
                            }
                            
                        }




                        found = true;
                        break;
                    }
                }
                //Validation: also validation for inproper input
                if (!found) Console.WriteLine("Person is not found.");


            }


            //===SafeEntry/TraceTogether===
            static void TraceTogetherToken(List<Person> personList, List<SafeEntry> safeEntryList) 
            {
                Console.WriteLine("Enter your name: ");
                string name = Console.ReadLine();
                bool found = false;
                foreach (Person p in personList)
                {
                    if (p is Resident)
                    {
                        found = true;
                        Resident r = (Resident)p;
                        if (r.Token is null)
                        {
                            string serialNo = ("T" + 12345);
                            string newserialNo = (serialNo + 1);
                            Console.WriteLine("Enter your preferred collection location.");
                            string collectionLocation = Console.ReadLine();
                            DateTime Date = DateTime.Today;
                            DateTime expiryDate = Date.AddMonths(6);
                            TraceTogetherToken t = new TraceTogetherToken(newserialNo, collectionLocation, expiryDate);
                            //add and assign token
                            r.Token = t;
                            Console.WriteLine("Your TraceTogether token s/n: " + newserialNo + "\nYour collection location: " + collectionLocation + "\nToken expiry date: " + expiryDate);
                            break;
                        }
                        
                        else
                        {
                            if (r.Token.ExpiryDate < DateTime.Now.AddMonths(1))
                            {
                                r.Token.IsEligibleForReplacement();
                                Console.WriteLine("Your token is expiring soon on " + r.Token.ExpiryDate + ". Would you like to replace it?");
                                Console.WriteLine("(1) Yes \n(2) No");
                                string choice = Console.ReadLine();
                                if (choice == "Yes")
                                {
                                    r.Token.IsEligibleForReplacement();
                                }
                                else if (choice == "No")
                                {
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid input. Please choose yes/no.");
                                }
                            } 
                        }
                    }
                    else if (!found) Console.WriteLine("Name not found. Please try again.");
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
                        Console.Write("Max capacity of " + bizname + "has been updated.");
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
                        p.SafeEntryList[p.SafeEntryList.Count - 1].PerformCheckOut();
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
                    Console.WriteLine(f + "\tFacility Vacancy: " + f.FacilityVacancy);
                }
            }


            static void CreateVisitor(List<Person> personList)
            {

                Console.Write("Enter visitor name: ");
                string name = Console.ReadLine();
                bool nameExist = false;
                foreach (Person person in personList)
                {
                    Console.WriteLine(person);
                    if (name == person.Name)
                    {
                        Console.WriteLine("Person exists.");
                        nameExist = true;
                        break;

                    }


                }
                if (!nameExist)
                {
                    Console.Write("Enter visitor passport number: ");
                    string passportNo = Console.ReadLine();
                    Console.Write("Enter visitor nationality: ");
                    string nationality = Console.ReadLine();
                    Person p = new Visitor(name, passportNo, nationality);
                    personList.Add(p);
                }




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
                            ListAllSHNFacilities(SHNFacilityList);
                            bool foundName = false;
                            bool selected = false;
                            while (selected == false)
                            {

                                Console.Write("Enter SHN Facility Name: ");
                                string fName = Console.ReadLine();


                                foreach (SHNFacility f in SHNFacilityList)
                                {

                                    if (fName == f.FacilityName)
                                    {

                                        if (f.IsAvailable() == true)
                                        {
                                            foundName = true;
                                            //Console.WriteLine(f.FacilityVacancy);
                                            f.FacilityVacancy -= 1;
                                            //Console.WriteLine(f.FacilityVacancy);
                                            //e.SHNStay = new SHNFacility(f.FacilityName, f.FacilityCapacity, f.DistFromAirCheckpoint, f.DistFromSeaCheckpoint, f.DistFromLandCheckpoint);

                                            e.AssignSHNFacility(new SHNFacility(f.FacilityName, f.FacilityCapacity, f.DistFromAirCheckpoint, f.DistFromSeaCheckpoint, f.DistFromLandCheckpoint));
                                            //Console.WriteLine(e.SHNStay);        
                                            //e.AssignSHNFacility();
                                            //Console.WriteLine("SHN Facility has been assigned.");
                                            Console.WriteLine("You are assigned to: \n" + e.SHNStay + "\tFacility Vacancy: " + f.FacilityVacancy);
                                            selected = true;

                                            break;
                                        }
                                        else
                                        {
                                            foundName = true;
                                            Console.WriteLine("No vacancy, please choose another.");
                                        }

                                    }
                                    //SHNFacility f = new SHNFacility()
                                }
                                if (!foundName) Console.WriteLine("Facility is not found.");
                            }






                        }
                        p.AddTravelEntry(e);
                        Console.WriteLine(p.TravelEntryList[p.TravelEntryList.Count - 1]);
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

                    if (p.Name == name)
                    {
                        if (p.TravelEntryList.Count != 0)
                        {
                            if (p.TravelEntryList[p.TravelEntryList.Count - 1].SHNEndDate < DateTime.Now)
                            {
                                if (p.TravelEntryList[p.TravelEntryList.Count - 1].IsPaid == false)
                                {
                                    Console.Write(p.TravelEntryList[p.TravelEntryList.Count - 1].ToString());
                                    Console.WriteLine("\tSHNEndDate: " + p.TravelEntryList[p.TravelEntryList.Count - 1].SHNEndDate);
                                    double finalCost = p.CalculateSHNCharges() * 1.07;
                                    Console.WriteLine("Total Payable: " + finalCost);
                                    Console.WriteLine("Please make payment. Enter 'Y' after payment is made.");
                                    string payment = Console.ReadLine();
                                    if (payment == "Y")
                                    {
                                        p.TravelEntryList[p.TravelEntryList.Count - 1].IsPaid = true;
                                    }
                                    found = true;
                                    break;
                                }
                                else if (p.TravelEntryList[p.TravelEntryList.Count - 1].IsPaid == true)
                                {
                                    Console.WriteLine("SHN Charges have been paid.");
                                    found = true;
                                    break;
                                }
                            }
                            else
                            {
                                found = true;
                                Console.WriteLine("SHN has not ended. Cannot proceed with payment.");
                            }
                        }
                        else
                        {
                            found = true;
                            Console.WriteLine("No Travel Entry Found.");
                        }



                    }

                }
                if (!found) Console.WriteLine("Person is not found.");
            }
        }
    }
}