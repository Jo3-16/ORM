namespace Contracts
{
    public class VertexData
    {
        public VertexData(string adressId, string fullName, string standardPhone, string addressImage)
        {
            this.VertexId = adressId;
            FullName = fullName;
            StandardPhone = standardPhone;
            AddressImage = addressImage;
        }

        public string VertexId { get; set; }
        public string FullName { get; set; }
        public string StandardPhone { get; set; }
        public string AddressImage { get; set; }
    }
}