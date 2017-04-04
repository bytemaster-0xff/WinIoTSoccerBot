using Newtonsoft.Json;
using SoccerBot.Core.Protocols;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.Core.Models
{
    public class NetworkMessage
    {
        public static short NextMessageSerialNumber { get; set; } = 150;

        const byte SOH = 0x01;
        const byte STX = 0x02;
        const byte ETX = 0x03;
        const byte EOT = 0x04;

        

        public short PayloadLength { get; set; }

        public PayloadFormats PayloadFormat { get; set; }

        public byte MessageTypeCode { get; set; }

        /* Very simple and naive approach to authenticating sender, additional security would sit on top of this protocol likely */
        public short Pin { get; set; }

        public short SerialNumber { get; set; }

        public byte[] Payload { get; set; }

        /* Message Check Sum includes everything from the first byte of the PIN to the STX */
        public byte CheckSum { get; set; }

        public static NetworkMessage CreateJSONMessage(object payload, byte messageTypeCode)
        {
            var message = new NetworkMessage();
            message.SerialNumber = NetworkMessage.NextMessageSerialNumber++;
            message.PayloadFormat = PayloadFormats.JSON;
            message.Payload = JsonConvert.SerializeObject(payload).ToByteArray();
            message.PayloadLength = (short)message.Payload.Length;
            message.MessageTypeCode = messageTypeCode;

            return message;
        }

        public static NetworkMessage CreateEmptyMessage(byte messageTypeCode)
        {
            var message = new NetworkMessage();
            message.SerialNumber = NetworkMessage.NextMessageSerialNumber++;
            message.MessageTypeCode = messageTypeCode;
            message.PayloadFormat = PayloadFormats.None;
            message.PayloadLength = 0;

            return message;
        }

        public byte[] GetBuffer()
        {
            var payloadLen = (Payload != null) ? Payload.Length : 0;

            var size = 0;
            size += 1; /* SOH */
            size += 2; /* PIN */
            size += 2; /* SN */
            size += 2; /* LEN */
            size += 1; /* Payload Format */
            size += 1; /* Message Type Code */
            size += 1; /* STX */
            size += payloadLen;
            size += 1; /* ETX */
            size += 1; /* CS */
            size += 1; /* EOT */


            var buffer = new byte[size];
            var idx = 0;
            buffer[idx++] = SOH;
            buffer[idx++] = (byte)(Pin & 0xFF);
            buffer[idx++] = (byte)(Pin >> 8 & 0xFF);
            buffer[idx++] = (byte)(SerialNumber & 0xFF);
            buffer[idx++] = (byte)(SerialNumber >> 8 & 0xFF);
            buffer[idx++] = (byte)(payloadLen & 0xFF);
            buffer[idx++] = (byte)(payloadLen >> 8 & 0xFF);
            buffer[idx++] = (byte)PayloadFormat;
            buffer[idx++] = (byte)MessageTypeCode;
            buffer[idx++] = STX;

            if (Payload != null)
            {
                foreach (var ch in Payload)
                {
                    buffer[idx++] = ch;
                }
            }

            buffer[idx++] = ETX;
            for (var csIdx = 1; csIdx < idx; ++csIdx)
            {
                buffer[idx] += buffer[csIdx];
            }

            idx++;
            buffer[idx++] = EOT;

            return buffer;
        }

        public T DeserializePayload<T>()
        {
            try
            {
                var str = System.Text.UTF8Encoding.UTF8.GetString(Payload, 0, Payload.Length);
                return JsonConvert.DeserializeObject<T>(str);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return default(T);
            }
        }
    }
}
