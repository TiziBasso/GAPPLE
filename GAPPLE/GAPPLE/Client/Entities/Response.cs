using System.Collections.Generic;

namespace GAPPLE.Client.Entities
{
    public class Response
    {
        public bool IsSuccessStatusCode { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; }
        public string Message { get; set; }
        public byte[] Data { get; set; }
        public List<Dictionary<string, string>> Content { get; set; }

        public Response(bool isSuccessStatusCode) => IsSuccessStatusCode = isSuccessStatusCode;

        public Response(bool isSuccessStatusCode, Dictionary<string, List<string>> errors) => (IsSuccessStatusCode, Errors) = (isSuccessStatusCode, errors);

        public Response(bool isSuccessStatusCode, string message) => (IsSuccessStatusCode, Message) = (isSuccessStatusCode, message);

        public Response(bool isSuccessStatusCode, Dictionary<string, List<string>> errors, string message) => (IsSuccessStatusCode, Errors, Message) = (isSuccessStatusCode, errors, message);

        public Response(bool isSuccessStatusCode, byte[] data) => (IsSuccessStatusCode, Data) = (isSuccessStatusCode, data);

        public Response(bool isSuccessStatusCode, List<Dictionary<string, string>> content) => (IsSuccessStatusCode, Content) = (isSuccessStatusCode, content);
    }
}
