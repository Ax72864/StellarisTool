using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using UnityEngine;
using System.Threading;
using System;
using System.Runtime.InteropServices;

namespace StellarisTool
{
    public class SaveManager : MonoBehaviour
    {

        private static Action<float> ProgressCallback;
        private static Action<StellarisSave> CompleteCallback;
        private static Thread runningTh;
        private static string LoadFilePath;


        public static StellarisSave saveData;
        public static string PersistentDataPath = Application.persistentDataPath;
        public static void LoadSaveFileAsync(string filepath, Action<float> progressCallback = null, Action<StellarisSave> completeCallback = null)
        {
            if (!File.Exists(filepath))
            {
                completeCallback?.Invoke(null);
                return;
            }
            LoadFilePath = filepath;
            ProgressCallback = progressCallback;
            CompleteCallback = completeCallback;
            runningTh = new Thread(LoadInThread);
            runningTh.Start();
        }

        private static void LoadInThread()
        {
            var filepath = LoadFilePath;
            var saveFile = new StellarisSave();
            var parser = new SaveFileParser(filepath, saveFile, ProgressCallback);
            parser.PersistentDataPath = PersistentDataPath;
            parser.Parse();
            CompleteCallback?.Invoke(saveFile);
            saveData = saveFile;
            runningTh = null;
        }

        public static StellarisSave LoadSaveFile(string filepath)
        {
            if (!File.Exists(filepath))
            {
                return null;
            }

            var saveFile = new StellarisSave();
            var parser = new SaveFileParser(filepath, saveFile, ProgressCallback);
            parser.PersistentDataPath = PersistentDataPath;
            parser.Parse();
            CompleteCallback?.Invoke(saveFile);
            saveData = saveFile;
            return saveFile;
        }


        #region Tool Funcion

        public static List<string> GetZipFileList(string zipFilePath)
        {
            List<string> fileNames = new List<string>();

            using (FileStream zipFile = new FileStream(zipFilePath, FileMode.Open))
            using (ZipArchive archive = new ZipArchive(zipFile, ZipArchiveMode.Read))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    fileNames.Add(entry.FullName);
                }
            }

            return fileNames;
        }
        public static string ReadFileFromZip(string zipFilePath, string fileName)
        {
            string fileContent = null;

            using (FileStream zipFile = new FileStream(zipFilePath, FileMode.Open))
            using (ZipArchive archive = new ZipArchive(zipFile, ZipArchiveMode.Read))
            {
                ZipArchiveEntry entry = archive.GetEntry(fileName);
                if (entry != null)
                {
                    using (StreamReader reader = new StreamReader(entry.Open()))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }
            return fileContent;
        }

        public static MemoryStream ReadFileStreamFromZip(string zipFilePath, string fileName, out long fileSize)
        {
            using (FileStream zipFile = new FileStream(zipFilePath, FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zipFile, ZipArchiveMode.Read))
                {
                    ZipArchiveEntry entry = archive.GetEntry(fileName);
                    if (entry != null)
                    {

                        MemoryStream memoryStream = new MemoryStream();
                        entry.Open().CopyTo(memoryStream);
                        fileSize = memoryStream.Length;
                        memoryStream.Seek(0, SeekOrigin.Begin); // 将内存流的位置定位到起始位置
                        return memoryStream;
                    }
                }
            }
            fileSize = -1;
            return null;
        }

        #endregion



        public static string WindowsOpenSelectFileDialog()
        {

            OpenFileDlg pth = new OpenFileDlg();
            pth.structSize = Marshal.SizeOf(pth);
            //pth.filter = "三菱(*.gxw)\0*.gxw\0西门子(*.mwp)\0*.mwp\0All Files\0*.*\0\0";
            pth.filter = $"Save Files\0*.sav\0All Files\0*.*\0\0";
            pth.file = new string(new char[256]);
            pth.maxFile = pth.file.Length;
            pth.fileTitle = new string(new char[64]);
            pth.maxFileTitle = pth.fileTitle.Length;
            //pth.initialDir = Application.dataPath.Replace("/", "\\") + "\\Resources"; //默认路径
            pth.title = "选择存档文件";
            pth.defExt = "txt";
            pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
            //Debug.LogError(1);
            if (OpenFileDialog.GetOpenFileName(pth))
            {
                Debug.Log($"Opened file: {pth.file}");
                if (File.Exists(pth.file))
                {
                    Debug.Log($" file exists: {pth.file}");
                    return pth.file;
                }
            }
            else
            {
                Debug.Log($"Opened file failed: {pth.file}");
            }
            return "";
        }


        public static string OpenWindowsFileDialog(string title,string defaultPath, string extension)
        {
            var opf = new ChinarFileDlog();
            opf.structSize = System.Runtime.InteropServices.Marshal.SizeOf(opf);
            opf.filter = $"Save Files\0{extension}\0All Files\0*.*\0\0";
            opf.file = new string(new char[256]);
            opf.maxFile = opf.file.Length;
            opf.title = title;
            //opf.initialDir = defaultPath;

            if (GetOpenFileName(opf))
            {
                return opf.file;
            }
            return "";

        }

        [DllImport("Comdlg32.dll",SetLastError =true,CharSet = CharSet.Auto)]
        public static extern bool GetOpenFileName([In, Out] ChinarFileDlog ofn);
        /// <summary>
        /// 文件日志类
        /// </summary>
        // [特性(布局种类.有序,字符集=字符集.自动)]
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class ChinarFileDlog
        {
            public int structSize = 0;
            public IntPtr dlgOwner = IntPtr.Zero;
            public IntPtr instance = IntPtr.Zero;
            public String filter = null;
            public String customFilter = null;
            public int maxCustFilter = 0;
            public int filterIndex = 0;
            public String file = null;
            public int maxFile = 0;
            public String fileTitle = null;
            public int maxFileTitle = 0;
            public String initialDir = null;
            public String title = null;
            public int flags = 0;
            public short fileOffset = 0;
            public short fileExtension = 0;
            public String defExt = null;
            public IntPtr custData = IntPtr.Zero;
            public IntPtr hook = IntPtr.Zero;
            public String templateName = null;
            public IntPtr reservedPtr = IntPtr.Zero;
            public int reservedInt = 0;
            public int flagsEx = 0;
        }



        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class OpenFileDlg : ChinarFileDlog
        {
        }

        public class OpenFileDialog
        {
            [DllImport("comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
            public static extern bool GetOpenFileName([In, Out] OpenFileDlg ofd);
        }

        public class SaveFileDialog
        {
            [DllImport("comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
            public static extern bool GetSaveFileName([In, Out] SaveFileDlg ofd);
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class SaveFileDlg : ChinarFileDlog
        {
        }
    }


}