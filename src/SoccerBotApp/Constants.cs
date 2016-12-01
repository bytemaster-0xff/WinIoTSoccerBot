using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBotApp
{
    public class Constants
    {
        public static readonly Guid RfcommChatServiceUuid = Guid.Parse("34B1CF4D-1069-4AD6-89B6-E161D79BE4D8");



        // The Id of the Service Name SDP attribute

        public const UInt16 SdpServiceNameAttributeId = 0x100;



        // The SDP Type of the Service Name SDP attribute.

        // The first byte in the SDP Attribute encodes the SDP Attribute Type as follows :

        //    -  the Attribute Type size in the least significant 3 bits,

        //    -  the SDP Attribute Type value in the most significant 5 bits.

        public const byte SdpServiceNameAttributeType = (4 << 3) | 5;



        // The value of the Service Name SDP attribute
        public const string SdpServiceName = "Bluetooth Rfcomm Chat Service}";
    }
}
