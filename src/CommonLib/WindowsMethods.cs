#if false
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
	public static class WindowsMethods
	{
		[DllImport("user32.dll")]
		private static extern System.IntPtr GetActiveWindow();
		public static System.IntPtr GetWindowHandle()
		{
			return GetActiveWindow();
		}

		[Flags]
		private enum SetWindowPosFlags : uint
		{
			SynchronousWindowPosition = 0x4000,
			DeferErase = 0x2000,
			DrawFrame = 0x0020,
			FrameChanged = 0x0020,
			HideWindow = 0x0080,
			DoNotActivate = 0x0010,
			DoNotCopyBits = 0x0100,
			IgnoreMove = 0x0002,
			DoNotChangeOwnerZOrder = 0x0200,
			DoNotRedraw = 0x0008,
			DoNotReposition = 0x0200,
			DoNotSendChangingEvent = 0x0400,
			IgnoreResize = 0x0001,
			IgnoreZOrder = 0x0004,
			ShowWindow = 0x0040,
		}

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);
		public static bool MoveWindow(IntPtr hWnd, int x, int y, int w, int h)
		{
			return SetWindowPos(hWnd, IntPtr.Zero, x, y, w, h, SetWindowPosFlags.ShowWindow);
		}
	}
}
#endif
