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

using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace DS.X2Core
{

/// <summary>
/// X3 Virtual File System.
/// </summary>
public class VFS
{
	[Flags]
	public enum FileAttribute { file = 1, directory = 2 };
	
	public enum ErrorCode { msg = -1, ok = 0 };
	
	public enum LoadingBehaviour { x2, x3tc }
	
	public struct FileInfo
	{
		public FileAttribute attributes;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst=255)]
		public string fileName;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst=255)]
		public string realName;
		
		public bool isFile { get { return (attributes & FileAttribute.file) > 0; } }
		public bool isDirectory { get { return (attributes & FileAttribute.directory) > 0; } }
	}

	enum Options
	{
		loadingBehaviour = 1
	}

	const int LOADING_BEHAVIOUR_X2 = 0;
	const int LOADING_BEHAVIOUR_X3TC = 1;

	#region X3VFS imports
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x3vfs.dll")]
	private static extern ErrorCode X3VFS_GetLastError();
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x3vfs.dll")]
	private static extern int X3VFS_TranslateError(ErrorCode error, IntPtr buffer, int buffSize, ref int needed);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x3vfs.dll")]
	private static extern int X3VFS_TranslateError(ErrorCode error, System.Text.StringBuilder buffer, int buffSize, ref int needed);
	
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x3vfs.dll")]
	static extern void X3VFS_AddVFSFilter(string filter);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x3vfs.dll")]
	static extern void X3VFS_CreateVFS(string dataRoot);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x3vfs.dll")]
	static extern int X3VFS_AppendCatalog(string catalogName);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x3vfs.dll")]
	static extern int X3VFS_CatFindInit(int hCat);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x3vfs.dll")]
	static extern int X3VFS_CatFindFirstFile(int hFind, string fileName, ref FileInfo info);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x3vfs.dll")]
	static extern int X3VFS_FindFirstFile(string fileName, ref FileInfo info);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x3vfs.dll")]
	static extern int X3VFS_FindClose(int hFind);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x3vfs.dll")]
	static extern int X3VFS_FindNextFile(int hFind, ref FileInfo info);
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x3vfs.dll")]
	static extern int X3VFS_GetVFSHandle();
	[System.Security.SuppressUnmanagedCodeSecurity]
	[DllImport("x3vfs.dll")]
	static extern int X3VFS_SetOption(Options option, int value);
	#endregion
	
	/// <summary>
	/// Static class for extraction of last error code and message from X3VFS.
	/// </summary>
	public class Error
	{
		public static ErrorCode code { get { return X3VFS_GetLastError(); } }
		public static string message { get { return TranslateMessage(code); } }
		
		Error() { }
		
		/// <summary>
		/// Translates error code to a text message.
		/// </summary>
		/// <param name="error">Error code to translate.</param>
		/// <returns>Text message.</returns>
		public static string TranslateMessage(ErrorCode error)
		{
			int needed=0;
			X3VFS_TranslateError(error, IntPtr.Zero, 0, ref needed);
			System.Text.StringBuilder msg=new System.Text.StringBuilder(needed);
			X3VFS_TranslateError(error, msg, needed, ref needed);
			return msg.ToString();
		}
	}
	
	/// <summary>
	/// Exception which can translate X3VFS error code to string message.
	/// </summary>
	public class Exception : System.ApplicationException
	{
		ErrorCode m_code;
		string m_msg;
		
		public ErrorCode code { get { return m_code; }}
		
		public Exception() { m_code=Error.code; m_msg=Error.message; }
		public Exception(ErrorCode error) { m_code=error; m_msg=Error.TranslateMessage(m_code); }
		
		public override string Message { get { return m_msg; } }
	}
	
	/// <summary>
	/// Return last error code from X3VFS library.
	/// </summary>
	/// <returns></returns>
	public static ErrorCode GetLastError() { return X3VFS_GetLastError(); }
	
	/// <summary>
	/// Translates error code to a text message.
	/// </summary>
	/// <param name="error">Error code to translate.</param>
	/// <returns>Text message.</returns>
	public static string TranslateMessage(ErrorCode error)
	{
		int needed=0;
		X3VFS_TranslateError(error, IntPtr.Zero, 0, ref needed);
		System.Text.StringBuilder msg=new System.Text.StringBuilder(needed);
		X3VFS_TranslateError(error, msg, needed, ref needed);
		return msg.ToString();
	}

	/// <summary>
	/// File system which can be traversed via the Find function.
	/// </summary>
	public class FileSystem : IDisposable
	{
		/// <summary>
		/// Forward only, once only enumerator for accessing file information.
		/// </summary>
		public class SearchResult : IEnumerable, IEnumerator, IDisposable
		{
			bool m_first;
			int m_hFind;
			FileInfo m_info;
			
			public SearchResult(int hFindRoot, string fileName) 
			{ 
				m_hFind = X3VFS_CatFindFirstFile(hFindRoot, fileName, ref m_info);
				m_first = true;
			}
			
			~SearchResult() { Dispose(); }
			
			public IEnumerator GetEnumerator() { return this; }
			object IEnumerator.Current { get { return Current; } }
			public FileInfo Current { get { return m_info; } }
			public bool MoveNext() 
			{	
				bool res;
				if(m_first) {
					res=m_hFind!=0;
					m_first=false;
				}
				else
					res=X3VFS_FindNextFile(m_hFind, ref m_info)!=0;
				if(!res)
					Dispose();
				return res;
			}
			
			public void Reset() { throw new NotImplementedException("Enumerator cannot be reset"); }
			
			public void Dispose() { X3VFS_FindClose(m_hFind); m_hFind=0; GC.SuppressFinalize(this); }
		} // SearchResult
		
		protected int m_searchHandle;
		private string m_catalogName;
		
		/// <summary>Name of the file system (ctalog).</summary>
		public string name { get { return m_catalogName; } }

		/// <summary>This class cannot be created directly.</summary>
		protected FileSystem() { }
		
		/// <summary>
		/// Scans the FS for files matching the fileName and returns the result.
		/// </summary>
		/// <param name="fileName">Matching pattern, can contain Windows wildcards (*, ?).</param>
		/// <returns>SearchResult.</returns>
		public SearchResult Find(string fileName) { return new SearchResult(m_searchHandle, fileName); }

		/// <summary>Finds and opens single file in read only mode.</summary>
		/// <param name="fileName">File name to look for.</param>
		/// <returns>Opened file.</returns>
		/// <exception cref="X2FD.Exception">Thrown when error occurs.</exception>
		public X2FD.File OpenFile(string fileName) { return OpenFile(fileName, X2FD.OpenMode.read); }
		
		/// <summary>Finds and opens single file.</summary>
		/// <param name="fileName">File name to look for.</param>
		/// <param name="mode">Read/write.</param>
		/// <returns>Opened file or null when file is not found.</returns>
		/// <exception cref="X2FD.Exception">Thrown when error occurs.</exception>
		public X2FD.File OpenFile(string fileName, X2FD.OpenMode mode)
		{
			// Note to self: should this throw exception when file not found or not??
			// Exceptions should be exceptional so I will stick with NULL.
			// Tried the FileNotFoundException - didn't like it
			X2FD.File file = null;
			FileSystem.SearchResult result = Find(fileName);
			if(result.MoveNext()) {
				file = X2FD.File.Open(((VFS.FileInfo)result.Current).realName, mode, X2FD.CreateDisposition.openExisting, X2FD.FileType.auto);
				if(file == null)
					throw new X2FD.Exception();
			}
			return file;
		}
		
		/// <summary>
		/// Creates file system on top of catalog.
		/// </summary>
		/// <param name="catalog">Catalog.</param>
		public FileSystem(X2FD.Catalog catalog)
		{
			m_catalogName = catalog.fileName;
			m_searchHandle = X3VFS_CatFindInit(catalog.handle);
		}
		
		~FileSystem() { Close(); }
		
		public virtual void Close() 
		{ 
			X3VFS_FindClose(m_searchHandle); 
			m_searchHandle = 0; 
			GC.SuppressFinalize(this); 
		}

		void IDisposable.Dispose() { Close(); }
	} // FileSystem

	/// <summary>
	/// Virtual File System (X3 file system) build on top of 'dataRoot'.
	/// </summary>
	public class DirectoryFileSystem : FileSystem
	{
		static bool m_created;

		static LoadingBehaviour m_loadingBehaviour;

		public LoadingBehaviour loadingBehaviour
		{
			get { return m_loadingBehaviour; }
			set {
				X3VFS_SetOption(Options.loadingBehaviour, value == LoadingBehaviour.x3tc ? LOADING_BEHAVIOUR_X3TC : LOADING_BEHAVIOUR_X2);
				m_loadingBehaviour = value;
			}
		}
		
		/// <summary>
		/// Initializes a "virtual file system" (VFS) on top of specified dataRoot.
		/// See <see cref="AddFilter"/>.
		/// </summary>
		/// <param name="dataRoot">Directory which will be scanned.</param>
		public void Create(string dataRoot)
		{
			if(m_created) throw new ApplicationException("Only one instance of VFS can exist at time.");
			if(String.IsNullOrEmpty(dataRoot)) return;
			X3VFS_CreateVFS(dataRoot);
			m_searchHandle = X3VFS_CatFindInit(X3VFS_GetVFSHandle());
			m_created = true;
		}

		public override void Close()
		{
			m_created = false;
			base.Close();
		}
		
		/// <summary>
		/// Adds filter which will be used when Create() is called.
		/// Only files which match the filter(s) will be added to the VFS.
		/// See <see cref="Create"/>.
		/// </summary>
		/// <param name="filter">Filter.</param>
		public void AddFilter(string filter)
		{
			if(m_created)
				throw new InvalidOperationException("Cannot add filter. The VFS has been already initialized.");
			X3VFS_AddVFSFilter(filter);
		}
		
		/// <summary>
		/// Load any catalog on top of already loaded VFS - used for loading of active mods 
		/// which override everything else.
		/// </summary>
		/// <param name="fileName">catalog to load</param>
		/// <returns></returns>
		public bool AppendCatalog(string fileName)
		{
			return X3VFS_AppendCatalog(fileName) != 0;
		}

	} // DirectoryFileSystem
	
} // class X3VFS

} // ns DS.X2
