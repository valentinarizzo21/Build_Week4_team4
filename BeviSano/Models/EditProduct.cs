using System.ComponentModel.DataAnnotations;

namespace BeviSano.Models
{
    public class EditProduct
    {
        public Guid Id_Product { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Il nome è obbligatorio")]
        [StringLength(
            250,
            MinimumLength = 2,
            ErrorMessage = "Il nome deve essere compreso tra 2 e 250 caratteri"
        )]
        public string? Name_Product { get; set; }

        [Display(Name = "Prezzo")]
        [Required(ErrorMessage = "Il prezzo è obbligatorio")]
        [Range(0, 100000, ErrorMessage = "Il prezzo deve essere compreso tra 0 e 100000 euro")]
        public decimal? Price_Product { get; set; }

        [Display(Name = "Quantità disponibile")]
        [Required(ErrorMessage = "La quantità è obbligatoria")]
        [Range(0, 1000000, ErrorMessage = "La quantità minima è 0")]
        public int? Stock_Product { get; set; }

        [Display(Name = "Produttore")]
        [Required(ErrorMessage = "Il produttore è obbligatorio")]
        [StringLength(
            250,
            MinimumLength = 3,
            ErrorMessage = "Il nome del produttore deve essere compreso tra 3 e 250 caratteri"
        )]
        public string? Seller_Product { get; set; }

        [Display(Name = "Sconto applicato")]
        [Range(0, 99, ErrorMessage = "Lo sconto applicabile deve essere compreso tra 0 e 99")]
        public decimal? Sale_Product { get; set; }

        [Display(Name = "Tempi consegna")]
        [Required(ErrorMessage = "Il tempo di consegna è obbligatorio")]
        [Range(1, 1000000, ErrorMessage = "Il numero di giorni di consegna deve essere almeno 1")]
        public int? Arrival_Date_Product { get; set; }

        [Display(Name = "Immagine principale")]
        public string? Cover_Product { get; set; }

        [Display(Name = "Immagine aggiuntiva")]
        public string? Image_One { get; set; }

        [Display(Name = "Immagine aggiuntiva")]
        public string? Image_Two { get; set; }

        [Display(Name = "Immagine aggiuntiva")]
        public string? Image_Three { get; set; }

        [Display(Name = "Categoria")]
        public int? Id_Category { get; set; }

        [Display(Name = "Descrizione")]
        [Required(ErrorMessage = "La descrizione è obbligatoria")]
        [StringLength(
            2000,
            MinimumLength = 20,
            ErrorMessage = "La descrizione deve contenere tra 20 e 2000 caratteri"
        )]
        public string? Description_Product { get; set; }
    }
}
