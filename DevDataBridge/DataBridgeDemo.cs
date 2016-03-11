using System.Collections.Generic;
using System.Linq;
using Contracts;

namespace DevDataBridge
{
    public class DataBridgeDemo : IDataBridge
    {
        public IEnumerable<string> GetConnectedVertexIdsForVertex(string vertexId)
        {
            if (vertexId == "165378")
            {
                yield return "Fahrrad";
                yield return "Krafttraining";
            }

            if (vertexId == "Krafttraining")
            {
                yield return "Chris";
                yield return "LangHantel";
            }
            if (vertexId == "Chris")
            {
                yield return "Pflanzen";
                yield return "Gitarre";
                yield return "Basketball";
                yield return "Volleyball";
                yield return "Schlafen";
            }
            if (vertexId == "LangHantel")
            {
                yield return "Hantel-Scheibe";
                yield return "Verschlüsse";
            }
        }

        public IEnumerable<VertexData> GetConnectedVerticesForVertex(string vertexId)
        {
            return GetConnectedVertexIdsForVertex(vertexId).Select(GetVertexData);
        }

        public VertexData GetVertexData(string vertexId)
        {
          return new VertexData(vertexId,vertexId,"","","Pastor","Gemeinde");
        }

        public string AddChild(string parentId)
        {
           //TODO ShowDialog

            return "NewChild";
        }
    }
}
