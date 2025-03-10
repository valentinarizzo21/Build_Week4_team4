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
('Vodka Belvedere', 38.00, 'Vodka polacca di alta qualit , dal gusto liscio e setoso, ideale per cocktail sofisticati.', 10, 'Belvedere', 0, 3, 'https://s.tannico.it/media/catalog/product/cache/1/thumbnail/0dc2d03fe217f8c83829496872af24a0/b/e/belv1.jpg', 5),
('Whiskey Jameson', 22.00, 'Whiskey irlandese morbido con un profilo di frutta secca e vaniglia, perfetto per un Irish Coffee.', 12, 'Jameson', 5, 3, 'https://m.media-amazon.com/images/I/61UziizAALL.jpg', 6),
('Amaretto Disaronno', 25.00, 'Liquore dolce italiano con aroma di mandorla, perfetto per cocktail o da sorseggiare da solo.', 15, 'Disaronno', 10, 3, 'https://shop.rivoldrink.it/wp-content/uploads/2023/04/Disaronno_v2.jpg', 4),
('Vermouth Martini Rosso', 14.00, 'Vermouth rosso italiano, con un profilo aromatico di erbe, spezie e agrumi, ideale per un Negroni.', 20, 'Martini', 5, 3, 'https://www.kalsa.store/wp-content/uploads/LQ0233.jpg', 4),
('Limoncello di Capri', 18.00, 'Liquore italiano a base di limoni di Capri, fresco e aromatico, perfetto come digestivo.', 8, 'Limoncello di Capri', 0, 3, 'https://www.valentinienoteca.it/272249-large_default/limoncello-di-capri-100cl-32-vol.jpg', 4),
('Cointreau', 28.00, 'Liquore francese all arancia, ideale per cocktails come Cosmopolitan o Margarita.', 19, 'Cointreau', 10, 3, 'https://www.carrefour.it/on/demandware.static/-/Sites-carrefour-master-catalog-IT/default/dwb27f44a8/large/COINTREAUGR40-3035542004206-1.png', 4),
('Bourbon Woodford Reserve', 55.00, 'Bourbon americano di alta classe, dal gusto ricco e morbido con note di vaniglia e caramello.', 14, 'Woodford Reserve', 15, 5, 'https://www.superbar.it/958-thickbox_default/woodford-reserve-kentucky-straight-bourbon-whiskey.jpg', 6),
('Gin Mare', 27.90, 'Gin Mare   un gin premium spagnolo che cattura l''essenza del Mediterraneo.', 3, 'Woodford Reserve', 0, 3, 'https://www.enotecacorsi.it/wp-content/uploads/GIN-MARE.png', 7),
('Bumbu Rum',33.90, 'il prodotto viene distillato e miscelato a Barbados, la culla del rum, dove il liquore è stato creato per la prima volta circa 400 anni fa',25, 'Bumbu Rum Company',0, 5,'https://d2j6dbq0eux0bg.cloudfront.net/images/73207262/2983448215.jpg',3),
('Bacardi Carta Blanca' ,15.20 , 'Floreale e fruttato, il rum BACARDÍ Carta Blanca racchiude aromi di fiori d arancio, lavanda e rosa, abbinati ad albicocca, lime, cocco leggero e banana matura.', 15, 'Bacardi',0 , 3, 'https://www.etilika.it/18179-large_default/rum-bacardi-carta-blanca-bacardi-1-lt.jpg', 3),
('The Kraken', 19.90 ,'Colore scuro con riflessi ramati, sprigiona aromi intensi di caramello, vaniglia e spezie, con note di caffè e cioccolato',10, 'Kraken Rum', 0, 3, 'https://media.prezzemoloevitale.it/media/catalog/product/cache/5d31a48827fd19ae04ad3d8423e100b5/0/0/000075157_1.jpg', 3),

('Absolut Vodka', 15.90, 'marchio leader di vodka Premium che offre il vero gusto della vodka nei gusti originali o nei tuoi gusti preferiti, realizzati con ingredienti naturali',13,'Absolut', 5,3,'https://www.absolut.com/cdn-cgi/image/format=auto,quality=55,width=414/wp-content/uploads/absolut-vodka-original-2021-against-white-background.jpg',5),
('Ciroc Vodka', 40.00, 'La vodka CÎROC è distillata da uve francesi pregiate e al centro di CÎROC si trova il maestro distillatore Jean-Sébastien Robicquet e la sua distilleria Maison Villevert',8,'CÎROC' , 0, 5, 'https://www.drinkshoponline.com/_upload/prodotti/thumb/16496_700_1200_0.jpg',5),

