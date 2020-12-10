using Net.HungryBug.Core.Attribute;
using System.Collections.Generic;
using UnityEngine;

namespace Net.HungryBug.Core.UI.ReusableCollection.Example
{
    public class ExampleUICollection : UIResolver
    {
        [Header("Reusable")]
        [UIOutlet("@ResuableGrid")]
        public UICollection ReusableGrid;

        [UIOutlet("@ResuableList")]
        public UICollection ReusableList;

        [UIOutlet("@GroupGrid")]
        public UICollection GroupGrid;

        [Header("Simple")]
        [UIOutlet("@SimpleGrid")]
        public UICollection SimpleGrid;

        [UIOutlet("@SimpleList")]
        public UICollection SimpleList;

        private void Start()
        {
            this.ReusableGrid.SetData(this.CreateCells(1000));
            this.ReusableList.SetData(this.CreateCells(1000));
            this.GroupGrid.SetData(this.CreateCells(1000));

            this.SimpleGrid.SetData(this.CreateCells(20));
            this.SimpleList.SetData(this.CreateCells(20));
        }

        /// <summary>
        /// Create cell data.
        /// </summary>
        private List<ICellData> CreateCells(int count)
        {
            var lst = new List<ICellData>();
            for (int i = 0; i < count; i++)
            {
                lst.Add(new TextCellData()
                {
                    UniqueId = i,
                    GroupId = UnityEngine.Random.Range(0, 10),
                });
            }

            return lst;
        }
    }
}
