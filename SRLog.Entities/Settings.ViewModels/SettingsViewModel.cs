using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRLog.Entities.Settings.ViewModels
{
    public class SettingsViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SectionName { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsFixedInGrid { get; set; }
    }
}
