Use Gestão_Eventos
GO

---------------Triggers------------------

--Trigger para avaliar se há bilhetes disponívies
Drop Trigger If exists ValidateTicketAvailability
GO

CREATE TRIGGER ValidateTicketAvailability
ON dbo.Sales
After INSERT,Update
AS
        DECLARE @EventID INT;
        DECLARE @TotalTicketsSoldVip INT;
        DECLARE @TotalTicketsSoldPublic INT;
        DECLARE @MaxTicketsVip INT;
        DECLARE @MaxTicketsPublic INT;
		Declare @insertedTickets int;
		Declare @inserted_Ttype Varchar(15);
        
        SELECT @EventID = Event_ID
        FROM inserted;
        
        SELECT @TotalTicketsSoldVip = ISNULL(SUM(N_tickets), 0)
        FROM Sales
        WHERE Event_ID = @EventID AND T_type = 'VIP';

        SELECT @TotalTicketsSoldPublic = ISNULL(SUM(N_tickets), 0)
        FROM Sales
        WHERE Event_ID = @EventID AND T_type = 'Normal';

        SELECT @MaxTicketsVip = ISNULL(N_tickets_vip,0)
        FROM Event
        WHERE Event_ID = @EventID;

        SELECT @MaxTicketsPublic = ISNULL(N_tickets_public,0)
        FROM Event
        WHERE Event_ID = @EventID;

		Select @insertedTickets = N_tickets
		From inserted
		
		Select @inserted_Ttype = T_type
		From inserted

        IF @inserted_Ttype = 'Normal' and @TotalTicketsSoldPublic + @insertedTickets > @MaxTicketsPublic  
		BEGIN
            RAISERROR ('Number of tickets exceeds available tickets.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
		END
		ELSE IF @inserted_Ttype = 'VIP' and @TotalTicketsSoldVip + @insertedTickets > @MaxTicketsVip
		BEGIN
            RAISERROR ('Number of tickets exceeds available tickets.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
		END
GO


Drop Trigger If exists PreventDuplicateLocationName
go

CREATE TRIGGER PreventDuplicateLocationName
ON Location
AFTER INSERT, UPDATE
AS
BEGIN
    DECLARE @duplicates INT;
    
    SELECT @duplicates = COUNT(*)
    FROM Location AS L
    WHERE L.L_Name IN (
        SELECT I.L_Name
        FROM inserted AS I
    )
    GROUP BY L.L_Name
    HAVING COUNT(*) > 1;

    IF @duplicates > 0
    BEGIN
        RAISERROR ('Invalid operation. Duplicated location name.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;
END;
GO