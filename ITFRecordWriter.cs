using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFRecord.NET {
    /// <summary>
    /// Provides methods for writing data in TFRecord format for TensorFlow.
    /// </summary>
    public interface ITFRecordWriter {
        /// <summary>
        /// Writes a single data element to the underlying stream.
        /// </summary>
        /// <param name="e"></param>
        void Write(byte[] data);
    }
}
