# TFRecord.NET
## Overview
A .NET library for creating TensorFlow TFRecord files.

## Description
The TensorFlow (www.tensorfow.org) uses an optional ".tfrecord" file format for more efficient data processing. Currently support for writing .tfrecord files seems to be limited to the TensorFlow Python libraries, and to a couple of Java-based Hadoop adapters. For more details on .tfrecord files, please see: https://www.tensorflow.org/tutorials/load_data/tf_records

This library is a .NET port of the TFRecordWriter.java class that is part of the TensorFlow project in:
https://github.com/tensorflow/ecosystem/tree/master/hadoop/src/main/java/org/tensorflow/hadoop/util

For compatibility with future .tfrecord versions, this library doesn't directly reference the "Example" protocol buffer. Instead, it relies on the user to convert each example to a byte array before writing, which can be trivially done. Please see the code example below.

##Dependencies
This library uses the Crc32C.NET library hosted on NuGet at: https://www.nuget.org/packages/Crc32C.NET/

##Building
Open the .csproj file in Visual Studio 2017 or later. Dependencies should be automatically downloaded when building.

## Example
	using Google.Protobuf;
	using Tensorflow;
	using TFRecord.NET;

	// Create a simple "Example" protocol buffer entry
    var example = new Example();
    example.Features = new Features();
    var f1 = new Feature {
        Int64List = new Int64List()
    };
    f1.Int64List.Value.Add(0);
    example.Features.Feature.Add("test", f1);
	
	// Write the example to a .tfrecord file.
    using (var outFile = File.Create("demo.tfrecord")) {
        var writer = new TFRecordWriter(outFile);
        writer.Write(example.ToByteArray());
    }