using System;
using System.Collections.Generic;

namespace Landopedia
{
    [Serializable]
    public struct DataPanelStruct
    {
        [Serializable]
        public struct Button
        {
            public string text;
            public Action action;
        }

        public string text;
        public List<Button> buttons;
    }
}
