﻿
using System.ComponentModel.DataAnnotations;

namespace OtoServisSatis.Entities
{
    public class Musteri : IEntity
    {
        public int Id { get; set; }
        [Display(Name = "Araç")]
        public int AracId { get; set; }
        [StringLength(50)]
        [Display(Name = "Adı"), Required(ErrorMessage = "{0} Boş Bırakılamaz")]
        public string Adi { get; set; }
        [StringLength(50), Required(ErrorMessage = "{0} Boş Bırakılamaz")]
        [Display(Name = "Soyadı")]
        public string Soyadi { get; set; }
        [StringLength(11)]
        [Display(Name = "TC Numarası")]
        public string? TcNo { get; set; }
        [StringLength(50), Required(ErrorMessage = "{0} Boş Bırakılamaz")]
        public string Email { get; set; }
        [StringLength(500)]
        public string Adres { get; set; }
        [StringLength(15)]
        public string Telefon { get; set; }
        public string? Notlar { get; set; }
        [Display(Name = "Araç")]
        public virtual Arac? Arac { get; set; }
        public string? AdSoyad
        {
            get
            {
                return this.Adi + " " + this.Soyadi;
            }
        }
    }
}
