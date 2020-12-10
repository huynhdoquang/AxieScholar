using Net.HungryBug.Core.Attribute;
using System.Collections.Generic;
using UnityEngine;

namespace Net.HungryBug.Core.UI.ReusableCollection.Example
{
    public class ExampleScrollView : UIResolver
    {
        [Header("Reusable")]
        [UIOutlet("@ResuableGrid")]
        public UIReusableScrollView ReusableGrid;
        
        [UIOutlet("@ResuableList")]
        public UIReusableScrollView ReusableList;

        [Header("Simple")]
        [UIOutlet("@SimpleGrid")]
        public UISimpleScrollView SimpleGrid;

        [UIOutlet("@SimpleList")]
        public UISimpleScrollView SimpleList;

        [UIOutlet("@GroupGrid")]
        public UIGroupScrollView GroupGrid;

        private void Start()
        {
            this.ReusableGrid.DataContext.AddRange(this.CreateCells(1000));
            this.ReusableList.DataContext.AddRange(this.CreateCells(1000));
            this.GroupGrid.DataContext.AddRange(this.CreateCells(1000));

            this.SimpleGrid.DataContext.AddRange(this.CreateCells(20));
            this.SimpleList.DataContext.AddRange(this.CreateCells(20));
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
                    GroupId  = UnityEngine.Random.Range(0, 10),
                });
            }

            return lst;
        }
    }
}
