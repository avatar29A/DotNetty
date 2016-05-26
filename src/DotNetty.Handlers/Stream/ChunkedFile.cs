// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DotNetty.Handlers.Stream
{
    using System;
    using System.IO;
    using DotNetty.Buffers;

    /// <summary>
    /// A <see cref="ChunkedInput"/> that fetches data from a file chunk by chunk.
    /// 
    /// If your operating system supports
    /// <a href="http://en.wikipedia.org/wiki/Zero-copy">zero-copy file transfer</a>
    /// such as <c>sendfile()</c>, you might want to use <see cref="FileRegion"/> instead.
    /// </summary>
    public class ChunkedFile : ChunkedInput<IByteBuffer>
    {
        FileStream file;
        readonly long startOffset;
        readonly long endOffset;
        readonly int chunkSize;
        long offset;

        public ChunkedFile(FileStream file)
            : this(file, ChunkedStream.DefaultChunkSize)
        {
        }

        public ChunkedFile(FileStream file, int chunkSize)
            : this(file, 0, file.Length, chunkSize)
        {
        }

        /// <summary>
        /// Creates a new instance that fetches data from the specified file.
        /// </summary>
        /// <param name="file">File.</param>
        /// <param name="offset">the offset of the file where the transfer begins</param>
        /// <param name="length">the number of bytes to transfer</param>
        /// <param name="chunkSize">the number of bytes to fetch on each {@link #ReadChunk(IChannelHandlerContext)} call</param>
        public ChunkedFile(FileStream file, long offset, long length, int chunkSize)
        {
            if (file == null)
            {
                throw new NullReferenceException(nameof(file));
            }
            if (offset < 0)
            {
                throw new ArgumentException($"offset: {offset} (expected: 0 or greater)");
            }
            if (length < 0)
            {
                throw new ArgumentException($"length: {length} (expected: 0 or greater)");
            }
            if (chunkSize <= 0)
            {
                throw new ArgumentException($"chunkSize: {chunkSize} (expected: a positive integer)");
            }

            this.file = file;
            this.offset = this.startOffset = offset;
            this.endOffset = offset + length;
            this.chunkSize = chunkSize;

            file.Seek(this.offset, SeekOrigin.Begin);
        }

        /// <summary>
        /// Returns the offset in the file where the transfer began.
        /// </summary>
        public long StartOffset { get { return this.startOffset; } }

        /// <summary>
        /// Returns the offset in the file where the transfer will end.
        /// </summary>
        public long EndOffset { get { return this.endOffset; } }

        /// <summary>
        /// Returns the offset in the file where the transfer is happening currently.
        /// </summary>
        /// <value>The current offset.</value>
        public long CurrentOffset { get { return this.offset; } }

        #region ChunkedInput implementation

        public void Close()
        {
            file.Close();
            file = null;
        }

        public IByteBuffer ReadChunk(IByteBufferAllocator allocator)
        {
            long offset = this.offset;
            if (offset >= this.endOffset)
            {
                return null;
            }

            int chunkSize = (int)Math.Min(this.chunkSize, this.endOffset - offset);
            // Check if the buffer is backed by an byte array. If so we can optimize it a bit an safe a copy

            IByteBuffer buf = allocator.Buffer();
            bool release = true;

            try
            {
                this.file.Read(buf.Array, buf.ArrayOffset, chunkSize);
                buf.SetWriterIndex(chunkSize);
                this.offset = offset + chunkSize;
                release = false;
                return buf;
            }
            finally
            {
                if(release)
                {
                    buf.Release();
                }
            }
        }

        public bool IsEndOfInput
        {
            get { return !(this.offset < this.endOffset && this.file != null); }
        }

        public long Length
        {
            get { return this.endOffset - this.startOffset; }
        }

        public long Progress
        {
            get { return this.offset - this.startOffset; }
        }

        #endregion
    }
}

