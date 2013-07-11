using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Controls;

namespace XS_Classes
{
    public class Prop
    {
        public String Property
        {
            get;
            set;
        }
        public Object PropertyValue
        {
            get;
            set;
        }
    }

    public class ListOfProperties
    {
        public static List<Prop> GetPropertyList<T>(T t)
        {
            List<Prop> PropertyList = new List<Prop>();
            foreach (PropertyInfo _PropertyInfo in t.GetType().GetProperties())
            {
                switch (_PropertyInfo.PropertyType.Name.ToString())
                {
                    case "SupportedPageOrientation":
                    case "String":
                    case "Boolean":
                    case "Int32":
                    case "Thickness":
                    case "HorizontalAlignment":
                    case "VerticalAlignment":
                    case "Brush":
                    case "FontFamily":
                    case "FontStyle":
                    case "FontWeight":
                    case "FontSize":
                    case "Double":
                        PropertyList.Add(new Prop
                        {
                            Property = _PropertyInfo.Name,
                            PropertyValue = _PropertyInfo.GetValue(t, new object[] { })
                        });
                        break;
                }
            }
            return PropertyList;
        }
    }
}