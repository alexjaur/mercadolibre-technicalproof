using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.DataProviders;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Domains.Implementation;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Enums;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Models;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.UnitTest.Domains
{
    public class TrilaterationDomainTest
    {
        #region [ SETUP ]

        internal TrilaterationDomain InstanceToTesting { get; }

        internal Mock<ISatelliteDataProvider> SatelliteDataProviderMock { get; }
        
        internal DistanceToSatellite[] ValidDistanceToSatellites = new DistanceToSatellite[]
        {
            new DistanceToSatellite(SatelliteType.Kenobi, 806.2258F),
            new DistanceToSatellite(SatelliteType.Skywalker, 860.2325F),
            new DistanceToSatellite(SatelliteType.Sato, 1029.563F)
        };

        internal SatelliteLocation[] RealSatelliteLocations = new SatelliteLocation[]
        {
            new SatelliteLocation(SatelliteType.Kenobi, -500, -200),
            new SatelliteLocation(SatelliteType.Skywalker, 100, -100),
            new SatelliteLocation(SatelliteType.Sato, 500, 100)
        };


        public TrilaterationDomainTest()
        {
            // mocks
            SatelliteDataProviderMock = new Mock<ISatelliteDataProvider>();

            SatelliteDataProviderMock
                .Setup(x => x.GetAvailableSatellites())
                .Returns(() => RealSatelliteLocations);

            // create instance for testing using mocks
            InstanceToTesting = new TrilaterationDomain(SatelliteDataProviderMock.Object);
        }


        // Utilities methods
        private static DistanceToSatellite[] GetDistances(float distanceToKenobi, float distanceToSkywalker, float distanceToSato)
        {
            var distances = new DistanceToSatellite[]
            {
                new DistanceToSatellite() { Satellite = SatelliteType.Kenobi, Distance = distanceToKenobi },
                new DistanceToSatellite() { Satellite = SatelliteType.Skywalker, Distance = distanceToSkywalker },
                new DistanceToSatellite() { Satellite = SatelliteType.Sato, Distance = distanceToSato },
            };

            return distances;
        }

        #endregion

        #region [ DATA (MemberData) ]

        public static IEnumerable<object[]> MemberDataFor__GetLocation_NullOrEmptyArgs_ThrowException
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
                        new DistanceToSatellite[0]
                    },
                    new object[]
                    {
                        new HashSet<DistanceToSatellite>()
                    },
                };
            }

        }

        public static IEnumerable<object[]> MemberDataFor__GetLocation_InvalidArgs_ThrowException
        {
            get
            {
                return new List<object[]>()
                {
                    // Only one item into array
                    new object[]
                    {
                        new DistanceToSatellite[]
                        {
                            new DistanceToSatellite()
                        }
                    },
                    
                    // Two items into array
                    new object[]
                    {
                        new DistanceToSatellite[]
                        {
                            new DistanceToSatellite(),
                            new DistanceToSatellite(),
                        }
                    },
                    
                    // More than 3 items into array
                    new object[]
                    {
                        new DistanceToSatellite[]
                        {
                            new DistanceToSatellite(),
                            new DistanceToSatellite(),
                            new DistanceToSatellite(),
                            new DistanceToSatellite(),
                        }
                    },
                    new object[]
                    {
                        new DistanceToSatellite[]
                        {
                            new DistanceToSatellite(),
                            new DistanceToSatellite(),
                            new DistanceToSatellite(),
                            new DistanceToSatellite(),
                            new DistanceToSatellite(),
                            new DistanceToSatellite(),
                        }
                    },
                };
            }

        }

        public static IEnumerable<object[]> MemberDataFor__GetLocation_InvalidAvailableSatellites_ThrowException
        {
            get
            {
                return new List<object[]>()
                {
                    // Different to 3 items (SatelliteLocation) into array or null
                    new object[]
                    {
                        null
                    },
                    new object[]
                    {
                        new SatelliteLocation[0]
                    },
                    new object[]
                    {
                        new SatelliteLocation[]
                        {
                            new SatelliteLocation(),
                            new SatelliteLocation(),
                        }
                    },
                    new object[]
                    {
                        new SatelliteLocation[]
                        {
                            new SatelliteLocation(),
                            new SatelliteLocation(),
                            new SatelliteLocation(),
                            new SatelliteLocation(),
                        }
                    },

                    // Same type of Satellite
                    new object[]
                    {
                        new SatelliteLocation[]
                        {
                            new SatelliteLocation() { Satellite = SatelliteType.Kenobi },
                            new SatelliteLocation() { Satellite = SatelliteType.Kenobi },
                            new SatelliteLocation() { Satellite = SatelliteType.Kenobi },
                        }
                    },
                    new object[]
                    {
                        new SatelliteLocation[]
                        {
                            new SatelliteLocation() { Satellite = SatelliteType.Kenobi },
                            new SatelliteLocation() { Satellite = SatelliteType.Skywalker },
                            new SatelliteLocation() { Satellite = SatelliteType.Skywalker },
                        }
                    },
                    new object[]
                    {
                        new SatelliteLocation[]
                        {
                            new SatelliteLocation() { Satellite = SatelliteType.Sato },
                            new SatelliteLocation() { Satellite = SatelliteType.Skywalker },
                            new SatelliteLocation() { Satellite = SatelliteType.Sato },
                        }
                    },
                };
            }

        }

        public static IEnumerable<object[]> MemberDataFor__GetLocation_BuildCirclesProcessFailed_ThrowException
        {
            get
            {
                //
                // Set Satellite as Unspecified type
                //

                return new List<object[]>()
                {
                    new object[]
                    {
                        new SatelliteLocation[]
                        {
                            new SatelliteLocation() { Satellite = SatelliteType.Sato },
                            new SatelliteLocation() { Satellite = SatelliteType.Unspecified },
                            new SatelliteLocation() { Satellite = SatelliteType.Kenobi },
                        }
                    },
                    new object[]
                    {
                        new SatelliteLocation[]
                        {
                            new SatelliteLocation() { Satellite = SatelliteType.Unspecified },
                            new SatelliteLocation() { Satellite = SatelliteType.Kenobi },
                            new SatelliteLocation() { Satellite = SatelliteType.Skywalker },
                        }
                    },
                    new object[]
                    {
                        new SatelliteLocation[]
                        {
                            new SatelliteLocation() { Satellite = SatelliteType.Sato },
                            new SatelliteLocation() { Satellite = SatelliteType.Skywalker },
                            new SatelliteLocation() { Satellite = SatelliteType.Unspecified },
                        }
                    },
                };
            }

        }

        public static IEnumerable<object[]> MemberDataFor__GetLocation_NoSolutionScenarioCirclesAreTooFarApart__ThrowException
        {
            get
            {
                return new List<object[]>()
                {
                    new object[]
                    {
                        GetDistances(100F, 10F, 1000F)
                    },
                    new object[]
                    {
                        GetDistances(350F, 350F, 5F)
                    },
                    new object[]
                    {
                        GetDistances(320F, 320F, 320F)
                    },
                };
            }

        }

        public static IEnumerable<object[]> MemberDataFor__GetLocation_NoSolutionScenarioOneCircleContainsTheOther__ThrowException
        {
            get
            {
                return new List<object[]>()
                {
                    new object[]
                    {
                        GetDistances(500F, 1120F, 100F)
                    },
                    new object[]
                    {
                        GetDistances(350F, 500F, 1120F)
                    },
                    new object[]
                    {
                        GetDistances(425.9407F, 268.622F, 419.0895F)
                    },
                };
            }

        }

        public static IEnumerable<object[]> MemberDataFor__GetLocation_ValidArgs_ExpectedResult
        {
            get
            {
                return new List<object[]>()
                {
                    new object[]
                    {
                        GetDistances(640.31F, 316.22F, 509.90F),
                        new LocationPoint(0F, 200F)
                    },
                    new object[]
                    {
                        GetDistances(412.31F, 761.57F, 1104.53F),
                        new LocationPoint(-600F, 200F)
                    },
                    new object[]
                    {
                        GetDistances(728.011F, 316.2278F, 583.0952F),
                        new LocationPoint(200F, -400F)
                    },
                    new object[]
                    {
                        GetDistances(647.6917F, 63.2811F, 398.2518F),
                        new LocationPoint(128.5F, -43.5F)
                    },
                    new object[]
                    {
                        GetDistances(1529.7059F, 921.9544F, 500F),
                        new LocationPoint(1000F, 100F)
                    },
                    new object[]
                    {
                        GetDistances(300F, 316.2278F, 761.5773F),
                        new LocationPoint(-200F, -200F)
                    },
                };
            }

        }


        public static IEnumerable<object[]> MemberDataFor__GetLocation2_NullOrEmptyArgs_ThrowException
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
                        new float[0]
                    },
                    new object[]
                    {
                        new float[] { }
                    },
                };
            }

        }

        public static IEnumerable<object[]> MemberDataFor__GetLocation2_InvalidArgs_ThrowException
        {
            get
            {
                return new List<object[]>()
                {
                    // Only one item into array
                    new object[]
                    {
                        new float[]
                        {
                            0.1F
                        }
                    },
                    
                    // Two items into array
                    new object[]
                    {
                        new float[]
                        {
                            0.1F,
                            0.2F,
                        }
                    },
                    
                    // More than 3 items into array
                    new object[]
                    {
                        new float[]
                        {
                            0.1F,
                            0.2F,
                            0.1F,
                            0.2F,
                        }
                    },
                    new object[]
                    {
                        new float[]
                        {
                            0.1F,
                            0.2F,
                            0.1F,
                            0.2F,
                            0.1F,
                            0.2F,
                        }
                    },
                };
            }

        }

        public static IEnumerable<object[]> MemberDataFor__GetLocation2_ValidArgs_ExpectedResult
        {
            get
            {
                return new List<object[]>()
                {
                    new object[]
                    {
                        new float[] { 640.31F, 316.22F, 509.90F },
                        new Tuple<float, float>(0F, 200F)
                    },
                    new object[]
                    {
                        new float[] { 412.31F, 761.57F, 1104.53F },
                        new Tuple<float, float>(-600F, 200F)
                    },
                    new object[]
                    {
                        new float[] { 728.011F, 316.2278F, 583.0952F },
                        new Tuple<float, float>(200F, -400F)
                    },
                    new object[]
                    {
                        new float[] { 647.6917F, 63.2811F, 398.2518F },
                        new Tuple<float, float>(128.5F, -43.5F)
                    },
                    new object[]
                    {
                        new float[] { 1529.7059F, 921.9544F, 500F },
                        new Tuple<float, float>(1000F, 100F)
                    },
                    new object[]
                    {
                        new float[] { 300F, 316.2278F, 761.5773F },
                        new Tuple<float, float>(-200F, -200F)
                    },
                };
            }

        }

        #endregion


        [Theory]
        [Trait("GetLocation", "ThrowException")]
        [MemberData(nameof(MemberDataFor__GetLocation_NullOrEmptyArgs_ThrowException))]
        public void GetLocation_NullOrEmptyArgs_ThrowException(DistanceToSatellite[] distancesToSatellites)
        {
            //assert
            Assert.Throws<ArgumentException>(() =>

                // act
                InstanceToTesting.GetLocation(distancesToSatellites)
            );
        }

        [Theory]
        [Trait("GetLocation", "ThrowException")]
        [MemberData(nameof(MemberDataFor__GetLocation_InvalidArgs_ThrowException))]
        public void GetLocation_InvalidArgs_ThrowException(DistanceToSatellite[] distancesToSatellites)
        {
            //assert
            Assert.Throws<ArgumentException>(() =>

                // act
                InstanceToTesting.GetLocation(distancesToSatellites)
            );
        }

        [Theory]
        [Trait("GetLocation", "ThrowException")]
        [MemberData(nameof(MemberDataFor__GetLocation_InvalidAvailableSatellites_ThrowException))]
        public void GetLocation_InvalidAvailableSatellites_ThrowException(SatelliteLocation[] availableSatellites)
        {
            // arrange
            SatelliteDataProviderMock
                .Setup(x => x.GetAvailableSatellites())
                .Returns(() => availableSatellites);

            //assert
            Assert.Throws<InvalidOperationException>(() =>

                // act
                InstanceToTesting.GetLocation(ValidDistanceToSatellites)
            );
        }

        [Theory]
        [Trait("GetLocation", "ThrowException")]
        [MemberData(nameof(MemberDataFor__GetLocation_BuildCirclesProcessFailed_ThrowException))]
        public void GetLocation_BuildCirclesProcessFailed_ThrowException(SatelliteLocation[] satelliteLocations)
        {
            // arrange
            SatelliteDataProviderMock
                .Setup(x => x.GetAvailableSatellites())
                .Returns(() => satelliteLocations);

            //assert
            Assert.Throws<InvalidOperationException>(() =>

                // act
                InstanceToTesting.GetLocation(ValidDistanceToSatellites)
            );
        }

        [Theory]
        [Trait("GetLocation", "ThrowException")]
        [MemberData(nameof(MemberDataFor__GetLocation_NoSolutionScenarioCirclesAreTooFarApart__ThrowException))]
        public void GetLocation_NoSolutionScenarioCirclesAreTooFarApart__ThrowException(DistanceToSatellite[] distancesToSatellites)
        {
            //assert
            Assert.Throws<InvalidOperationException>(() =>

                // act
                InstanceToTesting.GetLocation(distancesToSatellites)
            );
        }

        [Theory]
        [Trait("GetLocation", "ThrowException")]
        [MemberData(nameof(MemberDataFor__GetLocation_NoSolutionScenarioOneCircleContainsTheOther__ThrowException))]
        public void GetLocation_NoSolutionScenarioOneCircleContainsTheOther__ThrowException(DistanceToSatellite[] distancesToSatellites)
        {
            //assert
            Assert.Throws<InvalidOperationException>(() =>

                // act
                InstanceToTesting.GetLocation(distancesToSatellites)
            );
        }


        [Theory]
        [Trait("GetLocation", "Get Results")] 
        [MemberData(nameof(MemberDataFor__GetLocation_ValidArgs_ExpectedResult))]
        public void GetLocation_ValidArgs_ExpectedResult(DistanceToSatellite[] distancesToSatellites, LocationPoint expectedResult)
        {
            // act
            var finalLocationPoint = InstanceToTesting.GetLocation(distancesToSatellites);

            //assert
            AssertExtension.InRange(expectedResult, finalLocationPoint);
        }


        [Theory]
        [Trait("GetLocation (primitive)", "ThrowException")]
        [MemberData(nameof(MemberDataFor__GetLocation2_NullOrEmptyArgs_ThrowException))]
        public void GetLocation2_NullOrEmptyArgs_ThrowException(float[] distances)
        {
            //assert
            Assert.Throws<ArgumentException>(() =>

                // act
                InstanceToTesting.GetLocation(distances)
            );
        }

        [Theory]
        [Trait("GetLocation (primitive)", "ThrowException")]
        [MemberData(nameof(MemberDataFor__GetLocation2_InvalidArgs_ThrowException))]
        public void GetLocation2_InvalidArgs_ThrowException(float[] distances)
        {
            //assert
            Assert.Throws<ArgumentException>(() =>

                // act
                InstanceToTesting.GetLocation(distances)
            );
        }

        [Theory]
        [Trait("GetLocation (primitive)", "Get Results")]
        [MemberData(nameof(MemberDataFor__GetLocation2_ValidArgs_ExpectedResult))]
        public void GetLocation2_ValidArgs_ExpectedResult(float[] distancesToSatellites, Tuple<float, float> expectedResult)
        {
            // act
            var finalLocationPoint = InstanceToTesting.GetLocation(distancesToSatellites);

            //assert
            AssertExtension.InRange(expectedResult, finalLocationPoint);
        }

    }
}
