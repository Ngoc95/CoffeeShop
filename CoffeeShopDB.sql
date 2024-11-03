create database CoffeeShopDB
go
use CoffeeShopDB
go
create table GENRE_PRODUCT
(
	GP_ID int identity(1,1),
	GP_NAME nvarchar(max),
	constraint pk_GP primary key(GP_ID),
)
create table PRODUCT
(	
	PRO_ID int identity(1,1),
	PRO_NAME nvarchar(max),
	GP_ID int,
	PRO_IMG varchar(max),
	PRO_DESCRIPTION nvarchar(max),
	PRO_PRICE money default 0,
	IS_DELETED bit default 0,
	constraint pk_PRO primary key(PRO_ID),
	constraint fk_PRO_GENRE foreign key(GP_ID) references GENRE_PRODUCT(GP_ID),
)
create table GENRE_TABLE 
(
	GT_ID int identity(1,1),
	GT_NAME nvarchar(max),
	constraint pk_GT primary key(GT_ID),
)
create table _TABLE
(	
	TB_ID int identity(1,1),
	GT_ID int,
	TB_STATUS nvarchar(100) default N'Còn trống',
	IS_DELETED bit default 0,
	constraint pk_TB primary key(TB_ID),
	constraint fk_TABLE_GENRE foreign key(GT_ID) references GENRE_TABLE(GT_ID),
	constraint chk_TABLE_STATUS check (TB_STATUS in(N'Còn trống', N'Đã đặt', N'Đang sửa chữa')),
)
create table CUSTOMER
(
	CUS_ID int identity(1,1),
	CUS_NAME nvarchar(max),
	CUS_PHONE varchar(20),
	CUS_EMAIL nvarchar(max),
	IS_DELETED bit default 0,
	constraint pk_CUS primary key(CUS_ID),
)
create table EMPLOYEE
(
	EMP_ID int identity(1,1),
	EMP_NAME nvarchar(max),
	EMP_PHONE varchar(20),
	EMP_CCCD varchar(12),
	EMP_STARTDATE smalldatetime default getdate(),
	EMP_BIRTHDAY smalldatetime,
	EMP_USERNAME varchar(max),
	EMP_PASSWORD varchar(max),
	EMP_EMAIL varchar(max),
	EMP_GENDER nvarchar(3) not null,
	EMP_STATUS nvarchar(100) default N'Đang làm',
	EMP_SHIFT nvarchar(max),
	EMP_SALARY money,
	EMP_ROLE nvarchar(100),
	IS_DELETED bit default 0,
	constraint pk_EMP primary key(EMP_ID),
	constraint chk_EMP check(EMP_GENDER in (N'Nam',N'Nữ') and EMP_STATUS in (N'Đang làm',N'Xin nghỉ') and EMP_ROLE in(N'Quản lý',N'Pha chế',N'Thu ngân',N'Phục vụ')),
)
CREATE TABLE WORK_SHIFT (
    SHIFT_ID int identity(1,1),
    SHIFT_NAME nvarchar(100),
    START_TIME smalldatetime,
    END_TIME smalldatetime,
	IS_DELETED bit default 0,
	constraint pk_WORK_SHIFT primary key(SHIFT_ID),
	constraint chk_WORK_SHIFT check (SHIFT_NAME in (N'Ca sáng',N'Ca chiều',N'Ca tối')),
);

CREATE TABLE EMPLOYEE_SHIFT (
    EMP_ID int,
    SHIFT_ID int,
    WORK_DATE date,
	IS_DELETED bit default 0,
	constraint pk_EMP_SHIFT primary key (EMP_ID, SHIFT_ID),
    constraint fk_EMPSHIFT_EMP foreign key (EMP_ID) references EMPLOYEE(EMP_ID),
    constraint fk_EMPSHIFT_SHIFT foreign key (SHIFT_ID) references WORK_SHIFT(SHIFT_ID)
);

create table INGREDIENT
(
	ING_ID int identity(1,1),
	ING_NAME nvarchar(max),
	ING_UNIT nvarchar(100),
	IS_DELETED bit default 0,
	constraint pk_ING primary key(ING_ID),
)
create table SUPPLIER
(
	SUP_ID int identity(1,1),
	SUP_NAME nvarchar(max),
	SUP_PHONE varchar(20),
	IS_DELETED bit default 0,
	constraint pk_SUP primary key(SUP_ID),
)
create table IMPORT
(
	IMP_ID int identity(1,1),
	SUP_ID int,
	EMP_ID int,
	IMP_DATE smalldatetime default getdate(),
	TOTAL_COST money,
	IS_DELETED bit default 0,
	constraint pk_IMP primary key(IMP_ID),
	constraint fk_IMP_SUP foreign key(SUP_ID) references SUPPLIER(SUP_ID),
	constraint fk_IMP_EMP foreign key(EMP_ID) references EMPLOYEE(EMP_ID),
)
create table IMPORT_INFO
(
	IMP_ID int,
	ING_ID int,
	QUANTITY int default 1,
	PRICE_ITEM money,
	IS_DELETED bit default 0,
	constraint pk_ImpInfo primary key(IMP_ID, ING_ID),
	constraint fk_ImpInfo_IMP foreign key(IMP_ID) references IMPORT(IMP_ID),
	constraint fk_ImpInfo_ING foreign key(ING_ID) references INGREDIENT(ING_ID),
)
create table BILL
(
	BILL_ID int identity(1,1),
	CUS_ID int,
	EMP_ID int,
	TOTAL_COST money,
	CREATE_AT smalldatetime default getdate(),
	IS_DELETED bit default 0,
	constraint pk_BILL primary key(BILL_ID),
	constraint fk_BILL_CUS foreign key(CUS_ID) references CUSTOMER(CUS_ID),
	constraint fk_BILL_EMP foreign key(EMP_ID) references EMPLOYEE(EMP_ID),
)
create table BILL_INFO
(
	BILL_ID int,
	PRO_ID int,
	QUANTITY int default 1,
	PRICE_ITEM money,
	IS_DELETED bit default 0,
	constraint pk_BillInfo primary key(BILL_ID, PRO_ID),
	constraint fk_BillInfo_BILL foreign key(BILL_ID) references BILL(BILL_ID),
	constraint fk_BillInfo_PRO foreign key(PRO_ID) references PRODUCT(PRO_ID),
)
create table ERROR
(
	ER_ID int identity(1,1),
	ER_NAME nvarchar(max),
	ER_STATUS nvarchar(100) default N'Chưa khắc phục',
	ER_DESCRIPTION nvarchar(max),
	IS_DELETED bit default 0,
	constraint pk_ER primary key(ER_ID),
	constraint chk_ER check(ER_STATUS in (N'Chưa khắc phục', N'Đã khắc phục')),
)