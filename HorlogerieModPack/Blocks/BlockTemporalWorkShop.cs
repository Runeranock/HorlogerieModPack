using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace HorlogerieModPack
{
    public class BlockTemporalWorkshop : Block
    {
        bool hasDownVariant = true;
        public override void OnLoaded(ICoreAPI api)
        {
            base.OnLoaded(api);

            if (this.Attributes?.IsTrue("noDownVariant") == true)
            {
                hasDownVariant = false;
            }

        }
        public override void OnBlockPlaced(IWorldAccessor world, BlockPos pos, ItemStack byItemStack)
        {
            base.OnBlockPlaced(world, pos, byItemStack);
            // Associer le BlockEntity
            BlockEntity be = world.BlockAccessor.GetBlockEntity(pos);
            if (be == null)
            {
                world.BlockAccessor.SpawnBlockEntity(KeyContants.BETemporalWorkshopKey, pos);
            }
        }
        public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            if (world.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityTemporalWorkshop be)
            {
                be.OnPlayerRightClick(byPlayer, blockSel);
                return true; // Empêche les interactions par défaut
            }
            return false;
        }
        public BlockFacing GetHorizontalFacing()
        {
            string[] split = Code.Path.Split('-');
            return BlockFacing.FromCode(split[split.Length - 1]);
        }
        public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
        {
            if (!CanPlaceBlock(world, byPlayer, blockSel, ref failureCode))
            {
                return false;
            }
            BlockFacing[] horVer = SuggestedHVOrientation(byPlayer, blockSel);

            if (blockSel.Face.IsVertical)
            {
                horVer[1] = blockSel.Face;
            }
            else
            {
                horVer[1] = blockSel.HitPosition.Y < 0.5 || !hasDownVariant ? BlockFacing.UP : BlockFacing.DOWN;
            }
            AssetLocation blockCode = CodeWithVariants(new string[] { "verticalorientation", "horizontalorientation" }, new string[] { horVer[1].Code, horVer[0].Code });
            Block block = world.BlockAccessor.GetBlock(blockCode);
            if (block == null) return false;

            world.BlockAccessor.SetBlock(block.BlockId, blockSel.Position);

            return true;
        }

        public override AssetLocation GetRotatedBlockCode(int angle)
        {
            BlockFacing newFacing = BlockFacing.HORIZONTALS_ANGLEORDER[((360 - angle) / 90 + BlockFacing.FromCode(Variant["horizontalorientation"]).HorizontalAngleIndex) % 4];
            return CodeWithVariant("horizontalorientation", newFacing.Code);
        }

    }
}
