//-----------------------------------------------------------------------
// <copyright file="AlphaKeyGroup.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Phone.Globalization;

namespace AkvelonContacts.WindowsPhone
{
    /// <summary>
    /// Helper class for Grouping.
    /// </summary>
    /// <typeparam name="T">Type for grouping.</typeparam>
    public class AlphaKeyGroup<T> : List<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlphaKeyGroup{T}" /> class.
        /// </summary>
        /// <param name="key">The key for this group.</param>
        public AlphaKeyGroup(string key)
        {
            this.Key = key;
        }

        /// <summary>
        /// The delegate that is used to get the key information.
        /// </summary>
        /// <param name="item">An object of type <typeparamref name="T"/></param>
        /// <returns>The key value to use for this object</returns>
        public delegate string GetKeyDelegate(T item);

        /// <summary>
        /// Gets the Key of this group.
        /// </summary>
        public string Key
        {
            get;
            private set;
        }

        /// <summary>
        /// Create a list of AlphaGroup <typeparamref name="T"/> with keys set by a SortedLocaleGrouping.
        /// </summary>
        /// <param name="items">The items to place in the groups.</param>
        /// <param name="ci">The CultureInfo to group and sort by.</param>
        /// <param name="getKey">A delegate to get the key from an item.</param>
        /// <param name="sort">Will sort the data if true.</param>
        /// <returns>An items source for a LongListSelector</returns>
        public static List<AlphaKeyGroup<T>> CreateGroups(IEnumerable<T> items, CultureInfo ci, GetKeyDelegate getKey, bool sort)
        {
            SortedLocaleGrouping slg = new SortedLocaleGrouping(ci);
            List<AlphaKeyGroup<T>> list = CreateGroups(slg);

            foreach (T item in items)
            {
                int index = 0;

                index = slg.GetGroupIndex(getKey(item));

                if (index >= 0 && index < list.Count)
                {
                    list[index].Add(item);
                }
            }

            if (sort)
            {
                foreach (AlphaKeyGroup<T> group in list)
                {
                    group.Sort((c0, c1) => { return ci.CompareInfo.Compare(getKey(c0), getKey(c1)); });
                }
            }

            return list;
        }

        /// <summary>
        /// Create a list of AlphaGroup <typeparamref name="T"/> with keys set by a SortedLocaleGrouping.
        /// </summary>
        /// <param name="slg">The sorted locale grouping.</param>
        /// <returns>The items source for a LongListSelector.</returns>
        private static List<AlphaKeyGroup<T>> CreateGroups(SortedLocaleGrouping slg)
        {
            List<AlphaKeyGroup<T>> list = new List<AlphaKeyGroup<T>>();

            foreach (string key in slg.GroupDisplayNames)
            {
                list.Add(new AlphaKeyGroup<T>(key));
            }

            return list;
        }
    }
}