namespace HorlogerieModPack
{
    public class HMPConfig
    {
        public bool UseCharge { get; set; } = HMPConfigDefaults.UseCharge;
        public int MaxCharge { get; set; } = HMPConfigDefaults.MaxCharge;
        public bool DestroyAfterUse { get; set; } = HMPConfigDefaults.DestroyAfterUse;

        public bool CanConfigureForToNorth { get; set; } = HMPConfigDefaults.CanConfigureForToNorth;
        public int MaxNorthTeleportationRangeZ { get; set; } = HMPConfigDefaults.MaxNorthTeleportationRangeZ;
        public int MinNorthTeleportationRangeZ { get; set; } = HMPConfigDefaults.MinNorthTeleportationRangeZ;
        public int NorthTeleportationRangeX { get; set; } = HMPConfigDefaults.NorthTeleportationRangeX;

        public bool CanConfigureForToSouth { get; set; } = HMPConfigDefaults.CanConfigureForToSouth;
        public int MaxSouthTeleportationRangeZ { get; set; } = HMPConfigDefaults.MaxSouthTeleportationRangeZ;
        public int MinSouthTeleportationRangeZ { get; set; } = HMPConfigDefaults.MinSouthTeleportationRangeZ;
        public int SouthTeleportationRangeX { get; set; } = HMPConfigDefaults.SouthTeleportationRangeX;

        public bool CanConfigureForToEast { get; set; } = HMPConfigDefaults.CanConfigureForToEast;
        public int EastTeleportationRangeZ { get; set; } = HMPConfigDefaults.EastTeleportationRangeZ;
        public int MaxEastTeleportationRangeX { get; set; } = HMPConfigDefaults.MaxEastTeleportationRangeX;
        public int MinEastTeleportationRangeX { get; set; } = HMPConfigDefaults.MinEastTeleportationRangeX;

        public bool CanConfigureForToWest { get; set; } = HMPConfigDefaults.CanConfigureForToWest;
        public int WestTeleportationRangeZ { get; set; } = HMPConfigDefaults.WestTeleportationRangeZ;
        public int MaxWestTeleportationRangeX { get; set; } = HMPConfigDefaults.MaxWestTeleportationRangeX;
        public int MinWestTeleportationRangeX { get; set; } = HMPConfigDefaults.MinWestTeleportationRangeX;

        public bool CanConfigureForRandom { get; set; } = HMPConfigDefaults.CanConfigureForRandom;
        public int MaxRandomTeleportationRangeZ { get; set; } = HMPConfigDefaults.MaxRandomTeleportationRangeZ;
        public int MinRandomTeleportationRangeZ { get; set; } = HMPConfigDefaults.MinRandomTeleportationRangeZ;
        public int MaxRandomTeleportationRangeX { get; set; } = HMPConfigDefaults.MaxRandomTeleportationRangeX;
        public int MinRandomTeleportationRangeX { get; set; } = HMPConfigDefaults.MinRandomTeleportationRangeX;

        public bool CanConfigureForPlayer { get; set; } = HMPConfigDefaults.CanConfigureForPlayer;
        public bool CanConfigureForSpawn { get; set; } = HMPConfigDefaults.CanConfigureForSpawn;
        public bool CanConfigureForSpawnPlayer { get; set; } = HMPConfigDefaults.CanConfigureForSpawnPlayer;
    }
}
