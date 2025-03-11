using HorlogerieModPack.Behavior;
using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace HorlogerieModPack
{
    public class HorlogerieModPackModSystem : ModSystem
    {
        ICoreServerAPI serverApi;
        ICoreClientAPI clientApi;

        public static HMPConfig HMPconfig;
        public override void Start(ICoreAPI api)
        {
           
            api.RegisterItemClass(KeyContants.TranslocationDispositif, typeof(TranslocationDispositif));

           api.RegisterBlockClass(KeyContants.TemporalWorkshopKey, typeof(BlockTemporalWorkshop));
           api.RegisterBlockEntityClass(KeyContants.BETemporalWorkshopKey, typeof(BlockEntityTemporalWorkshop));
            api.RegisterCollectibleBehaviorClass("AnimatedRender", typeof(AnimatedRenderBehavior));
            api.Network.RegisterChannel(ChannelContants.ChannelDispositifDT)
                .RegisterMessageType(typeof(DispositifDeTranslocationPacket));
            api.Network.RegisterChannel(ChannelContants.ChannelTemporalWorkShop)
                .RegisterMessageType(typeof(TemporalWorkShopPacket));
        }
        private void GetConfig(ICoreAPI api)
        {
            try
            {
                HMPconfig = api.LoadModConfig<HMPConfig>("horlogeriemodpack.json");
                if (HMPconfig == null)
                {
                    HMPconfig = new HMPConfig();
                    api.StoreModConfig(HMPconfig, "horlogeriemodpack.json");
                }
                api.World.Config.SetBool(nameof(HMPConfigDefaults.UseCharge), HMPconfig.UseCharge);
                api.World.Config.SetInt(nameof(HMPConfigDefaults.MaxCharge), HMPconfig.MaxCharge);
                api.World.Config.SetBool(nameof(HMPConfigDefaults.DestroyAfterUse), HMPconfig.DestroyAfterUse);
            }
            catch (Exception e)
            {
                api.Logger.Error("Impossible de charger la configuration, utilisation des paramètres par défaut.");
                api.Logger.Error(e.Message);
                HMPconfig = new HMPConfig();
            }
        }
        public override void StartClientSide(ICoreClientAPI api)
        {
            clientApi = api;
            api.Network.RegisterChannel("TemporalWorkshopChannel")
        .RegisterMessageType(typeof(byte[]));
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            GetConfig(api);
            base.StartServerSide(api);
            serverApi = api;

            api.Network.GetChannel(ChannelContants.ChannelDispositifDT)
               .SetMessageHandler<DispositifDeTranslocationPacket>(OnTeleportRequestReceived);

            api.Network.RegisterChannel("TemporalWorkshopChannel")
        .RegisterMessageType(typeof(byte[]));

            api.ChatCommands.Create("dispositiftransloctome")
               .WithDescription("Téleleporte a")
               .WithArgs(api.ChatCommands.Parsers.Word("receiverUID"), api.ChatCommands.Parsers.Word("senderUID"), api.ChatCommands.Parsers.Word("tpMsgId"))
               .RequiresPrivilege(Privilege.chat)
               .HandleWith(OnTransloctToMeCommand);
        }


        private TextCommandResult OnTransloctToMeCommand(TextCommandCallingArgs args)
        {
            try
            {
                string receiverUID = args[0].ToString();
                string senderUID = args[1].ToString();
                string tpMsgId = args[2].ToString();
               
                IPlayer receiverPlayer = serverApi.World.PlayerByUid(receiverUID);
                IPlayer senderPlayer = serverApi.World.PlayerByUid(receiverUID);
                var TpMsgId = senderPlayer.Entity.Attributes.GetAsString("TpMsgId");
                //receiverPlayer vers senderPlayer
                EntityPos pos = senderPlayer.Entity.Pos;
                pos.Y = pos.Y + 1;
                if (TpMsgId == tpMsgId)
                {
                    receiverPlayer.Entity.TeleportTo(pos);
                    isTeleporting.Remove(receiverUID);
                    senderPlayer.Entity.Attributes.RemoveAttribute("TpMsgId");
                    return TextCommandResult.Success($"{receiverPlayer.PlayerName} se téléporte a vous !");
                }
                return TextCommandResult.Success($"Téléportation déjà effectuée !");
            }
            catch (Exception ex)
            {
                return TextCommandResult.Error("Commande inconnue.");
            }
        }
        
        private void OnTeleportRequestReceived(IServerPlayer fromPlayer, DispositifDeTranslocationPacket packet)
        {
            var useCharge = serverApi.World.Config.GetBool(nameof(HMPConfigDefaults.UseCharge));
            if (useCharge)
            {
                var slot = fromPlayer.InventoryManager.ActiveHotbarSlot;
                var charges = slot.Itemstack.Attributes.GetInt(KeyContants.DispositifChargeKey);
                slot.Itemstack.Attributes.SetInt(KeyContants.DispositifChargeKey, charges - 1);
                slot.MarkDirty();
            }
            if (serverApi.World.Config.GetBool(nameof(HMPConfigDefaults.DestroyAfterUse)))
            {
                var slot = fromPlayer.InventoryManager.ActiveHotbarSlot;
                if (useCharge)
                {
                    var charges = slot.Itemstack.Attributes.GetInt(KeyContants.DispositifChargeKey);
                    if (charges <= 0)
                    {
                        slot.Itemstack = null;
                        slot.MarkDirty();
                    }
                }
                else
                {
                    slot.Itemstack = null;
                    slot.MarkDirty();
                }
            }
           
            IPlayer targetPlayer = serverApi.World.PlayerByUid(packet.PlayerUID);

            var playerPos = fromPlayer.Entity.Pos;
            if (!isTeleporting.ContainsKey(packet.PlayerUID))
                isTeleporting.Add(packet.PlayerUID, false);

            var target = packet.TargetKey;
           
            if (!isTeleporting[packet.PlayerUID])
            {
                isTeleporting[packet.PlayerUID] = true;
                var x = 0;
                var z = 0;
                Random random = new Random();
                switch (target)
                {
                    case "North":
                        x = Convert.ToInt32(playerPos.X) + random.Next(HMPconfig.NorthTeleportationRangeX * -1, HMPconfig.NorthTeleportationRangeX);
                        z = Convert.ToInt32(playerPos.Z) - random.Next(HMPconfig.MinRandomTeleportationRangeZ, HMPconfig.MaxRandomTeleportationRangeZ);
                        LoadChunkAndSendPlayer(packet.PlayerUID, x, z);
                        break;
                    case "South":
                        x = Convert.ToInt32(playerPos.X) + random.Next(HMPconfig.SouthTeleportationRangeX * -1, HMPconfig.SouthTeleportationRangeX);
                        z = Convert.ToInt32(playerPos.Z) + random.Next(HMPconfig.MinRandomTeleportationRangeZ, HMPconfig.MaxRandomTeleportationRangeZ);
                        LoadChunkAndSendPlayer(packet.PlayerUID, x, z);
                        break;
                    case "East":
                        x = Convert.ToInt32(playerPos.X) + random.Next(HMPconfig.MinEastTeleportationRangeX, HMPconfig.MaxEastTeleportationRangeX);
                        z = Convert.ToInt32(playerPos.Z) + random.Next(HMPconfig.EastTeleportationRangeZ * -1, HMPconfig.EastTeleportationRangeZ);
                        LoadChunkAndSendPlayer(packet.PlayerUID, x, z);
                        break;
                    case "West":
                        x = Convert.ToInt32(playerPos.X) - random.Next(HMPconfig.MinWestTeleportationRangeX, HMPconfig.MaxWestTeleportationRangeX);
                        z = Convert.ToInt32(playerPos.Z) + random.Next(HMPconfig.WestTeleportationRangeZ * -1, HMPconfig.WestTeleportationRangeZ);
                        LoadChunkAndSendPlayer(packet.PlayerUID, x, z);
                        break;
                    case "Random":
                        x = Convert.ToInt32(playerPos.X) + random.Next(HMPconfig.MinRandomTeleportationRangeX*-1, HMPconfig.MaxRandomTeleportationRangeX);
                        z = Convert.ToInt32(playerPos.Z) + random.Next(HMPconfig.MinRandomTeleportationRangeZ * -1, HMPconfig.MaxRandomTeleportationRangeZ);
                        LoadChunkAndSendPlayer(packet.PlayerUID, x, z);
                        break;
                    case "Spawn":
                        EntityPos spawnPos = serverApi.World.DefaultSpawnPosition;
                        LoadChunkAndSendPlayer(packet.PlayerUID, Convert.ToInt32(spawnPos.X), Convert.ToInt32(spawnPos.Z));
                        break;
                    case "SpawnPlayer":
                        EntityPos spawnPlayerPos = fromPlayer.GetSpawnPosition(false);
                        x = Convert.ToInt32(spawnPlayerPos.X);
                        z = Convert.ToInt32(spawnPlayerPos.Z);
                        LoadChunkAndSendPlayer(packet.PlayerUID, x,z);
                        break;
                    default:
                        SendTeleportMessageWithLink(target, packet.PlayerUID);
                        break;
                }
            }
        }
        private void SendTeleportMessageWithLink(string receiverUID, string senderUID)
        {
            // Trouver le joueur par son nom
            IServerPlayer receiver = serverApi.World.PlayerByUid(receiverUID) as IServerPlayer;
            IServerPlayer sender = serverApi.World.PlayerByUid(senderUID) as IServerPlayer;
            Guid messageID = Guid.NewGuid();
            receiver.Entity.Attributes.SetString("TpMsgId", messageID.ToString());
            if (receiver != null)
            {
                string message = Lang.Get(LangContants.MsgAuthorizeTpa, sender.PlayerName, receiverUID, senderUID, messageID);
                receiver.SendMessage(GlobalConstants.GeneralChatGroup, message, EnumChatType.OwnMessage);
            }
            else
            {
                Console.WriteLine("Joueur non trouvé : " + receiver.PlayerName);
            }
        }
        private Dictionary<string, bool> isTeleporting = new Dictionary<string, bool>();
        private Action Callback(string PlayerUID, int X, int Z)
        {
            IServerPlayer player = serverApi.World.PlayerByUid(PlayerUID) as IServerPlayer;
           
            int? surfaceY = serverApi.WorldManager.GetSurfacePosY(X, Z);
            if (surfaceY != null)
            {
                player.Entity.TeleportTo(X, surfaceY.Value, Z);
                isTeleporting[PlayerUID] = false;
            }
            isTeleporting.Remove(PlayerUID);
            return null;
            
        }
        private void LoadChunkAndSendPlayer(string playerUID, int x, int z)
        {
            var worldManager = serverApi.WorldManager;
            var chunkX = x / worldManager.ChunkSize;
            var chunkZ = z / worldManager.ChunkSize;
            ChunkLoadOptions options = new ChunkLoadOptions
            {
                KeepLoaded = true,
                OnLoaded = () => Callback(playerUID, x, z)
            };
            serverApi.WorldManager.LoadChunkColumnPriority(chunkX, chunkZ, options);

        }

    }
}
