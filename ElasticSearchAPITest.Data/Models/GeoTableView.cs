using Nest;

namespace ElasticSearchAPITest.Data.Models
{
    public class GeoTableView
    {
        public int Id { get; set; }

        public int WardId { get; set; }

        [Completion]
        public string Ward { get; set; }

        public int DistrictId { get; set; }

        public string District { get; set; }

        public int CityId { get; set; }

        public string City { get; set; }

    }
}
