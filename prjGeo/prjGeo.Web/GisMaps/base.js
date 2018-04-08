$(function () {
    var height = $(window).height();
    var width = $(window).width();
    var gridData = null;

    $('#txtPrjName').combobox({
        url: "/mProject/GetListData",
        required: true,
        valueField: 'id',
        textField: 'CName'
    });
    $("#btnFind").click(find);
  //  InitProjectCombo();
    InitialDatagrid();
  
   

    window.onresize = function () {
        height = $(window).height();
        width = $(window).width();
        $("div.middle").css("height", height);
        $("div.middle").css("width", width);
        if ($("body > div.middle > div.leftpanel").length > 0 && $("body > div.middle > div.leftpanel").position().left == 0) {
            $("div.middle>div.map").css("height", height - 100);
        } else {
            $("div.middle>div.map").css("height", height - 100);
        }
    };
    if ($("body > div.middle > div.leftpanel").length > 0 && $("body > div.middle > div.leftpanel").position().left == 0) {
        $("div.middle>div.map").css("height", height - 100);
    } else {
        $("div.middle>div.map").css("height", height - 100);
    }    

    var flon = getQueryString('flon');
    var flat = getQueryString('flat');
    var mapname = getQueryString('name');
    var intID = getQueryString('id');
   //var objData = new GisInfos(mapname, intID);
    // objMaps.addMap(parseFloat(flon), parseFloat(flat), objData, mapname);

    mapCls.initMap();

    /*
    GisInfos(mapname, intID, function (obj) {
        var pos = obj.mploygon[0].coordinates[0];
        mapCls.setView(pos[0],pos[1], 13);
        if (obj) {

            mapCls.addPolygons(obj.mploygon);
        }
    });
    */

    function InitProjectCombo() {
             
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
            },
            error: function (jqXHR, textStatus, errorThrown) {

            }, complete: function () {

            }
        });
    }
    function InitialDatagrid() {
        $("#dg").datagrid({
            loader: loadData
        })
     /*   $('#dg').datagrid({
            loader: loadData,
            pagination: false,
         //   pageList: [5, 10, 50, 100, 500],
        //    pageSize: 5,
            rownumbers: false,
            fitColumns: true,
            singleSelect: true,
            cache: true,
            striped: true,
            autoRowHeight: true,
            nowrap: false,
            loadMsg: "正在加载数据，请稍等…",
            onSelect: function (index, row) {
                $('#dg').datagrid('options').selectRow = index;
            },
            onClickCell: function (rowIndex, field, value) {
               // MouseCellClick(rowIndex, field, value);
            },
            onLoadSuccess: function (data) {
                  $(".easyui-tooltip").tooltip(
                       {
                           onShow: function () {
                               $(this).tooltip('tip').css({
                                   width: '150',
                                   height: '35',
                                   boxShadow: '1px 1px 3px #292929'
                               });
                           },
                           position: 'bottom'
                       } );
           
            },
            columns: [[
                { field: 'IsVisible', checkbox: true, align: "center" },
                { field: 'LayerName', title: '图层名', align: "center" },
            ]]
        });*/

    }

    function loadData(param, success, error) {
        if (!gridData) {
            success(gridData);
        }
    }

    function find() {
        var PrjName = $("#txtPrjName").combobox('getText');
        if (!PrjName) {
            $.messager.alert('系统提示', '请选择项目名称!');
            return;
        }
        var param = {
            PrjName: PrjName
        }

        $.ajax(
         {
             url: '/mMap/GetListByFilter',
             type: 'POST',
             data: $.toJSON(param),
             cache: false,
             contentType: false, //禁止设置请求类型
             processData: false, //禁止jquery对DAta数据的处理,默认会处理
             //禁止的原因是,FormData已经帮我们做了处理
             async: false,
             success: function (data) {
                 //测试是否成功
                 //但需要你后端有返回值
                 if (data.errMsg != '') {
                     $.messager.alert('查询出错', data.errMsg);
                     return;
                 }
                 gridData = data;

                 $("#mKmlGrid").datagrid('reload');
             },
             error: function (jqXHR, textStatus, errorThrown) {

             },
             complete: function () {
             }
         });
    }

})
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

