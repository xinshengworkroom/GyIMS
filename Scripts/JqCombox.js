
var StatusJson = new Object();
var GetStausID = null;
var GetStausNo = null;

function GetStatusType(StatusJson) {
    var Id = StatusJson.ID;
    $.ajax({
        type: "POST",
        dataType: "json",
        async: false,
        url: '/StatusInfoes/GetComboxTypes',
        data: StatusJson,
        complete: function () { },
        success: function (result) {// result为返回的数据
            $(Id).combobox({
                data: result,
                textField: 'StatusNo',
                valueField: 'ID',
                panelHeight: 'auto',
            });
        }
    })
}

function GetTabStatusID(StatusJson) {
    $.ajax({
        type: "POST",
        dataType: "json",
        async: false,
        url: '/StatusInfoes/GetTabStatusID',
        data: StatusJson ,
        complete: function () { },
        success: function (result) {// result为返回的数据
            GetStausID = result.err;
            return result; 
        }
    })
}

function GetTabStatusNo(StatusJson) {
    $.ajax({
        type: "POST",
        dataType: "json",
        async: false,
        url: '/StatusInfoes/GetTabStatusNo',
        data: StatusJson,
        complete: function () { },
        success: function (result) {// result为返回的数据
            GetStausNo = result.err;
            return result;
        }
    })
}

function getstatusnobyid(value, row, index) {
    StatusJson.ID = value;
    GetTabStatusNo(StatusJson);
    return GetStausNo;
}

function getdate(value, row, index) {
    return new Date(parseInt(value.substr(6, 13))).toLocaleDateString();
}

function gettime(date) {
    var pa = /.*\((.*)\)/;
    var unixtime = date.match(pa)[1].substring(0, 10);
    return getTime(unixtime);
}

function GetDateFormat(str) {
    return new Date(parseInt(str.substr(6, 13))).toLocaleDateString();
}

function getTime(/** timestamp=0 **/) {
    var ts = arguments[0] || 0;
    var t, y, m, d, h, i, s;
    t = ts ? new Date(ts * 1000) : new Date();
    y = t.getFullYear();
    m = t.getMonth() + 1;
    d = t.getDate();
    h = t.getHours();
    i = t.getMinutes();
    s = t.getSeconds();
    // 可根据需要在这里定义时间格式  
    return y + '-' + (m < 10 ? '0' + m : m) + '-' + (d < 10 ? '0' + d : d) + ' ' + (h < 10 ? '0' + h : h) + ':' + (i < 10 ? '0' + i : i) + ':' + (s < 10 ? '0' + s : s);
}