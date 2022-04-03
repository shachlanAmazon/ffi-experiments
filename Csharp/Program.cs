using System;
using System.Runtime.InteropServices;

public static class main {
  [DllImport("rust_ffi", CallingConvention = CallingConvention.Cdecl, EntryPoint="get_val")]
  public static extern int GetValue();

  public static void Main() {
    Console.WriteLine(GetValue());
    Console.WriteLine("done");
  }
}
