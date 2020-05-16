namespace Crypto.Domain.Entities
{
    public class City
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ProvinceId { get; set; }
        public Province Province { get; set; }
    }
}
