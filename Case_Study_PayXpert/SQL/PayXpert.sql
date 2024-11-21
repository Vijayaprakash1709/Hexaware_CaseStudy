--create database
create database PayXpert

--Tables
-- Employee Table
create table Employee (
    EmployeeID int primary key identity(1,1),
    FirstName nvarchar(50) not null,
    LastName nvarchar(50) not null,
    DateOfBirth date not null,
    Gender nvarchar(10) not null,
    Email nvarchar(100) unique not null,
    PhoneNumber nvarchar(15) unique,
    [Address] nvarchar(255),
    Position nvarchar(50) not null,
    JoiningDate date not null,
    TerminationDate date)

-- Payroll Table
create table Payroll (
    PayrollID int primary key identity(1,1),
    EmployeeID int not null,
    PayPeriodStartDate date not null,
    PayPeriodEndDate date not null,
    BasicSalary decimal(18,2) not null,
    OvertimePay decimal(18,2) not null default 0,
    Deductions decimal(18,2) not null default 0,
    NetSalary as (BasicSalary + OvertimePay - Deductions) persisted, 
    foreign key (EmployeeID) references Employee(EmployeeID) on delete cascade)

-- Tax Table
create table Tax (
    TaxID int primary key identity(1,1),
    EmployeeID int not null,
    TaxYear int not null,
    TaxableIncome decimal(18,2) not null,
    TaxAmount decimal(18,2) not null,
    foreign key (EmployeeID) references Employee(EmployeeID) on delete cascade)

-- FinancialRecord Table
create table FinancialRecord (
    RecordID int primary key identity(1,1),
    EmployeeID int not null,
    RecordDate date not null,
    Description nvarchar(255),
    Amount decimal(18,2) not null,
    RecordType nvarchar(50) not null,
    foreign key (EmployeeID) references Employee(EmployeeID) on delete cascade)

--Insert Values into the tables

insert into Employee (FirstName, LastName, DateOfBirth, Gender, Email, PhoneNumber, [Address], Position, JoiningDate, TerminationDate)
values
('Vijayaprakash', 'M', '2003-09-17', 'Male', 'vijayaprakash885@gmail.com', '1234567890', 'Attur', 'Software Engineer', '2024-09-26', null),
('Jayes', 'Kumar', '2001-09-28', 'Male', 'jayeskumar@gmail.com', '9876543210', 'Salem', 'Manager', '2024-08-10', null)

insert into Payroll (EmployeeID, PayPeriodStartDate, PayPeriodEndDate, BasicSalary, OvertimePay, Deductions)
values
(1, '2024-11-01', '2024-11-30', 5000.00, 200.00, 50.00),
(2, '2024-11-01', '2024-11-30', 6000.00, 150.00, 100.00);

---------------
select datefromparts(year(getdate()), month(getdate()),1)
select eomonth(getdate())
-----------------

insert into Tax (EmployeeID, TaxYear, TaxableIncome, TaxAmount)
values
(1, 2024, 60000.00, 12000.00),
(2, 2024, 72000.00, 14400.00);

insert into FinancialRecord (EmployeeID, RecordDate, Description, Amount, RecordType)
values
(1, '2024-11-2', 'Bonus Payment', 5000.00, 'Credit'),
(2, '2024-11-1', 'Salary', 25000.00, 'Credit');

select * from Employee
select * from Payroll
select * from tax
select * from FinancialRecord


select 
    sum(p.NetSalary) as TotalNetSalary,
    Year as Year,
    EmployeeID as EmployeeID
from 
    Payroll p
where 
    p.EmployeeID = 1
    and year(p.PayPeriodStartDate) = 2024;

