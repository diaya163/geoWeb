


$(function () {
    var height = $(window).height();
    var width = $(window).width();
    var gridData = [];

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

    mapCls.initMap();

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
          $('#dg').datagrid({
               loader: loadData,
               pagination: false,
            //   pageList: [5, 10, 50, 100, 500],
           //    pageSize: 5,
               rownumbers: false,
               fitColumns: true,
           //    singleSelect: false,
           //    selectOnCheck: true,
           //    checkOnSelect:true,
               cache: true,
               striped: true,
               autoRowHeight: true,
               nowrap: false,
               loadMsg: "正在加载数据，请稍等…",
               onCheck: function (rowIndex, rowData) {
                   gridCheck(rowIndex, rowData,1);
               },
               onUncheck:function(rowIndex, rowData){
                   gridCheck(rowIndex, rowData, 0);
               },
               rowStyler: function (rowIndex, rowData) {
                   return 'background-color:#fff;';
               },
               onClickCell: function (rowIndex, field, value) {
                  // MouseCellClick(rowIndex, field, value);
               },
               onLoadSuccess: function (data) {
                     $(".datagrid-header-check").html(""); //去掉全选的Check，因其按钮操作不能进check事件。
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
                          });

                     if (data) {
                         $.each(data.rows, function (index, item) {
                             if (item.IsVisible && item.IsVisible===1) {
                                 $('#dg').datagrid('checkRow', index);
                             }
                         });
                     }

              
               },
               columns: [[
                   { field: 'IsVisible', checkbox: true, align: "center" },
                   { field: 'LayerName', title: '图层名', width: 150, align: "center" },
               ]]
           });

    }

    function loadData(param, success, error) {
          success(gridData);
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
        $.ajax({
            url: "/mMap/GetListByFilter",
            type: 'post',
            data: JSON.stringify(param),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data.errMsg) {
                    $.messager.alert('查询出错', data.errMsg);
                    return;
                }
                var urls = [];
                for (var i = 0; i < data.length;i++) {
                    urls.push(data[i].KmlPath);
                }
                mapCls.addKmls(urls);
                gridData = data;

                $("#dg").datagrid('reload');

            },
            error: function (jqXHR, textStatus, errorThrown) {

            }, complete: function () {

            }
        });
           
    }

    function gridCheck(rowIndex, rowData, bCheck) {
        var row = rowData;
        row.IsVisible = bCheck;
        var param ={
            action: "modify",
            model: rowData
            };
        //更新图层状态。
        $.ajax({
            url: "/mKml/SaveData",
            type: 'post',
            data: JSON.stringify(param),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data.errMsg && data.errMsg!="") {
                    $.messager.alert('更新图层显示状态出错', data.errMsg);
                    return;
                }

            },
            error: function (jqXHR, textStatus, errorThrown) {

            },
            complete: function () {

            }
        });
    }



})
