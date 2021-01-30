using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

//============================================================
// Student Number : S10205156 , S10205409
// Student Name : Teo Wei Shan, Angelica Sim
// Module Group : T04
//============================================================

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
                    SafeEntryCheckOut(personList, safeEntryList, businessList);
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
                                //Input data to Resident with Token
                                if (line[6] != "" && line[7] != "" && line[8] != "")
                                {
                                    TraceTogetherToken ttt = new TraceTogetherToken(line[6], line[7], Convert.ToDateTime(line[8]));
                                    Resident r = (Resident)p;
                                    r.Token = ttt;

                                }

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
                        Console.WriteLine(p);
                    }
                }

            }

            static void ListPersonDetails(List<Person> personList)
            {
                //Displays a person detail based on name search

                Console.Write("Enter person name: ");
                string name = Console.ReadLine();
                bool found = false;
                //Search through list for person
                foreach (Person p in personList)
                {
                    //If person exist in personList
                    if (p.Name == name)
                    {
                        Console.WriteLine("Person is found. Displaying person information: ");
                        Console.WriteLine(p);
                        //If person does not have travel entry
                        if (p.TravelEntryList.Count == 0)
                        {
                            Console.WriteLine("No travel entry details.");
                        }
                        //If person have travel entry
                        else
                        {
                            Console.WriteLine("Most recent travel entry detail: ");
                            TravelEntry last = p.TravelEntryList[p.TravelEntryList.Count - 1];
                            Console.Write(last);
                            Console.WriteLine("\tSHN End Date: " + last.SHNEndDate + "\tSHN Fee Paid: " + last.IsPaid);
                            //If person stayed at facility
                            if (last.SHNStay != null)
                            {
                                Console.WriteLine(last.SHNStay);
                            }
                            //If person did not stay at facility
                            else
                            {
                                Console.WriteLine("No SHN stay details.");
                            }

                        }
                        //If person is Resident
                        if (p is Resident)
                        {
                            Resident r = (Resident)p;
                            //If person have token
                            if (r.Token != null)
                            {
                                Console.WriteLine("Person has a token. Displaying token information: ");
                                Console.WriteLine(r.Token);
                            }
                            //If person don't have token
                            else
                            {
                                Console.WriteLine("No token found.");
                            }
                        }

                        found = true;
                        break;
                    }
                }
                //If person does not exist in personList
                if (!found) Console.WriteLine("Person is not found.");


            }


            //===SafeEntry/TraceTogether===
            static void TraceTogetherToken(List<Person> personList, List<SafeEntry> safeEntryList)
            {
                bool found = false;
                int serialNo = 12345;
                Console.WriteLine("Enter your name: ");
                string name = Console.ReadLine();
                foreach (Person p in personList)
                {
                    if (p.Name == name && p is Resident)                                                                    //ensure that the person is a resident as only residents have token
                    {
                        found = true;
                        Resident r = (Resident)p;
                        if (r.Token is null)                                                                                //check if resident has a token
                        {
                            serialNo += 1;
                            string newserialNo = "T" + serialNo;                                                       //generate a s/n for token
                            Console.WriteLine("Enter your preferred collection location (CCs only)");
                            string collectionLocation = Console.ReadLine();
                            Console.WriteLine("");
                            DateTime Date = DateTime.Today;
                            DateTime expiryDate = Date.AddMonths(6);
                            TraceTogetherToken t = new TraceTogetherToken(newserialNo, collectionLocation, expiryDate);     //create new token object
                            r.Token = t;                                                                                    //assign token to resident
                            Console.WriteLine("Your TraceTogether token s/n: " + newserialNo + "\nYour collection location: " + collectionLocation + "\nToken expiry date: " + expiryDate);
                            Console.WriteLine("Display" + r.Token.SerialNo + r.Token.CollectionLocation + r.Token.ExpiryDate);
                            break;
                        }
                        else
                        {
                            if (r.Token.ExpiryDate < DateTime.Now.AddMonths(1))
                            {
                                r.Token.IsEligibleForReplacement();
                                Console.WriteLine("Your token is expiring soon on " + r.Token.ExpiryDate + ". Would you like to replace it?");
                                Console.WriteLine("Enter 'Yes' or 'No'");
                                string choice = Console.ReadLine();
                                if (choice == "Yes")
                                {
                                    r.Token.IsEligibleForReplacement();
                                    Console.WriteLine("Enter your preferred collection location (CCs only)");
                                    string collectionLocation = Console.ReadLine();
                                    string v = Convert.ToString(serialNo);
                                    r.Token.ReplaceToken(v, collectionLocation);
                                }
                                else if (choice == "No")
                                {
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid input. Please choose yes/no."); //validation - only accept yes/no answer
                                }
                            }
                            else Console.WriteLine("Cannot change!");
                        }
                    }
                    //else Console.WriteLine( "Error?");
                }
                if (!found) Console.WriteLine("Name not found. Please try again."); //validation - only accept person names in personList
            }

            static void ListBizLocations(List<BusinessLocation> businessList)
            {
                foreach (BusinessLocation b in businessList)
                {
                    Console.WriteLine(b);                                     //display biz locations
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
                        b.MaximumCapacity = newmaxcap;                                          //update biz location capacity
                        Console.Write("Max capacity of " + bizname + "has been updated.");
                        break;
                    }
                    else if (!found)
                    {
                        Console.WriteLine("Business not found. Please try again.");             //validation - only accept businesses in businessList
                        break;
                    }
                }
            }

            static void SafeEntryCheckIn(List<Person> personList, List<BusinessLocation> businessList)
            {
                Console.WriteLine("Enter your name: ");
                string name = Console.ReadLine();
                bool found = false;
                foreach (Person p in personList)
                {
                    if (p.Name == name)                                 //check if name input belongs in personList
                    {
                        found = true;
                        foreach (BusinessLocation b in businessList)
                        {
                            Console.WriteLine(b);                       //display biz locations
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
                                    SafeEntry entry = new SafeEntry(checkin, b);
                                    p.AddSafeEntry(entry);                           //add safeentry object to person
                                    b.VisitorsNow += 1;                          //increase visitors count by 1 upon CheckIn
                                    b.MaximumCapacity -= 1;
                                    Console.WriteLine("You have successfully checked-in.");
                                    break;
                                }
                            }
                        }
                    }
                 }
                if (!found) Console.WriteLine("Name not found. Please try again.");    //validation - only accept names in personList 
            }

            static void SafeEntryCheckOut(List<Person> personList, List<SafeEntry> safeEntryList, List<BusinessLocation> businessList)
            {
                Console.WriteLine("Enter your name: ");
                string name = Console.ReadLine();
                bool found = false;
                foreach (Person p in personList)
                {

                    if (p.Name == name)
                    {
                        found = true;
                        //bool record = false;
                        foreach (SafeEntry entry in safeEntryList)
                        {
                            Console.WriteLine(entry);
                            Console.WriteLine(p.SafeEntryList[p.SafeEntryList.Count - 1].CheckOut);
                        }
                        for (int i = 0; i < p.SafeEntryList.Count; i++)
                        {
                            Console.WriteLine(p.SafeEntryList[i].Location);
                            Console.WriteLine("Select a record to check-out.");
                            string rec = Console.ReadLine();
                            if (p.SafeEntryList.Count >= 0)
                            {
                                p.SafeEntryList[^1].PerformCheckOut();        //remove person from SafeEntryList
                                Console.Write("You have successfully checked-out.");
                                foreach (BusinessLocation b in businessList)
                                {
                                    b.VisitorsNow -= 1;                          //reduce visitors count by 1 upon CheckOut
                                    b.MaximumCapacity += 1;
                                    break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("No records found.");
                            }
                            
                            //if (!record) { }
                        }
                    }

                }
                if (!found)
                    {
                        Console.WriteLine("Invalid input. Please try again.");          //validation - person not found
                    }
            }

            //===TravelEntry===
            static void ListAllSHNFacilities(List<SHNFacility> SHNFacilityList)
            {
                //Display all facilities 
                foreach (SHNFacility f in SHNFacilityList)
                {
                    Console.WriteLine(f + "\tFacility Vacancy: " + f.FacilityVacancy);
                }
            }

            static void CreateVisitor(List<Person> personList)
            {

                Console.Write("Enter visitor name: ");
                string name = Console.ReadLine();
                //Check for same name in personList
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
                //if no same name in personList
                if (!nameExist)
                {
                    //Create visitor
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
                    //Check for person in list
                    if (p.Name == name)
                    {
                        found = true;
                        while (true)
                        {
                            try
                            {
                                Console.Write("Enter last country of embarkation: ");
                                string lastCountry = Console.ReadLine();
                                Console.Write("Enter entry mode (Air/Land/Sea): ");
                                string entryMode = Console.ReadLine();
                                //Validate accepted forms of entry
                                if (entryMode == "Air" || entryMode == "Land" || entryMode == "Sea")
                                {
                                    Console.Write("Enter entry date (dd/MM/yyyy HH:mm:ss) : ");
                                    DateTime entryDate = Convert.ToDateTime(Console.ReadLine());
                                    //Create new object
                                    TravelEntry e = new TravelEntry(lastCountry, entryMode, entryDate);
                                    e.CalculateSHNDuration();
                                    Console.WriteLine(e.SHNEndDate);
                                    //Prompt SHN Selection Menu if SHN Duration is 14 Days
                                    if (e.SHNEndDate == e.EntryDate.AddDays(14))
                                    {
                                        ListAllSHNFacilities(SHNFacilityList);

                                        bool selected = false;
                                        while (selected == false)
                                        {
                                            //Select facility
                                            Console.Write("Enter SHN Facility Name: ");
                                            string fName = Console.ReadLine();
                                            bool foundName = false;

                                            foreach (SHNFacility f in SHNFacilityList)
                                            {
                                                //Check for facility in list
                                                if (fName == f.FacilityName)
                                                {
                                                    //Check if facility is full capacity
                                                    if (f.IsAvailable() == true)
                                                    {
                                                        //Assign facility
                                                        foundName = true;
                                                        f.FacilityVacancy -= 1;
                                                        e.AssignSHNFacility(new SHNFacility(f.FacilityName, f.FacilityCapacity, f.DistFromAirCheckpoint, f.DistFromSeaCheckpoint, f.DistFromLandCheckpoint));
                                                        Console.WriteLine("You are assigned to: \n" + e.SHNStay + "\tFacility Vacancy: " + f.FacilityVacancy);
                                                        selected = true;
                                                    }
                                                    else
                                                    {
                                                        //Facility unable to be assigned
                                                        foundName = true;
                                                        Console.WriteLine("No vacancy, please choose another.");
                                                    }
                                                }
                                            }
                                            //Facility not found
                                            if (!foundName) Console.WriteLine("Facility is not found.");
                                        }
                                    }
                                    //Create travel entry for person
                                    p.AddTravelEntry(e);
                                    Console.WriteLine(p.TravelEntryList[p.TravelEntryList.Count - 1]);
                                    Console.WriteLine("Travel Entry Record Created.");
                                    break;
                                }
                                //Validate accepted input words
                                else
                                {
                                    Console.WriteLine("Invalid input. Enter 'Air' , 'Land' or 'Sea' only. Please re-enter your information again.");

                                }
                            }
                            //Validate date input
                            catch (FormatException)
                            {
                                Console.WriteLine("Invalid input. Enter in the given format 'dd / MM / yyyy HH: mm:ss'. Please re - enter your information again");
                            }
                        }

                    }
                }
                //Person not found
                if (!found) Console.WriteLine("Person is not found.");
            }

            static void CalculateSHNCharges(List<Person> personList)
            {
                Console.Write("Enter person name: ");
                string name = Console.ReadLine();

                bool found = false;
                //Check for person in personList
                foreach (Person p in personList)
                {
                    //If person exist
                    if (p.Name == name)
                    {
                        //If person has a TravelEntry
                        if (p.TravelEntryList.Count != 0)
                        {
                            //If person has ended their SHN
                            if (p.TravelEntryList[p.TravelEntryList.Count - 1].SHNEndDate < DateTime.Now)
                            {
                                //If person has not paid SHN Fees
                                if (p.TravelEntryList[p.TravelEntryList.Count - 1].IsPaid == false)
                                {
                                    //Display payable
                                    Console.Write(p.TravelEntryList[p.TravelEntryList.Count - 1].ToString());
                                    Console.WriteLine("\tSHNEndDate: " + p.TravelEntryList[p.TravelEntryList.Count - 1].SHNEndDate);
                                    double finalCost = p.CalculateSHNCharges() * 1.07;
                                    Console.WriteLine("Total Payable: " + finalCost);
                                    Console.WriteLine("Please make payment. Enter 'Y' after payment is made. Enter others to not pay for you SHN fees.");
                                    string payment = Console.ReadLine();
                                    if (payment == "Y")
                                    {
                                        p.TravelEntryList[p.TravelEntryList.Count - 1].IsPaid = true;
                                        Console.WriteLine("SHN Charges have been paid.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("SHN Charges have NOT been paid. Please remember to pay!");
                                    }
                                    found = true;
                                    break;
                                }
                                //If person has paid SHN Fees
                                else if (p.TravelEntryList[p.TravelEntryList.Count - 1].IsPaid == true)
                                {
                                    Console.WriteLine("SHN Charges have been paid.");
                                    found = true;
                                    break;
                                }
                            }
                            //If person has not ended their SHN
                            else
                            {
                                found = true;
                                Console.WriteLine("SHN has not ended. Cannot proceed with payment.");
                            }
                        }
                        //If person does not have a TravelEntry
                        else
                        {
                            found = true;
                            Console.WriteLine("No Travel Entry Found.");
                        }

                    }

                }
                //If does not person exist
                if (!found) Console.WriteLine("Person is not found.");
            }
        }
    }
}