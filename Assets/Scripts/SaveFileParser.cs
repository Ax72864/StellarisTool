using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Threading;
using Stopwatch = System.Diagnostics.Stopwatch;
using System.Security.Cryptography;

namespace StellarisTool
{
    public class SaveFileParser
    {

        private Action<float> ProgressCallback;
        public string FilePath;
        public StellarisSave saveData;
        public string PersistentDataPath;
        public SaveFileParser(string filePath,StellarisSave saveInst = null, Action<float> progressCallback = null)
        {
            FilePath = filePath;
            saveData = saveInst ?? new StellarisSave();
            ProgressCallback = progressCallback;
        }

        private StreamReader _reader;
        private long _fileSize;
        private Stopwatch stopwatch;

        public void Parse() 
        {
            //string gameStatePath = PersistentDataPath + "/ironman.json";
            stopwatch = new Stopwatch();
            stopwatch.Start();

            var md5 = GetMd5(FilePath);
            var fileName = Path.GetFileName(FilePath).Replace(".sav",$"_{md5}.json");
            string gameStatePath = PersistentDataPath + "/" + fileName;
            if (!File.Exists(gameStatePath))
            {
                using (var stream = SaveManager.ReadFileStreamFromZip(FilePath, "gamestate", out var fileSize))
                {
                    _fileSize = fileSize;
                    using (var reader = new StreamReader(stream))
                    {
                        _reader = reader;
                        var patterns = GetAllPattern(reader);
                        gameStatePath = Deserilize(patterns, gameStatePath);
                    }
                }
            }
            if (gameStatePath != null)
            {
                try
                {

                    ProgressCallback?.Invoke(0.95f);
                    var file = File.OpenText(gameStatePath);
                    var content = file.ReadToEnd();
                    var tempSave = JsonConvert.DeserializeObject<StellarisGameState>(content);
                    saveData.gameState = tempSave;
                    ProgressCallback?.Invoke(0.99f);
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
            }
            var metafileName = Path.GetFileName(FilePath).Replace(".sav", $"_meta_{md5}.json");
            string metaPath = PersistentDataPath + "/" + metafileName;
            if (!File.Exists(metaPath))
            {
                using (var stream = SaveManager.ReadFileStreamFromZip(FilePath, "meta", out var fileSize))
                {
                    _fileSize = fileSize;
                    using (var reader = new StreamReader(stream))
                    {
                        _reader = reader;
                        var patterns = GetAllPattern(reader);
                        metaPath = Deserilize(patterns, metaPath);
                    }
                }
            }

            if (metaPath != null)
            {
                var metafile = File.OpenText(gameStatePath);
                var metacontent = metafile.ReadToEnd();
                var tempMeta = JsonConvert.DeserializeObject<StellarisGameMeta>(metacontent);
                saveData.meta = tempMeta;
                ProgressCallback?.Invoke(1f);
            }

            saveData.ScanBaseInfo();
            stopwatch.Stop();
        }
        #region Deserilize

        private enum ContainerType
        {
            Dict,
            List,
        }

        public string Deserilize(string[] patterns,string outpath = "")
        {
            //var depth = 0;
            // 
            dstack.Clear();
            tstack.Clear();
            pidx = 0;
            this.patterns = patterns;
            var root = new Dictionary<string, object>();
            //curType = ContainerType.Dict;
            dstack.Push(root);
            tstack.Push(ContainerType.Dict);
            tick = 0;
            try
            {
                GetValue();
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                //throw;
            }
            if (string.IsNullOrEmpty(outpath))
            {
                var md5 = GetMd5(FilePath);
                var fileName = Path.GetFileName(FilePath).Replace(".sav", $"_{md5}.json");
                outpath = PersistentDataPath + "/" + fileName;
            }
            using (StreamWriter file = File.CreateText(outpath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.DefaultValueHandling = DefaultValueHandling.Populate;
                serializer.Serialize(file, root);
            }
            ProgressCallback?.Invoke(0.95f);
            return outpath;
        }

        private int tick = 0;
        private Stack<object> dstack = new Stack<object>();
        private Stack<ContainerType> tstack = new Stack<ContainerType>();
        private int pidx = 0;
        private string[] patterns;
        object GetValue()
        {
            var p = patterns[pidx];
            while (p != "}")
            {
                tick++;
                if (pidx % 10000 == 0)
                {
                    ProgressCallback?.Invoke(0.5f + (float)pidx / patterns.Length * 0.4f);
                }
                var key = p;
                if (tstack.Peek() == ContainerType.List)
                {
                    //当前是List
                    //var root = dstack.Peek() as List<object>;
                    if (key == "{")
                    {
                        //可能是对象list,判断是list还是dict
                        if (patterns[pidx + 2] == "=")
                        {
                            //dict
                            var dict = new Dictionary<string, object>();
                            dstack.Push(dict);
                            tstack.Push(ContainerType.Dict);
                            pidx += 1;
                            GetValue();
                        }
                        else
                        {
                            //list
                            var list = new List<object>();
                            dstack.Push(list);
                            tstack.Push(ContainerType.List);
                            pidx += 1;
                            GetValue();
                        }
                        tstack.Pop();
                        var nvalue = dstack.Pop();
                        SetListValue(nvalue);
                        //root.Add(nvalue);

                    } else
                    {
                        SetListValue(ConvertValue(key));
                        //root.Add(key);
                        pidx += 1;
                    }
                } else
                {

                    //当前是Dict
                    if (pidx+2>=patterns.Length)
                    {
                        Debug.Log("");
                        return null;
                    }
                    var value = patterns[pidx+2];
                    //var root = dstack.Peek() as Dictionary<string, object>;
                    if (value == "{")
                    {
                        //判断value是list还是Dict
                        if (patterns[pidx + 3] == "}")
                        {
                            //空对象
                            var nvalue = new Dictionary<string, object>();
                            //root.Add(key, nvalue);
                            SetDictValue(key, nvalue);
                            pidx += 4;
                        } else if (patterns[pidx + 4] == "=")
                        {
                            //是dict
                            var dict = new Dictionary<string, object>();
                            dstack.Push(dict);
                            tstack.Push(ContainerType.Dict);
                            pidx += 3;
                            GetValue();
                            tstack.Pop();
                            var nvalue = dstack.Pop();
                            SetDictValue(key, nvalue);
                        } else
                        {
                            //是list
                            var list = new List<object>();
                            dstack.Push(list);
                            tstack.Push(ContainerType.List);
                            pidx += 3;
                            GetValue();
                            tstack.Pop();
                            var nvalue = dstack.Pop();
                            SetDictValue(key, nvalue);
                        }
                    } else
                    {
                        SetDictValue(key, ConvertValue(value));
                        pidx += 3;
                    }
                }
                if (pidx < patterns.Length)
                {
                    p = patterns[pidx];
                } else
                {
                    return dstack.Peek();
                }
            }
            pidx += 1;
            return dstack.Peek();
        }

        void SetListValue(object value)
        {
            var root = dstack.Peek() as List<object>;
            if (value.GetType() == typeof(string) && value.ToString() == "none")
            {
                return;
            }
            root.Add(value);
        }

        void SetDictValue(string key,object value)
        {
            var root = dstack.Peek() as Dictionary<string, object>;
            if (value.GetType() == typeof(string) && value.ToString() == "none")
            {
                return;
            }
            if (root.ContainsKey(key))
            {
                var pvalue = root[key];
                var nlist = pvalue as List<object>;
                if (nlist == null)
                {
                    nlist = new List<object>
                    {
                        pvalue,
                        value
                    };
                    root[key] = nlist;
                } else
                {
                    nlist.Add(value);
                }
            } else
            {
                root.Add(key, value);
            }
        }

        object ConvertValue(string oriValue)
        {
            if (int.TryParse(oriValue,out var intValue))
            {
                return intValue;
            }
            if (uint.TryParse(oriValue, out var uintValue))
            {
                return uintValue;
            }
            if (float.TryParse(oriValue,out var floatValue))
            {
                return floatValue;
            }
            if (oriValue.Equals("yes"))
            {
                return true;
            } else if (oriValue.Equals("no"))
            {
                return false;
            }
            return oriValue;
        }

        #endregion


        #region Pattern

        public HashSet<char> whiteChars = new HashSet<char>()
        {
            ' ','\t','\n'
        };
        public HashSet<char> separator = new HashSet<char>()
        {
            '=','{','}',' ','\n','\t',','//,'+','-','*','/','\\','[',']','(',')'
        };
        public HashSet<char> quote = new HashSet<char>()
        {
            '\'','"'
        };

        public Stack<char> stat = new Stack<char>();


        private char[] buffer = new char[1024];
        string[] GetAllPattern(StreamReader reader)
        {
            buffer = new char[1024];
            readPos = 0;
            pos = 0;
            bufferLen = 0;
            zone = 0;
            var elapsedTime = stopwatch.Elapsed;
            List<string> patterns = new List<string>();
            while (!isEnd())
            {
                var pattern = GetNextPattern();

                if (!string.IsNullOrEmpty(pattern))
                {
                    if (pattern.StartsWith("\"") && pattern.EndsWith("\""))
                    {
                        pattern = pattern.Substring(1, pattern.Length - 2);
                    }
                    patterns.Add(pattern);
                    if ((stopwatch.Elapsed - elapsedTime).TotalMilliseconds > 16f)
                    {
                        ProgressCallback?.Invoke((float)readPos / _fileSize * 0.5f);
                        elapsedTime = stopwatch.Elapsed;
                    }
                }                
            }
            Debug.Log($"Got All Patterns Done ! total: {patterns.Count}");
            return patterns.ToArray();
        }



        private int readPos = 0;
        private int pos = 0;
        private int bufferLen = 0;
        private int zone = 0;

        char GetChar()
        {
            if (pos < 0)
            {
                pos = 128 + pos;
            }
            if (pos >= zone*64 && pos < zone*64+bufferLen)
            {
                return buffer[pos];
            } else
            {
                bufferLen = _reader.Read(buffer, zone*64, 64);
                readPos += bufferLen;
                zone^=1;
                if (pos == 128)
                {
                    pos = 0;
                }
                return buffer[pos];
            }
        }

        bool isEnd()
        {
            if (!_reader.EndOfStream)
            {
                return false;
            } else
            {
                return pos >= zone * 64 + bufferLen;
            }
        }

        string GetNextPattern()
        {
            char curchar = '0';
            var partten = "";
            while (!isEnd())
            {
                if (partten.Length > 512)
                {
                    throw new System.Exception($"Parttern too long,check parser! current pointer At:{readPos},{partten}");
                }
                curchar = GetChar();
                if (stat.Count == 0)
                {
                    if (separator.Contains(curchar))
                    {
                        if (whiteChars.Contains(curchar))
                        {
                            while (whiteChars.Contains(curchar) && !isEnd())
                            {
                                pos++;
                                curchar = GetChar();
                            }
                            if (partten != string.Empty)
                            {
                                return partten;
                            }
                            else
                            {
                                pos--;
                            }
                        }
                        else
                        {
                            if (partten != string.Empty)
                            {
                                return partten;
                            }
                            else
                            {
                                partten += curchar;
                                pos++;
                                return partten;
                            }
                        }
                    }
                    else if (quote.Contains(curchar))
                    {
                        stat.Push(curchar);
                        partten += curchar;
                    }
                    else
                    {
                        partten += curchar;
                    }
                } else
                {
                    if (stat.Peek() == curchar)
                    {
                        stat.Pop();
                        partten += curchar;
                        if (stat.Count == 0)
                        {
                            pos++;
                            return partten;
                        } else
                        {
                            partten += curchar;
                        }
                    } else if (quote.Contains(curchar))
                    {
                        if (curchar == '"' && !stat.Contains('\''))
                        {
                            stat.Push(curchar);
                        } else if (curchar == '\'' && !stat.Contains('"'))
                        {
                            stat.Push(curchar);
                        }
                        partten += curchar;
                    }
                    else
                    {
                        partten += curchar;
                    }
                }
                pos++;
            }

            return null;
        }

        private static string GetMd5(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    byte[] hashBytes = md5.ComputeHash(stream);
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                }
            }
        }
        //public IEnumerator<string> GetEnumerator()
        //{
        //    while (!_reader.EndOfStream)
        //    {
        //        var pattern = GetNextPattern();
        //        if (!string.IsNullOrEmpty(pattern))
        //        {
        //            yield return pattern;
        //        }
        //    }
        //    yield return null;
        //}

        //public bool MoveNext()
        //{
        //    return _reader != null && !_reader.EndOfStream;
        //}

        //public void Reset()
        //{
        //    _reader = null;
        //}

        //public void Dispose()
        //{
        //    _reader.Dispose();
        //}
        #endregion
    }

}