// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DotNetty.Handlers.Stream
{
    using System;
    using DotNetty.Transport.Channels;
    using DotNetty.Buffers;

    /// <summary>
    /// A data stream of indefinite length which is consumed by <see cref="ChunkedWriteHandler"/>.
    /// </summary>
    public interface ChunkedInput<B>
    {
        /// <summary>
        /// Return <c>true</c> if and only if there is no data left in the stream
        /// and the stream has reached at its end.
        /// </summary>
        /// <value><c>true</c> if this instance is end of input; otherwise, <c>false</c>.</value>
        bool IsEndOfInput { get; private set; }

        /// <summary>
        /// Releases the resources associated with the input.
        /// </summary>
        void Close();

        /// <summary>
        /// Obsolete Use <see cref="ReadChunk(IByteBufferAllocator)"/>.
        /// 
        /// <p>Fetches a chunked data from the stream. Once this method returns the last chunk
        /// and thus the stream has reached at its end, any subsequent <see cref="IsEndOfInput"/>
        /// call must return <c>true</c>.
        /// 
        /// <c>null</c> if there is no data left in the stream.
        /// Please note that <c>null</c> does not necessarily mean that the
        /// stream has reached at its end.  In a slow stream, the next chunk
        /// might be unavailable just momentarily.
        /// </summary>
        /// <param name="context">The context which provides a <see cref="IByteBufferAllocator"/>if buffer allocation is necessary.</param>
        /// <returns>the fetched chunk.</returns>
        [Obsolete]
        B ReadChunk(IChannelHandlerContext context);

        /// <summary>
        ///  Fetches a chunked data from the stream. Once this method returns the last chunk
        ///  and thus the stream has reached at its end, any subsequent {@link #isEndOfInput()}
        ///  call must return {@code true}.
        /// <c>null</c> if there is no data left in the stream.
        /// Please note that <c>null</c> does not necessarily mean that the
        /// stream has reached at its end.  In a slow stream, the next chunk
        /// might be unavailable just momentarily.
        /// </summary>
        /// <returns>the fetched chunk.</returns>
        /// <param name="allocator"><see cref="IByteBufferAllocator"/> if buffer allocation is necessary.</param>
        B ReadChunk(IByteBufferAllocator allocator);

        /// <summary>
        /// Returns the length of the input.
        /// </summary>
        /// <value>the length of the input if the length of the input is known.
        /// a negative value if the length of the input is unknown..
        /// </value>
        long Length { get; private set; }

        /// <summary>
        /// Returns current transfer progress.
        /// </summary>
        long Progress { get; private set; }
    }
}

