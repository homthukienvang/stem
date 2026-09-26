namespace Model
{
    using System;
    using System.Collections.Generic;

    public class News
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string NewsType { get; set; }
        public string ImageUrl { get; set; }
    }
}