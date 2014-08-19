using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Decko
{
    public class PluralFormatProvider : IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type formatType)
        {
            return this;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (format != null)
            {
                string[] forms = format.Split(';');
                int value = (int)arg;
                int form = value == 1 ? 0 : 1;
                return forms[form];
            }
            else
            {
                return String.Format("{0}", arg);
            }
        }
    }
}