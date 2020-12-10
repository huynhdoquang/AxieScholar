using System;

namespace Net.HungryBug.Core.Security
{
    public struct SecureEnum<T> where T : System.Enum
    {
        public static SecureEnum<T> Default = new SecureEnum<T>(default(T));
        
        public SecureEnum(T defaultValue) { this.data = ByteProtect.EncryptBytes(System.BitConverter.GetBytes(EnumToInt(defaultValue))); }
        
        private byte[] data;
        /// <summary>
        /// Gets original value.
        /// </summary>
        public T Value
        {
            get
            {
                if (this.data == null)
                {
                    this.data = ByteProtect.EncryptBytes(System.BitConverter.GetBytes(EnumToInt(default(T))));
                }

                return IntToEnum(System.BitConverter.ToInt32(ByteProtect.DecryptBytes(this.data), 0));
            }
            set
            {
                this.data = ByteProtect.EncryptBytes(System.BitConverter.GetBytes(EnumToInt(value)));
            }
        }

        private static int EnumToInt(T value) => (int)(object)value;
        private static T IntToEnum(int value) => (T)(object)value;

        #region [Assign]
        public static implicit operator SecureEnum<T>(T value)
        {
            return new SecureEnum<T>(value);
        }

        public static implicit operator T(SecureEnum<T> value)
        {
            return value.Value;
        }
        #endregion

        #region [Operators]
        public static bool operator ==(SecureEnum<T> a, SecureEnum<T> b)
        {
            return EnumToInt(a.Value) == EnumToInt(b.Value);
        }

        public static bool operator !=(SecureEnum<T> a, SecureEnum<T> b)
        {
            return EnumToInt(a.Value) != EnumToInt(b.Value);
        }

        public static bool operator ==(SecureEnum<T> a, T b)
        {
            return EnumToInt(a.Value) == EnumToInt(b);
        }

        public static bool operator !=(SecureEnum<T> a, T b)
        {
            return EnumToInt(a.Value) != EnumToInt(b);
        }

        public static bool operator ==(T a, SecureEnum<T> b)
        {
            return EnumToInt(a) == EnumToInt(b.Value);
        }

        public static bool operator !=(T a, SecureEnum<T> b)
        {
            return EnumToInt(a) != EnumToInt(b.Value);
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
    }
}

