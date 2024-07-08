using BW4.Interafaces;
using BW4.Models;
using System.Data.SqlClient;

namespace BW4.Services
{
    public class CarrelloService : SQLServerServiceBase, iCarrelloService
    {
        public CarrelloService(IConfiguration config) : base(config) { }

        public Carrello getCarrello()
        {
            var carrello = new Carrello
            {
                CarrelloDettagli = new List<CarrelloDettaglio>()
            };

            using var connection = getConnection();
            connection.Open();
            using var command = getCommand("SELECT * FROM Carrelli WHERE CarrelloID = 1"); // Supponiamo che ci sia un solo carrello per utente
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                carrello.CarrelloID = (int)reader["CarrelloID"];
                carrello.DataCreazione = (DateTime)reader["DataCreazione"];
            }

            if (carrello.CarrelloID > 0)
            {
                using var dettaglioCommand = getCommand("SELECT * FROM CarrelloDettagli WHERE CarrelloID = @CarrelloID");
                dettaglioCommand.Parameters.Add(new SqlParameter("@CarrelloID", carrello.CarrelloID));
                using var dettaglioReader = dettaglioCommand.ExecuteReader();
                while (dettaglioReader.Read())
                {
                    carrello.CarrelloDettagli.Add(new CarrelloDettaglio
                    {
                        CarrelloDettaglioID = (int)dettaglioReader["CarrelloDettaglioID"],
                        CarrelloID = (int)dettaglioReader["CarrelloID"],
                        ProdottoID = (int)dettaglioReader["ProdottoID"],
                        Quantita = (int)dettaglioReader["Quantita"],
                        Prezzo = (decimal)dettaglioReader["Prezzo"]
                    });
                }
            }

            return carrello;
        }

        public void addCarrello(int prodottoId, int quantita)
        {
            using var connection = getConnection();
            connection.Open();

            var carrello = getCarrello();
            if (carrello.CarrelloID == 0)
            {
                using var createCommand = getCommand("INSERT INTO Carrelli (DataCreazione) VALUES (@DataCreazione); SELECT SCOPE_IDENTITY();");
                createCommand.Parameters.Add(new SqlParameter("@DataCreazione", DateTime.Now));
                carrello.CarrelloID = Convert.ToInt32(createCommand.ExecuteScalar());
            }

            using var dettaglioCommand = getCommand("SELECT * FROM CarrelloDettagli WHERE CarrelloID = @CarrelloID AND ProdottoID = @ProdottoID");
            dettaglioCommand.Parameters.Add(new SqlParameter("@CarrelloID", carrello.CarrelloID));
            dettaglioCommand.Parameters.Add(new SqlParameter("@ProdottoID", prodottoId));
            using var dettaglioReader = dettaglioCommand.ExecuteReader();
            if (dettaglioReader.Read())
            {
                var quantitaEsistente = (int)dettaglioReader["Quantita"];
                quantita += quantitaEsistente;

                using var updateCommand = getCommand("UPDATE CarrelloDettagli SET Quantita = @Quantita WHERE CarrelloID = @CarrelloID AND ProdottoID = @ProdottoID");
                updateCommand.Parameters.Add(new SqlParameter("@Quantita", quantita));
                updateCommand.Parameters.Add(new SqlParameter("@CarrelloID", carrello.CarrelloID));
                updateCommand.Parameters.Add(new SqlParameter("@ProdottoID", prodottoId));
                updateCommand.ExecuteNonQuery();
            }
            else
            {
                using var prodottoCommand = getCommand("SELECT Prezzo FROM Prodotti WHERE ProdottoID = @ProdottoID");
                prodottoCommand.Parameters.Add(new SqlParameter("@ProdottoID", prodottoId));
                var prezzo = (decimal)prodottoCommand.ExecuteScalar();

                using var insertCommand = getCommand("INSERT INTO CarrelloDettagli (CarrelloID, ProdottoID, Quantita, Prezzo) VALUES (@CarrelloID, @ProdottoID, @Quantita, @Prezzo)");
                insertCommand.Parameters.Add(new SqlParameter("@CarrelloID", carrello.CarrelloID));
                insertCommand.Parameters.Add(new SqlParameter("@ProdottoID", prodottoId));
                insertCommand.Parameters.Add(new SqlParameter("@Quantita", quantita));
                insertCommand.Parameters.Add(new SqlParameter("@Prezzo", prezzo));
                insertCommand.ExecuteNonQuery();
            }
        }

        public void deleteCarrello(int id)
        {
            using var connection = getConnection();
            connection.Open();
            using var command = getCommand("DELETE FROM CarrelloDettagli WHERE CarrelloDettaglioID = @CarrelloDettaglioID");
            command.Parameters.Add(new SqlParameter("@CarrelloDettaglioID", id));
            command.ExecuteNonQuery();
        }
    }
}
