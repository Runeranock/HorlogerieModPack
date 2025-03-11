using HorlogerieModPack.Enums;
using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using HorlogerieModPack;

namespace HorlogerieModPack.Gui
{

    public class GuiDialogTemporalWorkshop : GuiDialogBlockEntity
    {
        private HMPConfig config;
        const string SRC_INV_ID = "sourceInventory";
        const string SLOT_INPUT_ID = "runeSlot";
        const string SLOT_OUTPUT_ID = "resultSlot";
        const string DROPDOWN_ELEMENT_ID = "playerDropdown";
        public string MESSAGE_ELEMENT_ID = "messageGuidDialogurId";
        private readonly ICoreClientAPI _capi;
        private ItemStack previousItemStack;
        private string SelectedTarget;
        const string buttonKey = "buttonKey";
        public static double[] PrimaryColor = new double[] { 1, 1, 1, 1 };



        IPlayer player;
        public GuiDialogTemporalWorkshop(string DialogTitle, InventoryBase Inventory, BlockPos BlockEntityPosition, ICoreClientAPI capi, IPlayer byPlayer) : base(DialogTitle, Inventory, BlockEntityPosition, capi)
        {
            _capi = capi;
            player = byPlayer;
            capi.World.Player.InventoryManager.OpenInventory(Inventory);
            config = GetConfig(capi);
            ComposeDialog();

        }
        private HMPConfig GetConfig(ICoreClientAPI api)
        {
            var config = new HMPConfig();
            config.MaxCharge = api.World.Config.GetInt(nameof(HMPConfigDefaults.MaxCharge), HMPConfigDefaults.MaxCharge);
            config.UseCharge = api.World.Config.GetBool(nameof(HMPConfigDefaults.UseCharge), HMPConfigDefaults.UseCharge);
            config.DestroyAfterUse = api.World.Config.GetBool(nameof(HMPConfigDefaults.DestroyAfterUse), HMPConfigDefaults.DestroyAfterUse);
            config.CanConfigureForToNorth = api.World.Config.GetBool(nameof(HMPConfigDefaults.CanConfigureForToNorth), HMPConfigDefaults.CanConfigureForToNorth);
            config.CanConfigureForToSouth = api.World.Config.GetBool(nameof(HMPConfigDefaults.CanConfigureForToSouth), HMPConfigDefaults.CanConfigureForToSouth);
            config.CanConfigureForToEast = api.World.Config.GetBool(nameof(HMPConfigDefaults.CanConfigureForToEast), HMPConfigDefaults.CanConfigureForToEast);
            config.CanConfigureForToWest = api.World.Config.GetBool(nameof(HMPConfigDefaults.CanConfigureForToWest), HMPConfigDefaults.CanConfigureForToWest);
            config.CanConfigureForRandom = api.World.Config.GetBool(nameof(HMPConfigDefaults.CanConfigureForRandom), HMPConfigDefaults.CanConfigureForRandom);
            config.CanConfigureForPlayer = api.World.Config.GetBool(nameof(HMPConfigDefaults.CanConfigureForPlayer), HMPConfigDefaults.CanConfigureForPlayer);
            config.CanConfigureForSpawn = api.World.Config.GetBool(nameof(HMPConfigDefaults.CanConfigureForSpawn), HMPConfigDefaults.CanConfigureForSpawn);
            return config;
        }


        public override string ToggleKeyCombinationCode => "";

