DROP SCHEMA IF Exists Gestao_Stocks;
GO
CREATE SCHEMA Gestao_Stocks;
GO

CREATE TABLE tipo_fornecedor(
	codigo   INT,
	designacao  VARCHAR(30)
	PRIMARY KEY(codigo));

CREATE TABLE fornecedor(
	nif                  INT            NOT NULL,
	nome                 VARCHAR(30)    NOT NULL,
	fax                INT,
	endereco            VARCHAR(30)  ,
	condpag				INT,
	tipo			INT  ,
	PRIMARY KEY(nif));

CREATE TABLE produto(
	codigo       INT           NOT NULL,
	nome         VARCHAR(30)   NOT NULL,
	preco        MONEY         NOT NULL,
	iva     INT,
	unidades	INT          NOT NULL,
	PRIMARY KEY(Codigo));



CREATE TABLE encomenda(
	numero     INT      NOT NULL,
	data         DATE     NOT NULL,
	fornecedor          INT      NOT NULL,
	PRIMARY KEY(numero),
	FOREIGN KEY(fornecedor) REFERENCES fornecedor(Nif));


CREATE TABLE item(
	numEnc       INT,
	codProd      INT   NOT NULL,
	unidades     INT,
	FOREIGN KEY(numEnc) REFERENCES encomenda(numero));

