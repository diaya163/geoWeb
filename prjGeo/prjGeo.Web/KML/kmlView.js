

function InitGridSet(json) {
    if (json == undefined) return;

    var th = '';
    $.each(json.columns, function (colIndex, col) {
        if (col.Title != undefined) {
            th += '<th style="font-size:12px; line-height:21.3px">' + col.Title + '</th>';
        }
    });

    $("#myTb").append("<tr>" + th + "</tr>");
    //行遍历
    $('#myTb').append('<tbody id="lstCols">')
    $.each(json.rows, function (rowIndex, row) {
        var tr = '';

        //列遍历
        if (row.fldType = 'CHK') {
            tr += '<td><input type="checkbox" id="chksel" /></td>';
        }
        if (row.Title != undefined) {
            tr += '<td align="left" style="font-size:10px; line-height:21.3px">' + row.Title + '</td>';
        }
        else {
            tr += '<td align="center" style="font-size:10px; line-height:21.3px"></td>';
        }
        if (row.fldName != undefined) {
            tr += '<td align="left" style="font-size:10px; line-height:21.3px">' + row.fldName + '</td>';
        }
        else {
            tr += '<td align="center" style="font-size:10px; line-height:21.3px"></td>';
        }

        $("#myTb").append('<tr>' + tr + '</tr>');
    });
    $('#myTb').append('</tbody>');

    //$("#myTb").append('</tbody>');
    //$("#myTb").append('<tr>');
    //$("#myTb").append('<td></td>');
    //$("#myTb").append('<td><input type="button" value="确定" id="btnOk" /></td>');
    //$("#myTb").append('<td></td>');
    //$("#myTb").append('</tr>');
}
//隐藏某列的方法：     $('#tt').datagrid('hideColumn', 'XXX');  -----其中 XXX 是隐藏列的  field 属性值
//展示某列的方法：     $('#tt').datagrid('showColumn', 'XXX');  -----其中 XXX 是隐藏列的  field 属性值
function SetGridStyle(grid) {
    var mytable = document.getElementById('myTb');
    var data = [];
    var cols = [];
    var strCol = "";
    debugger
    for (var i = 1, rows = mytable.rows.length; i < rows; i++) {
        for (var j = 0, cells = mytable.rows[i].cells.length; j < cells; j++) {
            if (!data[i]) {
                data[i] = new Array();
            }
            if (j == 0) {
                if (document.getElementById('chksel').checked) {
                    strCol = mytable.rows[i].cells[2].innerHTML;
                    $("'#mKmlGrid").datagrid('hideColumn', '' + strCol);
                }
                else {
                    data[i][j] = 0;
                }
            }
            else {
                data[i][j] = mytable.rows[i].cells[j].innerHTML;
            }
        }
    }
    return data;
}
var action = '', id = -1;
var o = null;

