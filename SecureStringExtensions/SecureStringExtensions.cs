using System;
using System.Runtime.InteropServices;
using System.Security;

namespace SecureStringExtensions
{
    public static class SecureStringExtensions
    {
        public static SecureString ToSecureString(this string str)
        {
            return ToSecureString(str.ToCharArray());
        }

        public static SecureString ToSecureString(this char[] str)
        {
            var secureString = new SecureString();
            Array.ForEach(str, secureString.AppendChar);
            return secureString;
        }

        /// <summary>
        /// Creates a managed character array from the secure string using methods in System.Runetime.InteropServices
        /// copying data into a BSTR (unmanaged binary string) and then into a managed character array which is returned from this method.
        /// Data in the unmanaged memory temporarily used are freed up before the method returns.
        /// </summary>
        /// <param name="secureString"></param>
        /// <returns></returns>
        public static char[] FromSecureStringToCharArray(this SecureString secureString)
        {
            char[] bytes;
            var ptr = IntPtr.Zero;
            try
            {
                //alloc unmanaged binary string  (BSTR) and copy contents of SecureString into this BSTR
                ptr = Marshal.SecureStringToBSTR(secureString);
                bytes = new char[secureString.Length];
                //copy to managed memory char array from unmanaged memory 
                Marshal.Copy(ptr, bytes, 0, secureString.Length);
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                {
                    //free unmanaged memory
                    Marshal.ZeroFreeBSTR(ptr);
                }
            }
            return bytes;
        }

        /// <summary>
        /// Returns an unsafe string in managed memory from SecureString. 
        /// The use of this method is not recommended - use instead the <see cref="FromSecureStringToCharArray(SecureString)"/> method
        /// as that method has not got multiple copies of data in managed memory like this method.
        /// Data in unmanaged memory temporarily used are freed up before the method returns.
        /// </summary>
        /// <param name="secureString"></param>
        /// <returns></returns>
        public static string FromSecureStringToUnsafeString(this SecureString secureString)
        {
            if (secureString == null)
            {
                throw new ArgumentNullException(nameof(secureString));
            }
            var unmanagedString = IntPtr.Zero;
            try
            {
                //copy secure string into unmanaged memory
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                //alloc managed string and copy contents of unmanaged string data into it
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                    if (unmanagedString != IntPtr.Zero)
                {
                    Marshal.FreeBSTR(unmanagedString);
                }
            }
        }

    }
}
