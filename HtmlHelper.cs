using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
namespace project2_Html_Serializer
{
    internal class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;// { get { return _instance; } }
        public string[] HtmlTags { get; set; }
        public string[] HtmlVoidTags { get; set; }
        private HtmlHelper()
        {
            var HtmlTags = File.ReadAllText("JSON Files/HtmlTags.json");
            var HtmlVoidTags = File.ReadAllText("JSON Files/HtmlVoidTags.json");
            
            this.HtmlTags = JsonSerializer.Deserialize<string[]>(HtmlTags); 
            this.HtmlVoidTags = JsonSerializer.Deserialize<string[]>(HtmlVoidTags);
        }
    }
}
