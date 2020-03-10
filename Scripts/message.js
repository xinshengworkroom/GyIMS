$(function () {
    msgInterval = window.setInterval(showMessage, 1000);
});
function showMessage() {
    var param = {
        rows: 10,
        page: 1,
        name: "",
        ts: Math.random()
    };  
    if ($('#messagebox').length == 0) {
        $.ajax({
            url: "/Messages/GetMessageList",
            type: "get",
            data: param,
            async: false,
            success: function (data) {
                if (data) {
                    if ($('#messagebox').length == 0) {
                        var detail = "";
                        if (data.rows.length > 0) {
                            $.each(data.rows, function (i, o) {
                                if (window.location.href.indexOf("/Messages/Read/" + o.ID) < 0
                                    && window.location.href.indexOf("/Messages/Details/" + o.ID) < 0
                                ) {
                                    detail += "<font  color='green'>" + o.ID + " " + o.BizTypeName + " " + o.BizName + " </font > <a href='/Messages/Read/" + o.ID + "'>查看</a> <br />";
                                }
                            });
                        }
                        if (detail != "") {
                            $.messager.show({
                                id: 'messagebox',
                                title: '新消息提示',
                                msg: '您有'
                                    + '<font size="4" color="red">'
                                    + data.total.length
                                    + '</font>'
                                    + '条未读消息！<br/>'
                                    + detail,
                                //+ '点击确认阅读 <a style="color:blue;size:3" href="/Messages/Index" >确认</a>',
                                timeout: 20000,
                                showType: 'slide'
                            });
                        }
                    }
                }
            }
        });
    }
}

function closeMessagebox() {
    $('#messagebox').window('close');
    if (msgInterval) {
        window.clearInterval(msgInterval);
    }
}