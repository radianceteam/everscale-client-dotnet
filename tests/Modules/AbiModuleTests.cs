using System;
using System.Numerics;
using System.Threading.Tasks;
using TonSdk.Modules;
using Xunit;
using Xunit.Abstractions;

namespace TonSdk.Tests.Modules
{
    public class AbiModuleTests : IDisposable
    {
        private readonly ITonClient _client;
        private readonly Abi _abi;
        private readonly string _tvc;

        private readonly KeyPair _keys = new KeyPair
        {
            Public = "4c7c408ff1ddebb8d6405ee979c716a14fdd6cc08124107a61d3c25597099499",
            Secret = "cc8929d635719612a9478b9cd17675a39cfad52d8959e8a177389b8c0b9122a7"
        };

        public AbiModuleTests(ITestOutputHelper outputHelper)
        {
            _client = TestClient.Create(new XUnitTestLogger(outputHelper));
            (_abi, _tvc) = TestClient.Package("Events", 2);
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        [Fact]
        public async Task Should_Encode_Message_Constructor_External()
        {
            var unsigned = await _client.Abi.EncodeMessageAsync(new ParamsOfEncodeMessage
            {
                Abi = _abi,
                DeploySet = new DeploySet
                {
                    Tvc = _tvc
                },
                CallSet = new CallSet
                {
                    FunctionName = "constructor",
                    Header = new FunctionHeader
                    {
                        Time = new BigInteger(1599458364291),
                        Expire = 1599458404
                    }
                },
                Signer = new Signer.External
                {
                    PublicKey = "4c7c408ff1ddebb8d6405ee979c716a14fdd6cc08124107a61d3c25597099499"
                }
            });

            Assert.NotNull(unsigned);
            Assert.Equal(
                "te6ccgECFwEAA2gAAqeIAAt9aqvShfTon7Lei1PVOhUEkEEZQkhDKPgNyzeTL6YSEZTHxAj/Hd67jWQF7peccWoU/dbMCBJBB6YdPCVZcJlJkAAAF0ZyXLg19VzGRotV8/gGAQEBwAICA88gBQMBAd4EAAPQIABB2mPiBH+O713GsgL3S844tQp+62YECSCD0w6eEqy4TKTMAib/APSkICLAAZL0oOGK7VNYMPShCQcBCvSkIPShCAAAAgEgDAoByP9/Ie1E0CDXScIBjhDT/9M/0wDRf/hh+Gb4Y/hijhj0BXABgED0DvK91wv/+GJw+GNw+GZ/+GHi0wABjh2BAgDXGCD5AQHTAAGU0/8DAZMC+ELiIPhl+RDyqJXTAAHyeuLTPwELAGqOHvhDIbkgnzAg+COBA+iogggbd0Cgud6S+GPggDTyNNjTHwH4I7zyudMfAfAB+EdukvI83gIBIBINAgEgDw4AvbqLVfP/hBbo417UTQINdJwgGOENP/0z/TANF/+GH4Zvhj+GKOGPQFcAGAQPQO8r3XC//4YnD4Y3D4Zn/4YeLe+Ebyc3H4ZtH4APhCyMv/+EPPCz/4Rs8LAMntVH/4Z4AgEgERAA5biABrW/CC3Rwn2omhp/+mf6YBov/ww/DN8Mfwxb30gyupo6H0gb+j8IpA3SRg4b3whXXlwMnwAZGT9ghBkZ8KEZ0aCBAfQAAAAAAAAAAAAAAAAACBni2TAgEB9gBh8IWRl//wh54Wf/CNnhYBk9qo//DPAAxbmTwqLfCC3Rwn2omhp/+mf6YBov/ww/DN8Mfwxb2uG/8rqaOhp/+/o/ABkRe4AAAAAAAAAAAAAAAAIZ4tnwOfI48sYvRDnhf/kuP2AGHwhZGX//CHnhZ/8I2eFgGT2qj/8M8AIBSBYTAQm4t8WCUBQB/PhBbo4T7UTQ0//TP9MA0X/4Yfhm+GP4Yt7XDf+V1NHQ0//f0fgAyIvcAAAAAAAAAAAAAAAAEM8Wz4HPkceWMXohzwv/yXH7AMiL3AAAAAAAAAAAAAAAABDPFs+Bz5JW+LBKIc8L/8lx+wAw+ELIy//4Q88LP/hGzwsAye1UfxUABPhnAHLccCLQ1gIx0gAw3CHHAJLyO+Ah1w0fkvI84VMRkvI74cEEIoIQ/////byxkvI84AHwAfhHbpLyPN4=",
                unsigned.Message);

            Assert.Equal("KCGM36iTYuCYynk+Jnemis+mcwi3RFCke95i7l96s4Q=", unsigned.DataToSign);
        }

        [Fact]
        public async Task Should_Attach_Signature()
        {
            var signature = await _client.SignDetachedAsync("KCGM36iTYuCYynk+Jnemis+mcwi3RFCke95i7l96s4Q=", _keys);
            Assert.Equal(
                "6272357bccb601db2b821cb0f5f564ab519212d242cf31961fe9a3c50a30b236012618296b4f769355c0e9567cd25b366f3c037435c498c82e5305622adbc70e",
                signature);

            var signed = await _client.Abi.AttachSignatureAsync(new ParamsOfAttachSignature
            {
                Abi = _abi,
                PublicKey = _keys.Public,
                Message =
                    "te6ccgECFwEAA2gAAqeIAAt9aqvShfTon7Lei1PVOhUEkEEZQkhDKPgNyzeTL6YSEZTHxAj/Hd67jWQF7peccWoU/dbMCBJBB6YdPCVZcJlJkAAAF0ZyXLg19VzGRotV8/gGAQEBwAICA88gBQMBAd4EAAPQIABB2mPiBH+O713GsgL3S844tQp+62YECSCD0w6eEqy4TKTMAib/APSkICLAAZL0oOGK7VNYMPShCQcBCvSkIPShCAAAAgEgDAoByP9/Ie1E0CDXScIBjhDT/9M/0wDRf/hh+Gb4Y/hijhj0BXABgED0DvK91wv/+GJw+GNw+GZ/+GHi0wABjh2BAgDXGCD5AQHTAAGU0/8DAZMC+ELiIPhl+RDyqJXTAAHyeuLTPwELAGqOHvhDIbkgnzAg+COBA+iogggbd0Cgud6S+GPggDTyNNjTHwH4I7zyudMfAfAB+EdukvI83gIBIBINAgEgDw4AvbqLVfP/hBbo417UTQINdJwgGOENP/0z/TANF/+GH4Zvhj+GKOGPQFcAGAQPQO8r3XC//4YnD4Y3D4Zn/4YeLe+Ebyc3H4ZtH4APhCyMv/+EPPCz/4Rs8LAMntVH/4Z4AgEgERAA5biABrW/CC3Rwn2omhp/+mf6YBov/ww/DN8Mfwxb30gyupo6H0gb+j8IpA3SRg4b3whXXlwMnwAZGT9ghBkZ8KEZ0aCBAfQAAAAAAAAAAAAAAAAACBni2TAgEB9gBh8IWRl//wh54Wf/CNnhYBk9qo//DPAAxbmTwqLfCC3Rwn2omhp/+mf6YBov/ww/DN8Mfwxb2uG/8rqaOhp/+/o/ABkRe4AAAAAAAAAAAAAAAAIZ4tnwOfI48sYvRDnhf/kuP2AGHwhZGX//CHnhZ/8I2eFgGT2qj/8M8AIBSBYTAQm4t8WCUBQB/PhBbo4T7UTQ0//TP9MA0X/4Yfhm+GP4Yt7XDf+V1NHQ0//f0fgAyIvcAAAAAAAAAAAAAAAAEM8Wz4HPkceWMXohzwv/yXH7AMiL3AAAAAAAAAAAAAAAABDPFs+Bz5JW+LBKIc8L/8lx+wAw+ELIy//4Q88LP/hGzwsAye1UfxUABPhnAHLccCLQ1gIx0gAw3CHHAJLyO+Ah1w0fkvI84VMRkvI74cEEIoIQ/////byxkvI84AHwAfhHbpLyPN4=",
                Signature = signature
            });

            Assert.NotNull(signed);
            Assert.Equal(
                "te6ccgECGAEAA6wAA0eIAAt9aqvShfTon7Lei1PVOhUEkEEZQkhDKPgNyzeTL6YSEbAHAgEA4bE5Gr3mWwDtlcEOWHr6slWoyQlpIWeYyw/00eKFGFkbAJMMFLWnu0mq4HSrPmktmzeeAboa4kxkFymCsRVt44dTHxAj/Hd67jWQF7peccWoU/dbMCBJBB6YdPCVZcJlJkAAAF0ZyXLg19VzGRotV8/gAQHAAwIDzyAGBAEB3gUAA9AgAEHaY+IEf47vXcayAvdLzji1Cn7rZgQJIIPTDp4SrLhMpMwCJv8A9KQgIsABkvSg4YrtU1gw9KEKCAEK9KQg9KEJAAACASANCwHI/38h7UTQINdJwgGOENP/0z/TANF/+GH4Zvhj+GKOGPQFcAGAQPQO8r3XC//4YnD4Y3D4Zn/4YeLTAAGOHYECANcYIPkBAdMAAZTT/wMBkwL4QuIg+GX5EPKoldMAAfJ64tM/AQwAao4e+EMhuSCfMCD4I4ED6KiCCBt3QKC53pL4Y+CANPI02NMfAfgjvPK50x8B8AH4R26S8jzeAgEgEw4CASAQDwC9uotV8/+EFujjXtRNAg10nCAY4Q0//TP9MA0X/4Yfhm+GP4Yo4Y9AVwAYBA9A7yvdcL//hicPhjcPhmf/hh4t74RvJzcfhm0fgA+ELIy//4Q88LP/hGzwsAye1Uf/hngCASASEQDluIAGtb8ILdHCfaiaGn/6Z/pgGi//DD8M3wx/DFvfSDK6mjofSBv6PwikDdJGDhvfCFdeXAyfABkZP2CEGRnwoRnRoIEB9AAAAAAAAAAAAAAAAAAIGeLZMCAQH2AGHwhZGX//CHnhZ/8I2eFgGT2qj/8M8ADFuZPCot8ILdHCfaiaGn/6Z/pgGi//DD8M3wx/DFva4b/yupo6Gn/7+j8AGRF7gAAAAAAAAAAAAAAAAhni2fA58jjyxi9EOeF/+S4/YAYfCFkZf/8IeeFn/wjZ4WAZPaqP/wzwAgFIFxQBCbi3xYJQFQH8+EFujhPtRNDT/9M/0wDRf/hh+Gb4Y/hi3tcN/5XU0dDT/9/R+ADIi9wAAAAAAAAAAAAAAAAQzxbPgc+Rx5YxeiHPC//JcfsAyIvcAAAAAAAAAAAAAAAAEM8Wz4HPklb4sEohzwv/yXH7ADD4QsjL//hDzws/+EbPCwDJ7VR/FgAE+GcActxwItDWAjHSADDcIccAkvI74CHXDR+S8jzhUxGS8jvhwQQighD////9vLGS8jzgAfAB+EdukvI83g==",
                signed.Message);
        }

        [Fact]
        public async Task Should_Encode_Message_Constructor_Keys()
        {
            var signed = await _client.Abi.EncodeMessageAsync(new ParamsOfEncodeMessage
            {
                Abi = _abi,
                DeploySet = new DeploySet
                {
                    Tvc = _tvc
                },
                CallSet = new CallSet
                {
                    FunctionName = "constructor",
                    Header = new FunctionHeader
                    {
                        Time = new BigInteger(1599458364291),
                        Expire = 1599458404
                    }
                },
                Signer = new Signer.Keys
                {
                    KeysProperty = _keys
                }
            });

            Assert.NotNull(signed);
            Assert.Equal(
                "te6ccgECGAEAA6wAA0eIAAt9aqvShfTon7Lei1PVOhUEkEEZQkhDKPgNyzeTL6YSEbAHAgEA4bE5Gr3mWwDtlcEOWHr6slWoyQlpIWeYyw/00eKFGFkbAJMMFLWnu0mq4HSrPmktmzeeAboa4kxkFymCsRVt44dTHxAj/Hd67jWQF7peccWoU/dbMCBJBB6YdPCVZcJlJkAAAF0ZyXLg19VzGRotV8/gAQHAAwIDzyAGBAEB3gUAA9AgAEHaY+IEf47vXcayAvdLzji1Cn7rZgQJIIPTDp4SrLhMpMwCJv8A9KQgIsABkvSg4YrtU1gw9KEKCAEK9KQg9KEJAAACASANCwHI/38h7UTQINdJwgGOENP/0z/TANF/+GH4Zvhj+GKOGPQFcAGAQPQO8r3XC//4YnD4Y3D4Zn/4YeLTAAGOHYECANcYIPkBAdMAAZTT/wMBkwL4QuIg+GX5EPKoldMAAfJ64tM/AQwAao4e+EMhuSCfMCD4I4ED6KiCCBt3QKC53pL4Y+CANPI02NMfAfgjvPK50x8B8AH4R26S8jzeAgEgEw4CASAQDwC9uotV8/+EFujjXtRNAg10nCAY4Q0//TP9MA0X/4Yfhm+GP4Yo4Y9AVwAYBA9A7yvdcL//hicPhjcPhmf/hh4t74RvJzcfhm0fgA+ELIy//4Q88LP/hGzwsAye1Uf/hngCASASEQDluIAGtb8ILdHCfaiaGn/6Z/pgGi//DD8M3wx/DFvfSDK6mjofSBv6PwikDdJGDhvfCFdeXAyfABkZP2CEGRnwoRnRoIEB9AAAAAAAAAAAAAAAAAAIGeLZMCAQH2AGHwhZGX//CHnhZ/8I2eFgGT2qj/8M8ADFuZPCot8ILdHCfaiaGn/6Z/pgGi//DD8M3wx/DFva4b/yupo6Gn/7+j8AGRF7gAAAAAAAAAAAAAAAAhni2fA58jjyxi9EOeF/+S4/YAYfCFkZf/8IeeFn/wjZ4WAZPaqP/wzwAgFIFxQBCbi3xYJQFQH8+EFujhPtRNDT/9M/0wDRf/hh+Gb4Y/hi3tcN/5XU0dDT/9/R+ADIi9wAAAAAAAAAAAAAAAAQzxbPgc+Rx5YxeiHPC//JcfsAyIvcAAAAAAAAAAAAAAAAEM8Wz4HPklb4sEohzwv/yXH7ADD4QsjL//hDzws/+EbPCwDJ7VR/FgAE+GcActxwItDWAjHSADDcIccAkvI74CHXDR+S8jzhUxGS8jvhwQQighD////9vLGS8jzgAfAB+EdukvI83g==",
                signed.Message);
        }

        [Fact]
        public async Task Should_Encode_Message_ReturnValues_External()
        {
            var unsigned = await _client.Abi.EncodeMessageAsync(new ParamsOfEncodeMessage
            {
                Abi = _abi,
                Address = "0:05beb555e942fa744fd96f45a9ea9d0a8248208ca12421947c06e59bc997d309",
                CallSet = new CallSet
                {
                    FunctionName = "returnValue",
                    Header = new FunctionHeader
                    {
                        Time = new BigInteger(1599458364291),
                        Expire = 1599458404
                    },
                    Input = new
                    {
                        id = "0"
                    }.ToJson()
                },
                Signer = new Signer.External
                {
                    PublicKey = _keys.Public
                }
            });

            Assert.NotNull(unsigned);
            Assert.Equal(
                "te6ccgEBAgEAeAABpYgAC31qq9KF9Oifst6LU9U6FQSQQRlCSEMo+A3LN5MvphIFMfECP8d3ruNZAXul5xxahT91swIEkEHph08JVlwmUmQAAAXRnJcuDX1XMZBW+LBKAQBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=",
                unsigned.Message);

            Assert.Equal("i4Hs3PB12QA9UBFbOIpkG3JerHHqjm4LgvF4MA7TDsY=", unsigned.DataToSign);
        }

        [Fact]
        public async Task Should_Attach_Signature2()
        {
            var signature = await _client.SignDetachedAsync("i4Hs3PB12QA9UBFbOIpkG3JerHHqjm4LgvF4MA7TDsY=", _keys);
            Assert.Equal(
                "5bbfb7f184f2cb5f019400b9cd497eeaa41f3d5885619e9f7d4fab8dd695f4b3a02159a1422996c1dd7d1be67898bc79c6adba6c65a18101ac5f0a2a2bb8910b",
                signature);

            var signed = await _client.Abi.AttachSignatureAsync(new ParamsOfAttachSignature
            {
                Abi = _abi,
                PublicKey = _keys.Public,
                Message =
                    "te6ccgEBAgEAeAABpYgAC31qq9KF9Oifst6LU9U6FQSQQRlCSEMo+A3LN5MvphIFMfECP8d3ruNZAXul5xxahT91swIEkEHph08JVlwmUmQAAAXRnJcuDX1XMZBW+LBKAQBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=",
                Signature = signature
            });

            Assert.NotNull(signed);
            Assert.Equal(
                "te6ccgEBAwEAvAABRYgAC31qq9KF9Oifst6LU9U6FQSQQRlCSEMo+A3LN5MvphIMAQHhrd/b+MJ5Za+AygBc5qS/dVIPnqxCsM9PvqfVxutK+lnQEKzQoRTLYO6+jfM8TF4841bdNjLQwIDWL4UVFdxIhdMfECP8d3ruNZAXul5xxahT91swIEkEHph08JVlwmUmQAAAXRnJcuDX1XMZBW+LBKACAEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==",
                signed.Message);
        }

        [Fact]
        public async Task Should_Encode_Message_ReturnValues_Keys()
        {
            var signed = await _client.Abi.EncodeMessageAsync(new ParamsOfEncodeMessage
            {
                Abi = _abi,
                Address = "0:05beb555e942fa744fd96f45a9ea9d0a8248208ca12421947c06e59bc997d309",
                CallSet = new CallSet
                {
                    FunctionName = "returnValue",
                    Header = new FunctionHeader
                    {
                        Time = new BigInteger(1599458364291),
                        Expire = 1599458404
                    },
                    Input = new
                    {
                        id = "0"
                    }.ToJson()
                },
                Signer = new Signer.Keys
                {
                    KeysProperty = _keys
                }
            });

            Assert.NotNull(signed);
            Assert.Equal(
                "te6ccgEBAwEAvAABRYgAC31qq9KF9Oifst6LU9U6FQSQQRlCSEMo+A3LN5MvphIMAQHhrd/b+MJ5Za+AygBc5qS/dVIPnqxCsM9PvqfVxutK+lnQEKzQoRTLYO6+jfM8TF4841bdNjLQwIDWL4UVFdxIhdMfECP8d3ruNZAXul5xxahT91swIEkEHph08JVlwmUmQAAAXRnJcuDX1XMZBW+LBKACAEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==",
                signed.Message);
        }

        [Fact]
        public async Task Should_Encode_Message_ReturnValues_No_Keys()
        {
            var result = await _client.Abi.EncodeMessageAsync(new ParamsOfEncodeMessage
            {
                Abi = _abi,
                Address = "0:05beb555e942fa744fd96f45a9ea9d0a8248208ca12421947c06e59bc997d309",
                CallSet = new CallSet
                {
                    FunctionName = "returnValue",
                    Header = new FunctionHeader
                    {
                        Time = new BigInteger(1599458364291),
                        Expire = 1599458404
                    },
                    Input = new
                    {
                        id = "0"
                    }.ToJson()
                },
                Signer = new Signer.None()
            });

            Assert.NotNull(result);
            Assert.Equal(
                "te6ccgEBAQEAVQAApYgAC31qq9KF9Oifst6LU9U6FQSQQRlCSEMo+A3LN5MvphIAAAAC6M5Llwa+q5jIK3xYJAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAB",
                result.Message);
        }

        [Fact]
        public async Task Should_Decode_Message_Input()
        {
            var result = await _client.Abi.DecodeMessageAsync(new ParamsOfDecodeMessage
            {
                Abi = _abi,
                Message =
                    "te6ccgEBAwEAvAABRYgAC31qq9KF9Oifst6LU9U6FQSQQRlCSEMo+A3LN5MvphIMAQHhrd/b+MJ5Za+AygBc5qS/dVIPnqxCsM9PvqfVxutK+lnQEKzQoRTLYO6+jfM8TF4841bdNjLQwIDWL4UVFdxIhdMfECP8d3ruNZAXul5xxahT91swIEkEHph08JVlwmUmQAAAXRnJcuDX1XMZBW+LBKACAEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=="
            });

            Assert.NotNull(result);
            Assert.Equal(MessageBodyType.Input, result.BodyType);
            Assert.Equal("returnValue", result.Name);
            Assert.NotNull(result.Value);
            Assert.Equal("0x0000000000000000000000000000000000000000000000000000000000000000",
                result.Value.Value<string>("id"));
            Assert.NotNull(result.Header);
            Assert.Equal(1599458404u, result.Header.Expire);
            Assert.Equal(new BigInteger(1599458364291), result.Header.Time);
            Assert.Equal("4c7c408ff1ddebb8d6405ee979c716a14fdd6cc08124107a61d3c25597099499", result.Header.Pubkey);
        }

        [Fact]
        public async Task Should_Decode_Message_Event()
        {
            var result = await _client.Abi.DecodeMessageAsync(new ParamsOfDecodeMessage
            {
                Abi = _abi,
                Message =
                    "te6ccgEBAQEAVQAApeACvg5/pmQpY4m61HmJ0ne+zjHJu3MNG8rJxUDLbHKBu/AAAAAAAAAMJL6z6ro48sYvAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABA"
            });

            Assert.NotNull(result);
            Assert.Equal(MessageBodyType.Event, result.BodyType);
            Assert.Equal("EventThrown", result.Name);
            Assert.NotNull(result.Value);
            Assert.Equal("0x0000000000000000000000000000000000000000000000000000000000000000",
                result.Value.Value<string>("id"));
            Assert.Null(result.Header);
        }

        [Fact]
        public async Task Should_Decode_Message_Body()
        {
            var result = await _client.Abi.DecodeMessageBodyAsync(new ParamsOfDecodeMessageBody
            {
                Abi = _abi,
                Body =
                    "te6ccgEBAgEAlgAB4a3f2/jCeWWvgMoAXOakv3VSD56sQrDPT76n1cbrSvpZ0BCs0KEUy2Duvo3zPExePONW3TYy0MCA1i+FFRXcSIXTHxAj/Hd67jWQF7peccWoU/dbMCBJBB6YdPCVZcJlJkAAAF0ZyXLg19VzGQVviwSgAQBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=",
                IsInternal = false
            });

            Assert.NotNull(result);
            Assert.Equal(MessageBodyType.Input, result.BodyType);
            Assert.Equal("returnValue", result.Name);
            Assert.NotNull(result.Value);
            Assert.Equal("0x0000000000000000000000000000000000000000000000000000000000000000",
                result.Value.Value<string>("id"));
            Assert.NotNull(result.Header);
            Assert.Equal(1599458404u, result.Header.Expire);
            Assert.Equal(new BigInteger(1599458364291), result.Header.Time);
            Assert.Equal("4c7c408ff1ddebb8d6405ee979c716a14fdd6cc08124107a61d3c25597099499", result.Header.Pubkey);
        }

        [Fact]
        public async Task Should_Decode_Message_Output()
        {
            var result = await _client.Abi.DecodeMessageAsync(new ParamsOfDecodeMessage
            {
                Abi = _abi,
                Message =
                    "te6ccgEBAQEAVQAApeACvg5/pmQpY4m61HmJ0ne+zjHJu3MNG8rJxUDLbHKBu/AAAAAAAAAMKr6z6rxK3xYJAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABA"
            });

            Assert.NotNull(result);
            Assert.Equal(MessageBodyType.Output, result.BodyType);
            Assert.Equal("returnValue", result.Name);
            Assert.NotNull(result.Value);
            Assert.Equal("0x0000000000000000000000000000000000000000000000000000000000000000",
                result.Value.Value<string>("value0"));
            Assert.Null(result.Header);
        }

        [Fact]
        public async Task Should_Encode_Message_Body()
        {
            var signed = await _client.Abi.EncodeMessageBodyAsync(new ParamsOfEncodeMessageBody
            {
                Abi = _abi,
                IsInternal = false,
                CallSet = new CallSet
                {
                    FunctionName = "returnValue",
                    Header = new FunctionHeader
                    {
                        Time = new BigInteger(1599458364291),
                        Expire = 1599458404
                    },
                    Input = new
                    {
                        id = "0"
                    }.ToJson()
                },
                Signer = new Signer.External
                {
                    PublicKey = _keys.Public
                }
            });

            Assert.NotNull(signed);
            Assert.Equal(
                "te6ccgEBAgEAVgABYaY+IEf47vXcayAvdLzji1Cn7rZgQJIIPTDp4SrLhMpMgAAAujOS5cGvquYyCt8WCUABAEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==",
                signed.Body);
        }

        [Fact]
        public async Task Should_Encode_Account()
        {
            var elector = await _client.Abi.EncodeAccountAsync(new ParamsOfEncodeAccount
            {
                StateInit = new StateInitSource.StateInit
                {
                    Code =
                        "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",
                    Data =
                        "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"
                }
            });

            Assert.NotNull(elector);
            Assert.NotEmpty(elector.Account);
            Assert.NotEmpty(elector.Id);
            Assert.Equal(
                "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",
                elector.Account);
        }

        [Theory]
        [MemberData(nameof(GetEncodeMessageInternalDeployData))]
        public async Task Should_Encode_Message_Internal_Deploy(CallSet callSet, string expectedBoc)
        {
            var (abi, tvc) = TestClient.Package("Hello");
            var result = await _client.Abi.EncodeInternalMessageAsync(new ParamsOfEncodeInternalMessage
            {
                Abi = abi,
                DeploySet = new DeploySet
                {
                    Tvc = tvc
                },
                CallSet = callSet,
                Value = "0"
            });

            Assert.Equal(expectedBoc, result.Message);

            var parsed = await _client.Boc.ParseMessageAsync(new ParamsOfParse
            {
                Boc = result.Message
            });

            Assert.NotNull(parsed);

            var codeFromTvc = await _client.Boc.GetCodeFromTvcAsync(new ParamsOfGetCodeFromTvc
            {
                Tvc = tvc
            });

            Assert.Equal(codeFromTvc.Code, parsed.Parsed.Value<string>("code"));
        }

        [Theory]
        [MemberData(nameof(GetEncodeMessageInternalRunData))]
        public async Task Should_Encode_Message_Internal_Run(Abi abi, CallSet callSet, string src, string dst,
            string expectedBoc)
        {
            var result = await _client.Abi.EncodeInternalMessageAsync(new ParamsOfEncodeInternalMessage
            {
                Abi = abi,
                SrcAddress = src,
                Address = dst,
                CallSet = callSet,
                Value = "1000000000",
                Bounce = true
            });

            Assert.Equal(expectedBoc, result.Message);
            if (dst != null)
            {
                Assert.Equal(result.Address, dst);
            }

            var parsed = await _client.Boc.ParseMessageAsync(new ParamsOfParse
            {
                Boc = result.Message
            });

            Assert.NotNull(parsed);
            Assert.Equal("internal", parsed.Parsed.Value<string>("msg_type_name"));
            Assert.Equal(src ?? "", parsed.Parsed.Value<string>("src"));
            Assert.Equal(dst ?? result.Address, parsed.Parsed.Value<string>("dst"));
            Assert.Equal("0x3b9aca00", parsed.Parsed.Value<string>("value"));
            Assert.True(parsed.Parsed.Value<bool>("bounce"));
            Assert.True(parsed.Parsed.Value<bool>("ihr_disabled"));
        }

        public static object[][] GetEncodeMessageInternalDeployData()
        {
            return new[]
            {
                new object[]
                {
                    (CallSet) null,
                    "te6ccgECHAEABGkAAmFiADYO5IoxskLmUfURre2fOB04OmP32VjPwA/lDM/Cpvh8AAAAAAAAAAAAAAAAAAIyBg" +
                    "EBAcACAgPPIAUDAQHeBAAD0CAAQdgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABAIm/wD0pCAiwAGS9" +
                    "KDhiu1TWDD0oQkHAQr0pCD0oQgAAAIBIAwKAej/fyHTAAGOJoECANcYIPkBAXDtRND0BYBA9A7yitcL/wHtRyJv" +
                    "de1XAwH5EPKo3u1E0CDXScIBjhb0BNM/0wDtRwFvcQFvdgFvcwFvcu1Xjhj0Be1HAW9ycG9zcG92yIAgz0DJ0G9" +
                    "x7Vfi0z8B7UdvEyG5IAsAYJ8wIPgjgQPoqIIIG3dAoLneme1HIW9TIO1XMJSANPLw4jDTHwH4I7zyudMfAfFAAQ" +
                    "IBIBgNAgEgEQ4BCbqLVfP4DwH67UdvYW6OO+1E0CDXScIBjhb0BNM/0wDtRwFvcQFvdgFvcwFvcu1Xjhj0Be1HA" +
                    "W9ycG9zcG92yIAgz0DJ0G9x7Vfi3u1HbxaS8jOX7Udxb1btV+IA+ADR+CO1H+1HIG8RMAHIyx/J0G9R7VftR28S" +
                    "yPQA7UdvE88LP+1HbxYQABzPCwDtR28RzxbJ7VRwagIBahUSAQm0ABrWwBMB/O1Hb2FujjvtRNAg10nCAY4W9AT" +
                    "TP9MA7UcBb3EBb3YBb3MBb3LtV44Y9AXtRwFvcnBvc3BvdsiAIM9AydBvce1X4t7tR29lIG6SMHDecO1HbxKAQP" +
                    "QO8orXC/+68uBk+AD6QNEgyMn7BIED6HCBAIDIcc8LASLPCgBxz0D4KBQAjs8WJM8WI/oCcc9AcPoCcPoCgEDPQ" +
                    "Pgjzwsfcs9AIMki+wBfBTDtR28SyPQA7UdvE88LP+1HbxbPCwDtR28RzxbJ7VRwatswAQm0ZfaLwBYB+O1Hb2Fu" +
                    "jjvtRNAg10nCAY4W9ATTP9MA7UcBb3EBb3YBb3MBb3LtV44Y9AXtRwFvcnBvc3BvdsiAIM9AydBvce1X4t7R7Ud" +
                    "vEdcLH8iCEFDL7ReCEIAAAACxzwsfIc8LH8hzzwsB+CjPFnLPQPglzws/gCHPQCDPNSLPMbwXAHiWcc9AIc8XlX" +
                    "HPQSHN4iDJcfsAWyHA/44e7UdvEsj0AO1HbxPPCz/tR28WzwsA7UdvEc8Wye1U3nFq2zACASAbGQEJu3MS5FgaA" +
                    "PjtR29hbo477UTQINdJwgGOFvQE0z/TAO1HAW9xAW92AW9zAW9y7VeOGPQF7UcBb3Jwb3Nwb3bIgCDPQMnQb3Ht" +
                    "V+Le+ADR+CO1H+1HIG8RMAHIyx/J0G9R7VftR28SyPQA7UdvE88LP+1HbxbPCwDtR28RzxbJ7VRwatswAMrdcCH" +
                    "XSSDBII4rIMAAjhwj0HPXIdcLACDAAZbbMF8H2zCW2zBfB9sw4wTZltswXwbbMOME2eAi0x80IHS7II4VMCCCEP" +
                    "////+6IJkwIIIQ/////rrf35bbMF8H2zDgIyHxQAFfBw=="
                },
                new object[]
                {
                    new CallSet
                    {
                        FunctionName = "constructor"
                    },
                    "te6ccgECHAEABG0AAmliADYO5IoxskLmUfURre2fOB04OmP32VjPwA/lDM/Cpvh8AAAAAAAAAAAAAAAAAAIxot" +
                    "V8/gYBAQHAAgIDzyAFAwEB3gQAA9AgAEHYAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQCJv8A9KQgI" +
                    "sABkvSg4YrtU1gw9KEJBwEK9KQg9KEIAAACASAMCgHo/38h0wABjiaBAgDXGCD5AQFw7UTQ9AWAQPQO8orXC/8B" +
                    "7Ucib3XtVwMB+RDyqN7tRNAg10nCAY4W9ATTP9MA7UcBb3EBb3YBb3MBb3LtV44Y9AXtRwFvcnBvc3BvdsiAIM9" +
                    "AydBvce1X4tM/Ae1HbxMhuSALAGCfMCD4I4ED6KiCCBt3QKC53pntRyFvUyDtVzCUgDTy8OIw0x8B+CO88rnTHw" +
                    "HxQAECASAYDQIBIBEOAQm6i1Xz+A8B+u1Hb2FujjvtRNAg10nCAY4W9ATTP9MA7UcBb3EBb3YBb3MBb3LtV44Y9" +
                    "AXtRwFvcnBvc3BvdsiAIM9AydBvce1X4t7tR28WkvIzl+1HcW9W7VfiAPgA0fgjtR/tRyBvETAByMsfydBvUe1X" +
                    "7UdvEsj0AO1HbxPPCz/tR28WEAAczwsA7UdvEc8Wye1UcGoCAWoVEgEJtAAa1sATAfztR29hbo477UTQINdJwgG" +
                    "OFvQE0z/TAO1HAW9xAW92AW9zAW9y7VeOGPQF7UcBb3Jwb3Nwb3bIgCDPQMnQb3HtV+Le7UdvZSBukjBw3nDtR2" +
                    "8SgED0DvKK1wv/uvLgZPgA+kDRIMjJ+wSBA+hwgQCAyHHPCwEizwoAcc9A+CgUAI7PFiTPFiP6AnHPQHD6AnD6A" +
                    "oBAz0D4I88LH3LPQCDJIvsAXwUw7UdvEsj0AO1HbxPPCz/tR28WzwsA7UdvEc8Wye1UcGrbMAEJtGX2i8AWAfjt" +
                    "R29hbo477UTQINdJwgGOFvQE0z/TAO1HAW9xAW92AW9zAW9y7VeOGPQF7UcBb3Jwb3Nwb3bIgCDPQMnQb3HtV+L" +
                    "e0e1HbxHXCx/IghBQy+0XghCAAAAAsc8LHyHPCx/Ic88LAfgozxZyz0D4Jc8LP4Ahz0AgzzUizzG8FwB4lnHPQC" +
                    "HPF5Vxz0EhzeIgyXH7AFshwP+OHu1HbxLI9ADtR28Tzws/7UdvFs8LAO1HbxHPFsntVN5xatswAgEgGxkBCbtzE" +
                    "uRYGgD47UdvYW6OO+1E0CDXScIBjhb0BNM/0wDtRwFvcQFvdgFvcwFvcu1Xjhj0Be1HAW9ycG9zcG92yIAgz0DJ" +
                    "0G9x7Vfi3vgA0fgjtR/tRyBvETAByMsfydBvUe1X7UdvEsj0AO1HbxPPCz/tR28WzwsA7UdvEc8Wye1UcGrbMAD" +
                    "K3XAh10kgwSCOKyDAAI4cI9Bz1yHXCwAgwAGW2zBfB9swltswXwfbMOME2ZbbMF8G2zDjBNngItMfNCB0uyCOFT" +
                    "AgghD/////uiCZMCCCEP////6639+W2zBfB9sw4CMh8UABXwc="
                }
            };
        }

        public static object[][] GetEncodeMessageInternalRunData()
        {
            return new[]
            {
                new object[]
                {
                    TestClient.Abi("Hello"),
                    new CallSet
                    {
                        FunctionName = "sayHello"
                    },
                    null,
                    "0:1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef",
                    "te6ccgEBAQEAOgAAcGIACRorPEhV5veJGis8SFXm94kaKzxIVeb3iRorPEhV5veh3NZQAAAAAAAAAAAAAAAAAABQy+0X"
                },
                new object[]
                {
                    TestClient.Abi("Hello"),
                    new CallSet
                    {
                        FunctionName = "0x50cbed17" // sayHello func hex id
                    },
                    null,
                    "0:1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef",
                    "te6ccgEBAQEAOgAAcGIACRorPEhV5veJGis8SFXm94kaKzxIVeb3iRorPEhV5veh3NZQAAAAAAAAAAAAAAAAAABQy+0X"
                },
                new object[]
                {
                    TestClient.Abi("Hello"),
                    new CallSet
                    {
                        FunctionName = "1355541783" // sayHello func id
                    },
                    null,
                    "0:1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef",
                    "te6ccgEBAQEAOgAAcGIACRorPEhV5veJGis8SFXm94kaKzxIVeb3iRorPEhV5veh3NZQAAAAAAAAAAAAAAAAAABQy+0X"
                },
                // test_encode_internal_message_empty_body
                new object[]
                {
                    null,
                    null,
                    null,
                    "0:1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef",
                    "te6ccgEBAQEANgAAaGIACRorPEhV5veJGis8SFXm94kaKzxIVeb3iRorPEhV5veh3NZQAAAAAAAAAAAAAAAAAAA="
                },
                new object[]
                {
                    null,
                    null,
                    "0:841288ed3b55d9cdafa806807f02a0ae0c169aa5edfe88a789a6482429756a94",
                    "0:1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef",
                    "te6ccgEBAQEAWAAAq2gBCCUR2nars5tfUA0A/gVBXBgtNUvb/RFPE0yQSFLq1SkABI0VniQq83vEjRWeJCrze8SNFZ4kKvN7xI0VniQq83vQ7msoAAAAAAAAAAAAAAAAAABA"
                }
            };
        }

        [Fact]
        public async Task Test_Tips()
        {
            var (abi, tvc) = TestClient.Package("Events");

            var e = await Assert.ThrowsAsync<TonClientException>(async () =>
            {
                await _client.Abi.DecodeMessageAsync(new ParamsOfDecodeMessage
                {
                    Abi = abi,
                    Message =
                        "te6ccgEBAgEAlgAB4a3f2/jCeWWvgMoAXOakv3VSD56sQrDPT76n1cbrSvpZ0BCs0KEUy2Duvo3zPExePONW3TYy0MCA1i+FFRXcSIXTHxAj/Hd67jWQF7peccWoU/dbMCBJBB6YdPCVZcJlJkAAAF0ZyXLg19VzGQVviwSgAQBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA="
                });
            });

            Assert.Contains("Tip: Please check that you have specified the message's BOC, not body, as a parameter.",
                e.Message);

            e = await Assert.ThrowsAsync<TonClientException>(async () =>
            {
                await _client.Abi.DecodeMessageBodyAsync(new ParamsOfDecodeMessageBody
                {
                    Abi = abi,
                    Body =
                        "te6ccgEBAwEAvAABRYgAC31qq9KF9Oifst6LU9U6FQSQQRlCSEMo+A3LN5MvphIMAQHhrd/b+MJ5Za+AygBc5qS/dVIPnqxCsM9PvqfVxutK+lnQEKzQoRTLYO6+jfM8TF4841bdNjLQwIDWL4UVFdxIhdMfECP8d3ruNZAXul5xxahT91swIEkEHph08JVlwmUmQAAAXRnJcuDX1XMZBW+LBKACAEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=="
                });
            });

            Assert.Contains("Tip: Please check that you specified message's body, not full BOC.",
                e.Message);
        }
    }
}
