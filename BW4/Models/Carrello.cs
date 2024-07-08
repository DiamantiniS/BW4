namespace BW4.Models
{
    public class Carrello
    {
        public int CarrelloID { get; set; }
        public DateTime DataCreazione { get; set; }
        public ICollection<CarrelloDettaglio> CarrelloDettagli { get; set; }
    }
}
