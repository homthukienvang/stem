namespace Model
{
    using System;
    using System.Collections.Generic;
    public partial class AdvReport
    {
        public Nullable<long> RowIndex { get; set; }
        public int TotalRowCount { get; set; }
        public string CityCode { get; set; }
        public string DistrictCode { get; set; }
        public string SchoolCode { get; set; }
        public string UserName { get; set; }
        public int ClickCount { get; set; }
    }
}
