namespace BW4.Models
{
    public class Categoria
    {
        public int CategoriaID { get; set; }
        public string NomeCategoria { get; set; }
        public string Descrizione { get; set; }
        public ICollection<Prodotto> Prodotti { get; set; }
    }
}