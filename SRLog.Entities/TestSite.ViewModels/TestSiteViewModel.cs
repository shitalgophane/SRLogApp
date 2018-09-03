using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRLog.Entities.TestSite.ViewModels
{
    public class TestSiteViewModel
    {
        public int Id { get; set; }
        public string FlagName { get; set; }
        public string Value { get; set; }
        public bool DisplayMessage { get; set; }
    }
}
