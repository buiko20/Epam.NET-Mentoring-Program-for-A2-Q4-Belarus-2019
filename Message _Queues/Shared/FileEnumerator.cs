using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Shared
{
    public class FileEnumerator : IEnumerable<Chunk>, IEnumerable
    {
        private readonly string _filePath;
        private readonly int _chunkSize;

        public FileEnumerator(string filePath, int chunkSize = 1024 * 4)
        {
            _filePath = filePath;
            _chunkSize = chunkSize;
        }

        public IEnumerator<Chunk> GetEnumerator()
        {
            using (var fs = File.OpenRead(_filePath))
            {
                while (true)
                {
                    byte[] buffer = new byte[_chunkSize];
                    int read = fs.Read(buffer, 0, _chunkSize);
                    if (read == 0) break;
                    yield return new Chunk
                    {
                        Data = buffer,
                        Size = read,
                        FileName = Path.GetFileName(_filePath),
                        Offset = fs.Position,
                        TotalSize = fs.Length
                    };
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
