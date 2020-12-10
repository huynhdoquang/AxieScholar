using System;

namespace Net.HungryBug.Core.Security
{
    public struct SecureFloat : IComparable, IComparable<SecureFloat>
    {
        public static SecureFloat Default => new SecureFloat(0);

        public SecureFloat(float defaultValue = 0) { this.data = ByteProtect.EncryptBytes(System.BitConverter.GetBytes((double)defaultValue)); }

        private byte[] data;
        /// <summary>
        /// Gets original value.
        /// </summary>
        public float Value
        {
            get
            {
                if (this.data == null)
                {
                    this.data = ByteProtect.EncryptBytes(System.BitConverter.GetBytes((double)0));
                }

                return (float)System.BitConverter.ToDouble(ByteProtect.DecryptBytes(this.data), 0);
            }
            set
            {
                this.data = ByteProtect.EncryptBytes(System.BitConverter.GetBytes((double)value));
            }
        }

        #region [Assign]
        public static implicit operator SecureFloat(float value)
        {
            return new SecureFloat(value);
        }

        public static implicit operator float(SecureFloat value)
        {
            return value.Value;
        }

        public static implicit operator SecureFloat(long value)
        {
            return new SecureFloat(value);
        }

        public static implicit operator SecureFloat(int value)
        {
            return new SecureFloat(value);
        }
        #endregion

        #region [Comparer]
        public int CompareTo(object obj)
        {
            var source = (SecureFloat)obj;
            return this.Value.CompareTo(source.Value);
        }

        public int CompareTo(SecureFloat other)
        {
            return this.Value.CompareTo(other.Value);
        }
        #endregion

        #region [Operator With Self]
        public static SecureFloat operator +(SecureFloat a, SecureFloat b)
        {
            return new SecureFloat(a.Value + b.Value);
        }

        public static SecureFloat operator -(SecureFloat a, SecureFloat b)
        {
            return new SecureFloat(a.Value - b.Value);
        }

        public static SecureFloat operator *(SecureFloat a, SecureFloat b)
        {
            return new SecureFloat(a.Value * b.Value);
        }

        public static SecureFloat operator /(SecureFloat a, SecureFloat b)
        {
            return new SecureFloat(a.Value / b.Value);
        }

        public static SecureFloat operator ++(SecureFloat a)
        {
            return new SecureFloat(a.Value++);
        }

        public static SecureFloat operator --(SecureFloat a)
        {
            return new SecureFloat(a.Value--);
        }

        public static SecureFloat operator %(SecureFloat a, SecureFloat b)
        {
            return new SecureFloat(a.Value % b.Value);
        }

