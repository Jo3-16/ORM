namespace Contracts
{
    public class VertexData
    {
        public VertexData(string vertexId, string fullName, string standardPhone, string addressImage, string myRole="", string otherRole="")
        {
            VertexId = vertexId;
            FullName = fullName;
            StandardPhone = standardPhone;
            AddressImage = addressImage;
            MyRole = myRole;
            OtherRole = otherRole;
        }

        public string VertexId { get; set; }
        public string FullName { get; set; }
        public string StandardPhone { get; set; }
        public string AddressImage { get; set; }

        public string MyRole { get; set; }
        public string OtherRole { get; set; }
    }
}