using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace HorlogerieModPack.Behavior
{
    internal class AnimatedRenderBehavior : CollectibleBehavior
    {
        private string animationCode;
        private ICoreClientAPI capi;
        private bool autoStart;

        public AnimatedRenderBehavior(CollectibleObject collObj) : base(collObj) { }

        public override void Initialize(JsonObject properties)
        {
            base.Initialize(properties);

            animationCode = properties["animation"].AsString("default");
            autoStart = properties["autoStart"].AsBool(true);
        }

        public override void OnLoaded(ICoreAPI api)
        {
            base.OnLoaded(api);
            api.Logger.Notification($"AnimatedRenderBehavior attaché à {this.animationCode}");
        }

        public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
        {
            base.OnBeforeRender(capi, itemstack, target, ref renderinfo);

            if (autoStart)
            {
                //TODO une amnimation sur un item possible ?

            }
        }
    }
}