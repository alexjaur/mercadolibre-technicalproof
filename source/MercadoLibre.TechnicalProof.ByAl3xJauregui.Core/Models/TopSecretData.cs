namespace MercadoLibre.TechnicalProof.ByAl3xJauregui.Core.Models
{
    public class TopSecretData
    {
        public TopSecretData()
        { }

        public TopSecretData(float x, float y, string message)
        {
            X = x;
            Y = y;
            Message = message;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public string Message { get; set; }
    }
}
