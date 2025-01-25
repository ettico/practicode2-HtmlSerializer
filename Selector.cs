using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project2_Html_Serializer
{
    //internal class Selector
    //{
    //    public string TagName { get; set; }
    //    public string Id { get; set; }
    //    public List<string> Classes { get; set; }
    //    public Selector Parent { get; set; }
    //    public Selector Child { get; set; }
    //    public Selector(string tagName,string id,List<string> classes)
    //    {
    //        this.TagName = tagName;
    //        this.Id = id;
    //        this.Classes = classes??new List<string>();
    //    }
    //    public static Selector FromStringToSelector(string str)
    //    {
    //        var parts=str.Split(' ');
    //        Selector root = null;
    //        Selector currentSelector = null;
    //        foreach (var part in parts)
    //        {
    //            bool flagClass = false;
    //            string tagName = null;
    //            string selectorId = null;
    //            List<string> classes = new List<string>();
    //            if(part.Contains('#'))
    //            {
    //                var hashIndex = part.IndexOf('#');
    //                tagName=part.Substring(0, hashIndex);
    //                int dotIndex=part.IndexOf('.',hashIndex);
    //                if(dotIndex!=-1)
    //                {
    //                    selectorId=part.Substring(hashIndex+1,dotIndex-hashIndex-1);
    //                    var classPart=part.Substring(dotIndex+1);
    //                    classes.AddRange(classPart.Split(" "));
    //                    flagClass = true;
    //                }
    //                else
    //                {
    //                    selectorId=part.Substring(hashIndex+1);
    //                }

    //            }

    //            if(!flagClass)
    //            {
    //                if(parts.Contains("."))
    //                {
    //                    var classParts=part.Split(".");
    //                    tagName = classParts[0];
    //                    classes.AddRange(classParts[1].Split(" "));
    //                }
    //                else
    //                {
    //                   if(HtmlHelper.Instance.HtmlTags.Contains(part))
    //                   {
    //                       tagName=part;
    //                   }
    //                }
    //            }
    //            var newSelector = new Selector(tagName,selectorId,classes);
    //            if (root == null)
    //                root = newSelector;
    //            else
    //                currentSelector.Child = newSelector;
    //            currentSelector=newSelector;
    //        }
    //        return root;
    //    }
    //}
    internal class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }
        public Selector()
        {

        }
        public Selector(string tagName, string id, List<string> classes)
        {
            this.TagName = tagName;
            this.Id = id;
            this.Classes = classes ?? new List<string>();
        }

        public static Selector FromStringToSelector(string str)
        {
            var parts = str.Split(' ');
            Selector root = null;
            Selector currentSelector = null;

            foreach (var part in parts)
            {
                string tagName = null;
                string selectorId = null;
                List<string> classes = new List<string>();

                // בדוק אם יש ID ומחלקות
                if (part.Contains('#'))
                {
                    var hashIndex = part.IndexOf('#');
                    tagName = part.Substring(0, hashIndex);
                    int dotIndex = part.IndexOf('.', hashIndex);

                    if (dotIndex != -1)
                    {
                        selectorId = part.Substring(hashIndex + 1, dotIndex - hashIndex - 1);
                        var classPart = part.Substring(dotIndex + 1);
                        classes.AddRange(classPart.Split('.')); // מפצל מחלקות לפי נקודה
                    }
                    else
                    {
                        selectorId = part.Substring(hashIndex + 1);
                    }
                }
                else if (part.Contains('.'))
                {
                    var classParts = part.Split('.');
                    tagName = classParts[0];
                    classes.AddRange(classParts.Skip(1)); // קבל את כל המחלקות אחרי החלק הראשון
                }
                else if (HtmlHelper.Instance.HtmlTags.Contains(part))
                {
                    tagName = part; // תג HTML תקני
                }

                var newSelector = new Selector(tagName, selectorId, classes);

                if (root == null)
                    root = newSelector; // קבע שורש אם הוא לא קיים
                else
                {
                    currentSelector.Child = newSelector;
                    newSelector.Parent = currentSelector;
                }

                currentSelector = newSelector;
            }
            return root;
        }
        public override string ToString()
        {
            string str = $"Name: {TagName}, Id: {Id}, ";
            str += "Classes: ";
            foreach (var c in Classes)
            {
                str += c + " ";
            }
            return str;
        }
    }
}

