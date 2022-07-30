namespace ChartAPI.Model.DTOs
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public int TouristRating { get; set; }
        public int DateEstablished { get; set; }
        public int EstimatedPopulation { get; set; }
        public string Currency { get; set; }
        public string Weather { get; set; }
    }
}
