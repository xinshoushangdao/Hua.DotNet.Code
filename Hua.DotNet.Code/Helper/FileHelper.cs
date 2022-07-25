﻿using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Hua.DotNet.Code.Helper
{
    public class FileHelper
    {
        /// <summary>
        /// 按时间来创建文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <returns>eg: /{yourPath}/2020/11/3/</returns>
        public static string GetDirPath(string path = "")
        {
            var date = DateTime.Now;
            var timeDir = date.ToString("yyyyMMdd"); // date.ToString("yyyyMM/dd/HH/");

            if (!string.IsNullOrEmpty(path))
            {
                timeDir = Path.Combine(path, timeDir);
            }

            return timeDir;
        }

        /// <summary>
        /// 取文件名的MD5值(16位)
        /// </summary>
        /// <param name="str">文件名，不包括扩展名</param>
        /// <returns></returns>
        public static string HashFileName(string str = null)
        {
            if (string.IsNullOrEmpty(str))
            {
                str = Guid.NewGuid().ToString();
            }

            MD5CryptoServiceProvider md5 = new();
            return BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(str)), 4, 8).Replace("-", "");
        }

        /// <summary>
        /// 删除指定目录下的所有文件及文件夹(保留目录)
        /// </summary>
        /// <param name="file">文件目录</param>
        public static void DeleteDirectory(string file)
        {
            try
            {
                //判断文件夹是否还存在
                if (!Directory.Exists(file)) return;
                var fileInfo = new DirectoryInfo(file)
                {
                    //去除文件夹的只读属性
                    Attributes = FileAttributes.Normal & FileAttributes.Directory
                };
                foreach (var f in Directory.GetFileSystemEntries(file))
                {
                    if (File.Exists(f))
                    {
                        //去除文件的只读属性
                        File.SetAttributes(file, FileAttributes.Normal);
                        //如果有子文件删除文件
                        File.Delete(f);
                    }
                    else
                    {
                        //循环递归删除子文件夹
                        DeleteDirectory(f);
                    }
                }

                //删除空文件夹
                Directory.Delete(file);
            }
            catch (Exception ex) // 异常处理
            {
                LogHelper.Log(ex.Message);
            }
        }

        /// <summary>
        /// 压缩代码
        /// </summary>
        /// <param name="zipPath"></param>
        /// <param name="genCodePath"></param>
        /// <param name="zipFileName">压缩后的文件名</param>
        /// <returns></returns>
        public static bool ZipGenCode(string zipPath, string genCodePath, string zipFileName)
        {
            if (string.IsNullOrEmpty(zipPath)) return false;
            try
            {
                CreateDirectory(genCodePath);
                var zipFileFullName = Path.Combine(zipPath, zipFileName);
                if (File.Exists(zipFileFullName))
                {
                    File.Delete(zipFileFullName);
                }

                ZipFile.CreateFromDirectory(genCodePath, zipFileFullName);
                DeleteDirectory(genCodePath);

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Log("压缩文件出错。", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool CreateDirectory(string path)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                path = path.Replace("\\", "/").Replace("//", "/");
            }

            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log("创建文件夹出错了", ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="path">完整路径带扩展名的</param>
        /// <param name="content"></param>
        public static void WriteAndSave(string path, string content)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                path = path.Replace("\\", "/").Replace("//", "/");
            }

            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path) ?? string.Empty);
            }

            LogHelper.Log("开始写入文件，Path=" + path);
            try
            {
                //实例化一个文件流--->与写入文件相关联
                using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                //实例化一个StreamWriter-->与fs相关联
                using var sw = new StreamWriter(fs);
                //开始写入
                sw.Write(content);
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                LogHelper.Log("写入文件出错了:", ex.Message);
            }
        }
    }
}