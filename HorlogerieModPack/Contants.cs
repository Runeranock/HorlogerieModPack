using System.Runtime.CompilerServices;

namespace HorlogerieModPack
{
    public static class ChannelContants
    {
        public const string ChannelDispositifDT = "ChannelDispositifDT";
        public const string ChannelTemporalWorkShop = "ChannelTemporalWorkShop";
        public const string ChannelConfirmationTP = "ChannelConfirmationTP";
       
    }
    public static class KeyContants
    {
        public const string TemporalWorkshopKey = "blocktemporalworkshop";
        public const string TranslocationDispositif = "translocationdispositif";
        public const string BETemporalWorkshopKey = "BEblocktemporalworkshop";
        public const string DispositifTargetKey = "targetKey";
        public const string DispositifChargeKey = "chargeattribute";
    }
    public static class LangContants
    {
        public static string Modprefixe = "horlogeriemodpack:";
        public static string DispositifName = $"{Modprefixe}dispositifname";
        public static string BlockWorkshopName = $"{Modprefixe}translocationdispositif";
        public static string ConfigurationText = $"{Modprefixe}configuration";
        public static string Targetnorth = $"{Modprefixe}targetnorth";
        public static string Targetsouth = $"{Modprefixe}targetsouth";
        public static string Targeteast = $"{Modprefixe}targeteast";
        public static string Targetwest = $"{Modprefixe}targetwest";
        public static string Targetrandom = $"{Modprefixe}targetrandom";
        public static string Targetspawn = $"{Modprefixe}targetspawn";
        public static string Targetplayerspawn = $"{Modprefixe}targetplayerspawn";
        public static string Targetnotconfigured = $"{Modprefixe}targetnotconfigured";
        public static string Targetto = $"{Modprefixe}targetto";
        public static string TranslocationDispoInfo1 = $"{Modprefixe}tdinfo1";
        public static string TranslocationDispoInfo2 = $"{Modprefixe}tdinfo2";
        public static string TranslocationDispoInfo3 = $"{Modprefixe}tdinfo3";
        public static string TranslocationDispoInfo4 = $"{Modprefixe}tdinfo4";
        public static string SelectTargetConfiguration = $"{Modprefixe}selecttargetconfiguration";
        public static string ButConfigureDispositif = $"{Modprefixe}butconfiguredispositif";
        public static string GuiTemporalWorkshopTitle = $"{Modprefixe}guitemporalworkshoptitle";
        public static string DispositifCharges = $"{Modprefixe}dispositifcharges";
        public static string DispositifOutOfCharge = $"{Modprefixe}dispositifoutofcharge";
        public static string MsgAuthorizeTpa = $"{Modprefixe}MsgAuthorizeTpa";

    }
    public static class HMPConfigDefaults
    {      
        public const bool UseCharge = false;
        public const int MaxCharge = 5;
        public const bool DestroyAfterUse = true;

        public const bool CanConfigureForToNorth = true;
        public const int MaxNorthTeleportationRangeZ = 5000;
        public const int MinNorthTeleportationRangeZ = 500;
        public const int NorthTeleportationRangeX = 500;

        public const bool CanConfigureForToSouth = true;
        public const int MaxSouthTeleportationRangeZ = 5000;
        public const int MinSouthTeleportationRangeZ = 500;
        public const int SouthTeleportationRangeX = 500;

        public const bool CanConfigureForToEast = true;
        public const int EastTeleportationRangeZ = 500;
        public const int MaxEastTeleportationRangeX = 5000;
        public const int MinEastTeleportationRangeX = 500;

        public const bool CanConfigureForToWest = true;
        public const int WestTeleportationRangeZ = 500;
        public const int MaxWestTeleportationRangeX = 5000;
        public const int MinWestTeleportationRangeX = 500;

        public const bool CanConfigureForRandom = true;
        public const int MaxRandomTeleportationRangeZ = 5000;
        public const int MinRandomTeleportationRangeZ = 5000;
        public const int MaxRandomTeleportationRangeX = 5000;
        public const int MinRandomTeleportationRangeX = 5000;

        public const bool CanConfigureForPlayer = true;
        public const bool CanConfigureForSpawn = true;
        public const bool CanConfigureForSpawnPlayer = true;
    }
}
