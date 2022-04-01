using HFrame.OS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pal98Timer
{
    public abstract class MemoryReadBase
    {
        public abstract void Flush(IntPtr handle, int PID, int BaseAddr32, long BaseAddr64);
        public static bool ArrayContains<T>(T[] array, T ele)
        {
            foreach (T e in array)
            {
                if (e.Equals(ele))
                {
                    return true;
                }
            }
            return false;
        }
        public static T Readm<T>(IntPtr handle, int baseaddr, int[] offset)
        {
            int addr = baseaddr;
            for (var i = 0; i < offset.Length - 1; ++i)
            {
                addr = Readm<int>(handle, addr + offset[i]);
            }
            return Readm<T>(handle, addr + offset[offset.Length - 1]);
        }
        public static T Readm<T>(IntPtr handle, int addr)
        {
            T res = default(T);
            Type t = typeof(T);
            int size = 0;
            if (t.Name == "String")
            {
                size = 1024;
            }
            else
            {
                size = System.Runtime.InteropServices.Marshal.SizeOf(t);
            }
            byte[] buffer = new byte[size];
            int sizeofRead;

            if (Kernel32.ReadProcessMemory(handle, new IntPtr(addr), buffer, size, out sizeofRead))
            {
                if (t == typeof(short))
                {
                    short tmp = BitConverter.ToInt16(buffer, 0);
                    res = (T)Convert.ChangeType(tmp, t);
                }
                else if (t == typeof(ushort))
                {
                    ushort tmp = BitConverter.ToUInt16(buffer, 0);
                    res = (T)Convert.ChangeType(tmp, t);
                }
                else if (t == typeof(byte))
                {
                    byte tmp = buffer[0];
                    res = (T)Convert.ChangeType(tmp, t);
                }
                else if (t == typeof(int))
                {
                    int tmp = BitConverter.ToInt32(buffer, 0);
                    res = (T)Convert.ChangeType(tmp, t);
                }
                else if (t == typeof(long))
                {
                    long tmp = BitConverter.ToInt64(buffer, 0);
                    res = (T)Convert.ChangeType(tmp, t);
                }
                else if (t == typeof(bool))
                {
                    bool tmp = BitConverter.ToBoolean(buffer, 0);
                    res = (T)Convert.ChangeType(tmp, t);
                }
                else if (t == typeof(double))
                {
                    double tmp = BitConverter.ToDouble(buffer, 0);
                    res = (T)Convert.ChangeType(tmp, t);
                }
                else if (t == typeof(float))
                {
                    float tmp = BitConverter.ToSingle(buffer, 0);
                    res = (T)Convert.ChangeType(tmp, t);
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < sizeofRead; ++i)
                    {
                        //byte b = buffer[i];
                        char c = (char)buffer[i];
                        if (c == '\0')
                        {
                            res = (T)Convert.ChangeType(sb.ToString(), t);
                            break;
                        }
                        else
                        {
                            sb.Append(c);
                        }
                    }
                }
            }

            return res;
        }

        public static T Readm<T>(IntPtr handle, long baseaddr, int[] offset)
        {
            long addr = baseaddr;
            for (var i = 0; i < offset.Length - 1; ++i)
            {
                addr = Readm<long>(handle, addr + offset[i]);
            }
            return Readm<T>(handle, addr + offset[offset.Length - 1]);
        }
        public static T Readm<T>(IntPtr handle, long addr)
        {
            T res = default(T);
            Type t = typeof(T);
            int size = 0;
            if (t.Name == "String")
            {
                size = 1024;
            }
            else
            {
                size = System.Runtime.InteropServices.Marshal.SizeOf(t);
            }
            byte[] buffer = new byte[size];
            int sizeofRead;

            if (Kernel32.ReadProcessMemory(handle, new IntPtr(addr), buffer, size, out sizeofRead))
            {
                if (t == typeof(short))
                {
                    short tmp = BitConverter.ToInt16(buffer, 0);
                    res = (T)Convert.ChangeType(tmp, t);
                }
                else if (t == typeof(ushort))
                {
                    ushort tmp = BitConverter.ToUInt16(buffer, 0);
                    res = (T)Convert.ChangeType(tmp, t);
                }
                else if (t == typeof(byte))
                {
                    byte tmp = buffer[0];
                    res = (T)Convert.ChangeType(tmp, t);
                }
                else if (t == typeof(int))
                {
                    int tmp = BitConverter.ToInt32(buffer, 0);
                    res = (T)Convert.ChangeType(tmp, t);
                }
                else if (t == typeof(long))
                {
                    long tmp = BitConverter.ToInt64(buffer, 0);
                    res = (T)Convert.ChangeType(tmp, t);
                }
                else if (t == typeof(bool))
                {
                    bool tmp = BitConverter.ToBoolean(buffer, 0);
                    res = (T)Convert.ChangeType(tmp, t);
                }
                else if (t == typeof(double))
                {
                    double tmp = BitConverter.ToDouble(buffer, 0);
                    res = (T)Convert.ChangeType(tmp, t);
                }
                else if (t == typeof(float))
                {
                    float tmp = BitConverter.ToSingle(buffer, 0);
                    res = (T)Convert.ChangeType(tmp, t);
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < sizeofRead; ++i)
                    {
                        //byte b = buffer[i];
                        char c = (char)buffer[i];
                        if (c == '\0')
                        {
                            res = (T)Convert.ChangeType(sb.ToString(), t);
                            break;
                        }
                        else
                        {
                            sb.Append(c);
                        }
                    }
                }
            }

            return res;
        }
    }
}
