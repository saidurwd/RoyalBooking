DROP TABLE [dbo].[Prebooks_duplicates];

DROP TABLE [dbo].[KS_PrebooksBreakDowns];
DROP TABLE [dbo].[KS_PrebooksDetails];
DROP TABLE [dbo].[KS_Prebooks];

DROP TABLE [dbo].[Domestic_KS_PrebooksBreakDowns];
DROP TABLE [dbo].[Domestic_KS_PrebooksDetails];
DROP TABLE [dbo].[Domestic_KS_Prebooks];

DROP TABLE [dbo].[International_KS_PrebooksBreakDowns];
DROP TABLE [dbo].[International_KS_PrebooksDetails];
DROP TABLE [dbo].[International_KS_Prebooks];

DROP TABLE [dbo].[Demo_KS_PrebooksBreakDowns];
DROP TABLE [dbo].[Demo_KS_PrebooksDetails];
DROP TABLE [dbo].[Demo_KS_Prebooks];


CREATE TABLE [dbo].[Prebooks_duplicates](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[number] [varchar](500) NULL,
	[source] [varchar](500) NULL,
	[insert_date] [datetime] NULL CONSTRAINT [DF_Prebooks_duplicates_insert_date]  DEFAULT (getdate()),
	[status] [bit] NULL CONSTRAINT [DF_Prebooks_duplicates_status]  DEFAULT ((0)),
	[old_truckDate] [datetime] NULL,
	[new_truckDate] [datetime] NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[KS_Prebooks](
	[id] [float] NULL,
	[number] [float] NULL,
	[locationName] [nvarchar](255) NULL,
	[locationId] [float] NULL,
	[locationCode] [nvarchar](255) NULL,
	[customerName] [nvarchar](255) NULL,
	[customerId] [float] NULL,
	[customerCode] [nvarchar](255) NULL,

	[shipName] [nvarchar](255) NULL,
	[shipCity] [nvarchar](255) NULL,
	[shipState] [nvarchar](255) NULL,
	[shipAddress] [nvarchar](255) NULL,
	[shipZipCode] [nvarchar](255) NULL,
	[shipCountry] [nvarchar](255) NULL,

	[type] [nvarchar](1) NULL,
	[truckDate] [datetime] NULL,
	[customerPONumber] [nvarchar](255) NULL,
	[prebooks_Id] [float] NULL,
	[carrierId] [float] NULL,
	[carrierName] [nvarchar](255) NULL,
	[shipVia] [nvarchar](255) NULL,

	[comments] [nvarchar](255) NULL,
	[totalPrice] [float] NULL,
	[rootNode_Id] [float] NULL
)

CREATE TABLE [dbo].[KS_PrebooksDetails](
	[vendorId] [float] NULL,
	[vendorName] [nvarchar](255) NULL,
	[productId] [float] NULL,
	[productDescription] [nvarchar](255) NULL,
	[totalBoxes] [float] NULL,
	[boxType] [nvarchar](255) NULL,
	[bunches] [float] NULL,
	[stemsBunch] [float] NULL,
	[unitPrice] [nvarchar](255) NULL,
	[units] [float] NULL,
	[unitType] [nvarchar](255) NULL,
	[totalUnits] [float] NULL,
	[totalPrice] [float] NULL,
	[markCode] [nvarchar](255) NULL,
	[brandId] [float] NULL,
	[brandName] [nvarchar](255) NULL,
	[details_Id] [float] NULL,
	[prebooks_Id] [float] NULL,
	[number] [float] NULL,
	prebookItemId int NULL
)

alter table KS_PrebooksDetails add DeleteStatus bit;

CREATE TABLE [dbo].[KS_PrebooksBreakDowns](
	[productId] [float] NULL,
	[productDescription] [nvarchar](255) NULL,
	[stemsBunch] [float] NULL,
	[bunches] [float] NULL,
	[price] [nvarchar](255) NULL,
	[details_Id] [float] NULL,
	[number] [float] NULL,
	[prebooks_Id] [float] NULL,
)

-------------------------------Domestic-------------------------------
CREATE TABLE [dbo].[Domestic_KS_Prebooks](
	[id] [float] NULL,
	[number] [float] NULL,
	[locationName] [nvarchar](255) NULL,
	[locationId] [float] NULL,
	[locationCode] [nvarchar](255) NULL,
	[customerName] [nvarchar](255) NULL,
	[customerId] [float] NULL,
	[customerCode] [nvarchar](255) NULL,

	[shipName] [nvarchar](255) NULL,
	[shipCity] [nvarchar](255) NULL,
	[shipState] [nvarchar](255) NULL,
	[shipAddress] [nvarchar](255) NULL,
	[shipZipCode] [nvarchar](255) NULL,
	[shipCountry] [nvarchar](255) NULL,

	[type] [nvarchar](1) NULL,
	[truckDate] [datetime] NULL,
	[customerPONumber] [nvarchar](255) NULL,
	[prebooks_Id] [float] NULL,
	[carrierId] [float] NULL,
	[carrierName] [nvarchar](255) NULL,
	[shipVia] [nvarchar](255) NULL,

	[comments] [nvarchar](255) NULL,
	[totalPrice] [float] NULL,
	[rootNode_Id] [float] NULL
)

CREATE TABLE [dbo].[Domestic_KS_PrebooksDetails](
	[vendorId] [float] NULL,
	[vendorName] [nvarchar](255) NULL,
	[productId] [float] NULL,
	[productDescription] [nvarchar](255) NULL,
	[totalBoxes] [float] NULL,
	[boxType] [nvarchar](255) NULL,
	[bunches] [float] NULL,
	[stemsBunch] [float] NULL,
	[unitPrice] [nvarchar](255) NULL,
	[units] [float] NULL,
	[unitType] [nvarchar](255) NULL,
	[totalUnits] [float] NULL,
	[totalPrice] [float] NULL,
	[markCode] [nvarchar](255) NULL,
	[brandId] [float] NULL,
	[brandName] [nvarchar](255) NULL,
	[details_Id] [float] NULL,
	[prebooks_Id] [float] NULL,
	[number] [float] NULL,
	prebookItemId int NULL
)

alter table Domestic_KS_PrebooksDetails add DeleteStatus bit;

CREATE TABLE [dbo].[Domestic_KS_PrebooksBreakDowns](
	[productId] [float] NULL,
	[productDescription] [nvarchar](255) NULL,
	[stemsBunch] [float] NULL,
	[bunches] [float] NULL,
	[price] [nvarchar](255) NULL,
	[details_Id] [float] NULL,
	[number] [float] NULL,
	[prebooks_Id] [float] NULL
)

-------------------------------International-------------------------------
CREATE TABLE [dbo].[International_KS_Prebooks](
	[id] [float] NULL,
	[number] [float] NULL,
	[locationName] [nvarchar](255) NULL,
	[locationId] [float] NULL,
	[locationCode] [nvarchar](255) NULL,
	[customerName] [nvarchar](255) NULL,
	[customerId] [float] NULL,
	[customerCode] [nvarchar](255) NULL,

	[shipName] [nvarchar](255) NULL,
	[shipCity] [nvarchar](255) NULL,
	[shipState] [nvarchar](255) NULL,
	[shipAddress] [nvarchar](255) NULL,
	[shipZipCode] [nvarchar](255) NULL,
	[shipCountry] [nvarchar](255) NULL,

	[type] [nvarchar](1) NULL,
	[truckDate] [datetime] NULL,
	[customerPONumber] [nvarchar](255) NULL,
	[prebooks_Id] [float] NULL,
	[carrierId] [float] NULL,
	[carrierName] [nvarchar](255) NULL,
	[shipVia] [nvarchar](255) NULL,

	[comments] [nvarchar](255) NULL,
	[totalPrice] [float] NULL,
	[rootNode_Id] [float] NULL
)

CREATE TABLE [dbo].[International_KS_PrebooksDetails](
	[vendorId] [float] NULL,
	[vendorName] [nvarchar](255) NULL,
	[productId] [float] NULL,
	[productDescription] [nvarchar](255) NULL,
	[totalBoxes] [float] NULL,
	[boxType] [nvarchar](255) NULL,
	[bunches] [float] NULL,
	[stemsBunch] [float] NULL,
	[unitPrice] [nvarchar](255) NULL,
	[units] [float] NULL,
	[unitType] [nvarchar](255) NULL,
	[totalUnits] [float] NULL,
	[totalPrice] [float] NULL,
	[markCode] [nvarchar](255) NULL,
	[brandId] [float] NULL,
	[brandName] [nvarchar](255) NULL,
	[details_Id] [float] NULL,
	[prebooks_Id] [float] NULL,
	[number] [float] NULL,
	prebookItemId int NULL
)

alter table International_KS_PrebooksDetails add DeleteStatus bit;

CREATE TABLE [dbo].[International_KS_PrebooksBreakDowns](
	[productId] [float] NULL,
	[productDescription] [nvarchar](255) NULL,
	[stemsBunch] [float] NULL,
	[bunches] [float] NULL,
	[price] [nvarchar](255) NULL,
	[details_Id] [float] NULL,
	[number] [float] NULL,
	[prebooks_Id] [float] NULL
)


-------------------------------Demo-------------------------------
CREATE TABLE [dbo].[Demo_KS_Prebooks](
	[id] [float] NULL,
	[number] [float] NULL,
	[locationName] [nvarchar](255) NULL,
	[locationId] [float] NULL,
	[locationCode] [nvarchar](255) NULL,
	[customerName] [nvarchar](255) NULL,
	[customerId] [float] NULL,
	[customerCode] [nvarchar](255) NULL,

	[shipName] [nvarchar](255) NULL,
	[shipCity] [nvarchar](255) NULL,
	[shipState] [nvarchar](255) NULL,
	[shipAddress] [nvarchar](255) NULL,
	[shipZipCode] [nvarchar](255) NULL,
	[shipCountry] [nvarchar](255) NULL,

	[type] [nvarchar](1) NULL,
	[truckDate] [datetime] NULL,
	[customerPONumber] [nvarchar](255) NULL,
	[prebooks_Id] [float] NULL,
	[carrierId] [float] NULL,
	[carrierName] [nvarchar](255) NULL,
	[shipVia] [nvarchar](255) NULL,

	[comments] [nvarchar](255) NULL,
	[totalPrice] [float] NULL,
	[rootNode_Id] [float] NULL
)

CREATE TABLE [dbo].[Demo_KS_PrebooksDetails](
	[vendorId] [float] NULL,
	[vendorName] [nvarchar](255) NULL,
	[productId] [float] NULL,
	[productDescription] [nvarchar](255) NULL,
	[totalBoxes] [float] NULL,
	[boxType] [nvarchar](255) NULL,
	[bunches] [float] NULL,
	[stemsBunch] [float] NULL,
	[unitPrice] [nvarchar](255) NULL,
	[units] [float] NULL,
	[unitType] [nvarchar](255) NULL,
	[totalUnits] [float] NULL,
	[totalPrice] [float] NULL,
	[markCode] [nvarchar](255) NULL,
	[brandId] [float] NULL,
	[brandName] [nvarchar](255) NULL,
	[details_Id] [float] NULL,
	[prebooks_Id] [float] NULL,
	[number] [float] NULL,
	prebookItemId int NULL
)

alter table Demo_KS_PrebooksDetails add DeleteStatus bit;

CREATE TABLE [dbo].[Demo_KS_PrebooksBreakDowns](
	[productId] [float] NULL,
	[productDescription] [nvarchar](255) NULL,
	[stemsBunch] [float] NULL,
	[bunches] [float] NULL,
	[price] [nvarchar](255) NULL,
	[details_Id] [float] NULL,
	[number] [float] NULL,
	[prebooks_Id] [float] NULL
)

CREATE TABLE dbo.Prebook_Delete_Log(
	id int identity(1,1) primary key,
	PrebookId int,
	PrebookItemId int,
	number varchar(50),
	[productDescription] [nvarchar](255) NULL,
	[source] [varchar](500) NULL
)