﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pract.Models
{
    public class PagingViewModel<T> where T : class 
    {
        public IEnumerable<T> Elems { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}