        public void ComposeDialog()
        {
            ItemSlot hoveredSlot = capi.World.Player.InventoryManager.CurrentHoveredSlot;
            if (hoveredSlot != null && hoveredSlot.Inventory == Inventory)
            {
                capi.Input.TriggerOnMouseLeaveSlot(hoveredSlot);
            }
            else
            {
                hoveredSlot = null;
            }
            double marginLeft = 20L;
            var dialogBounds = ElementBounds.Fixed(20, 20, 400, 400);

            ElementBounds inputSlotBounds = ElementBounds.Fixed(0, 20, 50, 50);
            ElementBounds labelBounds = ElementBounds.Fixed(60, 20, 200, 25);
            ElementBounds dropdownBounds = ElementBounds.Fixed(60, 42, 217, 25);
            ElementBounds buttonBounds = ElementBounds.Fixed(0, 85, 200, 40);
            ElementBounds outputSlotBounds = ElementBounds.Fixed(230, 80, 50, 50);
            ElementBounds titleBarBounds = ElementBounds.Fixed(0, 0, 318, GuiStyle.TitleBarHeight);
            ElementBounds insetBounds = ElementBounds.Fixed(1, 140, 275, 50);
            ElementBounds messageBounds = ElementBounds.Fixed(10, 150, 255, 30);

            var fconf = new FontConfig();
            fconf.Fontname = "Arial";
            fconf.UnscaledFontsize = 11L;
            var f = new CairoFont(fconf);
            f.Color = PrimaryColor;
            var fb = f.Clone();
            fb.FontWeight = Cairo.FontWeight.Bold;

            var fup = fb.Clone();
            fup.UnscaledFontsize = 13L;

            ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
            bgBounds.BothSizing = ElementSizing.FitToChildren;
            bgBounds.WithChildren(inputSlotBounds, dropdownBounds, labelBounds, outputSlotBounds, buttonBounds, messageBounds, insetBounds);

            var dataPlayer = GetConnectedPlayer();
            ClearComposers();


            this.SingleComposer = capi.Gui.CreateCompo("anvilDialog", dialogBounds)
                .AddShadedDialogBG(bgBounds, true, 0, 1)
                .AddDialogTitleBarWithBg(Lang.Get(LangContants.GuiTemporalWorkshopTitle), OnTitleBarClose, fup, bounds: titleBarBounds)
                .AddItemSlotGrid(Inventory, SendInvPacket, 1, new int[] { 0 }, inputSlotBounds, SLOT_INPUT_ID)
                .AddItemSlotGrid(Inventory, SendInvPacket, 1, new int[] { 1 }, outputSlotBounds, SLOT_OUTPUT_ID)
                .AddStaticText(Lang.Get(LangContants.SelectTargetConfiguration), fb, labelBounds)
                .AddDropDown(
                    dataPlayer.Item1.ToArray(),
                    dataPlayer.Item2.ToArray(),
                    -1,
                    OnPlayerSelected,
                    dropdownBounds,
                    f.WithFontSize(11L),
                    key: DROPDOWN_ELEMENT_ID
                )
                .AddButton(Lang.Get(LangContants.ButConfigureDispositif), OnEngraveClick, buttonBounds, fb.WithOrientation(EnumTextOrientation.Center), EnumButtonStyle.Small, buttonKey)
                .AddInset(insetBounds)
                .AddDynamicText(Lang.Get(LangContants.TranslocationDispoInfo1), f, messageBounds, MESSAGE_ELEMENT_ID)
                .Compose();
            //CheckTeleportationRuneOk(sourceInv, resultInv);
        }
        private void SendInvPacket(object p)
        {
            capi.Network.SendBlockEntityPacket(BlockEntityPosition.X, BlockEntityPosition.Y, BlockEntityPosition.Z, p);
        }

        public int CheckTeleportationRuneOk(InventoryGeneric inventorySlot1, DummyInventory inventorySlot2)
        {
            var slot1 = inventorySlot1[0];
            var slot2 = inventorySlot2[0];
            if (slot1.Empty) return -1;
            var targetKey = slot1.Itemstack.Attributes?.GetString(KeyContants.DispositifTargetKey);
            if (!string.IsNullOrEmpty(targetKey))
            {
                slot2.Itemstack = slot1.Itemstack.Clone();
                slot1.Itemstack = null;

                inventorySlot1.MarkSlotDirty(0);
                inventorySlot2.MarkSlotDirty(0);
                return 1;
            }

            return 0;
        }

