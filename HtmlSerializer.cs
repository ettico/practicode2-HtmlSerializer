using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace project2_Html_Serializer
{
    internal class HtmlSerializer
    {
        public HtmlElement Root { get; set; }
        private HtmlHelper _helper = HtmlHelper.Instance;
        public HtmlSerializer()
        {
            Root=new HtmlElement();
        }
        private HtmlElement CreateElement(string line, string firstWord)
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
            if (name != "")
                newHtmlElement.Name = name;

            var id = newHtmlElement.Attributes.FirstOrDefault(m => m.Groups[1].Value == "id");
            if (id != null)
            {
                newHtmlElement.Id = id.Groups[2].Value;
            }

            return newHtmlElement;
        }
        public HtmlElement ToBuildTree( List<string> htmlLines)
        {
            HtmlElement currentElement = Root; // אתחול של currentElement
            foreach (var line in htmlLines)
            {
                var spaceIndex = line.IndexOf(' ');
                var firstWord = (spaceIndex > 0) ? line.Substring(0, spaceIndex) : line;

                // הגעת לסוף ה-html
                if (firstWord == "/html")
                {
                    // PrintHtmlTree(root);
                    //Root = Root.Children[0];
                    return Root;
                }

                // אם המילה הראשונה מתחילה ב-/
                if (firstWord.StartsWith('/') && firstWord != "")
                {
                    if (currentElement.Parent != null)
                    {
                        currentElement = currentElement.Parent; // חזרה לאבא
                    }
                    //continue;
                }

                // אם המילה הראשונה היא שם של תגית
                else if (_helper.HtmlTags.Contains(firstWord) || _helper.HtmlVoidTags.Contains(firstWord))
                {
                    var newHtmlElement = CreateElement(line, firstWord);
                    newHtmlElement.Parent = currentElement;
                    if(currentElement != null)
                       currentElement.Children.Add(newHtmlElement);
                    currentElement = newHtmlElement;
                    //currentElement.Name=firstWord;
                    // בדיקה אם התגית סוגרת את עצמה
                    if (line[line.Length - 1] != '/' && !_helper.HtmlVoidTags.Contains(firstWord))
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
            return Root;
        }
        //פונקציה להדפסת העץ
        public void PrintHtmlTree( HtmlElement h, int level = 0)
        {
            if (this == null) return; // בדוק אם האלמנט הוא null

            // הדפס את השם של האלמנט עם רווחים לפי רמת העומק
            Console.WriteLine(new string(' ', level * 2) + this.Root.Name + " ID: " + this.Root.Id + " InnerHtml: " + this.Root.InnerHtml);
            //foreach (var item in element.Classes)
            //{
            //    Console.Write(" "+item+" ");
            //}


            // עבור על כל הילדים והדפס אותם
            foreach (var child in this.Root.Children)
            {
                PrintHtmlTree(child, level + 1); // קריאה רקורסיבית עם רמת עומק מוגברת
            }
        }
    }
}
