using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UsMemoTool.Utility.Serializer
{
    class JsonSerializer 
    {
        public async Task<T> Read<T>(string filename)
        {
            string ret = "";
            using (System.IO.StreamReader sr = new System.IO.StreamReader(
                filename, System.Text.Encoding.GetEncoding("Shift_JIS")))
            {
                ret = await sr.ReadToEndAsync();
            }

            return await Json2Obj<T>(ret);
        }

        public async Task Write(string filename, object data)
        {
            var json = await Obj2Json(data);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(
                           filename, false, System.Text.Encoding.GetEncoding("Shift_JIS")))
            {
            
                await file.WriteAsync(json);
            }
        }
        public async Task<string> Obj2Json(object obj)
        {
            return await Task.Run(() => JsonConvert.SerializeObject(obj, Formatting.Indented));
        }
        public async Task<T> Json2Obj<T>(string json)
        {
            return await Task.Run(() => JsonConvert.DeserializeObject<T>(json));
        }
    }
}
