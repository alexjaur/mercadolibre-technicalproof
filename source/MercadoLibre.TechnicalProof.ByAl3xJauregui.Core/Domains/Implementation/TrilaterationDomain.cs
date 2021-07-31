using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.DataProviders;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Enums;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Extensions;
using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Domains.Implementation
{
    internal class TrilaterationDomain : ITrilaterationDomain
    {
        private double DefaultTolerance { get; } = 0.05D;
        private ISatelliteDataProvider SatelliteDataProvider { get; }


        public TrilaterationDomain(ISatelliteDataProvider satelliteDataProvider)
        {
            SatelliteDataProvider = satelliteDataProvider;
        }


        public Tuple<float, float> GetLocation(float[] distances)
        {
            if (!distances.HasValues())
            {
                throw new ArgumentException("Is null or empty.", nameof(distances));
            }

            if (distances.Length != 3)
            {
                throw new ArgumentException("Exactly 3 distances are required.", nameof(distances));
            }

            var distancesToSatellites = new DistanceToSatellite[]
            {
                new DistanceToSatellite(SatelliteType.Kenobi, distances[0]),
                new DistanceToSatellite(SatelliteType.Skywalker, distances[1]),
                new DistanceToSatellite(SatelliteType.Sato, distances[2]),
            };

            Tuple<float, float> tuple = null;
            LocationPoint finalLocationPoint = GetLocation(distancesToSatellites);

            if (finalLocationPoint != null)
            {
                tuple = new Tuple<float, float>(finalLocationPoint.X, finalLocationPoint.Y);
            }

            return tuple;
        }

        public LocationPoint GetLocation(DistanceToSatellite[] distancesToSatellites)
        {
            if (!distancesToSatellites.HasValues())
            {
                throw new ArgumentException("Is null or empty.", nameof(distancesToSatellites));
            }

            if (distancesToSatellites.Length != 3 || distancesToSatellites.Where(x => x?.Satellite != null).GroupBy(x => x.Satellite).Count() != 3)
            {
                throw new ArgumentException("Exactly 3 distances are required for the different satellites.", nameof(distancesToSatellites));
            }

            var satellites = SatelliteDataProvider.GetAvailableSatellites();

            var circles = BuildCircles(distancesToSatellites, satellites);
            var intersectionPoints = GetAllCirclesIntersectionPoints(circles);
            var finalLocationPoint = GetFinalLocationPoint(intersectionPoints);

            return finalLocationPoint;
        }


        private Circle[] BuildCircles(DistanceToSatellite[] distancesToSatellites, SatelliteLocation[] satellites)
        {
            satellites.Sanitize();

            var circles = distancesToSatellites
                            .Select(x =>
                            {
                                var satellite = satellites.FirstOrDefault(s => s.Satellite == x.Satellite);
                                if (satellite == null)
                                {
                                    return null;
                                }

                                var circle = new Circle
                                {
                                    Radius = x.Distance,
                                    Location = satellite.Location
                                };

                                return circle;
                            })
                            .Where(x => x != null)
                            .ToArray();

            if (circles.Length != 3)
            {
                throw new InvalidOperationException("Build circles process has failed. Doesn't match satellites data.");
            }

            return circles;
        }

        private LocationPoint[] GetAllCirclesIntersectionPoints(Circle[] circles)
        {
            //
            // Find intersections between circles
            // 

            var intersectionsResult1 = FindCirclesIntersections(circles[0], circles[1]);
            if (!intersectionsResult1.ResultType.IsValidResult())
            {
                throw new InvalidOperationException("'Circle 1' and 'Circle 2' do not intersect");
            }

            var intersectionsResult2 = FindCirclesIntersections(circles[1], circles[2]);
            if (!intersectionsResult2.ResultType.IsValidResult())
            {
                throw new InvalidOperationException("'Circle 2' and 'Circle 3' do not intersect");
            }

            var intersectionsResult3 = FindCirclesIntersections(circles[2], circles[0]);
            if (!intersectionsResult3.ResultType.IsValidResult())
            {
                throw new InvalidOperationException("'Circle 3' and 'Circle 1' do not intersect");
            }

            //
            // Group intersections into an array.
            // Discard the intersections that are equal for the same result.
            // 

            var locationPoints = new HashSet<LocationPoint>
            {
                intersectionsResult1.Intersection1,
                intersectionsResult2.Intersection1,
                intersectionsResult3.Intersection1
            };

            if (intersectionsResult1.Intersection1 != intersectionsResult1.Intersection2)
            {
                locationPoints.Add(intersectionsResult1.Intersection2);
            }

            if (intersectionsResult2.Intersection1 != intersectionsResult2.Intersection2)
            {
                locationPoints.Add(intersectionsResult2.Intersection2);
            }

            if (intersectionsResult3.Intersection1 != intersectionsResult3.Intersection2)
            {
                locationPoints.Add(intersectionsResult3.Intersection2);
            }

            var intersectionPoints = locationPoints.ToArray();

            return intersectionPoints;
        }

        private IntersectionsResult FindCirclesIntersections(Circle circle0, Circle circle1)
        {
            /*
             * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
             * FORMULAS:
             * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
             *
             *  dist = sqrt( ((circle0_X - circle1_X) ^ 2) + ((circle0_Y - circle1_Y) ^ 2) )
             *  
             *
             */

            double
                dx = Math.Pow(circle0.Location.X - circle1.Location.X, 2),
                dy = Math.Pow(circle0.Location.Y - circle1.Location.Y, 2),
                dist = Math.Sqrt(dx + dy);

            /*
             * 
             * Validate "no solution" scenarios 
             *              
             */

            if (dist > circle0.Radius + circle1.Radius)
            {
                // No solutions, the circles are too far apart.
                return new IntersectionsResult() 
                { 
                    ResultType = CirclesIntersectionsResultType.CirclesAreTooFarApart 
                };
            }
            
            if (dist < Math.Abs(circle0.Radius - circle1.Radius))
            {
                // No solutions, one circle contains the other.
                return new IntersectionsResult()
                {
                    ResultType = CirclesIntersectionsResultType.OneCircleContainsTheOther
                };
            }

            if (dist == 0 && circle0.Radius == circle1.Radius)
            {
                // No solutions, the circles coincide.
                return new IntersectionsResult()
                {
                    ResultType = CirclesIntersectionsResultType.TheCirclesCoincide
                };
            }

            /*
             * 
             * Calculate intersection points 
             * 
             * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
             * FORMULAS:
             * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
             * 
             *  a = (radius0^2 - radius1^2 + dist^2) / 2*dist
             *  h = sqrt( radius0^2 - a^2 )
             *  
             *  cx = circle0_X + (a * ((circle1_X - circle0_X) / dist))
             *  cy = circle0_Y + (a * ((circle1_Y - circle0_Y) / dist))
             *  
             *  x1 = cx + h * ((circle1_Y - circle0_Y) / dist)
             *  y1 = cy - h * ((circle1_X - circle0_X) / dist)
             *  x2 = cx - h * ((circle1_Y - circle0_Y) / dist)
             *  y2 = cy + h * ((circle1_X - circle0_X) / dist)
             *
             *
             */

            double
                a = (Math.Pow(circle0.Radius, 2) - Math.Pow(circle1.Radius, 2) + Math.Pow(dist, 2)) / (2 * dist),
                h = Math.Sqrt(Math.Pow(circle0.Radius, 2) - Math.Pow(a, 2)),

                tempX = (circle1.Location.X - circle0.Location.X) / dist,
                tempY = (circle1.Location.Y - circle0.Location.Y) / dist,

                cx = circle0.Location.X + a * tempX,
                cy = circle0.Location.Y + a * tempY,

                x1 = cx + h * tempY,
                y1 = cy - h * tempX,
                x2 = cx - h * tempY,
                y2 = cy + h * tempX;

            // 
            // Create object result 
            // 

            var intersection1 = new LocationPoint((float)x1, (float)y1);
            var intersection2 = new LocationPoint((float)x2, (float)y2);

            var resultType = dist == circle0.Radius + circle1.Radius
                             ? CirclesIntersectionsResultType.SuccessfulWithOneSolution
                             : CirclesIntersectionsResultType.SuccessfulWithTwoSolutions;

            var intersectionsResult = new IntersectionsResult(resultType, intersection1, intersection2);

            return intersectionsResult;
        }

        private LocationPoint GetFinalLocationPoint(LocationPoint[] circlesIntersectionPoints)
        {
            LocationPoint finalLocationPoint = null;
            ICollection<LocationPoint> similarLocationPoints = null;
            Queue<LocationPoint> intersectionPointsQueue = new Queue<LocationPoint>(circlesIntersectionPoints);

            do
            {
                LocationPoint item = intersectionPointsQueue.Dequeue();

                similarLocationPoints = intersectionPointsQueue
                                            .Where(x => 
                                                x.InRange(item, DefaultTolerance)
                                            )
                                            .Append(item)
                                            .ToArray();

                if (similarLocationPoints.Count == 3)
                {
                    float 
                        avgX = (float)Math.Round(similarLocationPoints.Average(x => x.X), 2, MidpointRounding.ToEven),
                        avgY = (float)Math.Round(similarLocationPoints.Average(x => x.Y), 2, MidpointRounding.ToEven);

                    finalLocationPoint = new LocationPoint(avgX, avgY);
                }

            } while (finalLocationPoint == null && intersectionPointsQueue.Count > 2);

            return finalLocationPoint;
        }
    }
}
