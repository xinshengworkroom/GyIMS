var downloadFile = function (url, data) {
    var inputs = '';
    for (var i in data) {
        inputs += '<input type="hidden" name="' + i + '" value="' + data[i] + '" />';
    }
    $('<form action="' + url + '" method="post">' + inputs + '</form>').appendTo('body').submit().remove();
};

var openNewWindow = function (url, name, iWidth, iHeight) {
    var url;                            //转向网页的地址;
    var name;                           //网页名称，可为空;
    var iWidth;                         //弹出窗口的宽度;
    var iHeight;                        //弹出窗口的高度;
    //window.screen.height获得屏幕的高，window.screen.width获得屏幕的宽
    var iTop = (window.screen.height - iHeight) / 2;       //获得窗口的垂直位置;
    var iLeft = (window.screen.width - 10 - iWidth) / 2;        //获得窗口的水平位置;
    window.open(url, name, 'height=' + iHeight + ',,innerHeight=' + iHeight + ',width=' + iWidth + ',innerWidth=' + iWidth + ',top=' + iTop + ',left=' + iLeft + ',toolbar=no,menubar=no,scrollbars=auto,resizeable=no,location=no,status=no');
    //window.open(url, "newwindow","height=400, width=600,top=200,left=500,toolbar=no, menubar=no, scrollbars=no,resizable=no, location=no,status=no");
}



function fileUpload(file, params, url, successfn, errorfn) {
    params["ts"] = new Date().getTime();
    //var hidden = $("#form_body").find("input[type='hidden']");
    //var idValue = hidden.val();
    var files = [];
    $.each(file, function (i, item) {
        if ($(item).val() != "")
            files.push($(item).attr("id"));
    });

    $.ajaxFileUpload({
        url: url,
        type: 'Post',
       // contentType: "application/json",
        data: params,
        secureuri: false,
        fileElementId: files,
        dataType: 'json',
        success: function (data, status) {
            if (typeof successfn === "function") {            
                successfn(data, status);
            }
        },
        error: function (err, r) {
            if (errorfn === "function") {
                errorfn(err);
            }
        }
    });
}