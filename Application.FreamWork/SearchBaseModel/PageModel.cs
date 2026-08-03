using System;
using System.Collections.Generic;
using System.Text;

namespace Application.FreamWork.SearchBaseModel
{
    public class PageModel
    {
        public int pageIndex { get; set; } = 1;

        private int _pageSize { get; set; }

        public int pageSize
        {
            get => _pageSize == 0 ? 10 : _pageSize;

            set => _pageSize = (value == 0 ? _pageSize = 10 : _pageSize = value);
        }

        private int _recordCount { get; set; }
        public int RecordCount { get => _recordCount; set => _recordCount = value; }



        public int pageCount
        {
            get
            {
                if (RecordCount % pageSize == 0 )
                {
                    return RecordCount / pageSize;
                    
                }
                else
                {
                    return (RecordCount / pageSize) + 1;
                }
            }
        }

    }
}
