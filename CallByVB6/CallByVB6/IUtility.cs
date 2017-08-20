using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CallByVB6
{
    [Guid("4191AEF9-BE9A-4453-90B5-E49433F46BA1")]
    public interface IUtility
    {
        string StrToPinyin(string str);
    }
}
