using AccuracyDAL.Models;
using AccuracyDAL.Repos;
using static System.Console;//C#6, use this to only write "WriteLine("bla")"
using CsvAnalyzer;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace TestDriveAccuracyDAL
{
    /*
     Double loading the test unit variables
         */

    //This test program does not operate against the context directly
    //but utilize the Repo's created in AutoLotDAL/Repos to operate on the database
    class Program
    {
        static void Main(string[] args)
        {
          //  Database.SetInitializer(new MyDataInitializer());
            WriteLine("***Test drive accuracyDAL***");
            
            //Populate datacolumns with wanted data column desriptions, data independant
            deserialize();

            while (true)
            {
                WriteLine("Commands:/n 1--exit ID to 2--display 3--Path to data 4--integer + d to delete  ");
                string line = ReadLine();
                //exit
                if (line == "exit"){ WriteLine("EXIT"); break; }

                /***************if the input is a integer do this get the data*****************/
                int i = -1;
                //try parse the line to int
                int.TryParse(line, out i);
                if((i != -1) && (i != 0))
                {
                    //try and get some column data
                    getcolumndata(i);
                }
                /***************if the input is a file name do this, load it and print*****************/

                if (HelperStatic.FileChecks(line))
                {
                    WriteLine("The file exist: {0}", line);
                    //populate from csv
                    populateDataTestUnit(line);//line is the console input
                    //Load into testpoint repo
                    loadrepos(line);//line is the console input
                    WriteLine("Done with testpoint repo!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    //*********************Do some data extractions************************
                    PrintAllTestRuns();
                    //wait for me console do not disappear
                }
                /*****If integer contains d then delete it*************/
                if((line.Length == 2) && (line.Contains("d")))
                {
                    //remove the d from the command line text, convert the integer
                    string temp = line.Remove(1, 1);
                    WriteLine("!" + temp + "!");
                    int j = -1;
                    //try parse the line to int
                    int.TryParse(temp, out j);
                    if (j != -1)
                    {
                        WriteLine("!" + j);
                        //try and get some column data
                        //
                        //using (var repo_testrun = new TestrunRepo()) { repo_testrun.Delete(j); }
                        //Delete testpoints
                       // using (var repo_testpoints = new TestpointRepo()) { repo_testpoints.Delete(j); }

                        WriteLine("Now using the testrun repo");
                        using (var repo_testrun = new TestrunRepo())
                        {
                            WriteLine("Inside using");
                            Testrun findTR = repo_testrun.GetOne(j);
                            if (findTR == null)
                                WriteLine("TR is null,, BOOOOOOOM!");
                            else
                            {
                                WriteLine("TR is NOT null");
                                repo_testrun.Context.Testruns.Remove(findTR);
                                repo_testrun.Context.SaveChanges();
                                //repo_testrun.Delete(j);
                                //WriteLine("I have REMOVED it");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Column data: Getting the float column data back from the repo, column by column name
        /// </summary>
        /// <param name="testid">The foreign key for tests run</param>
        private static void getcolumndata(int testid)
        {
            List<float> tempFloatList = new List<float>();
            using (var repo = new TestpointRepo())
            {
                tempFloatList = repo.GetColumnData("Vacpcu", testid);

                if(tempFloatList != null)
                {
                    foreach (var v in tempFloatList)Write(v + " ;");
                }
            }
        }
        //Load up the repo
        //the testrun repo populate function: needs the full .csv link to get
        //the extra data file (.txt)
        //The testpoint repo populate function needs the TESTRUN FOREIGN KEY
        private static void loadrepos(string fullcsvfilepath)
        {
            //Load the testrun repo then send the reference to Testpoint repo load to load testpoints
            //The Load_TestRun_repo returns a reference to the Testrun FK.
            Load_Testpoint_repo(Load_TestRun_repo(fullcsvfilepath));
        }

        /// <summary>
        /// Load data from the realpowerdictionary into Testpointrepo
        /// I need a file name for the function, to extract some more data need
        /// </summary>
        private static Testrun Load_TestRun_repo(string folderPathForTextFile)
        {
            //TODO: Get a better way to find the number of rows
            if ((realpowerdict["SerialNumber"].Columnvalues).Count == 0)
            {
                WriteLine("NULL from testrun load");
                return null;
            }
            //Get the properties for the EF.Model
            PropertyInfo[] test_fields = AccuracyDAL.Helper.GetPropertlyArray(typeof(Testrun));
            //PropertyInfo[] test_fields = typeof(Testrun).GetProperties(BindingFlags.Public |
            //    BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.NonPublic);
            //Populate the data in the Testrun model, iterate through the property info
            //create a temporay test run to populate
            Testrun tempTestRun = new Testrun();
            foreach(var property in test_fields )
            {
                switch (property.Name)
                {
                    case "HardwareType":
                        //SetValue takes the object reference and the value for the property
                        property.SetValue(tempTestRun, (realpowerdict[property.Name].Columnvalues)[0]);//TODO improve:Take the first element, dodgy
                        break;
                    case "SerialNumber":
                        property.SetValue(tempTestRun, (realpowerdict[property.Name].Columnvalues)[0]);//TODO improve:Take the first element, dodgy
                        break;
                    case "FirmwareRef":
                        property.SetValue(tempTestRun, 
                            HelperStatic.Firmware_version(folderPathForTextFile));
                        break;
                    case "ParameterRef":
                        property.SetValue(tempTestRun, 
                            HelperStatic.Parameter_version(folderPathForTextFile));
                        break;
                    case "TestName":
                        property.SetValue(tempTestRun,
                            HelperStatic.TestName(folderPathForTextFile));
                        break;
                    case "testrunID":
                        //Auto populated
                        break;
                    case "Timestamp":
                        //Autopopulated
                        break;
                    case "Testpoints":
                        //not a true field member
                        break;
                    default:
                        WriteLine("NULL from testrun load: switch {0}", property.Name);
                        return null;
                }
            }
            //********Add tesrun to repo, not needed seems like....*********
            //because it is a foreign key when I add it to foreign key get added
            //when foreign key linked
            //Add the tempTestRun that was created previously to the repo
            //using (var repo_testrun = new TestrunRepo()){ repo_testrun.Add(tempTestRun); }
            return tempTestRun;
        }

        /// <summary>
        /// Load data from the realpowerdictionary into Testpointrepo
        /// </summary>
        private static void Load_Testpoint_repo(Testrun testrun_instance)
        {
            //if Testrun is null the do not populate  datapoints
            if (testrun_instance == null) return;
            //Chart checking the data/csv quality vs repo
            using (var repo_tp = new TestpointRepo())
            {
                //WriteLine(repo_tp.ToString());
                //TODO: Get a better way to find the number of rows
                int numberOfRows = 0;
                //Get the properties for the EF.Model
                PropertyInfo[] testpoint_fields = AccuracyDAL.Helper.GetPropertlyArray(typeof(Testpoint));
               // PropertyInfo[] testpoint_fields = typeof(Testpoint).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.NonPublic);
                //Check if all the point in the database table can be populated and corrolate
                //Look through the tespoint properties and see if they exist in datadictionary
                WriteLine("Check data against model: ");
                for(int i = 0; i < testpoint_fields.Length; i++)
                {
                    //Check if the model property name is in the dictionary from CSV
                    if(realpowerdict.ContainsKey(testpoint_fields[i].Name))
                    {
                        //get the number of rows to be used later
                        numberOfRows = realpowerdict[testpoint_fields[i].Name].Columnvalues.Count;
                    }
                    else
                    {
                        //If the name contains ID that is ok, do not return, excluded from this
                        if (!(testpoint_fields[i].Name.Contains("ID") || testpoint_fields[i].Name.Contains("Testrun")))
                        {
                            //Return without populating the Repo, columns does not macth property
                            WriteLine("Oops returning without populating. {0}", testpoint_fields[i].Name);
                            return;
                        }
                    }
                    //If all the properties of the model match the column names, all there
                    //then continue
                }
                //REPO: Populate here
                //This will give the lenth of the columns
                WriteLine("Goingt to try and load data");
                for (int k = 0; k < numberOfRows; k++)
                {
                    //New test point created for each row...logical
                    Testpoint tempTP = new Testpoint();
                    //This will iterate through every column
                    Dictionary<string, float> tempDict = new Dictionary<string, float>();

                    foreach (var property in testpoint_fields)
                    {
                        //TODO: This is a bit annoying. Do not want to enter a ID
                        if (!(property.Name.Contains("ID") || property.Name.Contains("Testrun")))
                        {
                            if ((realpowerdict[property.Name].Columnvalues)[k] == "")
                                (realpowerdict[property.Name].Columnvalues)[k] = "0";
                            //I like this: set the specific property of object tempTP,
                            //Setvalue takes the object reference and the value for the property
                            property.SetValue(tempTP, float.Parse((realpowerdict[property.Name].Columnvalues)[k]));
                        }
                    }
                    //Foreign key operation here
                    //This is setting the testrunID foreign key in testpoint, Testpoint.cs model
                    tempTP.Testrun = testrun_instance;
                    //tempTP.Testrun.testrunID = testrun_instance.testrunID;
                    repo_tp.Add(tempTP);
                }
            }
        }

        /// <summary>
        /// Not Database, just data related
        /// Clear the old data for a replot, clear data not really need in the test but...ok
        /// </summary>
        private static void clearolddata()
        {
            //datacolumns comes from xml so do not clear it
            //Clear to reuse if it is not the first iteration
            if (datacolumns != null)
            {
                foreach (Column c in datacolumns.namealiaslist)
                {
                    //Clear the values in the columns
                    c.clearvalues();
                }
                realpowerdict = new Dictionary<string, Column>();
                columns = new List<IBaselist>();
            }
        }

        /// <summary>
        /// Not Database, just data related
        /// Gets the wanted columns from xml
        /// </summary>
        public static void deserialize()
        {
            //This only gets loaded once and not reloaded on new csv file load
            //It is just data desciptions and column names
            XmlManager<DataColumns> columnloader = new XmlManager<DataColumns>();
            //Initialize the datacolumns class with the wanted columns
            datacolumns = columnloader.Load("Content/XMLFile1.xml");
        }

        /// <summary>
        /// Not Database, just data related
        /// populating data from CSV using CsvAnalyzer.dll
        /// load the data to memory
        /// </summary>
        /// <param name="filename"></param>
        private static void populateDataTestUnit(string fullCSVfilepath)
        {
            //Get ready for the next load
            //Clear: realpowerdict = new Dictionary<string, Column>();
            //Clear: columns = new List<IBaselist>();
            //Clear: datacolumns, from CSVAnalyzer class DataColumns
            clearolddata();
            using (ReadWriteCsv.CsvFileReader reader = new ReadWriteCsv.CsvFileReader(fullCSVfilepath))
            {
                ReadWriteCsv.CsvRow row = new ReadWriteCsv.CsvRow();
                var rowcount = 0;
                while (reader.ReadRow(row))
                {
                    //the first row is the header
                    if (rowcount == 0)
                    {
                        foreach (string s in row)
                        {
                            foreach (Column c in datacolumns.namealiaslist)
                                //if the name is in the wanted columns save position
                                if (c.columnname == s) c.columnnumber = row.IndexOf(s);
                        }
                    }
                    else
                    {
                        foreach (Column c in datacolumns.namealiaslist)
                        {
                            c.colvalues.Add(row[c.columnnumber]);
                        }
                    }
                    //moveto the next row
                    rowcount++;
                }
                // Debug.WriteLine("Baseline rows imported: " + Convert.ToString(rowcount));
            }
            //Create a dictionary with the alias name and instance...not needed really. just lambda expr?
            foreach (Column c in datacolumns.namealiaslist)
            {
                realpowerdict.Add(c.alias, c);
            }
            //v key is a string, v.Value is a Column
            foreach (var v in realpowerdict)
            {
                //Add to the ValuelistI
                columns.Add(new ValuelistI(v.Value.Columnvalues, v.Key, v.Value));
            }
        }

        //to print call getall from inventory repo and then iterate the returned list
        //There is not much difference betwween doing this and coding directly
        //against the Context, but Repository pattern provides a consistent way to access 
        //and operate on all data across all classes
        private static void PrintAllTestRuns()
        {
            using (var repo = new TestrunRepo())
            {
                foreach (var c in repo.GetAll())
                {
                    WriteLine(c);
                }
            }
        }

        //Data columns is a list of wanted column names/aliases
        private static DataColumns datacolumns;
        //This is a dictionary that contains, alias name and column instance
        private static Dictionary<string, Column> realpowerdict;
        //Alist of column data class
        private static List<IBaselist> columns;
    }
}
