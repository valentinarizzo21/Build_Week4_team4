use bw_4_team_4;

create table Categories (
Id_Category INT IDENTITY(1, 1) PRIMARY KEY,
Title NVARCHAR(250) NOT NULL,
);

create table Products (
Id_Product UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
Name_Product NVARCHAR(250) NOT NULL,
Price_Product MONEY NOT NULL,
Description_Product NVARCHAR(2000),
Stock_Product INT NOT NULL,
Seller_Product NVARCHAR(250) NOT NULL,
Sale_Product DECIMAL(5, 2) DEFAULT 0,
Arrival_Date_Product INT NOT NULL,
Cover_Product NVARCHAR(2000) NOT NULL,
Id_Category INT,
CONSTRAINT FK_Product_Category FOREIGN KEY (Id_Category) REFERENCES Categories(Id_Category),
CONSTRAINT CK_Price_Product CHECK (Price_Product > 0),
CONSTRAINT CK_Stock_Product CHECK (Stock_Product >= 0),
CONSTRAINT CK_Sale_Product CHECK (Sale_Product < 100 and Sale_Product >= 0),
CONSTRAINT CK_Arrival_Date_Product CHECK (Arrival_Date_Product >= 1),
);

create table Images(
Id_Image UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
Id_Product UNIQUEIDENTIFIER,
Url_Image NVARCHAR(2000) NOT NULL,
CONSTRAINT FK_Image_Product FOREIGN KEY (Id_Product) REFERENCES Products(Id_Product),
)

create table  Account (
Id_Account UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
Name_Account NVARCHAR(20) NOT NULL,
Email NVARCHAR(20) UNIQUE NOT NULL,
Password_Account NVARCHAR(20) NOT NULL,
Admin_Bit BIT DEFAULT 0,
Fidelity_Card BIT DEFAULT 0,
)


create table Cart(
Id_Cart UNIQUEIDENTIFIER NOT NULL,
Date_Add DATETIME DEFAULT GETDATE(),
Quantity_Product INT NOT NULL DEFAULT 1,
Id_Product UNIQUEIDENTIFIER NOT NULL,
CONSTRAINT FK_Product_Cart FOREIGN KEY (Id_Product) REFERENCES Products(Id_Product),
CONSTRAINT CK_Quantity_Product CHECK (Quantity_Product >= 1),
CONSTRAINT PK_Cart PRIMARY KEY (Id_Cart, Id_Product)
)



insert into Account (Name_Account, Email, Password_Account, Admin_Bit) VALUES
('Cobain', 'kurt.cobain@ciao.rip', 'CourtneyLove1994', 1),
('sa', 'sa@ciao.com', 'sa', 1);

INSERT INTO Categories (Title) VALUES
('Vino Bianco'),
('Vino Rosso'),
('Rum'),
('Liquori'),
('Vodka'),
('Wiskey'),
('Gin');


