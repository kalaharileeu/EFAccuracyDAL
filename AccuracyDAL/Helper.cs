using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

/// <summary>
/// This helpwer class is directly related to AccuracyDAL
/// </summary>

namespace AccuracyDAL
{
    public class Helper
    {
        /*******************************************
         * 
         * Some helper functions regarding the DAL
         * 
         * ****************************************/
         /// <summary>
         /// Get a array of properties from the EF models
         /// </summary>
         /// <param name="T">type(Class name)</param>
         /// <returns></returns>
        static public PropertyInfo[] GetPropertlyArray(Type T)
        {
            if (T != null)
            {
                return T.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.NonPublic);
            }
            else
                return null;
        }
    }
}
