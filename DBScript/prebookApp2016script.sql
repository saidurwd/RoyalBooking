USE [BQAzure]
GO
/****** Object:  UserDefinedFunction [dbo].[DateBackbone]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[DateBackbone]
        (
        @StartDate Datetime,
        @EndDate DateTime
        )
        RETURNS
        @Dates TABLE
        (
        Date DateTime
        )
        AS
        BEGIN
        While @StartDate <= @EndDate
        begin
        insert @Dates (Date) Values (@StartDate)
        Set @StartDate = @StartDate + 1
        end
        RETURN
        END

--select convert(varchar(10),date,102) date from [dbo].[DateBackbone]('01/26/2014', '02/12/2014')


GO
/****** Object:  Table [dbo].[Demo_BoxType]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Demo_BoxType](
	[id] [int] NULL,
	[code] [varchar](50) NULL,
	[fbe] [varchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Demo_PB_PO_Boxes]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Demo_PB_PO_Boxes](
	[statusId] [int] NULL,
	[statusName] [varchar](255) NULL,
	[position] [varchar](255) NULL,
	[lotNumber] [varchar](50) NULL,
	[boxCode] [varchar](50) NULL,
	[details_Id] [int] NULL,
	[PO_number] [varchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Demo_PB_PO_BreakDowns]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Demo_PB_PO_BreakDowns](
	[stemsBunch] [numeric](9, 2) NULL,
	[cost] [numeric](18, 4) NULL,
	[productId] [int] NULL,
	[bunches] [numeric](9, 2) NULL,
	[productDescription] [varchar](255) NULL,
	[details_Id] [int] NULL,
	[PO_number] [varchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Demo_PB_PO_CustomFields]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Demo_PB_PO_CustomFields](
	[fieldName] [varchar](255) NULL,
	[customFieldId] [int] NULL,
	[customFieldValueId] [int] NULL,
	[value] [varchar](255) NULL,
	[details_Id] [int] NULL,
	[PO_number] [varchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Demo_PB_PO_Details]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Demo_PB_PO_Details](
	[orderType] [varchar](255) NULL,
	[stemsBunch] [numeric](18, 4) NULL,
	[notes] [nvarchar](max) NULL,
	[customField1] [varchar](255) NULL,
	[standingOrder] [varchar](255) NULL,
	[details_Id] [int] NULL,
	[bunches] [numeric](18, 4) NULL,
	[units] [numeric](18, 4) NULL,
	[totalBoxes] [numeric](18, 4) NULL,
	[unitType] [varchar](255) NULL,
	[carrierName] [varchar](255) NULL,
	[referenceNumber] [varchar](255) NULL,
	[poItemId] [int] NULL,
	[customerId] [int] NULL,
	[lineItemStatus] [varchar](255) NULL,
	[productDescription] [varchar](255) NULL,
	[productId] [int] NULL,
	[boxType] [varchar](255) NULL,
	[customerName] [varchar](255) NULL,
	[totalUnits] [numeric](18, 4) NULL,
	[markCode] [varchar](255) NULL,
	[prebook] [varchar](25) NULL,
	[unitCost] [numeric](18, 4) NULL,
	[prebookTruckDate] [varchar](50) NULL,
	[quantityConfirmed] [numeric](18, 4) NULL,
	[carrierId] [int] NULL,
	[totalCost] [numeric](18, 4) NULL,
	[purchaseOrders_Id] [int] NULL,
	[prebookItemId] [varchar](50) NULL,
	[PO_number] [varchar](50) NULL,
	[id_PO] [int] NULL,
	[DeleteStatus] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Demo_PB_PO_PurchaseOrders]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Demo_PB_PO_PurchaseOrders](
	[comments] [varchar](255) NULL,
	[number] [varchar](50) NOT NULL,
	[origin] [varchar](255) NULL,
	[vendorId] [int] NULL,
	[purchaseOrders_Id] [int] NULL,
	[id] [int] NULL,
	[locationId] [int] NULL,
	[locationName] [varchar](255) NULL,
	[updatedOn] [varchar](255) NULL,
	[vendorName] [varchar](255) NULL,
	[shipDate] [datetime] NULL,
	[totalBoxes] [numeric](9, 2) NULL,
	[createdOn] [varchar](255) NULL,
	[totalCost] [varchar](50) NULL,
	[status] [varchar](255) NULL,
	[rootNode_Id] [int] NULL,
 CONSTRAINT [PK__Demo_PB___FD291E40B83B0498] PRIMARY KEY CLUSTERED 
(
	[number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Domestic_PB_PO_Boxes]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Domestic_PB_PO_Boxes](
	[statusId] [int] NULL,
	[statusName] [varchar](255) NULL,
	[position] [varchar](255) NULL,
	[lotNumber] [float] NULL,
	[boxCode] [float] NULL,
	[details_Id] [int] NULL,
	[PO_number] [varchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Domestic_PB_PO_BreakDowns]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Domestic_PB_PO_BreakDowns](
	[stemsBunch] [numeric](9, 2) NULL,
	[cost] [numeric](18, 4) NULL,
	[productId] [int] NULL,
	[bunches] [numeric](9, 2) NULL,
	[productDescription] [varchar](255) NULL,
	[details_Id] [int] NULL,
	[PO_number] [varchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Domestic_PB_PO_CustomFields]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Domestic_PB_PO_CustomFields](
	[fieldName] [varchar](255) NULL,
	[customFieldId] [int] NULL,
	[customFieldValueId] [int] NULL,
	[value] [varchar](255) NULL,
	[details_Id] [int] NULL,
	[PO_number] [varchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Domestic_PB_PO_Details]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Domestic_PB_PO_Details](
	[orderType] [varchar](255) NULL,
	[stemsBunch] [numeric](18, 4) NULL,
	[notes] [nvarchar](max) NULL,
	[customField1] [varchar](255) NULL,
	[standingOrder] [varchar](255) NULL,
	[details_Id] [int] NULL,
	[bunches] [numeric](18, 4) NULL,
	[units] [numeric](18, 4) NULL,
	[totalBoxes] [numeric](18, 4) NULL,
	[unitType] [varchar](255) NULL,
	[carrierName] [varchar](255) NULL,
	[referenceNumber] [varchar](255) NULL,
	[poItemId] [int] NULL,
	[customerId] [int] NULL,
	[lineItemStatus] [varchar](255) NULL,
	[productDescription] [varchar](255) NULL,
	[productId] [int] NULL,
	[boxType] [varchar](255) NULL,
	[customerName] [varchar](255) NULL,
	[totalUnits] [numeric](18, 4) NULL,
	[markCode] [varchar](255) NULL,
	[prebook] [numeric](18, 4) NULL,
	[unitCost] [numeric](18, 4) NULL,
	[prebookTruckDate] [varchar](50) NULL,
	[quantityConfirmed] [numeric](18, 4) NULL,
	[carrierId] [int] NULL,
	[totalCost] [numeric](18, 4) NULL,
	[purchaseOrders_Id] [int] NULL,
	[prebookItemId] [int] NULL,
	[PO_number] [varchar](50) NULL,
	[id_PO] [int] NULL,
	[DeleteStatus] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Domestic_PB_PO_PurchaseOrders]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Domestic_PB_PO_PurchaseOrders](
	[comments] [varchar](255) NULL,
	[number] [varchar](50) NOT NULL,
	[origin] [varchar](255) NULL,
	[vendorId] [int] NULL,
	[purchaseOrders_Id] [int] NULL,
	[id] [int] NULL,
	[locationId] [int] NULL,
	[locationName] [varchar](255) NULL,
	[updatedOn] [varchar](255) NULL,
	[vendorName] [varchar](255) NULL,
	[shipDate] [datetime] NULL,
	[totalBoxes] [numeric](9, 2) NULL,
	[createdOn] [varchar](255) NULL,
	[totalCost] [numeric](18, 4) NULL,
	[status] [varchar](255) NULL,
	[rootNode_Id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GmbH_PB_PO_Boxes]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GmbH_PB_PO_Boxes](
	[statusId] [int] NULL,
	[statusName] [varchar](255) NULL,
	[position] [varchar](255) NULL,
	[lotNumber] [float] NULL,
	[boxCode] [float] NULL,
	[details_Id] [int] NULL,
	[PO_number] [varchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GmbH_PB_PO_BreakDowns]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GmbH_PB_PO_BreakDowns](
	[stemsBunch] [numeric](9, 2) NULL,
	[cost] [numeric](18, 4) NULL,
	[productId] [int] NULL,
	[bunches] [numeric](9, 2) NULL,
	[productDescription] [varchar](255) NULL,
	[details_Id] [int] NULL,
	[PO_number] [varchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GmbH_PB_PO_CustomFields]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GmbH_PB_PO_CustomFields](
	[fieldName] [varchar](255) NULL,
	[customFieldId] [int] NULL,
	[customFieldValueId] [int] NULL,
	[value] [varchar](255) NULL,
	[details_Id] [int] NULL,
	[PO_number] [varchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GmbH_PB_PO_Details]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GmbH_PB_PO_Details](
	[orderType] [varchar](255) NULL,
	[stemsBunch] [numeric](18, 4) NULL,
	[notes] [nvarchar](max) NULL,
	[customField1] [varchar](255) NULL,
	[standingOrder] [varchar](255) NULL,
	[details_Id] [int] NULL,
	[bunches] [numeric](18, 4) NULL,
	[units] [numeric](18, 4) NULL,
	[totalBoxes] [numeric](18, 4) NULL,
	[unitType] [varchar](255) NULL,
	[carrierName] [varchar](255) NULL,
	[referenceNumber] [varchar](255) NULL,
	[poItemId] [int] NULL,
	[customerId] [int] NULL,
	[lineItemStatus] [varchar](255) NULL,
	[productDescription] [varchar](255) NULL,
	[productId] [int] NULL,
	[boxType] [varchar](255) NULL,
	[customerName] [varchar](255) NULL,
	[totalUnits] [numeric](18, 4) NULL,
	[markCode] [varchar](255) NULL,
	[prebook] [numeric](18, 4) NULL,
	[unitCost] [numeric](18, 4) NULL,
	[prebookTruckDate] [varchar](50) NULL,
	[quantityConfirmed] [numeric](18, 4) NULL,
	[carrierId] [int] NULL,
	[totalCost] [numeric](18, 4) NULL,
	[purchaseOrders_Id] [int] NULL,
	[prebookItemId] [int] NULL,
	[PO_number] [varchar](50) NULL,
	[id_PO] [int] NULL,
	[DeleteStatus] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GmbH_PB_PO_PurchaseOrders]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GmbH_PB_PO_PurchaseOrders](
	[comments] [varchar](255) NULL,
	[number] [varchar](50) NOT NULL,
	[origin] [varchar](255) NULL,
	[vendorId] [int] NULL,
	[purchaseOrders_Id] [int] NULL,
	[id] [int] NULL,
	[locationId] [int] NULL,
	[locationName] [varchar](255) NULL,
	[updatedOn] [varchar](255) NULL,
	[vendorName] [varchar](255) NULL,
	[shipDate] [datetime] NULL,
	[totalBoxes] [numeric](9, 2) NULL,
	[createdOn] [varchar](255) NULL,
	[totalCost] [numeric](18, 4) NULL,
	[status] [varchar](255) NULL,
	[rootNode_Id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[International_PB_PO_Boxes]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[International_PB_PO_Boxes](
	[statusId] [int] NULL,
	[statusName] [varchar](255) NULL,
	[position] [varchar](255) NULL,
	[lotNumber] [float] NULL,
	[boxCode] [float] NULL,
	[details_Id] [int] NULL,
	[PO_number] [varchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[International_PB_PO_BreakDowns]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[International_PB_PO_BreakDowns](
	[stemsBunch] [numeric](9, 2) NULL,
	[cost] [numeric](18, 4) NULL,
	[productId] [int] NULL,
	[bunches] [numeric](9, 2) NULL,
	[productDescription] [varchar](255) NULL,
	[details_Id] [int] NULL,
	[PO_number] [varchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[International_PB_PO_CustomFields]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[International_PB_PO_CustomFields](
	[fieldName] [varchar](255) NULL,
	[customFieldId] [int] NULL,
	[customFieldValueId] [int] NULL,
	[value] [varchar](255) NULL,
	[details_Id] [int] NULL,
	[PO_number] [varchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[International_PB_PO_Details]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[International_PB_PO_Details](
	[orderType] [varchar](255) NULL,
	[stemsBunch] [numeric](18, 4) NULL,
	[notes] [nvarchar](max) NULL,
	[customField1] [varchar](255) NULL,
	[standingOrder] [varchar](255) NULL,
	[details_Id] [int] NULL,
	[bunches] [numeric](18, 4) NULL,
	[units] [numeric](18, 4) NULL,
	[totalBoxes] [numeric](18, 4) NULL,
	[unitType] [varchar](255) NULL,
	[carrierName] [varchar](255) NULL,
	[referenceNumber] [varchar](255) NULL,
	[poItemId] [int] NULL,
	[customerId] [int] NULL,
	[lineItemStatus] [varchar](255) NULL,
	[productDescription] [varchar](255) NULL,
	[productId] [int] NULL,
	[boxType] [varchar](255) NULL,
	[customerName] [varchar](255) NULL,
	[totalUnits] [numeric](18, 4) NULL,
	[markCode] [varchar](255) NULL,
	[prebook] [numeric](18, 4) NULL,
	[unitCost] [numeric](18, 4) NULL,
	[prebookTruckDate] [varchar](50) NULL,
	[quantityConfirmed] [numeric](18, 4) NULL,
	[carrierId] [int] NULL,
	[totalCost] [numeric](18, 4) NULL,
	[purchaseOrders_Id] [int] NULL,
	[prebookItemId] [int] NULL,
	[PO_number] [varchar](50) NULL,
	[id_PO] [int] NULL,
	[DeleteStatus] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[International_PB_PO_PurchaseOrders]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[International_PB_PO_PurchaseOrders](
	[comments] [varchar](255) NULL,
	[number] [varchar](50) NOT NULL,
	[origin] [varchar](255) NULL,
	[vendorId] [int] NULL,
	[purchaseOrders_Id] [int] NULL,
	[id] [int] NULL,
	[locationId] [int] NULL,
	[locationName] [varchar](255) NULL,
	[updatedOn] [varchar](255) NULL,
	[vendorName] [varchar](255) NULL,
	[shipDate] [datetime] NULL,
	[totalBoxes] [numeric](9, 2) NULL,
	[createdOn] [varchar](255) NULL,
	[totalCost] [numeric](18, 4) NULL,
	[status] [varchar](255) NULL,
	[rootNode_Id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[KS_PrebooksBreakDowns]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KS_PrebooksBreakDowns](
	[productId] [float] NULL,
	[productDescription] [nvarchar](255) NULL,
	[stemsBunch] [float] NULL,
	[bunches] [float] NULL,
	[price] [nvarchar](255) NULL,
	[details_Id] [float] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PB_PO_Boxes]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PB_PO_Boxes](
	[statusId] [int] NULL,
	[statusName] [varchar](255) NULL,
	[position] [varchar](255) NULL,
	[lotNumber] [varchar](50) NULL,
	[boxCode] [varchar](50) NULL,
	[details_Id] [int] NULL,
	[PO_number] [varchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PB_PO_BreakDowns]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PB_PO_BreakDowns](
	[stemsBunch] [numeric](9, 2) NULL,
	[cost] [numeric](18, 4) NULL,
	[productId] [int] NULL,
	[bunches] [numeric](9, 2) NULL,
	[productDescription] [varchar](255) NULL,
	[details_Id] [int] NULL,
	[PO_number] [varchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PB_PO_CustomFields]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PB_PO_CustomFields](
	[fieldName] [varchar](255) NULL,
	[customFieldId] [int] NULL,
	[customFieldValueId] [int] NULL,
	[value] [varchar](255) NULL,
	[details_Id] [int] NULL,
	[PO_number] [varchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PB_PO_Details]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PB_PO_Details](
	[orderType] [varchar](255) NULL,
	[stemsBunch] [numeric](18, 4) NULL,
	[notes] [nvarchar](max) NULL,
	[customField1] [varchar](255) NULL,
	[standingOrder] [varchar](255) NULL,
	[details_Id] [int] NULL,
	[bunches] [numeric](18, 4) NULL,
	[units] [numeric](18, 4) NULL,
	[totalBoxes] [numeric](18, 4) NULL,
	[unitType] [varchar](255) NULL,
	[carrierName] [varchar](255) NULL,
	[referenceNumber] [varchar](255) NULL,
	[poItemId] [int] NULL,
	[customerId] [int] NULL,
	[lineItemStatus] [varchar](255) NULL,
	[productDescription] [varchar](255) NULL,
	[productId] [int] NULL,
	[boxType] [varchar](255) NULL,
	[customerName] [varchar](255) NULL,
	[totalUnits] [numeric](18, 4) NULL,
	[markCode] [varchar](255) NULL,
	[prebook] [varchar](25) NULL,
	[unitCost] [numeric](18, 4) NULL,
	[prebookTruckDate] [varchar](50) NULL,
	[quantityConfirmed] [numeric](18, 4) NULL,
	[carrierId] [int] NULL,
	[totalCost] [numeric](18, 4) NULL,
	[purchaseOrders_Id] [int] NULL,
	[prebookItemId] [int] NULL,
	[PO_number] [varchar](50) NULL,
	[id_PO] [int] NULL,
	[DeleteStatus] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PB_PO_Details_Delete_Move_log]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PB_PO_Details_Delete_Move_log](
	[orderType] [varchar](255) NULL,
	[stemsBunch] [numeric](18, 4) NULL,
	[notes] [nvarchar](max) NULL,
	[customField1] [varchar](255) NULL,
	[standingOrder] [varchar](255) NULL,
	[details_Id] [int] NULL,
	[bunches] [numeric](18, 4) NULL,
	[units] [numeric](18, 4) NULL,
	[totalBoxes] [numeric](18, 4) NULL,
	[unitType] [varchar](255) NULL,
	[carrierName] [varchar](255) NULL,
	[referenceNumber] [varchar](255) NULL,
	[poItemId] [int] NULL,
	[customerId] [int] NULL,
	[lineItemStatus] [varchar](255) NULL,
	[productDescription] [varchar](255) NULL,
	[productId] [int] NULL,
	[boxType] [varchar](255) NULL,
	[customerName] [varchar](255) NULL,
	[totalUnits] [numeric](18, 4) NULL,
	[markCode] [varchar](255) NULL,
	[prebook] [varchar](25) NULL,
	[unitCost] [numeric](18, 4) NULL,
	[prebookTruckDate] [varchar](50) NULL,
	[quantityConfirmed] [numeric](18, 4) NULL,
	[carrierId] [int] NULL,
	[totalCost] [numeric](18, 4) NULL,
	[purchaseOrders_Id] [int] NULL,
	[prebookItemId] [int] NULL,
	[PO_number] [varchar](50) NULL,
	[id_PO] [int] NULL,
	[DeleteStatus] [bit] NULL,
	[source] [varchar](50) NULL,
	[truckDate] [datetime] NULL,
	[DeleteOrMove] [varchar](50) NULL,
	[IsSuccess] [bit] NULL,
	[NewPrebookId] [varchar](50) NULL,
	[NewTruckDate] [varchar](50) NULL,
	[IsNew] [bit] NULL,
	[ActionTime] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PB_PO_duplicate_invoices]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PB_PO_duplicate_invoices](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[PO_number] [varchar](500) NULL,
	[source] [varchar](500) NULL,
	[insert_date] [datetime] NULL,
	[status] [bit] NULL,
	[old_shipdate] [datetime] NULL,
	[new_shipdate] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PB_PO_PurchaseOrders]    Script Date: 1/12/2017 7:09:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PB_PO_PurchaseOrders](
	[comments] [varchar](255) NULL,
	[number] [varchar](50) NOT NULL,
	[origin] [varchar](255) NULL,
	[vendorId] [int] NULL,
	[purchaseOrders_Id] [int] NULL,
	[id] [int] NULL,
	[locationId] [int] NULL,
	[locationName] [varchar](255) NULL,
	[updatedOn] [varchar](255) NULL,
	[vendorName] [varchar](255) NULL,
	[shipDate] [datetime] NULL,
	[totalBoxes] [numeric](9, 2) NULL,
	[createdOn] [varchar](255) NULL,
	[totalCost] [varchar](50) NULL,
	[status] [varchar](255) NULL,
	[rootNode_Id] [int] NULL,
 CONSTRAINT [PK__PB_PO_Pu__FD291E405AF3D49E] PRIMARY KEY CLUSTERED 
(
	[number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[PB_PO_Details_Delete_Move_log] ADD  DEFAULT (getdate()) FOR [ActionTime]
GO
ALTER TABLE [dbo].[PB_PO_duplicate_invoices] ADD  CONSTRAINT [DF_PB_PO_duplicate_invoices_insert_date]  DEFAULT (getdate()) FOR [insert_date]
GO
ALTER TABLE [dbo].[PB_PO_duplicate_invoices] ADD  CONSTRAINT [DF_PB_PO_duplicate_invoices_status]  DEFAULT ((0)) FOR [status]
GO
ALTER TABLE [dbo].[Demo_PB_PO_Boxes]  WITH CHECK ADD  CONSTRAINT [FK__Demo_PB_P__PO_nu__3C54ED00] FOREIGN KEY([PO_number])
REFERENCES [dbo].[Demo_PB_PO_PurchaseOrders] ([number])
GO
ALTER TABLE [dbo].[Demo_PB_PO_Boxes] CHECK CONSTRAINT [FK__Demo_PB_P__PO_nu__3C54ED00]
GO
ALTER TABLE [dbo].[Demo_PB_PO_BreakDowns]  WITH CHECK ADD  CONSTRAINT [FK__Demo_PB_P__PO_nu__3E3D3572] FOREIGN KEY([PO_number])
REFERENCES [dbo].[Demo_PB_PO_PurchaseOrders] ([number])
GO
ALTER TABLE [dbo].[Demo_PB_PO_BreakDowns] CHECK CONSTRAINT [FK__Demo_PB_P__PO_nu__3E3D3572]
GO
ALTER TABLE [dbo].[Demo_PB_PO_CustomFields]  WITH CHECK ADD  CONSTRAINT [FK__Demo_PB_P__PO_nu__40257DE4] FOREIGN KEY([PO_number])
REFERENCES [dbo].[Demo_PB_PO_PurchaseOrders] ([number])
GO
ALTER TABLE [dbo].[Demo_PB_PO_CustomFields] CHECK CONSTRAINT [FK__Demo_PB_P__PO_nu__40257DE4]
GO
ALTER TABLE [dbo].[Demo_PB_PO_Details]  WITH CHECK ADD  CONSTRAINT [FK__Demo_PB_P__PO_nu__3A6CA48E] FOREIGN KEY([PO_number])
REFERENCES [dbo].[Demo_PB_PO_PurchaseOrders] ([number])
GO
ALTER TABLE [dbo].[Demo_PB_PO_Details] CHECK CONSTRAINT [FK__Demo_PB_P__PO_nu__3A6CA48E]
GO
ALTER TABLE [dbo].[Domestic_PB_PO_Boxes]  WITH CHECK ADD FOREIGN KEY([PO_number])
REFERENCES [dbo].[Domestic_PB_PO_PurchaseOrders] ([number])
GO
ALTER TABLE [dbo].[Domestic_PB_PO_BreakDowns]  WITH CHECK ADD FOREIGN KEY([PO_number])
REFERENCES [dbo].[Domestic_PB_PO_PurchaseOrders] ([number])
GO
ALTER TABLE [dbo].[Domestic_PB_PO_CustomFields]  WITH CHECK ADD FOREIGN KEY([PO_number])
REFERENCES [dbo].[Domestic_PB_PO_PurchaseOrders] ([number])
GO
ALTER TABLE [dbo].[Domestic_PB_PO_Details]  WITH CHECK ADD FOREIGN KEY([PO_number])
REFERENCES [dbo].[Domestic_PB_PO_PurchaseOrders] ([number])
GO
ALTER TABLE [dbo].[GmbH_PB_PO_Boxes]  WITH CHECK ADD FOREIGN KEY([PO_number])
REFERENCES [dbo].[GmbH_PB_PO_PurchaseOrders] ([number])
GO
ALTER TABLE [dbo].[GmbH_PB_PO_BreakDowns]  WITH CHECK ADD FOREIGN KEY([PO_number])
REFERENCES [dbo].[GmbH_PB_PO_PurchaseOrders] ([number])
GO
ALTER TABLE [dbo].[GmbH_PB_PO_CustomFields]  WITH CHECK ADD FOREIGN KEY([PO_number])
REFERENCES [dbo].[GmbH_PB_PO_PurchaseOrders] ([number])
GO
ALTER TABLE [dbo].[GmbH_PB_PO_Details]  WITH CHECK ADD FOREIGN KEY([PO_number])
REFERENCES [dbo].[GmbH_PB_PO_PurchaseOrders] ([number])
GO
ALTER TABLE [dbo].[International_PB_PO_Boxes]  WITH CHECK ADD FOREIGN KEY([PO_number])
REFERENCES [dbo].[International_PB_PO_PurchaseOrders] ([number])
GO
ALTER TABLE [dbo].[International_PB_PO_BreakDowns]  WITH CHECK ADD FOREIGN KEY([PO_number])
REFERENCES [dbo].[International_PB_PO_PurchaseOrders] ([number])
GO
ALTER TABLE [dbo].[International_PB_PO_CustomFields]  WITH CHECK ADD FOREIGN KEY([PO_number])
REFERENCES [dbo].[International_PB_PO_PurchaseOrders] ([number])
GO
ALTER TABLE [dbo].[International_PB_PO_Details]  WITH CHECK ADD FOREIGN KEY([PO_number])
REFERENCES [dbo].[International_PB_PO_PurchaseOrders] ([number])
GO
ALTER TABLE [dbo].[PB_PO_Boxes]  WITH NOCHECK ADD  CONSTRAINT [FK__PB_PO_Box__PO_nu__2EFAF1E2] FOREIGN KEY([PO_number])
REFERENCES [dbo].[PB_PO_PurchaseOrders] ([number])
GO
ALTER TABLE [dbo].[PB_PO_Boxes] CHECK CONSTRAINT [FK__PB_PO_Box__PO_nu__2EFAF1E2]
GO
ALTER TABLE [dbo].[PB_PO_BreakDowns]  WITH NOCHECK ADD  CONSTRAINT [FK__PB_PO_Bre__PO_nu__30E33A54] FOREIGN KEY([PO_number])
REFERENCES [dbo].[PB_PO_PurchaseOrders] ([number])
GO
ALTER TABLE [dbo].[PB_PO_BreakDowns] CHECK CONSTRAINT [FK__PB_PO_Bre__PO_nu__30E33A54]
GO
ALTER TABLE [dbo].[PB_PO_CustomFields]  WITH NOCHECK ADD  CONSTRAINT [FK__PB_PO_Cus__PO_nu__32CB82C6] FOREIGN KEY([PO_number])
REFERENCES [dbo].[PB_PO_PurchaseOrders] ([number])
GO
ALTER TABLE [dbo].[PB_PO_CustomFields] CHECK CONSTRAINT [FK__PB_PO_Cus__PO_nu__32CB82C6]
GO
ALTER TABLE [dbo].[PB_PO_Details]  WITH NOCHECK ADD  CONSTRAINT [FK__PB_PO_Det__PO_nu__2D12A970] FOREIGN KEY([PO_number])
REFERENCES [dbo].[PB_PO_PurchaseOrders] ([number])
GO
ALTER TABLE [dbo].[PB_PO_Details] CHECK CONSTRAINT [FK__PB_PO_Det__PO_nu__2D12A970]
GO
