using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode_2019_Csharp.Helper;

namespace AdventOfCode_2019_Csharp.Days
{
    class Day10
    {
        public static void Solve()
        {
            var lines = FileReader.ParseDataFromFile<string>("Data/Day10.txt", '\n');
            var asteroids = lines.Select((line, i) =>
                line.Select((c, j) => c == '#' ? new Point {X = j, Y = i} : new Point())
                    .Where(p => p.X != -1 && p.Y != -1)
            ).SelectMany(x => x).ToList();

            var asteroidsFromBestLocations = GetDetectableAsteroidsFromBestLocation(asteroids);
            Console.WriteLine(asteroidsFromBestLocations);

            VaporizeAsteroids(asteroids, asteroidsFromBestLocations);
        }

        private static int GetDetectableAsteroidsFromBestLocation(List<Point> asteroids)
        {
            var asteroidsFromLocation = asteroids.Max(asteroid =>
            {
                var gradients = new HashSet<GradientVector>();
                foreach (var other in asteroids)
                {
                    if (asteroid.X == other.X && asteroid.Y == other.Y) continue;
                    double deltaY = other.Y - asteroid.Y;
                    double deltaX = other.X - asteroid.X;
                    var distance = Math.Sqrt(deltaY * deltaY + deltaX * deltaX);

                    gradients.Add(new GradientVector
                    {
                        M = deltaY / distance,
                        N = deltaX / distance,
                    });
                }

                return gradients.Count;
            });

            return asteroidsFromLocation;
        }

        private static void VaporizeAsteroids(List<Point> asteroids, int asteroidsFromBestLocations)
        {
            var bestLocation = asteroids.Where(asteroid =>
            {
                var gradients = new HashSet<GradientVector>();
                foreach (var other in asteroids)
                {
                    if (asteroid.X == other.X && asteroid.Y == other.Y) continue;
                    double deltaY = other.Y - asteroid.Y;
                    double deltaX = other.X - asteroid.X;
                    var distance = Math.Sqrt(deltaY * deltaY + deltaX * deltaX);

                    gradients.Add(new GradientVector
                    {
                        M = deltaY / distance,
                        N = deltaX / distance,
                    });
                }

                return gradients.Count == asteroidsFromBestLocations;
            }).First();

            var angleToPointsDictionary = new SortedDictionary<double, List<Point>>();
            var asteroidVaporized = 0;
            foreach (var asteroid in asteroids)
            {
                if (asteroid.Y == bestLocation.Y && asteroid.X == bestLocation.X) continue;

                var angle = GetAngleBetweenTwoPoint(bestLocation, asteroid);
                angleToPointsDictionary.PutIfAbsent(angle, new List<Point>());
                angleToPointsDictionary[angle].Add(asteroid);
            }

            foreach (var (key, value) in angleToPointsDictionary)
            {
                if (!value.Any()) break;

                var asteroidToRemove = value.OrderBy(p =>
                {
                    double deltaY = p.Y - bestLocation.Y;
                    double deltaX = p.X - bestLocation.X;
                    var distance = Math.Sqrt(deltaY * deltaY + deltaX * deltaX);
                    return distance;
                }).First();
                angleToPointsDictionary[key].Remove(asteroidToRemove);
                asteroidVaporized++;

                Console.WriteLine($"Asteroid[{asteroidVaporized}] = ({asteroidToRemove.X}, {asteroidToRemove.Y})");

                if (asteroidVaporized == 200)
                {
                    Console.WriteLine($"Result: {asteroidToRemove.X * 100 + asteroidToRemove.Y}");
                    break;
                }
            }
        }


        private static double GetAngleBetweenTwoPoint(Point a, Point b)
        {
            double deltaY = -b.Y + a.Y;
            double deltaX = b.X - a.X;
            var angle = Math.Atan2(deltaX, deltaY);
            while (angle < 0)
            {
                angle += Math.PI * 2;
            }

            return angle * 180 / Math.PI;
        }

        private class Point : IEquatable<Point>
        {
            public int X { get; set; } = -1;
            public int Y { get; set; } = -1;

            public bool Equals(Point other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return X == other.X && Y == other.Y;
            }

            public override bool Equals(object obj)
            {
                if (obj is null) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Point) obj);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(X, Y);
            }
        }

        private class GradientVector : IEquatable<GradientVector>
        {
            public double M { get; set; }
            public double N { get; set; }

            public bool Equals(GradientVector other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return Math.Abs(M - other.M) <= 0.001 && Math.Abs(N - other.N) <= 0.001;
            }

            public override bool Equals(object obj)
            {
                if (obj is null) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((GradientVector) obj);
            }

            public override int GetHashCode()
            {
                var hashcode = 17;
                var m = Math.Round(M, 4);
                var n = Math.Round(N, 4);
                hashcode = hashcode * 23 + m.GetHashCode();
                hashcode = hashcode * 23 + n.GetHashCode();

                return hashcode;
            }
        }
    }
}
