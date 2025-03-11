using HorlogerieModPack.Behavior;
using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace HorlogerieModPack.Inventory
{
    public class Inv_TemporalWorkshop : InventoryBase, ISlotProvider
    {
        ItemSlot[] slots;
        public ItemSlot[] Slots { get { return slots; } }
        private static readonly string[] AuthorizedItems = { KeyContants.TranslocationDispositif };


        public Inv_TemporalWorkshop(string inventoryID, ICoreAPI api) : base(inventoryID, api)
        {
            // slot 0 = input
            // slot 1 = output
            slots = new ItemSlot[2];
            slots[0] = new Slot_temporalWorkshop_ItemSlot(this, AuthorizedItems); // Slot avec restriction
            slots[1] = new ItemSlot(this); // Slot normal pour l'output
        }
        public Inv_TemporalWorkshop(string className, string instanceID, ICoreAPI api) : base(className, instanceID, api)
        {
            slots = new ItemSlot[2];
            slots[0] = new Inv_ItemSlot(this, AuthorizedItems);
            slots[1] = new ItemSlot(this);
        }

        public override int Count
        {
            get { return 2; }
        }

        public override ItemSlot this[int slotId]
        {
            get
            {
                if (slotId < 0 || slotId >= Count) return null;
                return slots[slotId];
            }
            set
            {
                if (slotId < 0 || slotId >= Count) throw new ArgumentOutOfRangeException(nameof(slotId));
                if (value == null) throw new ArgumentNullException(nameof(value));
                slots[slotId] = value;
            }
        }

        public override void FromTreeAttributes(ITreeAttribute tree)
        {
            slots = SlotsFromTreeAttributes(tree, slots);
        }

        public override void ToTreeAttributes(ITreeAttribute tree)
        {
            SlotsToTreeAttributes(slots, tree);
        }

        protected override ItemSlot NewSlot(int i)
        {
            return new ItemSlotSurvival(this);
        }
        public override float GetSuitability(ItemSlot sourceSlot, ItemSlot targetSlot, bool isMerge)
        {
            if (targetSlot == slots[0] && sourceSlot.Itemstack.Collectible.HasBehavior<TranslocationDispositifBehavior>()) return 4f;

            return base.GetSuitability(sourceSlot, targetSlot, isMerge);
        }

        public override ItemSlot GetAutoPushIntoSlot(BlockFacing atBlockFace, ItemSlot fromSlot)
        {
            return slots[0];
        }
    }
}
