using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TechJobsConsole
{
    class JobData
    {
        static List<Dictionary<string, string>> AllJobs = new List<Dictionary<string, string>>(); //builds AllJobs, a List of str, stri Dictos
        static bool IsDataLoaded = false;

        public static List<Dictionary<string, string>> FindAll() //this is used to recall literally all data
        {
            LoadData();         //runs LoadData, just checks to make sure .csv is loaded
            return AllJobs;     //since we're not picking and pulling, this should just return all loaded data
        }

        /*
         * Returns a list of all values contained in a given column,
         * without duplicates. 
         */
        public static List<string> FindAll(string column)
        {
            LoadData();

            List<string> values = new List<string>(); //new list of just strings called Values

            foreach (Dictionary<string, string> job in AllJobs) 
            {
                string aValue = job[column]; //uses inhereted 'column' passed in to return the value associated with that key
                                             
                if (!values.Contains(aValue))//if our new list of just plain, non-repeated values does not contain this iterations value
                {                            //then add that value to the list of values
                    values.Add(aValue);
                }                            //otherwise it is already in the list and we dont want it to be repeated
            }
            return values;                   //send the list<str> back to the function
        }

        public static List<Dictionary<string, string>> FindByColumnAndValue(string column, string value)
        {
            // load data, if not already loaded
            LoadData();

            List<Dictionary<string, string>> jobs = new List<Dictionary<string, string>>(); //creates a new list of str, str dictos
            
            foreach (Dictionary<string, string> row in AllJobs) //looks at all jobs/rows in AllJobs
            {
                string aValue = row[column]; 

                if (aValue.Contains(value))
                {
                    jobs.Add(row);
                }
            }

            return jobs;
        }

        public static List<Dictionary<string, string>> FindByValue(string value)
        {
            LoadData(); //checks to make sure jobs are loaded

            List<Dictionary<string, string>> jobs = new List<Dictionary<string, string>>(); //creates new variable, list of dictionaries with string/string pairs

            foreach (Dictionary<string, string> row in AllJobs)
            {
                bool hasGoodData = false;
                foreach (KeyValuePair<string, string> field in row)
                {
                    string aValue = field.Value;
                    if (aValue.Contains(value))
                    {
                        hasGoodData = true;
                        
                    }

                }
                jobs.Add(row);
            }



            return jobs;
        }
        /*
         * Load and parse data from job_data.csv
         */
        private static void LoadData()
        {

            if (IsDataLoaded) //if data is already laoded, no need in loading it again
            {
                return; //bypasses rest of function
            }

            List<string[]> rows = new List<string[]>(); //buids a list of string arrays called Rows
                                                        //each array will be a row, each string will be a cell
            using (StreamReader reader = File.OpenText("job_data.csv")) //this is where we actually load the file
            {
                while (reader.Peek() >= 0) //honestly unsure whats going on with this conditional
                {
                    string line = reader.ReadLine(); //builds string called line from a line in csv
                    string[] rowArrray = CSVRowToStringArray(line); //converts CSVRow to an array now called rowArray
                    if (rowArrray.Length > 0) //checking to make sure a row is actually good
                    {
                        rows.Add(rowArrray); //appends the rows list with the next row
                    }
                }
            }

            string[] headers = rows[0]; //creates an array of strings called Headers, just equal to first array in rows list, 
            rows.Remove(headers); //uses headers to eliminate itself from the list of arrays

            // Parse each row array into a more friendly Dictionary
            foreach (string[] row in rows) //begins converting list of arrays to dicto
            {
                Dictionary<string, string> rowDict = new Dictionary<string, string>(); //builds dicto rowDict

                for (int i = 0; i < headers.Length; i++) //for each column, as defined by the length of the headers row
                {
                    rowDict.Add(headers[i], row[i]); //adds a key value pair of the headers and their respective rows. 
                }                                    //the significance of this, i believe, is that we can say, show me all of the
                AllJobs.Add(rowDict);                //X header, like location, and it will return a value of all cells in that column
            }

            IsDataLoaded = true;
        }

        /*
         * Parse a single line of a CSV file into a string array
         */
        private static string[] CSVRowToStringArray(string row, char fieldSeparator = ',', char stringSeparator = '\"')
        {
            bool isBetweenQuotes = false;
            StringBuilder valueBuilder = new StringBuilder();
            List<string> rowValues = new List<string>();

            // Loop through the row string one char at a time
            foreach (char c in row.ToCharArray())
            {
                if ((c == fieldSeparator && !isBetweenQuotes))
                {
                    rowValues.Add(valueBuilder.ToString());
                    valueBuilder.Clear();
                }
                else
                {
                    if (c == stringSeparator)
                    {
                        isBetweenQuotes = !isBetweenQuotes;
                    }
                    else
                    {
                        valueBuilder.Append(c);
                    }
                }
            }

            // Add the final value
            rowValues.Add(valueBuilder.ToString());
            valueBuilder.Clear();

            return rowValues.ToArray();
        }
    }
}
