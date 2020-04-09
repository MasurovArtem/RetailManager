using System;
using System.Collections.Generic;
using System.Linq;
using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Models;

namespace RMDataManager.Library.DataAccess
{
    public class SaleData
    {
        public void SaveSale(SaleModel saleInfo, string userId)
        {
            // TODO:: Make this SOLID/DRY/BETTER

            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            ProductData product = new ProductData();
            var taxRate = ConfigHelper.GetTaxRate() / 100;

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                //Get information about this product
                var productInfo = product.GetProductById(detail.ProductId);
                if (productInfo == null)
                {
                    throw new Exception($"The product id of  {detail.ProductId} could not be found in the database");
                }

                detail.PurchasePrice = (productInfo.RetailPrice * detail.Quantity);
                if (productInfo.IsTaxable)
                {
                    detail.Tax = (detail.PurchasePrice * taxRate);
                }
                details.Add(detail);
            }

            // Create the Sale model

            SaleDbModel sale = new SaleDbModel()
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = userId
            };

            sale.Total = sale.SubTotal + sale.Tax;

            using (SqlDataAccess sql = new SqlDataAccess())
            {
                try
                {
                    sql.StartTransaction("RMData");

                    // Save the sale model
                    sql.SaveDataInTransaction("dbo.spSale_Insert", sale);

                    // Get the ID from the sale mode
                    sale.Id = sql.LoadDataInTransaction<int, dynamic>(
                        "dbo.spSale_Lookup",
                        new
                        {
                            sale.CashierId,
                            sale.SaleDate
                        }).FirstOrDefault();

                    // Finish filling int the sale detail models
                    foreach (var item in details)
                    {
                        item.SaleId = sale.Id;

                        // Save the sale detail model
                        sql.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
                    }

                    sql.CommitTransaction();
                }
                catch(Exception ex)
                {
                   sql.RollBackTransaction();
                   throw;
                }
            }
        }
    }
}