using Newtonsoft.Json;
using System.Numerics;
using TonSdk.Modules;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TonSdk.Tests
{
    public class TonSerializerTests
    {
        private readonly TonSerializer _serializer;

        public TonSerializerTests(ITestOutputHelper outputHelper)
        {
            _serializer = new TonSerializer(new XUnitTestLogger(outputHelper));
        }

        [Fact]
        public void Should_Serialize_Null()
        {
            Assert.Equal("null", _serializer.Serialize(null));
        }

        [Fact]
        public void Should_Serialize_Empty_String()
        {
            Assert.Equal("\"\"", _serializer.Serialize(""));
        }

        [Fact]
        public void Should_Serialize_String()
        {
            Assert.Equal("\"test\"", _serializer.Serialize("test"));
        }

        [Fact]
        public void Should_Serialize_Empty_Object()
        {
            Assert.Equal("{}", _serializer.Serialize(new { }));
        }

        [Fact]
        public void Should_Serialize_Simple_Object()
        {
            Assert.Equal("{\"test\":1}",
                _serializer.Serialize(new
                {
                    test = 1
                }));
        }

        [Fact]
        public void Should_Serialize_Complex_Object()
        {
            Assert.Equal("{\"test\":{\"string\":\"value\"}}",
                _serializer.Serialize(new
                {
                    test = new
                    {
                        @string = "value"
                    }
                }));
        }

        [Fact]
        public void Should_Serialize_Simple_Library_Type()
        {
            Assert.Equal("{\"version\":\"1.0.0\"}",
                _serializer.Serialize(new ResultOfVersion
                {
                    Version = "1.0.0"
                }));
        }

        [Fact]
        public void Should_Serialize_Complex_Library_Type()
        {
            Assert.Equal("{\"network\":{\"server_address\":\"https://test:1234\",\"queries_protocol\":\"HTTP\"}}",
                _serializer.Serialize(new ClientConfig
                {
                    Network = new NetworkConfig
                    {
                        ServerAddress = "https://test:1234"
                    }
                }));
        }

        [Fact]
        public void Should_Serialize_Raw_Json()
        {
            Assert.Equal("{\"function_name\":\"test\",\"input\":{\"test\":1}}",
                _serializer.Serialize(new CallSet
                {
                    FunctionName = "test",
                    Input = new { test = 1 }.ToJson()
                }));
        }

        [Fact]
        public void Should_Serialize_Polymorphic_Library_Type_With_No_Properties()
        {
            Assert.Equal("{\"type\":\"AccountId\"}",
                _serializer.Serialize(new AddressStringFormat.AccountId()));
        }

        [Fact]
        public void Should_Serialize_Polymorphic_Library_Type_With_No_Properties_Hex()
        {
            Assert.Equal("{\"type\":\"Hex\"}",
                _serializer.Serialize(new AddressStringFormat.Hex()));
        }

        [Fact]
        public void Should_Serialize_Polymorphic_Library_Type_With_Properties()
        {
            Assert.Equal("{\"url\":false,\"test\":true,\"bounce\":true,\"type\":\"Base64\"}",
                _serializer.Serialize(new AddressStringFormat.Base64
                {
                    Bounce = true,
                    Test = true,
                    Url = false
                }));
        }

        [Fact]
        public void Should_Serialize_Polymorphic_Library_Type_As_Property()
        {
            Assert.Equal("{\"address\":\"123\",\"output_format\":{\"type\":\"Hex\"}}",
                _serializer.Serialize(new ParamsOfConvertAddress
                {
                    Address = "123",
                    OutputFormat = new AddressStringFormat.Hex()
                }));
        }

        [Fact]
        public void Should_Serialize_Polymorphic_Array()
        {
            Assert.Equal("[{\"collection\":\"test\",\"type\":\"QueryCollection\"}]",
                _serializer.Serialize(new ParamsOfQueryOperation[]
                {
                    new ParamsOfQueryOperation.QueryCollection
                    {
                        Collection = "test"
                    },
                }));
        }

        [Fact]
        public void Should_Deserialize_Null()
        {
            Assert.Null(_serializer.Deserialize<string>("null"));
        }

        [Fact]
        public void Should_Deserialize_Empty_String()
        {
            Assert.Equal("", _serializer.Deserialize<string>("\"\""));
        }

        [Fact]
        public void Should_Deserialize_String()
        {
            Assert.Equal("test", _serializer.Deserialize<string>("\"test\""));
        }

        [Fact]
        public void Should_Deserialize_Int()
        {
            Assert.Equal(100, _serializer.Deserialize<int>("100"));
        }

        [Fact]
        public void Should_Deserialize_Simple_Library_Type()
        {
            var version = _serializer.Deserialize<ResultOfVersion>(JsonConvert.SerializeObject(new
            {
                version = "1.0.0"
            }));
            Assert.NotNull(version);
            Assert.Equal("1.0.0", version.Version);
        }

        [Fact]
        public void Should_Deserialize_Complex_Library_Type()
        {
            var result = _serializer.Deserialize<ResultOfProcessMessage>(JsonConvert.SerializeObject(new
            {
                transaction = new
                {
                    test = "foo"
                },
                out_messages = new[]
                {
                    "one",
                    "two"
                },
                decoded = new
                {
                    out_messages = new[]
                    {
                        new
                        {
                            body_type = "Output",
                            name = "one",
                            value = new { test = "value" },
                            header = new
                            {
                                time = 100001,
                                expire = 1000,
                                pubkey = "a"
                            }
                        },
                        new
                        {
                            body_type = "Output",
                            name = "two",
                            value = new { test = "another value" },
                            header = new
                            {
                                time = 200001,
                                expire = 2000,
                                pubkey = "b"
                            }
                        }
                    },
                    output = new
                    {
                        raw = "value"
                    }
                },
                fees = new
                {
                    in_msg_fwd_fee = 1,
                    storage_fee = 2,
                    gas_fee = 3,
                    out_msgs_fwd_fee = 4,
                    total_account_fees = 5,
                    total_output = 6
                }
            }));

            Assert.NotNull(result);
            Assert.NotNull(result.Transaction);
            Assert.Equal("foo", result.Transaction.Value<string>("test"));

            Assert.NotEmpty(result.OutMessages);
            Assert.Equal(new[] { "one", "two" }, result.OutMessages);

            Assert.NotNull(result.Decoded);
            Assert.NotNull(result.Decoded.Output);
            Assert.Equal(2, result.Decoded.OutMessages.Length);

            Assert.Equal(MessageBodyType.Output, result.Decoded.OutMessages[0].BodyType);
            Assert.Equal("one", result.Decoded.OutMessages[0].Name);
            Assert.NotNull(result.Decoded.OutMessages[0].Value);
            Assert.Equal("value", result.Decoded.OutMessages[0].Value.Value<string>("test"));
            Assert.Equal(new BigInteger(100001), result.Decoded.OutMessages[0].Header.Time);
            Assert.Equal(1000u, result.Decoded.OutMessages[0].Header.Expire);
            Assert.Equal("a", result.Decoded.OutMessages[0].Header.Pubkey);

            Assert.Equal(MessageBodyType.Output, result.Decoded.OutMessages[1].BodyType);
            Assert.Equal("two", result.Decoded.OutMessages[1].Name);
            Assert.NotNull(result.Decoded.OutMessages[1].Value);
            Assert.Equal("another value", result.Decoded.OutMessages[1].Value.Value<string>("test"));
            Assert.Equal(new BigInteger(200001), result.Decoded.OutMessages[1].Header.Time);
            Assert.Equal(2000u, result.Decoded.OutMessages[1].Header.Expire);
            Assert.Equal("b", result.Decoded.OutMessages[1].Header.Pubkey);

            Assert.NotNull(result.Fees);
            Assert.Equal(1, result.Fees.InMsgFwdFee);
            Assert.Equal(2, result.Fees.StorageFee);
            Assert.Equal(3, result.Fees.GasFee);
            Assert.Equal(4, result.Fees.OutMsgsFwdFee);
            Assert.Equal(5, result.Fees.TotalAccountFees);
            Assert.Equal(6, result.Fees.TotalOutput);
        }

        [Fact]
        public void Should_Deserialize_Polymorphic_Library_Type_To_Concrete_Class()
        {
            var base64 = _serializer.Deserialize<AddressStringFormat.Base64>(JsonConvert.SerializeObject(new
            {
                type = "Base64",
                bounce = true,
                test = true,
                url = false
            }));

            Assert.NotNull(base64);
            Assert.True(base64.Bounce);
            Assert.True(base64.Test);
            Assert.False(base64.Url);
        }

        [Fact]
        public void Should_Deserialize_Polymorphic_Library_Type()
        {
            var addressStringFormat = _serializer.Deserialize<AddressStringFormat>(JsonConvert.SerializeObject(new
            {
                type = "Base64",
                bounce = true,
                test = true,
                url = false
            }));

            Assert.IsType<AddressStringFormat.Base64>(addressStringFormat);

            var base64 = addressStringFormat as AddressStringFormat.Base64;
            Assert.NotNull(base64);
            Assert.True(base64.Bounce);
            Assert.True(base64.Test);
            Assert.False(base64.Url);
        }

        [Fact]
        public void Should_Deserialize_Polymorphic_Array()
        {
            var result = _serializer.Deserialize<ParamsOfQueryOperation[]>(
                "[{\"collection\":\"test\",\"type\":\"QueryCollection\"}]");
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.IsType<ParamsOfQueryOperation.QueryCollection>(result[0]);
            Assert.Equal("test", ((ParamsOfQueryOperation.QueryCollection)result[0]).Collection);
        }

        [Fact]
        public void Should_Deserialize_Inner_Polymorphic_Type()
        {
            var json = @"{
   ""type"": ""Approve"",
   ""activity"": {
     ""type"": ""Transaction"",
     ""msg"": ""te6ccgECEwEAAuAAAueIATBjpe22QNmTNOFJrHgmzax1yWpc+bNru/3uYo99a8iUEZbhD9P0B3vR92w+DZd4XqMSO4XpmGS5wycJUpjnaRq8DnNATP29o9tMb+evpFqc6assx36UGh/Ah9P5kUVqDBDQAAAXiW5qAQYGjG52i1Xz+AMBAQHAAgBD0Bw+0EZhU7i5hOJb77e26D94vRuoKxMiMDVGIWQrvXwBYAIm/wD0pCAiwAGS9KDhiu1TWDD0oQgEAQr0pCD0oQUCA87ABwYALddqJoaf/pn+mAegL8NT/8MPwzfDH8MUAC/3whZGX//CHnhZ/8I2eFgHwlAPoAZPaqQCASAMCQEC/woB/n8h7UTQINdJwgGOE9P/0z/TAPQF+Gp/+GH4Zvhj+GKOG/QFbfhqcAGAQPQO8r3XC//4YnD4Y3D4Zn/4YeLTAAGOEoECANcYIPkBWPhCIPhl+RDyqN7TPwGOHvhDIbkgnzAg+COBA+iogggbd0Cgud6S+GPggDTyNNjTHwH4I7wLADTyudMfIcEDIoIQ/////byx8nwB8AH4R27yfAIBIA4NAOe9Rar5/8ILdHHfaiaBBrpOEAxwnp/+mf6YB6Avw1P/ww/DN8MfwxRw36Arb8NTgAwCB6B3le64X//DE4fDG4fDM//DDxb3wjeTm4/DNo/AA4/CVAgEBkZf+swBB6IZB8NTkAwCBkZf+swBB6Ifw1eAG//DPAIBIBIPAgFYERAAabZAgzo+EFukvAE3tMf0//R+EUgbpIwcN74Qrry4GT4ACH4SiLIy/9ZgCD0Q/hqW/ADf/hngAK22vKmffhBbpLwBN7TH9FwIfhKgCD0DpPXC/+RcOIxATAhwP+OKiPQ0wH6QDAxyM+HIM6NBAAAAAAAAAAAAAAAAAsvKmfYzxYhzwv/yXH7AN4w8AN/+GeAAVN1wItDXCwOpOADcIccA3CHTHyHdIcEDIoIQ/////byx8nwB8AH4R27yfA=="",
     ""dst"": ""0:9831d2f6db206cc99a70a4d63c1366d63ae4b52e7cd9b5ddfef73147beb5e44a"",
     ""out"": [],
     ""fee"": 14884001,
     ""setcode"": false,
     ""signkey"": ""70fb4119854ee2e613896fbededba0fde2f46ea0ac4c88c0d5188590aef5f005""
   }
 }";

            var browserParams = _serializer.Deserialize<ParamsOfAppDebotBrowser>(json);
            var approve = browserParams as ParamsOfAppDebotBrowser.Approve;
            Assert.NotNull(approve);

            var transaction = approve.Activity as DebotActivity.Transaction;
            Assert.NotNull(transaction);
            Assert.IsType<DebotActivity.Transaction>(approve.Activity);
            Assert.Equal("70fb4119854ee2e613896fbededba0fde2f46ea0ac4c88c0d5188590aef5f005", transaction.Signkey);
        }
    }
}
