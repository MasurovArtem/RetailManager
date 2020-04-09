CREATE PROCEDURE [dbo].[spInventory_Insert]
	@ProductId int,
	@Quantity int,
	@PurchasePrice money,
	@PurchaseDate datetime2

AS
Begin
	set nocount on;
	
	insert into dbo.Inventory(ProductId, Quantity, PurchasePrice, PurchaseDate)
	values (@ProductId, @Quantity, @PurchasePrice, @PurchaseDate); 
end
