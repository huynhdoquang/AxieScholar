using System.Collections.Generic;

namespace Net.HungryBug.Core.Utility
{
    public class IntegerInterpolate
    {
        private KeyValuePair<int, int>[] baseValues;

        public IntegerInterpolate(KeyValuePair<int, int>[] baseValues)
        {
            this.baseValues = baseValues;

            if (this.baseValues == null)
                throw new System.Exception("Can not create IntegerInterpulation, The base values can not be null");

            if (this.baseValues.Length < 1)
                throw new System.Exception("Can not create IntegerInterpulation, please provide atleast one pair of template values.");
        }

        public int Interpolate(int key)
        {
            if (this.baseValues.Length == 1)
                return this.baseValues[0].Value;


            //if match the end value.
            if (key == this.baseValues[this.baseValues.Length - 1].Key)
                return this.baseValues[this.baseValues.Length - 1].Value;

            KeyValuePair<int, int>? pair = null;
            KeyValuePair<int, int>? nextPair = null;

            if (key < this.baseValues[0].Key || key > this.baseValues[this.baseValues.Length - 1].Key)
            {
                throw new System.Exception("Interpolate value out of range");
            }
            else
            {
                //in range
                for (int i = 0; i < this.baseValues.Length - 1; i++)
                {
                    pair = this.baseValues[i];
                    nextPair = this.baseValues[i + 1];

                    if (key == pair.Value.Key)
                        return pair.Value.Value;

                    if (key > pair.Value.Key && key < nextPair.Value.Key)
                        break;
                }
            }

            float ratio = 1.0f * (key - pair.Value.Key) / (nextPair.Value.Key - pair.Value.Key);
            return (int)UnityEngine.Mathf.Lerp(pair.Value.Value, nextPair.Value.Value, ratio);
        }
    }

    public class ArrayIntegerInterpolate
    {
        private KeyValuePair<int, int[]>[] baseValues;

        public ArrayIntegerInterpolate(KeyValuePair<int, int[]>[] baseValues)
        {
            this.baseValues = baseValues;

            if (this.baseValues == null)
                throw new System.Exception("Can not create LinearArrayValueInterpolate, The base values can not be null");

            if (this.baseValues.Length < 1)
                throw new System.Exception("Can not create LinearArrayValueInterpolate, please provide atleast one pair of template values.");
        }

        public int[] Interpolate(int key)
        {
            if (this.baseValues.Length == 1)
                return this.baseValues[0].Value;

            //if match the end value.
            if (key == this.baseValues[this.baseValues.Length - 1].Key)
                return this.baseValues[this.baseValues.Length - 1].Value;

            KeyValuePair<int, int[]>? pair = null;
            KeyValuePair<int, int[]>? nextPair = null;

            if (key < this.baseValues[0].Key)
            {
                return this.baseValues[0].Value;
            }
            else if (key > this.baseValues[this.baseValues.Length - 1].Key)
            {
                return this.baseValues[this.baseValues.Length - 1].Value;
            }
            else
            {
                //in range
                for (int i = 0; i < this.baseValues.Length - 1; i++)
                {
                    pair = this.baseValues[i];
                    nextPair = this.baseValues[i + 1];

                    if (key == pair.Value.Key)
                        return pair.Value.Value;

                    if (key > pair.Value.Key && key < nextPair.Value.Key)
                        break;
                }
            }

            var start = pair.Value;
            var end = nextPair.Value;
            int count = start.Value.Length;
            //No mached item found, now interpolate.

            int[] result = new int[count];
            for (int i = 0; i < count; i++)
            {
                float ratio = 1.0f * (key - start.Key) / (end.Key - start.Key);
                result[i] = (int)UnityEngine.Mathf.Lerp(start.Value[i], end.Value[i], ratio);
            }

            return result;
        }
    }
}
