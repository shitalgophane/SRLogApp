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
            List<tblBID_Log> list = count > 0
                       ? query.Skip(startIndex).Take(count).ToList() //Paging
                       : query.ToList(); //No paging

            foreach (tblBID_Log b in list)
            {
                b.DOW = Convert.ToDateTime(b.BidDate).ToString("ddd");
                string[] strsplit = b.BiddingAs.Split('#');

                string strBidAs = "";
                if (strsplit.Length > 1)
                {
                    for (int i = 0; i < strsplit.Length - 1; i++)
                    {
                        switch (strsplit[i])
                        {
                            case "0":
                                strBidAs = strBidAs + "I&C, ";
                                break;
                            case "1":
                                strBidAs = strBidAs + "Electrical, ";
                                break;
                            case "2":
                                strBidAs = strBidAs + "Prime, ";
                                break;
                            case "3":
                                strBidAs = strBidAs + "Unknown, ";
                                break;
                            case "4":
                                strBidAs = strBidAs + "Not Bidding, ";
                                break;
                            case "5":
                                strBidAs = strBidAs + "Not Qualified, ";
                                break;
                            case "6":
                                strBidAs = strBidAs + "Mechanical, ";
                                break;
                        }
                    }
                    strBidAs = strBidAs.Remove(strBidAs.LastIndexOf(','));
                    b.BiddingAs = strBidAs;
                }
                else if (strsplit.Length == 1)
                {
                    switch (strsplit[0])
                    {
                        case "0":
                            strBidAs = "I&C ";
                            break;
                        case "1":
                            strBidAs = "Electrical ";
                            break;
                        case "2":
                            strBidAs = "Prime ";
                            break;
                        case "3":
                            strBidAs = "Unknown ";
                            break;
                        case "4":
                            strBidAs = "Not Bidding ";
                            break;
                        case "5":
                            strBidAs = "Not Qualified ";
                            break;
                        case "6":
                            strBidAs = "Mechanical ";
                            break;
                    }
                    b.BiddingAs = strBidAs;
                }
            }

            return list;


        }

        public int GetBidlogCount()
        {
            return db.tblBID_Logs.Count();
        }


    }
}
