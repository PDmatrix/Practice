﻿using System;

namespace Pract.App_LocalResources
{
    public static class ResourceClass
    {
        public static readonly int PageSize = Convert.ToInt32(Properties.Resources.PageSize);

        /*public static T PagingIndex<T,E>(IQueryable<E> items, int page) where T : new()
        {

            IEnumerable<E> itemsPerPages = items.OrderBy(r => r.Id).Skip((page - 1) * PageSize).Take(PageSize);
            PageInfo pageInfo = new PageInfo { PageNumber=page, TotalItems= items.Count()};
            return new T { PageInfo = pageInfo, Books = itemsPerPages };
        }*/
    }
}