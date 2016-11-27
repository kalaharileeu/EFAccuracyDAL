using System;
using System.Collections.Generic;
using System.Data.Entity;
using AccuracyDAL.Models;
using System.Diagnostics;

namespace AccuracyDAL.EF
{
    //Initiaze the database with data, inherit from base classes to drop and recreate 
    //always or only if the model changes
    //This code is used in a test run, it effectively just initializes the database with clean data
    //So everytime the database hase been "played with" you can drop the tables and start fresh
    //this datainitialer get called from the test drive project
    public class MyDataInitializer : DropCreateDatabaseAlways<AccuracyEntities>
    {
        //over load the seed method with own data seeding
        protected override void Seed(AccuracyEntities context)
        {
            //var testruns = new List<Testrun>
            //{

            //    new Testrun { testrunID = 34, HardwareType = "S290-60-LL", SerialNumber = "123123123", FirmwareRef = "23.23.23",
            //        ParameterRef = "23.43.43" }
            //};

            //testruns.ForEach(x => context.Testruns.Add(x));

            //var testpoints = new List<Testpoint>
            //{
            //    new Testpoint { Powerfactorpcu = 3.5f  }
            //};

            //testpoints.ForEach(x => context.Testpoints.Add(x));

            //Second way of programming agaisnt the context
            /*            context.CreditRisks.Add(
                new CreditRisk
                {
                    CustId = customers[4].CustId,
                    FirstName = customers[4].FirstName,
                    LastName = customers[4].LastName,
                });*/



            //context.SaveChanges();
        }
    }
}
