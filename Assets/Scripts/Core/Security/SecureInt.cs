using System;

namespace Net.HungryBug.Core.Security
{
    public struct SecureInt : IComparable, IComparable<SecureInt>
    {
        public static SecureInt Default => new SecureInt(0);
        
        public SecureInt(int defaultValue = 0) { this.data = ByteProtect.EncryptBytes(System.BitConverter.GetBytes(defaultValue)); }

        private byte[] data;
        /// <summary>
        /// Gets original value.
        /// </summary>
        public int Value
        {
            get
            {
                if (this.data == null)
                {
                    this.data = ByteProtect.EncryptBytes(System.BitConverter.GetBytes(0));
                }

                return System.BitConverter.ToInt32(ByteProtect.DecryptBytes(this.data), 0);
            }
            set
            {
                this.data = ByteProtect.EncryptBytes(System.BitConverter.GetBytes(value));
            }
        }

        #region [Assign]
        public static implicit operator SecureInt(int value)
        {
            return new SecureInt(value);
        }

        public static implicit operator int(SecureInt value)
        {
            return value.Value;
        }
        #endregion

        #region [Comparer]
        public int CompareTo(object obj)
        {
            var source = (SecureInt)obj;
            return this.Value.CompareTo(source.Value);
        }

        public int CompareTo(SecureInt other)
        {
            return this.Value.CompareTo(other.Value);
        } 
        #endregion

        #region [Operator With Self]
        public static SecureInt operator +(SecureInt a, SecureInt b)
        {
            return new SecureInt(a.Value + b.Value);
        }

        public static SecureInt operator -(SecureInt a, SecureInt b)
        {
            return new SecureInt(a.Value - b.Value);
        }

        public static SecureInt operator *(SecureInt a, SecureInt b)
        {
            return new SecureInt(a.Value * b.Value);
        }

        public static SecureInt operator /(SecureInt a, SecureInt b)
        {
            return new SecureInt(a.Value / b.Value);
        }

        public static SecureInt operator ++(SecureInt a)
        {
            return new SecureInt(a.Value++);
        }

        public static SecureInt operator --(SecureInt a)
        {
            return new SecureInt(a.Value--);
        }

        public static SecureInt operator %(SecureInt a, SecureInt b)
        {
            return new SecureInt(a.Value % b.Value);
        }

