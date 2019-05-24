using System;
using System.Collections.Generic;
using System.Text;

namespace ColtSmart.Data.Adapter
{
   public class PostgreAdapter: SqlAdapter
    {
        public PostgreAdapter() : base()
        {
            PartsQryGenerator = new PostgrePartsQryGenerator();
        }
    }
}
