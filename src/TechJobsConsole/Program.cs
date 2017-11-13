using System;
using System.Collections.Generic;

namespace TechJobsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create two Dictionary vars to hold info for menu and data

            // Top-level menu options
            Dictionary<string, string> actionChoices = new Dictionary<string, string>();
            actionChoices.Add("search", "Search");
            actionChoices.Add("list", "List");

            // Column options
            Dictionary<string, string> columnChoices = new Dictionary<string, string>();
            columnChoices.Add("core competency", "Skill");
            columnChoices.Add("employer", "Employer");
            columnChoices.Add("location", "Location");
            columnChoices.Add("position type", "Position Type");
            columnChoices.Add("all", "All");

            Console.WriteLine("Welcome to LaunchCode's TechJobs App!");

            // Allow user to search/list until they manually quit with ctrl+c
            while (true)
            {

                string actionChoice = GetUserSelection("View Jobs", actionChoices); //sends dicto and header string to G.U.S. function
                                                                                    //returns string of key
                if (actionChoice.Equals("list"))                                    //if key string is list
                {
                    string columnChoice = GetUserSelection("List", columnChoices);  //run the G.U.S. again but for column choices

                    if (columnChoice.Equals("all"))                                 //if returned string is all
                    {
                        PrintJobs(JobData.FindAll());                               //run jobData.FindAll() **note not FindAll(string) function
                    }
                    else
                    {
                        List<string> results = JobData.FindAll(columnChoice);       //runs a function that will return a list<str> of all different entries
                                                                                    //in that column

                        Console.WriteLine("\n*** All " + columnChoices[columnChoice] + " Values ***");  //this prints a sort of header, users the columnChoices dicto spit out a nicely formatted header
                        foreach (string item in results)                            //iterates over all entries in the list
                        {                                                           //simply writes each entry in list
                            Console.WriteLine(item);
                        }
                    }
                }
                else // choice is "search"
                {
                    // How does the user want to search (e.g. by skill or employer)
                    string columnChoice = GetUserSelection("Search", columnChoices);

                    // What is their search term?
                    Console.WriteLine("\nSearch term: ");
                    string searchTerm = Console.ReadLine();

                    List<Dictionary<string, string>> searchResults; //creates a new list of <str><Str> dictos called searchResults

                    // Fetch results
                    if (columnChoice.Equals("all"))
                    {
                        searchResults = JobData.FindByValue(searchTerm);
                        PrintJobs(searchResults);
                    }
                    else
                    {
                        searchResults = JobData.FindByColumnAndValue(columnChoice, searchTerm); //takes in columnChoice Key from GSU function, as well as the search term
                        PrintJobs(searchResults); //takes data back from FBCAV above, list of str str dictos, and runs that list back through print jobs
                    }
                }
            }
        }

        /*
         * Returns the key of the selected item from the choices Dictionary
         */
        private static string GetUserSelection(string choiceHeader, Dictionary<string, string> choices)
        {//literally all this function does is spit out a good key that's usable in other dictionaries, all based on numerical user input
            int choiceIdx;
            bool isValidChoice = false;
            string[] choiceKeys = new string[choices.Count]; //initializes array of X number slots, X equal to number of choices

            int i = 0;
            foreach (KeyValuePair<string, string> choice in choices)//for each entry in inhereted dicto "choices"
            {                                                       //the choiceKeys slot at index i gets filled
                choiceKeys[i] = choice.Key;                         //with the same spot key in the dicto
                i++;                                                //this just gives us a separate array
            }                                                       //of just keys, and we can use the number
                                                                    //input by the user to correspond to a dicto key
            do
            {
                Console.WriteLine("\n" + choiceHeader + " by:");    //NOTE choice header inhereted

                for (int j = 0; j < choiceKeys.Length; j++)                 //setting up iteration
                {                                                                   
                    Console.WriteLine(j + " - " + choices[choiceKeys[j]]);  //this just outputs a list of choices
                }                                                           //as well as their indexed numbers for better choice making
                    
                string input = Console.ReadLine();                          //takes user input and assigns to input string variable
                choiceIdx = int.Parse(input);                               //parses input into the choiceIdx variable set earlier
                                                                            //should also throw an error if it cannot parse as an int
                if (choiceIdx < 0 || choiceIdx >= choiceKeys.Length)        //allows only numbers within range to be selected
                {                                                           //throws errors otherwise
                    Console.WriteLine("Invalid choices. Try again.");
                }
                else
                {
                    isValidChoice = true;                                   //sets validchoice to true, breaks loop
                }

            } while (!isValidChoice);

            return choiceKeys[choiceIdx]; //having selected an index in choiceIdx, and using that index to gain a choice from choiceKeys
        }                                 //we can now return the appropriate key to the function that called this function

        private static void PrintJobs(List<Dictionary<string, string>> someJobs)
        {//here is step one, implement this function
            int numboDicto = someJobs.Count;
            if (numboDicto == 0)
            {
                Console.WriteLine("The requested data does not exist or turned up no results");
            
            }
            else
            {
                Console.WriteLine("Jobs:");
                foreach (Dictionary<string, string> dicto in someJobs)
                {
                    Console.WriteLine("*****");
                    foreach (KeyValuePair<string, string> job in dicto)
                    {
                        Console.Write(job.Key + ": ");
                        Console.Write(job.Value);
                        Console.Write('\n');
                    }
                    Console.WriteLine("*****\n");
                }
            }
        }
    }
}
