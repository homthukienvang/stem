namespace Model
{
    using System;
    using System.Collections.Generic;
    public partial class Advertisement
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AdvContent { get; set; }
        public string AdvImage { get; set; }
        public string AdvLink { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ClickPerDay { get; set; }
        public int Flag { get; set; }
        public int ClickLinkCount { get; set; }

        public long RowIndex { get; set; }
        public int TotalRowCount { get; set; }        
        public string CityCode { get; set; }
        public string DistrictCode { get; set; }
        public string SchoolCode { get; set; }
        public string UserName { get; set; }
    }
}
