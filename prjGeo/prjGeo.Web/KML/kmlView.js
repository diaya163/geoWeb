

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


    var o = {
        ProvinceName: ProvinceName
        , CityName: CityName
        , CountyName: CountyName
        , LayerName: LayerName
        , id: id
    };

    var PrjName = $("#txtPrjName").combobox('getValue');
    if (PrjName)
        o.PrjName = PrjName;

    var ProjId = $("#txtProjId").combobox('getValue');
    if (ProjId)
        o.ProjId = ProjId;

    var CountyName = $("#txtCountyName").textbox('getValue');
    if (CountyName)
        o.CountyName = CountyName;

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
}
function rowOpt(a) {
    $("#txtProjId").combobox('setValue');
    $("#txtPrjName").combobox('setValue');
    $("#txtProvinceName").textbox('setValue');
    $("#txtCityName").textbox('setValue');
    $("#txtCountyName").textbox('setValue');
    $("#txtTownName").textbox('setValue');
    $("#txtLayerName").textbox('setValue');
    $("#txtVillageName").textbox('setValue');
    $("#txtLayerOrder").textbox('setValue');


    $('#txtProjId').combobox({
        url: "/mProject/GetListData",
        required: true,
        valueField: 'id',
        textField: 'Ccode',
    });
    $('#txtPrjName').combobox({
        url: "/mProject/GetListData",
        required: true,
        valueField: 'id',
        textField: 'CName'
    });
    if (a == 'modify') {
        var row = $('#mKmlGrid').datagrid('getSelected');
        if (row == undefined) {
            $.messager.alert('系统提示', '请选择要修改的行!');//2w+
            return;
        }
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
    }
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
