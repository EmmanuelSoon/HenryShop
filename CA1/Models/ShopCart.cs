using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CA1.Models
{
    public class ShopCart
    {
        public ShopCart()
        {
            ShopCartItems = new List<ShopCartItem>();
            Id = new Guid();
        }
        public Guid Id { get; set; }
        public virtual ICollection<ShopCartItem> ShopCartItems { get; set; }
        public virtual Guid UserId { get; set; }
        public void Update(ShopCartItem item, int qty)
        {
            /*1.if qty == 0
             *      ask user want to delete or not 
             *      if yes, delete
             *      else do nothing return
             * 2. else if(qty > inventory qty)
             *          warning message
             *          do nothing return 
             * 3. else
             *         update the new Quantity in the ShopCartItems
             */
        }
        public void MakePurchase()
        {
            //for checking
            foreach(var item in ShopCartItems)
            {
                /*
                 * 1. (pre check)Check Inventory balance to that product 
                 *          if not okay
                 *                  warning message("inventory only have @balance amount for @product.name");
                 *                  return;
                 */
            }
            //for update
            foreach (var item in ShopCartItems)
            {
                /*
                 *  1. Minus amt from inventory database
                 *  2. Create new order in the ordata base that linked to userId  
                 */
            }
            // ShopCartItems.clear() and remove shopcartitems record where shopcartid = shopcartid;

        }
    }
}
