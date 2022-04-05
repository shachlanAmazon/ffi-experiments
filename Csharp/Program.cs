using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

[StructLayout(LayoutKind.Explicit)]
public struct CResult
{
  public enum CResultTag : byte
  {
    Ok,
    Err,
    Maybe
  }

  [FieldOffset(0)] public readonly CResultTag tag;
  [FieldOffset(8)] public readonly IntPtr value;
  [FieldOffset(8)] public readonly UIntPtr err;

  public string GetValue()
  {
    var result = Marshal.PtrToStringAnsi(value);
    Marshal.FreeCoTaskMem(value);
    return result;
  }
}

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

  [DllImport("rust_ffi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "get_string_with_result")]
  public static extern CResult GetStringWithResult(int i);


  public delegate int IntToInt(int number);
  [DllImport("rust_ffi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "call_callback")]
  public static extern int CallCallback(IntToInt fn);

  public delegate void IntAction(int number);
  [DllImport("rust_ffi", CallingConvention = CallingConvention.Cdecl, EntryPoint = "call_callback_no_return")]
  public static extern void CallCallbackNoReturn(IntAction fn);

  static int IncrementByTwo(int input)
  {
    return input + 2;
  }

  static async Task<int> ToTask()
  {
    var taskSource = new TaskCompletionSource<int>();
    CallCallbackNoReturn((res) =>
    {
      taskSource.SetResult(res);
    });
    return await taskSource.Task;
  }

  public static async Task Main()
  {
    Console.WriteLine("CallCallbackNoReturn");
    Console.WriteLine(await ToTask());
    Console.WriteLine("CallCallback");
    Console.WriteLine(CallCallback(IncrementByTwo));
    Console.WriteLine("GetStringWithResult");
    Console.WriteLine(GetStringWithResult(1).tag);
    Console.WriteLine(GetStringWithResult(1).err);
    Console.WriteLine(GetStringWithResult(0).tag);
    Console.WriteLine(GetStringWithResult(0).GetValue());
    Console.WriteLine("\nGetString");
    Console.WriteLine(GetString());
    Console.WriteLine("\nCountString");
    Console.WriteLine(CountString("so again"));
    Console.WriteLine("\nGetValue");
    Console.WriteLine(GetValue());
    Console.WriteLine("\nConcatString");
    Console.WriteLine(ConcatString("foobar"));
    Console.WriteLine("done");
  }
}
