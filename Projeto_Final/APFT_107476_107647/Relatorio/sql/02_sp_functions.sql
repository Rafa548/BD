Use Gestão_Eventos
GO

-----------------------Stored Procedures------------------------

----------------------Event Actions ---------------------------------

Drop proc if exists dbo.UpdateEvent ;
GO

CREATE PROC dbo.UpdateEvent (
    @EventID INT,
    @EventName VARCHAR(30),
    @N_tickets_vip INT,
    @N_tickets_public INT,
    @Price_vip INT,
    @Price_public INT,
	@Location_ID Int
)
AS
BEGIN
    UPDATE Event
    SET EventName = @EventName,
        N_tickets_vip = @N_tickets_vip,
        N_tickets_public = @N_tickets_public,
        Price_vip = @Price_vip,
		EventLocation_ID = @Location_ID,
        Price_public = @Price_public
    FROM Event
    INNER JOIN Location ON Event.EventLocation_ID = Location.Location_ID
    WHERE Event_ID = @EventID;

END;
GO

DROP PROC dbo.DeleteEvent
GO

CREATE PROCEDURE DeleteEvent @EventID INT
AS
BEGIN
	DELETE FROM Ad WHERE Event_ID= @EventID;
    DELETE FROM Sales WHERE Event_ID = @EventID;
    DELETE FROM Sponsor WHERE Event_ID = @EventID;
    DELETE FROM Organizer WHERE Event_ID = @EventID;
    DELETE FROM Supplier WHERE Event_ID = @EventID;
    DELETE FROM Schedule WHERE Event_ID = @EventID;
    DELETE FROM Activity WHERE Activity_ID IN (SELECT Activity_ID FROM Schedule WHERE Event_ID = @EventID);
    DELETE FROM Event WHERE Event_ID = @EventID;
END;
GO

-------------------------------------------------

-------------------Sale Actions------------------

DROP PROC dbo.DeleteSale
GO

CREATE PROCEDURE DeleteSale 
	@Sale_ID INT,
	@E_ID INT
AS
BEGIN
	DELETE FROM Sales WHERE Sale_ID=@Sale_ID and Event_ID=@E_ID;
END;
GO


Drop proc if exists dbo.UpdateSales ;
GO

CREATE PROCEDURE dbo.UpdateSales
    @Sale_ID INT,
    @Event_ID INT,
    @N_tickets INT,
    @Ssn INT,
    @T_type VARCHAR(15)
	AS
	BEGIN
		IF EXISTS (SELECT 1 FROM Sales WHERE Sale_ID = @Sale_ID)
		BEGIN
			UPDATE Sales
			SET 
				N_tickets = @N_tickets,
				Ssn = @Ssn,
				T_type = @T_type
			WHERE Sale_ID = @Sale_ID and Event_ID = @Event_ID;
		END
		ELSE
		BEGIN
			RAISERROR ('The specified sale ID does not exist.', 16, 1);
			RETURN;
		END
	END;
go

---------------------------------------------------

-------------SCHEDULE Actions----------------------

DROP PROC dbo.DeleteSchedule
GO

CREATE PROCEDURE DeleteSchedule 
	@Schedule_ID INT,
	@E_ID INT
AS
BEGIN
	DELETE FROM Schedule WHERE Schedule_ID=@Schedule_ID and Event_ID=@E_ID;
END;
GO


Drop proc if exists dbo.UpdateSchedule ;
GO

CREATE PROC dbo.UpdateSchedule(
    @Event_ID INT,
    @Day INT,
    @Activity_ID INT,
    @Hour INT,
    @Schedule_ID INT
)
AS
BEGIN
    UPDATE Schedule
    SET Day = @Day,
        Activity_ID = @Activity_ID,
        Hour = @Hour
        FROM Schedule
        WHERE Event_ID = @Event_ID AND Schedule_ID = @Schedule_ID
END;
GO

------------------------------------------------------

---------------------------Location Actions -----------------------------

DROP PROC if exists dbo.EliminateLocal
GO

CREATE PROCEDURE dbo.EliminateLocal @LocationID INT
AS
	DELETE FROM dbo.Sponsor WHERE Event_ID IN (SELECT Event_ID FROM dbo.Event WHERE EventLocation_ID = @LocationID)
	DELETE FROM dbo.Organizer WHERE Event_ID IN (SELECT Event_ID FROM dbo.Event WHERE EventLocation_ID = @LocationID)
	DELETE FROM dbo.Supplier WHERE Event_ID IN (SELECT Event_ID FROM dbo.Event WHERE EventLocation_ID = @LocationID)
	DELETE FROM dbo.Schedule WHERE Event_ID IN (SELECT Event_ID FROM dbo.Event WHERE EventLocation_ID = @LocationID)
	DELETE FROM dbo.Ad WHERE Event_ID IN (SELECT Event_ID FROM dbo.Event WHERE EventLocation_ID = @LocationID)
	DELETE FROM dbo.Sales WHERE Event_ID IN (SELECT Event_ID FROM dbo.Event WHERE EventLocation_ID = @LocationID)
	DELETE FROM dbo.Event WHERE  EventLocation_ID = @LocationID
	DELETE FROM dbo.Location WHERE Location_ID = @LocationID
GO


Drop proc if exists dbo.UpdateLocation ;
GO

CREATE PROC dbo.UpdateLocation (
	@Location_ID INT,
	@L_Name VARCHAR(30),
	@L_Address VARCHAR(30),
	@L_City VARCHAR(30)
)
AS
BEGIN
    UPDATE Location
    SET 
        L_Name = @L_Name,
        L_Address = @L_Address,
        L_City = @L_City
    FROM Location
	Where Location_ID = @Location_ID
