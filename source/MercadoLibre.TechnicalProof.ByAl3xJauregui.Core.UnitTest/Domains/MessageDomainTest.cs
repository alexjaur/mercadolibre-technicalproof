using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Domains.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.UnitTest.Domains
{
    public class MessageDomainTest
    {
        #region [ SETUP ]

        internal MessageDomain InstanceToTesting { get; }

        public MessageDomainTest()
        {
            // create instance for testing using mocks
            InstanceToTesting = new MessageDomain();
        }

        #endregion

        #region [ DATA (MemberData) ]

        public static IEnumerable<object[]> MemberDataFor__GetMessage_NullOrEmptyArgs_ThrowException
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
                        new string[0][] 
                    },
                    new object[]
                    {
                        new string[][] { }
                    },
                    new object[]
                    {
                        new HashSet<string[]>()
                    },
                    new object[]
                    {
                        new List<string[]>()
                    },
                    new object[]
                    {
                        Enumerable.Empty<string[]>()
                    },
                };
            }
        }

        public static IEnumerable<object[]> MemberDataFor__GetMessage_InvalidArgs_ThrowException
        {
            get
            {
                return new List<object[]>()
                {
                    // Only one item into array
                    new object[]
                    {
                        new string[3][]
                    },
                    
                    // Two items into array
                    new object[]
                    {
                        new string[][]
                        {
                            new string[0],
                            new string[0],
                        }
                    },
                    
                    // More than 3 items into array
                    new object[]
                    {
                        new string[][]
                        {
                            new string[0],
                            new string[0],
                            new string[0],
                            new string[0],
                        }
                    },
                    new object[]
                    {
                        new string[][]
                        {
                            new string[0],
                            new string[0],
                            new string[0],
                            new string[0],
                            new string[0],
                            new string[0],
                        }
                    },
                                        
                    // 3 items into array BUT with null or empty sub-item
                    new object[]
                    {
                        new string[][]
                        {
                            null,
                            null,
                            null,
                        }
                    },
                    new object[]
                    {
                        new string[][]
                        {
                            null,
                            new string[0],
                            new string[0]
                        }
                    },
                    new object[]
                    {
                        new string[][]
                        {
                            new string[] { },
                            new string[] { "A" },
                            new string[] { },
                        }
                    },
                    new object[]
                    {
                        new string[][]
                        {
                            new string[] { },
                            new string[] { "A" },
                            new string[] { "A", "B" },
                        }
                    },
                };
            }

        }

        public static IEnumerable<object[]> MemberDataFor__GetMessage_ValidArgs_ExpectedResult
        {
            get
            {
                return new List<object[]>()
                {
                    new object[]
                    {
                        new string[][]
                        {
                            new string[] { "", "este", "es", "un", "mensaje" },
                            new string[] { "este", "", "un", "mensaje" },
                            new string[] { "", "", "es", "", "mensaje" },
                        },
                        "este es un mensaje"
                    },
                    new object[]
                    {
                        new string[][]
                        {
                            new string[] { "este", "", "otro", "mensaje" },
                            new string[] { "", "este", "es", "otro", "mensaje" },
                            new string[] { "", "", "es", "", "mensaje" },
                        },
                        "este es otro mensaje"
                    },
                    new object[]
                    {
                        new string[][]
                        {
                            new string[] { "esta", "es", "una", "PoC", "por", "Al3x", "", "Jauregui", "", "", ":)" },
                            new string[] { "", "esta", "", "", "PoC", "", "Al3x", "", "Jauregui", "", "MercadoLibre", ":)" },
                            new string[] { "", "", "", "", "por", "", "Cubillos", "", "para", "MercadoLibre", ":)" },
                        },
                        "esta es una PoC por Al3x Cubillos Jauregui para MercadoLibre :)"
                    },
                };
            }

        }

        #endregion


        [Theory]
        [Trait("GetMessage", "ThrowException")]
        [MemberData(nameof(MemberDataFor__GetMessage_NullOrEmptyArgs_ThrowException))]
        public void GetMessage_NullOrEmptyArgs_ThrowException(string[][] messages)
        {
            //assert
            Assert.Throws<ArgumentException>(() =>

                // act
                InstanceToTesting.GetMessage(messages)
            );
        }

        [Theory]
        [Trait("GetMessage", "ThrowException")]
        [MemberData(nameof(MemberDataFor__GetMessage_InvalidArgs_ThrowException))] 
        public void GetMessage_InvalidArgs_ThrowException(string[][] messages)
        {
            //assert
            Assert.Throws<ArgumentException>(() =>

                // act
                InstanceToTesting.GetMessage(messages)
            );
        }

        [Theory]
        [Trait("GetMessage", "Get Results")]
        [MemberData(nameof(MemberDataFor__GetMessage_ValidArgs_ExpectedResult))]
        public void GetMessage_ValidArgs_ExpectedResult(string[][] messages, string expected)
        {
            // act
            var message = InstanceToTesting.GetMessage(messages);

            //assert
            Assert.Equal(expected, message);
        }
    }
}
