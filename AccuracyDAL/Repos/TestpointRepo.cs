using AccuracyDAL.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using static System.Console;
using System.Diagnostics;
using System.Reflection;
using System;

namespace AccuracyDAL.Repos
{
    public class TestpointRepo : BaseRepo<Testpoint>, IRepo<Testpoint>
    {
        public TestpointRepo()
        {
            Table = Context.Testpoints;
        }

        /// <summary>
        ///To delete an inventory record by primary key:
        ///-Create a new instance of the Inventory
        ///-assign the CarID to the id parameter
        ///-set the state to EntityState.Deleted
        ///-then call SaveChanges
        /// </summary>
        /// <param name="id">TestrunID the foreign key</param>
        /// <returns></returns>
        //public int Delete(int testrunid)
        //{
        //    Context.Testpoints.RemoveRange(Context.Testpoints.Where(x => x.testrunID == testrunid));
        //    return SaveChanges();
        //}
        //Delete all the record with that id, foreign key id
        //The delete here is implemented slihgtly different, I want to remove the 
        //All the data with a specific testid once a testid is removed
        //The results should stay in tackt, can not just delete one point
        public int Delete(int id/*, byte[] timeStamp*/)
        {
            //Context.Testpoints.RemoveRange(Context.Testpoints.Where(x => x.testrunID == id));
            List<Testpoint> toremove = (Context.Testpoints.Where(x => x.testrunID == id)).ToList();
            foreach(var tp in toremove)
            {
                //Context.Entry(tp).State = System.Data.Entity.EntityState.Deleted;
            }
            return 1;

            //Context.Testpoints.RemoveRange(Context.Testpoints.Where(x => x.Powerfactorpcu == id));
            //return SaveChanges();
        }
        /// <summary>
        /// Get a list of results from string column name
        /// The application needs column data per test run to do calculations and plots.
        /// </summary>
        /// <param name="wantedcolumn"></param>
        /// <param name="testrunid"></param>
        /// <returns>List<string></returns>
        public List<float> GetColumnData(string wantedcolumn, int testrunid)
        {
            //Error checking: the Table has to be checked if there are any value and
            //if the specific value is there, else exeptions
            //No data in the table return

            if (Table.Count() == 0)
                return null;
            //find the distinct id in the table for error checking, below
            var idgroup = (from pi in Table select pi.testrunID).Distinct();
            //Get the properties for the EF.Model
            PropertyInfo[] testpoint_fields = Helper.GetPropertlyArray(typeof(Testpoint));
            //TODO: Fix. if there is not a property in the Testpoint class with this name return null
            if ((from pi in testpoint_fields where pi.Name == wantedcolumn select pi).Count() == 0)
                return null;
            //LINQ expression here
            //Find the first Testpoint point property equal to wantedcolumn parameter
            //Find the first and only value
            PropertyInfo property = (from pi in testpoint_fields where pi.Name == wantedcolumn select pi).First();
            //if nothing in the idgroup then the id is not there, return null
            if (idgroup.Contains(testrunid))
            {
                //Some temp variable
                List<Testpoint> tempTestPointValues = new List<Testpoint>();
                List<float> temp = new List<float>();
                // var ids_FK = Table.Find(y => y.testrunID == testrunid).testrunID)
                //If the foreign key is in the data Table
                var ids_FK = Table.First(x => x.testrunID == testrunid).testrunID;
                //If the list of id's contains your id then get the column data from Testpoints
                if (ids_FK == testrunid)
                {
                    //Get a list of TestPoint with specific foreign key
                    tempTestPointValues = Table.Where(x => x.testrunID == testrunid).ToList();
                    foreach(Testpoint tp in tempTestPointValues)
                    {
                        //Get the value based on the property string name, 
                        var v = typeof(Testpoint).GetProperty(wantedcolumn).GetValue(tp);
                        //Convert back! System.Single is float, if it is convert
                        if (v is System.Single)
                            temp.Add(Convert.ToSingle(v));
                        else
                            return null;
                    }
                    //return the the end result 
                    return temp;
                    //return Table.Where(x => x.testrunID == testrunid).Select(y => y.Iacimagpcu).ToList();
                }
            }
            return null;
        }

        //Delete value asynchronous
        //public Task<int> DeleteAsync(int id)
        //{
        //    Context.Testpoints.RemoveRange(Context.Testpoints.Where(x => x.testrunID == id))
        //    return SaveChangesAsync();
        //}

        public Task<int> DeleteAsync(int id/*, byte[] timeStamp*/)
        {
            //Context.Testpoints.RemoveRange(Context.Testpoints.Where(x => x.testrunID == id));
            Context.Testpoints.RemoveRange(Context.Testpoints.Where(x => x.Powerfactorpcu == id));
            return SaveChangesAsync();
        }
    }
}
