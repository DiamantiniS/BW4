using BW4.Models;

namespace BW4.Interfaces
{
    public interface iCarrelloDettagliService
    {
        void AggiungiCarrello(int CarrelloID, int ProdottoID);

        IEnumerable<Prodotto> TuttiProdottiCarrello();

        void ModificaQuantita(int Quantita, int CarrelloID);


        void EliminaCarrello(int id);

    }
}
