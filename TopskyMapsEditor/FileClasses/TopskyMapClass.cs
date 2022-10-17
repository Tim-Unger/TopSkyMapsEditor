using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopskyMapsEditor
{
    //TODO Symbols
    public class TopskyMap
    {
        public string Name { get; set; }
        public string Folder { get; set; }
        public int? Layer { get; set; }
        public int? Zoom { get; set; }
        public List<Active>? ActiveList { get; set; }
        public string? Color { get; set; }
        public Style? Style { get; set; }
        public List<Line>? Lines { get; set; }
    }

    public class Folder
    {
        public string FolderName { get; set; }
        public List<string> FolderContent { get; set; }
    }

    //Symbols etc
    public enum ActiveType
    {
        Always,
        Time,
        Notam,
        Area,
        Runway,
        Id,
    };
    public class Active
    {
        public ActiveType ActiveType { get; set; }
        public bool IsAlwaysActive { get; set; }

        //Active by Time
        public bool? IsActiveByTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string[]? WeekDays { get; set; }

        //Active by NOTAM
        public bool? IsActiveByNotam { get; set; }
        public string NotamIcao { get; set; }
        public string NotamText { get; set; }

        //Active by Area
        public bool? IsActiveByArea { get; set; }
        public List<string>? ActiveAreasList { get; set; }
        public List<string>? NotActiveAreasList { get; set; }

        //Active by Runways
        //TODO
        public bool? IsActiveByRunway { get; set; }
        public ActiveRunway? ActiveRunway { get; set; }

        //Active by ID
        public bool IsActiveById { get; set; }

    }

    public class ActiveRunway
    {
        public List<TopskyRunway>? ArrivalRunways { get; set; }
        public List<TopskyRunway>? NotArrivalRunways { get; set; }
        public List<TopskyRunway>? DepartureRunways { get; set; }
        public List<TopskyRunway>? NotDepartureRunways { get; set; }
    }

    public class TopskyRunway
    {
        public string Icao { get; set; }
        public string Identifier { get; set; }
    }

    public enum StyleType
    {
        Solid,
        Dash,
        Dot,
        DashDot,
        DashDotDot,
    }
    public class Style
    {
        public StyleType StyleType { get; set; }
        public int? Width { get; set; }
    }

    public class Line
    {
        public string? StartLatitude { get;set; }
        public string? StartLongitude { get; set; }
        public string? EndLatitude { get; set; }
        public string? EndLongitude { get; set; }
        public string? StartPoint { get; set; }
        public string? EndPoint { get; set; }
    }
}
