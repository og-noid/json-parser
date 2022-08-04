using System;
using System.Collections;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace com.rivasolutions {
    class JsonParser {
        public static async Task Main(string[] args) {
            Task<JsonDocument> _task = getJson();
            JsonDocument _jsonDocument = await _task;
            Console.WriteLine(JsonDocumentToString(_jsonDocument));
        }

        public static async Task<JsonDocument> getJson() {
            string _apiKey = "nokey";
            string _keyFile = Path.Combine("apikey.txt");
            string _content = "{\"empty\":true}";

            if (File.Exists(_keyFile)) {
                _apiKey = File.ReadAllText(_keyFile);
                using var _client = new HttpClient();
                var _result = await _client.GetAsync(
                    string.Format("https://api.openweathermap.org/data/2.5/weather?q=London,uk&APPID={0}",
                    _apiKey)
                );
                _content = await _result.Content.ReadAsStringAsync();
            } else {
                Console.WriteLine("File {0} does not exist, cannot execute API call", _keyFile);
            }
            return JsonDocument.Parse(_content);
        }

        public static string JsonDocumentToString(JsonDocument jsonDocument) {
            using (var stream = new MemoryStream()) {
                Utf8JsonWriter writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
                jsonDocument.WriteTo(writer);
                writer.Flush();
                string _json = Encoding.UTF8.GetString(stream.ToArray());
                return (_json);
            }
        }
    }
}
