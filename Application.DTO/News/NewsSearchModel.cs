using Application.FreamWork.SearchBaseModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.News
{
    public class NewsSearchModel:PageModel
    {
        public string Phrase { get; set; } = string.Empty;
        public int? CategoryId { get; set; }
      

    }
}
