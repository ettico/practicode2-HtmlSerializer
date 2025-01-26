using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace project2_Html_Serializer
{
    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Match> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        //מאפינים לעץ של אוביקטים
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }
        public HtmlElement()
        {
            this.Attributes = new List<Match>();
            this.Classes = new List<string>();
            this.Children = new List<HtmlElement>();
        }
        public IEnumerable<HtmlElement> Descendants()
        {
            // HtmlElement current = root;
            Queue<HtmlElement> result = new Queue<HtmlElement>();
            result.Enqueue(this);
            while (result.Count > 0)
            {
                var element = result.Dequeue();
                yield return element;
                foreach (var child in element.Children)
                {
                    result.Enqueue(child);
                }
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            var current = this;

            while (current.Parent != null)
            {
                current = current.Parent;
                yield return current;
            }
        }
        public override string ToString()
        {
            string str = $"Name: {Name}, Id: {Id}, Attributes: ";
            foreach (var a in Attributes)
            {
                str += a + " ";
            }
            str += "Classes: ";
            foreach (var c in Classes)
            {
                str += c + " ";
            }
            str += $"InnerText: {InnerHtml}";
            return str;
        }

        //---------------------------------------------------------------------
        public HashSet<HtmlElement> FindElementsBySelector(Selector selector)
        {
            HashSet<HtmlElement> hashSetElements = new HashSet<HtmlElement>();
            FindElementsBySelectorRecursive(this, selector, hashSetElements);
            return hashSetElements;
        }
        public void FindElementsBySelectorRecursive(HtmlElement htmlElement, Selector selector, HashSet<HtmlElement> setElements)
        {
            var listElements = htmlElement.Descendants().Where(element => EqualHtmlElementAndSelector(element, selector)).ToList();

            if (selector.Child == null)
            {
                setElements.UnionWith(listElements);
            }
            else
            {
                foreach (var lstelement in listElements)
                {
                    FindElementsBySelectorRecursive(lstelement, selector.Child, setElements);
                }
            }
        }
        public bool EqualHtmlElementAndSelector(HtmlElement rootElement, Selector rootSelector)
        {
            foreach (string class1 in rootSelector.Classes)
            {
                if (!rootElement.Classes.Contains(class1))
                    return false;
            }
            return (rootElement.Name == rootSelector.TagName || rootSelector.TagName == null) &&
               (rootElement.Id == rootSelector.Id || rootSelector.Id == null);
        }
    }
}