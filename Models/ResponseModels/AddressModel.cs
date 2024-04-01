namespace Models.ResponseModels
{
    public class AddressModel
    {
        public string Street { get; set; }
        public string Suite { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public GeoModel Geo { get; set; }
    }
}
