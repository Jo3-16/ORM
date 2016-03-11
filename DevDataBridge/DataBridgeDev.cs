using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace DevDataBridge
{
    public class DataBridgeDev : IDataBridge
    {
        public IEnumerable<string> GetConnectedVerticesForVertex(string vertexId)
        {
            if (vertexId == "Gerhard")
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

        public VertexData GetVertexData(string vertxId)
        {
          return new VertexData(vertxId);
        }

        public string AddChild(string parentId)
        {
           //TODO ShowDialog

            return "NewChild";
        }
    }
}
