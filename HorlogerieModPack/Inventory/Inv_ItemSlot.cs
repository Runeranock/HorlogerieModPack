using System.Linq;
using System;
using Vintagestory.API.Common;
using System.Collections.Generic;
using System.Collections;

namespace HorlogerieModPack.Inventory
{
    public class Inv_ItemSlot : ItemSlot
    {
        private string[] AuthorizedItemList;
        public Inv_ItemSlot(InventoryBase inventory, string[] authorizedItemList = null) : base(inventory)
        {
            AuthorizedItemList = authorizedItemList ?? new string[0];
        }
        private bool IsAuthorizedList => (AuthorizedItemList != null && AuthorizedItemList.Length > 0);
        public override bool CanHold(ItemSlot sourceSlot)
        {
            if (!IsAuthorizedList)
                return true;

            var path = sourceSlot?.Itemstack?.Collectible?.Code?.Path?.ToString();
            if (AuthorizedItemList.ToList().Any(item => path.StartsWith(item)))
            {               
                return true;
            }
            return false;
        }
    }
}
