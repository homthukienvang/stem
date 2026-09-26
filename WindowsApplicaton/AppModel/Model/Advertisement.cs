namespace Model
{
    using System;
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
        public int ClickCount { get; set; }
        public int ClickLinkCount { get; set; }
    }
}
