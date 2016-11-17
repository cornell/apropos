﻿// <copyright file="GifFormat.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Formats
{
    /// <summary>
    /// Encapsulates the means to encode and decode gif images.
    /// </summary>
    public class GifFormat : IImageFormat
    {
        /// <inheritdoc/>
        public IImageDecoder Decoder => new GifDecoder();

        /// <inheritdoc/>
        public IImageEncoder Encoder => new GifEncoder();
    }
}
