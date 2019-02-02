using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFRecord.NET
{
    // Adapted from the Java implementation in:
    // https://github.com/tensorflow/ecosystem/blob/master/hadoop/src/main/java/org/tensorflow/hadoop/util/TFRecordWriter.java
    public class TFRecordWriter : ITFRecordWriter {
        private const uint MASK_DELTA = 0xa282ead8;
        private readonly Stream _stream;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="s">Stream to write TFRecord entries to</param>
        public TFRecordWriter(Stream s) {
            _stream = s;
        }

        public void Write(byte[] data) {
            // tfrecord format:
            //
            // uint64 length
            // uint32 masked_crc32_of_length
            // byte data[length]
            // uint32 masked_crc32_of_data
            byte[] len = toUInt64LittleEndian((ulong)data.Length);
            _stream.Write(len, 0, len.Length);
            byte[] lenCRC = toUInt32LittleEndian(maskedCRC32c(len));
            _stream.Write(lenCRC, 0, lenCRC.Length);
            _stream.Write(data, 0, data.Length);
            byte[] dataCRC = toUInt32LittleEndian(maskedCRC32c(data));
            _stream.Write(dataCRC, 0, dataCRC.Length);
        }

        private uint maskedCRC32c(byte[] data) {
            uint crc = Crc32C.Crc32CAlgorithm.Compute(data);
            return ((crc >> 15) | (crc << 17)) + MASK_DELTA;
        }

        // Converts an integer to a little endian byte array. Based partly on this Stack Overflow answer:
        // https://stackoverflow.com/a/2350112/353308
        private byte[] toUInt64LittleEndian(ulong length) {
            byte[] bytes = new byte[sizeof(ulong)];
            for(int i = 0; i < bytes.Length; ++i) {
                bytes[i] = (byte)((length >> (8 * i)) & 0xFF);
            }
            return bytes;
        }

        private byte[] toUInt32LittleEndian(uint length) {
            byte[] bytes = new byte[sizeof(uint)];
            for (int i = 0; i < bytes.Length; ++i) {
                bytes[i] = (byte)(((uint)length >> (8 * i)) & 0xFF);
            }
            return bytes;
        }
    }
}
