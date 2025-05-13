--Creates a database
use master;

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'EventEaseDB')
    DROP DATABASE EventEaseDB;

create database EventEaseDB;
use EventEaseDB;



--creates a Venue table in the database
Create table Venue(
Venue_ID int identity(1,1) primary key not null,
VenueName nvarchar(250) not null,
Location nvarchar(250) not null,
Capacity int not null CHECK (Capacity > 0),
ImageURL nvarchar(500) 
);

Select * from Venue;

--creates a Event table in the database
create table Event(
Event_ID int identity(1,1) primary key not null,
Venue_ID int null,
EventName varchar(250) not null,
EventDate DATETIME NOT NULL,
Description nvarchar(250) null,
ImageURL nvarchar(500) 
FOREIGN KEY (Venue_ID) REFERENCES Venue(Venue_ID) ON DELETE SET NULL
);

select * from Event;



--creates a Booking table referances the event and venue tables
create table Booking(
Booking_ID int identity(1,1) primary key not null,
Event_ID int not null,
Venue_ID int not null,
BookingDate DATETIME DEFAULT GETDATE(),
FOREIGN KEY (Event_ID) REFERENCES Event(Event_ID) ON DELETE CASCADE,
FOREIGN KEY (Venue_ID) REFERENCES Venue(Venue_ID) ON DELETE CASCADE,
CONSTRAINT UQ_Venue_Event UNIQUE (Venue_ID, Event_ID) -- Prevents double booking of the same venue for the same event
);

-- Ensure no two bookings overlap for the same venue
CREATE UNIQUE INDEX UQ_Venue_Booking ON Booking (Venue_ID, BookingDate);

select * from Booking;

insert into Venue (VenueName, Location, Capacity, ImageURL) values
('The cooking Studio', 'Olivedale, Johannesburg','100', 'https://eventeasecompstorage.blob.core.windows.net/eventeasecontainer/Cooking%20studio.jpg'),
('Emperors palace', 'Johanessburg, South Africa','100', 'https://eventeasecompstorage.blob.core.windows.net/eventeasecontainer/emperors%20palace.jpg'),
('Johannesburg Expo Centre', 'Johanessburg, South Africa','100', 'https://eventeasecompstorage.blob.core.windows.net/eventeasecontainer/Johannesburg%20Expo%20Centre.jpg'),
('Sandwich Habour','Walvis Bay, Namibia','200','https://eventeasecompstorage.blob.core.windows.net/eventeasecontainer/sandwich%20harbour.jpg');

insert into Event (Venue_ID, EventName, EventDate, Description, ImageURL) values
(1,'Top Baker','2025-07-05 10:00:00','Welcome to the baking competiton','https://eventeasecompstorage.blob.core.windows.net/eventeasecontainer/Baking%20Competition.jpg'),
(2,'Lights Show','2025-12-05 09:00:00','Why not enjoy this festive season with a fantastic light show','https://eventeasecompstorage.blob.core.windows.net/eventeasecontainer/Light%20show.jpg'),
(3,'Comic Con Africa','2025-08-28 09:00:00','Celebrate with your friends and family in a weekend that you will never forget','https://eventeasecompstorage.blob.core.windows.net/eventeasecontainer/comic%20con%20Africa.jpg'),
(3,'Hot Wheels', '2025-08-12 11:00:00' , 'Sand Dune Bike Racing Competition', 'https://eventeasecompstorage.blob.core.windows.net/eventeasecontainer/Quad-bike%20competition.jpg');

insert into Booking (Event_ID, Venue_ID,BookingDate) 
values(1,1,'2025-07-05 10:00:00'),
	  (2,2,'2025-12-05 18:00:00'),
	  (3,3,'2025-08-28 09:00:00'),
      (4,4,'2025-08-12 11:00:00');


