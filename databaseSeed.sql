CREATE DATABASE Imenik;

-- Create a table for Person
CREATE TABLE Person (
    id INT NOT NULL PRIMARY KEY IDENTITY,
    Name VARCHAR(50),
    Surname VARCHAR(50),
    Phone_number VARCHAR(20),
    Gender VARCHAR(10),
    Email VARCHAR(50),
    City INT REFERENCES Cities(CityId),
    Country INT REFERENCES Countries(CountryId),
    DOB VARCHAR(50),
    Age INT
);

-- Create a table for countries
CREATE TABLE Countries (
    CountryId INT PRIMARY KEY,
    CountryName VARCHAR(100) NOT NULL
);

-- Create a table for cities
CREATE TABLE Cities (
    CityId INT PRIMARY KEY,
    CityName VARCHAR(100) NOT NULL,
    CountryId INT REFERENCES Countries(CountryId)
);

-- Insert data into the Countries table
INSERT INTO Countries (CountryId, CountryName)
VALUES
    (1, 'Germany'),
    (2, 'France'),
    (3, 'Italy'),
    (4, 'Spain'),
    (5, 'United Kingdom'),
    (6, 'Netherlands'),
    (7, 'Belgium'),
    (8, 'Switzerland'),
    (9, 'Sweden'),
    (10, 'Norway');


-- Insert data into the Cities table
INSERT INTO Cities (CityId, CityName, CountryId)
VALUES
    (1, 'Berlin', 1),
    (2, 'Paris', 2),
    (3, 'Rome', 3),
    (4, 'Madrid', 4),
    (5, 'London', 5),
    (6, 'Amsterdam', 6),
    (7, 'Brussels', 7),
    (8, 'Zurich', 8),
    (9, 'Stockholm', 9),
    (10, 'Oslo', 10);

INSERT INTO Person (Name, Surname, Phone_number, Gender, Email, City, Country, DOB, Age)
VALUES
    ('Kerim', 'Muharembegovic', '061/123-123', 'Musko', 'kerim@test.com', 1, 1, '16.02.2000', 23)
