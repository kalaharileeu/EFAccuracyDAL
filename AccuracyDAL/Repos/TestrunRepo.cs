using AccuracyDAL.Models;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Reflection;
using System.Linq;
using System.Diagnostics;

namespace AccuracyDAL.Repos
{
    public class TestrunRepo : BaseRepo<Testrun>, IRepo<Testrun>
    {
        public TestrunRepo()
        {
            Table = Context.Testruns;
        }

        public int Delete(int id/*, byte[] timeStamp*/)
        {
            var toRemove = Context.Entry(new Testrun { testrunID = id /*, Timestamp = timeStamp*/ });
            Debug.WriteLine(toRemove.State);
            if (toRemove.State == EntityState.Detached)
            {
                Context.Testruns.Add(toRemove.Entity);
               // Context.Testruns.Attach(toRemove.Entity);
            }
            Debug.WriteLine(toRemove.State);
            //Context.Testruns.Remove(toRemove.Entity);
            toRemove.State = EntityState.Added;

            return SaveChanges();
           // Context.Entry(new Testrun() { testrunID = id /*, Timestamp = timeStamp*/ }).State = EntityState.Deleted;
            //return SaveChanges();
        }

        public Task<int> DeleteAsync(int id/*, byte[] timeStamp*/)
        {
            Context.Entry(new Testrun() { testrunID = id/* , Timestamp = timeStamp*/ }).State = EntityState.Deleted;
            return SaveChangesAsync();
        }

        //public PropertyInfo[] GetMenberInfo()
        //{
        //    return typeof(Testrun).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.NonPublic);
        //}
        ///// <summary>
        ///// Get a list of testruns, in string format
        ///// </summary>
        //public List<string> GetListofTests()
        //{
        //    //Get ny id smaller than 100, and only SELECT the column testrunID
        //    //var ids_FK = Table.Where(x => x.testrunID < 100).Select(y => y.testrunID).Distinct();

        //    var v = Table.

        //    return ids_FK;
        //}
    }
}
