create database CoffeeShopDB
go
use CoffeeShopDB
go
create table GENRE_PRODUCT
(
	GP_ID int identity(1,1),
	GP_NAME nvarchar(max) not null,
	constraint pk_GP primary key(GP_ID),
)
create table PRODUCT
(	
	PRO_ID  int identity(1,1),
	PRO_NAME nvarchar(max) not null,
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
	GT_ID  int identity(1,1),
	GT_NAME nvarchar(max) not null,
	constraint pk_GT primary key(GT_ID),
)
create table _TABLE
(	
	TB_ID  int identity(1,1),
	GT_ID int,
	TB_STATUS nvarchar(100) default N'Còn trống',
	IS_DELETED bit default 0,
	constraint pk_TB primary key(TB_ID),
	constraint fk_TABLE_GENRE foreign key(GT_ID) references GENRE_TABLE(GT_ID),
	constraint chk_TABLE_STATUS check (TB_STATUS in(N'Còn trống', N'Đã đặt', N'Đang sửa chữa')),
)
create table CUSTOMER
(
	CUS_ID  int identity(1,1),
	CUS_NAME nvarchar(max),
	CUS_GENDER nvarchar(3) not null,
	CUS_PHONE varchar(20),
	CUS_EMAIL nvarchar(max),
	CUS_POINT money default 0,
	IS_DELETED bit default 0,
	constraint pk_CUS primary key(CUS_ID),
	constraint chk_CUS check(CUS_GENDER in (N'Nam',N'Nữ')),
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
	EMP_SALARY money default 0,
	EMP_ROLE nvarchar(100),
	IS_DELETED bit default 0,
	constraint pk_EMP primary key(EMP_ID),
	constraint chk_EMP check(EMP_GENDER in (N'Nam',N'Nữ') and EMP_STATUS in (N'Đang làm',N'Xin nghỉ') and EMP_ROLE in(N'Quản lý',N'Pha chế',N'Thu ngân',N'Phục vụ')),
)
CREATE TABLE WORK_SHIFT (
    SHIFT_ID  int,
    SHIFT_NAME nvarchar(100),
    START_TIME smalldatetime,
    END_TIME smalldatetime,
	WAGE money,
	IS_DELETED bit default 0,
	constraint pk_WORK_SHIFT primary key(SHIFT_ID),
	constraint chk_WORK_SHIFT check (SHIFT_NAME in (N'Ca sáng',N'Ca chiều',N'Ca tối')),
);

CREATE TABLE EMPLOYEE_SHIFT (
    EMP_ID int,
    SHIFT_ID int,
    WORK_DAY int,
	IS_DELETED bit default 0,
	constraint pk_EMP_SHIFT primary key (EMP_ID, SHIFT_ID, WORK_DAY),
    constraint fk_EMPSHIFT_EMP foreign key (EMP_ID) references EMPLOYEE(EMP_ID),
    constraint fk_EMPSHIFT_SHIFT foreign key (SHIFT_ID) references WORK_SHIFT(SHIFT_ID),
	constraint chk_WORK_DAY check (WORK_DAY BETWEEN 1 AND 7)
);
CREATE TABLE REQUEST (
    REQ_ID int identity(1,1), 
    EMP_ID int, 
    REQ_TYPE nvarchar(50) NOT NULL,
    REQ_DATE smalldatetime NOT NULL default GETDATE(), 
    REQ_STATUS nvarchar(50) default N'Chờ duyệt', 
    EMP_COMMENT nvarchar(MAX), 
    APPROVER_COMMENT nvarchar(MAX), 
    APPROVED_BY int NULL, -- Mã người phê duyệt (quản lý)
    APPROVED_DATE smalldatetime NULL, 
	IS_DELETED bit default 0,
	constraint pk_REQ primary key(REQ_ID),
	constraint fk_REQ_EMP foreign key (EMP_ID) references EMPLOYEE(EMP_ID),
	constraint chk_REQ_STATUS check (REQ_STATUS in (N'Chờ duyệt',N'Đã duyệt',N'Từ chối')),
	constraint chk_REQ_TYPE check (REQ_TYPE in (N'Xin nghỉ', N'Đổi ca')),
)