INSERT INTO Products (Name_Product, Price_Product, Description_Product, Stock_Product, Seller_Product, Sale_Product, Arrival_Date_Product, Cover_Product, Id_Category)
VALUES
('Chianti Classico DOCG', 18.50, 'Vino rosso corposo con note di frutti rossi e spezie, tipico della Toscana.', 20, 'Cantina Toscana', 0, 3, 'https://boutiquemonteverdi.myshopify.com/cdn/shop/products/chianti_classicoDOCGLt.0.75famigliafalorni.jpg?v=1670415582', 2),
('Barolo DOCG', 35.00, 'Vino rosso robusto, con aromi di ciliegia, tabacco e un finale lungo e elegante.', 15, 'Azienda Agricola Rocca', 10, 3,'https://shop.giacosa.it/cdn/shop/files/01-barolo-docg.jpg?v=1692957595', 2),
('Prosecco DOC', 12.90, 'Vino spumante fresco e fruttato, perfetto come aperitivo.', 20, 'Cantina Bellavista', 5, 3, 'https://www.montelvini.it/cdn/shop/files/PROSECCODOCExtraDry.jpg?v=1702304735', 1),
('Brunello di Montalcino DOCG', 45.00, 'Vino rosso elegante con sentori di frutta matura, cuoio e spezie.', 50, 'Tenuta di Montalcino', 0, 3,'https://www.bottegaspa.com/wp-content/uploads/2018/02/V23090075_BRUNELLO-MONTALCINO-DOCG-BOTTEGA-CL75_WB_HR.jpg', 1),
('Sauvignon Blanc IGT', 14.00, 'Vino bianco secco, aromatico con note di agrumi e erbe fresche.', 10, 'Cantina Vigneti Bianchi', 0, 3, 'https://distilwine.com/484-pd4_def/sauvignon-blanc-igt-veneto-borin-75cl.jpg', 1),
('Nero d''Avola IGT', 16.50, 'Vino rosso siciliano con sentori di frutti scuri, prugna e un leggero retrogusto speziato.', 40, 'Azienda Agricola Sicilia', 0, 3, 'https://cantinesgarzi.com/wp-content/uploads/2020/02/Luigi-Leonardo-Nero-dAvola-Syrah-IGT.jpg', 2),
('Champagne Brut', 49.90, 'Spumante francese secco, con bollicine fini e un aroma di frutta matura e lievito.', 3, 'Champagne Pommery', 0, 7, 'https://telarodistribuzione.it/wp-content/uploads/2021/02/pol-roger-.png', 1),
('Vino Rosato IGT', 10.00, 'Vino fresco e fruttato con delicate note floreali, ideale per il pranzo estivo.', 12, 'Cantina Rosato del Sud', 5, 3, 'https://www.salentowineshop.com/wp-content/uploads/2019/06/Calafuria-negroamaro-rose-salento-igp-tormaresca_2021.jpg', 2),
('Amarone della Valpolicella DOCG', 55.00, 'Vino rosso strutturato con aromi di frutta secca, cioccolato e spezie.', 10, 'Cantina Valpolicella', 5, 14, 'https://www.marcobacco.it/wp-content/uploads/2020/07/zenato_amarone_della_valpolicella_veneto-1.jpg', 2),
('Merlot IGT', 13.90, 'Vino rosso morbido con note di prugna e frutta matura, ideale per pasti leggeri.', 18, 'Tenuta La Vigna', 0, 3, 'https://www.fratellimazza.it/4199/merlot-lazio-igt-casale-del-giglio.jpg', 2),
('Rum Ron Diplomatico', 42.00, 'Rum venezuelano premium, ricco di note di vaniglia e frutta secca, ideale per cocktail o da degustare puro.', 4, 'Diplomatico', 10, 3, 'https://www.piacenzadabere.it/wp-content/uploads/2020/04/diplomatico-1024x1024.jpg', 3),
('Tequila Patron Silver', 50.00, 'Tequila messicana premium, dal gusto fresco e pulito, perfetta per margarita o da bere liscio.', 6, 'Patron', 0, 7, 'https://www.marcobacco.it/wp-content/uploads/2020/07/patron_silver_tequila_messico.jpg', 4),
('Vodka Belvedere', 38.00, 'Vodka polacca di alta qualità, dal gusto liscio e setoso, ideale per cocktail sofisticati.', 10, 'Belvedere', 0, 3, 'https://s.tannico.it/media/catalog/product/cache/1/thumbnail/0dc2d03fe217f8c83829496872af24a0/b/e/belv1.jpg', 5),
('Whiskey Jameson', 22.00, 'Whiskey irlandese morbido con un profilo di frutta secca e vaniglia, perfetto per un Irish Coffee.', 12, 'Jameson', 5, 3, 'https://m.media-amazon.com/images/I/61UziizAALL.jpg', 6),
('Amaretto Disaronno', 25.00, 'Liquore dolce italiano con aroma di mandorla, perfetto per cocktail o da sorseggiare da solo.', 15, 'Disaronno', 10, 3, 'https://shop.rivoldrink.it/wp-content/uploads/2023/04/Disaronno_v2.jpg', 4),
('Vermouth Martini Rosso', 14.00, 'Vermouth rosso italiano, con un profilo aromatico di erbe, spezie e agrumi, ideale per un Negroni.', 20, 'Martini', 5, 3, 'https://www.kalsa.store/wp-content/uploads/LQ0233.jpg', 4),
('Limoncello di Capri', 18.00, 'Liquore italiano a base di limoni di Capri, fresco e aromatico, perfetto come digestivo.', 8, 'Limoncello di Capri', 0, 3, 'https://www.valentinienoteca.it/272249-large_default/limoncello-di-capri-100cl-32-vol.jpg', 4),
('Cointreau', 28.00, 'Liquore francese all''arancia, ideale per cocktails come Cosmopolitan o Margarita.', 19, 'Cointreau', 10, 3, 'https://www.carrefour.it/on/demandware.static/-/Sites-carrefour-master-catalog-IT/default/dwb27f44a8/large/COINTREAUGR40-3035542004206-1.png', 4),
('Bourbon Woodford Reserve', 55.00, 'Bourbon americano di alta classe, dal gusto ricco e morbido con note di vaniglia e caramello.', 14, 'Woodford Reserve', 15, 5, 'https://www.superbar.it/958-thickbox_default/woodford-reserve-kentucky-straight-bourbon-whiskey.jpg', 6),
('Gin Mare', 27.90, 'Gin Mare è un gin premium spagnolo che cattura l''essenza del Mediterraneo.', 3, 'Woodford Reserve', 0, 3, 'https://www.enotecacorsi.it/wp-content/uploads/GIN-MARE.png', 7),
('Bumbu Rum',33.90, 'il prodotto viene distillato e miscelato a Barbados, la culla del rum, dove il liquore è stato creato per la prima volta circa 400 anni fa',25, 'Bumbu Rum Company',0, 5,'https://d2j6dbq0eux0bg.cloudfront.net/images/73207262/2983448215.jpg',3),
('Bacardi Carta Blanca' ,15.20 , 'Floreale e fruttato, il rum BACARDÍ Carta Blanca racchiude aromi di fiori d arancio, lavanda e rosa, abbinati ad albicocca, lime, cocco leggero e banana matura.', 15, 'Bacardi',0 , 3, 'https://www.etilika.it/18179-large_default/rum-bacardi-carta-blanca-bacardi-1-lt.jpg', 3),
('The Kraken', 19.90 ,'Colore scuro con riflessi ramati, sprigiona aromi intensi di caramello, vaniglia e spezie, con note di caffè e cioccolato',10, 'Kraken Rum', 0, 3, 'https://media.prezzemoloevitale.it/media/catalog/product/cache/5d31a48827fd19ae04ad3d8423e100b5/0/0/000075157_1.jpg', 3),
('Absolut Vodka', 15.90, 'marchio leader di vodka Premium che offre il vero gusto della vodka nei gusti originali o nei tuoi gusti preferiti, realizzati con ingredienti naturali',13,'Absolut', 5,3,'https://www.absolut.com/cdn-cgi/image/format=auto,quality=55,width=414/wp-content/uploads/absolut-vodka-original-2021-against-white-background.jpg',5),
('Ciroc Vodka', 40.00, 'La vodka CÎROC è distillata da uve francesi pregiate e al centro di CÎROC si trova il maestro distillatore Jean-Sébastien Robicquet e la sua distilleria Maison Villevert',8,'CÎROC' , 0, 5, 'https://www.drinkshoponline.com/_upload/prodotti/thumb/16496_700_1200_0.jpg',5),
('Jack Daniels',23.50, 'Al naso: emergono note di caramello e vaniglia a cui seguono note di fumo di legno d acero e zucchero caramellato', 12,'Jack Daniels' , 0 , 4, 'https://liquo.it/123-pd4_def/jack-daniels-tennessee-1-litro-whisky.jpg', 6 ),
('Bombay Gin' , 16.90, 'Godetevi la straordinaria morbidezza e il gusto perfettamente bilanciato del Bombay Sapphire Gin',5,'Bombay', 0, 3 , 'https://www.marcobacco.it/wp-content/uploads/2020/07/bacardi_bombay_sapphire_london_dry_gin_inghilterra.jpg', 7),
('Malfy Gin' , 23.80, 'Malfy Gin è il distillato al ginepro di qualità super premium ispirato ai colori, ai sapori e allo spirito vivace della costiera amalfitana', 21, 'Malfy' , 15 , 3,'https://static.cosaporto.it/media/2023/03/5000299296028.jpg', 7);

 