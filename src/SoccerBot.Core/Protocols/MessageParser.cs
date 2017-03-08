using Newtonsoft.Json;
using SoccerBot.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.Core.Protocols
{


    public enum PayloadFormats
    {
        JSON = 100,
        CSV = 101,
        ByteArray = 102,
        Text = 103,
        Number = 104,
        None = 105
        /* If Payload Formats are added, make sure you adjust in the parser state machine */
    }

  

    public class MessageParser
    {
        const byte SOH = 0x01;
        const byte STX = 0x02;
        const byte ETX = 0x03;
        const byte EOT = 0x04;


        private enum ParserStates
        {
           
            ExpectingSOH,
            ExpectingPIN1,            
            ExpectingPIN2,
            ExpectingSN1,
            ExpectingSN2,
            ExpectingLEN1,
            ExpectingLEN2,

            ExpectingPayloadFormat,
            ExpectingMessageTypeCode,
            ExpectingSTX,
            ReadingMessage,
            ExpectingETX,
            ExpectingCS,
            ExpectingEOT
        }

        public event EventHandler<NetworkMessage> MessageReady;

        NetworkMessage _currentMessage;

        ParserStates _parserState;
        short _payloadIndex;

        public void Parse(byte[] array)
        {
            foreach(var ch in array)
            {
                try

                {
                    switch (_parserState)
                    {
                        case ParserStates.ExpectingSOH:
                            _currentMessage = new NetworkMessage();
                            _payloadIndex = 0;
                            _parserState = ParserStates.ExpectingPIN1;
                            break;
                        case ParserStates.ExpectingPIN1:
                            _currentMessage.CheckSum += ch;
                            _currentMessage.Pin = ch;
                            _parserState = ParserStates.ExpectingPIN2;
                            break;
                        case ParserStates.ExpectingPIN2:
                            _currentMessage.CheckSum += ch;
                            _currentMessage.Pin |= (short)(ch << 8);
                            _parserState = ParserStates.ExpectingSN1;
                            break;
                        case ParserStates.ExpectingSN1:
                            _currentMessage.CheckSum += ch;
                            _currentMessage.SerialNumber = ch;
                            _parserState = ParserStates.ExpectingSN2;
                            break;
                        case ParserStates.ExpectingSN2:
                            _currentMessage.CheckSum += ch;
                            _currentMessage.SerialNumber |= (short)(ch << 8);
                            _parserState = ParserStates.ExpectingLEN1;
                            break;
                        case ParserStates.ExpectingLEN1:

                            _currentMessage.CheckSum += ch;
                            _currentMessage.PayloadLength = ch;
                            _parserState = ParserStates.ExpectingLEN2;
                            break;
                        case ParserStates.ExpectingLEN2:
                            _currentMessage.CheckSum += ch;
                            _currentMessage.PayloadLength |= (short)(ch << 8);
                            _parserState = ParserStates.ExpectingPayloadFormat;
                            break;
                        case ParserStates.ExpectingPayloadFormat:
                            if (ch < (byte)PayloadFormats.JSON || ch > (byte)PayloadFormats.None) /* If Payload Formats are added, make sure you adjust here */
                            {
                                _currentMessage = null;
                                _parserState = ParserStates.ExpectingSOH;
                            }
                            else
                            {
                                _currentMessage.CheckSum += ch;
                                _currentMessage.PayloadFormat = (PayloadFormats)ch;
                                _parserState = ParserStates.ExpectingMessageTypeCode;
                            }
                            break;
                        case ParserStates.ExpectingMessageTypeCode:
                            _currentMessage.CheckSum += ch;
                            _currentMessage.MessageTypeCode = ch;
                            _parserState = ParserStates.ExpectingSTX;
                            break;
                        case ParserStates.ExpectingSTX:
                            if (ch != STX)
                            {
                                _currentMessage = null;
                                _parserState = ParserStates.ExpectingSOH;
                            }
                            else
                            {
                                _currentMessage.CheckSum += ch;
                                if (_currentMessage.PayloadLength > 0)
                                {
                                    _payloadIndex = 0;
                                    _currentMessage.Payload = new byte[_currentMessage.PayloadLength];
                                    _parserState = ParserStates.ReadingMessage;
                                }
                                else
                                {
                                    _parserState = ParserStates.ExpectingETX;
                                }
                            }
                            break;
                        case ParserStates.ReadingMessage:
                            _currentMessage.CheckSum += ch;

                            _currentMessage.Payload[_payloadIndex++] = ch;
                            if (_payloadIndex == _currentMessage.PayloadLength)
                            {
                                _parserState = ParserStates.ExpectingETX;
                            }
                            break;
                        case ParserStates.ExpectingETX:
                            _currentMessage.CheckSum += ch;
                            if (ch != ETX)
                            {
                                _currentMessage = null;
                                _parserState = ParserStates.ExpectingSOH;
                            }
                            else
                            {
                                _parserState = ParserStates.ExpectingCS; break;
                            }
                            break;
                        case ParserStates.ExpectingCS:
                            var checkSum = ch;
                            if (checkSum != _currentMessage.CheckSum)
                            {
                                _currentMessage = null;
                                _parserState = ParserStates.ExpectingSOH;
                            }
                            else
                            {
                                _parserState = ParserStates.ExpectingEOT;
                            }

                            break;
                        case ParserStates.ExpectingEOT:
                            _parserState = ParserStates.ExpectingSOH;

                            MessageReady?.Invoke(this, _currentMessage);
                            _currentMessage = null;
                            break;

                    }
                }
                catch(Exception)
                {
                    _parserState = ParserStates.ExpectingSOH;
                    _currentMessage = null;
                }

            }

        }

    }
}