        public static bool operator ==(SecureInt a, SecureInt b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(SecureInt a, SecureInt b)
        {
            return a.Value != b.Value;
        }

        public static bool operator >(SecureInt a, SecureInt b)
        {
            return a.Value > b.Value;
        }

        public static bool operator <(SecureInt a, SecureInt b)
        {
            return a.Value < b.Value;
        }

        public static bool operator >=(SecureInt a, SecureInt b)
        {
            return a.Value >= b.Value;
        }

        public static bool operator <=(SecureInt a, SecureInt b)
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
        public static SecureLong operator +(SecureInt a, SecureLong b)
        {
            return new SecureLong(a.Value + b.Value);
        }

        public static SecureLong operator -(SecureInt a, SecureLong b)
        {
            return new SecureLong(a.Value - b.Value);
        }

        public static SecureLong operator *(SecureInt a, SecureLong b)
        {
            return new SecureLong(a.Value * b.Value);
        }

        public static SecureLong operator /(SecureInt a, SecureLong b)
        {
            return new SecureLong(a.Value / b.Value);
        }

        public static SecureLong operator %(SecureInt a, SecureLong b)
        {
            return new SecureLong(a.Value % b.Value);
        }

        public static bool operator ==(SecureInt a, SecureLong b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(SecureInt a, SecureLong b)
        {
            return a.Value != b.Value;
        }

        public static bool operator >(SecureInt a, SecureLong b)
        {
            return a.Value > b.Value;
        }

        public static bool operator <(SecureInt a, SecureLong b)
        {
            return a.Value < b.Value;
        }

        public static bool operator >=(SecureInt a, SecureLong b)
        {
            return a.Value >= b.Value;
        }

        public static bool operator <=(SecureInt a, SecureLong b)
        {
            return a.Value <= b.Value;
        }
        #endregion

        #region [Operator With SecureFloat]
        public static SecureFloat operator +(SecureInt a, SecureFloat b)
        {
            return new SecureFloat(a.Value + b.Value);
        }

        public static SecureFloat operator -(SecureInt a, SecureFloat b)
        {
            return new SecureFloat(a.Value - b.Value);
        }

        public static SecureFloat operator *(SecureInt a, SecureFloat b)
        {
            return new SecureFloat(a.Value * b.Value);
        }

        public static SecureFloat operator /(SecureInt a, SecureFloat b)
        {
            return new SecureFloat(a.Value / b.Value);
        }

        public static SecureFloat operator %(SecureInt a, SecureFloat b)
        {
            return new SecureFloat(a.Value % b.Value);
        }

        public static bool operator ==(SecureInt a, SecureFloat b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(SecureInt a, SecureFloat b)
        {
            return a.Value != b.Value;
        }

        public static bool operator >(SecureInt a, SecureFloat b)
        {
            return a.Value > b.Value;
        }

        public static bool operator <(SecureInt a, SecureFloat b)
        {
            return a.Value < b.Value;
        }

        public static bool operator >=(SecureInt a, SecureFloat b)
        {
            return a.Value >= b.Value;
        }

        public static bool operator <=(SecureInt a, SecureFloat b)
        {
            return a.Value <= b.Value;
        }
        #endregion

        #region [Operator With SecureDouble]
        public static SecureDouble operator +(SecureInt a, SecureDouble b)
        {
            return new SecureDouble(a.Value + b.Value);
        }

        public static SecureDouble operator -(SecureInt a, SecureDouble b)
        {
            return new SecureDouble(a.Value - b.Value);
        }

        public static SecureDouble operator *(SecureInt a, SecureDouble b)
        {
            return new SecureDouble(a.Value * b.Value);
        }

        public static SecureDouble operator /(SecureInt a, SecureDouble b)
        {
            return new SecureDouble(a.Value / b.Value);
        }

        public static SecureDouble operator %(SecureInt a, SecureDouble b)
        {
            return new SecureDouble(a.Value % b.Value);
        }

        public static bool operator ==(SecureInt a, SecureDouble b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(SecureInt a, SecureDouble b)
        {
            return a.Value != b.Value;
        }

        public static bool operator >(SecureInt a, SecureDouble b)
        {
            return a.Value > b.Value;
        }

        public static bool operator <(SecureInt a, SecureDouble b)
        {
            return a.Value < b.Value;
        }

        public static bool operator >=(SecureInt a, SecureDouble b)
        {
            return a.Value >= b.Value;
        }

        public static bool operator <=(SecureInt a, SecureDouble b)
        {
            return a.Value <= b.Value;
        }
        #endregion

        #region [Operator With Int]
        public static SecureInt operator +(SecureInt a, int b)
        {
            return new SecureInt(a.Value + b);
        }

        public static SecureInt operator -(SecureInt a, int b)
        {
            return new SecureInt(a.Value - b);
        }

        public static SecureInt operator *(SecureInt a, int b)
        {
            return new SecureInt(a.Value * b);
        }

        public static SecureInt operator /(SecureInt a, int b)
        {
            return new SecureInt(a.Value / b);
        }

        public static SecureInt operator %(SecureInt a, int b)
        {
            return new SecureInt(a.Value % b);
        }

        public static bool operator ==(SecureInt a, int b)
        {
            return a.Value == b;
        }

        public static bool operator !=(SecureInt a, int b)
        {
            return a.Value != b;
        }

        public static bool operator >(SecureInt a, int b)
        {
            return a.Value > b;
        }

        public static bool operator <(SecureInt a, int b)
        {
            return a.Value < b;
        }

        public static bool operator >=(SecureInt a, int b)
        {
            return a.Value >= b;
        }

        public static bool operator <=(SecureInt a, int b)
        {
            return a.Value <= b;
        }
        #endregion

        #region [Operator With Long]
        public static SecureLong operator +(SecureInt a, long b)
        {
            return new SecureLong(a.Value + b);
        }

        public static SecureLong operator -(SecureInt a, long b)
        {
            return new SecureLong(a.Value - b);
        }

        public static SecureLong operator *(SecureInt a, long b)
        {
            return new SecureLong(a.Value * b);
        }

        public static SecureLong operator /(SecureInt a, long b)
        {
            return new SecureLong(a.Value / b);
        }

        public static SecureLong operator %(SecureInt a, long b)
        {
            return new SecureLong(a.Value % b);
        }

        public static bool operator ==(SecureInt a, long b)
        {
            return a.Value == b;
        }

        public static bool operator !=(SecureInt a, long b)
        {
            return a.Value != b;
        }

        public static bool operator >(SecureInt a, long b)
        {
            return a.Value > b;
        }

        public static bool operator <(SecureInt a, long b)
        {
            return a.Value < b;
        }

        public static bool operator >=(SecureInt a, long b)
        {
            return a.Value >= b;
        }

        public static bool operator <=(SecureInt a, long b)
        {
            return a.Value <= b;
        }
        #endregion

        #region [Operator With Float]
        public static SecureFloat operator +(SecureInt a, float b)
        {
            return new SecureFloat(a.Value + b);
        }

        public static SecureFloat operator -(SecureInt a, float b)
        {
            return new SecureFloat(a.Value - b);
        }

        public static SecureFloat operator *(SecureInt a, float b)
        {
            return new SecureFloat(a.Value * b);
        }

        public static SecureFloat operator /(SecureInt a, float b)
        {
            return new SecureFloat(a.Value / b);
        }

        public static SecureFloat operator %(SecureInt a, float b)
        {
            return new SecureFloat(a.Value % b);
        }

        public static bool operator ==(SecureInt a, float b)
        {
            return a.Value == b;
        }

        public static bool operator !=(SecureInt a, float b)
        {
            return a.Value != b;
        }

        public static bool operator >(SecureInt a, float b)
        {
            return a.Value > b;
        }

        public static bool operator <(SecureInt a, float b)
        {
            return a.Value < b;
        }

        public static bool operator >=(SecureInt a, float b)
        {
            return a.Value >= b;
        }

        public static bool operator <=(SecureInt a, float b)
        {
            return a.Value <= b;
        }
        #endregion

        #region [Operator With Double]
        public static SecureDouble operator +(SecureInt a, double b)
        {
            return new SecureDouble(a.Value + b);
        }

        public static SecureDouble operator -(SecureInt a, double b)
        {
            return new SecureDouble(a.Value - b);
        }

        public static SecureDouble operator *(SecureInt a, double b)
        {
            return new SecureDouble(a.Value * b);
        }

        public static SecureDouble operator /(SecureInt a, double b)
        {
            return new SecureDouble(a.Value / b);
        }

        public static SecureDouble operator %(SecureInt a, double b)
        {
            return new SecureDouble(a.Value % b);
        }

        public static bool operator ==(SecureInt a, double b)
        {
            return a.Value == b;
        }

        public static bool operator !=(SecureInt a, double b)
        {
            return a.Value != b;
        }

        public static bool operator >(SecureInt a, double b)
        {
            return a.Value > b;
        }

        public static bool operator <(SecureInt a, double b)
        {
            return a.Value < b;
        }

        public static bool operator >=(SecureInt a, double b)
        {
            return a.Value >= b;
        }

        public static bool operator <=(SecureInt a, double b)
        {
            return a.Value <= b;
        }
        #endregion

        #region [Int]
        public static SecureInt operator +(int a, SecureInt b)
        {
            return new SecureInt(a + b.Value);
        }

        public static SecureInt operator -(int a, SecureInt b)
        {
            return new SecureInt(a - b.Value);
        }

        public static SecureInt operator *(int a, SecureInt b)
        {
            return new SecureInt(a * b.Value);
        }

        public static SecureInt operator /(int a, SecureInt b)
        {
            return new SecureInt(a / b.Value);
        }

        public static SecureInt operator %(int a, SecureInt b)
        {
            return new SecureInt(a % b.Value);
        }

        public static bool operator ==(int a, SecureInt b)
        {
            return a == b.Value;
        }

        public static bool operator !=(int a, SecureInt b)
        {
            return a != b.Value;
        }

        public static bool operator >(int a, SecureInt b)
        {
            return a > b.Value;
        }

        public static bool operator <(int a, SecureInt b)
        {
            return a < b.Value;
        }

        public static bool operator >=(int a, SecureInt b)
        {
            return a >= b.Value;
        }

        public static bool operator <=(int a, SecureInt b)
        {
            return a <= b.Value;
        }
        #endregion

        #region [Long]
        public static SecureLong operator +(long a, SecureInt b)
        {
            return new SecureLong(a + b.Value);
        }

        public static SecureLong operator -(long a, SecureInt b)
        {
            return new SecureLong(a - b.Value);
        }

        public static SecureLong operator *(long a, SecureInt b)
        {
            return new SecureLong(a * b.Value);
        }

        public static SecureLong operator /(long a, SecureInt b)
        {
            return new SecureLong(a / b.Value);
        }

        public static SecureLong operator %(long a, SecureInt b)
        {
            return new SecureLong(a % b.Value);
        }

        public static bool operator ==(long a, SecureInt b)
        {
            return a == b.Value;
        }

        public static bool operator !=(long a, SecureInt b)
        {
            return a != b.Value;
        }

        public static bool operator >(long a, SecureInt b)
        {
            return a > b.Value;
        }

        public static bool operator <(long a, SecureInt b)
        {
            return a < b.Value;
        }

        public static bool operator >=(long a, SecureInt b)
        {
            return a >= b.Value;
        }

        public static bool operator <=(long a, SecureInt b)
        {
            return a <= b.Value;
        }
        #endregion

        #region [Float]
        public static SecureFloat operator +(float a, SecureInt b)
        {
            return new SecureFloat(a + b.Value);
        }

        public static SecureFloat operator -(float a, SecureInt b)
        {
            return new SecureFloat(a - b.Value);
        }

        public static SecureFloat operator *(float a, SecureInt b)
        {
            return new SecureFloat(a * b.Value);
        }

        public static SecureFloat operator /(float a, SecureInt b)
        {
            return new SecureFloat(a / b.Value);
        }

        public static SecureFloat operator %(float a, SecureInt b)
        {
            return new SecureFloat(a % b.Value);
        }

        public static bool operator ==(float a, SecureInt b)
        {
            return a == b.Value;
        }

        public static bool operator !=(float a, SecureInt b)
        {
            return a != b.Value;
        }

        public static bool operator >(float a, SecureInt b)
        {
            return a > b.Value;
        }

        public static bool operator <(float a, SecureInt b)
        {
            return a < b.Value;
        }

        public static bool operator >=(float a, SecureInt b)
        {
            return a >= b.Value;
        }

        public static bool operator <=(float a, SecureInt b)
        {
            return a <= b.Value;
        }
        #endregion

        #region [Double]
        public static SecureDouble operator +(double a, SecureInt b)
        {
            return new SecureDouble(a + b.Value);
        }

        public static SecureDouble operator -(double a, SecureInt b)
        {
            return new SecureDouble(a - b.Value);
        }

        public static SecureDouble operator *(double a, SecureInt b)
        {
            return new SecureDouble(a * b.Value);
        }

        public static SecureDouble operator /(double a, SecureInt b)
        {
            return new SecureDouble(a / b.Value);
        }

        public static SecureDouble operator %(double a, SecureInt b)
        {
            return new SecureDouble(a % b.Value);
        }

        public static bool operator ==(double a, SecureInt b)
        {
            return a == b.Value;
        }

        public static bool operator !=(double a, SecureInt b)
        {
            return a != b.Value;
        }

        public static bool operator >(double a, SecureInt b)
        {
            return a > b.Value;
        }

        public static bool operator <(double a, SecureInt b)
        {
            return a < b.Value;
        }

        public static bool operator >=(double a, SecureInt b)
        {
            return a >= b.Value;
        }

        public static bool operator <=(double a, SecureInt b)
        {
            return a <= b.Value;
        }
        #endregion
    }
}
