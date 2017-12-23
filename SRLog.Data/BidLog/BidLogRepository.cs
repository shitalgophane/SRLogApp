using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SRLog.Data;
using SRLog.Entities.BidLog.ViewModels;

namespace SRLog.Data.BidLog
{
    public class BidLogRepository
    {
        SR_Log_DatabaseSQLEntities1 db = new SR_Log_DatabaseSQLEntities1();

        public List<tblBID_Log> GetBidLogs()
        {

            var bidlogs = (from u in db.tblBID_Logs
                           select u).ToList();

            return bidlogs;

        }

        public List<tblBID_Log> GetBidLogsList(int startIndex, int count, string sorting)
        {
            IEnumerable<tblBID_Log> query = db.tblBID_Logs;


            //Sorting
            //This ugly code is used just for demonstration.
            //Normally, Incoming sorting text can be directly appended to an SQL query.
            if (string.IsNullOrEmpty(sorting) || sorting.Equals("BidDate ASC"))
            {
                query = query.OrderBy(p => p.BidDate);
            }
            else if (sorting.Equals("BidDate DESC"))
            {
                query = query.OrderByDescending(p => p.BidDate);
            }
            else if (sorting.Equals("BiddingAs ASC"))
            {
                query = query.OrderBy(p => p.BiddingAs);
            }
            else if (sorting.Equals("BiddingAs DESC"))
            {
                query = query.OrderByDescending(p => p.BiddingAs);
            }
            else if (sorting.Equals("CityState ASC"))
            {
                query = query.OrderBy(p => p.CityState);
            }
            else if (sorting.Equals("CityState DESC"))
            {
                query = query.OrderByDescending(p => p.CityState);
            }
            else if (sorting.Equals("Owner ASC"))
            {
                query = query.OrderBy(p => p.OwnerName);
            }
            else if (sorting.Equals("Owner DESC"))
            {
                query = query.OrderByDescending(p => p.OwnerName);
            }
            else if (sorting.Equals("ProjectName ASC"))
            {
                query = query.OrderBy(p => p.ProjectName);
            }
            else if (sorting.Equals("ProjectName DESC"))
            {
                query = query.OrderByDescending(p => p.ProjectName);
            }
            else if (sorting.Equals("IAndCEstimate ASC"))
            {
                query = query.OrderBy(p => p.IAndCEstimate);
            }
            else if (sorting.Equals("IAndCEstimate DESC"))
            {
                query = query.OrderByDescending(p => p.IAndCEstimate);
            }
            else if (sorting.Equals("Division ASC"))
            {
                query = query.OrderBy(p => p.Division);
            }
            else if (sorting.Equals("Division DESC"))
            {
                query = query.OrderByDescending(p => p.Division);
            }
            else if (sorting.Equals("LastAddendumRecvd ASC"))
            {
                query = query.OrderBy(p => p.LastAddendumRecvd);
            }
            else if (sorting.Equals("LastAddendumRecvd DESC"))
            {
                query = query.OrderByDescending(p => p.LastAddendumRecvd);
            }
            else if (sorting.Equals("Estimator ASC"))
            {
                query = query.OrderBy(p => p.Estimator);
            }
            else if (sorting.Equals("Estimator DESC"))
            {
                query = query.OrderByDescending(p => p.Estimator);
            }
            else if (sorting.Equals("AdevertiseDate ASC"))
            {
                query = query.OrderBy(p => p.AdvertiseDate);
            }
            else if (sorting.Equals("AdevertiseDate DESC"))
            {
                query = query.OrderByDescending(p => p.AdvertiseDate);
            }
            else if (sorting.Equals("ProjectFolder ASC"))
            {
                query = query.OrderBy(p => p.ProjectFolder);
            }
            else if (sorting.Equals("ProjectFolder DESC"))
            {
                query = query.OrderByDescending(p => p.ProjectFolder);
            }
            else if (sorting.Equals("QuoteNumber ASC"))
            {
                query = query.OrderBy(p => p.QuoteNumber);
            }
            else if (sorting.Equals("QuoteNumber DESC"))
            {
                query = query.OrderByDescending(p => p.QuoteNumber);
            }
            else
            {
                query = query.OrderBy(p => p.OwnerName); //Default!
            }

            return count > 0
                       ? query.Skip(startIndex).Take(count).ToList() //Paging
                       : query.ToList(); //No paging
        }

        public int GetBidlogCount()
        {
            return db.tblBID_Logs.Count();
        }

       
    }
}
