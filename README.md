# IsomCoreMetadata
ISO Base Media File Format is a base specification, derivatives of which include MPEG-4 (.mp4), QuickTime (.mov), AAC audio (.m4a), and many others. IsomCoreMetadata is a class for retrieving and setting "core metadata" properties of ISOM format files.
 
The software is distributed in C# source code as a [CodeBit](http://FileMeta.org/CodeBit.html) located [here](https://github.com/FileMeta/IsomCoreMetadata/raw/master/IsomCoreMetadata.cs). It is released under an [Unlicense](http://unlicense.org) public domain dedication.

IsomCoreMetadata is part of the [FileMeta](http://www.filemeta.org) initiative because it provides convenient access to metadata on a particular file format.

This project includes master copy of the IsomCoreMetadata.cs CodeBit plus a set of unit tests which may also serve as sample code. 

IsomCoreMetadata does not depend on any other CodeBits. The unit test project depends on the following CodeBit:
* [ConsoleHelper](https://github.com/FileMeta/ConsoleHelper)

The "core metadata" supported are those stored in the intrinsic structures of the file format. The broader set of metadata fields supported by formats like .MP4 and .MOV may be accessed through the Windows Property System using the [Windows Shell Property Store](https://github.com/FileMeta/WinShellPropertyStore) CodeBit. The fields supported in this class are NOT accessible through the Windows Property System.

## Supported Properties
Access to following properties is supported by this class:
 * MajorBrand (Read Only)
 * MinorVersion (Read Only)
 * CompatibleBrands (Read Only)
 * Duration (Read Only)
 * CreationTime (Read/Write)
 * ModificationTime (Read/Write)

## About CodeBits
A [CodeBit](http://FileMeta.org/CodeBit.html) is a way to share common code that's lighter weight than NuGet. Each CodeBit consists of a single source code file. A structured comment at the beginning of the file indicates where to find the master copy so that automated tools can retrieve and update CodeBits to the latest version.