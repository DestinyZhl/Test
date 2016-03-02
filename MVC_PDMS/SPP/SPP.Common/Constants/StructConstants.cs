using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPP.Common.Constants
{
    public class StructConstants
    {
        public struct IsClosedStatus
        {
            public const int AllKey = 0;
            public const string AllValue = "全部";

            public const int ClosedKey = 1;
            public const string ClosedValue = "关闭";

            public const int ProcessKey = 2;
            public const string ProcessValue = "进行中";

            public const int ApproveKey = 3;
            public const string ApproveValue = "未生效";
        }

        public struct IsLastestStatus
        {
            public const int AllKey = 0;
            public const string AllValue = "全部";

            public const int LastestKey = 1;
            public const string LastestValue = "最新版";

        }
    }
}
