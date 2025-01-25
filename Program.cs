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
    Console.WriteLine("line : " + item);
}

// בניית עץ 
HtmlElement root = new HtmlElement();
//HtmlElement currentElement = root;

static HtmlElement CreateElement(string line,string firstWord)
{
    HtmlElement newHtmlElement = new HtmlElement
    {
        Attributes = new List<Match>(), // אתחול של Attributes
        Classes = new List<string>() // אתחול של Classes
    };

    var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line);
    newHtmlElement.Attributes = attributes.Cast<Match>().ToList();
    
    var classAttribute = newHtmlElement.Attributes.FirstOrDefault(m => m.Groups[1].Value == "class");
    if (classAttribute != null)
    {
        var classNames = classAttribute.Groups[2].Value.Split(' ');
        newHtmlElement.Classes.AddRange(classNames);
    }

    //var name = Regex.Match(line, @"<(\w+)");
    var name = firstWord;
    if (name!="")
        newHtmlElement.Name = name;

    var id = newHtmlElement.Attributes.FirstOrDefault(m => m.Groups[1].Value == "id");
    if (id != null)
    {
        newHtmlElement.Id = id.Groups[2].Value;
    }

    return newHtmlElement;
}

//static HtmlElement ToBuildTree(HtmlElement root, List<string> htmlLines)
//{
//    HtmlElement currentElement = root; // אתחול של currentElement
//    foreach (var line in htmlLines)
//    {
//        var spaceIndex = line.IndexOf(' ');
//        var firstWord = (spaceIndex > 0) ? line.Substring(0, spaceIndex) : line;

//        // הגעת לסוף ה-html
//        if (firstWord == "/html")
//        {
//            return root;
//        }

//        // אם המילה הראשונה מתחילה ב-/
//        if (firstWord[0] == '/')
//        {
//            currentElement = currentElement.Parent; // חזרה לאבא
//            continue;
//        }

//        // אם המילה הראשונה היא שם של תגית
//        if (HtmlHelper.Instance.HtmlTags.Contains(firstWord))
//        {
//            var newHtmlElement = CreateElement(line);
//            currentElement.Children.Add(newHtmlElement);
//            newHtmlElement.Parent = currentElement;

//            // בדיקה אם התגית סוגרת את עצמה
//            if (line[line.Length - 1] != '/' && !HtmlHelper.Instance.HtmlVoidTags.Contains(firstWord))
//            {
//                currentElement = newHtmlElement;
//            }
//        }
//        else
//        {
//            currentElement.InnerHtml = line; 
//        }
//    }
//    return root;
//}


static HtmlElement ToBuildTree(HtmlElement root, List<string> htmlLines)
{
    HtmlElement currentElement = root; // אתחול של currentElement
    foreach (var line in htmlLines)
    {
        var spaceIndex = line.IndexOf(' ');
        var firstWord = (spaceIndex > 0) ? line.Substring(0, spaceIndex) : line;

        // הגעת לסוף ה-html
        if (firstWord == "/html")
        {
           // PrintHtmlTree(root);
            return root;
        }

        // אם המילה הראשונה מתחילה ב-/
        if (firstWord.StartsWith('/')&&firstWord!="")
        {
            if (currentElement.Parent != null)
            {
                currentElement = currentElement.Parent; // חזרה לאבא
            }
            //continue;
        }

        // אם המילה הראשונה היא שם של תגית
        else if (HtmlHelper.Instance.HtmlTags.Contains(firstWord) || HtmlHelper.Instance.HtmlVoidTags.Contains(firstWord))
        {
            var newHtmlElement = CreateElement(line,firstWord);
            currentElement.Children.Add(newHtmlElement);
            newHtmlElement.Parent = currentElement;
            currentElement = newHtmlElement;
            //currentElement.Name=firstWord;
            // בדיקה אם התגית סוגרת את עצמה
            if (line[line.Length - 1] != '/' && !HtmlHelper.Instance.HtmlVoidTags.Contains(firstWord))
            {
                currentElement = newHtmlElement;
            }
        }
        else
        {
            // טיפול בתוכן פנימי
            if (currentElement.InnerHtml == null)
            {
                currentElement.InnerHtml = line;
            }
            else
            {
                currentElement.InnerHtml += line; // הוספת תוכן קיים
            }
        }
    }
    return root;
}

HtmlElement h =ToBuildTree(root, htmlLines);
//פונקציה להדפסת העץ
static void PrintHtmlTree(HtmlElement element, int level = 0)
{
    if (element == null) return; // בדוק אם האלמנט הוא null

    // הדפס את השם של האלמנט עם רווחים לפי רמת העומק
    Console.WriteLine(new string(' ', level * 2) + element.Name + " ID: " + element.Id+" InnerHtml: "+element.InnerHtml );
    //foreach (var item in element.Classes)
    //{
    //    Console.Write(" "+item+" ");
    //}
    

    // עבור על כל הילדים והדפס אותם
    foreach (var child in element.Children)
    {
        PrintHtmlTree(child, level + 1); // קריאה רקורסיבית עם רמת עומק מוגברת
    }
}
PrintHtmlTree(h);
Console.WriteLine("============================");
Selector s = Selector.FromStringToSelector("div#mydiv.class-name.class2.class3 width=\"100%\"");
Console.WriteLine(s.ToString());
