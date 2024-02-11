namespace Daark.Entities
{
    public class ReportDto
    {
        public DateTime? Date { get; set; }

        public int? Calls { get; set; }

        public int? FollowUp { get; set; }

        public int? Meeting { get; set; }

        public int? Deal { get; set; }

        public int? LeedsInSheet { get; set; }
     
        public int? PortalBayutDubai { get; set; }

        public int? PortalBayutOther { get; set; }

        public int? PortalPropertyFinderDubai { get; set; }

        public int? PortalPropertyFinderRak { get; set; }
        public int? PortalSemsar { get; set; }

        public int? LeadBayut { get; set; }

        public int? LeadPropertyFinder { get; set; }

        public int? LeadSemsar { get; set; }
        public string? ThingsIDidToday { get; set; }
    }
}