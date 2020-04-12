using Domain.NoSQL.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.NoSQL
{
    /// <summary>
    /// Generic repository to work with mongodb nested collections
    /// </summary>
    /// <typeparam name="TRootEntity">Root entity</typeparam>
    /// <typeparam name="TChildEntity">Entity of arr item</typeparam>
    public interface IChildRepository<TRootEntity,TChildEntity> 
        where TRootEntity : class, IRootEntityBase, new() 
        where TChildEntity : class, IChildEntityBase, new()
    {
        /// <summary>
        /// Insert one child entity in root entities
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <param name="childEntity">Iserted entity</param>
        /// <param name="path">Lists of tuples (array name, id of item we need from this array) </param>
        /// <param name="arrName">Array name in which we istert new item</param>
        /// Path example: push in arr1.$[i].arr2.$[j].arrName
        /// <returns>UpdateResult</returns>
        Task<UpdateResult> PushAsync(Expression<Func<TRootEntity, bool>> predicate, TChildEntity childEntity, IEnumerable<(string nestedArrayName, string itemId)> path, string arrName);

        /// <summary>
        /// Insert one child entity in one root entity
        /// </summary>
        /// <param name="rootId">Root entity id</param>
        /// <param name="childEntity">Iserted entity</param>
        /// <param name="path">Lists of tuples (array name, id of item we need from this array) </param>
        /// <param name="arrName">Array name in which we istert new item</param>
        /// Path example: push in arr1.$[i].arr2.$[j].arrName 
        /// <returns>UpdateResult</returns>
        Task<UpdateResult> PushAsync(string rootId, TChildEntity childEntity, IEnumerable<(string nestedArrayName, string itemId)> path, string arrName);

        /// <summary>
        /// Insert one child entity in root entities
        /// </summary>
        /// <param name="filterEntity">Root entity</param>
        /// <param name="childEntity">Iserted entity</param>
        /// <param name="path">Lists of tuples (array name, id of item we need from this array) </param>
        /// <param name="arrName">Array name in which we istert new item</param>
        /// Path example: push in arr1.$[i].arr2.$[j].arrName 
        /// <returns>UpdateResult</returns>
        Task<UpdateResult> PushAsync(TRootEntity filterEntity, TChildEntity childEntity, IEnumerable<(string nestedArrayName, string itemId)> path, string arrName);

        /// <summary>
        /// Update fields in one child entity from root entities
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <param name="childEntity">Updated values</param>
        /// <param name="path">Lists of tuples (array name, id of item we need from this array) </param>
        /// Path example: set in arr1.$[i].arr2.$[j].fieldName = newValue 
        /// <returns>UpdateResult</returns>
        Task<UpdateResult> SetAsync(Expression<Func<TRootEntity, bool>> predicate, TChildEntity childEntity, IEnumerable<(string nestedArrayName, string itemId)> path);

        /// <summary>
        /// Update field in one child entity from root entities
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <param name="fieldName">Updated field name</param>
        /// <param name="fieldNewValue">Updated field values</param>
        /// <param name="path">Lists of tuples (array name, id of item we need from this array) </param>
        /// Path example: set in arr1.$[i].arr2.$[j].fieldName = newValue 
        /// <returns>UpdateResult</returns>
        Task<UpdateResult> SetAsync(Expression<Func<TRootEntity, bool>> predicate, string fieldName, string fieldNewValue, IEnumerable<(string nestedArrayName, string itemId)> path);

        /// <summary>
        /// Update fields in one child entity from one root entity
        /// </summary>
        /// <param name="rootId">Root entity id</param>
        /// <param name="childEntity">Updated values</param>
        /// <param name="path">Lists of tuples (array name, id of item we need from this array) </param>
        /// Path example: set in arr1.$[i].arr2.$[j].fieldName = newValue 
        /// <returns>UpdateResult</returns>
        Task<UpdateResult> SetAsync(string rootId, TChildEntity childEntity, IEnumerable<(string nestedArrayName, string itemId)> path);

        /// <summary>
        /// Update field in one child entity from one root entity
        /// </summary>
        /// <param name="rootId">Root entity id</param>
        /// <param name="fieldName">Updated field name</param>
        /// <param name="fieldNewValue">Updated field values</param>
        /// <param name="path">Lists of tuples (array name, id of item we need from this array) </param>
        /// Path example: set in arr1.$[i].arr2.$[j].fieldName = newValue 
        /// <returns>UpdateResult</returns>
        Task<UpdateResult> SetAsync(string rootId, string fieldName, string fieldNewValue, IEnumerable<(string nestedArrayName, string itemId)> path);

        /// <summary>
        /// Update fields in one child entity from root entities
        /// </summary>
        /// <param name="filterEntity">Root entity</param>
        /// <param name="childEntity">Updated values</param>
        /// <param name="path">Lists of tuples (array name, id of item we need from this array) </param>
        /// Path example: set in arr1.$[i].arr2.$[j].fieldName = newValue 
        /// <returns>UpdateResult</returns>
        Task<UpdateResult> SetAsync(TRootEntity filterEntity, TChildEntity childEntity, IEnumerable<(string nestedArrayName, string itemId)> path);

        /// <summary>
        /// Update field in one child entity from root entities
        /// </summary>
        /// <param name="filterEntity">Root entity</param>
        /// <param name="fieldName">Updated field name</param>
        /// <param name="fieldNewValue">Updated field values</param>
        /// <param name="path">Lists of tuples (array name, id of item we need from this array) </param>
        /// Path example: set in arr1.$[i].arr2.$[j].fieldName = newValue 
        /// <returns>UpdateResult</returns>
        Task<UpdateResult> SetAsync(TRootEntity filterEntity, string fieldName, string fieldNewValue, IEnumerable<(string nestedArrayName, string itemId)> path);

        /// <summary>
        /// Delete child entities from root entities
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <param name="childEntity">Deleted entities filter (by not defoult or null properties)</param>
        /// <param name="path">Lists of tuples (array name, id of item we need from this array) </param>
        /// <param name="arrName">Array name from which we delete item</param>
        /// Path example: pull from arr1.$[i].arr2.$[j] element == (condition)
        /// <returns>UpdateResult</returns>
        Task<UpdateResult> PullAsync(Expression<Func<TRootEntity, bool>> predicate, TChildEntity childEntity, IEnumerable<(string nestedArrayName, string itemId)> path, string arrName);

        /// <summary>
        /// Delete one child entitiy from root entities
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <param name="childId">Deleted entity id</param>
        /// <param name="path">Lists of tuples (array name, id of item we need from this array) </param>
        /// <param name="arrName">Array name from which we delete item</param>
        /// Path example: pull from arr1.$[i].arr2.$[j] element == (condition)
        /// <returns>UpdateResult</returns>
        Task<UpdateResult> PullAsync(Expression<Func<TRootEntity, bool>> predicate, string childId, IEnumerable<(string nestedArrayName, string itemId)> path, string arrName);

        /// <summary>
        /// Delete child entities from one root entity
        /// </summary>
        /// <param name="rootId">Root entity id</param>
        /// <param name="childEntity">Deleted entities filter (by not defoult or null properties)</param>
        /// <param name="path">Lists of tuples (array name, id of item we need from this array) </param>
        /// <param name="arrName">Array name from which we delete item</param>
        /// Path example: pull from arr1.$[i].arr2.$[j] element == (condition)
        /// <returns>UpdateResult</returns>
        Task<UpdateResult> PullAsync(string rootId, TChildEntity childEntity, IEnumerable<(string nestedArrayName, string itemId)> path, string arrName);

        /// <summary>
        /// Delete one child entity from one root entity
        /// </summary>
        /// <param name="predicate">Root id</param>
        /// <param name="childId">Deleted entity id</param>
        /// <param name="path">Lists of tuples (array name, id of item we need from this array) </param>
        /// <param name="arrName">Array name from which we delete item</param>
        /// Path example: pull from arr1.$[i].arr2.$[j] element == (condition)
        /// <returns>UpdateResult</returns>
        Task<UpdateResult> PullAsync(string rootId, string childId, IEnumerable<(string nestedArrayName, string itemId)> path, string arrName);

        /// <summary>
        /// Delete child entities from root entities
        /// </summary>
        /// <param name="filterEntity">Root entity</param>
        /// <param name="childEntity">Deleted entities filter (by not defoult or null properties)</param>
        /// <param name="path">Lists of tuples (array name, id of item we need from this array) </param>
        /// <param name="arrName">Array name from which we delete item</param>
        /// Path example: pull from arr1.$[i].arr2.$[j] element == (condition)
        /// <returns>UpdateResult</returns>
        Task<UpdateResult> PullAsync(TRootEntity filterEntity, TChildEntity childEntity, IEnumerable<(string nestedArrayName, string itemId)> path, string arrName);

        /// <summary>
        /// Delete one child entitiy from root entities
        /// </summary>
        /// <param name="filterEntity">Root entity</param>
        /// <param name="childId">Deleted entity id</param>
        /// <param name="path">Lists of tuples (array name, id of item we need from this array) </param>
        /// <param name="arrName">Array name from which we delete item</param>
        /// Path example: pull from arr1.$[i].arr2.$[j] element == (condition)
        /// <returns>UpdateResult</returns>
        Task<UpdateResult> PullAsync(TRootEntity filterEntity, string childId, IEnumerable<(string nestedArrayName, string itemId)> path, string arrName);
    }
}
