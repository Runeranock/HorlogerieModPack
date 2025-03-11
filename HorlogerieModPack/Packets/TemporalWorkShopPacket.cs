using ProtoBuf;

namespace HorlogerieModPack
{
    [ProtoContract]
    public class TemporalWorkShopPacket
    {
        [ProtoMember(1)]
        public string ItemSlot { get; set; }
    }
}
