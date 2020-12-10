using System;

namespace Net.HungryBug.Core.Security
{
    public struct SecureLong : IComparable, IComparable<SecureLong>
    {
        public static SecureLong Default { get { return new SecureLong(0); } }
        public static SecureLong MinValue { get { return new SecureLong(long.MinValue); } }
        public static SecureLong MaxValue { get { return new SecureLong(long.MaxValue); } }

        public SecureLong(long defaultValue = 0) { this.data = ByteProtect.EncryptBytes(System.BitConverter.GetBytes(defaultValue)); }

        private byte[] data;
        /// <summary>
        /// Gets original value.
        /// </summary>
        public long Value
        {
            get
            {
                if (this.data == null)
                {
                    this.data = ByteProtect.EncryptBytes(System.BitConverter.GetBytes(0));
                }

                return System.BitConverter.ToInt64(ByteProtect.DecryptBytes(this.data), 0);
            }
            set
            {
                this.data = ByteProtect.EncryptBytes(System.BitConverter.GetBytes(value));
            }
        }

        #region [Comparer]
        public int CompareTo(object obj)
        {
            var source = (SecureLong)obj;
            return this.Value.CompareTo(source.Value);
        }

        public int CompareTo(SecureLong other)
        {
            return this.Value.CompareTo(other.Value);
        } 
        #endregion

        #region [Assign]
        public static implicit operator SecureLong(long value)
        {
            return new SecureLong(value);
        }

        public static implicit operator long(SecureLong value)
        {
            return value.Value;
        }

        public static implicit operator SecureLong(int value)
        {
            return new SecureLong(value);
        }
        #endregion

        #region [Operator With Self]
        public static SecureLong operator +(SecureLong a, SecureLong b)
        {
            return new SecureLong(a.Value + b.Value);
        }

        public static SecureLong operator -(SecureLong a, SecureLong b)
        {
            return new SecureLong(a.Value - b.Value);
        }

        public static SecureLong operator *(SecureLong a, SecureLong b)
        {
            return new SecureLong(a.Value * b.Value);
        }

        public static SecureLong operator /(SecureLong a, SecureLong b)
        {
            return new SecureLong(a.Value / b.Value);
        }

        public static SecureLong operator ++(SecureLong a)
        {
            return new SecureLong(a.Value++);
        }

        public static SecureLong operator --(SecureLong a)
        {
            return new SecureLong(a.Value--);
        }

        public static SecureLong operator %(SecureLong a, SecureLong b)
        {
            return new SecureLong(a.Value % b.Value);
        }

