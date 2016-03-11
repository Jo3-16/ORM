using System;
using System.Collections.Generic;
using System.Linq;
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

        private VertexData GetDataFromAdressId(string adressId)
        {
            OdisAddressServiceClient proxyAsync = null;
            try
            {
                proxyAsync = odicConnector.AddressServiceAsync;
                var adress = proxyAsync.GetAddress(int.Parse(adressId), false);

                return new VertexData(adressId, adress.FullName, adress.StandardPhone, adress.AddressImage,"","");
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

        private IEnumerable<VertexData> GetRelationShipsForAdress2(string adress)
        {
            OdisAddressServiceClient proxyAsync = null;
            IEnumerable<VertexData> relationShipIds;
            try
            {
                proxyAsync = odicConnector.AddressServiceAsync;
                var relationships = proxyAsync.GetRelationships(int.Parse(adress), RelationshipDirection.Outgoing, true);
                relationShipIds = relationships
                    .Take(5)
                    .Select(r =>
                    new VertexData(r.CounterPartAddressId.ToString(),
                        r.CounterPartAddress.FullName,
                        r.CounterPartAddress.StandardPhone,
                        r.CounterPartAddress.AddressImage,
                        r.Type.Name,
                        r.ReverseType.Name));
                ;
                proxyAsync.Close();
            }
            catch (Exception)
            {
                proxyAsync?.Abort();
                throw;
            }

            return relationShipIds.Distinct();
        }

        public IEnumerable<VertexData> GetConnectedVerticesForVertex(string vertexId)
        {
            return GetRelationShipsForAdress2(vertexId);
        }

        public VertexData GetVertexData(string vertexId)
        {
            if (vertexId.Equals("NewChild"))
            {
                return new VertexData("NewChild", "NewChild", "","","","");
            }

            return GetDataFromAdressId(vertexId);
        }

        public string AddChild(string parentId)
        {
            MessageBox.Show("Not yet supported in ODISDataBridge. Do NOT Click on the new Child");

            //TODO ShowDialog

            return "NewChild";
        }
    }
}