--create table INGREDIENT
--(
--	ING_ID int identity(1,1),
--	ING_NAME nvarchar(max),
--	ING_UNIT nvarchar(100),
--	IS_DELETED bit default 0,
--	constraint pk_ING primary key(ING_ID),
--)
--create table SUPPLIER
--(
--	SUP_ID int identity(1,1),
--	SUP_NAME nvarchar(max),
--	SUP_PHONE varchar(20),
--	IS_DELETED bit default 0,
--	constraint pk_SUP primary key(SUP_ID),
--)
--create table IMPORT
--(
--	IMP_ID int identity(1,1),
--	SUP_ID int,
--	EMP_ID int,
--	IMP_DATE smalldatetime default getdate(),
--	TOTAL_COST money,
--	IS_DELETED bit default 0,
--	constraint pk_IMP primary key(IMP_ID),
--	constraint fk_IMP_SUP foreign key(SUP_ID) references SUPPLIER(SUP_ID),
--	constraint fk_IMP_EMP foreign key(EMP_ID) references EMPLOYEE(EMP_ID),
--)
--create table IMPORT_INFO
--(
--	IMP_ID int,
--	ING_ID int,
--	QUANTITY int default 1,
--	PRICE_ITEM money,
--	IS_DELETED bit default 0,
--	constraint pk_ImpInfo primary key(IMP_ID, ING_ID),
--	constraint fk_ImpInfo_IMP foreign key(IMP_ID) references IMPORT(IMP_ID),
--	constraint fk_ImpInfo_ING foreign key(ING_ID) references INGREDIENT(ING_ID),
--)
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
	ER_ID  int identity(1,1),
	ER_NAME nvarchar(max),
	ER_STATUS nvarchar(100) default N'Chưa khắc phục',
	ER_DESCRIPTION nvarchar(max),
	IS_DELETED bit default 0,
	constraint pk_ER primary key(ER_ID),
	constraint chk_ER check(ER_STATUS in (N'Chưa khắc phục', N'Đã khắc phục')),
)

INSERT INTO EMPLOYEE (EMP_NAME, EMP_PHONE, EMP_CCCD, EMP_BIRTHDAY, EMP_USERNAME, EMP_PASSWORD, EMP_EMAIL, EMP_GENDER, EMP_ROLE)
VALUES 
(N'Ngọc Nguyên', '0912345678',	'012345678901', '2005-01-01', 'admin', '202cb962ac59075b964b07152d234b70', 'ngocnguyen@example.com', N'Nữ', N'Quản lý'),
(N'Ngọc', '098',	'01', '2005-01-01', 'ngoc', '202cb962ac59075b964b07152d234b70', 'ngoc', N'Nữ', N'Phục vụ');
-- Insert sample data for WORK_SHIFT
INSERT INTO WORK_SHIFT (SHIFT_ID, SHIFT_NAME, WAGE, START_TIME, END_TIME)
VALUES 
    (1, N'Ca sáng', 230000, '06:00:00', '14:00:00'),
    (2, N'Ca chiều', 200000, '14:00:00', '17:30:00'),
    (3, N'Ca tối', 250000, '17:30:00', '22:00:00');
INSERT INTO EMPLOYEE_SHIFT (EMP_ID, SHIFT_ID, WORK_DAY)
VALUES 
    (1, 1, 1), -- Thứ Hai
    (1, 1, 2), -- Thứ Ba
    (1, 1, 3), -- Thứ Tư
    (1, 1, 4), -- Thứ Năm
    (1, 1, 5), -- Thứ Sáu
    (1, 1, 6), -- Thứ Bảy
    (1, 1, 7), -- Chủ nhật
	(1, 2, 5),
	(1, 3, 3),
	(2, 3, 3);
INSERT INTO REQUEST(EMP_ID, REQ_TYPE, EMP_COMMENT) VALUES (2,N'Đổi ca',N'Xin đổi sang ca sáng thứ 2')