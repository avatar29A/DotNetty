// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
namespace DotNetty.Handlers.Stream
{
    using System;
    using DotNetty.Buffers;

    public class ChunkedStream : ChunkedInput<IByteBuffer>
    {
        public static readonly int DefaultChunkSize = 8192;

        #region ChunkedInput implementation

        public void Close()
        {
            throw new NotImplementedException();
        }

        public IByteBuffer ReadChunk(DotNetty.Transport.Channels.IChannelHandlerContext context)
        {
            throw new NotImplementedException();
        }

        public IByteBuffer ReadChunk(IByteBufferAllocator allocator)
        {
            throw new NotImplementedException();
        }

        public bool IsEndOfInput
        {
            get
            {
                throw new NotImplementedException();
            }
            private set
            {
                throw new NotImplementedException();
            }
        }

        public long Length
        {
            get
            {
                throw new NotImplementedException();
            }
            private set
            {
                throw new NotImplementedException();
            }
        }

        public long Progress
        {
            get
            {
                throw new NotImplementedException();
            }
            private set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        public ChunkedStream()
        {
        }
    }
}

