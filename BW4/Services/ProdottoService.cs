using BW4.Interaface;
using BW4.Models;
using System.Data.SqlClient;

namespace BW4.Services
{
    public class ProdottoService : SQLServerServiceBase, iProdottoService
    {
        public ProdottoService(IConfiguration config) : base(config) { }

        public void CreaProdotto(Prodotto nuovoProdotto)
        {
            using var connection = getConnection();
            connection.Open();
            using var command = getCommand("INSERT INTO Prodotti (NomeProdotto, Descrizione, Prezzo, QuantitaDisponibile, CategoriaID, ImmagineCoverHome, ImmagineDettagli, DataAggiunta) VALUES (@NomeProdotto, @Descrizione, @Prezzo, @QuantitaDisponibile, @CategoriaID, @ImmagineCoverHome, @ImmagineDettagli, @DataAggiunta)");
            command.Parameters.Add(new SqlParameter("@NomeProdotto", nuovoProdotto.NomeProdotto));
            command.Parameters.Add(new SqlParameter("@Descrizione", nuovoProdotto.Descrizione));
            command.Parameters.Add(new SqlParameter("@Prezzo", nuovoProdotto.Prezzo));
            command.Parameters.Add(new SqlParameter("@QuantitaDisponibile", nuovoProdotto.QuantitaDisponibile));
            command.Parameters.Add(new SqlParameter("@CategoriaID", nuovoProdotto.CategoriaID));
            command.Parameters.Add(new SqlParameter("@ImmagineCoverHome", nuovoProdotto.ImmagineCoverHome));
            command.Parameters.Add(new SqlParameter("@ImmagineDettagli", nuovoProdotto.ImmagineDettagli));
            command.Parameters.Add(new SqlParameter("@DataAggiunta", nuovoProdotto.DataAggiunta));
            command.ExecuteNonQuery();
        }

        public Prodotto SingoloProdotto(int id)
        {
            using var connection = getConnection();
            connection.Open();
            using var command = getCommand("SELECT * FROM Prodotti WHERE ProdottoID = @ProdottoID");
            command.Parameters.Add(new SqlParameter("@ProdottoID", id));
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Prodotto
                {
                    ProdottoID = (int)reader["ProdottoID"],
                    NomeProdotto = (string)reader["NomeProdotto"],
                    Descrizione = (string)reader["Descrizione"],
                    Prezzo = (decimal)reader["Prezzo"],
                    QuantitaDisponibile = (int)reader["QuantitaDisponibile"],
                    CategoriaID = (int)reader["CategoriaID"],
                    ImmagineCoverHome = (string)reader["ImmagineCoverHome"],
                    ImmagineDettagli = (string)reader["ImmagineDettagli"],
                    DataAggiunta = (DateTime)reader["DataAggiunta"]
                };
            }
            return null;
        }

        public IEnumerable<Prodotto> TuttiProdotti()
        {
            var prodotti = new List<Prodotto>();
            using var connection = getConnection();
            connection.Open();
            using var command = getCommand("SELECT * FROM Prodotti");
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                prodotti.Add(new Prodotto
                {
                    ProdottoID = (int)reader["ProdottoID"],
                    NomeProdotto = (string)reader["NomeProdotto"],
                    Descrizione = (string)reader["Descrizione"],
                    Prezzo = (decimal)reader["Prezzo"],
                    QuantitaDisponibile = (int)reader["QuantitaDisponibile"],
                    CategoriaID = (int)reader["CategoriaID"],
                    ImmagineCoverHome = (string)reader["ImmagineCoverHome"],
                    ImmagineDettagli = (string)reader["ImmagineDettagli"],
                    DataAggiunta = (DateTime)reader["DataAggiunta"]
                });
            }
            return prodotti;
        }

        public void ModificaProdotto(int id, Prodotto prodotto)
        {
            using var connection = getConnection();
            connection.Open();
            using var command = getCommand("UPDATE Prodotti SET NomeProdotto = @NomeProdotto, Descrizione = @Descrizione, Prezzo = @Prezzo, QuantitaDisponibile = @QuantitaDisponibile, CategoriaID = @CategoriaID, ImmagineCoverHome = @ImmagineCoverHome, ImmagineDettagli = @ImmagineDettagli WHERE ProdottoID = @ProdottoID");
            command.Parameters.Add(new SqlParameter("@NomeProdotto", prodotto.NomeProdotto));
            command.Parameters.Add(new SqlParameter("@Descrizione", prodotto.Descrizione));
            command.Parameters.Add(new SqlParameter("@Prezzo", prodotto.Prezzo));
            command.Parameters.Add(new SqlParameter("@QuantitaDisponibile", prodotto.QuantitaDisponibile));
            command.Parameters.Add(new SqlParameter("@CategoriaID", prodotto.CategoriaID));
            command.Parameters.Add(new SqlParameter("@ImmagineCoverHome", prodotto.ImmagineCoverHome));
            command.Parameters.Add(new SqlParameter("@ImmagineDettagli", prodotto.ImmagineDettagli));
            command.Parameters.Add(new SqlParameter("@ProdottoID", id));
            command.ExecuteNonQuery();
        }

        public void EliminaProdotto(int id)
        {
            using var connection = getConnection();
            connection.Open();
            using var command = getCommand("DELETE FROM Prodotti WHERE ProdottoID = @ProdottoID");
            command.Parameters.Add(new SqlParameter("@ProdottoID", id));
            command.ExecuteNonQuery();
        }
    }

}
