use master;
go
if exists (select 1 from sys.databases where name = 'WorkTimeAudit')
begin
	print 'Delete existing database'
	alter database WorkTimeAudit SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	drop database WorkTimeAudit
end
go 
create database WorkTimeAudit
on
(Name = 'WorkTimeAudit',
filename = 'd:\TestDB\WorkTimeAudit.mdf',
size = 15,
maxsize = 200,
filegrowth = 5)
log on
(Name = 'WorkTimeAudit_Log',
filename = 'd:\TestDB\WorkTimeAudit.ldf',
size = 15,
maxsize = 200,
filegrowth = 5
);
go

use WorkTimeAudit;
go

create table tblEmployeeCategories(
EmployeeCategoryID tinyint identity(1,1)
	primary key,
Category nvarchar(200) not null
);
go

create table tblEmployees(
EmployeeID int identity(1,1)
	primary key,
EmployeeCategoryID tinyint not null
	foreign key references tblEmployeeCategories(EmployeeCategoryID),
PayLoadData nvarchar(4000)
)
go

create table tblDepartaments(
DepartamentID smallint identity(1,1)
	primary key,
DepartamentHead int not null
	foreign key references tblEmployees(EmployeeID),
PayLoadData nvarchar(4000)
)
go

alter table tblEmployees
add DepartamentID smallint not null
	constraint FK_Employees_Depar foreign key(DepartamentID) references tblDepartaments(DepartamentID);
go

create table tblCustomers( -- заказчик проекта
CustomerID int identity(1,1)
	primary key,
OuterCustomer bit not null -- является ли заказчик внешним?
	default 0,
PayLoadData nvarchar(4000)
)
go

create table tblProjects(
ProjectID int identity(1,1)
	primary key,
CustomerID int not null
	foreign key references tblCustomers(CustomerID),
PayLoadData nvarchar(4000)
)
go

create table tblProjectStages( -- этапы проекта
ProjectStageID int identity(1,1)
	primary key,
ProjectID int not null
	foreign key references tblProjects(ProjectID),
PayLoadData nvarchar(4000)
)
go

create table tblEmployee_ProjectPermissions( -- разрешенные к выполнению сотруднику этапы проекта
PermissionID int identity(1,1)
	primary key,
EmployeeID int not null
	foreign key references tblEmployees(EmployeeID),
ProjectStageID int not null
	foreign key references tblProjectStages(ProjectStageID),
IsValid bit not null
	default 1
)
go


create table tblCompleatedTasks(
CompleatedTaskID int identity(1,1)
	primary key,
EmployeePermissionID int not null
	foreign key references tblEmployee_ProjectPermissions(PermissionID),
SpendHours tinyint not null
	check (SpendHours < 11),
TaskDate date not null
	default getdate(),
PayLoadData nvarchar(4000)
)
go

create nonclustered index inx_EmplPerm on tblCompleatedTasks(EmployeePermissionID) include (TaskDate, SpendHours);
go

create trigger tgHoursLimit on tblCompleatedTasks
after insert, update
as
	if exists(
			select 1
			from inserted as i
			join tblEmployee_ProjectPermissions as ep
				on i.EmployeePermissionID = ep.PermissionID
			join tblCompleatedTasks as ct
				on ep.PermissionID = ct.EmployeePermissionID and ct.TaskDate = i.TaskDate
			group by ep.EmployeeID, ct.TaskDate
			having Sum(ct.SpendHours + i.SpendHours) > 10
			)
		begin
			rollback
			Raiserror('Общее время работ сотрудника за день превышает 10 часов ',14,1)
			return
		end
go

create proc GetDepartamentSummaryReport
	@HeadID int, -- id руководителя отдела
	@StartDate date, -- начало отчетного периода
	@EndDate date -- окончание отчетного периода
as
	Raiserror('Функция не реализована',14,1)
go

