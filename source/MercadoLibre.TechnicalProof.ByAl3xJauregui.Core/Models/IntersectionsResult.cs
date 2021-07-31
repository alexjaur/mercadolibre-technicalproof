using MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Enums;

namespace MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Models
{
    internal class IntersectionsResult
    {
        public IntersectionsResult()
        { }

        public IntersectionsResult(CirclesIntersectionsResultType resultType, LocationPoint intersection1, LocationPoint intersection2)
        {
            ResultType = resultType;
            Intersection1 = intersection1;
            Intersection2 = intersection2;
        }

        public LocationPoint Intersection1 { get; set; }
        public LocationPoint Intersection2 { get; set; }
        public CirclesIntersectionsResultType ResultType { get; set; }
    }
}
