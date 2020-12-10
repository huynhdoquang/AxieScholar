using System;

namespace Net.HungryBug.Core.Security
{
    public struct SecureBool : IComparable, IComparable<SecureBool>
    {
        public SecureBool(bool defaultValue = false) { this.data = ByteProtect.EncryptBytes(System.BitConverter.GetBytes(defaultValue)); }

        private byte[] data;
        /// <summary>
        /// Gets original value.
        /// </summary>
        public bool Value
        {
            get
            {
                if (this.data == null)
                {
                    this.data = ByteProtect.EncryptBytes(System.BitConverter.GetBytes(false));
                }

                return System.BitConverter.ToBoolean(ByteProtect.DecryptBytes(this.data), 0);
            }
            set
            {
                this.data = ByteProtect.EncryptBytes(System.BitConverter.GetBytes(value));
            }
        }

        #region [Assign]
        public static implicit operator SecureBool(bool value)
        {
            return new SecureBool(value);
        }

        public static implicit operator bool(SecureBool value)
        {
            return value.Value;
        }
        #endregion

        #region [Comparer]
        public int CompareTo(object obj)
        {
            var source = (SecureBool)obj;
            return this.Value.CompareTo(source.Value);
        }

        public int CompareTo(SecureBool other)
        {
            return this.Value.CompareTo(other.Value);
        } 
        #endregion

        #region [Operator With Self]
        public static bool operator ==(SecureBool a, SecureBool b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(SecureBool a, SecureBool b)
        {
            return a.Value != b.Value;
        }
        public static bool operator !(SecureBool a)
        {
            return !a.Value;
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

        #region [Operator With Bool]
        public static bool operator ==(SecureBool a, bool b)
        {
            return a.Value == b;
        }

        public static bool operator !=(SecureBool a, bool b)
        {
            return a.Value != b;
        }
        #endregion

        #region [Bool]
        public static bool operator ==(bool a, SecureBool b)
        {
            return a == b.Value;
        }

        public static bool operator !=(bool a, SecureBool b)
        {
            return a != b.Value;
        }
        #endregion
    }
}
