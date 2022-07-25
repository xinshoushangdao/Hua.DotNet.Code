using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using Hua.DotNet.Code.Ex;
using Newtonsoft.Json;

namespace Hua.DotNet.Code.Helper
{
    /// <summary>
    /// 计算机帮助类
    /// </summary>
    public class ComputerHelper
    {
        /// <summary>
        /// 内存使用情况
        /// </summary>
        /// <returns></returns>
        public static MemoryMetrics GetComputerInfo()
        {
            try
            {
                MemoryMetricsClient client = new();
                var memoryMetrics = IsUnix() ? client.GetUnixMetrics() : client.GetWindowsMetrics();

                memoryMetrics.FreeRam = Math.Round(memoryMetrics.Free / 1024, 2) + "GB";
                memoryMetrics.UsedRam = Math.Round(memoryMetrics.Used / 1024, 2) + "GB";
                memoryMetrics.TotalRAM = Math.Round(memoryMetrics.Total / 1024, 2) + "GB";
                memoryMetrics.RAMRate = Math.Ceiling(100 * memoryMetrics.Used / memoryMetrics.Total)
                    .ToString(CultureInfo.InvariantCulture) + "%";
                memoryMetrics.CPURate = Math.Ceiling(GetCPURate().ToDouble()) + "%";
                return memoryMetrics;
            }
            catch (Exception ex)
            {
                LogHelper.Log($"获取内存使用出错，msg={ex.Message},{ex.StackTrace}");
            }

            return new MemoryMetrics();
        }

        /// <summary>
        /// 获取内存大小
        /// </summary>
        /// <returns></returns>
        public static List<DiskInfo> GetDiskInfos()
        {
            List<DiskInfo> diskInfos = new();

            if (IsUnix())
            {
                try
                {
                    var output = ShellHelper.Bash("df -m / | awk '{print $2,$3,$4,$5,$6}'");
                    var arr = output.Split('\n');
                    if (arr.Length == 0) return diskInfos;

                    var rootDisk = arr[1].Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
                    if (rootDisk.Length == 0)
                    {
                        return diskInfos;
                    }

                    DiskInfo diskInfo = new()
                    {
                        DiskName = "/",
                        TotalSize = long.Parse(rootDisk[0]) / 1024,
                        Used = long.Parse(rootDisk[1]) / 1024,
                        AvailableFreeSpace = long.Parse(rootDisk[2]) / 1024,
                        AvailablePercent = decimal.Parse(rootDisk[3].Replace("%", ""))
                    };
                    diskInfos.Add(diskInfo);
                }
                catch (Exception ex)
                {
                    LogHelper.Log("获取磁盘信息出错了" + ex.Message);
                }
            }
            else
            {
                var driv = DriveInfo.GetDrives();
                foreach (var item in driv)
                {
                    try
                    {
                        var obj = new DiskInfo()
                        {
                            DiskName = item.Name,
                            TypeName = item.DriveType.ToString(),
                            TotalSize = item.TotalSize / 1024 / 1024 / 1024,
                            AvailableFreeSpace = item.AvailableFreeSpace / 1024 / 1024 / 1024,
                        };
                        obj.Used = obj.TotalSize - obj.AvailableFreeSpace;
                        obj.AvailablePercent = decimal.Ceiling(obj.Used / (decimal)obj.TotalSize * 100);
                        diskInfos.Add(obj);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Log("获取磁盘信息出错了" + ex.Message);
                        continue;
                    }
                }
            }

            return diskInfos;
        }

        public static bool IsUnix()
        {
            var isUnix = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ||
                         RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            return isUnix;
        }

        public static string GetCPURate()
        {
            string cpuRate;
            if (IsUnix())
            {
                var output = ShellHelper.Bash("top -b -n1 | grep \"Cpu(s)\" | awk '{print $2 + $4}'");
                cpuRate = output.Trim();
            }
            else
            {
                var output = ShellHelper.Cmd("wmic", "cpu get LoadPercentage");
                cpuRate = output.Replace("LoadPercentage", string.Empty).Trim();
            }

            return cpuRate;
        }

