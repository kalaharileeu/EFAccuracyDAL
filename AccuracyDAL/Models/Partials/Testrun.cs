using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuracyDAL.Models
{
    public partial class Testrun
    {
        //Over the ToString so that what ever is printed makes sense!
        public override string ToString()
        {
            //return $"{this.testrunID}, {this.HardwareType}, {this.SerialNumber}, {this.TestName}, {this.FirmwareRef}, {this.ParameterRef}, {this.Timestamp} .";
            return $"{this.testrunID}, {this.HardwareType}, {this.SerialNumber}, {this.TestName}, {this.FirmwareRef}, {this.ParameterRef} .";
        }
    }
}