('Jack Daniels',23.50, 'Al naso: emergono note di caramello e vaniglia a cui seguono note di fumo di legno d acero e zucchero caramellato', 12,'Jack Daniels' , 0 , 4, 'https://liquo.it/123-pd4_def/jack-daniels-tennessee-1-litro-whisky.jpg', 6 ),

('Bombay Gin' , 16.90, 'Godetevi la straordinaria morbidezza e il gusto perfettamente bilanciato del Bombay Sapphire Gin',5,'Bombay', 0, 3 , 'https://www.decantalo.it/17816-product_img/bombay-sapphire-gin.jpg', 7),
('Malfy Gin' , 23.80, 'Malfy Gin è il distillato al ginepro di qualità super premium ispirato ai colori, ai sapori e allo spirito vivace della costiera amalfitana', 21, 'Malfy' , 15 , 3,'https://static.cosaporto.it/media/2023/03/5000299296028.jpg', 7);



insert into Images (Id_Product , Url_Image) values
((select Id_Product from Products where Name_Product = 'Barolo DOCG') , 'https://i.ebayimg.com/images/g/9KAAAOSwwX5nm7px/s-l1200.jpg'),
((select Id_Product from Products where Name_Product = 'Barolo DOCG'), 'https://images.pandolfini.it/@img/_large/9e9d38dec24ce9e8e0b9ca2e536bef1851f8756c.webp/-barolo-vigneto-pira-giacosa-fratelli-1985--asta-.webp'),

