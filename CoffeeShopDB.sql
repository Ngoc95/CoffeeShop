create database CoffeeShopDB
go
use CoffeeShopDB
go
create table GENRE_PRODUCT
(
	GP_ID int identity(1,1),
	GP_NAME nvarchar(max) not null,
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

INSERT INTO EMPLOYEE (EMP_NAME, EMP_PHONE, EMP_CCCD, EMP_BIRTHDAY, EMP_USERNAME, EMP_PASSWORD, EMP_EMAIL, EMP_GENDER, EMP_ROLE)
VALUES 
(N'Ngọc Nguyên', '0912345678',	'012345678901', '2005-01-01', 'admin', '202cb962ac59075b964b07152d234b70', 'ngocnguyen@example.com', N'Nữ', N'Quản lý'),
(N'Ngọc', '098',	'01', '2005-01-01', 'ngoc', '202cb962ac59075b964b07152d234b70', 'ngoc', N'Nữ', N'Phục vụ');

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
INSERT INTO _TABLE (GT_ID, TB_STATUS) VALUES (2, N'Đang sửa chữa')
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

INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Trà đào', 3, 'pack://application:,,,/DemoDataPrdImg/tradao.jpg', 15000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Cacao sữa xay', 1, 'pack://application:,,,/DemoDataPrdImg/cacaoSua.jpg', 20000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Espresso', 1, 'pack://application:,,,/DemoDataPrdImg/espresso.jpg', 18000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Trà sữa pudding', 2, 'pack://application:,,,/DemoDataPrdImg/trasuapudding.jpg', 23000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Coffee muối', 1, 'pack://application:,,,/DemoDataPrdImg/coffeemuoi.jpg', 18000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Trà ổi', 3, 'pack://application:,,,/DemoDataPrdImg/traoi.jpg', 20000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Ép cam', 5, 'pack://application:,,,/DemoDataPrdImg/epcam.jpg', 18000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Bạc xỉu', 1, 'pack://application:,,,/DemoDataPrdImg/bacxiu.jpg', 18000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Trà chanh', 3, 'pack://application:,,,/DemoDataPrdImg/trachanh.jpg', 15000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Sinh tố xoài', 4, 'pack://application:,,,/DemoDataPrdImg/sinhtoxoai.jpg', 20000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Trà tắc', 3, 'pack://application:,,,/DemoDataPrdImg/tratac.jpg', 15000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Sinh tố dâu', 4, 'pack://application:,,,/DemoDataPrdImg/sinhtoDau.jpg', 20000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Sữa tươi đường đen', 2, 'pack://application:,,,/DemoDataPrdImg/suatuoitc.jpg', 23000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Sinh tố dưa hấu', 4, 'pack://application:,,,/DemoDataPrdImg/sinhtoduahau.png', 20000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Ép táo', 5, 'pack://application:,,,/DemoDataPrdImg/eptao.jpg', 18000)
INSERT INTO PRODUCT (PRO_NAME, GP_ID, PRO_IMG, PRO_PRICE) VALUES (N'Coffee', 1, 'pack://application:,,,/DemoDataPrdImg/coffee.jpg', 15000)
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

set dateformat dmy

INSERT INTO RESERVATION (CUS_ID, TABLE_ID, RES_DATE, RES_TIME, NUM_OF_PEOPLE, RES_STATUS, SPECIAL_REQUEST)
VALUES
(1, 1, '31-12-2024', '15:00:00', 2, N'Khách đã nhận bàn', N'Chỗ gần cửa sổ'),
(2, 3, '2-1-2025', '13:30:00', 3, N'Khách chưa nhận bàn', NULL),
(3, 6, '2-1-2025', '8:00:00', 4, N'Khách chưa nhận bàn', N'Yêu cầu yên tĩnh'),
(4, 15, '09-1-2025', '19:00:00', 5, N'Khách chưa nhận bàn', NULL),
(5, 1, '30-12-2024', '12:00:00', 2, N'Khách đã nhận bàn', N'Không dùng thức uống lạnh'),
(6, 9, '3-1-2025', '8:00:00', 6, N'Khách chưa nhận bàn', N'Chỗ gần máy lạnh'),
(7, 13, '1-1-2025', '15:00:00', 4, N'Khách chưa nhận bàn', N'Thích đồ uống ít đường'),
(8, 1, '2-1-2025', '17:00:00', 2, N'Khách chưa nhận bàn', NULL),
(1, 4, '2-1-2025', '13:00:00', 3, N'Khách chưa nhận bàn', NULL),
(2, 16, '2-1-2024', '8:00:00', 5, N'Khách chưa nhận bàn', NULL);


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

set dateformat dmy
-- Bill
INSERT INTO Bill (CUS_ID, EMP_ID, SUBTOTAL, DISCOUNT, TOTAL_COST, CREATE_AT) VALUES (1, 2, 56000, 0, 56000, '10-12-2024 7:40:00');
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (1, 1, 1, 15000);
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (1, 3, 1, 18000);
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (1, 4, 1, 23000);

INSERT INTO Bill (CUS_ID, EMP_ID, SUBTOTAL, DISCOUNT, TOTAL_COST, CREATE_AT) VALUES (2, 2, 43000, 0, 43000, '10-12-2024 17:40:00');
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (2, 2, 1, 20000);
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (2, 4, 1, 23000);

INSERT INTO Bill (CUS_ID, EMP_ID, SUBTOTAL, DISCOUNT, TOTAL_COST, CREATE_AT) VALUES (3, 2, 43000, 6000, 37000, '10-12-2024 17:40:00');
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (3, 2, 1, 20000);
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (3, 4, 1, 23000);

INSERT INTO Bill (CUS_ID, EMP_ID, SUBTOTAL, DISCOUNT, TOTAL_COST, CREATE_AT) VALUES (3, 2, 35000, 1850, 33150, '10-12-2024 17:40:00');
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (4, 1, 1, 15000);
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (4, 2, 1, 20000);

INSERT INTO Bill (CUS_ID, EMP_ID, SUBTOTAL, DISCOUNT, TOTAL_COST, CREATE_AT) VALUES (8, 2, 96000, 7000, 89000, '10-12-2024 17:40:00');
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (5, 1, 1, 15000);
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (5, 2, 1, 20000);
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (5, 3, 1, 18000);
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (5, 4, 1, 23000);
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (5, 12, 1, 20000);


INSERT INTO Bill (CUS_ID, EMP_ID, SUBTOTAL, DISCOUNT, TOTAL_COST, CREATE_AT) VALUES (2, 2, 59000, 6150, 52850, '10-12-2024 17:40:00');
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (6, 3, 1, 18000);
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (6, 4, 1, 23000);
INSERT INTO BILL_INFO (BILL_ID, PRO_ID, QUANTITY, PRICE_ITEM) VALUES (6, 8, 1, 18000);
