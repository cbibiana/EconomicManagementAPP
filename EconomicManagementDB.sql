CREATE DATABASE [EconomicManagementDB]
GO
USE [EconomicManagementDB]
GO
CREATE TABLE [Users](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[StandarEmail] [nvarchar](256) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
)
GO
CREATE TABLE [OperationTypes](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
)
GO
CREATE TABLE [AccountTypes](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[UserId] [int] NOT NULL,
	[OrderAccount] [int] NOT NULL,
	CONSTRAINT [FK_AccountTypesUsers] FOREIGN KEY (UserId) REFERENCES Users(Id)
)
GO
CREATE TABLE [Accounts](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[AccountTypeId] [int] NOT NULL,
	[Balance] [decimal](18, 2) NOT NULL,
	[Description] [nvarchar](1000) NULL,
    CONSTRAINT [FK_AccountType] FOREIGN KEY (AccountTypeId) REFERENCES AccountTypes(Id)
)
GO
CREATE TABLE Categories(
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[OperationTypeId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
    CONSTRAINT [FK_CategoriesOperations] FOREIGN KEY (OperationTypeId) REFERENCES OperationTypes(Id)	
) 
GO
CREATE TABLE [Transactions](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[UserId] [int] NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[OperationTypeId] [int] NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[AccountId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
    CONSTRAINT [FK_TransactionsUsers] FOREIGN KEY (UserId) REFERENCES Users(Id),
	CONSTRAINT [FK_TransactiosOperationType] FOREIGN KEY (OperationTypeId) REFERENCES OperationTypes(Id),
	CONSTRAINT [FK_TransactionsAccount] FOREIGN KEY (AccountId) REFERENCES Accounts(Id),
	CONSTRAINT [FK_TransactionsCategories] FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
)

-- =============================================
--Procedimientos almacemados
-- =============================================

-- =============================================
--AccountTypes_Insertar
-- =============================================

CREATE PROCEDURE [dbo].[AccountTypes_Insertar] 
	@Name nvarchar (50),
	@UserId int 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   DECLARE @OrderAccount int;
   SELECT @OrderAccount = COALESCE (MAX(OrderAccount),0)+1
   FROM AccountTypes
   WHERE UserId = @UserId

   INSERT INTO AccountTypes(Name, UserId, OrderAccount)
   VALUES(@Name, @UserId, @OrderAccount);

   SELECT SCOPE_IDENTITY();
END

-- =============================================
--Transactions_Delete
-- =============================================

CREATE PROCEDURE [dbo].[Transactions_Delete]
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Total decimal (18,2);
	DECLARE @AccountId int;
	DECLARE @OperationTypeId int;

	SELECT @Total = Total, @AccountId = AccountId, @OperationTypeId = cat.OperationTypeId
	FROM Transactions
	inner join Categories cat
	ON cat.Id = Transactions.CategoryId
	WHERE Transactions.Id = @Id;

	DECLARE @FactorMultiplicative int = 1;

	IF(@OperationTypeId = 2)
	SET @FactorMultiplicative = -1;

	SET @Total = @Total * @FactorMultiplicative;

	UPDATE Accounts
	SET Balance -=@Total
	WHERE Id = @AccountId;

	DELETE Transactions
	WHERE Id = @Id;

END

-- =============================================
--Transactions_Insertar
-- =============================================
CREATE PROCEDURE [dbo].[Transactions_Insertar]
  @UserId int,
  @TransactionDate date,
  @Total decimal,
  @AccountId int,
  @CategoryId int,
  @Description nvarchar (1000) = NULL


AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Transactions(UserId, TransactionDate, Total, AccountId, CategoryId, Description)
	Values (@UserId, @TransactionDate, ABS(@Total), @AccountId, @CategoryId, @Description)

	UPDATE Accounts
	SET Balance += @Total
	WHERE Id = @AccountId;
    
	SELECT SCOPE_IDENTITY();

END


-- =============================================
--Transactions_Modify
-- =============================================


CREATE PROCEDURE [dbo].[Transactions_Modify]
	-- Add the parameters for the stored procedure here
	@Id int,
	@TransactionDate datetime,
	@Total decimal(18,2),
	@TotalPrevious decimal (18,2),
	@AccountId int,
	@AccountPreviousId int,
	@CategoryId int,
	@Description nvarchar(1000) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    --Revertir transacición anterior

	UPDATE Accounts
	SET Balance -= @TotalPrevious
	WHERE Id = @AccountPreviousId;

	--Realizar nueva transaccion
	
	UPDATE Accounts
	SET Balance += @Total
	WHERE Id = @AccountId;

	UPDATE Transactions
	SET @Total = ABS(@Total), TransactionDate = @TransactionDate,
	CategoryId = @CategoryId, AccountId = @AccountId, Description = @Description
	WHERE Id = Id;
END

