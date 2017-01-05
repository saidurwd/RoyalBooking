CREATE TABLE [International_PB_PO_PurchaseOrders] (
[comments] varchar(255),
[number] varchar(50) primary key,
[origin] varchar(255),
[vendorId] int,
[purchaseOrders_Id] int,
[id] int,
locationId int,
locationName varchar(255),
[updatedOn] varchar(255),
[vendorName] varchar(255),
[shipDate] datetime,
[totalBoxes] numeric(9,2),
[createdOn] varchar(255),
[totalCost] varchar(50),
[status] varchar(255),
[rootNode_Id] int,
);

CREATE TABLE [International_PB_PO_Details] (
[orderType] varchar(255),
[stemsBunch] numeric(18,4),
[notes] nvarchar(max),
[customField1] varchar(255),
[standingOrder] varchar(255),
[details_Id] int,
[bunches] numeric(18,4),
[units] numeric(18,4),
[totalBoxes] numeric(18,4),
[unitType] varchar(255),
[carrierName] varchar(255),
[referenceNumber] varchar(255),
[poItemId] int,
[customerId] int,
[lineItemStatus] varchar(255),
[productDescription] varchar(255),
[productId] int,
[boxType] varchar(255),
[customerName] varchar(255),
[totalUnits] numeric(18,4),
[markCode] varchar(255),
[prebook] varchar(50),
[unitCost] numeric(18,4),
[prebookTruckDate] [varchar](50),
[quantityConfirmed] numeric(18,4),
[carrierId] int,
[totalCost] numeric(18,4),
[purchaseOrders_Id] int,
prebookItemId int,
[PO_number] varchar(50) references [International_PB_PO_PurchaseOrders](number),
[id_PO] int
);

CREATE TABLE [dbo].[International_PB_PO_Boxes](
	[statusId] int,
	[statusName] [varchar](255) NULL,
	position [varchar](255) NULL,
	[lotNumber] varchar(50),
	[boxCode] varchar(50),
	[details_Id] int,
	[PO_number] varchar(50) references [International_PB_PO_PurchaseOrders](number)
);

CREATE TABLE [International_PB_PO_BreakDowns] (
[stemsBunch] numeric(9,2),
[cost] numeric(18,4),
[productId] int,
[bunches] numeric(9,2),
[productDescription] varchar(255),
[details_Id] int,
[PO_number] varchar(50) references [International_PB_PO_PurchaseOrders](number)
);

CREATE TABLE [International_PB_PO_CustomFields] (
[fieldName] varchar(255),
[customFieldId] int,
[customFieldValueId] int,
[value] varchar(255),
[details_Id] int,
[PO_number] varchar(50) references [International_PB_PO_PurchaseOrders](number)
);