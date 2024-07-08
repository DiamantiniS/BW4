namespace BW4.Models
{
    public class Prodotto
    {
        public int ProdottoID { get; set; }
        public string NomeProdotto { get; set; }
        public string Descrizione { get; set; }
        public decimal Prezzo { get; set; }
        public int QuantitaDisponibile { get; set; }
        public int CategoriaID { get; set; }
        public string ImmagineCoverHome { get; set; }
        public string ImmagineDettagli { get; set; }
        public Categoria Categoria { get; set; }
        public DateTime DataAggiunta { get; set; }
    }
}