function save() {
    var ProvinceName = $("#txtProvinceName").textbox('getValue');
    if (ProvinceName == '') {
        $.messager.alert('系统提示', '省不能为空!');
        return;
    }
    var CityName = $("#txtCityName").textbox('getValue');
    if (CityName == '') {
        $.messager.alert('系统提示', '市不能为空!');
        return;
    }

    var CountyName = $("#txtCountyName").textbox('getValue');
    if (CountyName == '') {
        $.messager.alert('系统提示', '市不能为空!');
        return;
    }
    var LayerName = $("#txtLayerName").textbox('getValue');
    if (LayerName == '') {
        $.messager.alert('系统提示', '图层名不能为空!');
        return;
    }
    var FileName = $("#txtFileName").textbox('getValue');
    if (FileName == '') {
        $.messager.alert('系统提示', '请选择上传的KML文件!');
        return;
    }



    o = {
        ProvinceName: ProvinceName
       , CityName: CityName
       , CountyName: CountyName
       , LayerName: LayerName
       , FileName: FileName
       , id: id
    };

    var PrjName = $("#txtPrjName").combobox('getText');
    if (PrjName)
        o.PrjName = PrjName;

    var ProjId = $("#txtProjId").combobox('getText');
    if (ProjId)
        o.ProjId = ProjId;
    
    var TownName = $("#txtTownName").textbox('getValue');
    if (TownName)
        o.TownName = TownName;

    var VillageName = $("#txtVillageName").textbox('getValue');
    if (VillageName)
        o.VillageName = VillageName;

    var LayerOrder = $("#txtLayerOrder").textbox('getValue');
    if (LayerOrder)
        o.LayerOrder = LayerOrder;

    var FileSize = $("#txtFileSize").textbox('getValue');
    if (FileSize)
        o.FileSize = FileSize;

    var FileType = $("#txtFileType").textbox('getValue');
    if (FileType)
        o.FileType = FileType;
    

    //var file = document.getElementById('browse').files[0];
    var fm = new FormData();
    fm.append('action', action);
    fm.append('kmlData', JSON.stringify(o));
    if (file) {
        fm.append('kmlfile', file);
    }
    $.ajax(
        {
            url: '/mKml/uploadKml',   
            type: 'POST',
            data: fm,
            cache: false,
            contentType: false, //禁止设置请求类型
            processData: false, //禁止jquery对DAta数据的处理,默认会处理
            //禁止的原因是,FormData已经帮我们做了处理
            async: false,
            success: function (data) {
                //测试是否成功
                //但需要你后端有返回值
                if (data.errMsg != '') {
                    $.messager.alert('保存出错', data.errMsg);
                    return;
                }
                $('#w').window('close');
                $("#mKmlGrid").datagrid('reload');
            },
            error: function (jqXHR, textStatus, errorThrown) {

            },
            complete: function () {
            }
        }
    );
}
/*
function saveData() {
    var PrjName = $("#txtPrjName").combobox('getText');
    if (PrjName)
        o.PrjName = PrjName;

    var ProjId = $("#txtProjId").combobox('getText');
    if (ProjId)
        o.ProjId = ProjId;


    var TownName = $("#txtTownName").textbox('getValue');
    if (TownName)
        o.TownName = TownName;

    var VillageName = $("#txtVillageName").textbox('getValue');
    if (VillageName)
        o.VillageName = VillageName;

    var LayerOrder = $("#txtLayerOrder").textbox('getValue');
    if (LayerOrder)
        o.LayerOrder = LayerOrder;

    $.ajax({
        url: "/mKml/SaveData",
        type: 'post',
        data: $.toJSON({ action: action, model: o }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.errMsg != '') {
                $.messager.alert('保存出错', data.errMsg);
                return;
            }
            $('#w').window('close');
            $("#mKmlGrid").datagrid('reload');
        },
        error: function (jqXHR, textStatus, errorThrown) {

        }, complete: function () {

        }
    });
}*/
function initProjCnt(record) {
     $("#txtProvinceName").textbox('setValue',record.Province);
     $("#txtCityName").textbox('setValue',record.City);
     $("#txtCountyName").textbox('setValue',record.County);
     $("#txtTownName").textbox('setValue',record.Town);
     $("#txtVillageName").textbox('setValue', record.Village);
 }
function rowOpt(a) {
    if (a == 'modify') {
        var row = $('#mKmlGrid').datagrid('getSelected');
        if (row == undefined) {
            $.messager.alert('系统提示', '请选择要修改的行!');//2w+
            return;
        }
    }


 //   $("#txtProjId").combobox('setValue');
 //   $("#txtPrjName").combobox('setValue');
    $("#txtProvinceName").textbox('setValue');
    $("#txtCityName").textbox('setValue');
    $("#txtCountyName").textbox('setValue');
    $("#txtTownName").textbox('setValue');
    $("#txtLayerName").textbox('setValue');
    $("#txtVillageName").textbox('setValue');
    $("#txtLayerOrder").textbox('setValue');
    $("#txtFileName").textbox('setValue');
    $("#txtFileSize").textbox('setValue');
    $("#txtFileType").textbox('setValue');
        
    $.ajax({
        url: "/mProject/GetListData",
        type: 'post',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (!data) {
                $.messager.alert('获取项目数据出错', data.errMsg);
                return;
            }
            $('#txtProjId').combobox({
                data: data,
                valueField: 'id',
                textField: 'Ccode',
                onSelect: function (record) {
                    $("#txtPrjName").combobox('setValue', record.CName);
                    initProjCnt(record);
                }


            });
            $('#txtPrjName').combobox({
                data: data,
                valueField: 'id',
                textField: 'CName',
                onSelect: function (record) {
                    $("#txtProjId").combobox('setValue', record.Ccode);
                    initProjCnt(record);
                }

            });

            if (action == 'modify') {
                var row = $('#mKmlGrid').datagrid('getSelected');
                id = row.id;
                $("#txtProjId").combobox('setValue', row.ProjId);
                $("#txtPrjName").combobox('setValue', row.PrjName);
                $("#txtProvinceName").textbox('setValue', row.ProvinceName);
                $("#txtCityName").textbox('setValue', row.CityName);
                $("#txtCountyName").textbox('setValue', row.CountyName);
                $("#txtTownName").textbox('setValue', row.TownName);
                $("#txtLayerName").textbox('setValue', row.LayerName);
                $("#txtVillageName").textbox('setValue', row.VillageName);
                $("#txtLayerOrder").textbox('setValue', row.LayerOrder);
                $("#txtFileName").textbox('setValue', row.FileName);
                $("#txtFileSize").textbox('setValue', row.FileSize);
                $("#txtFileType").textbox('setValue', row.FileType);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {

        }, complete: function () {

        }
    });

  

    action = a;
    $('#w').window('open');
}
function rowDel() {
    var row = $('#mKmlGrid').datagrid('getSelected');
    if (row == undefined) {
        $.messager.alert('系统提示', '请选择要删除的行!');
        return;
    }
    $.messager.confirm('系统提示', '你确定要删除所选择的行吗?', function (r) {
        if (r) {
            id = row.id;
            var o = { id: id };
            $.ajax({
                url: "/mKml/GetListDataById",
                type: 'post',
                data: $.toJSON(o),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data.errMsg && data.errMsg != '') {
                        $.messager.alert('id不存在！', data.errMsg);
                        return;
                    }

                    $.ajax({
                        url: "/mKml/Delete",
                        type: 'post',
                        data: $.toJSON({ model: data[0]}),
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        success: function (data) {
                            if (data.errMsg != '') {
                                $.messager.alert('删除出错', data.errMsg);
                                return;
                            }
                            $("#mKmlGrid").datagrid('reload');
                        },
                        error: function (jqXHR, textStatus, errorThrown) {

                        }, complete: function () {

                        }
                    });
                   
                },
                error: function (jqXHR, textStatus, errorThrown) {

                }, complete: function () {

                }
            });

           




            $.ajax({
                url: "/mKml/Delete",
                type: 'post',
                data: $.toJSON({ model: o }),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data.errMsg != '') {
                        $.messager.alert('删除出错', data.errMsg);
                        return;
                    }
                    $("#mKmlGrid").datagrid('reload');
                },
                error: function (jqXHR, textStatus, errorThrown) {

                }, complete: function () {

                }
            });
        }
    });
}

