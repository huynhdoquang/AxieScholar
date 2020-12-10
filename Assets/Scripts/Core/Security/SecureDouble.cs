using System;

namespace Net.HungryBug.Core.Security
{
    public struct SecureDouble : IComparable, IComparable<SecureDouble>
    {
        public SecureDouble(double defaultValue = 0) { this.data = ByteProtect.EncryptBytes(System.BitConverter.GetBytes(defaultValue)); }

        private byte[] data;
        /// <summary>
        /// Gets original value.
        /// </summary>
        public double Value
        {
            get
            {
                if (this.data == null)
                {
                    this.data = ByteProtect.EncryptBytes(System.BitConverter.GetBytes(0));
                }

                return System.BitConverter.ToDouble(ByteProtect.DecryptBytes(this.data), 0);
            }
            set
            {
                this.data = ByteProtect.EncryptBytes(System.BitConverter.GetBytes(value));
            }
        }

        #region [Assign]
        public static implicit operator SecureDouble(double value)
        {
            return new SecureDouble(value);
        }

        public static implicit operator double(SecureDouble value)
        {
            return value.Value;
        }

        public static implicit operator SecureDouble(float value)
        {
            return new SecureDouble(value);
        }

        public static implicit operator SecureDouble(long value)
        {
            return new SecureDouble(value);
        }

        public static implicit operator SecureDouble(int value)
        {
            return new SecureDouble(value);
        }
        #endregion

        #region [Comparer]
        public int CompareTo(object obj)
        {
            var source = (SecureDouble)obj;
            return this.Value.CompareTo(source.Value);
        }

        public int CompareTo(SecureDouble other)
        {
            return this.Value.CompareTo(other.Value);
        } 
        #endregion

        #region [Operator With Self]
        public static SecureDouble operator +(SecureDouble a, SecureDouble b)
        {
            return new SecureDouble(a.Value + b.Value);
        }

        public static SecureDouble operator -(SecureDouble a, SecureDouble b)
        {
            return new SecureDouble(a.Value - b.Value);
        }

        public static SecureDouble operator *(SecureDouble a, SecureDouble b)
        {
            return new SecureDouble(a.Value * b.Value);
        }

        public static SecureDouble operator /(SecureDouble a, SecureDouble b)
        {
            return new SecureDouble(a.Value / b.Value);
        }

        public static SecureDouble operator ++(SecureDouble a)
        {
            return new SecureDouble(a.Value++);
        }

        public static SecureDouble operator --(SecureDouble a)
        {
            return new SecureDouble(a.Value--);
        }

        public static SecureDouble operator %(SecureDouble a, SecureDouble b)
        {
            return new SecureDouble(a.Value % b.Value);
        }

