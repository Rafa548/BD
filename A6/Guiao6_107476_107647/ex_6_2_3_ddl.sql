DROP TABLE IF EXISTS presc_farmaco,Farmaco, Prescricao, Farmaceutica, Farmacia, Paciente, Medico;

DROP SCHEMA IF EXISTS Prescricao_Med;
GO

CREATE SCHEMA Prescricao_Med;
GO


CREATE TABLE Medico(
    numSns              INT             NOT NULL,
    nome                VARCHAR(30)     NOT NULL,
    especialidade       VARCHAR(30)     NOT NULL,
    PRIMARY KEY (numSns)	
);

CREATE TABLE Paciente(
    numUtente               INT             NOT NULL,
    nome                    VARCHAR(30)     NOT NULL,
    dataNasc                DATE            NOT NULL,
    endereco                VARCHAR(30)      NOT NULL,
    PRIMARY KEY (numUtente)
);

CREATE TABLE Farmacia(
    nome                    VARCHAR(30)     NOT NULL,
    telefone                VARCHAR(9)      NOT NULL,
    endereco                VARCHAR(30)     NOT NULL,
    PRIMARY KEY (nome)
);

Create TABLE Farmaceutica(
    numReg                  INT             NOT NULL,
    nome                    VARCHAR(30)     NOT NULL,
    Endereco                VARCHAR(50)     NOT NULL,
    PRIMARY KEY (numReg)
);

CREATE TABLE Prescricao(
    numPresc               INT             NOT NULL,
    numUtente              INT             NOT NULL,
    numMedico              INT             NOT NULL,
    farmacia               VARCHAR(30),
    dataProc               DATE,
    PRIMARY KEY (numPresc),
    FOREIGN KEY (numUtente) REFERENCES Paciente(numUtente),
    FOREIGN KEY (numMedico) REFERENCES Medico(numSns),
    FOREIGN KEY (farmacia) REFERENCES Farmacia(nome)
);


CREATE TABLE Farmaco(
    numRegFarm             INT             NOT NULL,
    nome                   VARCHAR(30)     NOT NULL,
    formula                VARCHAR(30)     NOT NULL,
    PRIMARY KEY (numRegFarm, nome),
    FOREIGN KEY (numRegFarm) REFERENCES Farmaceutica(numReg)
);


CREATE TABLE presc_farmaco (
    numPresc              INT             NOT NULL,
    numRegFarm            INT             NOT NULL,
    nomeFarmaco           VARCHAR(30)     NOT NULL,
    PRIMARY KEY (numPresc, numRegFarm, nomeFarmaco),
    FOREIGN KEY (numPresc) REFERENCES Prescricao(numPresc),
    FOREIGN KEY (numRegFarm, nomeFarmaco) REFERENCES Farmaco(numRegFarm, nome)  
)
