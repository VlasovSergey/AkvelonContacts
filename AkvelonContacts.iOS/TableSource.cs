//-----------------------------------------------------------------------
// <copyright file="TableSource.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AkvelonContacts.Common;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace AkvelonContacts.iOS
{
    /// <summary>
    /// Class contains data for display for table.
    /// </summary>
    public class TableSource : UITableViewSource
    {
        /// <summary>
        /// Contacts for display.
        /// </summary>
        private List<Contact> tableItems;

        /// <summary>
        /// Cell id.
        /// </summary>
        private string cellIdentifier = "TableCell";

        /// <summary>
        /// Initializes a new instance of the <see cref="TableSource" /> class.
        /// </summary>
        /// <param name="items">Contact list.</param>
        public TableSource(List<Contact> items)
        {
            this.tableItems = items;
        }

        /// <summary>
        /// Gets number of rows in selection.
        /// </summary>
        /// <param name="tableview">Table view.</param>
        /// <param name="section">Section table.</param>
        /// <returns>Number of rows in selection</returns>
        public override int RowsInSection(UITableView tableview, int section)
        {
            return this.tableItems.Count;
        }

        /// <summary>
        /// Gets cell by index.
        /// </summary>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        /// <returns>Cell by index.</returns>
        public override UITableViewCell GetCell(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(this.cellIdentifier);

            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, this.cellIdentifier);
            }

            cell.TextLabel.Text = this.tableItems[indexPath.Row].FullName;
            return cell;
        }
    }
}