        public static bool operator ==(SecureDouble a, SecureDouble b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(SecureDouble a, SecureDouble b)
        {
            return a.Value != b.Value;
        }

        public static bool operator >(SecureDouble a, SecureDouble b)
        {
            return a.Value > b.Value;
        }

        public static bool operator <(SecureDouble a, SecureDouble b)
        {
            return a.Value < b.Value;
        }

        public static bool operator >=(SecureDouble a, SecureDouble b)
        {
            return a.Value >= b.Value;
        }

        public static bool operator <=(SecureDouble a, SecureDouble b)
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

        #region [Operator With SecureLong]
        public static SecureDouble operator +(SecureDouble a, SecureLong b)
        {
            return new SecureDouble(a.Value + b.Value);
        }

        public static SecureDouble operator -(SecureDouble a, SecureLong b)
        {
            return new SecureDouble(a.Value - b.Value);
        }

        public static SecureDouble operator *(SecureDouble a, SecureLong b)
        {
            return new SecureDouble(a.Value * b.Value);
        }

        public static SecureDouble operator /(SecureDouble a, SecureLong b)
        {
            return new SecureDouble(a.Value / b.Value);
        }

        public static SecureDouble operator %(SecureDouble a, SecureLong b)
        {
            return new SecureDouble(a.Value % b.Value);
        }

        public static bool operator ==(SecureDouble a, SecureLong b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(SecureDouble a, SecureLong b)
        {
            return a.Value != b.Value;
        }

        public static bool operator >(SecureDouble a, SecureLong b)
        {
            return a.Value > b.Value;
        }

        public static bool operator <(SecureDouble a, SecureLong b)
        {
            return a.Value < b.Value;
        }

        public static bool operator >=(SecureDouble a, SecureLong b)
        {
            return a.Value >= b.Value;
        }

        public static bool operator <=(SecureDouble a, SecureLong b)
        {
            return a.Value <= b.Value;
        }
        #endregion

        #region [Operator With SecureFloat]
        public static SecureDouble operator +(SecureDouble a, SecureFloat b)
        {
            return new SecureDouble(a.Value + b.Value);
        }

        public static SecureDouble operator -(SecureDouble a, SecureFloat b)
        {
            return new SecureDouble(a.Value - b.Value);
        }

        public static SecureDouble operator *(SecureDouble a, SecureFloat b)
        {
            return new SecureDouble(a.Value * b.Value);
        }

        public static SecureDouble operator /(SecureDouble a, SecureFloat b)
        {
            return new SecureDouble(a.Value / b.Value);
        }

        public static SecureDouble operator %(SecureDouble a, SecureFloat b)
        {
            return new SecureDouble(a.Value % b.Value);
        }

        public static bool operator ==(SecureDouble a, SecureFloat b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(SecureDouble a, SecureFloat b)
        {
            return a.Value != b.Value;
        }

        public static bool operator >(SecureDouble a, SecureFloat b)
        {
            return a.Value > b.Value;
        }

        public static bool operator <(SecureDouble a, SecureFloat b)
        {
            return a.Value < b.Value;
        }

        public static bool operator >=(SecureDouble a, SecureFloat b)
        {
            return a.Value >= b.Value;
        }

        public static bool operator <=(SecureDouble a, SecureFloat b)
        {
            return a.Value <= b.Value;
        }
        #endregion

        #region [Operator With SecureInt]
        public static SecureDouble operator +(SecureDouble a, SecureInt b)
        {
            return new SecureDouble(a.Value + b.Value);
        }

        public static SecureDouble operator -(SecureDouble a, SecureInt b)
        {
            return new SecureDouble(a.Value - b.Value);
        }

        public static SecureDouble operator *(SecureDouble a, SecureInt b)
        {
            return new SecureDouble(a.Value * b.Value);
        }

        public static SecureDouble operator /(SecureDouble a, SecureInt b)
        {
            return new SecureDouble(a.Value / b.Value);
        }

        public static SecureDouble operator %(SecureDouble a, SecureInt b)
        {
            return new SecureDouble(a.Value % b.Value);
        }

        public static bool operator ==(SecureDouble a, SecureInt b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(SecureDouble a, SecureInt b)
        {
            return a.Value != b.Value;
        }

        public static bool operator >(SecureDouble a, SecureInt b)
        {
            return a.Value > b.Value;
        }

        public static bool operator <(SecureDouble a, SecureInt b)
        {
            return a.Value < b.Value;
        }

        public static bool operator >=(SecureDouble a, SecureInt b)
        {
            return a.Value >= b.Value;
        }

        public static bool operator <=(SecureDouble a, SecureInt b)
        {
            return a.Value <= b.Value;
        }
        #endregion

        #region [Operator With Int]
        public static SecureDouble operator +(SecureDouble a, int b)
        {
            return new SecureDouble(a.Value + b);
        }

        public static SecureDouble operator -(SecureDouble a, int b)
        {
            return new SecureDouble(a.Value - b);
        }

        public static SecureDouble operator *(SecureDouble a, int b)
        {
            return new SecureDouble(a.Value * b);
        }

        public static SecureDouble operator /(SecureDouble a, int b)
        {
            return new SecureDouble(a.Value / b);
        }

        public static SecureDouble operator %(SecureDouble a, int b)
        {
            return new SecureDouble(a.Value % b);
        }

        public static bool operator ==(SecureDouble a, int b)
        {
            return a.Value == b;
        }

        public static bool operator !=(SecureDouble a, int b)
        {
            return a.Value != b;
        }

        public static bool operator >(SecureDouble a, int b)
        {
            return a.Value > b;
        }

        public static bool operator <(SecureDouble a, int b)
        {
            return a.Value < b;
        }

        public static bool operator >=(SecureDouble a, int b)
        {
            return a.Value >= b;
        }

        public static bool operator <=(SecureDouble a, int b)
        {
            return a.Value <= b;
        }
        #endregion

        #region [Operator With Long]
        public static SecureDouble operator +(SecureDouble a, long b)
        {
            return new SecureDouble(a.Value + b);
        }

        public static SecureDouble operator -(SecureDouble a, long b)
        {
            return new SecureDouble(a.Value - b);
        }

        public static SecureDouble operator *(SecureDouble a, long b)
        {
            return new SecureDouble(a.Value * b);
        }

        public static SecureDouble operator /(SecureDouble a, long b)
        {
            return new SecureDouble(a.Value / b);
        }

        public static SecureDouble operator %(SecureDouble a, long b)
        {
            return new SecureDouble(a.Value % b);
        }

        public static bool operator ==(SecureDouble a, long b)
        {
            return a.Value == b;
        }

        public static bool operator !=(SecureDouble a, long b)
        {
            return a.Value != b;
        }

        public static bool operator >(SecureDouble a, long b)
        {
            return a.Value > b;
        }

        public static bool operator <(SecureDouble a, long b)
        {
            return a.Value < b;
        }

        public static bool operator >=(SecureDouble a, long b)
        {
            return a.Value >= b;
        }

        public static bool operator <=(SecureDouble a, long b)
        {
            return a.Value <= b;
        }
        #endregion

        #region [Operator With Float]
        public static SecureDouble operator +(SecureDouble a, float b)
        {
            return new SecureDouble(a.Value + b);
        }

        public static SecureDouble operator -(SecureDouble a, float b)
        {
            return new SecureDouble(a.Value - b);
        }

        public static SecureDouble operator *(SecureDouble a, float b)
        {
            return new SecureDouble(a.Value * b);
        }

        public static SecureDouble operator /(SecureDouble a, float b)
        {
            return new SecureDouble(a.Value / b);
        }

        public static SecureDouble operator %(SecureDouble a, float b)
        {
            return new SecureDouble(a.Value % b);
        }

        public static bool operator ==(SecureDouble a, float b)
        {
            return a.Value == b;
        }

        public static bool operator !=(SecureDouble a, float b)
        {
            return a.Value != b;
        }

        public static bool operator >(SecureDouble a, float b)
        {
            return a.Value > b;
        }

        public static bool operator <(SecureDouble a, float b)
        {
            return a.Value < b;
        }

        public static bool operator >=(SecureDouble a, float b)
        {
            return a.Value >= b;
        }

        public static bool operator <=(SecureDouble a, float b)
        {
            return a.Value <= b;
        }
        #endregion

        #region [Operator With Double]
        public static SecureDouble operator +(SecureDouble a, double b)
        {
            return new SecureDouble(a.Value + b);
        }

        public static SecureDouble operator -(SecureDouble a, double b)
        {
            return new SecureDouble(a.Value - b);
        }

        public static SecureDouble operator *(SecureDouble a, double b)
        {
            return new SecureDouble(a.Value * b);
        }

        public static SecureDouble operator /(SecureDouble a, double b)
        {
            return new SecureDouble(a.Value / b);
        }

        public static SecureDouble operator %(SecureDouble a, double b)
        {
            return new SecureDouble(a.Value % b);
        }

        public static bool operator ==(SecureDouble a, double b)
        {
            return a.Value == b;
        }

        public static bool operator !=(SecureDouble a, double b)
        {
            return a.Value != b;
        }

        public static bool operator >(SecureDouble a, double b)
        {
            return a.Value > b;
        }

        public static bool operator <(SecureDouble a, double b)
        {
            return a.Value < b;
        }

        public static bool operator >=(SecureDouble a, double b)
        {
            return a.Value >= b;
        }

        public static bool operator <=(SecureDouble a, double b)
        {
            return a.Value <= b;
        }
        #endregion

        #region [Int]
        public static SecureDouble operator +(int a, SecureDouble b)
        {
            return new SecureDouble(a + b.Value);
        }

        public static SecureDouble operator -(int a, SecureDouble b)
        {
            return new SecureDouble(a - b.Value);
        }

        public static SecureDouble operator *(int a, SecureDouble b)
        {
            return new SecureDouble(a * b.Value);
        }

        public static SecureDouble operator /(int a, SecureDouble b)
        {
            return new SecureDouble(a / b.Value);
        }

        public static SecureDouble operator %(int a, SecureDouble b)
        {
            return new SecureDouble(a % b.Value);
        }

        public static bool operator ==(int a, SecureDouble b)
        {
            return a == b.Value;
        }

        public static bool operator !=(int a, SecureDouble b)
        {
            return a != b.Value;
        }

        public static bool operator >(int a, SecureDouble b)
        {
            return a > b.Value;
        }

        public static bool operator <(int a, SecureDouble b)
        {
            return a < b.Value;
        }

        public static bool operator >=(int a, SecureDouble b)
        {
            return a >= b.Value;
        }

        public static bool operator <=(int a, SecureDouble b)
        {
            return a <= b.Value;
        }
        #endregion

        #region [Long]
        public static SecureDouble operator +(long a, SecureDouble b)
        {
            return new SecureDouble(a + b.Value);
        }

        public static SecureDouble operator -(long a, SecureDouble b)
        {
            return new SecureDouble(a - b.Value);
        }

        public static SecureDouble operator *(long a, SecureDouble b)
        {
            return new SecureDouble(a * b.Value);
        }

        public static SecureDouble operator /(long a, SecureDouble b)
        {
            return new SecureDouble(a / b.Value);
        }

        public static SecureDouble operator %(long a, SecureDouble b)
        {
            return new SecureDouble(a % b.Value);
        }

        public static bool operator ==(long a, SecureDouble b)
        {
            return a == b.Value;
        }

        public static bool operator !=(long a, SecureDouble b)
        {
            return a != b.Value;
        }

        public static bool operator >(long a, SecureDouble b)
        {
            return a > b.Value;
        }

        public static bool operator <(long a, SecureDouble b)
        {
            return a < b.Value;
        }

        public static bool operator >=(long a, SecureDouble b)
        {
            return a >= b.Value;
        }

        public static bool operator <=(long a, SecureDouble b)
        {
            return a <= b.Value;
        }
        #endregion

        #region [Float]
        public static SecureDouble operator +(float a, SecureDouble b)
        {
            return new SecureDouble(a + b.Value);
        }

        public static SecureDouble operator -(float a, SecureDouble b)
        {
            return new SecureDouble(a - b.Value);
        }

        public static SecureDouble operator *(float a, SecureDouble b)
        {
            return new SecureDouble(a * b.Value);
        }

        public static SecureDouble operator /(float a, SecureDouble b)
        {
            return new SecureDouble(a / b.Value);
        }

        public static SecureDouble operator %(float a, SecureDouble b)
        {
            return new SecureDouble(a % b.Value);
        }

        public static bool operator ==(float a, SecureDouble b)
        {
            return a == b.Value;
        }

        public static bool operator !=(float a, SecureDouble b)
        {
            return a != b.Value;
        }

        public static bool operator >(float a, SecureDouble b)
        {
            return a > b.Value;
        }

        public static bool operator <(float a, SecureDouble b)
        {
            return a < b.Value;
        }

        public static bool operator >=(float a, SecureDouble b)
        {
            return a >= b.Value;
        }

        public static bool operator <=(float a, SecureDouble b)
        {
            return a <= b.Value;
        }
        #endregion

        #region [Double]
        public static SecureDouble operator +(double a, SecureDouble b)
        {
            return new SecureDouble(a + b.Value);
        }

        public static SecureDouble operator -(double a, SecureDouble b)
        {
            return new SecureDouble(a - b.Value);
        }

        public static SecureDouble operator *(double a, SecureDouble b)
        {
            return new SecureDouble(a * b.Value);
        }

        public static SecureDouble operator /(double a, SecureDouble b)
        {
            return new SecureDouble(a / b.Value);
        }

        public static SecureDouble operator %(double a, SecureDouble b)
        {
            return new SecureDouble(a % b.Value);
        }

        public static bool operator ==(double a, SecureDouble b)
        {
            return a == b.Value;
        }

        public static bool operator !=(double a, SecureDouble b)
        {
            return a != b.Value;
        }

        public static bool operator >(double a, SecureDouble b)
        {
            return a > b.Value;
        }

        public static bool operator <(double a, SecureDouble b)
        {
            return a < b.Value;
        }

        public static bool operator >=(double a, SecureDouble b)
        {
            return a >= b.Value;
        }

        public static bool operator <=(double a, SecureDouble b)
        {
            return a <= b.Value;
        }
        #endregion
    }
}
