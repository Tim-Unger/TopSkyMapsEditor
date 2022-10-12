using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopskyMapsEditor
{
    public class TopskyMap
    {
        public string Name { get; set; }
        public string Folder { get; set; }
        public int? Layer { get; set; }
        public int? Zoom { get; set; }
        public Active? Active { get; set; }
    }

    public class Active
    {
        public bool IsAlwaysActive { get; set; }

        //Active by Time
        public bool? IsActiveByTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int[]? WeekDays { get; set; }

        //Active by NOTAM
        public bool? IsActiveByNotam { get; set; }
        public string NotamIcao { get; set; }
        public string NotamText { get; set; }

        //Active by Area
        public bool? IsActiveByArea { get; set; }
        public List<string>? ActiveAreasList { get; set; }
        public List<string>? NotActiveAreasList { get; set; }

        //Active by Runways

    }
}
