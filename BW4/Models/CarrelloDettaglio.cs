namespace BW4.Models
{
    public class CarrelloDettaglio
    {
        public int CarrelloDettaglioID { get; set; }
        public int CarrelloID { get; set; }
        public Carrello Carrello { get; set; }
        public int ProdottoID { get; set; }
        public Prodotto Prodotto { get; set; }
        public int Quantita { get; set; }
        public decimal Prezzo { get; set; }
    }
}
