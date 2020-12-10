using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Net.HungryBug.Core.UI.ReusableCollection
{
    /// <summary>
    /// 
    /// </summary>
    public enum DataMode
    {
        Flatten,
        Group,
    }

    /// <summary>
    /// 
    /// </summary>
    public class DataGroup
    {
        private readonly ICellData[] Cells;
        public readonly int Index;
        public ICellData[][] Records { get; private set; }

        /// <summary>
        /// Create <see cref="DataGroup"/>.
        /// </summary>
        public DataGroup(int groupIndex, ICellData[] cells)
        {
            this.Index = groupIndex;
            this.Cells = cells;
        }

        /// <summary>
        /// Rebuild list of record
        /// </summary>
        /// <param name="cellsPerRecord"></param>
        public ICellData[][] BuildRecords(int cellsPerRecord)
        {
            var lst = new List<ICellData[]>();
            List<ICellData> record = null;

            //build cells by record.
            for (int i = 0; i < this.Cells.Length; i++)
            {
                if (i % cellsPerRecord == 0)
                {
                    if (record != null)
                    {
                        lst.Add(record.ToArray());
                    }

                    record = new List<ICellData>();
                }

                record.Add(this.Cells[i]);
            }

            //add final record.
            if (record != null)
            {
                lst.Add(record.ToArray());
                record = null;
            }

            //then collect records.
            this.Records = lst.ToArray();
            return this.Records;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CollectionSource
    {
        private readonly bool defaultExpand;
        private readonly Dictionary<int, bool> expandingState;
        public readonly DataMode Mode = DataMode.Flatten;
        public DataGroup[] Groups { get; private set; }

        /// <summary>
        /// Create collection data source.
        /// </summary>
        public CollectionSource(DataMode mode, bool defaultExpand = true)
        {
            this.Mode = mode;
            this.defaultExpand = defaultExpand;
            this.expandingState = new Dictionary<int, bool>();
        }

        /// <summary>
        /// Sets source cells on <see cref="DataMode.Flatten"/>
        /// </summary>
        public void SetData(ICellData[] cells)
        {
            this.Groups = new DataGroup[1];
            this.Groups[1] = new DataGroup(0, cells);
        }

        /// <summary>
        /// Sets source groups on <see cref="DataMode.Group"/>
        /// </summary>
        public void SetData(DataGroup[] groups)
        {
            var validation = new HashSet<int>();
            for (int i = 0; i < groups.Length; i++)
            {
                if (!validation.Add(groups[i].Index))
                {
                    throw new System.InvalidOperationException("[CollectionSource][SetGroups] found duplicate group index, " + groups[i].Index);
                }
            }

            this.Groups = groups;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsExpanding(int groupIndex)
        {
            if (this.expandingState.TryGetValue(groupIndex, out var val))
            {
                return val;
            }
            else
            {
                for (int i = 0; i < this.Groups.Length; i++)
                {
                    if (groupIndex == this.Groups[i].Index)
                    {
                        this.expandingState.Add(groupIndex, this.defaultExpand);
                        return this.defaultExpand;
                    }
                }

                throw new System.InvalidOperationException("[CollectionSource][IsExpanding] Group index not found, " + groupIndex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetExpanding(int groupIndex, bool isExpand)
        {
            if (this.expandingState.ContainsKey(groupIndex))
            {
                this.expandingState[groupIndex] = isExpand;
            }
            else
            {
                this.expandingState.Add(groupIndex, isExpand);
            }
        }
    }
}
