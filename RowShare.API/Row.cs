﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net;
using CodeFluent.Runtime.Utilities;

namespace RowShare.Api
{
    public class Row
    {
        public Guid Id { get; set; }
        public string ListId { get; set; }
        public Dictionary<string, object> Values { get; private set; }

        private List _parent;
        [JsonUtilities(IgnoreWhenSerializing = true)]
        public List Parent
        {
            get
            {
                return _parent;
            }
        }

        //public Dictionary<string, object> ValuesObject
        //{
        //    get
        //    {
        //        Dictionary<string, object> _valuesObject = new Dictionary<string, object>();

        //        if (Parent != null)
        //        {
        //            var cols = Parent.Columns;
        //            if (cols != null)
        //            {
        //                foreach (Column column in cols)
        //                {
        //                    _valuesObject.Add(column.DisplayName, Values.GetValue(column.DisplayName, column.Type, null));
        //                }
        //            }
        //        }
        //        return _valuesObject;
        //    }
        //}

        public Row()
        {
            Values = new Dictionary<string, object>();
        }

        public static Collection<Row> GetRowsByList(List list)
        {
            if (list == null)
                return null;

            Collection<Row> rows = GetRowsByListId(list.Id.ToString().Replace("-", ""));
            foreach (Row row in rows)
            {
                row._parent = list;
            }
            return rows;
        }
        public static Collection<Row> GetRowsByListId(string id)
        {
            string url = string.Format(CultureInfo.CurrentCulture, "https://www.rowshare.com/api/row/loadForParent/{0}", id);
            string json;
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString(url);
            }
            return JsonUtilities.Deserialize<Collection<Row>>(json, Utility.DefaultOptions);

        }
        public static Row GetRowById(string id)
        {
            string url = string.Format(CultureInfo.CurrentCulture, "https://www.rowshare.com/api/row/load/{0}", id);
            string json;
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString(url);
            }
            return JsonUtilities.Deserialize<Row>(json, Utility.DefaultOptions);
        }
    }
}
