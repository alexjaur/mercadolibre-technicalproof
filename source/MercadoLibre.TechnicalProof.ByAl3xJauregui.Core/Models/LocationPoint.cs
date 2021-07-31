namespace MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Models
{
    public class LocationPoint
    {
        public LocationPoint()
            : this(float.NaN, float.NaN)
        { }

        public LocationPoint(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; set; }
        public float Y { get; set; }


        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }

        public override bool Equals(object obj)
        {
            return obj is LocationPoint point && point == this;
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(X.GetHashCode(), Y.GetHashCode());
        }

        public static bool operator !=(LocationPoint left, LocationPoint right)
        {
            return !(left == right);
        }

        public static bool operator ==(LocationPoint left, LocationPoint right)
        {
            return left?.X == right?.X && left?.Y == right?.Y;
        }
    }
}
