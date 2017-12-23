using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRLog
{
    public class GlobalSettings
    {
        public enum FixedColumnsInSRLog
        {
            SRNumber,
            Customer,
            Owner,
            ProjectDescription,
            Division,
            InactiveJob,
            ProjectManager,
            Estimator
        };

    }
}