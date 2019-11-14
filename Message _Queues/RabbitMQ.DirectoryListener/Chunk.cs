namespace RabbitMQ.DirectoryListener
{
    public class Chunk
    {
        public string FileName { get; set; }

        public long TotalSize { get; set; }

        public byte[] Data { get; set; }

        public int Size { get; set; }

        public long Offset { get; set; }
    }
}
