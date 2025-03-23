
namespace OtoServisSatis.Entities
{
    public class Satis : IEntity
    {
        public int Id { get; set; }
        public int AracId { get; set; }
        public int MusteriId { get; set; }
        public decimal  SatisFiyati { get; set; }
        public DateTime SatisTarihi { get; set; }
        public Arac? Arac { get; set; }
        public  Musteri? Musteri { get; set; }

    }
}
