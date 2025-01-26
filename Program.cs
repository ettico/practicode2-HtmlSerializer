using project2_Html_Serializer;
using System.Text.RegularExpressions;
async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}

// יצירת מערך עם כל תכני התגיות 
var html = await Load("https://chat.malkabruk.co.il/#/chat");
html = "<html>" +
           "<div class=\"k\">" +
           "<br id=\"my-id\" class=\"my class\"/>" +
           "</div>" +
           "<br/>" +
        "</html>";
var onlySpaces = new Regex(@"^\s*$");
var htmlClean = new Regex("\\n").Replace(html, string.Empty);
var htmlLines = new Regex("<(.*?)>").Split(htmlClean).Where(s=>!onlySpaces.IsMatch(s)&&s.Length>0).ToList();

foreach (var item in htmlLines)
{
    Console.WriteLine(item);
}
HtmlSerializer htmlSerializer=new HtmlSerializer();
htmlSerializer.ToBuildTree(htmlLines);


Console.WriteLine("============================");
//div#mydiv.class-name.class2.class3 p#id-p.class-p h1.my-h1
Selector s = Selector.FromStringToSelector("html br#my-id");
HashSet<HtmlElement> htmlElement = htmlSerializer.Root.FindElementsBySelector(s);
while (s!=null)
{
    Console.WriteLine(s.ToString());
    s = s.Child;
}
Console.WriteLine("-----HASHSET-------------------");
foreach (var item in htmlElement)
{
    Console.WriteLine(item.ToString());
}


