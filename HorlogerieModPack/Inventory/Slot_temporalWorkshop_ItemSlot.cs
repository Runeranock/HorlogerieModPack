using System.Linq;
using System;
using Vintagestory.API.Common;
using System.Collections.Generic;
using System.Collections;
using Vintagestory.API.Datastructures;

namespace HorlogerieModPack.Inventory
{
    public class Slot_temporalWorkshop_ItemSlot : Inv_ItemSlot
    {
        private string[] AuthorizedItemList;
        private string[] AuthorizedItemHoldPropList;
        private Dictionary<string, object[]?> NeedPropValueList;

        public Slot_temporalWorkshop_ItemSlot(InventoryBase inventory, string[] authorizedItemList = null, string[] authorizedItemHoldPropList = null, Dictionary<string, object[]> needPropValueList = null) : base(inventory, authorizedItemList)
        {
        }

        public override bool CanHold(ItemSlot sourceSlot)
        {

            IAttribute value=null;
            sourceSlot?.Itemstack?.Attributes.TryGetAttribute(KeyContants.DispositifTargetKey,out value);
            if (value != null && !string.IsNullOrEmpty(value.GetValue().ToString()))
            {
                return false;
            }
            return base.CanHold(sourceSlot);
        }
    }
}
