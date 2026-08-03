using System;
using System.Collections.Generic;
using System.Text;

namespace Application.FreamWork.SearchBaseModel
{
    public class GenericComplexResult<TSearchModel,TListIteam>
    {
        public TSearchModel? SearchModel { get; set; }

        public List<TListIteam> ListIteams { get; set; }= new List<TListIteam>();
    }
}
