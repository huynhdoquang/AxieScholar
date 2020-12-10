using System;

namespace Net.HungryBug.Core.Security
{
    public struct SecureString : IComparable, IComparable<SecureString>
    {
        public static SecureString Empty => new SecureString(string.Empty);

        public SecureString(string defaultValue = null)
        {
            if (defaultValue == null)
            {
                this.data = null;
            }
            else
            {
                data = ByteProtect.EncryptBytes(System.Text.Encoding.ASCII.GetBytes(defaultValue));
            }
        }

        private byte[] data;
        /// <summary>
        /// Gets original value.
        /// </summary>
        public string Value
        {
            get { return data != null ? System.Text.Encoding.ASCII.GetString(ByteProtect.DecryptBytes(data)) : null; }
            set
            {
                if (value == null)
                {
                    data = null;
                }
                else
                {
                    data = ByteProtect.EncryptBytes(System.Text.Encoding.ASCII.GetBytes(value));
                }
            }
        }

        #region [Assign]
        public static implicit operator SecureString(string value)
        {
            return new SecureString(value);
        }

        public static implicit operator string(SecureString value)
        {
            return value.Value;
        }
        #endregion

        #region [Comparer]
        public int CompareTo(object obj)
        {
            var source = (SecureString)obj;
            return this.Value.CompareTo(source.Value);
        }

        public int CompareTo(SecureString other)
        {
            return this.Value.CompareTo(other.Value);
        }
        #endregion

        #region [String Behaviour]
        public int Length => this.Value.Length;
        public char this[int index] => this.Value[index];

        public bool Contains(string target)
        {
            return this.Value.Contains(target);
        }

        public bool StartsWith(string target)
        {
            return this.Value.StartsWith(target);
        } 
        #endregion

        #region [Operator With Self]
        public static SecureString operator +(SecureString a, SecureString b)
        {
            return new SecureString(a.Value + b.Value);
        }

        public static bool operator ==(SecureString a, SecureString b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(SecureString a, SecureString b)
        {
            return a.Value != b.Value;
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

        #region [Operator With String]
        public static SecureString operator +(SecureString a, string b)
        {
            return new SecureString(a.Value + b);
        }

        public static bool operator ==(SecureString a, string b)
        {
            return a.Value == b;
        }

        public static bool operator !=(SecureString a, string b)
        {
            return a.Value != b;
        }

        public static SecureString operator +(string a, SecureString b)
        {
            return new SecureString(a + b.Value);
        }

        public static bool operator ==(string a, SecureString b)
        {
            return a == b.Value;
        }

        public static bool operator !=(string a, SecureString b)
        {
            return a != b.Value;
        }
        #endregion

        #region [String Adders]
        public static SecureString operator +(SecureString a, SecureInt b)
        {
            return new SecureString(a.Value + b.Value);
        }

        public static SecureString operator +(SecureString a, SecureLong b)
        {
            return new SecureString(a.Value + b.Value);
        }

        public static SecureString operator +(SecureString a, SecureDouble b)
        {
            return new SecureString(a.Value + b.Value);
        }

        public static SecureString operator +(SecureString a, SecureBool b)
        {
            return new SecureString(a.Value + b.Value);
        }

        public static SecureString operator +(SecureString a, int b)
        {
            return new SecureString(a.Value + b);
        }

        public static SecureString operator +(SecureString a, long b)
        {
            return new SecureString(a.Value + b);
        }

        public static SecureString operator +(SecureString a, float b)
        {
            return new SecureString(a.Value + b);
        }

        public static SecureString operator +(SecureString a, double b)
        {
            return new SecureString(a.Value + b);
        } 
        #endregion
    }
}
