﻿using System;
using System.IO;
using Bing.Barcode.Abstractions;
using Bing.Barcode.Enums;

namespace Bing.Barcode.Core
{
    /// <summary>
    /// 条形码服务基类
    /// </summary>
    public abstract class BarcodeServiceBase : IBarcodeService
    {
        /// <summary>
        /// 条形码参数
        /// </summary>
        private BarcodeParam _param;

        /// <summary>
        /// 设置条形码参数
        /// </summary>
        /// <param name="param">条形码参数</param>
        public IBarcodeService Param(BarcodeParam param)
        {
            _param = param;
            Init(_param);
            return this;
        }

        /// <summary>
        /// 转换成流
        /// </summary>
        public virtual Stream ToStream() => new MemoryStream(Create(_param));

        /// <summary>
        /// 转换成字节数组
        /// </summary>
        public virtual byte[] ToBytes() => Create(_param);

        /// <summary>
        /// 转换成Base64字符串
        /// </summary>
        public virtual string ToBase64String() => Convert.ToBase64String(Create(_param));

        /// <summary>
        /// 转换成Base64字符串，并附带前缀
        /// </summary>
        /// <param name="type">图片类型</param>
        public virtual string ToBase64String(Base64ImageType type) =>
            $"{GetBase64StringPrefix(type)}{ToBase64String()}";

        /// <summary>
        /// 获取Base64字符串前缀
        /// </summary>
        /// <param name="type">图片类型</param>
        private string GetBase64StringPrefix(Base64ImageType type)
        {
            switch (type)
            {
                case Base64ImageType.Gif:
                    return "data:image/gif;base64,";
                case Base64ImageType.Jpeg:
                    return "data:image/jpeg;base64,";
                case Base64ImageType.Png:
                    return "data:image/png;base64,";
                case Base64ImageType.XIcon:
                    return "data:image/x-icon;base64,";
            }

            throw new NotImplementedException($"未知图片类型 : {type}");
        }

        /// <summary>
        /// 写入到文件
        /// </summary>
        /// <param name="path">文件路径</param>
        public virtual string WriteToFile(string path)
        {
            File.WriteAllBytes(path, Create(_param));
            return path;
        }

        /// <summary>
        /// 处理容错级别
        /// </summary>
        /// <param name="level">容错级别</param>
        protected abstract void HandlerCorrectionLevel(ErrorCorrectionLevel level);

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void Init(BarcodeParam param)
        {
            HandlerCorrectionLevel(param.Level);
        }

        /// <summary>
        /// 创建条形码
        /// </summary>
        /// <param name="param">条形码参数</param>
        protected abstract byte[] Create(BarcodeParam param);
    }
}
