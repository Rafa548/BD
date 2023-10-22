USE Gestão_Eventos;
GO

INSERT INTO dbo.Location (Location_ID, L_Name, L_Address, L_City) VALUES
    (1, 'Parque de Feiras e Exposições', 'Av. Dom Manuel', 'Aveiro'),
    (2, 'FIL', 'Rua do Bojador', 'Lisboa'),
    (3, 'Exponor', 'Rua Trindade', 'Porto'),
    (4, 'Altice Arena', 'Rossio dos Olivais', 'Lisboa'),
    (5, 'Centro de Congressos', 'Avenida Central', 'Coimbra'),
    (6, 'Pavilhão Multiusos', 'Rua das Flores', 'Braga'),
    (7, 'Estádio Municipal', 'Avenida dos Desportos', 'Porto'),
    (8, 'Centro de Convenções', 'Rua das Flores', 'Braga'),
    (9, 'Centro de Eventos', 'Avenida Central', 'Coimbra'),
    (10, 'Centro Cultural', 'Praça da Liberdade', 'Porto'),
    (11, 'Centro de Exposições', 'Rua dos Artistas', 'Lisboa'),
    (12, 'Centro de Convenções 2', 'Avenida das Flores', 'Porto'),
    (13, 'Arena Music Hall', 'Rua dos Cantores', 'Braga'),
    (14, 'Pavilhão Desportivo', 'Avenida dos Campeões', 'Coimbra'),
    (15, 'Centro Cultural 2', 'Praça da Cultura', 'Aveiro'),
    (17, 'Expo Center', 'Avenida da Expo', 'Lisboa'),
    (16, 'Centro de Congressos 3', 'Rua das Artes', 'Lisboa');



INSERT INTO dbo.Event VALUES
	(1,'Feira de Março',NULL,10001,NULL,3,1),
	(2,'IberAnime-Lisboa',1000,10000,60,30,2),
	(3,'IberAnime-Porto',1000,10000,60,30,3),
	(4,'Lisbon Games Week',NULL,1000,150,70,2),
	(5,'Web Summit',NULL,1000,260,100,2),
	(6,'SUPERBOCK SUPER ROCK',NULL,15000,NULL,60,4),
	(7,'Festival de Verão', NULL, 5000, NULL, 40, 5),
    (8,'Feira de Artesanato', NULL, 8000, NULL, 20, 6),
	(9,'Feira de Verão', NULL, 5000, 40, 5, 6),
    (10,'Conferência de Tecnologia', NULL, 2000, 120, 50, 7),
    (11,'Exposição de Arte Moderna', NULL, 1000, 80, 30, 8),
	(12,'Conferência de Tecnologia 2', NULL, 2000, 120, 50, 17),
	(13,'Exposição de Arte Moderna 2', NULL, 1000, 80, 30, 10);


INSERT INTO dbo.Company VALUES
	('Altice',NULL,'Rua D.João',1),
	('Redbull',NULL,'Rua D.Dinis',2),
	('2045',1000,'Rua das Flores',3),
	('Monster',NULL,'Rua do Barreiro',4),
	('Super Bock Company',Null,'Rua de S.António',5),
	('Coca-Cola', NULL, 'Rua das Colinas', 6),
    ('PepsiCo', NULL, 'Avenida das Fontes', 7),
    ('Nike', NULL, 'Rua dos Desportistas', 8),
	('Google', NULL, 'Rua dos Programadores', 9),
    ('Microsoft', NULL, 'Avenida da Inovação', 10),
    ('Apple', NULL, 'Rua da Maçã', 11),
	('Amazon', NULL, 'Avenida da Inovação', 12),
	('Netflix', NULL, 'Rua das Séries', 13),
	('Tesla', NULL, 'Electric Avenue', 14),
	('Facebook', NULL, 'Social Media Street', 15),
	('Samsung', NULL, 'Tech Boulevard', 16),
	('Toyota', NULL, 'Automobile Road', 17),
	('Adobe', NULL, 'Creative Drive', 18);

INSERT INTO dbo.Sponsor VALUES
	(2,4,500000),
	(4,5,600000),
	(5,6,550000),
	(2,2,300000),
	(6, 7,400000),
    (7, 8,300000),
	(8, 4, 800000),
    (10, 7, 1000000),
    (11, 6, 900000),
	(17, 12, 1000000),
	(16, 13, 900000),
	(18, 9, 800000);


INSERT INTO dbo.Organizer VALUES
	(2,1,500000),
	(3,1,600000),
	(4,12,800000),
	(6,5,500000),
	(7,3,500000),
	(8,2,600000),
	(9,3,500000),
	(10,13,600000),
    (11,2,700000),
	(12,1,600000),
	(13,7,700000),
	(14,10,500000);

INSERT INTO dbo.Supplier VALUES
	(2,1,400000,'Bebidas'),
	(4,5,600000,'Bebidas'),
	(3,2,10000,'Segurança'),
	(3,3,10000,'Segurança'),
	(3,4,10000,'Segurança'),
	(3,5,10000,'Segurança'),
	(5,7,200000,'Bebidas'),
    (6,8,100000,'Decoração'),
    (7,8,50000,'Segurança'),
	(8, 6, 400000, 'Decoração'),
    (10, 9, 50000, 'Segurança'),
    (11, 7, 300000, 'Bebidas'),
	(12, 9, 50000, 'Segurança'),
	(13, 7, 300000, 'Bebidas'),
	(14, 11, 200000, 'Tecnologia');


