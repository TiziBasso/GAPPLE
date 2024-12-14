using RestSharp;
using GAPPLE.Shared.Model;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace IntegrationService.Controllers
{

    public class ClientsController
    {
        RestClient Client = new("http://192.168.10.10:17000/Api/");

        public void GetClientes()
        {
            var request = new RestRequest("Get?process=2117&pageSize=2365&pageIndex=0&view=Vista%20API", Method.Get);
            request.AddHeader("ApiAuthorization", "D2D0ABBE-9E80-464E-85FC-40B0EDBB5C1E");
            request.AddHeader("Company", "53");
            var response = Client.Execute(request);
            RootObject clients = JsonConvert.DeserializeObject<RootObject>(response.Content!)!;
            foreach(var cliente in clients.ResultData.List)
            {
                InsertarCliente(cliente.CodigoCliente,cliente.RazonSocial,cliente.NombreComercial,cliente.CUIT,cliente.Clasificacion!,
                    false,cliente.Observaciones!,"ADMIN");
            }
        }

        void InsertarCliente(string codCliente, string razonSocial, string nombreComercial, string CUIT, string clasificacion, bool clienteEspecial,
             string observaciones, string altaUsuario, string? edicionUsuario = null)
        {
            using (SqlConnection cnn = new("Server=192.168.10.10,1433;Database=ZENTRA;User Id=TBasso;Password=colectivosinfrenos;"))
            {
                var cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "prc_ins_Clientes";
                cmd.Parameters.AddWithValue("@pCodigoCliente", codCliente);
                cmd.Parameters.AddWithValue("@pRazonSocial", razonSocial);
                cmd.Parameters.AddWithValue("@pNombreComercial", nombreComercial);
                cmd.Parameters.AddWithValue("@pCUIT", CUIT);
                cmd.Parameters.AddWithValue("@pClasificacion", clasificacion);
                cmd.Parameters.AddWithValue("@pClienteEspecial", clienteEspecial);
                if (altaUsuario != null) cmd.Parameters.AddWithValue("@pObservaciones", observaciones);
                cmd.Parameters.AddWithValue("@pAltaUsuario", altaUsuario);
                if (edicionUsuario != null) cmd.Parameters.AddWithValue("@pEdicionUsuario", edicionUsuario);

                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
        }

        public class RootObject
        {
            [JsonProperty("resultData")]
            public ResultData ResultData { get; set; }
        }

            public class ResultData
        {
            [JsonProperty("list")]
            public List<Cliente> List { get; set; } = new();
        }
    }
}
