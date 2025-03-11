using ProtoBuf;

namespace HorlogerieModPack
{
    [ProtoContract]
    public class DispositifDeTranslocationPacket
    {
        [ProtoMember(1)]
        public string PlayerUID { get; set; }
        [ProtoMember(2)]
        public string TargetKey { get; set; }
        [ProtoMember(3)]
        public string PlayerName { get; set; }
    }
}
