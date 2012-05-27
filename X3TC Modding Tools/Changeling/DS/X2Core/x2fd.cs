/* 
 * Copyright (c) 2009 doubleshadow
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
 
 /*
  * This implementation is now thread safe.
  * Key functions are synchronized with .NET lock {} facility.
  * This allows you to Create/Delete files from multiple threads.
  * Functions operating on handles are not synchronized. If multiple threads will 
  * read/write from/to the same File instance, behaviour is not defined.
  */
 
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DS.X2Core
{

/// <summary>
/// X2 File Driver.
/// </summary>
public static class X2FD
{
  /// <summary>Critical section.</summary>
  static readonly Object key = new Object();

	public enum OpenMode 
	{
		/// <summary>File will be opened as read-only.</summary>
		read = 0,
		/// <summary>File will be opened for read-write.</summary>
		write = 1 
	};
	
	public enum CreateDisposition 
	{
		/// <summary>If file doesn't exist, the operation will fail.</summary>
		openExisting = 0,
		/// <summary>Open existing file, or create new if it doesn't exist.</summary>
		createNew = 1 
	};
	
	public enum SeekOrigin 
	{ 
		begin = 0, 
		end = 1, 
		current = 2 
	};
	
	public enum FileType 
	{ 
		/// <summary>Automatic compression detection. Works with non-empty files.</summary>
		auto = 0,
		/// <summary>File is not compressed.</summary>
		plain = 1,
		/// <summary>File is compressed (X2, X3).</summary>
		pck = 2,
		/// <summary>File is compressed (X3TC).</summary>
		deflate = 3 
	};
	
	public enum ErrorCode 
	{
		msg = -1,
		ok = 0,
		handle = 1,
		seek = 2,
		badSeekSpec = 3,
		
		fileError = 4,
		fileAccess = 5,
		fileExists = 6,
		fileNoEntry = 7,
		
		fileBadMode = 8,
		fileBadData = 9,
		fileEmpty = 10,
		fileLocked = 11,
		
		catNotOpen = 12,
		catNoEntry = 13,
		catNoDat = 14,
		catInvalidSize = 15,
		catFilesLocked = 16,
		catInvalidFileName = 17,
		
		error = 18,
		malloc = 19,
		badFlags = 20,
		
		gzBegin = 21,
		
		gzFlags = gzBegin,
		gzHeader = gzBegin + 1,
		gzTooSmall = gzBegin + 2,
		gzCrc = gzBegin + 3,
		gzHCrc = gzBegin + 4,
		gzCompression = gzBegin + 5,
		gzDeflate = gzBegin + 6
	}
	
	[Flags]
	public enum FileFlags 
	{ 
		/// <summary>File is Win32 physical file (not in catalog).</summary>
		file = 1, 
		/// <summary>File is not compressed.</summary>
		plain = 2,
		/// <summary>File is compressed (X2, X3).</summary>
		pck = 4,
		/// <summary>File is compressed (X3TC).</summary>
		deflate = 8 
	};
	
	/// <summary>
	/// Information about file.
	/// </summary>
	public struct FileInfo
	{
		/// <summary>File name.</summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string fileName;
		/// <summary>Modification time (Unix time stamp).</summary>
    public int mtime;
		/// <summary>Logical file size (bytes).</summary>
    public uint size;
		/// <summary>Physical file size (bytes).</summary>
    public uint physicalSize;
		/// <summary>File flags.</summary>
    public FileFlags flags;

		/// <summary>Modification time in UTC.</summary>
		public DateTime GetModificationTimeUtc()
		{
			if(mtime == -1)
				return DateTime.MinValue;
			return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(mtime);
		}

		[System.Security.SuppressUnmanagedCodeSecurity]
		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		static extern int FileTimeToLocalFileTime(ref long inTime, out long outTime);

		/// <summary>Modification time in local time.</summary>
		public DateTime GetModificationTime()
		{
			long res;
			DateTime utc = GetModificationTimeUtc();
			if(utc == DateTime.MinValue) 
				return utc;
				
			long ftime = utc.ToFileTime();
			FileTimeToLocalFileTime(ref ftime, out res);

			return DateTime.FromFileTimeUtc(res);
		}
	}
	
	/// <summary>
	/// Information about file in catalog.
	/// </summary>
	public struct CatFileRecord
	{
		/// <summary>File name.</summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string fileName;
		/// <summary>Physical file size (bytes).</summary>
		public uint size;
	}
	
	#region X2FD Imports
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern ErrorCode X2FD_GetLastError();
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern int X2FD_TranslateError(ErrorCode error, IntPtr buffer, int buffSize, ref int needed);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern int X2FD_TranslateError(ErrorCode error, System.Text.StringBuilder buffer, int buffSize, ref int needed);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern int X2FD_OpenFile(string pszName, OpenMode mode, CreateDisposition disposition, FileType type);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern int X2FD_OpenFileConvert(string pszName, CreateDisposition disposition, FileType fromType, FileType toType);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern int X2FD_CloseFile(int hFile);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern int X2FD_FileSize(int hFile);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern int X2FD_EOF(int hFile);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern uint X2FD_ReadFile(int hFile, byte[] buffer, uint size);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern int X2FD_SeekFile(int hFile, int offset, SeekOrigin origin);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern uint X2FD_FileTell(int hFile);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern int X2FD_SetEndOfFile(int hFile);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern uint X2FD_WriteFile(int hFile, byte[] buffer, uint size);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern uint X2FD_WriteFile(int hFile, IntPtr buffer, uint size);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern int X2FD_FileStat(string fileName, ref FileInfo info);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern bool X2FD_FileStatByHandle(int hFile, ref FileInfo info);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern bool X2FD_FileExists(string fileName);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern int X2FD_GetFileCompressionType(string fileName);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern bool X2FD_DeleteFile(string fileName);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern bool X2FD_MoveFile(string fileName, string newName);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern int X2FD_OpenCatalog(string fileName, CreateDisposition disposition);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern bool X2FD_CloseCatalog(int hCat);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern bool X2FD_DeleteCatalog(string fileName);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern int X2FD_SetFileTime(int hFile, int mtime);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern int X2FD_CatFindFirstFile(int hCat, string fileName, ref CatFileRecord info);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern int X2FD_CatFindNextFile(int hFind, ref CatFileRecord info);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern bool X2FD_CatFindClose(int hFind);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x2fd.dll")]
	private static extern bool X2FD_CopyFile(int hSource, int hDest);
	#endregion
	
	/// <summary>
	/// Static class for extraction of last error code and message from X2FD.
	/// </summary>
	public static class Error
	{
		public static ErrorCode code { get { return X2FD_GetLastError(); } }
		public static string message { get { return TranslateMessage(code); } }
		
		/// <summary>
		/// Translates error code to a text message.
		/// </summary>
		/// <param name="error">Error code to translate.</param>
		/// <returns>Text message.</returns>
		public static string TranslateMessage(ErrorCode error)
		{
			int needed = 0;
			X2FD_TranslateError(error, IntPtr.Zero, 0, ref needed);
			System.Text.StringBuilder msg = new System.Text.StringBuilder(needed);
			X2FD_TranslateError(error, msg, needed, ref needed);
			return msg.ToString();
		}
	}
	
	/// <summary>
	/// Exception which can translate X2FD error code to string message.
	/// </summary>
	public class Exception : System.ApplicationException
	{
		ErrorCode m_code;
		string m_msg;
		
		public ErrorCode code { get { return m_code; } }

		public Exception() { m_code = Error.code; m_msg = Error.message; }
		public Exception(ErrorCode error) { m_code = error; m_msg = Error.TranslateMessage(m_code); }
		public Exception(ErrorCode error, string message) { m_code = error; m_msg = message; }
		
		public override string Message { get { return m_msg; } }

	} // class Exception
	
	/// <summary>
	/// Class representing a file.
	/// </summary>
	public class File : System.IO.Stream, IDisposable
	{
		bool m_writeable;
		
		/// <summary>Internal handle.</summary>
		public int handle { get; private set; }
		
		/// <summary>Name of the file.</summary>
		public string fileName { get; private set; }
		
		File(int handle) { this.handle = handle; }
		~File() { Close(); }
		
		/// <summary>
		/// Open or creates a file.
		/// </summary>
		/// <param name="fileName">Name of the file to open/create.</param>
		/// <param name="mode">Specifies file access: read only or read/write.</param>
		/// <param name="disposition">If new file should be created in case it doesn't exist.</param>
		/// <param name="type">How to handle the file data: 'auto' only works when opening an existing file.</param>
		/// <returns>File.</returns>
		public static File Open(string fileName, OpenMode mode, CreateDisposition disposition, FileType type)
		{
		  lock(key){
			  // x2fd itself will return a bit cryptic message fileLocked, so I'll handle it better here
			  if(String.IsNullOrEmpty(fileName)) 
				  throw new Exception(ErrorCode.fileNoEntry);
			  int handle = X2FD_OpenFile(fileName, mode, disposition, type);
			  if(handle == 0)
          throw new Exception();
          
			  File f = new File(handle);
			  f.m_writeable = (mode & OpenMode.write) > 0;
			  f.fileName = fileName;
			  return f;
			}
		}
		/// <summary>Opens file for reading with type=auto.</summary>
		/// <param name="fileName">name of the file to open.</param>
		public static File Open(string fileName)
		{
			return Open(fileName, OpenMode.read, CreateDisposition.openExisting, FileType.auto);
		}

		/// <summary>
		/// Opens/creates file for writing and converts to specified type. File is opened in 'auto' mode.
		/// </summary>
		/// <param name="fileName">Name of the file to open/create.</param>
		/// <param name="disposition">If new file should be created in case it doesn't exist.</param>
		/// <param name="toType">Type to which the data will be converted to.</param>
		/// <returns>File.</returns>
		public static File OpenConvert(string fileName, CreateDisposition disposition, FileType toType)
		{
			return OpenConvert(fileName, disposition, FileType.auto, toType);
		}

		/// <summary>
		/// Opens/creates file for writing and converts to specified type.
		/// </summary>
		/// <param name="fileName">Name of the file to open/create.</param>
		/// <param name="disposition">If new file should be created in case it doesn't exist.</param>
		/// <param name="fromType">How to handle the file data: 'auto' only works when opening an existing file.</param>
		/// <param name="toType">Type to which the data will be converted to.</param>
		/// <returns>File.</returns>
		public static File OpenConvert(string fileName, CreateDisposition disposition, FileType fromType, FileType toType)
		{
		  lock(key){
			  // x2fd itself will return a bit cryptic message fileLocked, so I'll handle it better here
			  if(String.IsNullOrEmpty(fileName))
				  throw new Exception(ErrorCode.fileNoEntry);
			  int handle = X2FD_OpenFileConvert(fileName, disposition, fromType, toType);
			  if(handle == 0)
			    throw new Exception();
  			  
			  File f = new File(handle);
			  f.m_writeable = true;
			  f.fileName = fileName;
			  return f;
			}
		}
		
		/// <summary>
		/// Checks if the file exists.
		/// </summary>
		/// <param name="fileName">File name to check.</param>
		/// <returns>True if the file exists.</returns>
		public static bool Exists(string fileName)
		{
		  lock(key)
			  return X2FD_FileExists(fileName);
		}
		
		void IDisposable.Dispose() { Close(); }
		
		/// <summary>Closes the file.</summary>
		public override void Close() 
		{ 
		  lock(key)
			  X2FD_CloseFile(handle); 
			  
			handle = 0;
			GC.SuppressFinalize(this); 
		}
		
		/// <summary>
		/// returns information about current file.
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public FileInfo GetInfo()
		{
			FileInfo info = new FileInfo();
			if(!X2FD_FileStatByHandle(handle, ref info))
				throw new Exception();
			return info;
		}
		
		/// <summary>
		/// Returns information about a file.
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="info"></param>
		/// <returns></returns>
		public static bool GetInfo(string fileName, out FileInfo info)
		{
		  lock(key){
			  info = new FileInfo();
			  return X2FD_FileStat(fileName, ref info) != 0;
			}
		}
		
		/// <summary>
		/// Deletes file from either catalog or real file system.
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static void Delete(string fileName)
		{
		  lock(key){
			  if(!X2FD_DeleteFile(fileName))
			    throw new Exception();
			}
		}
		
		/// <summary>
		/// Returns size of file.
		/// </summary>
		/// <returns></returns>
		long GetSize()
		{
			long old = Position;
			Seek(0, System.IO.SeekOrigin.End);
			long size = Position;
			Seek(old, System.IO.SeekOrigin.Begin);			
			return size;
		}
		
		/// <summary>
		/// Read whole content of file.
		/// </summary>
		/// <param name="data">buffer</param>
		/// <returns>true if all data could be read</returns>
		public bool ReadToEnd(out byte[] data)
		{
			long size = GetSize() - Position;
			data = new byte[size];
			bool res = (Read(data, 0, (int)size) == size);
			return res;
		}
		
		/************ System.IO.Stream stuff ************/
		
		public override bool CanRead { get { return handle != 0; } }
		public override bool CanWrite { get { return CanRead && m_writeable; } }
		public override bool CanSeek { get { return CanRead; } }
		public override long Length { get { return GetSize(); } }

		public override long Position
		{
			get { return X2FD_FileTell(handle); }
			set { Seek(value, System.IO.SeekOrigin.Begin); }
		}

		/// <summary>
		/// Read up to size bytes from current position in file.
		/// </summary>
		/// <param name="buffer">Buffer which will receive the data.</param>
		/// <param name="offset">Offset at which to start writing data into the buffer - must be zero.</param>
		/// <param name="count">Number of bytes to read.</param>
		/// <returns>Number of bytes sucessfully read.</returns>
		public override int Read(byte[] buffer, int offset, int count)
		{
      if(offset < 0) throw new ArgumentException("Offset cannot be less then 0.");
      if(count < 0) throw new ArgumentException("Count cannot be less then 0.");
			if(offset > 0) throw new NotSupportedException("Offset > 0 not supported.");
			
			int res = (int)X2FD_ReadFile(handle, buffer, (uint)count);
			if(res == -1) throw new Exception();
			return (int)res;
		}

		public override long Seek(long offset, System.IO.SeekOrigin origin)
		{
			SeekOrigin o;
			switch(origin){
				default:
				case System.IO.SeekOrigin.Begin:
					o = SeekOrigin.begin;
					break;
				case System.IO.SeekOrigin.Current:
					o = SeekOrigin.current;
					break;
				case System.IO.SeekOrigin.End:
					o = SeekOrigin.end;
					break;
			}
			
			return (long)X2FD_SeekFile(handle, (int)offset, o);
		}

		public override void SetLength(long value)
		{
			if(!CanWrite) throw new InvalidOperationException("Stream is readonly.");
			long old = Position;
			Position = value;
			X2FD_SetEndOfFile(handle);
			Position = old;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if(!CanWrite) throw new InvalidOperationException("Stream is readonly.");
			if(offset < 0)
			  throw new ArgumentException("Offset cannot be less than 0.");
      if(count < 0)
        throw new ArgumentException("Count cannot be less than 0.");
      if(offset + count > buffer.Length)
        throw new ArgumentException("Offset + Count overrun the specified buffer size.");
			
			uint res;
			if(offset > 0){
			  GCHandle h = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, offset);
        res = X2FD_WriteFile(handle, ptr, (uint)count);
        h.Free();
			}
			else
        res = X2FD_WriteFile(handle, buffer, (uint)count);
			if((int)res == -1) throw new Exception();
		}

		public override void WriteByte(byte value)
		{
			if(!CanWrite) throw new InvalidOperationException("Stream is readonly.");
			
			byte[] bt = new byte[] { value };
			Write(bt, 0, 1);
		}
		
		public override void Flush() { /* nothing */ }

		/// <summary>Returns compression type of an existing file.</summary>
		/// <param name="fileName">File to check.</param>
		/// <returns>One of <see cref="FileType"/> enumerations or FileType.auto on error.</returns>
		public static FileType GetCompression(string fileName)
		{
			FileType type = FileType.auto;
			FileInfo info;
			if(GetInfo(fileName, out info)) {
				if((info.flags & FileFlags.pck) > 0)
					type = FileType.pck;
				else if((info.flags & FileFlags.deflate) > 0)
					type = FileType.deflate;
				else if((info.flags & FileFlags.plain) > 0)
					type = FileType.plain;
			}
			return type;
		}

    public static FileType GetCompression(System.IO.Stream stream)
    {
      if(!stream.CanSeek)
        throw new InvalidOperationException("Cannot determine compression type: the stream does not support seeking.");
        
      long orig = stream.Position;
      
      stream.Position = 0;
      
      
      byte[] buffer = new byte[3];
      int size = stream.Read(buffer, 0, buffer.Length);
      FileType type = GetCompression(buffer, size);
      
      stream.Position = orig;
      
      return type;
    }

    static FileType GetCompression(byte[] buffer, int length)
    {
	    FileType type = FileType.auto;
	    
	    if(length >= 3){
		    byte magic = (byte)(buffer[0] ^ 0xC8);
        if((buffer[1] ^ magic) == 0x1F && (buffer[2] ^ magic) == 0x8B) 
			    type = FileType.pck;
	    }
	    if(type == FileType.auto){
        if(length >= 2 && (buffer[0] == 0x1F && buffer[1] == 0x8B))
			    type = FileType.deflate;
		    else
			    type = FileType.plain;
	    }	
	    return type;
    }

    public static void Rename(string oldName, string newName)
    {
      if(!X2FD_MoveFile(oldName, newName))
        throw new Exception();
    }

		public static void Copy(System.IO.Stream src, File dest)
		{
			if(src == null || dest == null) 
				throw new ArgumentNullException();
			
			if(src is File){
				if(!X2FD_CopyFile(((File)src).handle, dest.handle))
					throw new Exception();
			}
			else {
				byte[] data = new byte[src.Length];
				src.Position = 0;
				src.Read(data, 0, data.Length);
				dest.Position = 0;
				dest.Write(data, 0, data.Length);
				dest.SetLength(dest.Position);
			}
		}
	} // class File
	
	/// <summary>
	/// Class representing a catalog.
	/// </summary>
	public class Catalog : IDisposable
	{
		/// <summary>
		/// Formward only, once only iterator used to extract basic file informations.
		/// </summary>
		public class SearchResult : IEnumerable<CatFileRecord>, IEnumerator<CatFileRecord>, IDisposable
		{
			int m_hFind;
			CatFileRecord m_info;
			bool m_first;
			
			public SearchResult(int hCat, string fileName) 
			{ 
				m_hFind=X2FD_CatFindFirstFile(hCat, fileName, ref m_info);
				m_first=true;
			}
			
			~SearchResult() { Dispose(); }
			
			public IEnumerator<CatFileRecord> GetEnumerator() { return this; }
			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return GetEnumerator(); }

			public CatFileRecord Current { get { return m_info; } }
			object System.Collections.IEnumerator.Current { get { return Current; } }
			
			public bool MoveNext() 
			{	
				bool res;
				if(m_first) {
					res = m_hFind != 0;
					m_first = false;
				}
				else
					res = X2FD_CatFindNextFile(m_hFind, ref m_info) != 0;
				if(!res)
					Dispose();
				return res;
			}
			
			public void Reset() { throw new NotImplementedException("Enumerator cannot be reset."); }
			
			public void Dispose() { X2FD_CatFindClose(m_hFind); m_hFind = 0; GC.SuppressFinalize(this); }
		}
		
		/// <summary>Internal handle.</summary>
		public int handle { get; private set; }
		/// <summary>Name of the catalog (of the .cat file).</summary>
		public string fileName { get; private set; }
		
		Catalog(string fileName, int handle) { this.fileName = fileName; this.handle = handle; }
		~Catalog() { Close(); }
		
		/// <summary>Opens or creates a catalog.</summary>
		/// <param name="fileName">Name of catalog to open (the .cat file).</param>
		/// <param name="disposition">If new catalog should be created in case it does not exist.</param>
		/// <returns>Catalog.</returns>
		public static Catalog Open(string fileName, CreateDisposition disposition)
		{
			int handle;
		  lock(key)
			  handle = X2FD_OpenCatalog(fileName, disposition);
		  if(handle == 0) 
			  throw new Exception();
		  return new Catalog(fileName, handle);
		}
		
		void IDisposable.Dispose() { Close(); }
		
		/// <summary>Closes the catalog.</summary>
		public void Close() 
		{ 
		  lock(key)
		    X2FD_CloseCatalog(handle); 
		  handle = 0; 
		  GC.SuppressFinalize(this); 
		}
		
		/// <summary>Search for files matching the fileName.</summary>
		/// <param name="fileName">File name - can contain Windows wildcards (*, ?).</param>
		/// <returns>Found files.</returns>
		public SearchResult Find(string fileName)
		{
			return new SearchResult(handle, fileName);
		}
		
		/// <summary>
		/// Opens file inside the catalog.
		/// This function is shortcut for calling File.Open(catalogName + "::" + fileName).
		/// </summary>
		/// <param name="fileName">Name file to open (without the catalog part).</param>
		/// <param name="mode">Specifies file access: read only or read/write.</param>
		/// <param name="disposition">If new file should be created if it doesn't exist.</param>
		/// <param name="type">How to handle the file: 'auto' only works for existing files.</param>
		/// <returns>File.</returns>
		public File OpenFile(string fileName, OpenMode mode, CreateDisposition disposition, FileType type)
		{
			return File.Open(this.fileName + "::" + fileName, mode, disposition, type);
		}
		
		/// <summary>
		/// Deletes the catalog - both the .cat and .dat files.
		/// </summary>
		/// <param name="catalogName">Name of the catalog.</param>
		/// <returns>True or false.</returns>
		public static void Delete(string catalogName)
		{
			if(!X2FD_DeleteCatalog(catalogName))
			  throw new Exception();
		}
	} // class Catalog

	public static class Path
	{
		/// <summary>Returns catalog name part and file name part from full file name.</summary>
		/// <param name="fileName">Full file name.</param>
		/// <param name="catPart">Catalog file name if fileName is in X2FD format or null.</param>
		/// <param name="filePart">File part if fileName is in X2FD format or just fileName.</param>
		public static void Parse(string fileName, out string catPart, out string filePart)
		{
			if(fileName == null) 
				throw new ArgumentNullException("shortFileName");
				
			int pos = fileName.IndexOf("::");
			if(pos == -1) {
			  catPart = null;
			  filePart = fileName;
			}
			else {
			  catPart = fileName.Substring(0, pos);
			  filePart = fileName.Substring(Math.Min(fileName.Length, pos + 2));
			}
		}

		public static string Create(string catalogName, string fileName)
		{
			return catalogName + "::" + fileName;
		}

		/// <summary>
		/// Returns file name with stripped <c>root</c> prefix and extension.
		/// </summary>
		/// <param name="fileName">Absolute file name.</param>
		/// <param name="root">Root of VFS.</param>
		/// <returns>File name.</returns>
		public static string GetVfsRelativePath(string fileName, string root)
		{
			if(fileName == null) return null;
			
			int idx = fileName.IndexOf("::");
			if(idx >= 0)
				fileName = fileName.Substring(idx + 2);
			else{
				if(String.Compare(fileName, 0, root, 0, root.Length, true) == 0){
					fileName = fileName.Substring(root.Length);
				}
				else
					fileName = null;
			}
			if(fileName != null){
				string ext = System.IO.Path.GetExtension(fileName);
				fileName = fileName.Substring(0, fileName.Length - ext.Length);
			}
			return fileName;
		}
	} // Path
	
} // X2FD

} // ns DS.X2
