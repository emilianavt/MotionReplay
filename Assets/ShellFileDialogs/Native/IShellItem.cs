﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ShellFileDialogs
{
	[ComImport]
	[Guid( ShellIIDGuid.IShellItem )]
	[InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
	internal interface IShellItem
	{
		// Not supported: IBindCtx.
		[PreserveSig]
		[MethodImpl( MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime )]
		HResult BindToHandler(
			[In] IntPtr pbc,
			[In] ref Guid bhid,
			[In] ref Guid riid,
			[Out, MarshalAs( UnmanagedType.Interface )] out IShellFolder ppv);

		[MethodImpl( MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime )]
		void GetParent([MarshalAs( UnmanagedType.Interface )] out IShellItem ppsi);

		[PreserveSig]
		[MethodImpl( MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime )]
		HResult GetDisplayName(
			[In] ShellItemDesignNameOptions sigdnName,
			out IntPtr ppszName);

		[MethodImpl( MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime )]
		void GetAttributes([In] ShellFileGetAttributesOptions sfgaoMask, out ShellFileGetAttributesOptions psfgaoAttribs);

		[PreserveSig]
		[MethodImpl( MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime )]
		HResult Compare(
			[In, MarshalAs( UnmanagedType.Interface )] IShellItem psi,
			[In] SICHINTF hint,
			out int piOrder);
	}
}
