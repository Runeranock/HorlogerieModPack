using HorlogerieModPack.Enums;
using HorlogerieModPack.Extension;
using HorlogerieModPack.Gui;
using HorlogerieModPack.Inventory;
using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace HorlogerieModPack
{
    public class BlockEntityTemporalWorkshop : BlockEntityOpenableContainer
    {
        public virtual string DialogTitle
        {
            get { return "Temporal Workshop"; }
//TODO: Lang.Get("Temporal Workshop");
        }
       
        Inv_TemporalWorkshop inventory;
        GuiDialogTemporalWorkshop clientDialog;
        public BlockEntityTemporalWorkshop()
        {
            inventory = new Inv_TemporalWorkshop(null, null);
            inventory.SlotModified += Inventory_SlotModified;
        }

        private void Inventory_SlotModified(int obj)
        {
                
        }

        public ItemSlot InputSlot
        {
            get { return inventory[0]; }
        }

        public ItemSlot OutputSlot
        {
            get { return inventory[1]; }
        }
        public ItemStack InputStack
        {
            get { return inventory[0].Itemstack; }
            set { inventory[0].Itemstack = value; inventory[0].MarkDirty(); }
        }

        public ItemStack OutputStack
        {
            get { return inventory[1].Itemstack; }
            set { inventory[1].Itemstack = value; inventory[1].MarkDirty(); }
        }
        #region inventory
        public override InventoryBase Inventory
        {
            get { return inventory; }
        }
        public override string InventoryClassName
        {
            get { return nameof(Inv_TemporalWorkshop); }
        }
        public void EngraveItem(string selectedTarget)
        {
            if (Api?.Side != EnumAppSide.Server)
                return;

            if (InputSlot?.Itemstack == null || string.IsNullOrEmpty(selectedTarget))
                return;

            if (OutputSlot == null || !OutputSlot.CanHold(InputSlot))
                return;

            InputSlot.Itemstack.Attributes?.SetString(KeyContants.DispositifTargetKey, selectedTarget);
            if (!EnumUtils.IsInEnum<EnumTargetLocation>(selectedTarget))
            {
                string playerName = Api.World.PlayerByUid(selectedTarget).PlayerName;
                InputSlot.Itemstack.Attributes?.SetString("playerName", playerName);
            }
            InputSlot.Itemstack.Attributes?.SetInt(KeyContants.DispositifChargeKey, Api.World.Config.GetAsInt(nameof(HMPConfigDefaults.MaxCharge)));
            Inventory[0].TryPutInto(Api.World, OutputSlot, 1);
            
            MarkDirty();
        }

        #endregion
        public override void OnStoreCollectibleMappings(Dictionary<int, AssetLocation> blockIdMapping, Dictionary<int, AssetLocation> itemIdMapping)
        {
            foreach (var slot in Inventory)
            {
                if (slot.Itemstack == null) continue;

                if (slot.Itemstack.Class == EnumItemClass.Item)
                {
                    itemIdMapping[slot.Itemstack.Item.Id] = slot.Itemstack.Item.Code;
                }
                else
                {
                    blockIdMapping[slot.Itemstack.Block.BlockId] = slot.Itemstack.Block.Code;
                }
                slot.Itemstack?.Collectible.OnStoreCollectibleMappings(Api.World, slot, blockIdMapping, itemIdMapping);
            }
        }

        public override void OnLoadCollectibleMappings(IWorldAccessor worldForResolve, Dictionary<int, AssetLocation> oldBlockIdMapping, Dictionary<int, AssetLocation> oldItemIdMapping, int schematicSeed, bool resolveImports)
        {
            foreach (var slot in Inventory)
            {
                if (slot.Itemstack == null) continue;
                if (!slot.Itemstack.FixMapping(oldBlockIdMapping, oldItemIdMapping, worldForResolve))
                {
                    slot.Itemstack = null;
                }
                slot.Itemstack?.Collectible.OnLoadCollectibleMappings(worldForResolve, slot, oldBlockIdMapping, oldItemIdMapping, resolveImports);
            }
        }
        public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
        {
            base.FromTreeAttributes(tree, worldForResolving);
        }

        public override void ToTreeAttributes(ITreeAttribute tree)
        {
            base.ToTreeAttributes(tree);
        }
        public override bool OnPlayerRightClick(IPlayer byPlayer, BlockSelection blockSel)
        {
           
            if (blockSel.SelectionBoxIndex == 1) return false;

            if (Api.Side == EnumAppSide.Client)
            {
                toggleInventoryDialogClient(byPlayer, () => {
                    clientDialog = new GuiDialogTemporalWorkshop(DialogTitle, Inventory, Pos, Api as ICoreClientAPI, byPlayer);
                  
                    //clientDialog.Update(inputGrindTime, maxGrindingTime());
                    return clientDialog;
                });
            }

            return true;
        }
        public override void Initialize(ICoreAPI api)
        {
            base.Initialize(api);

            inventory.LateInitialize("temporalworkshop-" + Pos.X + "/" + Pos.Y + "/" + Pos.Z, api);
        }

        public override void OnReceivedClientPacket(IPlayer player, int packetid, byte[] data)
        {
            base.OnReceivedClientPacket(player, packetid, data);
            if (packetid == 1001 && data!=null)
            {
                string selectedTarget = SerializerUtil.Deserialize<string>(data);
                
                EngraveItem(selectedTarget);
            }
        }
       
    }
}
