using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace HorlogerieModPack
{
    public class AnimationContainer
    {
        [JsonProperty("animations")]
        public List<AnimationData> Animations { get; set; }
    }

    public class AnimationData
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("quantityframes")]
        public int QuantityFrames { get; set; }

        [JsonProperty("keyframes")]
        public List<Keyframe> Keyframes { get; set; }

        public Keyframe GetKeyframe(int frame)
        {
            return Keyframes.Find(k => k.Frame == frame) ?? Keyframes[0];
        }
    }

    public class Keyframe
    {
        [JsonProperty("frame")]
        public int Frame { get; set; }

        [JsonProperty("elements")]
        public Dictionary<string, TransformData> Elements { get; set; }
    }

    public class TransformData
    {
        [JsonProperty("rotationX")]
        public float RotationX { get; set; }

        [JsonProperty("rotationY")]
        public float RotationY { get; set; }

        [JsonProperty("rotationZ")]
        public float RotationZ { get; set; }
    }
    public class TranslocationDispositif : Item
    {
        private bool UseCharge;
        private int MaxCharge;
        private bool DestroyAfterUse;
        public static HMPConfig HMPconfig;
        private Animation currentAnimation;
        public override void OnLoaded(ICoreAPI api)
        {
            base.OnLoaded(api);

            UseCharge = api.World.Config.GetBool(nameof(HMPConfigDefaults.UseCharge));
            MaxCharge = api.World.Config.GetInt(nameof(HMPConfigDefaults.MaxCharge));
            DestroyAfterUse = api.World.Config.GetBool(nameof(HMPConfigDefaults.DestroyAfterUse));

            Shape shape = api.Assets.TryGet("horlogeriemodpack:shapes/translocationdispositif.json")?.ToObject<Shape>();
            var animationsContainer = api.Assets.TryGet("horlogeriemodpack:shapes/translocationdispositif.animation.json")?.ToObject<JObject>();

            if (animationsContainer != null && animationsContainer.TryGetValue("animations", out var animationsToken))
            {
                var animations = animationsToken.ToObject<Animation[]>();
                currentAnimation = animations[0];
            }
           
        }
        //public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
        //{
        //    base.OnBeforeRender(capi, itemstack, target, ref renderinfo);

        //    if (currentAnimation != null)
        //    {
        //        ApplyAnimation(ref renderinfo);
        //        animationFrame += 1f; // Avancer l'animation
        //        if (animationFrame >= currentAnimation.KeyFrames.Count()) // Adapter au nombre de frames de l'animation
        //        {
        //            animationFrame = 0f; // Boucle infinie
        //        }
        //    }
        //}
        //private void ApplyAnimation(ref ItemRenderInfo renderInfo)
        //{
        //    var keyframe = currentAnimation.KeyFrames[(int)animationFrame];

        //    // Si le keyframe contient une transformation, applique-la
        //    if (keyframe.Elements.ContainsKey("rotation"))
        //    {
        //        var rotation = keyframe.Elements["rotation"]; 
        //        renderInfo.Transform.Rotation = new Vec3f((float)rotation.RotationX.Value, (float)rotation.RotationY.Value, (float)rotation.RotationZ.Value);
        //    }
        //}
        
        //private float animationFrame = 0f;
        public override void InGuiIdle(IWorldAccessor world, ItemStack stack)
        {
            //Rotation dans le slot
            //GuiTransform.Origin.X = 0.5F;
            //GuiTransform.Origin.Z = 0.5F;
            //GuiTransform.Rotation.Y = GameMath.Mod(world.ElapsedMilliseconds / 50f, 360);
        }


        public override void GetHeldItemInfo(ItemSlot slot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
        {

            string targetdesignation = Lang.Get(LangContants.Targetnotconfigured);
            string targetKey = slot.Itemstack.Attributes.GetString(KeyContants.DispositifTargetKey, "");
            // Ajout d'une ligne de texte avec le nom de la cible
            switch (targetKey)
            {
                case "North":
                    targetdesignation = Lang.Get(LangContants.Targetnorth);
                    break;
                case "South":
                    targetdesignation = Lang.Get(LangContants.Targetsouth);
                    break;
                case "East":
                    targetdesignation = Lang.Get(LangContants.Targeteast);
                    break;
                case "West":
                    targetdesignation = Lang.Get(LangContants.Targetwest);
                    break;
                case "Spawn":
                    targetdesignation = Lang.Get(LangContants.Targetspawn);
                    break;
                case "Random":
                    targetdesignation = Lang.Get(LangContants.Targetrandom);
                    break;
                case "SpawnPlayer":
                    targetdesignation = Lang.Get(LangContants.Targetplayerspawn);
                    break;
                case "":
                    return;
                default:
                    var player = world.PlayerByUid(targetKey);
                    targetdesignation = $"{Lang.Get(LangContants.Targetto)} {player.PlayerName}";
                    break;

            }
            base.GetHeldItemInfo(slot, dsc, world, withDebugInfo);
            dsc.AppendLine($"<font color=\"#A0B8C4\" size=\"11\">{Lang.Get(LangContants.ConfigurationText)} : </font><font color=\"#A8D08D\" size=\"11\">{targetdesignation}</font>");
            if (UseCharge)
            {

                var charges = slot.Itemstack.Attributes.GetInt(KeyContants.DispositifChargeKey);
                dsc.AppendLine($"<font color=\"#A0B8C4\" size=\"11\">{Lang.Get(LangContants.DispositifCharges)} : </font><font color=\"#A8D08D\" size=\"11\">{charges}/{MaxCharge}</font>");
            }
        }
        
        public override string GetHeldItemName(ItemStack itemstack)
        {
            return Lang.Get(LangContants.DispositifName);
        }

        public override void OnCreatedByCrafting(ItemSlot[] allInputslots, ItemSlot outputSlot, GridRecipe byRecipe)
        {
            base.OnCreatedByCrafting(allInputslots, outputSlot, byRecipe);
            this.Attributes[KeyContants.DispositifChargeKey].Token = HMPConfigDefaults.MaxCharge;
            outputSlot.Itemstack.Attributes.SetInt(KeyContants.DispositifChargeKey, HMPConfigDefaults.MaxCharge);
        }

        public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
        {
            base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handling);
            handling = EnumHandHandling.Handled;

        }
        private void DestroyItem(ItemSlot slot)
        {
            slot.TakeOut(slot.StackSize);
            slot.MarkDirty();
        }
        private void SendPacket(ItemSlot slot, EntityAgent byEntity, ICoreClientAPI clientApi)
        {
            string targetKey = slot.Itemstack.Attributes.GetString(KeyContants.DispositifTargetKey, "");
            if (byEntity is EntityPlayer playerEntity && !string.IsNullOrEmpty(targetKey))
            {
                IPlayer player = playerEntity.Player;
                clientApi.Network.GetChannel(ChannelContants.ChannelDispositifDT).SendPacket(new DispositifDeTranslocationPacket
                {
                    PlayerUID = player.PlayerUID,
                    TargetKey = targetKey
                });


            }
        }
        public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
        {
            base.OnHeldInteractStop(secondsUsed, slot, byEntity, blockSel, entitySel);
            oneByOne = 0;
        }
        int oneByOne = 0;
        public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
        {
            if (byEntity.World is IClientWorldAccessor)
            {
                var clientApi = byEntity.World.Api as ICoreClientAPI;

                //ModelTransform tf = new ModelTransform();
                //tf.EnsureDefaultValues();

                //tf.Origin.Set(0, -1, 0);
                //tf.Rotation.Z = Math.Min(30, secondsUsed * 40);
                string targetKey = slot.Itemstack.Attributes.GetString(KeyContants.DispositifTargetKey, "");
                if (string.IsNullOrEmpty(targetKey))
                    return false;
                if ( secondsUsed > 0 && secondsUsed<1)
                {
                    if (oneByOne == 0)
                    {
                        oneByOne = 1;
                        clientApi.ShowChatMessage("3");
                        
                    }
                }
                else if (secondsUsed > 1 && secondsUsed < 2)
                {
                    if (oneByOne == 1)
                    {
                        oneByOne = 2;
                        clientApi.ShowChatMessage("2");
                    }
                }
                else if (secondsUsed > 2 && secondsUsed < 3)
                {
                    if (oneByOne == 2)
                    {
                        oneByOne = 3;
                        clientApi.ShowChatMessage("1");
                    }
                }
                if (secondsUsed > 3)
                {
                    if (oneByOne==3)
                    {
                        clientApi.ShowChatMessage("0");
                        oneByOne = 4;
                        SendPacket(slot, byEntity, clientApi);
                    }
                }
            }
            return true;
        }
    }
}
