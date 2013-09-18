using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace XSClasses
{
    public class C_Property
    {
        public C_Property(string name, string html, int position)
        {
            PropertyName = name;
            OriginalString = html;
            PropertyValue = C_Helper.GetProperty(name, html);
            PropertyPosition = position;
        }

        public string OriginalString { get; private set; }
        public string PropertyName { get; private set; }
        public string PropertyValue { get; private set; }
        public int PropertyPosition { get; private set; }
    }

    public class C_Element
    {
        public C_Element(string name, string html, C_ElementType tagtype, int position)
        {
            ElementName = name;
            OriginalString = html;
            ElementType = tagtype;
            ElementPosition = position;
            Properties = C_Helper.GetProperties(html);
        }

        public string OriginalString { get; private set; }
        public string ElementName { get; private set; }
        public C_ElementType ElementType { get; private set; }
        public int ElementPosition { get; private set; }

        public List<C_Property> Properties { get; private set; }
    }

    public class C_Document
    {
        public C_Document Parse(string html)
        {
            List<C_Element> elementList = new List<C_Element>();
            int PositionCounter = -1;

            foreach (char c in html.ToCharArray())
            {
                PositionCounter++;

                #region START OF TAG

                if (c == '<')
                {
                    InsideTag = true;

                    if (TextString.Trim().Length != 0)
                    {
                        elementList.Add(new C_Element("Content", TextString, C_ElementType.text, PositionCounter));
                        TextString = string.Empty;
                    }

                    continue;
                }

                #endregion

                #region END OF TAG

                if (c == '>')
                {
                    InsideTag = false;
                    TagString = TagString.Trim();

                    if (TagString.Length != 0)
                    {
                        elementList.Add(new C_Element(C_Helper.GetTag(TagString), TagString, C_ElementType.tag, PositionCounter));
                        TagString = string.Empty;
                    }

                    continue;
                }

                #endregion

                #region BUILD STRINGS

                if (InsideTag)
                {
                    TagString += c;
                }
                else
                {
                    TextString += c;
                }

                #endregion
            }

            return new C_Document
            {
                Elements = elementList,
                OriginalString = html,
            };
        }
        public List<C_Element> Elements { get; private set; }
        public string OriginalString { get; private set; }

        private bool InsideTag = false;
        private string TextString = string.Empty;
        private string TagString = string.Empty;
    }

    public static class C_Helper
    {
        public static string GetProperty(string PropertyName, string StringToTest)
        {
            if (StringToTest.Contains(PropertyName))
            {
                try
                {
                    int TagIndex = StringToTest.IndexOf(PropertyName);
                    int StartIndex = StringToTest.IndexOf("\"", TagIndex) + 1;
                    int EndIndex = StringToTest.IndexOf("\"", StartIndex);

                    return StringToTest.Substring(StartIndex, EndIndex - StartIndex);
                }
                catch
                {
                    int TagIndex = StringToTest.IndexOf(PropertyName);
                    int StartIndex = StringToTest.IndexOf("\'", TagIndex) + 1;
                    int EndIndex = StringToTest.IndexOf("\'", StartIndex);

                    return StringToTest.Substring(StartIndex, EndIndex - StartIndex);
                }
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetTag(string StringToTest)
        {
            int i;

            if (StringToTest.Contains(" "))
            {
                i = StringToTest.IndexOf(" ");
                return StringToTest.Substring(0, i + 1).Trim();
            }
            else
            {
                return StringToTest.Trim();
            }
        }
        public static string RemoveEncoding(string html)
        {
            try
            {
                string temp = Regex.Replace(html.
                    Replace("&ndash;", "-").
                    Replace("&nbsp;", " ").
                    Replace("&rsquo;", "'").
                    Replace("&amp;", "&").
                    Replace("&#038;", "&").
                    Replace("&quot;", "\"").
                    Replace("&#039;", "'").
                    Replace("&#8230;", "...").
                    Replace("&#8212;", "—").
                    Replace("&#8211;", "-").
                    Replace("&#8220;", "“").
                    Replace("&#8221;", "”").
                    Replace("&#8217;", "'").
                    Replace("&#160;", " ").
                    Replace("&gt;", ">").
                    Replace("&rdquo;", "\"").
                    Replace("&ldquo;", "\"").
                    Replace("&lt;", "<").
                    Replace("&#215;", "×").
                    Replace("&#8242;", "′").
                    Replace("&#8243;", "″").
                    Replace("&#8216;", "'"),
                    "<[^<>]+>", string.Empty);

                temp = HttpUtility.HtmlDecode(temp);

                return temp;
            }
            catch
            {
                return html;
            }
        }
        public static List<C_Property> GetProperties(string html)
        {

            if (html.Contains("="))
            {
                return new List<C_Property>();
            }
            else
            {
                return new List<C_Property>();
            }
        }
    }

    public enum C_ElementType
    {
        text,
        tag
    }
}