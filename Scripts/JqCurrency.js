var DataJson = new Object();

var ajaxResult = null;
//数据初始加载
function GetModelValue(DataJson) {
    var FormId = DataJson.FormId;
    delete DataJson['FormId'];
    $(FormId).form('load', DataJson);
}
//表单提交确认
function SubmitModelValue(DataJson) {
    var FormId = DataJson.FormId;
    var Url = DataJson.Url;
    var DataType = DataJson.DataType;
    var Href = DataJson.Href;

    delete DataJson['Url'];
    delete DataJson['DataType'];
    delete DataJson['Href'];
    console.log(DataJson);
    if (FormId == null) {
        $.ajax({
            type: "POST",
            dataType: DataType,
            async: false,
            url: Url,
            data: DataJson,
            complete: function () { },
            success: function (data) {// result为返回的数据

                ajaxResult = 0;
                data = JSON.parse(data);
                if (data.Code == 1) {
                    if (Href != null) {
                        window.location.href = Href;
                    }
                }
            

            }
        })
    }
    else {
        delete DataJson['FormId'];

        $(FormId).form('submit', {
            url: Url,
            dataType: DataType,
            async: false,
            onSubmit: function (param) {
            
                DataJson
            },
            success: function (data) {
                ajaxResult = 0;
                data = JSON.parse(data);
                alert(data.Message);
                if (data.Code == 1) {
                    window.location.href = Href;
                }
            }
        });

    }
}

function GetLoadInit(DataJson)
{
    var id = DataJson.ID;
    var title = DataJson.Title;
    var url = DataJson.Url;
    var columns = DataJson.Columns;
    $.ajax({
        type: "POST",
        dataType: "json",
        async: false,
        url: url,
        data: columns,
        complete: function () { },
        success: function (result) {// result为返回的数据
            console.log(result);
            id.datagrid('loadData', JSON.parse(result));
        }
    })



}

function getstatusnobyid(value, row, index) {
    StatusJson.ID = value;
    GetTabStatusNo(StatusJson);
    return GetStausNo;
}


function trim(str) {
    if (str != null) {

        return str.replace(/^\s+|\s+$/gm, '');
    }

}

