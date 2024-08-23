using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Helpers
{
    public class PaginationList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }

        public PaginationList(int totalCount, int pageNumber, int pageSize, List<T> items)
        {
            CurrentPage = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int) Math.Ceiling(totalCount / (double) pageSize);
            AddRange(items);
        }

        // Factory method pattern
        // 因为我们需要封装分页并从数据库中获取数据的异步操作，所以构造函数与工厂函数是分离的

        public static async Task<PaginationList<T>> CreateAsync(int pageNumber, int pageSize, IQueryable<T> result)     // Factory method pattern
        {
            int totalCount = await result.CountAsync();
            
            // pagination
            // skip
            var skipAmount = (pageNumber - 1) * pageSize;
            result = result.Skip(skipAmount);
            // 以 pageSize 为标准显示一定量的数据
            result = result.Take(pageSize);

            var data = await result.ToListAsync();

            return new PaginationList<T>(totalCount, pageNumber, pageSize, data);
        }

    }
}
