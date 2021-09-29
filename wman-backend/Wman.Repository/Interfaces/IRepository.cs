using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wman.Repository
{
    /// <summary>
    /// Common interface for all repositories
    /// </summary>
    /// <typeparam name="TReturnType">.</typeparam>
    /// <typeparam name="TKeyType">.</typeparam>
    public interface IRepository<TReturnType, TKeyType>
    {
        /// <summary>
        /// Return a single instance
        /// </summary>
        /// <param name="key">identifier of the instance</param>
        /// <returns></returns>
        TReturnType GetOne(TKeyType key);

        /// <summary>
        /// Gets all the objects in the repository
        /// </summary>
        /// <returns>A collection of the objects in the repository</returns>
        IQueryable<TReturnType> GetAll();

        /// <summary>
        /// Add a new item to the repository
        /// </summary>
        /// <param name="element">The item to add</param>
        void Add(TReturnType element);

        /// <summary>
        /// Delete an item from the repository
        /// </summary>
        /// <param name="element">The item to be deleted</param>
        void Delete(TKeyType element);

        /// <summary>
        /// Update an item in the repository
        /// </summary>
        /// <param name="oldKey">Old key of the item</param>
        /// <param name="element">Updated object</param>
        void Update(TKeyType oldKey ,TReturnType element);
    }
}