END;
GO

-------------------------------------------------------------

-------------AD Actions-----------------------

DROP PROC if exists dbo.DeleteAd
GO

CREATE PROCEDURE dbo.DeleteAd 
	@Ad_ID INT,
	@Event_ID INT
AS
BEGIN
	DELETE FROM Ad WHERE Ad_ID=@Ad_ID and Event_ID=@Event_ID;
END;
GO

Drop proc if exists dbo.UpdateAd ;
GO

CREATE PROC dbo.UpdateAd (
	@Duration INT,
	@Ad_ID INT,
	@Cost MONEY,
	@Event_ID INT
)
AS
BEGIN
    UPDATE Ad
    SET Duration = @Duration,
        Cost = @Cost
    FROM Ad
	WHERE Ad_ID = @Ad_ID AND Event_ID = @Event_ID
END;
GO

--------------------------------------------------------

-----------------Client Actions --------------------------

DROP PROC if exists dbo.DeleteClient
GO

CREATE PROCEDURE DeleteClient @Ssn INT
AS
BEGIN
	DELETE FROM Sales WHERE Ssn IN (SELECT Ssn FROM dbo.Client WHERE Ssn = @Ssn)
	DELETE FROM Client WHERE Ssn = @Ssn
END;
GO

Drop proc if exists dbo.UpdateClients ;
GO

CREATE PROC dbo.UpdateClients(
	@Ssn INT,
	@C_Name VARCHAR(30),
	@Age INT
)
AS
BEGIN
    BEGIN
		UPDATE Client
		SET C_Name = @C_Name,
			Age = @Age
		WHERE Ssn = @Ssn
	END
END;
GO
--------------------------------------------------------

--------------------Sales 2 Action -------------------------
Drop proc if exists dbo.UpdateSalesC;
GO

CREATE PROCEDURE dbo.UpdateSalesC
    @Sale_ID INT,
    @Event_ID INT,
    @N_tickets INT,
    @T_type VARCHAR(15),
	@Ssn INT
    AS
    BEGIN
        IF EXISTS (SELECT 1 FROM Sales WHERE Sale_ID = @Sale_ID)
        BEGIN
            UPDATE Sales
            SET 
                N_tickets = @N_tickets,
                T_type = @T_type,
				Event_ID = @Event_ID
            WHERE Sale_ID = @Sale_ID AND Ssn = @Ssn;
        END
        ELSE
        BEGIN
            RAISERROR ('The specified sale ID does not exist.', 16, 1);
            RETURN;
        END
    END;
GO

-----------------------------------------------------------

-------------------------Event 2 Actions---------------------

DROP PROC IF EXISTS dbo.UpdateEventL;
GO

CREATE PROC dbo.UpdateEventL (
    @EventID INT,
    @EventName VARCHAR(30),
    @N_tickets_vip INT,
    @N_tickets_public INT,
    @Location_ID Int
)
AS
BEGIN
    UPDATE Event
    SET EventName = @EventName,
        N_tickets_vip = @N_tickets_vip,
        N_tickets_public = @N_tickets_public,
        EventLocation_ID = @Location_ID
    WHERE Event_ID = @EventID;

END;
GO

-------------------------------------------------------------
-----------------------User Defined Function------------------------

--Obter profit total de um evento

DROP FUNCTION IF EXISTS dbo.CalculateProfit;
GO

CREATE FUNCTION dbo.CalculateProfit(@EventID INT)
RETURNS MONEY
AS
BEGIN
	DECLARE @TotalRevenue MONEY;
    DECLARE @TotalCost MONEY;
    DECLARE @Profit MONEY;

    SELECT @TotalRevenue = COALESCE(SUM(CASE
                                    WHEN Sales.T_type = 'VIP' THEN Sales.N_tickets * Event.Price_vip
                                    ELSE Sales.N_tickets * Event.Price_public
                                END), 0)
    FROM Sales
    JOIN Event ON Sales.Event_ID = Event.Event_ID
    WHERE Event.Event_ID = @EventID;

    SELECT @TotalCost = COALESCE(SUM(Sponsorship_amount), 0)
    FROM Sponsor
    WHERE Sponsor.Event_ID = @EventID;

    SELECT @TotalCost = @TotalCost + COALESCE(SUM(Cost), 0)
    FROM Ad
    WHERE Ad.Event_ID = @EventID;

    SELECT @TotalCost = @TotalCost + COALESCE(SUM(Cost), 0)
    FROM Organizer
    WHERE Organizer.Event_ID = @EventID;

    SELECT @TotalCost = @TotalCost + COALESCE(SUM(Cost), 0)
    FROM Supplier
    WHERE Supplier.Event_ID = @EventID;

    SET @Profit = @TotalRevenue - @TotalCost;
    RETURN @Profit;
END;
GO

DECLARE @Profit MONEY;
SET @Profit = dbo.CalculateProfit(4);



------------------------Indices -----------------------------

--Util ao pesquisar por um nome de uma localização em especifico isto é usado no trigger e na pesquisa nos forms--
DROP INDEX IF EXISTS IX_Location_LName ON Location;

CREATE INDEX IX_Location_LName ON Location (L_Name);

--Util para qualquer query q use locations e eventos ao mesmo tempo
DROP INDEX IF EXISTS IX_Event_EventLocationID ON Event;

CREATE INDEX IX_Event_EventLocationID ON Event (EventLocation_ID);

--Util para qualquer query que use schdules com atividades para um evento em especifico--
DROP INDEX IF EXISTS IX_Schedule_EventID_ActivityID ON Schedule;

CREATE INDEX IX_Schedule_EventID_ActivityID ON Schedule (Event_ID, Activity_ID);


---------------------------------------------------------------