        /// <summary>
        /// 获取系统运行时间
        /// </summary>
        /// <returns></returns>
        public static string GetRunTime()
        {
            var runTime = string.Empty;
            try
            {
                if (IsUnix())
                {
                    var output = ShellHelper.Bash("uptime -s").Trim();
                    runTime = DateTimeHelper.FormatTime(
                        (DateTime.Now - output.ToDateTime()).TotalMilliseconds.ToString().Split('.')[0].ToLong());
                }
                else
                {
                    var output = ShellHelper.Cmd("wmic", "OS get LastBootUpTime/Value");
                    var outputArr = output.Split('=', (char)StringSplitOptions.RemoveEmptyEntries);
                    if (outputArr.Length == 2)
                    {
                        runTime = DateTimeHelper.FormatTime((DateTime.Now - outputArr[1].Split('.')[0].ToDateTime())
                            .TotalMilliseconds.ToString(CultureInfo.InvariantCulture).Split('.')[0].ToLong());
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log("获取runTime出错" + ex.Message);
            }

            return runTime;
        }
    }

    /// <summary>
    /// 内存信息
    /// </summary>
    public class MemoryMetrics
    {
        [JsonIgnore] public double Total { get; set; }
        [JsonIgnore] public double Used { get; set; }
        [JsonIgnore] public double Free { get; set; }

        public string UsedRam { get; set; }

        /// <summary>
        /// CPU使用率%
        /// </summary>
        public string CPURate { get; set; }

        /// <summary>
        /// 总内存 GB
        /// </summary>
        public string TotalRAM { get; set; }

        /// <summary>
        /// 内存使用率 %
        /// </summary>
        public string RAMRate { get; set; }

        /// <summary>
        /// 空闲内存
        /// </summary>
        public string FreeRam { get; set; }
    }

    public class DiskInfo
    {
        /// <summary>
        /// 磁盘名
        /// </summary>
        public string DiskName { get; set; }

        public string TypeName { get; set; }
        public long TotalFree { get; set; }
        public long TotalSize { get; set; }

        /// <summary>
        /// 已使用
        /// </summary>
        public long Used { get; set; }

        /// <summary>
        /// 可使用
        /// </summary>
        public long AvailableFreeSpace { get; set; }

        public decimal AvailablePercent { get; set; }
    }

    public class MemoryMetricsClient
    {
        #region 获取内存信息

        /// <summary>
        /// windows系统获取内存信息
        /// </summary>
        /// <returns></returns>
        public MemoryMetrics GetWindowsMetrics()
        {
            var output = ShellHelper.Cmd("wmic", "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value");
            var metrics = new MemoryMetrics();
            var lines = output.Trim().Split('\n', (char)StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length <= 0) return metrics;

            var freeMemoryParts = lines[0].Split('=', (char)StringSplitOptions.RemoveEmptyEntries);
            var totalMemoryParts = lines[1].Split('=', (char)StringSplitOptions.RemoveEmptyEntries);

            metrics.Total = Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 0);
            metrics.Free = Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 0); //m
            metrics.Used = metrics.Total - metrics.Free;

            return metrics;
        }

        /// <summary>
        /// Unix系统获取
        /// </summary>
        /// <returns></returns>
        public MemoryMetrics GetUnixMetrics()
        {
            var output = ShellHelper.Bash("free -m | awk '{print $2,$3,$4,$5,$6}'");
            var metrics = new MemoryMetrics();
            var lines = output.Split('\n', (char)StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length <= 0) return metrics;

            if (lines.Length <= 0) return metrics;
            var memory = lines[1].Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
            if (memory.Length < 3) return metrics;
            metrics.Total = double.Parse(memory[0]);
            metrics.Used = double.Parse(memory[1]);
            metrics.Free = double.Parse(memory[2]);
            return metrics;
        }

        #endregion
    }
}