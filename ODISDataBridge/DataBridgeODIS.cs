using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Contracts;
using Odis.AppService.Common.DTO.Address;
using Odis.Client.ServerApi;
using Odis.Client.ServerApi.OdisAddressWcfService;
using ODISDataBridge.Properties;

namespace ODISDataBridge
{
    public class DataBridgeODIS : IDataBridge
    {
        private OdisConnector odicConnector;

        private void InitConnection()
        {
            odicConnector = new OdisConnector(false, Settings.Default.ServerName, Settings.Default.Port, "dev");
        }

        public DataBridgeODIS()
        {
            InitConnection();
        }

        private IEnumerable<string> GetRelationShipsForAdress(string adress)
        {
            OdisAddressServiceClient proxyAsync = null;
            IEnumerable<string> relationShipIds;
            try
            {
                proxyAsync = odicConnector.AddressServiceAsync;
                var relationships = proxyAsync.GetRelationships(int.Parse(adress), RelationshipDirection.All, true);
                relationShipIds = relationships.Take(5).Select(r => r.CounterPartAddressId.ToString());
                proxyAsync.Close();
            }
            catch (Exception)
            {
                proxyAsync?.Abort();
                throw;
            }

            return relationShipIds.Distinct();
        }

        private VertexData GetDataFromAdressId(string adressId)
        {
            OdisAddressServiceClient proxyAsync = null;
            try
            {
                proxyAsync = odicConnector.AddressServiceAsync;
                var adress = proxyAsync.GetAddress(int.Parse(adressId), false);

                return new VertexData(adressId, adress.FullName, adress.StandardPhone, adress.AddressImage);
            }
            catch (Exception)
            {
                proxyAsync?.Abort();
                throw;
            }
            finally
            {
                proxyAsync?.Close();
            } 
        }


        public IEnumerable<string> GetConnectedVerticesForVertex(string vertexId)
        {
           return GetRelationShipsForAdress(vertexId);
        }

        public VertexData GetVertexData(string vertxId)
        {
            if (vertxId.Equals("NewChild"))
            {
                return new VertexData("NewChild", "NewChild", "","");
            }

            return GetDataFromAdressId(vertxId);
        }

        public string AddChild(string parentId)
        {
            MessageBox.Show("Not yet supported in ODISDataBridge. Do NOT Click on the new Child");

            //TODO ShowDialog

            return "NewChild";
        }
    }
}