INSERT INTO dbo.Client VALUES
	(123456789, 'João da Silva', 30),
	(987654321, 'Maria Oliveira', 25),
	(456789123, 'Antônio Santos', 40),
	(111222333, 'Pedro Rocha', 35),
	(444555666, 'Sara Pereira', 28),
	(222333444, 'Ana Costa', 32),
	(777888999, 'Paulo Almeida', 50),
	(555666777, 'Lúcia Cardoso', 42),
	(999888777, 'Ricardo Souza', 37),
	(666777888, 'Mariana Fernandes', 29),
	(123123123, 'Carlos Pereira', 35),
    (456456456, 'Marta Costa', 27),
    (789789789, 'Rui Sousa', 42),
    (321321321, 'Inês Almeida', 30),
    (654654654, 'Ricardo Silva', 31),
    (987987987, 'Sofia Santos', 29),
	(159753852, 'Rita Pereira', 26),
    (753951852, 'Hugo Santos', 31),
    (852963741, 'Mariana Silva', 28),
	(123123523, 'Carlos Pereira', 35),
	(456756456, 'Marta Costa', 27),
	(789779789, 'Rui Sousa', 42),
	(321381321, 'Inês Sousa', 30),
	(654614654, 'Ricardo Gomes', 31),
	(987937987, 'Sofia Oliveira', 29);

INSERT INTO dbo.Sales VALUES
	(1, 1, 2, 123456789, 'Normal'),
	(2, 2, 1, 987654321, 'Normal'),
	(3, 2, 1, 666777888, 'VIP'),
	(4, 3, 4, 456789123, 'Normal'),
	(5, 1, 3, 111222333, 'VIP'),
	(6, 2, 2, 444555666, 'Normal'),
	(7, 4, 3, 123123123, 'Normal'),
    (8, 8, 5, 456456456, 'VIP'),
    (9, 7, 2, 789789789, 'Normal'),
    (10, 8, 1, 321321321, 'Normal'),
	(10, 1, 2, 123456789, 'Normal'),
	(11, 9, 2, 159753852, 'Normal'),
    (12, 10, 1, 753951852, 'VIP'),
    (13, 11, 3, 852963741, 'Normal'),
	(1, 10, 1, 753951852, 'VIP'),
	(10, 11, 3, 852963741, 'Normal'),
	(14, 12, 2, 987987987, 'VIP');


INSERT INTO dbo.Ad VALUES
	(1,40,1,5000),
	(2,30,2,7000),
	(3,30,3,7000),
	(4,70,4,10000),
	(4,30,5,3000),
	(5, 40, 6, 8000),
    (7, 30, 7, 5000),
    (2, 60, 8, 10000),
    (3, 30, 9, 3000),
    (1, 45, 10, 6000),
	(5, 50, 11, 12000),
    (6, 30, 12, 8000),
    (7, 40, 13, 10000),
	(8, 60, 8, 10000),
	(9, 30, 9, 3000),
	(10, 45, 10, 6000);

INSERT INTO dbo.Activity VALUES
	('Slow-J',3000,1,'Música'),
	('Fernando Rocha',2000,2,'Comédia'),
	('Xutos e Pontapés',3000,3,'Música'),
	('Black Midi',5000,4,'Música'),
	('Kanye West',100000,5,'Música'),
	('Ricky Gervais',2000,6,'Comédia'),
	('Gilmário Mbemba',1000,7,'Comédia'),
	('Tricotar',100,8,'Cívico'),
	('Pensamento criativo',200,9,'Cívico'),
	('Falar em Público',500,10,'Cívico'),
	('Marketing digital',600,11,'Cívico'),
	('The Weeknd',5000,12,'Música'),
	('Exposição de Arte', 1000, 13, 'Cultural'),
    ('Palestra sobre Sustentabilidade', 500, 14, 'Educativo'),
    ('Concurso de Talentos', 2000, 15, 'Entretenimento'),
	('Dança Contemporânea', 800, 16, 'Arte'),
    ('Workshop de Fotografia', 400, 17, 'Educativo'),
    ('Seminário de Marketing', 600, 18, 'Negócios'),
	('The Weeknd', 5000, 20, 'Música'),
	('Exposição de Arte', 1000, 21, 'Cultural'),
	('Palestra sobre Sustentabilidade', 500, 19, 'Educativo');


INSERT INTO dbo.Schedule VALUES
	(1, 1, 1, 9, 1),
    (2, 1, 2, 10, 2),
    (1, 2, 3, 14, 3),
    (3, 1, 4, 11, 4),
    (2, 2, 1, 16, 5),
    (3, 2, 3, 13, 6),
    (1, 3, 2, 12, 7),
    (2, 3, 3, 15, 8),
    (3, 3, 1, 17, 9),
	(4, 1, 13, 10, 10),
    (5, 1, 14, 11, 11),
    (6, 1, 15, 12, 12),
    (7, 2, 13, 14, 13),
    (8, 2, 14, 15, 14),
    (6, 2, 15, 16, 15),
    (1, 3, 13, 18, 16),
    (7, 3, 14, 19, 17),
    (8, 3, 15, 20, 18),
	(4, 4, 16, 15, 19),
    (5, 5, 17, 16, 20),
    (6, 6, 18, 17, 21);
	

INSERT INTO dbo.Performer VALUES
	(1,1),
	(1,2),
	(4,3),
	(4,4),
	(1,5),
	(1,6),
	(2,7),
	(1,12);

INSERT INTO dbo.Workshop VALUES
	('Aprende a Tricotar',8),
	('Pensamento criativo e resolução de problemas',9),
	('Técnicas de apresentação e fala em público',10),
	('Estratégias de marketing digital',11),
	('Fotografia Digital', 13),
    ('Dança de Salão', 14),
    ('Artesanato em Madeira', 15),
	('Gastronomia Regional', 16),
    ('Artesanato em Cerâmica', 17),
    ('Introdução ao Yoga', 18);

