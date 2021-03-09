using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace HFrame.OS
{
    /// <summary>
    /// 对内存操作的工具类
    /// </summary>
    public class MemoryUtils
    {
        /// <summary>
        /// 读取进程的内存块
        /// </summary>
        /// <param name="processHandle">进程的句柄</param>
        /// <param name="baseAddress">要读取的内存块的起始地址</param>
        /// <param name="buf">装要读取的数据的字节数组</param>
        /// <param name="size">要读取的长度，以字节为单位</param>
        /// <param name="sizeOfRead">实际读取的长度</param>
        /// <returns></returns>
        public static bool ReadMemoryBlock(IntPtr processHandle, IntPtr baseAddress, byte[] buf, int size, out int sizeOfRead)
        {
            return Kernel32.ReadProcessMemory(processHandle, baseAddress, buf, size, out sizeOfRead);
        }

        /// <summary>
        /// 从buffer中查找input,buffer为从进程中地读取的数据
        /// 本方法考虑到一般32位系统是4或8对齐的，为了查找的效率与兼容性，
        /// 以假设内存是4对齐来进行查找
        /// </summary>
        /// <param name="buffer">从进程中读取的数据</param>
        /// <param name="input">转化成字节数组后的期待值</param>
        /// <param name="type">期待值的类型</param>
        /// <returns></returns>
        public static IList<int> SearchResult(byte[] buffer, byte[] input, Type type)
        {
            //默认每次取4字节与期待值进行比较
            int offset = 4;
            if (type.IsPrimitive)
            {
                //如果期待值的长度小于4，则用期待值的长度，这里缺少校验
                if (input.Length < 4)
                {
                    offset = input.Length;
                }

            }
            IList<int> results = new List<int>();
            int i = 0;
            while (i < buffer.Length)
            {
                bool fit = true;
                for (int j = i; j < i + input.Length; j++)
                {
                    //当j超过长度时，为防止数组越界
                    if (j >= buffer.Length)
                    {
                        fit = false;
                        break;
                    }
                    fit = fit && (buffer[j] == input[j - i]);
                    //只要有一个字节没有匹配，没这一块内存没有匹配
                    if (!fit)
                    {
                        break;
                    }
                }
                if (fit)
                {
                    results.Add(i);
                }

                i += offset;//如果把offset改成1,则会有最好的兼容性
            }
            return results;
        }


        /// <summary>
        /// 获取把value转化成对应的类型后内存中的表示方式，返回一个字节数组
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type">要转化的类型，只支持基础类型</param>
        /// <returns></returns>
        public static byte[] GetBytes(string value, Type type)
        {
            return (byte[])typeof(BitConverter).GetMethod("GetBytes", new Type[] { type }).Invoke(null, new object[] { Convert.ChangeType(value, type) });
        }

        /// <summary>
        /// 写进程的内存
        /// </summary>
        /// <param name="processHandle">进程的句柄</param>
        /// <param name="baseAddress">起始地址</param>
        /// <param name="buf">要写入的内容</param>
        /// <param name="size">要写入的长度</param>
        /// <param name="sizeOfWrite">实际写入的长度</param>
        /// <returns></returns>
        public static bool WriteMemory(IntPtr processHandle, IntPtr baseAddress, byte[] buf, int size, out int sizeOfWrite)
        {
            return Kernel32.WriteProcessMemory(processHandle, baseAddress, buf, size, out sizeOfWrite);
        }

    }
}