        public static bool operator ==(SecureFloat a, SecureFloat b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(SecureFloat a, SecureFloat b)
        {
            return a.Value != b.Value;
        }

        public static bool operator >(SecureFloat a, SecureFloat b)
        {
            return a.Value > b.Value;
        }

        public static bool operator <(SecureFloat a, SecureFloat b)
        {
            return a.Value < b.Value;
        }

        public static bool operator >=(SecureFloat a, SecureFloat b)
        {
            return a.Value >= b.Value;
        }

        public static bool operator <=(SecureFloat a, SecureFloat b)
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
        public static SecureFloat operator +(SecureFloat a, SecureLong b)
        {
            return new SecureFloat(a.Value + b.Value);
        }

        public static SecureFloat operator -(SecureFloat a, SecureLong b)
        {
            return new SecureFloat(a.Value - b.Value);
        }

        public static SecureFloat operator *(SecureFloat a, SecureLong b)
        {
            return new SecureFloat(a.Value * b.Value);
        }

        public static SecureFloat operator /(SecureFloat a, SecureLong b)
        {
            return new SecureFloat(a.Value / b.Value);
        }

        public static SecureFloat operator %(SecureFloat a, SecureLong b)
        {
            return new SecureFloat(a.Value % b.Value);
        }

        public static bool operator ==(SecureFloat a, SecureLong b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(SecureFloat a, SecureLong b)
        {
            return a.Value != b.Value;
        }

        public static bool operator >(SecureFloat a, SecureLong b)
        {
            return a.Value > b.Value;
        }

        public static bool operator <(SecureFloat a, SecureLong b)
        {
            return a.Value < b.Value;
        }

        public static bool operator >=(SecureFloat a, SecureLong b)
        {
            return a.Value >= b.Value;
        }

        public static bool operator <=(SecureFloat a, SecureLong b)
        {
            return a.Value <= b.Value;
        }
        #endregion

        #region [Operator With SecureInt]
        public static SecureFloat operator +(SecureFloat a, SecureInt b)
        {
            return new SecureFloat(a.Value + b.Value);
        }

        public static SecureFloat operator -(SecureFloat a, SecureInt b)
        {
            return new SecureFloat(a.Value - b.Value);
        }

        public static SecureFloat operator *(SecureFloat a, SecureInt b)
        {
            return new SecureFloat(a.Value * b.Value);
        }

        public static SecureFloat operator /(SecureFloat a, SecureInt b)
        {
            return new SecureFloat(a.Value / b.Value);
        }

        public static SecureFloat operator %(SecureFloat a, SecureInt b)
        {
            return new SecureFloat(a.Value % b.Value);
        }

        public static bool operator ==(SecureFloat a, SecureInt b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(SecureFloat a, SecureInt b)
        {
            return a.Value != b.Value;
        }

        public static bool operator >(SecureFloat a, SecureInt b)
        {
            return a.Value > b.Value;
        }

        public static bool operator <(SecureFloat a, SecureInt b)
        {
            return a.Value < b.Value;
        }

        public static bool operator >=(SecureFloat a, SecureInt b)
        {
            return a.Value >= b.Value;
        }

        public static bool operator <=(SecureFloat a, SecureInt b)
        {
            return a.Value <= b.Value;
        }
        #endregion

        #region [Operator With SecureDouble]
        public static SecureDouble operator +(SecureFloat a, SecureDouble b)
        {
            return new SecureDouble(a.Value + b.Value);
        }

        public static SecureDouble operator -(SecureFloat a, SecureDouble b)
        {
            return new SecureDouble(a.Value - b.Value);
        }

        public static SecureDouble operator *(SecureFloat a, SecureDouble b)
        {
            return new SecureDouble(a.Value * b.Value);
        }

        public static SecureDouble operator /(SecureFloat a, SecureDouble b)
        {
            return new SecureDouble(a.Value / b.Value);
        }

        public static SecureDouble operator %(SecureFloat a, SecureDouble b)
        {
            return new SecureDouble(a.Value % b.Value);
        }

        public static bool operator ==(SecureFloat a, SecureDouble b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(SecureFloat a, SecureDouble b)
        {
            return a.Value != b.Value;
        }

        public static bool operator >(SecureFloat a, SecureDouble b)
        {
            return a.Value > b.Value;
        }

        public static bool operator <(SecureFloat a, SecureDouble b)
        {
            return a.Value < b.Value;
        }

        public static bool operator >=(SecureFloat a, SecureDouble b)
        {
            return a.Value >= b.Value;
        }

        public static bool operator <=(SecureFloat a, SecureDouble b)
        {
            return a.Value <= b.Value;
        }
        #endregion

        #region [Operator With Int]
        public static SecureFloat operator +(SecureFloat a, int b)
        {
            return new SecureFloat(a.Value + b);
        }

        public static SecureFloat operator -(SecureFloat a, int b)
        {
            return new SecureFloat(a.Value - b);
        }

        public static SecureFloat operator *(SecureFloat a, int b)
        {
            return new SecureFloat(a.Value * b);
        }

        public static SecureFloat operator /(SecureFloat a, int b)
        {
            return new SecureFloat(a.Value / b);
        }

        public static SecureFloat operator %(SecureFloat a, int b)
        {
            return new SecureFloat(a.Value % b);
        }

        public static bool operator ==(SecureFloat a, int b)
        {
            return a.Value == b;
        }

        public static bool operator !=(SecureFloat a, int b)
        {
            return a.Value != b;
        }

        public static bool operator >(SecureFloat a, int b)
        {
            return a.Value > b;
        }

        public static bool operator <(SecureFloat a, int b)
        {
            return a.Value < b;
        }

        public static bool operator >=(SecureFloat a, int b)
        {
            return a.Value >= b;
        }

        public static bool operator <=(SecureFloat a, int b)
        {
            return a.Value <= b;
        }
        #endregion

        #region [Operator With Long]
        public static SecureFloat operator +(SecureFloat a, long b)
        {
            return new SecureFloat(a.Value + b);
        }

        public static SecureFloat operator -(SecureFloat a, long b)
        {
            return new SecureFloat(a.Value - b);
        }

        public static SecureFloat operator *(SecureFloat a, long b)
        {
            return new SecureFloat(a.Value * b);
        }

        public static SecureFloat operator /(SecureFloat a, long b)
        {
            return new SecureFloat(a.Value / b);
        }

        public static SecureFloat operator %(SecureFloat a, long b)
        {
            return new SecureFloat(a.Value % b);
        }

        public static bool operator ==(SecureFloat a, long b)
        {
            return a.Value == b;
        }

        public static bool operator !=(SecureFloat a, long b)
        {
            return a.Value != b;
        }

        public static bool operator >(SecureFloat a, long b)
        {
            return a.Value > b;
        }

        public static bool operator <(SecureFloat a, long b)
        {
            return a.Value < b;
        }

        public static bool operator >=(SecureFloat a, long b)
        {
            return a.Value >= b;
        }

        public static bool operator <=(SecureFloat a, long b)
        {
            return a.Value <= b;
        }
        #endregion

        #region [Operator With Float]
        public static SecureFloat operator +(SecureFloat a, float b)
        {
            return new SecureFloat(a.Value + b);
        }

        public static SecureFloat operator -(SecureFloat a, float b)
        {
            return new SecureFloat(a.Value - b);
        }

        public static SecureFloat operator *(SecureFloat a, float b)
        {
            return new SecureFloat(a.Value * b);
        }

        public static SecureFloat operator /(SecureFloat a, float b)
        {
            return new SecureFloat(a.Value / b);
        }

        public static SecureFloat operator %(SecureFloat a, float b)
        {
            return new SecureFloat(a.Value % b);
        }

        public static bool operator ==(SecureFloat a, float b)
        {
            return a.Value == b;
        }

        public static bool operator !=(SecureFloat a, float b)
        {
            return a.Value != b;
        }

        public static bool operator >(SecureFloat a, float b)
        {
            return a.Value > b;
        }

        public static bool operator <(SecureFloat a, float b)
        {
            return a.Value < b;
        }

        public static bool operator >=(SecureFloat a, float b)
        {
            return a.Value >= b;
        }

        public static bool operator <=(SecureFloat a, float b)
        {
            return a.Value <= b;
        }
        #endregion

        #region [Operator With Double]
        public static SecureDouble operator +(SecureFloat a, double b)
        {
            return new SecureDouble(a.Value + b);
        }

        public static SecureDouble operator -(SecureFloat a, double b)
        {
            return new SecureDouble(a.Value - b);
        }

        public static SecureDouble operator *(SecureFloat a, double b)
        {
            return new SecureDouble(a.Value * b);
        }

        public static SecureDouble operator /(SecureFloat a, double b)
        {
            return new SecureDouble(a.Value / b);
        }

        public static SecureDouble operator %(SecureFloat a, double b)
        {
            return new SecureDouble(a.Value % b);
        }

        public static bool operator ==(SecureFloat a, double b)
        {
            return a.Value == b;
        }

        public static bool operator !=(SecureFloat a, double b)
        {
            return a.Value != b;
        }

        public static bool operator >(SecureFloat a, double b)
        {
            return a.Value > b;
        }

        public static bool operator <(SecureFloat a, double b)
        {
            return a.Value < b;
        }

        public static bool operator >=(SecureFloat a, double b)
        {
            return a.Value >= b;
        }

        public static bool operator <=(SecureFloat a, double b)
        {
            return a.Value <= b;
        }
        #endregion

        #region [Int]
        public static SecureFloat operator +(int a, SecureFloat b)
        {
            return new SecureFloat(a + b.Value);
        }

        public static SecureFloat operator -(int a, SecureFloat b)
        {
            return new SecureFloat(a - b.Value);
        }

        public static SecureFloat operator *(int a, SecureFloat b)
        {
            return new SecureFloat(a * b.Value);
        }

        public static SecureFloat operator /(int a, SecureFloat b)
        {
            return new SecureFloat(a / b.Value);
        }

        public static SecureFloat operator %(int a, SecureFloat b)
        {
            return new SecureFloat(a % b.Value);
        }

        public static bool operator ==(int a, SecureFloat b)
        {
            return a == b.Value;
        }

        public static bool operator !=(int a, SecureFloat b)
        {
            return a != b.Value;
        }

        public static bool operator >(int a, SecureFloat b)
        {
            return a > b.Value;
        }

        public static bool operator <(int a, SecureFloat b)
        {
            return a < b.Value;
        }

        public static bool operator >=(int a, SecureFloat b)
        {
            return a >= b.Value;
        }

        public static bool operator <=(int a, SecureFloat b)
        {
            return a <= b.Value;
        }
        #endregion

        #region [Long]
        public static SecureFloat operator +(long a, SecureFloat b)
        {
            return new SecureFloat(a + b.Value);
        }

        public static SecureFloat operator -(long a, SecureFloat b)
        {
            return new SecureFloat(a - b.Value);
        }

        public static SecureFloat operator *(long a, SecureFloat b)
        {
            return new SecureFloat(a * b.Value);
        }

        public static SecureFloat operator /(long a, SecureFloat b)
        {
            return new SecureFloat(a / b.Value);
        }

        public static SecureFloat operator %(long a, SecureFloat b)
        {
            return new SecureFloat(a % b.Value);
        }

        public static bool operator ==(long a, SecureFloat b)
        {
            return a == b.Value;
        }

        public static bool operator !=(long a, SecureFloat b)
        {
            return a != b.Value;
        }

        public static bool operator >(long a, SecureFloat b)
        {
            return a > b.Value;
        }

        public static bool operator <(long a, SecureFloat b)
        {
            return a < b.Value;
        }

        public static bool operator >=(long a, SecureFloat b)
        {
            return a >= b.Value;
        }

        public static bool operator <=(long a, SecureFloat b)
        {
            return a <= b.Value;
        }
        #endregion

        #region [Float]
        public static SecureFloat operator +(float a, SecureFloat b)
        {
            return new SecureFloat(a + b.Value);
        }

        public static SecureFloat operator -(float a, SecureFloat b)
        {
            return new SecureFloat(a - b.Value);
        }

        public static SecureFloat operator *(float a, SecureFloat b)
        {
            return new SecureFloat(a * b.Value);
        }

        public static SecureFloat operator /(float a, SecureFloat b)
        {
            return new SecureFloat(a / b.Value);
        }

        public static SecureFloat operator %(float a, SecureFloat b)
        {
            return new SecureFloat(a % b.Value);
        }

        public static bool operator ==(float a, SecureFloat b)
        {
            return a == b.Value;
        }

        public static bool operator !=(float a, SecureFloat b)
        {
            return a != b.Value;
        }

        public static bool operator >(float a, SecureFloat b)
        {
            return a > b.Value;
        }

        public static bool operator <(float a, SecureFloat b)
        {
            return a < b.Value;
        }

        public static bool operator >=(float a, SecureFloat b)
        {
            return a >= b.Value;
        }

        public static bool operator <=(float a, SecureFloat b)
        {
            return a <= b.Value;
        }
        #endregion

        #region [Double]
        public static SecureDouble operator +(double a, SecureFloat b)
        {
            return new SecureDouble(a + b.Value);
        }

        public static SecureDouble operator -(double a, SecureFloat b)
        {
            return new SecureDouble(a - b.Value);
        }

        public static SecureDouble operator *(double a, SecureFloat b)
        {
            return new SecureDouble(a * b.Value);
        }

        public static SecureDouble operator /(double a, SecureFloat b)
        {
            return new SecureDouble(a / b.Value);
        }

        public static SecureDouble operator %(double a, SecureFloat b)
        {
            return new SecureDouble(a % b.Value);
        }

        public static bool operator ==(double a, SecureFloat b)
        {
            return a == b.Value;
        }

        public static bool operator !=(double a, SecureFloat b)
        {
            return a != b.Value;
        }

        public static bool operator >(double a, SecureFloat b)
        {
            return a > b.Value;
        }

        public static bool operator <(double a, SecureFloat b)
        {
            return a < b.Value;
        }

        public static bool operator >=(double a, SecureFloat b)
        {
            return a >= b.Value;
        }

        public static bool operator <=(double a, SecureFloat b)
        {
            return a <= b.Value;
        }
        #endregion
    }
}
