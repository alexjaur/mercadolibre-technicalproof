using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Data.Entities;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.DataProviders;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Domains;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Domains.Implementation;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Enums;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Models;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.UnitTest.Domains
{
    public class TopSecretDomainTest
    {
        #region [ SETUP ]

        internal static string ValidMessageWords { get; } = string.Format("este{0}{0}mensaje{0}", TopSecretDomain.Separator);
        internal static ReceivedMessages[] ValidReceivedMessages { get; } = new ReceivedMessages[]
        {
            new ReceivedMessages()
            {
                Id = SatelliteType.Kenobi,
                MessageWords = ValidMessageWords,
                Distance = 100F
            },
            new ReceivedMessages()
            {
                Id = SatelliteType.Skywalker,
                MessageWords = ValidMessageWords,
                Distance = 200F
            },
            new ReceivedMessages()
            {
                Id = SatelliteType.Sato,
                MessageWords = ValidMessageWords,
                Distance = 300F
            }
        };

        internal TopSecretDomain InstanceToTesting { get; }
        internal Mock<ITrilaterationDomain> TrilaterationDomainMock { get; }
        internal Mock<IMessageDomain> MessageDomainMock { get; }
        internal Mock<ITopSecretDataProvider> TopSecretDataProviderMock { get; }


        public TopSecretDomainTest()
        {
            //
            // mocks
            //

            TopSecretDataProviderMock = new Mock<ITopSecretDataProvider>();
            TrilaterationDomainMock = new Mock<ITrilaterationDomain>();
            MessageDomainMock = new Mock<IMessageDomain>();

            TopSecretDataProviderMock
                .Setup(x => x.GetReceivedMessages())
                .Returns(() => ValidReceivedMessages);

            TrilaterationDomainMock
                .Setup(x => x.GetLocation(It.IsAny<DistanceToSatellite[]>()))
                .Returns(() => new LocationPoint(1F, 1F));

            MessageDomainMock
                .Setup(x => x.GetMessage(It.IsAny<string[][]>()))
                .Returns(() => "este es un mensaje");

            //
            // create instance for testing using mocks
            //

            InstanceToTesting = new TopSecretDomain
            (
                TrilaterationDomainMock.Object,
                MessageDomainMock.Object,
                TopSecretDataProviderMock.Object
            );
        }

        #endregion

        #region [ DATA (MemberData) ]

        public static IEnumerable<object[]> MemberDataFor__SaveData_MessagePropIsNullOrEmpty_ThrowException
        {
            get
            {
                return new List<object[]>()
                {
                    new object[]
                    {
                        new MessageReceived()
                    },
                    new object[]
                    {
                        new MessageReceived()
                        {
                            Message = new string[0]
                        }
                    },
                    new object[]
                    {
                        new MessageReceived()
                        {
                            Message = new string[] { }
                        }
                    },
                };
            }
        }

        public static IEnumerable<object[]> MemberDataFor__SaveData_ValidArgs_ExpectedResult
        {
            get
            { 
                return new List<object[]>()
                {
                    new object[]
                    {
                        new MessageReceived()
                        {
                            Message = new string[] { "este", "", "", "mensaje", "" }
                        },
                        true
                    },
                    new object[]
                    {
                        new MessageReceived()
                        {
                            Message = new string[] { "este", "", "", "mensaje", "" }
                        },
                        false
                    },
                };
            }
        }

        public static IEnumerable<object[]> MemberDataFor__GetData_InvalidReceivedMessages_NullExpectedResult
        {
            get
            {
                return new List<object[]>()
                {
                    new object[]
                    {
                        null
                    },
                    new object[]
                    {
                        new ReceivedMessages[0]
                    },
                    new object[]
                    {
                        new ReceivedMessages[]
                        {
                            new ReceivedMessages()
                        }
                    },
                    new object[]
                    {
                        new ReceivedMessages[]
                        {
                            new ReceivedMessages(),
                            new ReceivedMessages()
                        }
                    },
                    new object[]
                    {
                        new ReceivedMessages[]
                        {
                            new ReceivedMessages(),
                            new ReceivedMessages(),
                            new ReceivedMessages(),
                            new ReceivedMessages()
                        }
                    },
                };
            }
        }

        #endregion


        [Fact]
        [Trait("SaveData", "ThrowException")]
        public void SaveData_NullArgument_ThrowException()
        {
            // arrange
            MessageReceived messageReceived = null;

            //assert
            Assert.Throws<ArgumentNullException>(() =>

                // act
                InstanceToTesting.SaveData(messageReceived)
            );
        }

        [Theory]
        [Trait("SaveData", "ThrowException")]
        [MemberData(nameof(MemberDataFor__SaveData_MessagePropIsNullOrEmpty_ThrowException))]
        public void SaveData_MessagePropIsNullOrEmpty_ThrowException(MessageReceived messageReceived)
        {
            //assert
            Assert.Throws<ArgumentException>(() =>

                // act
                InstanceToTesting.SaveData(messageReceived)
            );
        }

        [Theory]
        [Trait("SaveData", "Get Result")]
        [MemberData(nameof(MemberDataFor__SaveData_ValidArgs_ExpectedResult))]
        public void SaveData_ValidArgs_ExpectedResult(MessageReceived messageReceived, bool expectedResult)
        {
            // arrange
            TopSecretDataProviderMock
                .Setup(x => x.AddOrUpdateReceivedMessage(It.IsAny<ReceivedMessages>()))
                .Returns(() => expectedResult);

            // act
            bool result = InstanceToTesting.SaveData(messageReceived);

            //assert
            Assert.Equal(expectedResult, result);
        }


        [Theory]
        [Trait("GetData", "Get Result")]
        [MemberData(nameof(MemberDataFor__GetData_InvalidReceivedMessages_NullExpectedResult))]
        public void GetData_InvalidReceivedMessages_NullExpectedResult(ReceivedMessages[] receivedMessages)
        {
            // arrange
            TopSecretDataProviderMock
                .Setup(x => x.GetReceivedMessages())
                .Returns(() => receivedMessages);

            // act 
            TopSecretData topSecretData = InstanceToTesting.GetData();

            //assert
            Assert.Null(topSecretData);
        }

        [Fact]
        [Trait("GetData", "Get Result")]
        public void GetData_GetLocationThrowException_NullExpectedResult()
        {
            // arrange 
            TrilaterationDomainMock
                .Setup(x => x.GetLocation(It.IsAny<DistanceToSatellite[]>()))
                .Returns(() => throw new Exception());

            // act 
            TopSecretData topSecretData = InstanceToTesting.GetData();

            //assert
            Assert.Null(topSecretData);
        }

        [Fact]
        [Trait("GetData", "Get Result")]
        public void GetData_GetLocationNullResult_NullExpectedResult()
        {
            // arrange
            TrilaterationDomainMock
                .Setup(x => x.GetLocation(It.IsAny<DistanceToSatellite[]>()))
                .Returns(() => null);

            // act 
            TopSecretData topSecretData = InstanceToTesting.GetData();

            //assert
            Assert.Null(topSecretData);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [Trait("GetData", "Get Result")]
        public void GetData_GetMessageNullOrEmptyResult_NullExpectedResult(string messageResult)
        {
            // arrange 
            MessageDomainMock
                .Setup(x => x.GetMessage(It.IsAny<string[][]>()))
                .Returns(() => messageResult);

            // act 
            TopSecretData topSecretData = InstanceToTesting.GetData();

            //assert
            Assert.Null(topSecretData);
        }

        [Fact]
        [Trait("GetData", "Get Result")]
        public void GetData_GetMessageThrowException_NullExpectedResult()
        {
            // arrange 
            MessageDomainMock
                .Setup(x => x.GetMessage(It.IsAny<string[][]>()))
                .Returns(() => throw new Exception());

            // act 
            TopSecretData topSecretData = InstanceToTesting.GetData();

            //assert
            Assert.Null(topSecretData);
        }

        [Fact]
        [Trait("GetData", "Get Result")]
        public void GetData_ValidData_NotNullResult()
        {
            // act 
            TopSecretData topSecretData = InstanceToTesting.GetData();

            //assert
            Assert.NotNull(topSecretData);
        }
    }
}