var uploadifyOnSelectError = function (file, errorCode, errorMsg) {
    var msgText = "上传失败\n";
    switch (errorCode) {
        case SWFUpload.QUEUE_ERROR.QUEUE_LIMIT_EXCEEDED:
            //this.queueData.errorMsg = "每次最多上传 " + this.settings.queueSizeLimit + "个文件";
            msgText += "每次最多上传 " + this.settings.queueSizeLimit + "个文件";
            break;
        case SWFUpload.QUEUE_ERROR.FILE_EXCEEDS_SIZE_LIMIT:
            msgText += "文件大小超过限制( " + this.settings.fileSizeLimit + " )";
            break;
        case SWFUpload.QUEUE_ERROR.ZERO_BYTE_FILE:
            msgText += "文件大小为0";
            break;
        case SWFUpload.QUEUE_ERROR.INVALID_FILETYPE:
            msgText += "文件格式不正确，仅限 " + this.settings.fileTypeExts;
            break;
        default:
            msgText += "错误代码：" + errorCode + "\n" + errorMsg;
    }
    layer.msg(msgText);
};
var uploadifyOnUploadError = function (file, errorCode, errorMsg, errorString) {
    // 手工取消不弹出提示
    if (errorCode == SWFUpload.UPLOAD_ERROR.FILE_CANCELLED
      || errorCode == SWFUpload.UPLOAD_ERROR.UPLOAD_STOPPED) {
        return;
    }
    var msgText = "上传失败\n";
    switch (errorCode) {
        case SWFUpload.UPLOAD_ERROR.HTTP_ERROR:
            msgText += "HTTP 错误\n" + errorMsg;
            break;
        case SWFUpload.UPLOAD_ERROR.MISSING_UPLOAD_URL:
            msgText += "上传文件丢失，请重新上传";
            break;
        case SWFUpload.UPLOAD_ERROR.IO_ERROR:
            msgText += "IO错误";
            break;
        case SWFUpload.UPLOAD_ERROR.SECURITY_ERROR:
            msgText += "安全性错误\n" + errorMsg;
            break;
        case SWFUpload.UPLOAD_ERROR.UPLOAD_LIMIT_EXCEEDED:
            msgText += "每次最多上传 " + this.settings.uploadLimit + "个";
            break;
        case SWFUpload.UPLOAD_ERROR.UPLOAD_FAILED:
            msgText += errorMsg;
            break;
        case SWFUpload.UPLOAD_ERROR.SPECIFIED_FILE_ID_NOT_FOUND:
            msgText += "找不到指定文件，请重新操作";
            break;
        case SWFUpload.UPLOAD_ERROR.FILE_VALIDATION_FAILED:
            msgText += "参数错误";
            break;
        default:
            msgText += "文件:" + file.name + "\n错误码:" + errorCode + "\n"
              + errorMsg + "\n" + errorString;
    }
    layer.msg(msgText);
};

