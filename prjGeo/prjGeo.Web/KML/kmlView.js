

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
var oldFile = '';

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
    fm.append("oldFile", oldFile);
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
                oldFile = row.FileName;
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
            console.log("id"+id);
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
                    console.log("GetListDataById");
                    console.log(data);
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

var findProjName = null
    , findProjId = null
    , findLayerName = null;
function loadData(param, success, error) {
   // var grid = $("#mKmlGrid").datagrid("getPager").data("pagination").options;

    var obj = {
        PrjName: findProjName,
        PrjId: findProjId,
        LayerName: findLayerName,
        pager: {
            order: "desc",
            sort: "LayerOrder",
            rows: param.rows,
            page: param.page
        }
    }
    $.ajax({
        url: "/mKml/GetListByFilter",
        type: 'post',
        data: JSON.stringify(obj),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.errMsg) {
                $.messager.alert('查询出错', data.errMsg);
                success({ total: 0, rows: [] });
                return;
            }

            success(data);
            // gridData = data;

            //  $("#dg").datagrid('reload');

        },
        error: function (jqXHR, textStatus, errorThrown) {

            success({ total: 0, rows: [] });

        }, complete: function () {

        }
    });
}
function onBtnFind() {
    findProjName = $("#findProjName").textbox('getValue');
    findProjId = $("#findProjId").textbox('getValue');
    findLayerName = $("#findLayerName").textbox('getValue');
    $("#mKmlGrid").datagrid('reload');
}


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
});

