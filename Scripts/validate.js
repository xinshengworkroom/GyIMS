function verifyMobilePhone(phoneNo) {
    return /^1[3456789]\d{9}$/.test(phoneNo);
}

function verifyTel(telNo) {    
    return /^(\(\d{3,4}\)|\d{3,4}-|\s)?\d{7,14}$/.test(telNo);
}

function verifyMail(email) {
    var reg = /^\w+((.\w+)|(-\w+))@[A-Za-z0-9]+((.|-)[A-Za-z0-9]+).[A-Za-z0-9]+$/; 
    return reg.test(email);     
}
