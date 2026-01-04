using System.Net;
using System.Net.Http.Json;

namespace GAPPLE.Client.Entities
{
    public class Response
    {
        public HttpStatusCode? HttpStatusCode { private get; set; }
        public Dictionary<string, List<string>> Errors { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public byte[] DataBytes { get; set; }
        public List<Dictionary<string, string>> Content { get; set; }

        public bool IsOk => HttpStatusCode == System.Net.HttpStatusCode.OK;
        public bool IsBadRequest => HttpStatusCode == System.Net.HttpStatusCode.BadRequest;
        public bool IsInternalServerError => HttpStatusCode == System.Net.HttpStatusCode.InternalServerError;
        public bool IsUnauthorized => HttpStatusCode == System.Net.HttpStatusCode.Unauthorized;
        public bool IsNoContent => HttpStatusCode == System.Net.HttpStatusCode.NoContent;
        public bool IsUnavailableForLegalReasons => HttpStatusCode == System.Net.HttpStatusCode.UnavailableForLegalReasons;
        public bool IsCanceled => HttpStatusCode == null;
        public bool IsConflict => HttpStatusCode == System.Net.HttpStatusCode.Conflict;

        public Response() { HttpStatusCode = null; }
        public Response(HttpStatusCode httpStatusCode) => HttpStatusCode = httpStatusCode;

        public Response(HttpStatusCode httpStatusCode, Dictionary<string, List<string>> errors) => (HttpStatusCode, Errors) = (httpStatusCode, errors);

        public Response(HttpStatusCode httpStatusCode, string message) => (HttpStatusCode, Message) = (httpStatusCode, message);

        public Response(HttpStatusCode httpStatusCode, Dictionary<string, List<string>> errors, string message) => (HttpStatusCode, Errors, Message) = (httpStatusCode, errors, message);

        public Response(HttpStatusCode httpStatusCode, object data) => (HttpStatusCode, Data) = (httpStatusCode, data);

        public Response(HttpStatusCode httpStatusCode, byte[] dataBytes) => (HttpStatusCode, DataBytes) = (httpStatusCode, dataBytes);

        public Response(HttpStatusCode httpStatusCode, List<Dictionary<string, string>> content) => (HttpStatusCode, Content) = (httpStatusCode, content);

        public static async Task<Response> CreateAsync(HttpResponseMessage httpResponse)
        {
            Response r = new(httpResponse.StatusCode);

            if (r.HttpStatusCode == System.Net.HttpStatusCode.BadRequest)
                r.Errors = await httpResponse.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>();
            else if (r.HttpStatusCode == System.Net.HttpStatusCode.InternalServerError)
                r.Message = await httpResponse.Content.ReadAsStringAsync();

            return r;
        }
    }
}
