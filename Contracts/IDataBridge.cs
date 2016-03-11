using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IDataBridge
    {
        IEnumerable<string> GetConnectedVerticesForVertex(string vertexId);

        VertexData GetVertexData(string vertxId);

        string AddChild(string parentId);
    }
}
