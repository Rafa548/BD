Drop TABLE IF EXISTS dept_locations, dependent, works_on, project, employee, department;

--Create DataBase Company;
--Go





Use company
GO

Drop schema if exists c1;
Go


Create Schema c1;
Go



CREATE TABLE department (
    Dname VARCHAR(50) NOT NULL,
    Dnumber INT NOT NULL,
    Mgr_ssn INT ,
    Mgr_start_date DATE,
    PRIMARY KEY (Dnumber)
);

CREATE TABLE employee (
    Fname VARCHAR(15) NOT NULL,
    Minit CHAR(1) NOT NULL,
    Lname VARCHAR(15) NOT NULL,
    Ssn INT NOT NULL,
    Bdate DATE NOT NULL,
    Adress VARCHAR(30) NOT NULL,
    Sex  CHAR(1) NOT NULL,
    Salary INT NOT NULL,
    Super_ssn INT ,
    Dno INT NOT NULL,
    PRIMARY KEY (Ssn),
    FOREIGN KEY (Dno) REFERENCES department(Dnumber)
);

CREATE TABLE project (
    Pname VARCHAR(15) NOT NULL,
    Pnumber INT NOT NULL,
    Plocation VARCHAR(15) NOT NULL,
    Dnum INT NOT NULL,
    PRIMARY KEY (Pnumber),
    FOREIGN KEY (Dnum) REFERENCES department(Dnumber)
);

CREATE TABLE works_on (
    Essn INT NOT NULL,
    Pno INT NOT NULL,
    Hours INT NOT NULL,
    PRIMARY KEY (Essn, Pno),
    FOREIGN KEY (Essn) REFERENCES employee(Ssn),
    FOREIGN KEY (Pno) REFERENCES project(Pnumber)
);

CREATE TABLE dependent (
    Essn INT NOT NULL,
    Dependent_name VARCHAR(30) NOT NULL,
    Sex CHAR(1) NOT NULL,
    Bdate DATE NOT NULL,
    Relationship VARCHAR(15) NOT NULL,
    PRIMARY KEY (Essn, Dependent_name),
    FOREIGN KEY (Essn) REFERENCES employee(Ssn)
);

Create table dept_locations (
    Dnumber INT NOT NULL,
    Dlocation VARCHAR(15) NOT NULL,
    PRIMARY KEY (Dnumber, Dlocation),
    FOREIGN KEY (Dnumber) REFERENCES department(Dnumber)
);
