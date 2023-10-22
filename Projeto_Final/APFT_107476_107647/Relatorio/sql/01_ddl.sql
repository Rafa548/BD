DROP TABLE IF EXISTS Schedule, Workshop, Performer, Activity, Ad, Sales, Client, Supplier, Organizer, Sponsor, Company, Event, Location;

--DROP DATABASE  IF EXISTS Gestão_Eventos;
--GO

DROP SCHEMA IF EXISTS Gestão_Eventos;
GO

--CREATE DATABASE Gestão_Eventos;
--GO

CREATE SCHEMA Gestão_Eventos;
GO

USE Gestão_Eventos;
GO

CREATE TABLE Location (
    Location_ID INT  NOT NULL,
    L_Name VARCHAR(30) NOT NULL,
    L_Address VARCHAR(30) NOT NULL,
    L_City VARCHAR(15) NOT NULL,
    PRIMARY KEY (Location_ID)
);

CREATE TABLE Event (
    Event_ID INT NOT NULL,
    EventName VARCHAR(30) NOT NULL,
    N_tickets_vip INT ,
    N_tickets_public INT NOT NULL,
    Price_vip INT ,
    Price_public INT NOT NULL,
    EventLocation_ID INT NOT NULL,
    PRIMARY KEY (Event_ID),
    FOREIGN KEY (EventLocation_ID) REFERENCES Location(Location_ID)
);


CREATE TABLE Company (
    Name VARCHAR(30) NOT NULL,
    N_func INT,
    Address VARCHAR(30) NOT NULL,
    ID INT NOT NULL,
    PRIMARY KEY (ID)
)

CREATE TABLE Sponsor (
    SP_ID INT NOT NULL,
    Event_ID INT,
	Sponsorship_amount INT NOT NULL,
    PRIMARY KEY (SP_ID,Event_ID),
    FOREIGN KEY (SP_ID) REFERENCES Company(ID),
    FOREIGN KEY (Event_ID) REFERENCES Event(Event_ID)
)

CREATE TABLE Organizer (
	O_ID INT NOT NULL,
    Event_ID INT ,
    Cost INT NOT NULL,
    PRIMARY KEY (Event_ID,O_ID),
    FOREIGN KEY (O_ID) REFERENCES Company(ID),
    FOREIGN KEY (Event_ID) REFERENCES Event(Event_ID)
)

CREATE TABLE Supplier (
    S_ID INT NOT NULL,
    Event_ID INT ,
    Cost INT NOT NULL,
    Resource_S VARCHAR(15) NOT NULL,
    PRIMARY KEY (S_ID,Event_ID),
    FOREIGN KEY (S_ID) REFERENCES Company(ID),
    FOREIGN KEY (Event_ID) REFERENCES Event(Event_ID)
)

CREATE TABLE Client (
    Ssn INT NOT NULL,
    C_Name VARCHAR(30) NOT NULL,
    Age INT,
    PRIMARY KEY (Ssn)
);

CREATE TABLE Sales(
    Sale_ID INT NOT NULL,
    Event_ID INT NOT NULL,
    N_tickets INT NOT NULL,
    Ssn INT NOT NULL,
    T_type VARCHAR(15) , 
    PRIMARY KEY (Sale_ID,Ssn,Event_ID),
	FOREIGN KEY (Event_ID) REFERENCES Event(Event_ID),
    FOREIGN KEY (Ssn) REFERENCES Client(Ssn)
);

CREATE TABLE Ad (
	Event_ID  INT NOT NULL,
	Duration INT  NOT NULL,
	Ad_ID    INT NOT NULL,
	Cost   MONEY  NOT NULL,
	PRIMARY KEY(Event_ID,Ad_ID),
	FOREIGN KEY(Event_ID) REFERENCES Event(Event_ID),
);


CREATE TABLE Activity (
	Name  VARCHAR(40) NOT NULL,
	Cost    MONEY    ,
	Activity_ID   INT NOT NULL,
	Objective VARCHAR(30) NOT NULL,
	PRIMARY KEY(Activity_ID),
);

CREATE TABLE Schedule (
	Event_ID INT NOT NULL,
	Day   INT    NOT NULL,
	Activity_ID INT NOT NULL,
	Hour  INT  NOT NULL,
	Schedule_ID INT NOT NULL,
	PRIMARY KEY(Schedule_ID,Event_ID),
	FOREIGN KEY(Event_ID) REFERENCES Event(Event_ID),
	FOREIGN KEY(Activity_ID) REFERENCES Activity(Activity_ID),
);

CREATE TABLE Performer (
	N_elements   INT NOT NULL,
	Performer_ID  INT  NOT NULL,
	PRIMARY KEY(Performer_ID),
	FOREIGN KEY (Performer_ID) REFERENCES Activity (Activity_ID),
);

CREATE TABLE Workshop (
	Theme   VARCHAR(60)  NOT NULL,
	Workshop_ID   INT NOT NULL,
	PRIMARY KEY(Workshop_ID),
	FOREIGN KEY(Workshop_ID) REFERENCES Activity (Activity_ID)
);