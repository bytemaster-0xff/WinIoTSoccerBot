using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SoccerBot.Core.Models;
using SoccerBot.Core.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBotTests.Protocol
{
    [TestClass]
    public class MessageParserTests
    {
        public class TestObject
        {
            public String Field1 { get; set; }
            public int Field2 { get; set; }
        }

        [TestMethod]
        public void MessageParser()
        {
            var payload = new TestObject() { Field1 = "HelloWorld", Field2 = 123 };

            var message = NetworkMessage.CreateJSONMessage(payload, 101);
            message.Pin = 1234;
            var buffer = message.GetBuffer();

            var parser = new MessageParser();
            parser.MessageReady += (e, args) =>
            {
                var parstedInstance = args.DeserializePayload<TestObject>();
                Assert.AreEqual(payload.Field1, parstedInstance.Field1);
                Assert.AreEqual(payload.Field2, parstedInstance.Field2);
            };
            parser.Parse(buffer);
        }
    }
}
