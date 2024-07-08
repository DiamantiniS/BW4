using BW4.Models;

namespace BW4.Interfaces
{
    public interface iProdottoService
    {
        void CreaProdotto(Prodotto nuovoProdotto);

        Prodotto SingoloProdotto(int id);

        IEnumerable<Prodotto> TuttiProdotti();

        void ModificaProdotto(int id, Prodotto prodotto);

        void EliminaProdotto(int id);
    }
}