        public static bool operator ==(SecureLong a, SecureLong b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(SecureLong a, SecureLong b)
        {
            return a.Value != b.Value;
        }

        public static bool operator >(SecureLong a, SecureLong b)
        {
            return a.Value > b.Value;
        }

        public static bool operator <(SecureLong a, SecureLong b)
        {
            return a.Value < b.Value;
        }

        public static bool operator >=(SecureLong a, SecureLong b)
        {
            return a.Value >= b.Value;
        }

        public static bool operator <=(SecureLong a, SecureLong b)
        {
            return a.Value <= b.Value;
        }

        public override bool Equals(object b)
        {
            return this.Value.Equals(b);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }
        #endregion

        #region [Operator With SecureInt]
        public static SecureLong operator +(SecureLong a, SecureInt b)
        {
            return new SecureLong(a.Value + b.Value);
        }

        public static SecureLong operator -(SecureLong a, SecureInt b)
        {
            return new SecureLong(a.Value - b.Value);
        }

        public static SecureLong operator *(SecureLong a, SecureInt b)
        {
            return new SecureLong(a.Value * b.Value);
        }

        public static SecureLong operator /(SecureLong a, SecureInt b)
        {
            return new SecureLong(a.Value / b.Value);
        }

        public static SecureLong operator %(SecureLong a, SecureInt b)
        {
            return new SecureLong(a.Value % b.Value);
        }

        public static bool operator ==(SecureLong a, SecureInt b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(SecureLong a, SecureInt b)
        {
            return a.Value != b.Value;
        }

        public static bool operator >(SecureLong a, SecureInt b)
        {
            return a.Value > b.Value;
        }

        public static bool operator <(SecureLong a, SecureInt b)
        {
            return a.Value < b.Value;
        }

        public static bool operator >=(SecureLong a, SecureInt b)
        {
            return a.Value >= b.Value;
        }

        public static bool operator <=(SecureLong a, SecureInt b)
        {
            return a.Value <= b.Value;
        }
        #endregion

        #region [Operator With SecureFloat]
        public static SecureFloat operator +(SecureLong a, SecureFloat b)
        {
            return new SecureFloat(a.Value + b.Value);
        }

        public static SecureFloat operator -(SecureLong a, SecureFloat b)
        {
            return new SecureFloat(a.Value - b.Value);
        }

        public static SecureFloat operator *(SecureLong a, SecureFloat b)
        {
            return new SecureFloat(a.Value * b.Value);
        }

        public static SecureFloat operator /(SecureLong a, SecureFloat b)
        {
            return new SecureFloat(a.Value / b.Value);
        }

        public static SecureFloat operator %(SecureLong a, SecureFloat b)
        {
            return new SecureFloat(a.Value % b.Value);
        }

        public static bool operator ==(SecureLong a, SecureFloat b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(SecureLong a, SecureFloat b)
        {
            return a.Value != b.Value;
        }

        public static bool operator >(SecureLong a, SecureFloat b)
        {
            return a.Value > b.Value;
        }

        public static bool operator <(SecureLong a, SecureFloat b)
        {
            return a.Value < b.Value;
        }

        public static bool operator >=(SecureLong a, SecureFloat b)
        {
            return a.Value >= b.Value;
        }

        public static bool operator <=(SecureLong a, SecureFloat b)
        {
            return a.Value <= b.Value;
        }
        #endregion

        #region [Operator With SecureDouble]
        public static SecureDouble operator +(SecureLong a, SecureDouble b)
        {
            return new SecureDouble(a.Value + b.Value);
        }

        public static SecureDouble operator -(SecureLong a, SecureDouble b)
        {
            return new SecureDouble(a.Value - b.Value);
        }

        public static SecureDouble operator *(SecureLong a, SecureDouble b)
        {
            return new SecureDouble(a.Value * b.Value);
        }

        public static SecureDouble operator /(SecureLong a, SecureDouble b)
        {
            return new SecureDouble(a.Value / b.Value);
        }

        public static SecureDouble operator %(SecureLong a, SecureDouble b)
        {
            return new SecureDouble(a.Value % b.Value);
        }

        public static bool operator ==(SecureLong a, SecureDouble b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(SecureLong a, SecureDouble b)
        {
            return a.Value != b.Value;
        }

        public static bool operator >(SecureLong a, SecureDouble b)
        {
            return a.Value > b.Value;
        }

        public static bool operator <(SecureLong a, SecureDouble b)
        {
            return a.Value < b.Value;
        }

        public static bool operator >=(SecureLong a, SecureDouble b)
        {
            return a.Value >= b.Value;
        }

        public static bool operator <=(SecureLong a, SecureDouble b)
        {
            return a.Value <= b.Value;
        }
        #endregion

        #region [Operator With Int]
        public static SecureLong operator +(SecureLong a, int b)
        {
            return new SecureLong(a.Value + b);
        }

        public static SecureLong operator -(SecureLong a, int b)
        {
            return new SecureLong(a.Value - b);
        }

        public static SecureLong operator *(SecureLong a, int b)
        {
            return new SecureLong(a.Value * b);
        }

        public static SecureLong operator /(SecureLong a, int b)
        {
            return new SecureLong(a.Value / b);
        }

        public static SecureLong operator %(SecureLong a, int b)
        {
            return new SecureLong(a.Value % b);
        }

        public static bool operator ==(SecureLong a, int b)
        {
            return a.Value == b;
        }

        public static bool operator !=(SecureLong a, int b)
        {
            return a.Value != b;
        }

        public static bool operator >(SecureLong a, int b)
        {
            return a.Value > b;
        }

        public static bool operator <(SecureLong a, int b)
        {
            return a.Value < b;
        }

        public static bool operator >=(SecureLong a, int b)
        {
            return a.Value >= b;
        }

        public static bool operator <=(SecureLong a, int b)
        {
            return a.Value <= b;
        }
        #endregion

        #region [Operator With Long]
        public static SecureLong operator +(SecureLong a, long b)
        {
            return new SecureLong(a.Value + b);
        }

        public static SecureLong operator -(SecureLong a, long b)
        {
            return new SecureLong(a.Value - b);
        }

        public static SecureLong operator *(SecureLong a, long b)
        {
            return new SecureLong(a.Value * b);
        }

        public static SecureLong operator /(SecureLong a, long b)
        {
            return new SecureLong(a.Value / b);
        }

        public static SecureLong operator %(SecureLong a, long b)
        {
            return new SecureLong(a.Value % b);
        }

        public static bool operator ==(SecureLong a, long b)
        {
            return a.Value == b;
        }

        public static bool operator !=(SecureLong a, long b)
        {
            return a.Value != b;
        }

        public static bool operator >(SecureLong a, long b)
        {
            return a.Value > b;
        }

        public static bool operator <(SecureLong a, long b)
        {
            return a.Value < b;
        }

        public static bool operator >=(SecureLong a, long b)
        {
            return a.Value >= b;
        }

        public static bool operator <=(SecureLong a, long b)
        {
            return a.Value <= b;
        }
        #endregion

        #region [Operator With Float]
        public static SecureFloat operator +(SecureLong a, float b)
        {
            return new SecureFloat(a.Value + b);
        }

        public static SecureFloat operator -(SecureLong a, float b)
        {
            return new SecureFloat(a.Value - b);
        }

        public static SecureFloat operator *(SecureLong a, float b)
        {
            return new SecureFloat(a.Value * b);
        }

        public static SecureFloat operator /(SecureLong a, float b)
        {
            return new SecureFloat(a.Value / b);
        }

        public static SecureFloat operator %(SecureLong a, float b)
        {
            return new SecureFloat(a.Value % b);
        }

        public static bool operator ==(SecureLong a, float b)
        {
            return a.Value == b;
        }

        public static bool operator !=(SecureLong a, float b)
        {
            return a.Value != b;
        }

        public static bool operator >(SecureLong a, float b)
        {
            return a.Value > b;
        }

        public static bool operator <(SecureLong a, float b)
        {
            return a.Value < b;
        }

        public static bool operator >=(SecureLong a, float b)
        {
            return a.Value >= b;
        }

        public static bool operator <=(SecureLong a, float b)
        {
            return a.Value <= b;
        }
        #endregion

        #region [Operator With Double]
        public static SecureDouble operator +(SecureLong a, double b)
        {
            return new SecureDouble(a.Value + b);
        }

        public static SecureDouble operator -(SecureLong a, double b)
        {
            return new SecureDouble(a.Value - b);
        }

        public static SecureDouble operator *(SecureLong a, double b)
        {
            return new SecureDouble(a.Value * b);
        }

        public static SecureDouble operator /(SecureLong a, double b)
        {
            return new SecureDouble(a.Value / b);
        }

        public static SecureDouble operator %(SecureLong a, double b)
        {
            return new SecureDouble(a.Value % b);
        }

        public static bool operator ==(SecureLong a, double b)
        {
            return a.Value == b;
        }

        public static bool operator !=(SecureLong a, double b)
        {
            return a.Value != b;
        }

        public static bool operator >(SecureLong a, double b)
        {
            return a.Value > b;
        }

        public static bool operator <(SecureLong a, double b)
        {
            return a.Value < b;
        }

        public static bool operator >=(SecureLong a, double b)
        {
            return a.Value >= b;
        }

        public static bool operator <=(SecureLong a, double b)
        {
            return a.Value <= b;
        }
        #endregion

        #region [Int]
        public static SecureLong operator +(int a, SecureLong b)
        {
            return new SecureLong(a + b.Value);
        }

        public static SecureLong operator -(int a, SecureLong b)
        {
            return new SecureLong(a - b.Value);
        }

        public static SecureLong operator *(int a, SecureLong b)
        {
            return new SecureLong(a * b.Value);
        }

        public static SecureLong operator /(int a, SecureLong b)
        {
            return new SecureLong(a / b.Value);
        }

        public static SecureLong operator %(int a, SecureLong b)
        {
            return new SecureLong(a % b.Value);
        }

        public static bool operator ==(int a, SecureLong b)
        {
            return a == b.Value;
        }

        public static bool operator !=(int a, SecureLong b)
        {
            return a != b.Value;
        }

        public static bool operator >(int a, SecureLong b)
        {
            return a > b.Value;
        }

        public static bool operator <(int a, SecureLong b)
        {
            return a < b.Value;
        }

        public static bool operator >=(int a, SecureLong b)
        {
            return a >= b.Value;
        }

        public static bool operator <=(int a, SecureLong b)
        {
            return a <= b.Value;
        }
        #endregion

        #region [Long]
        public static SecureLong operator +(long a, SecureLong b)
        {
            return new SecureLong(a + b.Value);
        }

        public static SecureLong operator -(long a, SecureLong b)
        {
            return new SecureLong(a - b.Value);
        }

        public static SecureLong operator *(long a, SecureLong b)
        {
            return new SecureLong(a * b.Value);
        }

        public static SecureLong operator /(long a, SecureLong b)
        {
            return new SecureLong(a / b.Value);
        }

        public static SecureLong operator %(long a, SecureLong b)
        {
            return new SecureLong(a % b.Value);
        }

        public static bool operator ==(long a, SecureLong b)
        {
            return a == b.Value;
        }

        public static bool operator !=(long a, SecureLong b)
        {
            return a != b.Value;
        }

        public static bool operator >(long a, SecureLong b)
        {
            return a > b.Value;
        }

        public static bool operator <(long a, SecureLong b)
        {
            return a < b.Value;
        }

        public static bool operator >=(long a, SecureLong b)
        {
            return a >= b.Value;
        }

        public static bool operator <=(long a, SecureLong b)
        {
            return a <= b.Value;
        }
        #endregion

        #region [Float]
        public static SecureFloat operator +(float a, SecureLong b)
        {
            return new SecureFloat(a + b.Value);
        }

        public static SecureFloat operator -(float a, SecureLong b)
        {
            return new SecureFloat(a - b.Value);
        }

        public static SecureFloat operator *(float a, SecureLong b)
        {
            return new SecureFloat(a * b.Value);
        }

        public static SecureFloat operator /(float a, SecureLong b)
        {
            return new SecureFloat(a / b.Value);
        }

        public static SecureFloat operator %(float a, SecureLong b)
        {
            return new SecureFloat(a % b.Value);
        }

        public static bool operator ==(float a, SecureLong b)
        {
            return a == b.Value;
        }

        public static bool operator !=(float a, SecureLong b)
        {
            return a != b.Value;
        }

        public static bool operator >(float a, SecureLong b)
        {
            return a > b.Value;
        }

        public static bool operator <(float a, SecureLong b)
        {
            return a < b.Value;
        }

        public static bool operator >=(float a, SecureLong b)
        {
            return a >= b.Value;
        }

        public static bool operator <=(float a, SecureLong b)
        {
            return a <= b.Value;
        }
        #endregion

        #region [Double]
        public static SecureDouble operator +(double a, SecureLong b)
        {
            return new SecureDouble(a + b.Value);
        }

        public static SecureDouble operator -(double a, SecureLong b)
        {
            return new SecureDouble(a - b.Value);
        }

        public static SecureDouble operator *(double a, SecureLong b)
        {
            return new SecureDouble(a * b.Value);
        }

        public static SecureDouble operator /(double a, SecureLong b)
        {
            return new SecureDouble(a / b.Value);
        }

        public static SecureDouble operator %(double a, SecureLong b)
        {
            return new SecureDouble(a % b.Value);
        }

        public static bool operator ==(double a, SecureLong b)
        {
            return a == b.Value;
        }

        public static bool operator !=(double a, SecureLong b)
        {
            return a != b.Value;
        }

        public static bool operator >(double a, SecureLong b)
        {
            return a > b.Value;
        }

        public static bool operator <(double a, SecureLong b)
        {
            return a < b.Value;
        }

        public static bool operator >=(double a, SecureLong b)
        {
            return a >= b.Value;
        }

        public static bool operator <=(double a, SecureLong b)
        {
            return a <= b.Value;
        }
        #endregion
    }
}


