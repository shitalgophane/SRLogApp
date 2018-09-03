using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRLog.Entities.BidLog.ViewModels
{
   public class BidResultViewModel
    {

        public Nullable<int> UID { get; set; }
        public Nullable<int> BidStatus { get; set; }
        public bool BidStatusLowBldder { get; set; }
        public bool BidStatusHighBladder { get; set; }
        public bool BidStatusDidNotGet { get; set; }
        public string TelstarQuote { get; set; }
        public string LowGC { get; set; }
        public string LowElecSub { get; set; }
        public Nullable<int> BidType { get; set; }
        public string Description { get; set; }
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public string Column3 { get; set; }
        public string Column4 { get; set; }
        public string Column5 { get; set; }
        public string Column6 { get; set; }
        public string Column7 { get; set; }
        public string Column8 { get; set; }
        public string Column9 { get; set; }
        public string Column10 { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int BIDResults_Id { get; set; }
        public int UserId { get; set; }
    }
}
