﻿create database CoffeeShopDB
go
use CoffeeShopDB
go
create table GENRE_PRODUCT
(
	GP_ID int identity(1,1),
	GP_NAME nvarchar(max) not null,
	IS_DELETED bit default 0,
	constraint pk_GP_ver1 primary key(GP_ID),
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
	constraint pk_PRO_v1 primary key(PRO_ID),
	constraint fk_PRO_GENRE_v1 foreign key(GP_ID) references GENRE_PRODUCT(GP_ID),
)

create table GENRE_TABLE 
(
	GT_ID  int identity(1,1),
	GT_NAME nvarchar(max) not null,
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
	constraint chk_TABLE_STATUS check (TB_STATUS in(N'Còn trống', N'Đang bận', N'Đang sửa chữa')),
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

create table RESERVATION
(
	RES_ID int identity(1,1),
	CUS_ID int not null,
	TABLE_ID int not null, 
	RES_DATE datetime2 not null, 
	RES_TIME datetime2 not null, 
	NUM_OF_PEOPLE int not null, 
	RES_STATUS nvarchar(100) default N'Khách chưa nhận bàn', 
	SPECIAL_REQUEST nvarchar(max),  
	CREATE_AT smalldatetime default getdate(), 
	IS_DELETED bit default 0, 
	constraint pk_RES primary key(RES_ID),
	constraint fk_CUS foreign key(CUS_ID) references CUSTOMER(CUS_ID),
	constraint fk_TABLE foreign key(TABLE_ID) references _TABLE(TB_ID),
	constraint chk_RES check(RES_STATUS in ( N'Khách chưa nhận bàn',  N'Khách đã nhận bàn'))
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
	SUP_ADDR nvarchar(max),
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

create table EXPORT
(
	EXP_ID int identity(1,1),
	EMP_ID int,
	EXP_DATE smalldatetime default getdate(),
	TOTAL_COST money,
	IS_DELETED bit default 0,
	constraint pk_EXP primary key(EXP_ID),
	constraint fk_EXP_EMP foreign key(EMP_ID) references EMPLOYEE(EMP_ID),
)

create table EXPORT_INFO
(
	EXP_ID int,
	ING_ID int,
	QUANTITY int default 1,
	PRICE_ITEM money,
	IS_DELETED bit default 0,
	constraint pk_ExpInfo primary key(EXP_ID, ING_ID),
	constraint fk_ExpInfo_EXP foreign key(EXP_ID) references EXPORT(EXP_ID),
	constraint fk_ExpInfo_PRO foreign key(ING_ID) references INGREDIENT(ING_ID),
)

create table BILL
(
	BILL_ID int identity(1,1),
	CUS_ID int,
	EMP_ID int,
	SUBTOTAL money,
	DISCOUNT money,
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
set dateformat dmy

INSERT INTO EMPLOYEE (EMP_NAME, EMP_PHONE, EMP_CCCD, EMP_BIRTHDAY, EMP_USERNAME, EMP_PASSWORD, EMP_EMAIL, EMP_GENDER, EMP_ROLE)
VALUES 
(N'Ngọc Nguyên', '0912345678',	'012345678901', '01-01-2005', 'admin', '202cb962ac59075b964b07152d234b70', 'ngocnguyen@example.com', N'Nữ', N'Quản lý'),
(N'Khánh Ngọc', '0704768077',	'079358699603', '09-05-2005', 'ngoc', '63b63159b50d87d00baabfb3f679fe23', 'duongkhanhngoc2005@gmail.com', N'Nữ', N'Pha chế'),
(N'Bảo Ngọc', '0704768088',	'079578699803', '22-02-2005', 'baongoc', '0d88c2b05d966896ecf7e2983c5383bc', 'daolebaongoc@gmail.com', N'Nữ', N'Pha chế'),
(N'Huệ Nguyên', '0954768088',	'079658699803', '31-10-2005', 'nguyen', '27ff2ffe376b2edcc7c2de309173f0d8', 'huenguyen@gmail.com', N'Nữ', N'Quản lý'),
(N'Phong Linh', '0934567890', '123456789012', '15-02-2004', 'phonglinh', '30421bbcdcefc1d3da04228f722d74ae', 'phonglinh@example.com', N'Nữ', N'Phục vụ'),
(N'Quốc Bảo', '0912345679', '012345678902', '10-03-2003', 'quocbao', '7c90dee826557e9a536a59daf64bf87c', 'quocbao@example.com', N'Nam', N'Pha chế'),
(N'Lan Anh', '0934768099', '079578699804', '20-06-2002', 'lananh', '9d9ca0162c8cc09f018f37c3d37bc8b3', 'lananh@gmail.com', N'Nữ', N'Phục vụ'),
(N'Tiến Dũng', '0904768099', '079678699804', '25-07-2002', 'tiendung', '6b12523e2367e1de9b8936caad798a1f', 'tiendung@example.com', N'Nam', N'Thu ngân');

INSERT INTO GENRE_PRODUCT (GP_NAME) VALUES (N'Coffee')
INSERT INTO GENRE_PRODUCT (GP_NAME) VALUES (N'Trà sữa')
INSERT INTO GENRE_PRODUCT (GP_NAME) VALUES (N'Trà')
INSERT INTO GENRE_PRODUCT (GP_NAME) VALUES (N'Sinh tố')
INSERT INTO GENRE_PRODUCT (GP_NAME) VALUES (N'Nước ép')

INSERT INTO GENRE_TABLE (GT_NAME) VALUES (N'Bàn 2')
INSERT INTO GENRE_TABLE (GT_NAME) VALUES (N'Bàn 3')
INSERT INTO GENRE_TABLE (GT_NAME) VALUES (N'Bàn 4')
INSERT INTO GENRE_TABLE (GT_NAME) VALUES (N'Bàn 6')

INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (1, N'Còn trống')
INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (1, N'Đang bận')
INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (2, N'Còn trống')
INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (4, N'Còn trống')
INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (2, N'Đang bận')
INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (1, N'Đang bận')
INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (2, N'Còn trống')
INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (4, N'Còn trống')
INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (2, N'Đang bận')
INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (3, N'Đang sửa chữa')
INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (3, N'Đang bận')
INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (3, N'Còn trống')
INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (4, N'Đang sửa chữa')
INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (2, N'Còn trống')
INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (2, N'Đang bận')
INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (1, N'Đang sửa chữa')
INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (3, N'Đang bận')
INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (3, N'Đang sửa chữa')
INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (4, N'Còn trống')
INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (4, N'Còn trống')
INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (2, N'Đang bận')
INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (1, N'Còn trống')
INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (2, N'Còn trống')
INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (1, N'Đang bận')
INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (2, N'Còn trống')
INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (4, N'Còn trống')
INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (2, N'Đang bận')

INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_DESCRIPTION, PRO_PRICE) VALUES (N'Trà đào', 3, 'pack://application:,,,/DemoDataPrdImg/tradao.jpg',N'Đào tươi ngon theo mùa', 15000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Cacao sữa xay', 1, 'pack://application:,,,/DemoDataPrdImg/cacaoSua.jpg', 20000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Espresso', 1, 'pack://application:,,,/DemoDataPrdImg/espresso.jpg', 18000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Trà sữa pudding', 2, 'pack://application:,,,/DemoDataPrdImg/trasuapudding.jpg', 23000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Coffee muối', 1, 'pack://application:,,,/DemoDataPrdImg/coffeemuoi.jpg', 18000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_DESCRIPTION, PRO_PRICE) VALUES (N'Trà ổi', 3, 'pack://application:,,,/DemoDataPrdImg/traoi.jpg', N'Ổi mới nhập hàng', 20000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_DESCRIPTION, PRO_PRICE) VALUES (N'Ép cam', 5, 'pack://application:,,,/DemoDataPrdImg/epcam.jpg',N'Ép bằng máy', 18000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Bạc xỉu', 1, 'pack://application:,,,/DemoDataPrdImg/bacxiu.jpg', 18000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_DESCRIPTION, PRO_PRICE) VALUES (N'Trà chanh', 3, 'pack://application:,,,/DemoDataPrdImg/trachanh.jpg', N'Chanh tươi không đắng', 15000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Sinh tố xoài', 4, 'pack://application:,,,/DemoDataPrdImg/sinhtoxoai.jpg', 20000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Trà tắc', 3, 'pack://application:,,,/DemoDataPrdImg/tratac.jpg', 15000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Sinh tố dâu', 4, 'pack://application:,,,/DemoDataPrdImg/sinhtoDau.jpg', 20000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Sữa tươi đường đen', 2, 'pack://application:,,,/DemoDataPrdImg/suatuoitc.jpg', 23000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Sinh tố dưa hấu', 4, 'pack://application:,,,/DemoDataPrdImg/sinhtoduahau.png', 20000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Ép táo', 5, 'pack://application:,,,/DemoDataPrdImg/eptao.jpg', 18000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_DESCRIPTION, PRO_PRICE) VALUES (N'Coffee', 1, 'pack://application:,,,/DemoDataPrdImg/coffee.jpg', N'Cà phê pha máy', 15000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Sinh tố mãng cầu', 4, 'pack://application:,,,/DemoDataPrdImg/stmangcau.jpg', 20000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Ép lê', 5, 'pack://application:,,,/DemoDataPrdImg/eple.jpg', 18000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Trà sữa thái xanh', 2, 'pack://application:,,,/DemoDataPrdImg/tsthaixanh.jpg', 15000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Trà sữa socola', 2, 'pack://application:,,,/DemoDataPrdImg/tssocola.jpg', 25000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Trà sen', 3, 'pack://application:,,,/DemoDataPrdImg/trasen.jpg', 15000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Sinh tố dừa', 4, 'pack://application:,,,/DemoDataPrdImg/stdua.jpg', 20000)

--1/20 gia tri hoa don
INSERT INTO CUSTOMER (CUS_NAME, CUS_GENDER, CUS_PHONE, CUS_EMAIL, CUS_POINT) VALUES (N'Huệ Nguyên', N'Nữ', '0967484222', 'huenguyentran3110@gmail.com', 2000)
INSERT INTO CUSTOMER (CUS_NAME, CUS_GENDER, CUS_PHONE, CUS_EMAIL, CUS_POINT) VALUES (N'Chị Khánh', N'Nữ', '0966484224', 'khanh@gmail.com' ,4000)
INSERT INTO CUSTOMER (CUS_NAME, CUS_GENDER, CUS_PHONE, CUS_EMAIL, CUS_POINT) VALUES (N'Anh Duy', N'Nam', '0674112998', 'duy@gmail.com',6000)
INSERT INTO CUSTOMER (CUS_NAME, CUS_GENDER, CUS_PHONE, CUS_EMAIL, CUS_POINT) VALUES (N'Chị Ngọc', N'Nữ', '0674484122', 'ngoc2413@gmail.com', 2000)
INSERT INTO CUSTOMER (CUS_NAME, CUS_GENDER, CUS_PHONE, CUS_EMAIL, CUS_POINT) VALUES (N'Bảo Ngọc', N'Nam', '0000555000', 'baongoc25@gmail.com', 2500)
INSERT INTO CUSTOMER (CUS_NAME, CUS_GENDER, CUS_PHONE, CUS_EMAIL, CUS_POINT) VALUES (N'Trinh', N'Nữ', '0962245993', 'ngoctrinh@gmail.com', 2000)
INSERT INTO CUSTOMER (CUS_NAME, CUS_GENDER, CUS_PHONE, CUS_EMAIL, CUS_POINT) VALUES (N'Khánh', N'Nữ', '092459869', 'KhanhNgoc@gmail.com', 5000)
INSERT INTO CUSTOMER (CUS_NAME, CUS_GENDER, CUS_PHONE, CUS_EMAIL, CUS_POINT) VALUES (N'Quốc Anh', N'Nam', '0000555440', 'quocanh22@gmail.com', 7000)

INSERT INTO RESERVATION (CUS_ID, TABLE_ID, RES_DATE, RES_TIME, NUM_OF_PEOPLE, RES_STATUS, SPECIAL_REQUEST)
VALUES
(6, 16, '28-12-2024', '8:00:00', 5, N'Khách chưa nhận bàn', NULL),
(1, 1, '31-12-2024', '15:00:00', 2, N'Khách đã nhận bàn', N'Chỗ gần cửa sổ'),
(5, 1, '30-12-2024', '12:00:00', 2, N'Khách đã nhận bàn', N'Không dùng thức uống lạnh'),
(2, 3, '2-1-2025', '13:30:00', 3, N'Khách chưa nhận bàn', NULL),
(3, 6, '3-1-2025', '8:00:00', 4, N'Khách chưa nhận bàn', N'Yêu cầu yên tĩnh'),
(6, 9, '3-1-2025', '8:00:00', 6, N'Khách chưa nhận bàn', N'Chỗ gần máy lạnh'),
(7, 13, '1-1-2025', '15:00:00', 4, N'Khách chưa nhận bàn', NULL),
(8, 1, '3-1-2025', '17:00:00', 2, N'Khách chưa nhận bàn', NULL),
(1, 4, '3-1-2025', '13:00:00', 3, N'Khách chưa nhận bàn', NULL),
(8, 1, '1-1-2025', '17:00:00', 2, N'Khách đã nhận bàn', NULL),
(4, 16, '3-1-2024', '8:00:00', 5, N'Khách chưa nhận bàn', N'Yêu cầu background'),
(3, 4, '2-1-2025', '8:00:00', 3, N'Khách chưa nhận bàn', NULL),
(4, 15, '09-1-2025', '19:00:00', 5, N'Khách chưa nhận bàn', NULL);


-- WORK_SHIFT
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
	(1, 2, 5), (1, 3, 3), (1, 2, 6), (1, 2, 7),
	(2, 3, 3), (2,1,2), (2,2,4), (2,1,5), (2, 3, 1), (2, 1, 7), (2, 2, 6),
	(3, 1, 1), (3, 2, 3), (3, 3, 6), (3, 3, 4), (3, 1, 7), (3, 3, 2),
	(4, 2, 1), (4, 2, 5), (4, 1, 3), (4, 3, 7), (4, 3, 6), (4, 2, 7), (4, 1, 4), (4, 1, 1), (4, 3, 1), (4, 3, 2),
	(5, 1, 1), (5, 1, 2), (5, 2, 3), (5, 2, 4), (5, 3, 5), (5, 3, 2), (5, 2, 7),
    (6, 2, 2), (6, 2, 4), (6, 3, 5), (6, 1, 7), (6, 1, 4), (6, 2, 6), 
    (7, 3, 1), (7, 1, 3), (7, 2, 5), (7, 3, 7), (7, 2, 2), (7, 3, 2), (7, 2, 6),
    (8, 1, 2), (8, 2, 4), (8, 3, 6), (8, 1, 7), (8, 3, 5), (8, 2, 1), (8, 2, 6);

INSERT INTO REQUEST(EMP_ID, REQ_TYPE, EMP_COMMENT) VALUES 
(2,N'Đổi ca',N'Xin đổi ca tối thứ 4 sang thứ 5'),
(4,N'Xin nghỉ',N'Xin nghỉ phép vào ca tối thứ 5 vì nhà có việc ạ.');

set dateformat dmy;
INSERT INTO REQUEST(EMP_ID, REQ_DATE, REQ_TYPE, EMP_COMMENT, APPROVED_BY, REQ_STATUS, APPROVED_DATE, APPROVER_COMMENT) VALUES 
(2,'18-12-2024',N'Đổi ca',N'Xin nghỉ ca sáng chủ nhật ngày 22/12/2024', 4, N'Từ chối', '20-12-2024', N'Có thể đổi với ca chiều hoặc vào ngày thứ 2 hôm sau'),
(2,'27-12-2024',N'Xin nghỉ',N'Xin nghỉ ca sáng thứ 6 ngày 3/1/2025', 1, N'Đã duyệt', '29-12-2024', N'Oke em check lịch lại trên app nhé');

-- Bill
INSERT INTO Bill (CUS_ID, EMP_ID, SUBTOTAL, DISCOUNT, TOTAL_COST, CREATE_AT) VALUES (1, 2, 56000, 0, 56000, '27-12-2024 7:40:00');
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (1, 1, 1, 15000);
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (1, 3, 1, 18000);
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (1, 4, 1, 23000);

INSERT INTO Bill (CUS_ID, EMP_ID, SUBTOTAL, DISCOUNT, TOTAL_COST, CREATE_AT) VALUES (2, 2, 43000, 0, 43000, '28-12-2024 17:40:00');
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (2, 2, 1, 20000);
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (2, 4, 1, 23000);

INSERT INTO Bill (CUS_ID, EMP_ID, SUBTOTAL, DISCOUNT, TOTAL_COST, CREATE_AT) VALUES (3, 2, 43000, 6000, 37000, '28-12-2024 16:40:00');
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (3, 2, 1, 20000);
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (3, 4, 1, 23000);

INSERT INTO Bill (CUS_ID, EMP_ID, SUBTOTAL, DISCOUNT, TOTAL_COST, CREATE_AT) VALUES (3, 2, 35000, 1850, 33150, '29-12-2024 17:40:00');
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (4, 1, 1, 15000);
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (4, 2, 1, 20000);

INSERT INTO Bill (CUS_ID, EMP_ID, SUBTOTAL, DISCOUNT, TOTAL_COST, CREATE_AT) VALUES (8, 2, 96000, 7000, 89000, '30-12-2024 17:40:00');
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (5, 1, 1, 15000);
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (5, 2, 1, 20000);
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (5, 3, 1, 18000);
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (5, 4, 1, 23000);
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (5, 12, 1, 20000);


INSERT INTO Bill (CUS_ID, EMP_ID, SUBTOTAL, DISCOUNT, TOTAL_COST, CREATE_AT) VALUES (2, 2, 59000, 6150, 52850, '31-12-2024 17:40:00');
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (6, 3, 1, 18000);
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (6, 4, 1, 23000);
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (6, 8, 1, 18000);

INSERT INTO Bill (EMP_ID, SUBTOTAL, DISCOUNT, TOTAL_COST, CREATE_AT) VALUES (2, 38000, 0, 38000, '04-01-2025 13:40:00');
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (7, 2, 1, 20000);
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (7, 3, 1, 18000);

INSERT INTO Bill (EMP_ID, SUBTOTAL, DISCOUNT, TOTAL_COST, CREATE_AT) VALUES (2, 43000, 0, 43000, '05-01-2025 13:40:00');
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (8, 2, 1, 20000);
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (8, 4, 1, 23000);

-- Thêm dữ liệu mẫu vào bảng INGREDIENT
INSERT INTO INGREDIENT (ING_NAME, ING_UNIT) 
VALUES 
(N'Bột mì', 'Kg'),
(N'Đường', 'Kg'),
(N'Muối', 'Kg'),
(N'Sữa', N'Lít'),
(N'Trứng', N'Chục'),
(N'Cà phê','Kg'),
(N'Sữa đặc','Lon'),
(N'Siro','Lít');

-- Thêm dữ liệu mẫu vào bảng SUPPLIER
-- Thêm dữ liệu mẫu vào bảng SUPPLIER
INSERT INTO SUPPLIER (SUP_NAME, SUP_PHONE, SUP_ADDR) 
VALUES 
(N'Công ty TNHH thực phẩm A', '123456789', N'123 Đường Thống Nhất'),
(N'Nhà máy sản xuất B', '987654321', N'456 Đường Linh Trung, Thủ Đức'),
(N'Công ty lương thực C', '555555555', N'789 Đường Võ Văn Ngân'),
(N'Công ty Cà phê D', '0912345679', N'123 Đường Quang Trung, TP.HCM'),
(N'Công ty Sản xuất E', '0987654321', N'10 Đường Nguyễn Thái Học, Q1'),
(N'Công ty Nước giải khát F', '0123456789', N'987 Đường Lê Quang Định, Bình Thạnh');

-- Thêm dữ liệu mẫu vào bảng IMPORT
INSERT INTO IMPORT (SUP_ID, EMP_ID, TOTAL_COST) 
VALUES 
(1, 1, 20 * 50000.00 + 15 * 60000.00), -- Nhập từ nhà cung cấp A (Bột mì và Đường)
(2, 4, 25 * 20000.00 + 30 * 45000.00), -- Nhập từ nhà cung cấp B (Muối và Sữa)
(3, 1, 12 * 70000.00), -- Nhập từ nhà cung cấp C (Trứng) 
(4, 4, 10 * 40000.00 + 15 * 35000.00), -- Nhập từ nhà cung cấp D (Cà phê và Sữa đặc) 
(5, 1, 30 * 80000.00); -- Nhập từ nhà cung cấp E (Siro) 

-- Thêm dữ liệu mẫu vào bảng IMPORT_INFO
INSERT INTO IMPORT_INFO (IMP_ID, ING_ID, QUANTITY, PRICE_ITEM) 
VALUES 
(1, 1, 20, 50000.00), -- Bột mì nhập từ nhà cung cấp A
(1, 2, 15, 60000.00), -- Đường nhập từ nhà cung cấp A
(2, 3, 25, 20000.00), -- Muối nhập từ nhà cung cấp B
(2, 4, 30, 45000.00), -- Sữa nhập từ nhà cung cấp B
(3, 5, 12, 70000.00), -- Trứng nhập từ nhà cung cấp C
(4, 6, 10, 40000.00), -- Cà phê nhập từ nhà cung cấp D
(4, 7, 15, 35000.00), -- Sữa đặc nhập từ nhà cung cấp D
(5, 8, 30, 80000.00); -- Siro nhập từ nhà cung cấp E

-- Thêm dữ liệu mẫu vào bảng EXPORT
INSERT INTO EXPORT (EMP_ID, TOTAL_COST) 
VALUES 
(2, 15 * 60000.00 + 10 * 60000.00),  -- Xuất Bột mì và Đường 
(2, 13 * 25000.00 + 14 * 55000.00),  -- Xuất Muối và Sữa
(3, 5 * 75000.00),  -- Xuất Trứng 
(2, 10 * 40000.00 + 12 * 35000.00),  -- Xuất Cà phê và Sữa đặc 
(4, 6 * 80000.00);  -- Xuất Siro 

-- Thêm dữ liệu mẫu vào bảng EXPORT_INFO
INSERT INTO EXPORT_INFO (EXP_ID, ING_ID, QUANTITY, PRICE_ITEM) 
VALUES 
(1, 1, 15, 60000.00),  -- Bột mì xuất
(1, 2, 10, 60000.00),  -- Đường xuất
(2, 3, 13, 25000.00),  -- Muối xuất
(2, 4, 14, 55000.00),  -- Sữa xuất
(3, 5, 5, 75000.00),  -- Trứng xuất
(4, 6, 10, 40000.00),  -- Cà phê xuất
(4, 7, 12, 35000.00),  -- Sữa đặc xuất
(5, 8, 6, 80000.00);  -- Siro xuất



-- Thêm dữ liệu mẫu về sự cố trong quán cà phê vào bảng ERROR
INSERT INTO ERROR (ER_NAME, ER_DESCRIPTION, ER_STATUS) 
VALUES 
(N'Hư máy xay cà phê', N'Máy xay cà phê không hoạt động, cần kiểm tra hoặc thay thế.', N'Đã khắc phục'),
(N'Hỏng đèn chiếu sáng', N'Đèn trong khu vực khách ngồi bị cháy, gây thiếu sáng.',N'Chưa khắc phục'),
(N'Sự cố đường ống nước', N'Đường ống nước bị rò rỉ, cần sửa chữa ngay.',N'Chưa khắc phục'),
(N'Hết giấy in hóa đơn', N'Máy in hóa đơn không hoạt động vì hết giấy, cần bổ sung giấy.',N'Đã khắc phục'),
(N'Lỗi kết nối Wi-Fi', N'Mạng Wi-Fi không ổn định, gây khó chịu cho khách hàng.',N'Đã khắc phục'),
(N'Hư khóa cửa', N'Khóa cửa chính bị kẹt, không thể đóng hoặc mở.',N'Chưa khắc phục'),
(N'Sự cố quạt trần', N'Quạt trần kêu to hoặc không quay, cần sửa chữa.',N'Chưa khắc phục');