        private Tuple<List<string>, List<string>> GetConnectedPlayer()
        {
            List<string> playerNames = new List<string>();
            List<string> playerIds = new List<string>();
            if (config.CanConfigureForToNorth)
            {
                playerNames.Add(Lang.Get(LangContants.Targetnorth));
                playerIds.Add(EnumTargetLocation.North.ToString());
            }
            if (config.CanConfigureForToSouth)
            {
                playerNames.Add(Lang.Get(LangContants.Targetsouth));
                playerIds.Add(EnumTargetLocation.South.ToString());
            }
            if (config.CanConfigureForToEast)
            {
                playerNames.Add(Lang.Get(LangContants.Targeteast));
                playerIds.Add(EnumTargetLocation.East.ToString());
            }
            if (config.CanConfigureForToWest)
            {
                playerNames.Add(Lang.Get(LangContants.Targetwest));
                playerIds.Add(EnumTargetLocation.West.ToString());
            }
            if (config.CanConfigureForRandom)
            {
                playerNames.Add(Lang.Get(LangContants.Targetrandom));
                playerIds.Add(EnumTargetLocation.Random.ToString());
            }
            if (config.CanConfigureForSpawn)
            {
                playerNames.Add(Lang.Get(LangContants.Targetspawn));
                playerIds.Add(EnumTargetLocation.Spawn.ToString());
            }
            if (config.CanConfigureForSpawnPlayer)
            {
                playerNames.Add(Lang.Get(LangContants.Targetplayerspawn));
                playerIds.Add(EnumTargetLocation.SpawnPlayer.ToString());
            }
            if (config.CanConfigureForPlayer)
            {
                playerNames.Add("");
                playerIds.Add("");
                foreach (var player in _capi.World.AllOnlinePlayers)
                {
                    playerNames.Add(player.PlayerName);
                    playerIds.Add(player.PlayerUID);
                }
            }
            return new Tuple<List<string>, List<string>>(playerIds, playerNames);
        }

        private bool OnEngraveClick()
        {
            if (string.IsNullOrEmpty(SelectedTarget) || Inventory[0]?.Itemstack == null)
                return false;

            capi.Network.SendBlockEntityPacket(BlockEntityPosition, 1001, SerializerUtil.Serialize(SelectedTarget));

            if (SingleComposer != null && SingleComposer.GetDynamicText(MESSAGE_ELEMENT_ID) is GuiElementDynamicText dynamicTextElement)
            {
                dynamicTextElement.SetNewText(Lang.Get(LangContants.TranslocationDispoInfo4));
            }
            return true;
        }

        private void OnPlayerSelected(string selectedKey, bool selected)
        {
            if (selected && !string.IsNullOrEmpty(selectedKey))
            {
                SelectedTarget = selectedKey;
            }
            else
            {
                SelectedTarget = null;
            }
            Inventory_SlotModified(0);
        }

        private void OnTitleBarClose()
        {
            TryClose();
        }

        public override void OnGuiOpened()
        {
            Inventory.SlotModified += Inventory_SlotModified;
        }


        public void Transfert()
        {
            Inventory[1].Itemstack = Inventory[0].Itemstack.Clone();
            Inventory[0].Itemstack = null;

            Inventory.MarkSlotDirty(0);
            Inventory.MarkSlotDirty(1);
        }
        private void Inventory_SlotModified(int obj)
        {
            if (SingleComposer != null && SingleComposer.GetDynamicText(MESSAGE_ELEMENT_ID) is GuiElementDynamicText dynamicTextElement)
            {
                var dropDown = SingleComposer.GetDropDown(DROPDOWN_ELEMENT_ID);
                
                if (Inventory[1].Itemstack != null)
                {
                    dynamicTextElement.SetNewText(Lang.Get(LangContants.TranslocationDispoInfo4));
                }
                else if (Inventory[0].Itemstack != null && (dropDown.SelectedIndices.Length == 0 || (dropDown.SelectedIndices.Length != 0 && dropDown.SelectedValue == "")))
                {
                    dynamicTextElement.SetNewText(Lang.Get(LangContants.TranslocationDispoInfo2));
                }
                else if (Inventory[0].Itemstack != null && dropDown.SelectedIndices.Length != 0 && dropDown.SelectedValue != "")
                {
                    dynamicTextElement.SetNewText(Lang.Get(LangContants.TranslocationDispoInfo3));
                }
                else
                {
                    dynamicTextElement.SetNewText(Lang.Get(LangContants.TranslocationDispoInfo1));
                }
            }
        }

        public override bool TryClose()
        {
            Inventory.SlotModified -= Inventory_SlotModified;

            return base.TryClose();
        }



    }
}
