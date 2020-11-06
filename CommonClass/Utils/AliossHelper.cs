using Aliyun.OSS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Common.Utils
{
    internal class AliossHelper
    {
        private const string accessKeyId = "";
        private const string accessKeySecret = "";
        private const string endpoint = "";
        private const string bucketName = "";
        public static string basePath = "";

        private static readonly OssClient client = new OssClient(endpoint, accessKeyId, accessKeySecret);
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public static bool UpLoadFile(byte[] binaryData, string filename)
        {
            _logger.Info(filename + ": 开始上传...");
            var metadata = new ObjectMetadata
            {
                //ContentDisposition = $"attachment; filename=\"{basePath}{filename}\"",
                ContentLength = binaryData.Length
            };
            try
            {
                using (var requestContent = new MemoryStream(binaryData))
                {
                    client.PutObject(bucketName, basePath + filename, requestContent, metadata);
                    _logger.Info(filename + ": 成功上传到OSS");
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message ?? "上传文件失败");
                return false;
            }
        }

        /// <summary>
        /// 删除OSS上的文件
        /// </summary>
        /// <param name="filename">文件的完整文件名列表(包含路径)</param>
        /// <returns></returns>
        public static bool DeleteFiles(List<string> filename)
        {
            _logger.Info("开始删除OSS数据...");
            try
            {
                var request = new DeleteObjectsRequest(bucketName, filename.Select(x => basePath + x).ToList(), false);
                var res = client.DeleteObjects(request);
                _logger.Info("OSS文件删除成功...");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message ?? "文件删除失败..." + string.Join(",", filename));
                return false;
            }
        }

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="fileName">下载的文件名称</param>
        /// <param name="orginFileName">原始文件名</param>
        /// <returns></returns>
        public static Tuple<string, byte[]> DownLoadFile(string fileName, string orginFileName)
        {
            _logger.Info(orginFileName + ": 开始下载...");
            try
            {
                var result = GetImagsList(orginFileName);
                // 没有获取到数据
                if (result.Count == 0) return Tuple.Create(fileName, new byte[0]);

                string realName = result[0];
                int index = realName.LastIndexOf(".");
                if (index != -1)
                {
                    fileName += realName.Substring(index);
                }

                var obj = client.GetObject(bucketName, realName);
                // 初始化byte数组
                var buff = new byte[obj.Content.Length];
                using (var requestStream = obj.Content)
                {
                    int pos = 0, length = 0;
                    int loopCount = buff.Length / 256;
                    for (int i = 0; i < loopCount; i++)
                    {
                        // length是读取的长度,pos每次读取的起始位置
                        length = requestStream.Read(buff, pos, 256);
                        pos += length;
                    }
                    // 最后一次手动读取
                    requestStream.Read(buff, pos, (int)(obj.Content.Length - pos));
                }
                return Tuple.Create(fileName, buff);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message ?? "下载文件失败");
                return Tuple.Create(fileName, new byte[0]);
            }
        }

        // 单张图片删除
        public static bool DeleteFile(string orginFileName)
        {
            _logger.Info("开始删除OSS数据...");
            // 根据图片前缀模糊删除
            try
            {
                var result = GetImagsList(orginFileName);
                if (result.Count > 0)
                {
                    var request = new DeleteObjectsRequest(bucketName, result, false);
                    var res = client.DeleteObjects(request);
                    _logger.Info("OSS文件删除成功...");
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message ?? "文件删除失败..." + string.Join(",", orginFileName));
                return false;
            }
        }

        // 根据路径匹配获取图片列表
        public static List<string> GetImagsList(string orginFileName)
        {
            var list = new List<string>();
            // 根据文件名称前缀去查询图片列表，一次查询20条数据，从第一个开始查询
            var listObjectsRequest = new ListObjectsRequest(bucketName)
            {
                Marker = string.Empty,
                MaxKeys = 100,
                Prefix = basePath + orginFileName,
            };
            ObjectListing result = client.ListObjects(listObjectsRequest);

            foreach (var summary in result.ObjectSummaries)
            {
                list.Add(summary.Key);
            }
            return list;
        }

        /// <summary>
        /// 获取图片的url,临时访问，1小时后过期
        /// </summary>
        /// <param name="orginFileName">原始文件名</param>
        /// <returns></returns>
        public static string GetTemporaryUrl(string orginFileName)
        {
            _logger.Info(orginFileName + ": 开始获取...");
            try
            {
                var result = GetImagsList(orginFileName);
                if (result.Count > 0)
                {
                    // 找到需要的数据，结束查找
                    var generatePresignedUriRequest = new GeneratePresignedUriRequest(bucketName, result[0], SignHttpMethod.Get)
                    {
                        Expiration = DateTime.Now.AddHours(1),
                    };
                    var signedUrl = client.GeneratePresignedUri(generatePresignedUriRequest);
                    return signedUrl.ToString();
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message ?? "下载文件失败");
                return null;
            }
        }
    }
}