var uploadifyOnSelect = function () {
};
var uploadifyOnUploadSuccess = function (file, data, response) {
    layer.msg(file.name + "\n\n" + response + "\n\n" + data);
    saveData();
};
var btnBrowse = function () {
    $("#browse").click();
}


var file = null;
$(function () {
    $("#browse").change(function () {
        file = this.files[0];
        if (file) {
            $("#txtFileName").textbox('setValue', file.name);
            var name = file.name.substring(0,file.name.indexOf('.'));
            $("#txtLayerName").textbox('setValue', name);
           
            var fileSize = 0;
            if (file.size > 1024 * 1024)
                fileSize = (Math.round(file.size * 100 / (1024 * 1024)) / 100).toString() + 'MB';
            else
                fileSize = (Math.round(file.size * 100 / 1024) / 100).toString() + 'KB';
            $("#txtFileSize").textbox('setValue', fileSize);
            $("#txtFileType").textbox('setValue', file.type);
           // document.getElementById('fileSize').innerHTML = 'Size: ' + fileSize;
          //  document.getElementById('fileType').innerHTML = 'Type: ' + file.type;
        }
    })

    /*
    $("#uploadify").uploadify({
        uploader: '/mKml/UploadifyFile', //处理上传的方法
        swf: '/Scripts/lib/uploadify/uploadify.swf',
        queueID: '', //文件队列的ID，该ID与存放文件队列的div的ID一致
        width: 80, // 按钮宽度
        height: 60, //按钮高度
        buttonText: "",
        buttonCursor: 'hand',
        fileSizeLimit: 204800,
        fileobjName: 'Filedata',
        fileTypeExts: '*.kml;*.xml', //扩展名
        fileTypeDesc: "请选择kml文件", //文件说明
        auto: false, //是否自动上传
        multi: true, //是否一次可以选中多个文件
        queueSizeLimit: 5, //允许同时上传文件的个数
        overrideEvents: ['onSelectError', 'onDialogClose'], // 是否要默认提示 要就不配置
        onSelect: uploadifyOnSelect,
        onSelectError: uploadifyOnSelectError,
        onUploadError: uploadifyOnUploadError,
        onUploadSuccess: uploadifyOnUploadSuccess
    });*/
});




function uploadFile(dt) {

    //var file = document.getElementById('browse').files[0];
    var fm = new FormData();
 //   fm.append('userName', userName);
    fm.append('kmlfile', file);
    $.ajax(
        {
            url: '/mKml/Doit',    //UploadifyFile',
            type: 'POST',
            data: fm,
            cache: false,
            contentType: false, //禁止设置请求类型
            processData: false, //禁止jquery对DAta数据的处理,默认会处理
            //禁止的原因是,FormData已经帮我们做了处理
            async:false,
            success: function (result) {
                //测试是否成功
                //但需要你后端有返回值
                alert(result);
            }
        }
    );

/*
    if (window.FileReader) {
        var reader = new FileReader();
        reader.readAsDataURL(file);
        //监听文件读取结束后事件    
        reader.onloadend = function (e) {
           // $(".img").attr("src", e.target.result);    //e.target.result就是最后的路径地址
            fd.append("file", e.target.result);
            var xhr = new XMLHttpRequest();
            xhr.upload.addEventListener("progress", uploadProgress, false);
            xhr.addEventListener("load", uploadComplete, false);
            xhr.addEventListener("error", uploadFailed, false);
            xhr.addEventListener("abort", uploadCanceled, false);
            xhr.open("POST", '/mKml/UploadifyFile');//修改成自己的接口
            xhr.send(fd);
        };
    }
    */
}
function uploadProgress(evt) {
    if (evt.lengthComputable) {
        var percentComplete = Math.round(evt.loaded * 100 / evt.total);
        document.getElementById('progressNumber').innerHTML = percentComplete.toString() + '%';
    }
    else {
        document.getElementById('progressNumber').innerHTML = 'unable to compute';
    }
}
function uploadComplete(evt) {
    /* 服务器端返回响应时候触发event事件*/
    alert(evt.target.responseText);
}
function uploadFailed(evt) {
    alert("There was an error attempting to upload the file.");
}
function uploadCanceled(evt) {
    alert("The upload has been canceled by the user or the browser dropped the connection.");
}