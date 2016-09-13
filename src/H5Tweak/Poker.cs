using System;
using System.Diagnostics;
using System.Linq;

using Binarysharp.MemoryManagement;
using Binarysharp.MemoryManagement.Helpers;
using System.Text.RegularExpressions;

namespace H5Tweak
{
    public class Poker
    {
        const string EXPECTED_VERSION = "1.114.4592.2";
        static Regex versionRegex = new Regex(@"Microsoft.Halo5Forge_([0-9]+.[0-9]+.[0-9]+.[0-9]+)_x64__8wekyb3d8bbwe");

        Process process;

        public static bool TryGetHaloPoker(out Poker poker)
        {
            Process haloProcess = ApplicationFinder.FromProcessName("halo5forge").FirstOrDefault();

            if (haloProcess == null)
            {
                poker = null;
                return false;
            }

            Match match = versionRegex.Match(haloProcess.MainModule.FileName);
            if (!match.Success)
            {
                throw new Exception(String.Format("Wrong version. Failed to parse: {0}.", haloProcess.MainModule.FileName));
            }

            String version = match.Groups[1].Value;
            if (version != EXPECTED_VERSION)
            {
                throw new Exception(String.Format("Wrong version. Expected={0}. Actual={1}.", EXPECTED_VERSION, version));
            }

            poker = new Poker { process = haloProcess };
            return true;
        }

        public int GetFPS()
        {
            using (var m = new MemorySharp(this.process))
            {
                var fpsPtr = new IntPtr(0x34B8C50);
                int[] integers = m.Read<int>(fpsPtr, 1);
                int math = 1000000 / integers[0];
                return math;
            }
        }

        public int GetFOV()
        {
            using (var m = new MemorySharp(this.process))
            {
                var fovPtr = new IntPtr(0x58ECF90);
                return Convert.ToInt16(m[fovPtr].Read<float>());
            }
        }

        public void SetFOV(float fov)
        {
            using (var m = new MemorySharp(this.process))
            {
                var fovPtr = new IntPtr(0x58ECF90);
                m[fovPtr].Write<float>(fov);
            }
        }

        public void SetFPS(int fps)
        {
            using (var m = new MemorySharp(this.process))
            {
                var fovPtr = new IntPtr(0x34B8C50);
                var fovPtr1 = new IntPtr(0x34B8C60);
                var fovPtr2 = new IntPtr(0x34B8C70);
                m[fovPtr].Write<int>(fps);
                m[fovPtr1].Write<int>(fps);
                m[fovPtr2].Write<int>(fps);
            }
        }
    }
}
