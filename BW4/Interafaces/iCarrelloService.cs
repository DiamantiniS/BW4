using BW4.Models;

namespace BW4.Interafaces
{
    public interface iCarrelloService
    {
       public Carrello getCarrello();

       public void addCarrello(int ProdottoID, int Quantita);

       public void deleteCarrello(int id);
    }
}
