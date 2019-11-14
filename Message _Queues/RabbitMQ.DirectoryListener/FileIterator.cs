using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace RabbitMQ.DirectoryListener
{
    public class FileIterator : IEnumerable<Chunk>, IEnumerable, IDisposable
    {
        private readonly string _filePath;
        private readonly int _chunkSize;
        private FileStream _fileStream;
        private bool _isDisposed;

        public FileIterator(string filePath, int chunkSize = 1024 * 4)
        {
            _filePath = filePath;
            _chunkSize = chunkSize;
        }

        public IEnumerator<Chunk> GetEnumerator()
        {
            ThrowIfDisposed();
            using (_fileStream = File.OpenRead(_filePath))
            {
                while (_fileStream.Position != _fileStream.Length)
                {
                    byte[] buffer = new byte[_chunkSize];
                    int read = _fileStream.Read(buffer, 0, _chunkSize);
                    if (read == 0) break;
                    yield return new Chunk
                    {
                        Data = buffer,
                        Size = read,
                        FileName = Path.GetFileName(_filePath),
                        Offset = _fileStream.Position,
                        TotalSize = _fileStream.Length
                    };
                    ThrowIfDisposed();
                }
            }

            _fileStream = null;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _fileStream?.Dispose();
                _fileStream = null;
            }

            _isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~FileIterator()
        {
            Dispose(false);
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(FileIterator));
            }
        }
    }
}
