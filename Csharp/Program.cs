using System;
using System.Runtime.InteropServices;

public static class main
{
  [DllImport("rust_ffi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "get_val")]
  public static extern int GetValue();

  [DllImport("rust_ffi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "concat_string")]
  public static extern string ConcatString(string str);

  [DllImport("rust_ffi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "count_string")]
  public static extern ulong CountString(string str);

  [DllImport("rust_ffi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "get_string")]
  public static extern string GetString();

  public static void Main()
  {
    Console.WriteLine(GetString());
    Console.WriteLine(CountString("so again"));
    Console.WriteLine(GetValue());
    Console.WriteLine(ConcatString("foobar"));
    Console.WriteLine("done");
  }
}
