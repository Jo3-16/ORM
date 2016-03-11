using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IDataBridge
    {
      //  IEnumerable<string> GetConnectedVertexIdsForVertex(string vertexId);
        IEnumerable<VertexData> GetConnectedVerticesForVertex(string vertexId);

        VertexData GetVertexData(string vertexId);

        string AddChild(string parentId);
    }
}
