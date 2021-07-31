namespace MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Enums
{
    public enum CirclesIntersectionsResultType : short
    {
        CirclesAreTooFarApart = -1,
        OneCircleContainsTheOther = -2,
        TheCirclesCoincide = -3,

        SuccessfulWithOneSolution = 1,
        SuccessfulWithTwoSolutions = 2 
    }
}
