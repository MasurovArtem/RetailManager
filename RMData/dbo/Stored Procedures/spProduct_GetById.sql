CREATE PROCEDURE [dbo].[spProduct_GetById]
	@id int
AS
begin
	set nocount on;
	select Id, ProductName, [Description], RetailPrice, QuantityInStock, IsTaxable
	from Product
	Where Id = @id
end