((select Id_Product from Products where Name_Product = 'Prosecco DOC'),'https://www.montelvini.it/cdn/shop/files/DSCF7652.jpg?v=1668694970&width=533'),
((select Id_Product from Products where Name_Product = 'Prosecco DOC'), 'https://www.wine-searcher.com/images/labels/45/07/10984507.jpg'),
((select Id_Product from Products where Name_Product = 'Prosecco DOC'), 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS2S6zmchrh7_aIA4lGhVGokOBkp9RaL3ntIAejo__zX2Dhx_xecz1k-iIwV2szKqWjyDg&usqp=CAU'),

((select Id_Product from Products where Name_Product = 'Brunello di Montalcino DOCG'), 'https://citydrinks.com/cdn-cgi/image/fit=contain,width=936/https://s3.me-central-1.amazonaws.com/catalog.citydrinks.com/offers/52e0c1fe-a7ee-4aaa-8b42-dbd292a3b252.jpeg'),
((select Id_Product from Products where Name_Product = 'Brunello di Montalcino DOCG'), 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQGoD1RGLjetkUYEPFaftUe5nq12fRpitOrNg&s'),

((select Id_Product from Products where Name_Product = 'Sauvignon Blanc IGT'), 'https://www.ilmangiaweb.it/immagini/prodotti/2017_05/82/822823586738/gallery/t_sauvignon-blanc-igt-veneto_p822823586738_01.jpg'),
((select Id_Product from Products where Name_Product = 'Sauvignon Blanc IGT'), 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTXJ2O-ZoV_2_ZiPiL1fSMk7xS7bzLtLKAyiw&s'),

((select Id_Product from Products where Name_Product = 'Nero d''Avola IGT'), 'https://images.vivino.com/labels/TeKeVPnQQsCCN8zqKbn54A.jpg'),

((select Id_Product from Products where Name_Product = 'Champagne Brut') , 'https://wine.il-quadrifogliostore.it/37213-large_default/pol-roger-champagne-brut-reserve-aoc-astuccio.jpg'),
((select Id_Product from Products where Name_Product = 'Champagne Brut') , 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRhu9XbnL6qj-zPcP66pZ5vBsO1tjWrMNeWTg&s' ),
((select Id_Product from Products where Name_Product = 'Champagne Brut') , 'https://strictlywine.co.uk/6008/champagne-pol-roger-brut-reserve-nv.jpg'),

((select Id_Product from Products where Name_Product = 'Vino Rosato IGT'), 'https://m.media-amazon.com/images/I/61jlDNJLEUS._AC_UF350,350_QL80_.jpg'),
((select Id_Product from Products where Name_Product = 'Vino Rosato IGT'),'https://www.salentowineshop.com/wp-content/uploads/2019/06/calafuria-negroamaro-ros%C3%A8-igp-salento-tormaresca-scaled-e1615104578978.jpg'),
((select Id_Product from Products where Name_Product = 'Vino Rosato IGT'), 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRRimSb3pQJTTOHfPKnHhrPw9m7RyAHg4lpmg&s'),

((select Id_Product from Products where Name_Product = 'Amarone della Valpolicella DOCG'), 'https://vinebrio.com/12673-large_default/10212-amarone-valpolicella-docg-2018-lt-15-zenato.jpg'),
((select Id_Product from Products where Name_Product = 'Amarone della Valpolicella DOCG') , 'https://htshop.it/cdn/shop/files/Amarone_2015_Zenato_4.jpg?v=1709556327'),


((select Id_Product from Products where Name_Product = 'Merlot IGT'),'https://i.ebayimg.com/images/g/i8oAAOSwjDBgUgl9/s-l1600.webp'),
((select Id_Product from Products where Name_Product = 'Merlot IGT'),'https://www.wine-searcher.com/images/labels/53/22/10425322.jpg'),

((select Id_Product from Products where Name_Product =  'Rum Ron Diplomatico'), 'https://www.enoteca-maggiolini.it/wp-content/uploads/2023/11/diplomatico-reserva-exlusiva.jpg'),
((select Id_Product from Products where Name_Product =  'Rum Ron Diplomatico'), 'https://www.bargiornale.it/wp-content/uploads/sites/4/2022/10/Diplomatico-Rum-Brown%E2%80%91Forman-.jpg'),
((select Id_Product from Products where Name_Product =  'Rum Ron Diplomatico'), 'https://enotecaostinati.com/cdn/shop/files/Unknown-11_9207604d-83c0-4234-b0fc-3789fe028de4.jpg?v=1727429402&width=1445'),

((select Id_Product from Products where Name_Product = 'Tequila Patron Silver') , 'https://m.media-amazon.com/images/I/71WYWZ01YGL.jpg'),
((select Id_Product from Products where Name_Product = 'Tequila Patron Silver'), 'https://www.patrontequila.com/binaries/mobile/content/gallery/patrontequila/recipes/patron-silver/ranch-water/lifestyle-portrait-2023.jpg'),
((select Id_Product from Products where Name_Product = 'Tequila Patron Silver') , 'https://content.thirtyonewhiskey.com/wp-content/uploads/2021/03/05090745/PXL_20210305_144854745.PORTRAIT-scaled.jpg'),

((select Id_Product from Products where Name_Product = 'Vodka Belvedere') , 'https://m.media-amazon.com/images/I/91dcB+w42VL.jpg'),
((select Id_Product from Products where Name_Product = 'Vodka Belvedere') , 'https://www.myspirits.it/155/vodka-belvedere-lt3-luminous.jpg'),
((select Id_Product from Products where Name_Product = 'Vodka Belvedere') , 'https://manintowncom.ams3.digitaloceanspaces.com/2020/06/Schermata-2020-06-10-alle-18.31.04-817x1024.png'),

((select Id_Product from Products where Name_Product = 'Whiskey Jameson') , 'https://www.jamesonwhiskey.com/wp-content/uploads/2023/09/180630_WADES_JAMESON_CT_D4_SHOT_47_ORIGINAL_SPRITE_LIME_NOPEOPLE_A_047_TYPES_RGB-1-scaled-aspect-ratio-1.02-1-e1706793544684.jpg'),
((select Id_Product from Products where Name_Product = 'Whiskey Jameson'), 'https://m.media-amazon.com/images/I/81kNerd-LvL.jpg'),

((select Id_Product from Products where Name_Product = 'Amaretto Disaronno'), 'https://www.disaronno.com/wp-content/uploads/hero-disaronno-mob.jpg'),
((select Id_Product from Products where Name_Product = 'Amaretto Disaronno') , 'https://www.bottledandboxed.com/images/products/fb-09.jpg'),
((select Id_Product from Products where Name_Product = 'Amaretto Disaronno') , 'https://d2f5fuie6vdmie.cloudfront.net/asset/ita/2024/7/15/d2f1372e7a56ca34d96665748babccb9f4d60e4d.jpg'),

((select Id_Product from Products where Name_Product = 'Vermouth Martini Rosso'), 'https://m.media-amazon.com/images/I/81+mhGh0jaL.jpg'),
((select Id_Product from Products where Name_Product = 'Vermouth Martini Rosso') , 'https://www.oakandbarrelnyc.com/wp-content/uploads/2015/02/MARTINI-amp-ROSSI-SWEET-VERMOUTH-ROSSO-750ML.jpg'),

((select Id_Product from Products where Name_Product = 'Limoncello di Capri') , 'https://www.limoncello.com/wp-content/uploads/2022/04/Limoncello-di-Capri_Di-Capri-Tonic_piscina.jpeg'),
((select Id_Product from Products where Name_Product = 'Limoncello di Capri') , 'https://bestwhisky.be/cdn/shop/files/limoncelli2kopie.jpg?v=1702831928'),
((select Id_Product from Products where Name_Product = 'Limoncello di Capri') , 'https://gustidipuglia.it/6250/molinari-liquori-limoncello-di-capri-molinari-1l.jpg'),

((select Id_Product from Products where Name_Product ='Cointreau') , 'https://images.food52.com/16qzeAHAVmxA7dhT8faPkWvS4vs=/1200x900/70bc7aa0-72d6-47b4-a976-7f6df2525daa--2018-1212_sponsored_cointreau_rb-ginger-cocktail_recipe-hero_3x2_julia-gartland_415.jpg'),
((select Id_Product from Products where Name_Product = 'Cointreau') , 'https://cdn11.bigcommerce.com/s-ey94ahpn/images/stencil/1280x1280/products/13942/7030/image_50366721__97674.1725409367.JPG?c=2'),

((select Id_Product from Products where Name_Product = 'Bourbon Woodford Reserve') , 'https://kybourbontrail.com/wp-content/uploads/2024/05/6-WoodfordReserveDistilleryHero.jpg.webp'),
((select Id_Product from Products where Name_Product = 'Bourbon Woodford Reserve') , 'https://www.sanlorenzoenoteca.it/wp-content/uploads/2022/01/woodford-reserve-bourbon-retro-enoteca-san-lorenzo-riccione.jpg'),
((select Id_Product from Products where Name_Product = 'Bourbon Woodford Reserve') , 'https://images.squarespace-cdn.com/content/v1/61affa9c931dc960c27460a1/1647849516879-6LX2UDBHI6VL0RZHRT5D/Woodford3.JPG?format=750w'),

((select Id_Product from Products where Name_Product = 'Gin Mare') , 'https://www.enotecapirovano.com/8362-thickbox_default/gin-mare-mediterranean-175-cl.jpg'),
((select Id_Product from Products where Name_Product = 'Gin Mare') , 'https://i0.wp.com/labevanda.it/wp-content/uploads/2023/09/GIN-MARE-CL.-70.png?fit=1440%2C1440&ssl=1'),
((select Id_Product from Products where Name_Product = 'Gin Mare'),'https://m.media-amazon.com/images/I/61UCJEm0yQL.jpg'), 

((select Id_Product from Products where Name_Product = 'Bumbu Rum') , 'https://i.imgur.com/fByQhW5.jpg'),
((select Id_Product from Products where Name_Product =  'Bumbu Rum' ) , 'https://www.bogeysny.com/cdn/shop/products/BumbuRum_1200x1601.jpg?v=1670619140'),
((select Id_Product from Products where Name_Product =  'Bumbu Rum' ) , 'https://icdn.bottlenose.wine/images/full/416254.jpg?ar=4:3&fit=crop&crop=entropy'),


((select Id_Product from Products where Name_Product = 'Bacardi Carta Blanca') , 'https://bodegaslacatedral.com/cdn/shop/files/RonBacardiCartaBlanca_bodegaslacatedral.jpg?crop=center&height=1200&v=1707908941&width=1200'),
((select Id_Product from Products where Name_Product = 'Bacardi Carta Blanca') , 'https://www.raschvin.com/wp-content/uploads/2023/04/FY21_UK_Bacardi_Cartablanca_Globalassettoolkit_Cola_Website_Banner_3200x1800_755886-1.jpg'),
((select Id_Product from Products where Name_Product = 'Bacardi Carta Blanca') , 'https://gowine.id/wp-content/uploads/2020/08/Bacardi-Superior.jpg'),

((select Id_Product from Products where Name_Product = 'The Kraken') , 'https://m.media-amazon.com/images/I/81XdqrEYd8L.jpg'),
((select Id_Product from Products where Name_Product = 'The Kraken') , 'https://content.thirtyonewhiskey.com/wp-content/uploads/2020/12/08084516/PXL_20201208_142710310.PORTRAIT-scaled.jpg'),
((select Id_Product from Products where Name_Product =  'The Kraken') , 'https://www.instacart.com/assets/domains/product-image/file/large_c5f8ea40-5617-455b-8756-c6636e76b8b7.jpg'),

((select Id_Product from Products where Name_Product =  'Absolut Vodka') , 'https://www.absolut.com/cdn-cgi/image/fit=cover,format=auto,height=552,quality=55,width=414/wp-content/uploads/product_absolut-vodka_atlas_global_1x1.jpg'),
((select Id_Product from Products where Name_Product =	'Absolut Vodka') , 'https://topshelfwineandspirits.com/cdn/shop/products/ScreenShot2022-02-23at3.25.57PM.jpg?v=1689634841&width=2160'),
((select Id_Product from Products where Name_Product =  'Absolut Vodka') , 'https://chillout.my/cdn/shop/products/absolut-1-01.jpg?v=1627998511'), 

((select Id_Product from Products where Name_Product =  'Ciroc Vodka') , 'https://m.media-amazon.com/images/I/61cP9LyrkpL.jpg'),
((select Id_Product from Products where Name_Product =  'Ciroc Vodka') , 'https://popmenucloud.com/cdn-cgi/image/width%3D1200%2Cheight%3D1200%2Cfit%3Dscale-down%2Cformat%3Dauto%2Cquality%3D60/jzxhsemt/b3007001-9f02-4dc0-ae80-e8a39c839e29.jpg'),
((select Id_Product from Products where Name_Product =  'Ciroc Vodka') , 'https://www.liquor.com/thmb/w759GoXWuPc-F5n2i6bB2fRdQgU=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/__opt__aboutcom__coeus__resources__content_migration__liquor__2014__01__brand-page-ciroc-1-4fc2a98d1bec419490f25559254a0bb2.jpg'),

((select Id_Product from Products where Name_Product =  'Jack Daniels' ) , 'https://www.oaks.delivery/wp-content/uploads/Jack-Daniels-Honey-Whiskey-1-1600x900-1-1200x900-cropped.webp'),
((select Id_Product from Products where Name_Product =  'Jack Daniels') , 'https://m.media-amazon.com/images/I/817bm26JPnL.jpg'),
((select Id_Product from Products where Name_Product =  'Jack Daniels') , 'https://www.weedram.co.uk/app/uploads/2020/09/IMG_6444-scaled-e1713568940272.jpg'),

((select Id_Product from Products where Name_Product = 'Bombay Gin') , 'https://oldwine.it/2630-large_default/london-dry-gin-bombay-sapphire-con-1-bicchiere-omaggio.jpg'), 
((select Id_Product from Products where Name_Product = 'Bombay Gin') , 'https://bodegaslacatedral.com/cdn/shop/files/Bombay_sapphire_ginebra_-70cl_bodegas_la_catedral.jpg?crop=center&height=1200&v=1704729645&width=1200'),
((select Id_Product from Products where Name_Product = 'Bombay Gin ') , 'https://www.enotecavinoinanfora.it/wp-content/uploads/2019/04/DSC_0181_2-e1556297663698.jpg'),

((select Id_Product from Products where Name_Product = 'Malfy Gin') , 'https://www.acchiari.it/10374-large_default/malfy-gin-original-70-cl.jpg'),
((select Id_Product from Products where Name_Product = 'Malfy Gin') , 'https://cdn.mafrservices.com/sys-master-root/hef/h40/49842432475166/166776_3.jpg?im=Resize=480'),
((select Id_Product from Products where Name_Product = 'Malfy Gin') , 'https://resources.vino.com/data/offertaFileFile/offertaFileFile-230510.jpg');