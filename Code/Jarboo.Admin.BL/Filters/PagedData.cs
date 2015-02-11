using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Jarboo.Admin.DAL.Entities;

namespace Jarboo.Admin.BL.Filters
{
    /// <summary>
    /// The paged data.
    /// </summary>
    public class PagedData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedData"/> class.
        /// </summary>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <param name="pageNumber">
        /// The page number. Starts from 1.
        /// </param>
        /// <param name="totalItems">
        /// The total items.
        /// </param>
        public PagedData(int pageSize, int pageNumber, int totalItems)
        {
            this.PageSize = pageSize;
            this.PageNumber = pageNumber;
            this.TotalItems = totalItems;
        }

        /// <summary>
        /// Gets the page size.
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// Gets the page number. Starts from 1.
        /// </summary>
        public int PageNumber { get; private set; }

        /// <summary>
        /// Gets the total items.
        /// </summary>
        public int TotalItems { get; private set; }

        /// <summary>
        /// Creates <see cref="PagedData"/> with no paging. All data on one page.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <typeparam name="TData">
        /// Source type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="PagedData"/>.
        /// </returns>
        public static PagedData<TData> AllOnOnePage<TData>(IQueryable<TData> source)
        {
            var list = source.ToList();

            return new PagedData<TData>(
                list,
                int.MaxValue,
                1,
                list.Count);
        }

        public static async Task<PagedData<TData>> AllOnOnePageAsync<TData>(IQueryable<TData> source)
        {
            var list = await source.ToListAsync();

            return new PagedData<TData>(
                list,
                int.MaxValue,
                1,
                list.Count);
        }

        /// <summary>
        ///  Creates <see cref="PagedData"/>.
        /// </summary>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <param name="pageNumber">
        /// The page number.
        /// </param>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <typeparam name="TEntity">
        /// Source type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="PagedData"/>.
        /// </returns>
        public static PagedData<TEntity> Create<TEntity>(int pageSize, int pageNumber, IQueryable<TEntity> source)
        {
            return new PagedData<TEntity>(
                source.Skip(pageNumber * pageSize).Take(pageSize).ToList(),
                pageSize,
                pageNumber,
                source.Count());
        }

        public static async Task<PagedData<TEntity>> CreateAsync<TEntity>(int pageSize, int pageNumber, IQueryable<TEntity> source)
        {
            return new PagedData<TEntity>(
                await source.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync(),
                pageSize,
                pageNumber,
                source.Count());
        }

        /// <summary>
        ///  Creates <see cref="PagedData"/>.
        /// </summary>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <param name="pageNumber">
        /// The page number.
        /// </param>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <typeparam name="TEntity">
        /// Source type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="PagedData"/>.
        /// </returns>
        public static PagedData<TEntity> Create<TEntity>(int pageSize, int pageNumber, IEnumerable<TEntity> source)
        {
            return new PagedData<TEntity>(
                source.Skip(pageNumber * pageSize).Take(pageSize).ToList(),
                pageSize,
                pageNumber,
                source.Count());
        }
    }

    /// <summary>
    /// The paged data.
    /// </summary>
    /// <typeparam name="TData">
    /// Paged data type.
    /// </typeparam>
    public class PagedData<TData> : PagedData, IEnumerable<TData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedData{TData}"/> class.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <param name="pageNumber">
        /// The page number. Starts from 1.
        /// </param>
        /// <param name="totalItems">
        /// The total items.
        /// </param>
        public PagedData(List<TData> data, int pageSize, int pageNumber, int totalItems)
            : base(pageSize, pageNumber, totalItems)
        {
            this.Data = data;
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        public List<TData> Data { get; private set; }

        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerator"/>.
        /// </returns>
        public IEnumerator<TData> GetEnumerator()
        {
            return this.Data.GetEnumerator();
        }

        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerator"/>.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public PagedData<T> Convert<T>(Func<TData, T> converter)
        {
            return new PagedData<T>(Data.Select(converter).ToList(), PageSize, PageNumber, TotalItems);
        }
    }
}
