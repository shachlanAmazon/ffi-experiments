use std::ffi::CString;
use std::os::raw::c_char;

#[no_mangle]
pub extern "C" fn get_val() -> i32 {
    print!("check");
    42
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

#[no_mangle]
pub extern "C" fn concat_string(input: *mut c_char) -> *const c_char {
    unsafe {
        print!("bef");
        let cstr = CString::from_raw(input);
        print!("first");
        let str = cstr.into_string().expect("ti worked?");
        print!("sec");
        let concat = format!("{str}{str}");
        print!("3rd");
        let result = CString::new(concat).expect("second new failed");
        print!("4th");
        let p = result.as_ptr();
        std::mem::forget(str);
        std::mem::forget(result);
        p
    }
}
