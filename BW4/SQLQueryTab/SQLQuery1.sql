ALTER TABLE Prodotti
ADD ImmagineCoverHome NVARCHAR(255),
    ImmagineDettagli NVARCHAR(255);
INSERT INTO Categorie (NomeCategoria, Descrizione) VALUES
('Elettronica', 'Prodotti elettronici e gadget'),
('Abbigliamento', 'Vestiti e accessori'),
('Cucina', 'Utensili e accessori da cucina'),
('Giardinaggio', 'Attrezzi e piante per il giardino'),
('Libri', 'Libri di vario genere'),
('Giocattoli', 'Giocattoli per bambini di tutte le età'),
('Sport', 'Attrezzature sportive e abbigliamento'),
('Bellezza', 'Prodotti per la cura personale'),
('Auto e Moto', 'Accessori per auto e moto'),
('Ufficio', 'Materiale da ufficio e cancelleria');