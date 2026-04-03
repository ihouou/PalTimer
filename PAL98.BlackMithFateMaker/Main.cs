using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using TimerPluginBase;

namespace PAL98.BlackMithFateMaker
{
    public class Main : TimerPlugin
    {
        private const string FateRandom = "随机模式";
        private const string FateMax = "战斗模式";
        private const string FateMin = "Q偷模式";
        private const int FateRandomCode = 1;
        private const int FateMaxCode = 2;
        private const int FateMinCode = 3;

        private const int RandomSeedBaseAddrPTR = 0xF7AE7CC;

        private readonly Dictionary<int, string> FateNameMapping = new Dictionary<int, string>
        {
            { FateRandomCode, FateRandom },
            { FateMaxCode, FateMax },
            { FateMinCode, FateMin },
        };
        private int currentFate = 1;
        private Random random = new Random();

        public override EPluginPosition GetPosition()
        {
            return EPluginPosition.BR;
        }
        public override string GetResult()
        {
            if (FateNameMapping.TryGetValue(currentFate, out var display))
            {
                return display;
            }

            return FateRandom;
        }

        public override void OnLoad()
        {

        }

        public override void OnUnload()
        {
        }

        public override void Flush(IntPtr handle, int PID, int BaseAddr32, long BaseAddr64)
        {
            int RandomSeedBaseAddr = Readm<int>(handle, RandomSeedBaseAddrPTR);
            int RandomSeedAddr = RandomSeedBaseAddr + 0x2C;

            switch (currentFate)
            {
                case FateMinCode:
                    Writem(handle, RandomSeedAddr, 0x0);
                    break;
                case FateMaxCode:
                    Writem(handle, RandomSeedAddr, 0x0FFFFFF);
                    break;
                case FateRandomCode:
                    Writem(handle, RandomSeedAddr, random.Next() & 0x0FFFFFF);
                    break;
            }
        }

        public override void OnEvent(string name, object data)
        {
            if (name == "MakeFate" && data != null)
            {
                currentFate = (int)data;
            }
        }

        protected static bool Writem(IntPtr handle, int addr, int data)
        {
            byte[] buffer = BitConverter.GetBytes(data);
            int size = buffer.Length;
            int sizeofWrite;

            if (Kernel32.WriteProcessMemory(handle, new IntPtr(addr), buffer, size, out sizeofWrite))
            {
                if (sizeofWrite == size)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public static class Kernel32
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            int size,
            out int lpNumberOfBytesWritten
        );
    }
}
