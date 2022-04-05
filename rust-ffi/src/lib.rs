use std::ffi::{CString};
use std::os::raw::c_char;

#[no_mangle]
pub extern "C" fn get_val() -> i32 {
    print!("check");
    42
}

#[no_mangle]
pub extern "C" fn call_callback(callback: unsafe extern "C" fn(i32) -> i32) -> i32 {
    unsafe { callback(1) + 1 }
}

#[no_mangle]
pub extern "C" fn call_callback_no_return(callback: unsafe extern "C" fn(i32)) {
    unsafe { callback(42); }
}

#[no_mangle]
pub extern "C" fn count_string(input: *mut c_char) -> usize {
    unsafe {
        let cstr = CString::from_raw(input);
        let vec = cstr.as_bytes();
        let length = vec.len();
        std::mem::forget(cstr);
        length
    }   
}

#[no_mangle]
pub extern "C" fn get_string() -> *const c_char {
    let str = CString::new("foobar").expect("hoo ha");
    let p = str.as_ptr();
    std::mem::forget(str);
    p
}

#[repr(C)]
pub enum CResult<T, E> {
    Ok(T),
    Err(E),
    Maybe,
}

#[no_mangle]
pub extern "C" fn get_string_with_result(should_return: i32) -> CResult<*const c_char, usize> {
    let source_str = if should_return == 1 {"get_string\0_with_result foobar" } else {"get_string_with_result foobar"};
    let str = CString::new(source_str);
    match str{
        Err(e) => CResult::Err(e.nul_position()),
        Ok(v) => {
            let p = v.as_ptr();
            std::mem::forget(v);
            CResult::Ok(p)
        }
    }
}

#[no_mangle]
pub extern "C" fn concat_string(input: *mut c_char) -> *const c_char {
    unsafe {
        let cstr = CString::from_raw(input);
        let str = cstr.into_string().expect("ti worked?");
        let concat = format!("{str}{str}");
        let result = CString::new(concat).expect("second new failed");
        let p = result.as_ptr();
        std::mem::forget(str);
        std::mem::forget(result);
        p
    }
}
