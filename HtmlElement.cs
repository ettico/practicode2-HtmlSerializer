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
            this.Children= new List<HtmlElement>();
        }
        public IEnumerable<HtmlElement> Descendants()
        {
           // HtmlElement current = root;
            Queue<HtmlElement> result = new Queue<HtmlElement>();
            result.Enqueue(this);
            while(result.Count > 0)
            {
                var element= result.Dequeue();
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

           while(current.Parent != null)
           {
                current = current.Parent;
                yield return current;
           }
        }
        public List<HtmlElement> HtmlElementsWithSelector( Selector selector)
        {
            HashSet<HtmlElement> result = new HashSet<HtmlElement>();
            RecursiveScan(this, selector, result);
            return result.ToList();
        }
        private static void RecursiveScan(HtmlElement htmlElement, Selector selector, HashSet<HtmlElement> HashHtmlElements)
        {
            if (selector == null)
            {
                return;
            }
            List<HtmlElement> descendants = htmlElement.Descendants().ToList();
            foreach (HtmlElement element in descendants)
            {
                if (selector.Id == element.Id && selector.TagName == element.Name && selector.Classes.Any(x => element.Classes.Contains(x)))
                {
                    HashHtmlElements.Add(element);
                }
            }
            RecursiveScan(htmlElement, selector.Child, HashHtmlElements);

        }
    }
}
